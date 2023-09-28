using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio
{
    public class SetorAppService : SWMANAGERAppServiceBase, ISetorAppService
    {

        private readonly IListarSetorsExcelExporter _listarSetorsExcelExporter;
        private readonly IRepository<Setor, long> _setorRepositorio;


        public SetorAppService(IRepository<Setor, long> setorRepositorio, IListarSetorsExcelExporter listarSetorsExcelExporter)
        {
            _setorRepositorio = setorRepositorio;
            _listarSetorsExcelExporter = listarSetorsExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(SetorDto input)
        {
            try
            {
                var setor = input.MapTo<Setor>();
                if (input.Id.Equals(0))
                {
                    await _setorRepositorio.InsertOrUpdateAsync(setor);
                }
                else
                {
                    var ori = await _setorRepositorio.GetAsync(input.Id);
                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;
                    ori.IsSistema = input.IsSistema;
                    ori.OrdemSetor = input.OrdemSetor;

                    await _setorRepositorio.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(SetorDto input)
        {
            try
            {
                await _setorRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<Setor>> ListarTodos()
        {
            try
            {
                var query = await _setorRepositorio
                    .GetAllListAsync();

                var setorsDto = query.MapTo<List<Setor>>();

                return new ListResultDto<Setor> { Items = setorsDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<SetorDto>> Listar(ListarSetorsInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<Setor> setors;
            List<SetorDto> setorsDtos = new List<SetorDto>();
            try
            {
                var query = _setorRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                setors = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                setorsDtos = setors
                    .MapTo<List<SetorDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<SetorDto>(
                contarTiposTabelaDominio,
                setorsDtos
                );
        }


        public async Task<SetorDto> Obter(long id)
        {
            try
            {
                var result = await _setorRepositorio.GetAsync(id);
                var setor = result.MapTo<SetorDto>();
                return setor;
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
                var query = await _setorRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input)
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var Setors = new ListResultDto<GenericoIdNome> { Items = query };

                List<SetorDto> SetorsList = new List<SetorDto>();

                return Setors;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarSetorsInput input)
        {
            try
            {
                var result = await Listar(input);
                var Setors = result.Items;
                return _listarSetorsExcelExporter.ExportToFile(Setors.ToList());
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
            List<SetorDto> pacientesDtos = new List<SetorDto>();
            try
            {
                //get com filtro
                var query = from p in _setorRepositorio.GetAll()
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

