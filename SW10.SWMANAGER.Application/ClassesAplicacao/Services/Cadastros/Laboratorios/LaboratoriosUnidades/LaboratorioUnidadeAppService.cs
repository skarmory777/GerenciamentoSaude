using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades
{
    public class LaboratorioUnidadeAppService : SWMANAGERAppServiceBase, ILaboratorioUnidadeAppService
    {

        private readonly IListarLaboratorioUnidadesExcelExporter _listarLaboratorioUnidadesExcelExporter;
        private readonly IRepository<LaboratorioUnidade, long> _laboratorioUnidadeRepositorio;


        public LaboratorioUnidadeAppService(IRepository<LaboratorioUnidade, long> laboratorioUnidadeRepositorio, IListarLaboratorioUnidadesExcelExporter listarLaboratorioUnidadesExcelExporter)
        {
            _laboratorioUnidadeRepositorio = laboratorioUnidadeRepositorio;
            _listarLaboratorioUnidadesExcelExporter = listarLaboratorioUnidadesExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(LaboratorioUnidadeDto input)
        {
            try
            {
                var laboratorioUnidade = LaboratorioUnidadeDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    await _laboratorioUnidadeRepositorio.InsertOrUpdateAsync(laboratorioUnidade);
                }
                else
                {
                    var ori = await _laboratorioUnidadeRepositorio.GetAsync(input.Id);
                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;
                    ori.IsSistema = input.IsSistema;

                    await _laboratorioUnidadeRepositorio.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(LaboratorioUnidadeDto input)
        {
            try
            {
                await _laboratorioUnidadeRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<LaboratorioUnidade>> ListarTodos()
        {
            try
            {
                var query = await _laboratorioUnidadeRepositorio
                    .GetAllListAsync();

                //var laboratorioUnidadesDto = query.MapTo<List<LaboratorioUnidade>>();

                return new ListResultDto<LaboratorioUnidade> { Items = query };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<LaboratorioUnidadeDto>> Listar(ListarInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<LaboratorioUnidade> laboratorioUnidades;
            List<LaboratorioUnidadeDto> laboratorioUnidadesDtos = new List<LaboratorioUnidadeDto>();
            try
            {
                var query = _laboratorioUnidadeRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                laboratorioUnidades = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                laboratorioUnidadesDtos = LaboratorioUnidadeDto.Mapear(laboratorioUnidades).ToList();

                return new PagedResultDto<LaboratorioUnidadeDto>(
                    contarTiposTabelaDominio,
                    laboratorioUnidadesDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<LaboratorioUnidadeDto> Obter(long id)
        {
            try
            {
                var result = await _laboratorioUnidadeRepositorio.GetAsync(id);
                var laboratorioUnidade = LaboratorioUnidadeDto.Mapear(result);
                return laboratorioUnidade;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _laboratorioUnidadeRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input)
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var LaboratorioUnidades = new ListResultDto<GenericoIdNome> { Items = query };

                List<LaboratorioUnidadeDto> LaboratorioUnidadesList = new List<LaboratorioUnidadeDto>();

                return LaboratorioUnidades;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarInput input)
        {
            try
            {
                var result = await Listar(input);
                var LaboratorioUnidades = result.Items;
                return _listarLaboratorioUnidadesExcelExporter.ExportToFile(LaboratorioUnidades.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<LaboratorioUnidadeDto> pacientesDtos = new List<LaboratorioUnidadeDto>();
            try
            {
                //get com filtro
                var query = from p in _laboratorioUnidadeRepositorio.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                        //  m.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}

