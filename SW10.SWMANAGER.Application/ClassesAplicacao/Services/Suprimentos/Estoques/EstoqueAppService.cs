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
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    public class EstoqueAppService : SWMANAGERAppServiceBase, IEstoqueAppService
    {
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Edit)]
        [UnitOfWork]
        public async Task CriarOuEditar(EstoqueDto input)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var estoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Estoque, long>>())
                using (var estoqueGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueGrupo, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var estoque = EstoqueDto.MapearBase<Estoque>(input);

                    if (input.Id.Equals(0))
                    {
                        var idEstoque = await estoqueRepositorio.Object.InsertAndGetIdAsync(estoque);

                        foreach (var item in input.EstoquesGrupo)
                        {
                            var EstoqueGrupo = new EstoqueGrupo
                            {
                                EstoqueId = idEstoque,
                                GrupoId = item.id,
                                IsTodosItens = item.checado
                            };

                            await estoqueGrupoRepository.Object.InsertOrUpdateAsync(EstoqueGrupo.MapTo<EstoqueGrupo>()).ConfigureAwait(false);
                        };
                    }
                    else
                    {
                        //Atualiza o estoque
                        await estoqueRepositorio.Object.UpdateAsync(estoque).ConfigureAwait(false);

                        //------------------------------------------------------

                        //Atualiza os estoquesGrupos
                        var aListaEstoquesGrupoBD =
                            from details in estoqueGrupoRepository.Object.GetAll()
                            where details.EstoqueId == input.Id
                            select (new MultiSelectItem { id = (long)details.GrupoId, checado = (bool)details.IsTodosItens });

                        var aListaIdEstoquesGrupoBD = new List<MultiSelectItem>();
                        var aListaIdEstoquesGrupoFront = new List<MultiSelectItem>();

                        aListaIdEstoquesGrupoBD = aListaEstoquesGrupoBD.ToList();
                        aListaIdEstoquesGrupoFront = input.EstoquesGrupo.Select(m => new MultiSelectItem { id = m.id, checado = m.checado }).ToList();

                        //excluindo grupos relacionados
                        var aListaDiferencaExcluir = aListaIdEstoquesGrupoBD.Except(aListaIdEstoquesGrupoFront);

                        foreach (var itemLoop in aListaDiferencaExcluir)
                        {
                            await estoqueGrupoRepository.Object.DeleteAsync(m => m.GrupoId == itemLoop.id && m.EstoqueId == input.Id).ConfigureAwait(false);
                        }

                        //incluindo grupos selecionados
                        var aListaDiferencaAdd = aListaIdEstoquesGrupoFront.Except(aListaIdEstoquesGrupoBD);

                        foreach (var itemLoop in aListaDiferencaAdd)
                        {
                            var EstoqueGrupo = new EstoqueGrupo
                            {
                                EstoqueId = input.Id,
                                GrupoId = itemLoop.id,
                                IsTodosItens = itemLoop.checado
                            };

                            await estoqueGrupoRepository.Object.InsertOrUpdateAsync(EstoqueGrupo.MapTo<EstoqueGrupo>()).ConfigureAwait(false);
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(EstoqueDto input)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var estoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Estoque, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await estoqueRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);

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

        public async Task<PagedResultDto<EstoqueDto>> Listar(ListarProdutosEstoqueInput input)
        {
            try
            {
                using (var estoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Estoque, long>>())
                {
                    var contarProdutosEstoque = 0;
                    List<Estoque> produtosEstoque;
                    List<EstoqueDto> produtosEstoqueDtos = new List<EstoqueDto>();

                    var query = estoqueRepositorio.Object
                        .GetAll()
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Descricao.Contains(input.Filtro));

                    contarProdutosEstoque = await query
                                                .CountAsync().ConfigureAwait(false);

                    produtosEstoque = await query
                                          .AsNoTracking()
                                          .OrderBy(input.Sorting)
                                          .PageBy(input)
                                          .ToListAsync().ConfigureAwait(false);

                    produtosEstoqueDtos = EstoqueDto.Mapear(produtosEstoque);

                    return new PagedResultDto<EstoqueDto>(contarProdutosEstoque, produtosEstoqueDtos);
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarProdutosEstoqueInput input)
        {
            try
            {
                var query = await this.Listar(input).ConfigureAwait(false);

                var produtosEstoqueDtos = query.Items;

                return null;
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        public async Task<EstoqueDto> Obter(long id)
        {
            try
            {
                using (var estoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Estoque, long>>())
                using (var estoqueGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueGrupo, long>>())
                {
                    var result = await estoqueRepositorio.Object.GetAsync(id).ConfigureAwait(false);
                    var Estoque = EstoqueDto.MapearBase<EstoqueDto>(result);//.MapTo<EstoqueDto>();

                    //-------------------------------------------------------------------
                    var query = estoqueGrupoRepository.Object
                        .GetAll()
                        .Where(m => m.EstoqueId == id);

                    var contarEstoqueGrupo = await query
                                                 .CountAsync().ConfigureAwait(false);

                    var estoquesGrupo = await query
                                            .AsNoTracking()
                                            .ToListAsync().ConfigureAwait(false);

                    //-------------------------------------------------------------------

                    // VERSAO ANTIGA ARRAY DE IDS

                    //long[] idsGrupos;

                    //idsGrupos = new long[contarEstoqueGrupo];

                    //long i = 0;

                    //foreach (var item in estoquesGrupo)
                    //{
                    //    idsGrupos[i] = (long)item.GrupoId;
                    //    i = i + 1;
                    //}

                    //Estoque.aEstoquesGrupo = idsGrupos;

                    //-------------------------------------------------------------------

                    // VERSAO NOVA - CHECKS FUNCIONANDOS

                    Estoque.EstoquesGrupo = new List<MultiSelectItem>();
                    foreach (var item in estoquesGrupo)
                    {
                        var msi = new MultiSelectItem
                        {
                            id = (long)item.GrupoId,
                            checado = (bool)item.IsTodosItens
                        };
                        Estoque.EstoquesGrupo.Add(msi);
                    }
                    return Estoque;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<EstoqueDto>> ListarTodos()
        {
            try
            {
                List<EstoqueDto> objListaDto = new List<EstoqueDto>();
                using (var estoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Estoque, long>>())
                {

                    var query = await estoqueRepositorio.Object.GetAllListAsync().ConfigureAwait(false);

                    objListaDto = EstoqueDto.Mapear(query);//.MapTo<List<EstoqueDto>>();

                    return new ListResultDto<EstoqueDto> { Items = objListaDto };
                }

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
                using (var estoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Estoque, long>>())
                {

                    var query = await estoqueRepositorio.Object
                        .GetAll()
                        .WhereIf(!input.IsNullOrEmpty(), m => m.Descricao.Contains(input))
                        .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                        .ToListAsync().ConfigureAwait(false);

                    return new ListResultDto<GenericoIdNome> { Items = query };
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------------------------------
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<EstoqueGrupoDto>> ListarEstoqueGrupo(ListarEstoqueGrupoInput input)
        //public async Task<PagedResultDto<EstoqueDto>> ListarEstoqueGrupo()
        {
            var contarEstoquesGrupo = 0;
            //List<EstoqueGrupo> estoquesGrupo;
            List<EstoqueGrupo> estoquesGrupo = new List<EstoqueGrupo>();
            List<EstoqueGrupoDto> estoquesGruposDtos = new List<EstoqueGrupoDto>();
            try
            {
                using (var estoqueGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueGrupo, long>>())
                {

                    var query = estoqueGrupoRepository.Object
                        .GetAll()
                        .Include(m => m.Grupo)
                         .Select(s => EstoqueGrupoDto.MapearBase<EstoqueGrupoDto>(s));
                    //.WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Descricao.ToUpper().Contains(input.Filtro.ToUpper()));

                    contarEstoquesGrupo = await query
                                              .CountAsync().ConfigureAwait(false);

                    List<EstoqueGrupo> lista = null;

                    //if (contarEstoquesGrupo == 0) {
                    if (contarEstoquesGrupo == 100)
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            EstoqueGrupo estqoueGrupo_loop = new EstoqueGrupo();
                            //lista.Add(estqoueGrupo_loop);
                            estqoueGrupo_loop.Codigo = null;
                            estoquesGrupo.Add(estqoueGrupo_loop);
                        }
                    }
                    else
                    {
                        estoquesGruposDtos = await query
                            .AsNoTracking()
                            .OrderBy(input.Sorting)
                            .PageBy(input)
                            .ToListAsync().ConfigureAwait(false);
                    };
                }

                //estoquesGruposDtos = estoquesGrupo
                //    .MapTo<List<EstoqueGrupoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<EstoqueGrupoDto>(
                contarEstoquesGrupo,
                estoquesGruposDtos);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ResultDropdownList(DropdownInput dropdownInput)
        {
            using (var estoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Estoque, long>>())
            {
                return await this.CreateSelect2(estoqueRepositorio.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var EstoqueRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Estoque, long>>())
            {
                return await this.CreateSelect2(EstoqueRepositorio.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }
    }
}
