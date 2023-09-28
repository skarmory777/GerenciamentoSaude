using Abp.Application.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Interfaces
{
    public interface ITISSAppService : IApplicationService
    {
        DefaultReturn<string> GerarLoteXML(long loteId);
    }
}
