//using EnvioEmail.Core;
using EnvioEmail.Core;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Manutencoes.MailingTemplates;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Manutencoes
{
    public class MailingTemplatesController : Controller
    {
        private readonly IMailingTemplateAppService _mailingTemplateAppService;

        public MailingTemplatesController(IMailingTemplateAppService mailingTemplateAppService)
        {
            _mailingTemplateAppService = mailingTemplateAppService;
        }

        // GET: Mpa/MailingTemplates
        public ActionResult Index()
        {
            var model = new MailingTemplatesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Manutencoes/MailingTemplates/Index.cshtml", model);
        }

        public async System.Threading.Tasks.Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var model = new MailingTemplateDto();
            if (id.HasValue)
            {
                model = await _mailingTemplateAppService.Obter(id.Value);
            }
            List<MailingCore.ClassDetails> classes = new List<MailingCore.ClassDetails>();
            var coreConfig = new MailingCore();

            classes.Add(coreConfig.SetTypeFields(new Medico().GetType()));
            classes.Add(coreConfig.SetTypeFields(new Paciente().GetType()));
            classes.Add(coreConfig.SetTypeFields(new Empresa().GetType()));

            List<SelectListItem> combo = new List<SelectListItem>();
            combo.AddRange(classes
                .Select(c => new SelectListItem
                {
                    Value = c.PropriertyTemplate,
                    Text = c.Name + ", " + c.FullName
                }));

            //ViewBag.comboClasses = combo;

            var viewModel = new CriarOuEditarMailingTemplateModalViewModel(model);
            viewModel.Classes = new SelectList(combo, "Value", "Text");
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Manutencoes/MailingTemplates/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}