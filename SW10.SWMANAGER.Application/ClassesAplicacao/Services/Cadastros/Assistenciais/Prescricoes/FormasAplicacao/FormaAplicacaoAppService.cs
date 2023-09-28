using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao
{
    public class FormaAplicacaoAppService : SWMANAGERAppServiceBase, IFormaAplicacaoAppService
    {
        [UnitOfWork]
        public async Task<FormaAplicacaoDto> CriarOuEditar(FormaAplicacaoDto input)
        {
            try
            {
                using (var formaAplicacaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var formaAplicacao = FormaAplicacaoDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await formaAplicacaoRepositorio.Object.InsertAndGetIdAsync(formaAplicacao);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                    else
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            formaAplicacao = await formaAplicacaoRepositorio.Object.UpdateAsync(formaAplicacao);
                            unitOfWork.Complete();
                            unitOfWork.Dispose();
                            return input;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(FormaAplicacaoDto input)
        {
            try
            {
                using (var formaAplicacaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await formaAplicacaoRepositorio.Object.DeleteAsync(input.Id);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<FormaAplicacaoDto>> Listar(ListarInput input)
        {
            var contarFormaAplicacao = 0;
            List<FormaAplicacao> formaAplicacao;
            List<FormaAplicacaoDto> FormaAplicacaoDtos = new List<FormaAplicacaoDto>();
            try
            {
                using (var formaAplicacaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
                {
                    var query = formaAplicacaoRepositorio.Object
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                        );

                    contarFormaAplicacao = await query
                                               .CountAsync().ConfigureAwait(false);

                    formaAplicacao = await query
                                         .AsNoTracking()
                                         .OrderBy(input.Sorting)
                                         .PageBy(input)
                                         .ToListAsync().ConfigureAwait(false);

                    FormaAplicacaoDtos = FormaAplicacaoDto.Mapear(formaAplicacao).ToList();

                    return new PagedResultDto<FormaAplicacaoDto>(contarFormaAplicacao, FormaAplicacaoDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        

        [UnitOfWork(false)]
        public async Task<FormaAplicacaoDto> Obter(long id)
        {
            try
            {
                using (var formaAplicacaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
                {
                    var result = await formaAplicacaoRepositorio.Object
                    .GetAll()
                    .AsNoTracking()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                    var formaAplicacao = FormaAplicacaoDto.Mapear(result);

                    return formaAplicacao;
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<IEnumerable<FormaAplicacaoDto>> ObterIds(List<long> ids)
        {
            try
            {
                if (ids.IsNullOrEmpty())
                {
                    return null;
                }

                var query = $@"
                    SELECT 
                        {QueryHelper.CreateQueryFields<FormaAplicacao>(tableAlias: "FormaAplicacao").GetFields()}
                    FROM AssFormaAplicacao AS FormaAplicacao
                    WHERE 
                        FormaAplicacao.Id IN @ids
                        AND FormaAplicacao.IsDeleted = 0
                    ";

                using (var sqlConnection = new SqlConnection(this.GetConnection()))
                {
                    return await sqlConnection.QueryAsync<FormaAplicacaoDto>(query, new { ids = ids.Distinct().ToList() });
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<FormaAplicacaoDto> Obter(string formaAplicacao)
        {
            try
            {
                using (var formaAplicacaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
                {
                    var query = formaAplicacaoRepositorio.Object
                    .GetAll()
                    .WhereIf(!formaAplicacao.IsNullOrWhiteSpace(), m =>
                     m.Codigo.Contains(formaAplicacao) ||
                     m.Descricao.Contains(formaAplicacao)
                    );
                    var result = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                    var _formaAplicacao = FormaAplicacaoDto.Mapear(result);

                    return _formaAplicacao;
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormaAplicacaoDto>> ListarTodos()
        {
            try
            {
                using (var formaAplicacaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
                {
                    var query = formaAplicacaoRepositorio.Object
                    .GetAll();

                    var formaAplicacao = await query
                                             .AsNoTracking()
                                             .ToListAsync().ConfigureAwait(false);

                    var formasAplicacaoDto = FormaAplicacaoDto.Mapear(formaAplicacao).ToList();

                    return new ListResultDto<FormaAplicacaoDto>
                    {
                        Items = formasAplicacaoDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<ListResultDto<FormaAplicacaoDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var formaAplicacaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
                {
                    var query = formaAplicacaoRepositorio.Object
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrEmpty(), m => m.Codigo.Contains(filtro) || m.Descricao.Contains(filtro));

                    var formaAplicacao = await query
                                             .AsNoTracking()
                                             .ToListAsync().ConfigureAwait(false);

                    var formasAplicacaoDto = FormaAplicacaoDto.Mapear(formaAplicacao).ToList();

                    return new ListResultDto<FormaAplicacaoDto>
                    {
                        Items = formasAplicacaoDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(ConfiguracaoPrescricaoItemDropDownInput dropdownInput)
        {
            using (var formaAplicacaoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<FormaAplicacao, long>>())
            {
                return await this.CreateSelect2(formaAplicacaoRepositorio.Object).AddTextField("Descricao").AddOrderByClause("Descricao")
                    .AddWhereMethod((input, dapperParameters) =>
                    {
                        var whereBuilder = new StringBuilder(Select2Helper.DefaultWhereMethod(input, dapperParameters));
                        var ids = new List<int>();

                        if (!dropdownInput.JsonFilter.IsNullOrEmpty())
                        {
                            var jsonFilter = JsonConvert.DeserializeObject<ConfiguracaoPrescricaoItemJsonFilter>(dropdownInput.JsonFilter);
                            dapperParameters.Add("options", jsonFilter.Options);
                            dapperParameters["id"] = jsonFilter.Id;
                            if (!jsonFilter.Id.IsNullOrEmpty())
                            {
                                whereBuilder.Append(" AND  id IN @id");
                                return whereBuilder.ToString();
                            }
                        }

                        if (!input.id.IsNullOrEmpty())
                        {
                            var id = 0;
                            int.TryParse(input.id, out id);
                            dapperParameters["id"] = id;
                            whereBuilder.Append(" AND  id = @id");
                            return whereBuilder.ToString();
                        }
                        if (!input.filtro.IsNullOrEmpty())
                        {
                            var id = 0;
                            int.TryParse(input.filtro, out id);
                            dapperParameters["filtroId"] = id;
                            ids.Add(id);
                        }

                        if (!input.filtros.IsNullOrEmpty())
                        {
                            var filtroIds = Array.ConvertAll(input.filtros, s => int.Parse(s));
                            dapperParameters["filtros"] = filtroIds;
                            ids.AddRange(filtroIds);
                        }
                        if (!ids.IsNullOrEmpty())
                        {
                            dapperParameters["ids"] = ids;
                            whereBuilder.Append(" AND  id IN (SELECT FormaApplicacaoId FROM AssVelocidadeInfusaoFormaAplicacao WHERE VelocidadeInfusaoId IN @ids AND IsDeleted = @deleted)");
                        }
                        return whereBuilder.ToString();
                    })
                    .ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
        }

        //public Task<FileDto> ListarParaExcel(ListarInput input)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
