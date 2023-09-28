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
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Kits.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Kits.Exporting
{
    public class KitAppService : SWMANAGERAppServiceBase, IKitAppService
    {

        private readonly IListarKitsExcelExporter _listarKitsExcelExporter;
        private readonly IRepository<Kit, long> _kitRepositorio;


        public KitAppService(IRepository<Kit, long> kitRepositorio, IListarKitsExcelExporter listarKitsExcelExporter)
        {
            _kitRepositorio = kitRepositorio;
            _listarKitsExcelExporter = listarKitsExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(KitDto input)
        {
            try
            {
                var kit = input.MapTo<Kit>();
                if (input.Id.Equals(0))
                {
                    await _kitRepositorio.InsertOrUpdateAsync(kit);
                }
                else
                {
                    await _kitRepositorio.UpdateAsync(kit);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(KitDto input)
        {
            try
            {
                await _kitRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<Kit>> ListarTodos()
        {
            try
            {
                var query = await _kitRepositorio
                    .GetAllListAsync();

                var kitsDto = query.MapTo<List<Kit>>();

                return new ListResultDto<Kit> { Items = kitsDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<KitDto>> Listar(ListarKitsInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<Kit> kits;
            List<KitDto> kitsDtos = new List<KitDto>();
            try
            {
                var query = _kitRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                kits = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                kitsDtos = kits
                    .MapTo<List<KitDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<KitDto>(
                contarTiposTabelaDominio,
                kitsDtos
                );
        }


        public async Task<KitDto> Obter(long id)
        {
            try
            {
                var result = await _kitRepositorio.GetAsync(id);
                var kit = result.MapTo<KitDto>();
                return kit;
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
                var query = await _kitRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input)
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var Kits = new ListResultDto<GenericoIdNome> { Items = query };

                List<KitDto> KitsList = new List<KitDto>();

                return Kits;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarKitsInput input)
        {
            try
            {
                var result = await Listar(input);
                var Kits = result.Items;
                return _listarKitsExcelExporter.ExportToFile(Kits.ToList());
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
            List<KitDto> pacientesDtos = new List<KitDto>();
            try
            {
                //get com filtro
                var query = from p in _kitRepositorio.GetAll()
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

