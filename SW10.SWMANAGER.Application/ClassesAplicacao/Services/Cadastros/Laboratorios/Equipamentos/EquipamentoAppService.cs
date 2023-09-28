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
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Equipamentos
{
    public class EquipamentoAppService : SWMANAGERAppServiceBase, IEquipamentoAppService
    {

        private readonly IListarEquipamentosExcelExporter _listarEquipamentosExcelExporter;
        private readonly IRepository<Equipamento, long> _equipamentoRepositorio;


        public EquipamentoAppService(IRepository<Equipamento, long> equipamentoRepositorio, IListarEquipamentosExcelExporter listarEquipamentosExcelExporter)
        {
            _equipamentoRepositorio = equipamentoRepositorio;
            _listarEquipamentosExcelExporter = listarEquipamentosExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(EquipamentoDto input)
        {
            try
            {
                var equipamento = input.MapTo<Equipamento>();
                if (input.Id.Equals(0))
                {
                    await _equipamentoRepositorio.InsertOrUpdateAsync(equipamento);
                }
                else
                {
                    await _equipamentoRepositorio.UpdateAsync(equipamento);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(EquipamentoDto input)
        {
            try
            {
                await _equipamentoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<Equipamento>> ListarTodos()
        {
            try
            {
                var query = await _equipamentoRepositorio
                    .GetAllListAsync();

                var equipamentosDto = query.MapTo<List<Equipamento>>();

                return new ListResultDto<Equipamento> { Items = equipamentosDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<EquipamentoDto>> Listar(ListarEquipamentosInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<Equipamento> equipamentos;
            List<EquipamentoDto> equipamentosDtos = new List<EquipamentoDto>();
            try
            {
                var query = _equipamentoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                equipamentos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                equipamentosDtos = equipamentos
                    .MapTo<List<EquipamentoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<EquipamentoDto>(
                contarTiposTabelaDominio,
                equipamentosDtos
                );
        }


        public async Task<EquipamentoDto> Obter(long id)
        {
            try
            {
                var result = await _equipamentoRepositorio.GetAsync(id);
                var equipamento = result.MapTo<EquipamentoDto>();
                return equipamento;
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
                var query = await _equipamentoRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var Equipamentos = new ListResultDto<GenericoIdNome> { Items = query };

                List<EquipamentoDto> EquipamentosList = new List<EquipamentoDto>();

                return Equipamentos;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarEquipamentosInput input)
        {
            try
            {
                var result = await Listar(input);
                var Equipamentos = result.Items;
                return _listarEquipamentosExcelExporter.ExportToFile(Equipamentos.ToList());
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
            List<EquipamentoDto> pacientesDtos = new List<EquipamentoDto>();
            try
            {
                //get com filtro
                var query = from p in _equipamentoRepositorio.GetAll()
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

