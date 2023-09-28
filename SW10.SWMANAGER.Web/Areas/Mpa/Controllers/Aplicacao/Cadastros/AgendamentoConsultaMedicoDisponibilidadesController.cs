//using Abp.AutoMapper;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.Web.Controllers;
using SW10.SWMANAGER.Web.Extensions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class AgendamentoConsultaMedicoDisponibilidadesController : SWMANAGERControllerBase
    {
        private readonly IAgendamentoConsultaMedicoDisponibilidadeAppService _agendamentoConsultaMedicoDisponibilidadeAppService;
        private readonly IMedicoAppService _medicoAppService;
        private readonly IIntervaloAppService _intervaloAppService;
        private readonly IEspecialidadeAppService _especialidadeAppService;

        public AgendamentoConsultaMedicoDisponibilidadesController(
            IAgendamentoConsultaMedicoDisponibilidadeAppService agendamentoConsultaMedicoDisponibilidadeAppService,
            IMedicoAppService medicoAppService,
            IIntervaloAppService intervaloAppService,
            IEspecialidadeAppService especialidadeAppService
            )
        {
            _agendamentoConsultaMedicoDisponibilidadeAppService = agendamentoConsultaMedicoDisponibilidadeAppService;
            _medicoAppService = medicoAppService;
            _intervaloAppService = intervaloAppService;
            _especialidadeAppService = especialidadeAppService;
        }

        public ActionResult Index()
        {
            var model = new AgendamentoConsultaMedicoDisponibilidadesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/AgendamentoConsultaMedicoDisponibilidades/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_AgendamentoConsultaMedicoDisponibilidade_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var teste = Thread.CurrentThread.CurrentCulture;
            var intervalos = await _intervaloAppService.ListarTodos();
            CriarOuEditarAgendamentoConsultaMedicoDisponibilidadeModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _agendamentoConsultaMedicoDisponibilidadeAppService.Obter((long)id);
                var horaIni = output.HoraInicio;
                var horaFim = output.HoraFim;
                var _horaIni = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, horaIni.Hour, horaIni.Minute, horaIni.Second);
                var _horaFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, horaFim.Hour, horaFim.Minute, horaFim.Second);
                // var agendamento = output.MapTo<CriarOuEditarAgendamentoConsultaMedicoDisponibilidade>();
                viewModel = new CriarOuEditarAgendamentoConsultaMedicoDisponibilidadeModalViewModel(output);
                viewModel.HoraInicio = _horaIni;
                viewModel.HoraFim = _horaFim;
                viewModel.HorariosInicio = new SelectList(FuncoesGlobais.Horarios().Select(m => new { HoraInicio = m, Text = string.Format("{0:D2}:{1:D2}", m.Hour, m.Minute) }), "HoraInicio", "Text", _horaIni);
                viewModel.HorariosFim = new SelectList(FuncoesGlobais.Horarios().Select(m => new { HoraFim = m, Text = string.Format("{0:D2}:{1:D2}", m.Hour, m.Minute) }), "HoraFim", "Text", _horaFim);
                viewModel.Intervalos = new SelectList(intervalos.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.Nome) }), "Id", "Nome", output.IntervaloId);
            }
            else
            {
                viewModel = new CriarOuEditarAgendamentoConsultaMedicoDisponibilidadeModalViewModel(new CriarOuEditarAgendamentoConsultaMedicoDisponibilidade());
                viewModel.Intervalos = new SelectList(intervalos.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.Nome) }), "Id", "Nome");
                viewModel.HorariosInicio = new SelectList(FuncoesGlobais.Horarios().Select(m => new { HoraInicio = teste.ToString().ToUpper() == "PT-BR" ? m.ToString("dd/MM/yyyy HH:mm") : teste.ToString().ToUpper() == "EN" ? m.ToString("MM/dd/yyyy HH:mm") : m.ToString("yyyy/MM/dd HH:mm"), Text = string.Format("{0:D2}:{1:D2}", m.Hour, m.Minute) }), "HoraInicio", "Text", DateTime.Now);
                viewModel.HorariosFim = new SelectList(FuncoesGlobais.Horarios().Select(m => new { HoraFim = teste.ToString().ToUpper() == "PT-BR" ? m.ToString("dd/MM/yyyy HH:mm") : teste.ToString().ToUpper() == "EN" ? m.ToString("MM/dd/yyyy HH:mm") : m.ToString("yyyy/MM/dd HH:mm"), Text = string.Format("{0:D2}:{1:D2}", m.Hour, m.Minute) }), "HoraFim", "Text", DateTime.Now);
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/AgendamentoConsultaMedicoDisponibilidades/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var input = new ListarAgendamentoConsultaMedicoDisponibilidadesInput();
            input.Filtro = term;
            input.MaxResultCount = 50;
            input.Sorting = "Descricao";
            var query = await _agendamentoConsultaMedicoDisponibilidadeAppService.Listar(input);
            var result = query.Items.Select(m => new { m.Id, m.HoraFim }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}