using Abp.Application.Services;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Interfaces
{
    public interface IOperacoesTISSAppService : IApplicationService
    {
        DefaultReturn<string> GerarLoteXML(long loteId);
    }
}
