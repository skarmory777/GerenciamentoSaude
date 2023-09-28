using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.AtestadosMedicos;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Assistenciais
{
    public class AtestadosMedicosController : SWMANAGERControllerBase
    {
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IMedicoAppService _medicoAppService;
        private readonly IMailingTemplateAppService _mailingTemplateAppService;

        public AtestadosMedicosController(
            IPacienteAppService pacienteAppService,
            IMedicoAppService medicoAppService,
            IMailingTemplateAppService mailingTemplateAppService
            )
        {
            _mailingTemplateAppService = mailingTemplateAppService;
            _pacienteAppService = pacienteAppService;
            _medicoAppService = medicoAppService;
        }
        // GET: Mpa/AtestadosMedicos
        public async Task<ActionResult> Index()
        {
            var paciente = TempData.Peek("Paciente") as PacienteDto;
            var medico = TempData.Peek("Medico") as MedicoDto;
            var empresa = TempData.Peek("Empresa") as EmpresaDto;

            var viewModel = new AtestadoMedicoViewModel();
            var templates = await _mailingTemplateAppService.ListarTodos();
            viewModel.Templates = templates.Items.Where(m => m.Name.Contains("Atestado Médico")).ToList();
            if (paciente != null)
            {
                viewModel.Paciente = paciente;
            }
            if (medico != null)
            {
                viewModel.Medico = medico;
            }
            if (empresa != null)
            {
                viewModel.Empresa = empresa;
            }
            return View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/AtestadosMedicos/Index.cshtml", viewModel);
        }
    }
}