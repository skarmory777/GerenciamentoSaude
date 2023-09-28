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
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames
{
    public class KitExameAppService : SWMANAGERAppServiceBase, IKitExameAppService
    {

        private readonly IListarKitExamesExcelExporter _listarKitExamesExcelExporter;
        private readonly IRepository<KitExame, long> _kitExameRepositorio;


        public KitExameAppService(IRepository<KitExame, long> kitExameRepositorio, IListarKitExamesExcelExporter listarKitExamesExcelExporter)
        {
            _kitExameRepositorio = kitExameRepositorio;
            _listarKitExamesExcelExporter = listarKitExamesExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(KitExameDto input)
        {
            try
            {
                var kitExame = input.MapTo<KitExame>();
                if (input.Id.Equals(0))
                {
                    await _kitExameRepositorio.InsertOrUpdateAsync(kitExame);
                }
                else
                {
                    await _kitExameRepositorio.UpdateAsync(kitExame);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(KitExameDto input)
        {
            try
            {
                await _kitExameRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<KitExameDto>> Listar(ListarKitExamesInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<KitExame> kitExames;
            List<KitExameDto> kitExamesDtos = new List<KitExameDto>();
            try
            {
                var query = _kitExameRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                kitExames = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                kitExamesDtos = kitExames
                    .MapTo<List<KitExameDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<KitExameDto>(
                contarTiposTabelaDominio,
                kitExamesDtos
                );
        }

        public async Task<ListResultDto<KitExameDto>> ListarTodos()
        {
            try
            {
                var query = await _kitExameRepositorio
                    .GetAllListAsync();

                var kitExamesDto = query.MapTo<List<KitExameDto>>();

                return new ListResultDto<KitExameDto> { Items = kitExamesDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<KitExameDto>> ListarFiltro(string filtro)
        {
            try
            {
                var query = _kitExameRepositorio
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrWhiteSpace(), m =>
                         m.CreationTime.ToShortTimeString().Contains(filtro) ||
                         m.Descricao.Contains(filtro) ||
                         m.Codigo.Contains(filtro)
                    );

                var admissoesMedicas = await query.ToListAsync();
                var admissoesMedicasDto = admissoesMedicas.MapTo<List<KitExameDto>>();

                return new ListResultDto<KitExameDto> { Items = admissoesMedicasDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<KitExameDto> Obter(long id)
        {
            try
            {
                var result = await _kitExameRepositorio.GetAsync(id);
                var kitExame = result.MapTo<KitExameDto>();
                return kitExame;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarKitExamesInput input)
        {
            try
            {
                var result = await Listar(input);
                var KitExames = result.Items;
                return _listarKitExamesExcelExporter.ExportToFile(KitExames.ToList());
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
            List<KitExameDto> pacientesDtos = new List<KitExameDto>();
            try
            {
                //get com filtro
                var query = from p in _kitExameRepositorio.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
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

