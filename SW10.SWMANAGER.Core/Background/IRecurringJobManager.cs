using Abp.BackgroundJobs;
using Abp.Threading.BackgroundWorkers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Background
{
    public interface IRecurringJobManager : IBackgroundWorker
    {
        Task AddOrUpdateAsync<TJob, TArgs>(string recurringJobId, TArgs args, string cronExpressions, BackgroundJobPriority priority = BackgroundJobPriority.Normal) where TJob : IBackgroundJob<TArgs>;
        Task AddOrUpdateAsync<TJob, TArgs>(TArgs args, string cronExpressions, BackgroundJobPriority priority = BackgroundJobPriority.Normal)
            where TJob : IBackgroundJob<TArgs>;


        void RemoveIfExists(string recurringJobId);

        string GetJobStatus(string recurringName);
    }
}
