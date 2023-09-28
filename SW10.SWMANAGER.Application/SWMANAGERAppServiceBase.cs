using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER
{
    using Abp.Application.Services;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Extensions;
    using Abp.IdentityFramework;
    using Abp.Linq.Extensions;
    using Abp.MultiTenancy;
    using Abp.Runtime.Session;
    using Abp.UI;
    using Microsoft.AspNet.Identity;
    using MoreLinq;
    using SW10.SWMANAGER.Authorization.Users;
    using SW10.SWMANAGER.ClassesAplicacao;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
    using SW10.SWMANAGER.MultiTenancy;
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// All application services in this application is derived from this class.
    /// We can add common application service methods here.
    /// </summary>
    public abstract class SWMANAGERAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }

        protected SWMANAGERAppServiceBase()
        {
            LocalizationSourceName = SWMANAGERConsts.LocalizationSourceName;
        }

        protected virtual Task<User> GetCurrentUserAsync()
        {
            var user = UserManager.FindByIdAsync(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }

        protected virtual User GetCurrentUser()
        {
            var user = UserManager.FindById(AbpSession.GetUserId());
            if (user == null)
            {
                throw new ApplicationException("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
            }
        }

        public virtual Tenant GetCurrentTenant()
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                return TenantManager.GetById(AbpSession.GetTenantId());
            }
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            try
            {
                identityResult.CheckErrors(this.LocalizationManager);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }

        public string GetConnection()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            using (var tenantCache = IocManager.Instance.ResolveAsDisposable<ITenantCache>())
            {
                var tenant = tenantCache.Object.Get(this.AbpSession.GetTenantId());
                return ConfigurationManager.ConnectionStrings
                    .Cast<ConnectionStringSettings>()
                    .FirstOrDefault(v => string.Compare(v.Name, tenant.TenancyName, StringComparison.OrdinalIgnoreCase) == 0)
                    ?.ConnectionString;
            }
        }

        public string GetConnectionStringName()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            using (var tenantCache = IocManager.Instance.ResolveAsDisposable<ITenantCache>())
            {
                var tenant = tenantCache.Object.Get(this.AbpSession.GetTenantId());
                return GetConnectionStringNameByTenant(tenant.TenancyName);
            }
        }
        
        public string GetConnectionStringName(int tenantId)
        {
            using (var tenantCache = IocManager.Instance.ResolveAsDisposable<ITenantCache>())
            {
                var tenant = tenantCache.Object.Get(tenantId);
                return GetConnectionStringNameByTenant(tenant.TenancyName);
            }
        }

        public string GetConnectionStringNameByTenant(string tenancyName)
        {
            return ConfigurationManager.ConnectionStrings
                .Cast<ConnectionStringSettings>()
                .FirstOrDefault(v => string.Compare(v.Name, tenancyName, StringComparison.OrdinalIgnoreCase) == 0)
                ?.Name;
        }

        protected virtual async Task<ResultDropdownList<long>> ListarDropdownLambda<T>(
            DropdownInput dropdownInput,
            IRepository<T, long> repository,
            Expression<Func<T, bool>> myWhere,
            Expression<Func<T, DropdownItems<long>>> mySelect,
            Expression<Func<T, string>> myOrderBy)
            where T : CamposPadraoCRUD
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                var query = repository.GetAll().AsNoTracking()
                     .Where(myWhere)
                     .OrderBy(myOrderBy)
                     .Select(mySelect);

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var total = await query.CountAsync().ConfigureAwait(false);
                var list = await queryResultPage.ToListAsync().ConfigureAwait(false);

                return new ResultDropdownList<long>() { Items = list, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        protected virtual async Task<ResultDropdownList> ListarCodigoDescricaoDropdown<T>(DropdownInput dropdownInput, IRepository<T, long> repository) where T : CamposPadraoCRUD
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                var query = from p in repository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync().ConfigureAwait(false);

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        protected static void ErrorHandler<TTYpe>(DefaultReturn<TTYpe> retorno, Exception ex) where TTYpe : class
        {
            if (ex.InnerException != null)
            {
                var inner = ex.InnerException;
                retorno.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
            }
            else
            {
                retorno.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
            }
        }



        protected virtual async Task<ResultDropdownList> ListarDescricaoDropdown<T>(DropdownInput dropdownInput, IRepository<T, long> repository) where T : CamposPadraoCRUD
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                var query = from p in repository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m => m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()))
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Descricao) };

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