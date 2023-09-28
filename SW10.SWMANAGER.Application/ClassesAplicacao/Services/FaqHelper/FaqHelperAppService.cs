using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.FaqHelper
{
    public class FaqHelperAppService : SWMANAGERAppServiceBase, IFaqHelperAppService
    {
        public List<FaqPerguntasViewModel> PerguntasPorUrl(string urlPath)
        {
            return new List<FaqPerguntasViewModel>{
                new FaqPerguntasViewModel()
                {
                    UrlPath = "/Internacoes",
                    Pergunta = "Como fazer uma internação?",
                    Resposta = " Teste",
                    VideoPath = null
                }
            };
        }

        public List<FaqPerguntasViewModel> VideosPorUrl(string urlPath)
        {
            throw new NotImplementedException();
        }
    }

}
