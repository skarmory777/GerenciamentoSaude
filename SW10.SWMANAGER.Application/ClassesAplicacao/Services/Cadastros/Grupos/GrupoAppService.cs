using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos
{
    public class GrupoAppService : SWMANAGERAppServiceBase, IGrupoAppService
    {
        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                using (var grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                {
                    var query = await grupoRepositorio.Object.GetAll().AsNoTracking()
                                    .WhereIf(
                                        !input.IsNullOrEmpty(),
                                        m => m.Descricao.ToUpper().Contains(input.ToUpper()))
                                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao }).ToListAsync()
                                    .ConfigureAwait(false);


                    return new ListResultDto<GenericoIdNome> { Items = query };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Grupo_Edit)]
        [UnitOfWork]
        public async Task<GrupoDto> CriarOuEditar(GrupoDto input)
        {
            try
            {
                var grupo = input.MapTo<Grupo>();
                using (var grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var classeAppService = IocManager.Instance.ResolveAsDisposable<IGrupoClasseAppService>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var classes = new List<GrupoClasseDto>();
                    if (!input.ClasseList.IsNullOrWhiteSpace())
                    {
                        classes = JsonConvert.DeserializeObject<List<GrupoClasseDto>>(input.ClasseList);
                    }

                    if (input.Id.Equals(0))
                    {

                        grupo.Codigo = ultimoIdAppService.Object.ObterProximoCodigo("grupo").Result;
                        grupo.Id = await grupoRepositorio.Object.InsertAndGetIdAsync(grupo).ConfigureAwait(false);
                    }
                    else
                    {
                        await grupoRepositorio.Object.UpdateAsync(grupo).ConfigureAwait(false);
                    }

                    foreach (var classe in classes)
                    {
                        if (input.Id.Equals(0))
                        {
                            classe.GrupoId = grupo.Id;
                        }
                        else
                        {
                            classe.GrupoId = input.Id;
                        }

                        if (classe.IsDeleted)
                        {
                            await classeAppService.Object.Excluir(classe).ConfigureAwait(false);
                        }
                        else
                        {
                            await classeAppService.Object.CriarOuEditar(classe).ConfigureAwait(false);
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

                return grupo.MapTo<GrupoDto>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        [UnitOfWork]
        public async Task Excluir(GrupoDto input)
        {
            try
            {
                using (var grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await grupoRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);

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

        public async Task<ListResultDto<GrupoDto>> ListarTodos()
        {
            List<Grupo> listCore;
            List<GrupoDto> listDtos = new List<GrupoDto>();
            try
            {
                using (var grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                {
                    listCore = await grupoRepositorio.Object
                                   .GetAll()
                                   .ToListAsync().ConfigureAwait(false);

                    listDtos = listCore
                        .MapTo<List<GrupoDto>>();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new ListResultDto<GrupoDto> { Items = listDtos };
        }

        public async Task<PagedResultDto<GrupoDto>> Listar(ListarGruposInput input)
        {
            var contarGrupo = 0;
            List<Grupo> Grupo;
            List<GrupoDto> GrupoDtos = new List<GrupoDto>();
            try
            {
                using (var _grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                {
                    var query = _grupoRepositorio.Object.GetAll().AsNoTracking().WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m => m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                        //m.Palavra.Contains(input.Filtro) ||
                        //m.Observacao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                    contarGrupo = await query.CountAsync().ConfigureAwait(false);

                    Grupo = await query.AsNoTracking().OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                .ConfigureAwait(false);

                    GrupoDtos = Grupo.MapTo<List<GrupoDto>>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<GrupoDto>(contarGrupo, GrupoDtos);
        }

        /// <summary>
        /// Cria um novo grupo e retorna um GrupoDto com seu Id
        /// </summary>
        [UnitOfWork]
        public GrupoDto CriarGetId(GrupoDto input)
        {
            try
            {
                //input.Codigo = ObterProximoNumero(input);
                GrupoDto grupoDto;
                var grupo = input.MapTo<Grupo>();
                using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var _grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                using (var unitOfWork = _unitOfWorkManager.Object.Begin())
                {
                    if (input.Id.Equals(0))
                    {
                        //Inclui o grupo e retorna grupoDto com o Id
                        grupoDto = new GrupoDto { Id = AsyncHelper.RunSync(() => _grupoRepositorio.Object.InsertAndGetIdAsync(grupo)) };
                    }
                    else
                    {
                        grupoDto = AsyncHelper.RunSync(() => _grupoRepositorio.Object.UpdateAsync(grupo)).MapTo<GrupoDto>();
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

                //grupoDto.Codigo = input.Codigo;
                return grupoDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarGruposInput input)
        {
            try
            {
                var query = await this.Listar(input).ConfigureAwait(false);

                var GrupoDtos = query.Items;
                using (var listarGrupoExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarGrupoExcelExporter>())
                {
                    return listarGrupoExcelExporter.Object.ExportToFile(GrupoDtos.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<GrupoDto> Obter(long id)
        {
            try
            {
                using (var _grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                {
                    var result = await _grupoRepositorio.Object.GetAsync(id).ConfigureAwait(false);
                    var grupo = result.MapTo<GrupoDto>();
                    return grupo;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            try
            {
                using (var grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                {
                    return await Select2Helper.CreateSelect2(this, grupoRepositorio.Object).ExecuteAsync(dropdownInput);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<IResultDropdownList<long>> ListarPorEstoqueDropdown(DropdownInput dropdownInput)
        {
            //return await ListarCodigoDescricaoDropdown(dropdownInput, _prescricaoItemRepositorio);
            long estoqueId = 0;
            var isEstoque = long.TryParse(dropdownInput.id, out estoqueId);
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var estoqueGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueGrupo, long>>())
                {
                    var query = from g in estoqueGrupoRepository.Object.GetAll().AsNoTracking().Include(m => m.Grupo)
                                    //.Include(m => m.Divisao.Subdivisoes)
                                    //.Where(m => (m.Divisao.IsDivisaoPrincipal && m.Divisao.Subdivisoes.Count() == 0))
                                    .WhereIf(estoqueId > 0, m => m.EstoqueId.Equals(estoqueId)).WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(),
                                        m => m.Descricao.Contains(dropdownInput.search)
                                             || m.Codigo.ToString().Contains(dropdownInput.search))
                                orderby g.Grupo.Descricao ascending
                                select new DropdownItems<long>
                                {
                                    id = g.Grupo.Id,
                                    text = string.Concat(g.Grupo.Codigo.ToString(), " - ", g.Grupo.Descricao)
                                };

                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList<long>() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<IResultDropdownList<long>> ListarDropdownGruposPorEstoque(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<GrupoDto> dtos = new List<GrupoDto>();
            try
            {
                //se um país estiver selecionado, filtra apenas pelos seus estados
                long estoqueId;
                long.TryParse(dropdownInput.filtro, out estoqueId);
                using (var grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                using (var estoqueGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueGrupo, long>>())
                {
                    //get com filtro
                    var query = (from p in estoqueGrupoRepository.Object.GetAll().AsNoTracking().WhereIf(
                                     !dropdownInput.filtro.IsNullOrEmpty(),
                                     m => m.EstoqueId == estoqueId)
                                 join grupo in grupoRepositorio.Object.GetAll().AsNoTracking() on p.GrupoId equals grupo.Id
                                 select grupo).Distinct();

                    var query2 = query.Where(m => m.Descricao != null && m.Descricao.Trim() != "").WhereIf(
                        !dropdownInput.search.IsNullOrEmpty(),
                        m => m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                             || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()));

                    var lista = from ss in query2
                                orderby ss.Descricao ascending
                                select new DropdownItems<long>
                                {
                                    id = ss.Id,
                                    text = string.Concat(ss.Id, " - ", ss.Codigo, " - ", ss.Descricao)
                                };

                    //paginação 
                    var queryResultPage = lista //query
                        .Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList<long>() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdownGruposPorEstoqueIdObrigatorio(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<GrupoDto> dtos = new List<GrupoDto>();
            try
            {
                //se um país estiver selecionado, filtra apenas pelos seus estados
                long estoqueId;
                long.TryParse(dropdownInput.filtro, out estoqueId);

                if (estoqueId == 0)
                {
                    return new ResultDropdownList() { Items = new List<DropdownItems>(), TotalCount = 0 };
                }

                using (var grupoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Grupo, long>>())
                using (var estoqueGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueGrupo, long>>())
                {
                    //get com filtro
                    var query = (from p in estoqueGrupoRepository.Object.GetAll().AsNoTracking().WhereIf(
                                     !dropdownInput.filtro.IsNullOrEmpty(),
                                     m => m.EstoqueId == estoqueId)
                                 join grupo in grupoRepositorio.Object.GetAll().AsNoTracking() on p.GrupoId equals grupo.Id
                                 select grupo).Distinct();

                    var query2 = query.Where(m => m.Descricao != null && m.Descricao.Trim() != "").WhereIf(
                        !dropdownInput.search.IsNullOrEmpty(),
                        m => m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                             || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()));

                    var lista = from ss in query2
                                orderby ss.Descricao ascending
                                select new DropdownItems<long>
                                {
                                    id = ss.Id,
                                    text = string.Concat(ss.Id, " - ", ss.Codigo, " - ", ss.Descricao)
                                };

                    //paginação 
                    var queryResultPage = lista //query
                        .Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList<long>() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


    }
}
