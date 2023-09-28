using Abp.BackgroundJobs;
using Abp.Hangfire.Configuration;
using Abp.Threading.BackgroundWorkers;
using Hangfire;
using Hangfire.Storage;
using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Web.Background
{
    public class HangfireRecurringJobManager : BackgroundWorkerBase, SWMANAGER.Background.IRecurringJobManager
    {
        private readonly IBackgroundJobConfiguration backgroundJobConfiguration;
        private readonly IAbpHangfireConfiguration hangfireConfiguration;

        public HangfireRecurringJobManager(IAbpHangfireConfiguration hangfireConfiguration, IBackgroundJobConfiguration backgroundJobConfiguration)
        {
            this.hangfireConfiguration = hangfireConfiguration;
            this.backgroundJobConfiguration = backgroundJobConfiguration;
        }

        public override void Start()
        {
            base.Start();

            if (hangfireConfiguration.Server == null && backgroundJobConfiguration.IsJobExecutionEnabled)
            {
                hangfireConfiguration.Server = new BackgroundJobServer();
            }
        }

        public override void WaitToStop()
        {
            if (hangfireConfiguration.Server != null)
            {
                try
                {
                    hangfireConfiguration.Server.Dispose();
                }
                catch (Exception ex)
                {
                    Logger.Warn(ex.ToString(), ex);
                }
            }

            base.WaitToStop();
        }

        public Task AddOrUpdateAsync<TJob, TArgs>(string recurringJobId, TArgs args, string cronExpressions, BackgroundJobPriority priority = BackgroundJobPriority.Normal)
            where TJob : IBackgroundJob<TArgs>
        {
            RecurringJob.AddOrUpdate<TJob>(recurringJobId, job => job.Execute(args), cronExpressions);
            return Task.FromResult(0);
        }

        public Task AddOrUpdateAsync<TJob, TArgs>(TArgs args, string cronExpressions, BackgroundJobPriority priority = BackgroundJobPriority.Normal)
            where TJob : IBackgroundJob<TArgs>
        {
            RecurringJob.AddOrUpdate<TJob>(job => job.Execute(args), cronExpressions);
            return Task.FromResult(0);
        }


        public void RemoveIfExists(string recurringJobId)
        {
            RecurringJob.RemoveIfExists(recurringJobId);
        }

        public string GetJobStatus(string recurringJobId)
        {
            var recJob = JobStorage.Current.GetConnection().GetAllEntriesFromHash($"recurring-job:{ recurringJobId}");
            var job = JobStorage.Current.GetConnection().GetRecurringJobs().Where(x => x.Id == recurringJobId).FirstOrDefault();

            if (job != null)
            {
                return job.LastJobState;
            }
            return null;
        }
    }
}