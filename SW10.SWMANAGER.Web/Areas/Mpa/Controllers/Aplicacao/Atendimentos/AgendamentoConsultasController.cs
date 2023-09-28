using Abp.Collections.Extensions;
using Abp.Extensions;
using Abp.Dependency;
using MoreLinq;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class AgendamentoConsultasController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public async Task<ActionResult> Index()
        {
            using (var agendamentoConsultaMedicoDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaMedicoDisponibilidadeAppService>())
            using (var especialidadeAppService = IocManager.Instance.ResolveAsDisposable<IEspecialidadeAppService>())
            {
                var agendamentos = await agendamentoConsultaMedicoDisponibilidadeAppService.Object.ListarAtivos(0, 0).ConfigureAwait(false);
                var especialidadesAgendadas = agendamentos.Select(m => m.MedicoEspecialidade.EspecialidadeId).Distinct().ToList();
                var especialidades = await especialidadeAppService.Object.Listar(especialidadesAgendadas).ConfigureAwait(false);

                var viewModel = new AgendamentoConsultasViewModel
                {
                    Especialidades = new SelectList(especialidades.Items, "Id", "Nome"),
                    IsConsulta = true
                };

                return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/Index.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> _CriarModal(DateTime date, long? medicoId)
        {
            using (var medicoAppService = IocManager.Instance.ResolveAsDisposable<IMedicoAppService>())
            {
                var model = new CriarOuEditarAgendamentoConsulta();
                //var pacientes = await _pacienteAppService.ListarTodos();
                //var convenios = await _convenioAppService.ListarTodos();
                //var planos = await _planoAppService.ListarTodos();
                var viewModel = new CriarOuEditarAgendamentoConsultaModal(model);

                if (medicoId != null)
                {
                    var medico = await medicoAppService.Object.Obter((long)medicoId).ConfigureAwait(false);

                    if (medico != null)
                    {
                        viewModel.MedicoId = medico.Id;
                        viewModel.Medico = medico;
                    }
                }

                viewModel.DataAgendamento = date;
                if (!date.ToString("HH:mm").Equals("00:00"))
                {
                    viewModel.HoraAgendamento = date;
                }

                //viewModel.Pacientes = new SelectList(pacientes.Items, "Id", "NomeCompleto", model.PacienteId);
                //viewModel.Convenios = new SelectList(convenios.Items, "Id", "NomeFantasia");
                //viewModel.Planos = new SelectList(planos.Items, "Id", "Descricao");

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> _EditarModal(long id)
        {
            using (var agendamentoConsultaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaAppService>())
            {
                var model = await agendamentoConsultaAppService.Object.Obter(id).ConfigureAwait(false);
                //var pacientes = await _pacienteAppService.ListarTodos();
                //var convenios = await _convenioAppService.ListarTodos();
                //var planos = await _planoAppService.ListarTodos();
                var viewModel = new CriarOuEditarAgendamentoConsultaModal(model);
                //viewModel.Pacientes = new SelectList(pacientes.Items, "Id", "NomeCompleto", model.PacienteId);
                //viewModel.Convenios = new SelectList(convenios.Items, "Id", "NomeFantasia");
                //viewModel.Planos = new SelectList(planos.Items, "Id", "Descricao");

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> _ListarMedicoDisponibilidades(long? medicoId, long? especialidadeId)
        {
            using (var agendamentoConsultaMedicoDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaMedicoDisponibilidadeAppService>())
            {
                var agendamentos = await agendamentoConsultaMedicoDisponibilidadeAppService.Object.ListarAtivos(medicoId, especialidadeId).ConfigureAwait(false);
                var viewModel = new List<ListarMedicoEspecialidadesViewModel>();
                if (agendamentos.IsNullOrEmpty())
                {
                    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_ListarMedicoDisponibilidades.cshtml", viewModel);
                }
                var medicos = agendamentos.Where(m => m.Medico != null).Select(m => m.Medico).DistinctBy(m => m.Id).ToList();

                foreach (var _medicoId in medicos)
                {
                    var _viewModel = new ListarMedicoEspecialidadesViewModel();
                    var diasSemana = string.Empty;
                    var listAgendamentos = new List<AgendamentoViewModel>();
                    _viewModel.Medico = _medicoId; //agendamentosFiltro.Where(m => m.MedicoId == _medicoId).FirstOrDefault().Medico;

                    var medicoEspecialidades = agendamentos.Where(m => m.MedicoId == _medicoId.Id).Select(m => m.MedicoEspecialidade).DistinctBy(m => m.Id).ToList(); // medicosEspecialidades.Select(m=>m.esp.Where(m=>m. //agendamentosFiltro.Where(m => m.MedicoId == _medicoId).Select(m => m.MedicoEspecialidade.EspecialidadeId).Distinct();
                    foreach (var _medicoEspecialidade in medicoEspecialidades)
                    {
                        var _agendamento = new AgendamentoViewModel();
                        var _medicoEspecialidadeId = _medicoEspecialidade.Id;  //_viewModel.Medico.MedicoEspecialidades.Where(m => m.EspecialidadeId == _medicoEspecialidade.Id).FirstOrDefault().Id;
                        var _agendamentoFiltro = agendamentos.Where(m => m.MedicoId == _medicoId.Id && m.MedicoEspecialidadeId == _medicoEspecialidadeId).FirstOrDefault();
                        _agendamento.Especialidade = _agendamentoFiltro.MedicoEspecialidade.Especialidade;
                        _agendamento.IntervaloMinutos = _agendamentoFiltro.Intervalo.IntervaloMinutos;
                        _agendamento.Horarios = _agendamentoFiltro.HoraInicio.ToString("HH:mm") + " - " + _agendamentoFiltro.HoraFim.ToString("HH:mm");
                        if (_agendamentoFiltro.Domingo)
                        {
                            diasSemana += diasSemana.IndexOf(L("Dom")) == -1 ? L("Dom") + ", " : string.Empty;
                            _agendamento.DiasSemana += L("Dom");
                        }
                        if (_agendamentoFiltro.Segunda)
                        {
                            diasSemana += diasSemana.IndexOf(L("Seg")) == -1 ? L("Seg") + ", " : string.Empty;
                            _agendamento.DiasSemana += L("Seg");
                        }
                        if (_agendamentoFiltro.Terca)
                        {
                            diasSemana += diasSemana.IndexOf(L("Ter")) == -1 ? L("Ter") + ", " : string.Empty;
                            _agendamento.DiasSemana += L("Ter");
                        }
                        if (_agendamentoFiltro.Quarta)
                        {
                            diasSemana += diasSemana.IndexOf(L("Qua")) == -1 ? L("Qua") + ", " : string.Empty;
                            _agendamento.DiasSemana += L("Qua");
                        }
                        if (_agendamentoFiltro.Quinta)
                        {
                            diasSemana += diasSemana.IndexOf(L("Qui")) == -1 ? L("Qui") + ", " : string.Empty;
                            _agendamento.DiasSemana += L("Qui");
                        }
                        if (_agendamentoFiltro.Sexta)
                        {
                            diasSemana += diasSemana.IndexOf(L("Sex")) == -1 ? L("Sex") + ", " : string.Empty;
                            _agendamento.DiasSemana += L("Sex");
                        }
                        if (_agendamentoFiltro.Sabado)
                        {
                            diasSemana += diasSemana.IndexOf(L("Sab")) == -1 ? L("Sab") + ", " : string.Empty;
                            _agendamento.DiasSemana += L("Sab");
                        }
                        _agendamento.DiasSemana = _agendamento.DiasSemana.Substring(0, _agendamento.DiasSemana.Length - 2);

                        listAgendamentos.Add(_agendamento);
                    }
                    _viewModel.Agendamentos = listAgendamentos;
                    diasSemana = diasSemana.Substring(0, diasSemana.Length - 2);
                    _viewModel.DiasSemana = diasSemana;
                    viewModel.Add(_viewModel);
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_ListarMedicoDisponibilidades.cshtml", viewModel);
            }
        }

        public async Task<JsonResult> EventosPorMedico(DateTime start, DateTime end, long? medicoId, long? medicoEspecialidadeId)
        {
            using (var agendamentoConsultaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaAppService>())
            {
                var eventos = await agendamentoConsultaAppService.Object.ListarPorMedico(medicoId, medicoEspecialidadeId, start, end, EnumTipoAgendamento.Consulta).ConfigureAwait(false);
                var eventosViewModel = new List<EventosViewModel>();
                foreach (var item in eventos)
                {
                    var horaAgendamento = new DateTime(item.DataAgendamento.Year, item.DataAgendamento.Month, item.DataAgendamento.Day, item.HoraAgendamento.Hour, item.HoraAgendamento.Minute, item.HoraAgendamento.Second);
                    var eventoViewModel = new EventosViewModel();
                    eventoViewModel.id = item.Id;
                    eventoViewModel.title = string.Format("{0}", item.Paciente != null ? item.Paciente.NomeCompleto : item.NomeReservante != null ? item.NomeReservante : string.Empty);
                    eventoViewModel.start = horaAgendamento;
                    eventoViewModel.end = horaAgendamento.AddMinutes((item.AgendamentoConsultaMedicoDisponibilidade.Intervalo.IntervaloMinutos * (item.QuantidadeHorarios != 0 ? item.QuantidadeHorarios : 1)));
                    eventoViewModel.allDay = false;
                    eventoViewModel.color = item.Medico.CorAgendamentoConsulta; //agendamento.CorAgendamento;
                    eventosViewModel.Add(eventoViewModel);
                }
                return Json(eventosViewModel.ToArray(), "application/json; charset=utf-8", JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<PartialViewResult> _MontarComboMedicos(DateTime date, long? medicoId)
        {
            //var dayOfWeek = (int)date.DayOfWeek;
            using (var agendamentoConsultaMedicoDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaMedicoDisponibilidadeAppService>())
            {
                var agendamentos = await agendamentoConsultaMedicoDisponibilidadeAppService.Object.ListarPorData(date).ConfigureAwait(false);
                var medicos = agendamentos.Select(m => m.Medico).DistinctBy(m => m.Id).ToList();
                var viewModel = new MontarComboMedicosViewModel();
                viewModel.Medicos = new SelectList(medicos, "Id", "NomeCompleto", medicoId.HasValue ? medicoId.ToString() : medicos.Count() == 1 ? medicos.FirstOrDefault().Id.ToString() : "");
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_MontarComboMedicos.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> _MontarComboMedicoEspecialidades(DateTime date, long? medicoId)
        {
            using (var agendamentoConsultaMedicoDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaMedicoDisponibilidadeAppService>())
            {
                var agendamentos = await agendamentoConsultaMedicoDisponibilidadeAppService.Object.ListarPorData(date).ConfigureAwait(false);

                var medicosEspecialidades = agendamentos
                    .WhereIf(medicoId.HasValue, m => m.MedicoId == medicoId)
                    .Select(m => m.MedicoEspecialidade)
                    .DistinctBy(m => m.EspecialidadeId)
                    .ToList();

                var especialidades = medicosEspecialidades
                    .Select(m => m.Especialidade)
                    .DistinctBy(m => m.Id)
                    .ToList();

                string medicoEspecialidadeId = string.Empty;
                if (medicosEspecialidades.Count() == 1)
                {
                    medicoEspecialidadeId = medicosEspecialidades.FirstOrDefault().Id.ToString();
                }
                var viewModel = new MontarComboMedicoEspecialidadesViewModel();
                viewModel.MedicoEspecialidades = new SelectList(medicosEspecialidades.Select(m => new { Id = m.Id, Nome = especialidades.Single(e => e.Id == m.EspecialidadeId).Nome }), "Id", "Nome", medicoEspecialidadeId);

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_MontarComboMedicoEspecialidades.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> _MontarComboHorarios(DateTime date, long medicoId, long? medicoEspecialidadeId, long id = 0, DateTime? dataHora = null)
        {
            using (var agendamentoConsultaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaAppService>())
            using (var agendamentoConsultaMedicoDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaMedicoDisponibilidadeAppService>())
            {
                var dayOfWeek = (int)date.DayOfWeek;
                var start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                var end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
                var agendamentos = await agendamentoConsultaMedicoDisponibilidadeAppService.Object.ListarPorData(date).ConfigureAwait(false);
                var marcacoes = await agendamentoConsultaAppService.Object.ListarPorData(start, end).ConfigureAwait(false);
                var agendamentosData =
                    agendamentos
                    .Where(m => m.MedicoId == medicoId
                    && m.MedicoEspecialidadeId == medicoEspecialidadeId)
                    .DistinctBy(m => m.Id);
                var horarios = new List<SelectListItem>();

                string Horario;
                foreach (var item in agendamentosData)
                {
                    var _marcacoes = marcacoes.Where(m => m.MedicoId == item.MedicoId && m.MedicoEspecialidadeId == item.MedicoEspecialidadeId).ToList();
                    var horariosMarcados = _marcacoes.Select(m => m.HoraAgendamento).ToList();
                    bool disponivel = false;
                    var horaIni = new DateTime(date.Year, date.Month, date.Day, item.HoraInicio.Hour, item.HoraInicio.Minute, item.HoraInicio.Second);
                    var horaFim = new DateTime(date.Year, date.Month, date.Day, item.HoraFim.Hour, item.HoraFim.Minute, item.HoraFim.Second);
                    var intervaloMinutos = item.Intervalo.IntervaloMinutos;
                    var horaLoop = horaIni;
                    var agendamentoAtual = false;

                    int horarioComplementar = 0;

                    while (horaLoop <= horaFim)
                    {
                        if (horarioComplementar == 0)
                        {
                            if (horaLoop < horaFim)
                            {
                                if (id > 0 && !agendamentoAtual)
                                {
                                    var agendamento = _marcacoes
                                        .SingleOrDefault(m => m.Id == id && m.HoraAgendamento == horaLoop);
                                    if (agendamento != null)
                                    {
                                        disponivel = true;
                                        agendamentoAtual = true;
                                        horarioComplementar = agendamento.QuantidadeHorarios != 0 ? agendamento.QuantidadeHorarios - 1 : 0;
                                    }
                                    else
                                    {
                                        disponivel = !horaLoop.IsIn(horariosMarcados.ToArray()); //await _agendamentoConsultaAppService.ChecarDisponibilidade(item.Id, horaLoop, id);
                                    }
                                }
                                else
                                {
                                    disponivel = !horaLoop.IsIn(horariosMarcados.ToArray()); //await _agendamentoConsultaAppService.ChecarDisponibilidade(item.Id, horaLoop, id);

                                    var agendamento = _marcacoes
                                       .SingleOrDefault(m => m.HoraAgendamento == horaLoop);
                                    if (agendamento != null)
                                    {
                                        horarioComplementar = agendamento.QuantidadeHorarios != 0 ? agendamento.QuantidadeHorarios - 1 : 0;
                                    }
                                }

                                if (disponivel)
                                {
                                    horarios.Add(new SelectListItem
                                    {
                                        Value = item.Id.ToString(),
                                        Text = horaLoop.ToString("HH:mm"),
                                        Selected = horaLoop.Equals(dataHora)
                                    });


                                }
                            }
                        }
                        else
                        {
                            horarioComplementar--;
                        }

                        horaLoop = horaLoop.AddMinutes(intervaloMinutos);
                    }
                }
                var viewModel = new MontarComboHorariosViewModel();

                viewModel.Horario = string.Format("{0:HH:mm}", dataHora);

                viewModel.Horarios = new SelectList(horarios, "Value", "Text");
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_MontarComboHorarios.cshtml", viewModel);
            }
        }



        public async Task<PartialViewResult> _MontarComboQuantidadeHorarios(DateTime date, long medicoId, long? medicoEspecialidadeId, long id = 0, DateTime? dataHora = null, int quantidadeHorarios = 0)
        {
            using (var agendamentoConsultaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaAppService>())
            using (var agendamentoConsultaMedicoDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaMedicoDisponibilidadeAppService>())
            {
                var quantidesHorarios = new List<SelectListItem>();
                var viewModel = new MontarComboQuantidadeHorariosViewModel();

                //var horaLoop = dataHora;
                var start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                var end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

                var agendamento = (await agendamentoConsultaMedicoDisponibilidadeAppService.Object.ListarPorDataMedicoEspecialidade(date, medicoId, medicoEspecialidadeId ?? 0).ConfigureAwait(false)).FirstOrDefault();
                var marcacoes = await agendamentoConsultaAppService.Object.ListarPorDataMedicoEspecialidade(start, end, medicoId, medicoEspecialidadeId ?? 0).ConfigureAwait(false);
                int qtd = 1;

                if (agendamento == null)
                {
                    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_MontarComboQuantidadeHorarios.cshtml", viewModel);
                }

                var horaIni = new DateTime(date.Year, date.Month, date.Day, agendamento.HoraInicio.Hour, agendamento.HoraInicio.Minute, agendamento.HoraInicio.Second);
                var horaFim = new DateTime(date.Year, date.Month, date.Day, agendamento.HoraFim.Hour, agendamento.HoraFim.Minute, agendamento.HoraFim.Second);
                var intervaloMinutos = agendamento.Intervalo.IntervaloMinutos;
                //var horaLoop = horaIni;

                DateTime horaLoop = dataHora ?? horaFim;


                while (horaLoop <= horaFim)
                {
                    var ishora = marcacoes.ToList().Where(w => w.Id != id && w.HoraAgendamento == horaLoop).FirstOrDefault();

                    if (ishora != null)
                    {
                        break;
                    }

                    if (horaLoop < horaFim)
                    {
                        quantidesHorarios.Add(new SelectListItem { Text = qtd.ToString(), Value = qtd.ToString() });
                        qtd++;
                    }

                    horaLoop = horaLoop.AddMinutes(intervaloMinutos);
                }

                viewModel.QuantidadeHorarios = new SelectList(quantidesHorarios, "Value", "Text");
                viewModel.QuantidadeHorario = quantidadeHorarios != 0 ? quantidadeHorarios.ToString() : "1";

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_MontarComboQuantidadeHorarios.cshtml", viewModel);
            }
        }
    }
}