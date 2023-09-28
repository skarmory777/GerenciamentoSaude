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
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse
{
    public class GrupoClasseAppService : SWMANAGERAppServiceBase, IGrupoClasseAppService
    {
        //private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly IUltimoIdAppService _ultimoIdAppService;
        //private readonly IRepository<GrupoClasse, long> _grupoClasseRepositorio;
        //private readonly IListarGrupoClasseExcelExporter _listarGrupoClasseExcelExporter;

        //public GrupoClasseAppService(IRepository<GrupoClasse, long> grupoClasseRepositorio,
        //    IListarGrupoClasseExcelExporter listarGrupoClasseExcelExporter,
        //    IUnitOfWorkManager UnitOfWorkManager,
        //    IUltimoIdAppService ultimoServicoAppService
        //    )
        //{
        //    _unitOfWorkManager = UnitOfWorkManager;
        //    _grupoClasseRepositorio = grupoClasseRepositorio;
        //    _listarGrupoClasseExcelExporter = listarGrupoClasseExcelExporter;
        //    _ultimoIdAppService = ultimoServicoAppService;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Edit)]
        [UnitOfWork]
        public async Task<GrupoClasseDto> CriarOuEditar(GrupoClasseDto input)
        {
            try
            {
                var grupoClasse = input.MapTo<GrupoClasse>();
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var grupoClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoClasse, long>>())
                {
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            grupoClasse.Codigo = ultimoIdAppService.Object.ObterProximoCodigo("grupoClasse").Result;
                            grupoClasse.Id = await grupoClasseRepositorio.Object.InsertAndGetIdAsync(grupoClasse);

                            unitOfWork.Complete();

                            unitOfWorkManager.Object.Current.SaveChanges();

                            unitOfWork.Dispose();
                        }
                    }
                    else
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await grupoClasseRepositorio.Object.UpdateAsync(grupoClasse);
                            unitOfWork.Complete();

                            unitOfWorkManager.Object.Current.SaveChanges();

                            unitOfWork.Dispose();
                        }
                    }

                    return grupoClasse.MapTo<GrupoClasseDto>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task Excluir(GrupoClasseDto input)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var grupoClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoClasse, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await grupoClasseRepositorio.Object.DeleteAsync(input.Id);

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<GrupoClasseDto>> ListarTodos()
        {
            List<GrupoClasse> listCore;
            List<GrupoClasseDto> listDtos = new List<GrupoClasseDto>();
            try
            {
                using (var grupoClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoClasse, long>>())
                {
                    listCore = await grupoClasseRepositorio.Object.GetAll().AsNoTracking().ToListAsync().ConfigureAwait(false);

                    listDtos = listCore.MapTo<List<GrupoClasseDto>>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<GrupoClasseDto> { Items = listDtos };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ListResultDto<GrupoClasseDto>> ListarPorGrupo(long id)
        {
            try
            {
                using (var grupoClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoClasse, long>>())
                {
                    var idGrid = 0;
                    var query = grupoClasseRepositorio.Object.GetAll().AsNoTracking().Where(m => m.GrupoId == id);

                    var list = await query.ToListAsync();

                    var listDto = list.MapTo<List<GrupoClasseDto>>();

                    listDto.ForEach(m => m.IdGridClasse = ++idGrid);

                    return new ListResultDto<GrupoClasseDto> { Items = listDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Recebe uma lista de objetos e devolve a mesma lista para popular um grid
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GrupoClasseDto>> ListarJson(List<GrupoClasseDto> list)
        {
            try
            {
                if (list == null)
                {
                    list = new List<GrupoClasseDto>();
                }

                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].IdGridClasse = i + 1;
                }

                var count = list.Count();

                return new PagedResultDto<GrupoClasseDto>(count, list);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar", ex));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<GrupoClasseDto>> Listar(ListarGruposClasseInput input)
        {
            var contarGruposClasse = 0;
            List<GrupoClasse> gruposClasse;
            List<GrupoClasseDto> gruposClasseDtos = new List<GrupoClasseDto>();
            try
            {
                //string id; 
                //long.TryParse(input.Filtro.ToString(), out id);

                using (var grupoClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoClasse, long>>())
                {
                    var query = grupoClasseRepositorio.Object
                    .GetAll().AsNoTracking()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Descricao.Contains(input.Filtro) || m.Id.ToString() == input.Filtro.ToString());

                    contarGruposClasse = await query.CountAsync();

                    gruposClasse = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync();

                    gruposClasseDtos = gruposClasse.MapTo<List<GrupoClasseDto>>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<GrupoClasseDto>(contarGruposClasse, gruposClasseDtos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CriarOuEditarGrupoClasse> Obter(long id)
        {
            try
            {
                using (var grupoClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoClasse, long>>())
                {
                    var result = await grupoClasseRepositorio.Object.GetAsync(id);
                    var grupoClasse = result.MapTo<CriarOuEditarGrupoClasse>();
                    return grupoClasse;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ListResultDto<GrupoClasseDto>> ObterPorGrupo(long id)
        {
            List<GrupoClasseDto> listDtos = new List<GrupoClasseDto>();
            try
            {
                using (var grupoClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoClasse, long>>())
                {
                    var result = await grupoClasseRepositorio.Object
                    .GetAll().AsNoTracking()
                    .Where(w => w.GrupoId == id)
                    .FirstOrDefaultAsync();

                    listDtos = result.MapTo<List<GrupoClasseDto>>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

            return new ListResultDto<GrupoClasseDto> { Items = listDtos };

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var grupoClasseRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<GrupoClasse, long>>())
            {
                return await Select2Helper.CreateSelect2(this, grupoClasseRepositorio.Object)
                .AddWhereMethod((input, dapperParamters) =>
                {
                    var whereBuilder = new StringBuilder(Select2Helper.DefaultWhereMethod(input, dapperParamters));

                    long grupoId = 0;
                    long.TryParse(input.filtro, out grupoId);

                    whereBuilder.Append(" AND GrupoId = @grupoId");

                    dapperParamters.Add("grupoId", grupoId);

                    return whereBuilder.ToString();
                }).ExecuteAsync(dropdownInput);
            }
        }
    }
}
