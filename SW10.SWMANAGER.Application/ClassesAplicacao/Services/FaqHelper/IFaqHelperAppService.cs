using Abp.Application.Services;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.FaqHelper
{
    public interface IFaqHelperAppService : IApplicationService
    {
        List<FaqPerguntasViewModel> PerguntasPorUrl(string urlPath);

        List<FaqPerguntasViewModel> VideosPorUrl(string urlPath);

    }


    public class FaqPerguntasViewModel
    {
        public string UrlPath { get; set; }
        public string Pergunta { get; set; }

        public string Resposta { get; set; }

        public string VideoPath { get; set; }
    }

}
