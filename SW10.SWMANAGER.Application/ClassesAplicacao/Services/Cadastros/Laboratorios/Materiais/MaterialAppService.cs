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
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio
{
    public class MaterialAppService : SWMANAGERAppServiceBase, IMaterialAppService
    {

        private readonly IListarMaterialsExcelExporter _listarMaterialsExcelExporter;
        private readonly IRepository<Material, long> _materialRepositorio;


        public MaterialAppService(IRepository<Material, long> materialRepositorio, IListarMaterialsExcelExporter listarMaterialsExcelExporter)
        {
            _materialRepositorio = materialRepositorio;
            _listarMaterialsExcelExporter = listarMaterialsExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(MaterialDto input)
        {
            try
            {
                var material = input.MapTo<Material>();
                if (input.Id.Equals(0))
                {
                    await _materialRepositorio.InsertOrUpdateAsync(material);
                }
                else
                {
                    var ori = await _materialRepositorio.GetAsync(input.Id);
                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;
                    ori.IsSistema = input.IsSistema;
                    ori.Ordem = input.Ordem;

                    await _materialRepositorio.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(MaterialDto input)
        {
            try
            {
                await _materialRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<MaterialDto>> ListarTodos()
        {
            try
            {
                var query = await _materialRepositorio
                    .GetAllListAsync();

                var materialsDto = query.MapTo<List<MaterialDto>>();

                return new ListResultDto<MaterialDto> { Items = materialsDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<MaterialDto>> Listar(ListarMaterialsInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<Material> materials;
            List<MaterialDto> materialsDtos = new List<MaterialDto>();
            try
            {
                var query = _materialRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                materials = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                materialsDtos = materials
                    .MapTo<List<MaterialDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<MaterialDto>(
                contarTiposTabelaDominio,
                materialsDtos
                );
        }


        public async Task<MaterialDto> Obter(long id)
        {
            try
            {
                var result = await _materialRepositorio.GetAsync(id);
                var material = result.MapTo<MaterialDto>();
                return material;
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
                var query = await _materialRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var Materials = new ListResultDto<GenericoIdNome> { Items = query };

                List<MaterialDto> MaterialsList = new List<MaterialDto>();

                return Materials;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarMaterialsInput input)
        {
            try
            {
                var result = await Listar(input);
                var Materials = result.Items;
                return _listarMaterialsExcelExporter.ExportToFile(Materials.ToList());
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
            List<MaterialDto> pacientesDtos = new List<MaterialDto>();
            try
            {
                //get com filtro
                var query = from p in _materialRepositorio.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = p.Descricao };
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

