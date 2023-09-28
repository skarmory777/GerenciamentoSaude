using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using SW10.SWMANAGER.MultiTenancy;

namespace SW10.SWMANAGER.ClassesAplicacao.Avisos
{
    public class AvisoBackgroundWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        public AvisoBackgroundWorker(AbpTimer timer) : base(timer)
        {
            //Timer.Period = 600000; // 10 minutos;
            Timer.Period = 60000; // 10 minutos;
        }
        
        [UnitOfWork(false)]
        protected override void DoWork()
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
                        using (var avisoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Aviso, long>>())
                        using (var backgroundJobManager = IocManager.Instance.ResolveAsDisposable<IBackgroundJobManager>())
                        {
                            var avisosPendentes = avisoRepository.Object
                                .GetAll()
                                .AsNoTracking()
                                .Where(x => x.DataProgramada <= DateTime.Now && !x.DataFinalDisparo.HasValue && !x.DisparoAtivo).Select(x => x.Id)
                                .ToList();
                            var timeDelay = 0d;
                            unitOfWorkManager.Object.Current.SetTenantId(null);
                            foreach (var avisoPendenteId in avisosPendentes)
                            {
                                backgroundJobManager.Object.Enqueue<AvisoJob, AvisoJobArgs>(
                                        new AvisoJobArgs
                                        {
                                            Id = avisoPendenteId,
                                            TenantId = tenantId
                                        },
                                        delay: TimeSpan.FromSeconds(timeDelay));
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