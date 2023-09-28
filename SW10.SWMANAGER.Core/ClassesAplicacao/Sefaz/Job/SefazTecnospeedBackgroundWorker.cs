using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Threading.BackgroundWorkers;
using Hangfire;
using SW10.SWMANAGER.Background;
using SW10.SWMANAGER.Hangfire;
using SW10.SWMANAGER.MultiTenancy;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Sefaz
{
    public class SefazTecnospeedSincronizaNotasBackgroundWorker : BackgroundWorkerBase, ISingletonDependency
    {
        [DisableConcurrentExecutionWithParametersAttribute(24 * 60 * 60)]
        [UnitOfWork(false)]
        public override void Start()
        {
            using (var tenantRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Tenant, int>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            {
                var tenantIds = tenantRepository.Object.GetAll().Select(x => x.Id).ToList();
                //unitOfWorkManager.Object.Current.

                foreach (var tenantId in tenantIds)
                {
                    try
                    {
                        using (var tenantCache = IocManager.Instance.ResolveAsDisposable<ITenantCache>())
                        {
                            var tenant = tenantCache.Object.Get(tenantId);
                            var config = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>()
                                .FirstOrDefault(v => string.Compare(v.Name, tenant.TenancyName, StringComparison.OrdinalIgnoreCase) == 0)?.ConnectionString;
                            using (var conn = new SqlConnection(config))
                            {
                                conn.Open();
                                conn.Close();
                            }
                        }

                        unitOfWorkManager.Object.Current.SetTenantId(tenantId);
                        using (var sefazTecnoSpeedConfiguracoesRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AbpSefazTecnoSpeedConfiguracoes, long>>())
                        using (var recurringJobManager = IocManager.Instance.ResolveAsDisposable<Background.IRecurringJobManager>())
                        {
                            var sefazConfiguracoes = sefazTecnoSpeedConfiguracoesRepository.Object.GetAll().AsNoTracking().Select(x => x.Id).ToList();
                            unitOfWorkManager.Object.Current.SetTenantId(null);
                            foreach (var SefazTecnoSpeedConfiguracaoId in sefazConfiguracoes)
                            {
                                recurringJobManager.Object.AddOrUpdateAsync<SefazTecnoSpeedSincronizaNotasJob, SefazTecnoSpeedSincronizaNotasJobArgs>(
                                   SefazTecnoSpeedSincronizaNotasJob.JobName(tenantId, SefazTecnoSpeedConfiguracaoId),
                                    new SefazTecnoSpeedSincronizaNotasJobArgs
                                    {
                                        SefazTecnoSpeedConfiguracaoId = SefazTecnoSpeedConfiguracaoId,
                                        TenantId = tenantId
                                    }, "0 */2 * * *");
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //
                    }
                }
            }
        }
    }
}
