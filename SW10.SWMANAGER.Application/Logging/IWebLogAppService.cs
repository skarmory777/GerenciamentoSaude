using Abp.Application.Services;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Logging.Dto;

namespace SW10.SWMANAGER.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
