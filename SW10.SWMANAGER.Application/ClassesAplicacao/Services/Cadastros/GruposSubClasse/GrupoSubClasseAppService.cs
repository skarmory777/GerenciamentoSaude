using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse
{
    public class GrupoSubClasseAppService : SWMANAGERAppServiceBase, IGrupoSubClasseAppService
    {
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Edit)]
        public async Task CriarOuEditar(CriarOuEditarGrupoSubClasse input)
        {
            try
            {
                using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
                {
                    var classe = input.MapTo<GrupoSubClasse>();
                    if (input.Id.Equals(0))
                    {
                        await grupoSubClasseRepositorio.Object.InsertOrUpdateAsync(classe);
                    }
                    else
                    {
                        await grupoSubClasseRepositorio.Object.UpdateAsync(classe);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input, long idGrupo)
        {
            try
            {
                using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
                {
                    var query = await grupoSubClasseRepositorio.Object
                    .GetAll().AsNoTracking()
                    .WhereIf(!input.IsNullOrEmpty(), m => m.Descricao.Contains(input) && m.GrupoClasseId == idGrupo)
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                    return new ListResultDto<GenericoIdNome> { Items = query };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarGrupoSubClasse input)
        {
            try
            {
                using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
                {
                    await grupoSubClasseRepositorio.Object.DeleteAsync(input.Id);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<GrupoSubClasseDto>> ListarTodos()
        {
            List<GrupoSubClasse> listCore;
            List<GrupoSubClasseDto> listDtos = new List<GrupoSubClasseDto>();
            try
            {
                using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
                {
                    listCore = await grupoSubClasseRepositorio.Object
                    .GetAll()
                    .ToListAsync();

                    listDtos = listCore
                        .MapTo<List<GrupoSubClasseDto>>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<GrupoSubClasseDto> { Items = listDtos };
        }

        public async Task<PagedResultDto<GrupoSubClasseDto>> Listar(ListarGruposSubClasseInput input)
        {
            var contarGruposSubClasse = 0;
            List<GrupoSubClasse> GruposSubClasse;
            List<GrupoSubClasseDto> GruposSubClasseDtos = new List<GrupoSubClasseDto>();
            try
            {
                using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
                {
                    var query = grupoSubClasseRepositorio.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Descricao.Contains(input.Filtro)
                    //m.Palavra.Contains(input.Filtro) ||
                    //m.Observacao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                    contarGruposSubClasse = await query
                        .CountAsync();

                    GruposSubClasse = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    GruposSubClasseDtos = GruposSubClasse
                        .MapTo<List<GrupoSubClasseDto>>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<GrupoSubClasseDto>(contarGruposSubClasse,GruposSubClasseDtos);
        }

        public async Task<FileDto> ListarParaExcel(ListarGruposSubClasseInput input)
        {
            try
            {
                using (var listarGrupoSubClasseExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarGrupoSubClasseExcelExporter>())
                {
                    var query = await Listar(input);

                    var GruposSubClasseDtos = query.Items;

                    return listarGrupoSubClasseExcelExporter.Object.ExportToFile(GruposSubClasseDtos.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<CriarOuEditarGrupoSubClasse> Obter(long id)
        {
            try
            {
                using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
                {
                    var result = await grupoSubClasseRepositorio.Object.GetAsync(id);
                    var classe = result.MapTo<CriarOuEditarGrupoSubClasse>();
                    return classe;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ListResultDto<GrupoSubClasseDto>> ObterPorClasse(long id)
        {
            try
            {
                using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
                {
                    var result = await grupoSubClasseRepositorio.Object
                    .GetAll()
                    .Where(w => w.GrupoClasseId == id)
                    .FirstOrDefaultAsync();

                    var classe = result.MapTo<List<GrupoSubClasseDto>>();

                    return new ListResultDto<GrupoSubClasseDto> { Items = classe };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<ListResultDto<GrupoSubClasseDto>> ListarPorClasse(long id)
        {
            List<GrupoSubClasse> listCore;
            List<GrupoSubClasseDto> listDtos = new List<GrupoSubClasseDto>();
            try
            {
                using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
                {
                    listCore = await grupoSubClasseRepositorio.Object
                    .GetAll()
                    .Where(w => w.GrupoClasseId == id)
                    .ToListAsync();

                    listDtos = listCore
                        .MapTo<List<GrupoSubClasseDto>>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<GrupoSubClasseDto> { Items = listDtos };
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var grupoSubClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoSubClasse, long>>())
            {
                return await Select2Helper.CreateSelect2(this, grupoSubClasseRepositorio.Object)
               .AddWhereMethod((input, dapperParamters) =>
               {
                   var whereBuilder = new StringBuilder(Select2Helper.DefaultWhereMethod(input, dapperParamters));

                   long grupoClasseId = 0;
                   long.TryParse(input.filtro, out grupoClasseId);

                   whereBuilder.Append(" AND GrupoClasseId = @grupoClasseId");

                   dapperParamters.Add("grupoClasseId", grupoClasseId);

                   return whereBuilder.ToString();
               }).ExecuteAsync(dropdownInput);
            }
        }
    }
}
