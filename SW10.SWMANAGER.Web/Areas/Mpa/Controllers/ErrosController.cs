using Abp.Web.Mvc.Controllers;

using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Erros;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{
    public class ErrosController : AbpController
    {
        public class ErrosString
        {
            public string CodigoErro { get; set; }
            public string Descricao { get; set; }
        }


        public ActionResult ExibirErros(List<ErroDto> erros)
        {
            //List<ErroDto> _erros = JsonConvert.DeserializeObject<List<ErroDto>>(erros);

            var view = new ErrosViewModel { Erros = erros };

            return PartialView("~/Areas/Mpa/Views/Erros/Index.cshtml", view);
        }

        public ActionResult ExibirErrosWarnings(List<ErroDto> erros, List<ErroDto> warnings)
        {
            //List<ErroDto> _erros = JsonConvert.DeserializeObject<List<ErroDto>>(erros);

            var view = new ErrosViewModel { Erros = erros, Warnings = warnings };

            return PartialView("~/Areas/Mpa/Views/Erros/Index.cshtml", view);
        }

        public ActionResult ExibirErrosString(List<ErrosString> errosString)
        {
            List<ErroDto> erros = new List<ErroDto>();

            foreach (var item in errosString)
            {
                erros.Add(new ErroDto { CodigoErro = item.CodigoErro, Descricao = item.Descricao });
            }


            return ExibirErros(erros);
        }
    }
}