using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.MultiTenancy;
using Castle.Core.Internal;
using Hangfire;
using Sefaz;
using Sefaz.Entities;
using SW10.SWMANAGER.Background;
using SW10.SWMANAGER.Hangfire;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;

namespace SW10.SWMANAGER.ClassesAplicacao.Sefaz
{
    public class SefazTecnoSpeedSincronizaNotasJob : BackgroundJob<SefazTecnoSpeedSincronizaNotasJobArgs>, ITransientDependency
    {
        public static string JobName(string tenantId, string sefazConfiguracaoId)
        {
            return $"sefaz_nota_tenant_{tenantId}_sefaz_configuracao_{sefazConfiguracaoId}";
        }

        public static string JobName(int tenantId, long sefazConfiguracaoId)
        {
            return JobName(tenantId.ToString(), sefazConfiguracaoId.ToString());
        }

        //[DisableConcurrentExecutionWithParametersAttribute(10 * 60)]
        [UnitOfWork(false)]
        public override void Execute(SefazTecnoSpeedSincronizaNotasJobArgs args)
        {
            try
            {
                using (var recurringJobManager = IocManager.Instance.ResolveAsDisposable<Background.IRecurringJobManager>())
                {

                }
            }
            catch (Exception)
            {
                return;
            }

            var enableSefaz = ConfigurationManager.AppSettings["EnableSefaz"];
            if(enableSefaz == null || !bool.Parse(enableSefaz))
            {
                return;
            }

            using (var recurringJobManager = IocManager.Instance.ResolveAsDisposable<Background.IRecurringJobManager>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var sefazTecnospeedConfiguracaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AbpSefazTecnoSpeedConfiguracoes, long>>())
            {
                unitOfWorkManager.Object.Current.SetTenantId(args.TenantId);
                var connectionString = string.Empty;
                try
                {
                    using (var tenantCache = IocManager.Instance.ResolveAsDisposable<ITenantCache>())
                    {
                        var tenant = tenantCache.Object.Get(args.TenantId);
                        connectionString = ConfigurationManager.ConnectionStrings.Cast<ConnectionStringSettings>()
                            .FirstOrDefault(v => string.Compare(v.Name, tenant.TenancyName, StringComparison.OrdinalIgnoreCase) == 0)?.ConnectionString;
                        using (var conn = new SqlConnection(connectionString))
                        {
                            conn.Open();
                            conn.Close();
                        }
                    }
                }

                catch (Exception)
                {
                    return;
                }

                var config = sefazTecnospeedConfiguracaoRepository.Object.FirstOrDefault(args.SefazTecnoSpeedConfiguracaoId);
                if (config == null)
                {
                    recurringJobManager.Object.RemoveIfExists(JobName(args.TenantId, args.SefazTecnoSpeedConfiguracaoId));
                    return;
                }
                try
                {
                    using (var sefazCon = SefazHelper.ConexaoSefaz(SefazTecnoSpeedConfiguracoes.MapToSefazConfig(config)))
                    {
                        sefazCon.SincronizaNotasCronAsync(connectionString).GetAwaiter().GetResult();
                    }

                }
                
                catch (Exception e)
                {
                    SefazLogHelper.Error(config.Cnpj, $"SefazTecnoSpeedSincronizaNotasJob - {e.Message}", e);
                    if(SefazHelper.CaseInsensitiveContains(e.Message, "EspdManNFeStandBYException"))
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }
                }
            }
        }
    }
}
