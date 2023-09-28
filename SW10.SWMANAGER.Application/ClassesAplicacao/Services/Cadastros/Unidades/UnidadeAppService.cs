using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using Dapper;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades
{
    public class UnidadeAppService : SWMANAGERAppServiceBase, IUnidadeAppService
    {

        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Edit)]
        public async Task CriarOuEditar(UnidadeDto input)
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    var unidade = input.MapTo<Unidade>();
                    if (input.Id.Equals(0))
                    {

                        await unidadeRepositorio.Object.InsertOrUpdateAsync(unidade).ConfigureAwait(false);
                    }
                    else
                    {
                        await unidadeRepositorio.Object.UpdateAsync(unidade).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        /// <summary>
        /// Cria um novo produto e retorna um ProdutoDto com seu Id
        /// </summary>
        [UnitOfWork]
        public UnidadeDto CriarGetId(UnidadeDto input)
        {
            try
            {
                //input.Codigo = ObterProximoNumero(input);
                UnidadeDto unidadeDto;
                var unidade = input.MapTo<Unidade>();
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    if (input.Id.Equals(0))
                    {
                        //Inclui o unidade e retorna unidadeDto com o Id
                        unidadeDto = new UnidadeDto { Id = AsyncHelper.RunSync(() => unidadeRepositorio.Object.InsertAndGetIdAsync(unidade)) };
                    }
                    else
                    {
                        unidadeDto = AsyncHelper.RunSync(() => unidadeRepositorio.Object.UpdateAsync(unidade)).MapTo<UnidadeDto>();
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

                //unidadeDto.Codigo = input.Codigo;
                return unidadeDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }


        public async Task Excluir(UnidadeDto input)
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    await unidadeRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<UnidadeDto>> Listar(ListarUnidadesInput input)
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    var query = unidadeRepositorio.Object
                    .GetAll().AsNoTracking()
                    .Where(u => u.UnidadeReferenciaId == null)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Id.ToString().Contains(input.Filtro) ||
                        m.Sigla.Contains(input.Filtro) ||
                        m.Fator.ToString().Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                    );

                    var contarUnidades = await query
                                         .CountAsync().ConfigureAwait(false);

                    var unidades = await query
                                   .OrderBy(input.Sorting)
                                   .PageBy(input)
                                   .ToListAsync().ConfigureAwait(false);

                    var unidadesDtos = unidades.Select(UnidadeDto.Mapear).ToList();

                    return new PagedResultDto<UnidadeDto>(contarUnidades, unidadesDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<FileDto> ListarParaExcel(ListarUnidadesInput input)
        {
            try
            {
                using (var listarUnidadeExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarUnidadeExcelExporter>())
                {
                    var query = await this.Listar(input).ConfigureAwait(false);

                    var unidadesDtos = query.Items;

                    return listarUnidadeExcelExporter.Object.ExportToFile(unidadesDtos.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }

        }

        [UnitOfWork(false)]
        public async Task<UnidadeDto> Obter(long id)
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    return UnidadeDto.Mapear(await unidadeRepositorio.Object
                                             .GetAll().AsNoTracking()
                                             .Include(m => m.UnidadeReferencia)
                                             .Where(m => m.Id == id)
                                             .FirstOrDefaultAsync().ConfigureAwait(false));
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<IEnumerable<UnidadeDto>> ObterIds(List<long> ids)
        {
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    return null;
                }

                var query = $@"
                    SELECT 
                        {QueryHelper.CreateQueryFields<Unidade>(tableAlias: "Unidade").GetFields()}
                    FROM Est_Unidade AS Unidade
                    WHERE 
                        Unidade.Id IN @ids
                        AND Unidade.IsDeleted = 0
                    ";

                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    return await sqlConnection.QueryAsync<UnidadeDto>(query, new { ids = ids.Distinct().ToList() });
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<UnidadeDto> ObterUnidadeDto(long id)
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    return UnidadeDto.Mapear(await unidadeRepositorio.Object
                                 .GetAll()
                                 .Include(m => m.UnidadeReferencia)
                                 .Where(m => m.Id == id)
                                 .FirstOrDefaultAsync().ConfigureAwait(false));
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                ///TODO: NOVO PRODUTO
                //var query = await _unidadeRepositorio
                //    .GetAll()
                //    .WhereIf(!input.IsNullOrEmpty(), m =>
                //        m.Descricao.Contains(input)
                //        )
                //    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                //    .ToListAsync();

                //return new ListResultDto<GenericoIdNome> { Items = query };
                return new ListResultDto<GenericoIdNome> { Items = null };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<GenericoIdNome>> ListarAutoCompleteNaoUtilizado(string input, long produtoId)
        {
            try
            {
                ///TODO: NOVO PRODUTO
                //var query = await _unidadeRepositorio
                //    .GetAll()
                //    .WhereIf(!input.IsNullOrEmpty(), m =>
                //        m.Descricao.Contains(input
                //         )
                //        )
                //    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                //    .ToListAsync();

                return new ListResultDto<GenericoIdNome> { Items = null };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna todas as Unidades disponiveis
        /// </summary>
        [UnitOfWork(false)]
        public async Task<ListResultDto<UnidadeDto>> ListarTodos()
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    var query = await unidadeRepositorio.Object
                                .GetAll().AsNoTracking()
                                .Include(m => m.UnidadeReferencia)
                                .ToListAsync().ConfigureAwait(false);

                    return new ListResultDto<UnidadeDto> { Items = query.Select(UnidadeDto.Mapear).ToList() };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna todas as Unidades Referenciais disponiveis
        /// </summary>
        [UnitOfWork(false)]
        public async Task<ListResultDto<UnidadeDto>> ListarUnidadesReferenciais()
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    var query = await unidadeRepositorio.Object
                                .GetAll()
                                .AsNoTracking()
                                .Include(m => m.UnidadeReferencia)
                                .Where(a => a.UnidadeReferenciaId == null)
                                .ToListAsync().ConfigureAwait(false);

                    return new ListResultDto<UnidadeDto> { Items = query.Select(UnidadeDto.Mapear).ToList() };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna unidades vinculadas a uma Unidade Referencial. 
        /// Pode trazer a propria unidade referencial juntamente com suas "filhas"
        /// </summary>
        [UnitOfWork(false)]
        public async Task<ListResultDto<UnidadeDto>> ListarPorReferencial(long? id, bool addPai = false)
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    if (id.HasValue)
                    {
                        List<Unidade> query = null;

                        if (addPai)
                        {
                            query = await unidadeRepositorio.Object.GetAll().AsNoTracking().Include(m => m.UnidadeReferencia)
                                        .Where(a => a.Id == id || a.UnidadeReferenciaId == id).ToListAsync()
                                        .ConfigureAwait(false);
                        }
                        else
                        {
                            query = await unidadeRepositorio.Object.GetAll().AsNoTracking().Include(m => m.UnidadeReferencia)
                                        .Where(a => a.UnidadeReferenciaId == id).ToListAsync().ConfigureAwait(false);
                        }

                        return new ListResultDto<UnidadeDto> { Items = query.Select(UnidadeDto.Mapear).ToList() };
                    }
                    else
                    {
                        return new ListResultDto<UnidadeDto> { Items = new List<UnidadeDto>() };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<UnidadeDto>> ListarPorReferencial(ListarUnidadesInput input)
        {
            var contarUnidades = 0;
            List<Unidade> unidades;
            List<UnidadeDto> unidadesDtos = new List<UnidadeDto>();
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    var query = unidadeRepositorio.Object
                    .GetAll()
                    //.WhereIf(!input.Filtro.IsNullOrEmpty(), a => a.UnidadeReferenciaId.ToString().Contains(input.Filtro));
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), a => a.UnidadeReferenciaId.ToString() == input.Filtro);

                    contarUnidades = await query.CountAsync().ConfigureAwait(false);

                    unidades = await query
                                   .AsNoTracking()
                                   .OrderBy(input.Sorting)
                                   .PageBy(input)
                                   .ToListAsync().ConfigureAwait(false);

                    return new PagedResultDto<UnidadeDto>(contarUnidades, unidades.Select(UnidadeDto.Mapear).ToList());
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna todos os Ids de Unidade para uma Sigla especifica
        /// </summary>
        public async Task<List<long>> ListarIdsPorSigla(string sigla)
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    var query = await unidadeRepositorio.Object
                                .GetAll()
                                .Include(m => m.UnidadeReferencia)
                                .Where(a => a.Sigla == sigla)
                                //.Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                                .Select(m => m.Id)
                                .ToListAsync().ConfigureAwait(false);

                    var Ids = query.MapTo<List<long>>();

                    return Ids;
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna o Id de uma Unidade pela sua Sigla
        /// </summary>
        //public async Task<long> getIdUnidadelPorSigla(string sigla, bool? isReferencia = null)
        public async Task<long> GetIdUnidadelPorSigla(string sigla, bool? isReferencia = null, long? idRef = null)
        {
            try
            {
                List<long> query;
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    if (isReferencia.HasValue && isReferencia == true)
                    {
                        query = await unidadeRepositorio.Object
                                    .GetAll()
                                    .Include(m => m.UnidadeReferencia)
                                    .Where(a => a.Sigla == sigla && a.UnidadeReferenciaId == null)
                                    .Select(m => m.Id)
                                    .ToListAsync().ConfigureAwait(false);

                    }
                    else
                    {
                        query = await unidadeRepositorio.Object
                                    .GetAll()
                                    .Include(m => m.UnidadeReferencia)
                                    .Where(a => a.Sigla == sigla && ((a.UnidadeReferenciaId == idRef) || (a.UnidadeReferenciaId == null)))
                                    .Select(m => m.Id)
                                    .ToListAsync().ConfigureAwait(false);
                    };

                    long Id;

                    if (query.Count <= 0)
                    {
                        Id = -1;
                    }
                    else
                    {
                        Id = query.First();
                    };

                    return Id;
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna a Sigla de uma Unidade pelo seu Id
        /// </summary>
        public async Task<string> GetSiglaUnidadePeloId(long id)
        {
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    var query = await unidadeRepositorio.Object
                                .GetAll()
                                .Include(m => m.UnidadeReferencia)
                                .Where(a => a.Id == id)
                                .Select(m => m.Sigla)
                                .ToListAsync().ConfigureAwait(false);

                    return query.Count <= 0 ? "" : query.First();;
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
        public decimal ObterQuantidadeReferencia(long unidadeId, decimal quantidade)
        {
            using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
            {
                var unidade = unidadeRepositorio.Object
                .GetAll().AsNoTracking()
                .FirstOrDefault(m => m.Id == unidadeId);
                if (unidade != null)
                {
                    return unidade.Fator * quantidade;
                }

                return 0;
            }
        }

        public decimal ObterQuantidadePorFator(long unidadeId, decimal quantidade)
        {
            using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
            {
                var unidade = unidadeRepositorio.Object
                .GetAll().AsNoTracking()
                .FirstOrDefault(m => m.Id == unidadeId);
                if (unidade != null)
                {
                    return quantidade / unidade.Fator;
                }

                return 0;
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                {
                    var query = from p in unidadeRepositorio.Object.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.Contains(dropdownInput.search)
                                                                          || m.Codigo.ToString().Contains(dropdownInput.search)
                                                                      )
                                orderby p.Descricao ascending
                                select new DropdownItems { id = p.Id, text = string.Concat(p.Sigla.ToString(), " - ", p.Descricao) };

                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    int total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarUnidadePorProduto2Dropdown(DropdownInput dropdownInput)
        {
            long unidadeId;

            long.TryParse(dropdownInput.filtro, out unidadeId);
            using (var unidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
            using (var produtoUnidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade,long>>())
            {
                var produtosUnidade = produtoUnidadeRepositorio.Object.GetAll().AsNoTracking();

                return await this.ListarDropdownLambda(dropdownInput
                           , unidadeRepositorio.Object
                           , m => produtosUnidade.Any(a => a.UnidadeId == m.Id && a.UnidadeId == unidadeId)
                           , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                           , o => o.Descricao
                       ).ConfigureAwait(false);
            }

        }
    }
}
