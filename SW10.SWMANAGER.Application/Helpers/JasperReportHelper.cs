using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Helpers
{
    public static class JasperReportHelper
    {
        public static JasperReport CreateJasperReport(this SWMANAGERAppServiceBase appService, string reportUrl)
        {
            var jasperReport = new JasperReport(ConfigurationManager.AppSettings.Get("reportBaseUrl").ToString(), reportUrl);
            return jasperReport.AddParameter("TenancyName", appService.GetCurrentTenant().TenancyName);
        }
        public static JasperReport CreateJasperReport(string baseUrl, string reportUrl)
        {
            return new JasperReport(baseUrl, reportUrl);
        }
    }

    public class JasperReport
    {
        public JasperReport()
        {

        }

        public JasperReport(string baseUrl, string reportUrl)
        {
            this.baseUrl = baseUrl;
            this.reportUrl = reportUrl;
        }

        private string baseUrl;
        private Method MethodType;
        private string reportUrl;
        private Dictionary<string, string> Parameters = new Dictionary<string, string>();

        public JasperReport SetMethod(Method method)
        {
            MethodType = method;
            return this;
        }

        public JasperReport AddParameter(string paramaterName, string value)
        {
            if (!Parameters.ContainsKey(paramaterName))
            {
                Parameters.Add(paramaterName, value);
            }
            return this;
        }

        public JasperReport AddParameters(ExpandoObject parameters)
        {
            foreach (var item in parameters)
            {
                AddParameter(item.Key, item.Value.ToString());
            }
            return this;
        }

        public JasperReport AddParameters(Dictionary<string,string> parameters)
        {
            foreach (var item in parameters)
            {
                AddParameter(item.Key, item.Value);
            }
            return this;
        }

        public JasperReport UpdateParameter(string paramaterName, string value)
        {
            if (Parameters.ContainsKey(paramaterName))
            {
                Parameters[paramaterName] = value;
            }
            return this;
        }

        public JasperReport RemoveParameter(string paramaterName)
        {
            if (Parameters.ContainsKey(paramaterName))
            {
                Parameters.Remove(paramaterName);
            }
            return this;
        }

        public byte[] GenerateReport()
        {
            var client = new RestClient(this.baseUrl);
            var request = new RestRequest(this.reportUrl, MethodType);

            foreach (var parameter in Parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }

            return client.DownloadData(request);
        }

        public string GetUrlReport()
        {
            var client = new RestClient(this.baseUrl);
            var request = new RestRequest(this.reportUrl, MethodType);

            foreach (var parameter in Parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }

            return client.BuildUri(request).ToString();
        }
    }
}
