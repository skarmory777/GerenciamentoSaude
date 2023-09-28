

using Abp.Application.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.Helpers;
using System.Dynamic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.JasperRelatorios
{
    public class JasperReportsAppService : SWMANAGERAppServiceBase, IJasperReportsAppService
    {
        public byte[] Relatorio(string reportUrl, string parameters)
        {
            return this.CreateJasperReport(reportUrl).GenerateReport();
        }

        public string RelatorioUrl(string reportUrl, string parameters)
        {
            dynamic JSONparameters = JsonConvert.DeserializeObject<ExpandoObject>(parameters, new ExpandoObjectConverter());
            JSONparameters.tenancyName = this.GetCurrentTenant()?.TenancyName;

            return this.CreateJasperReport(reportUrl).AddParameters(JSONparameters).GetUrlReport();
        }
    }

    public interface IJasperReportsAppService : IApplicationService
    {
        byte[] Relatorio(string reportUrl, string parameters);
        string RelatorioUrl(string reportUrl, string parameters);
    }
}
