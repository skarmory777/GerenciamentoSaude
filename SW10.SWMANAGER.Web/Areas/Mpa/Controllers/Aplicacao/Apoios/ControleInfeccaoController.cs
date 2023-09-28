﻿using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.ControleInfeccao;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class ControleInfeccaoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new ControleInfeccaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/ControleInfeccao/Index.cshtml", viewModel);
        }
    }
}