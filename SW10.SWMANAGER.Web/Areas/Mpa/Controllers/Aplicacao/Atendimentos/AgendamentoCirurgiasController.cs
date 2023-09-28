using Abp.Dependency;
using Abp.Extensions;
using Microsoft.Reporting.WebForms;
using MoreLinq;
using Newtonsoft.Json;

using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentosSalaCirurgicas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.SalasCirurgicas.Servicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.TiposCirurgias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoSalasCirurgicasDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoSalasCirurgicasDisponibilidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoCirurgias;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Relatorios;
using SW10.SWMANAGER.Web.Controllers;
using SW10.SWMANAGER.Web.Relatorios.Atendimento.Agendamento;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class AgendamentoCirurgiasController : SWMANAGERControllerBase
    {

        public async Task<ActionResult> Index()
        {
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var agendamentoSalaCirurgicaDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaDisponibilidadeAppService>())
            {
                var agendamentos = await agendamentoSalaCirurgicaDisponibilidadeAppService.Object.ListarAtivos(null, null, null).ConfigureAwait(false);

                var viewModel = new AgendamentoConsultasViewModel();

                var userId = this.AbpSession.UserId;
                var userEmpresas = await userAppService.Object.GetUserEmpresas(userId.Value).ConfigureAwait(false);

                if (userEmpresas != null && userEmpresas.Items != null && userEmpresas.Items.Count == 1)
                {
                    viewModel.Empresa = userEmpresas.Items[0];
                    viewModel.EmpresaId = userEmpresas.Items[0].Id;
                }

                return this.View(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/Index.cshtml",
                    viewModel);
            }
        }

        public async Task<ActionResult> IndexListagem()
        {
            var viewModel = new FiltroModel();

            return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/IndexListagemAgendamento.cshtml", viewModel);
        }

        public async Task<PartialViewResult> _ListarSalasCirurgicas(long? salaCirurgicaId, long? tipoCirurgiaId, long? empresaId)
        {
            var viewModel = new List<ListarSalasCirurgicasViewModel>();
            using (var agendamentoSalaCirurgicaDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaDisponibilidadeAppService>())
            {
                var disponibilidades = await agendamentoSalaCirurgicaDisponibilidadeAppService.Object
                                           .ListarAtivos(salaCirurgicaId, tipoCirurgiaId, empresaId)
                                           .ConfigureAwait(false);

                if (disponibilidades != null)
                {
                    var salas = disponibilidades.Select(s => s.SalaCirurgica).DistinctBy(d => d.Id).ToList()
                        .OrderBy(o => o.Descricao);

                    foreach (var item in salas)
                    {
                        var listarSalasCirurgicasViewModel = new ListarSalasCirurgicasViewModel();

                        var diasSemana = string.Empty;
                        var listAgendamentos = new List<AgendamentoViewModel>();
                        listarSalasCirurgicasViewModel.SalaCirurgicaId =
                            salaCirurgicaId; //agendamentosFiltro.Where(m => m.MedicoId == _medicoId).FirstOrDefault().Medico;
                        listarSalasCirurgicasViewModel.SalaCirurgica = item;

                        var tipoCirurgias = disponibilidades.Where(m => m.SalaCirurgicaId == item.Id)
                            .Select(m => m.TipoCirurgia).DistinctBy(m => m.Id).ToList();

                        foreach (var tipoCirurgia in tipoCirurgias)
                        {
                            var _agendamento = new AgendamentoViewModel();
                            var _tipoCirurgiaId =
                                tipoCirurgia
                                    .Id; //_viewModel.Medico.MedicoEspecialidades.Where(m => m.EspecialidadeId == _medicoEspecialidade.Id).FirstOrDefault().Id;
                            var _agendamentoFiltro = disponibilidades.FirstOrDefault(
                                m => m.SalaCirurgicaId == item.Id && m.TipoCirurgiaId == _tipoCirurgiaId);
                            _agendamento.TipoCirurgia = _agendamentoFiltro.TipoCirurgia;
                            _agendamento.IntervaloMinutos = _agendamentoFiltro.Intervalo.IntervaloMinutos;
                            _agendamento.Horarios = _agendamentoFiltro.HoraInicio.ToString("HH:mm") + " - "
                                                                                                    + _agendamentoFiltro
                                                                                                        .HoraFim
                                                                                                        .ToString(
                                                                                                            "HH:mm");
                            if (_agendamentoFiltro.Domingo)
                            {
                                diasSemana += diasSemana.IndexOf(this.L("Dom")) == -1
                                                  ? this.L("Dom") + ", "
                                                  : string.Empty;
                                _agendamento.DiasSemana += this.L("Dom");
                            }

                            if (_agendamentoFiltro.Segunda)
                            {
                                diasSemana += diasSemana.IndexOf(this.L("Seg")) == -1
                                                  ? this.L("Seg") + ", "
                                                  : string.Empty;
                                _agendamento.DiasSemana += this.L("Seg");
                            }

                            if (_agendamentoFiltro.Terca)
                            {
                                diasSemana += diasSemana.IndexOf(this.L("Ter")) == -1
                                                  ? this.L("Ter") + ", "
                                                  : string.Empty;
                                _agendamento.DiasSemana += this.L("Ter");
                            }

                            if (_agendamentoFiltro.Quarta)
                            {
                                diasSemana += diasSemana.IndexOf(this.L("Qua")) == -1
                                                  ? this.L("Qua") + ", "
                                                  : string.Empty;
                                _agendamento.DiasSemana += this.L("Qua");
                            }

                            if (_agendamentoFiltro.Quinta)
                            {
                                diasSemana += diasSemana.IndexOf(this.L("Qui")) == -1
                                                  ? this.L("Qui") + ", "
                                                  : string.Empty;
                                _agendamento.DiasSemana += this.L("Qui");
                            }

                            if (_agendamentoFiltro.Sexta)
                            {
                                diasSemana += diasSemana.IndexOf(this.L("Sex")) == -1
                                                  ? this.L("Sex") + ", "
                                                  : string.Empty;
                                _agendamento.DiasSemana += this.L("Sex");
                            }

                            if (_agendamentoFiltro.Sabado)
                            {
                                diasSemana += diasSemana.IndexOf(this.L("Sab")) == -1
                                                  ? this.L("Sab") + ", "
                                                  : string.Empty;
                                _agendamento.DiasSemana += this.L("Sab");
                            }
                            //  _agendamento.DiasSemana = _agendamento.DiasSemana.Substring(0, _agendamento.DiasSemana.Length - 2);

                            listAgendamentos.Add(_agendamento);
                        }

                        listarSalasCirurgicasViewModel.Agendamentos = listAgendamentos;
                        diasSemana = diasSemana.Substring(0, diasSemana.Length - 2);
                        listarSalasCirurgicasViewModel.DiasSemana = diasSemana;
                        viewModel.Add(listarSalasCirurgicasViewModel);
                    }
                }

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/_ListarSalasCirurgicas.cshtml",
                    viewModel);
            }
        }

        public async Task<PartialViewResult> _CriarModal(DateTime date, long? salaCirurgicaId, long? tipoCirurgiaId)
        {
            var viewModel = new AgendamentoCirurgiasViewModel
            {
                CirurgiasJson =
                    JsonConvert.SerializeObject(new List<GenericoIdNome>()),
                MateriaisOPMEJson =
                    JsonConvert.SerializeObject(
                        new List<AgendamentoMaterialOPMEJson>()),
                MateriaisJson =
                    JsonConvert.SerializeObject(new List<AgendamentoMaterialJson>())
            };

            try
            {
                var model = new AgendamentoCirurgiasViewModel();
                //var pacientes = await _pacienteAppService.ListarTodos();
                //var convenios = await _convenioAppService.ListarTodos();
                //var planos = await _planoAppService.ListarTodos();
                //  var viewModel = new AgendamentoCirurgiasViewModel();

                if (salaCirurgicaId != null)
                {
                    using (var salaCirurgicaAppService = IocManager.Instance.ResolveAsDisposable<ISalaCirurgicaAppService>())
                    {
                        var sala = await salaCirurgicaAppService.Object.Obter((long)salaCirurgicaId).ConfigureAwait(false);

                        if (sala != null)
                        {
                            viewModel.AgendamentoSalaCirurgicaDisponibilidade = new AgendamentoSalaCirurgicaDisponibilidadeDto { SalaCirurgicaId = sala.Id, SalaCirurgica = sala };
                        }
                    }
                }

                if (tipoCirurgiaId != null && salaCirurgicaId != null)
                {
                    using (var tipoCirurgiaAppService = IocManager.Instance.ResolveAsDisposable<ITipoCirurgiaAppService>())
                    {
                        var tipoCirurgia =
                            await tipoCirurgiaAppService.Object.Obter((long)tipoCirurgiaId).ConfigureAwait(false);

                        if (tipoCirurgia != null)
                        {
                            if (viewModel.AgendamentoSalaCirurgicaDisponibilidade == null)
                            {
                                viewModel.AgendamentoSalaCirurgicaDisponibilidade =
                                    new AgendamentoSalaCirurgicaDisponibilidadeDto();
                            }

                            viewModel.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgiaId = tipoCirurgia.Id;
                            viewModel.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgia = tipoCirurgia;
                        }
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

                viewModel.AgendamentoStatus = new AgendamentoStatusDto { Id = 1, Descricao = "Inicial", Codigo = "In" };
                viewModel.StatusId = 1;//Inicial

            }
            catch (Exception ex)
            {

            }
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> EventosPorSala(DateTime start, DateTime end, long? salaId, long? tipoCirurgiaId, long? empresaId, long? medicoId)
        {
            using (var agendamentoSalaCirurgicaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaAppService>())
            {
                var eventos = await agendamentoSalaCirurgicaAppService.Object.ListarPorSala(salaId, tipoCirurgiaId, start, end, empresaId, medicoId).ConfigureAwait(false);
                var eventosViewModel = new List<EventosViewModel>();

                foreach (var item in eventos)
                {
                    var horaAgendamento = new DateTime(
                        item.AgendamentoConsulta.DataAgendamento.Year,
                        item.AgendamentoConsulta.DataAgendamento.Month,
                        item.AgendamentoConsulta.DataAgendamento.Day,
                        item.AgendamentoConsulta.HoraAgendamento.Hour,
                        item.AgendamentoConsulta.HoraAgendamento.Minute,
                        item.AgendamentoConsulta.HoraAgendamento.Second);
                    var eventoViewModel = new EventosViewModel
                    {
                        id = item.AgendamentoConsulta.Id,
                        title =
                                                      $"({item.AgendamentoConsulta.AgendamentoStatus?.Codigo}) - {(item.AgendamentoConsulta.Paciente != null ? item.AgendamentoConsulta.Paciente.NomeCompleto : item.AgendamentoConsulta.NomeReservante != null ? item.AgendamentoConsulta.NomeReservante : string.Empty)}",
                        start = horaAgendamento,
                        end = horaAgendamento.AddMinutes(
                                                      item.AgendamentoSalaCirurgicaDisponibilidade.Intervalo
                                                          .IntervaloMinutos
                                                      * (item.AgendamentoConsulta.QuantidadeHorarios != 0
                                                             ? item.AgendamentoConsulta.QuantidadeHorarios
                                                             : 1)),
                        allDay = false,
                        color = item.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgica
                                                      .CorAgendamento
                    };
                    //agendamento.CorAgendamento;
                    eventosViewModel.Add(eventoViewModel);
                }

                return this.Json(
                    eventosViewModel.ToArray(),
                    "application/json; charset=utf-8",
                    JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<PartialViewResult> _MontarComboHorarios(DateTime date, long salaId, long? tipoCirurgiaId, long id = 0, DateTime? dataHora = null)
        {
            using (var agendamentoSalaCirurgicaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaAppService>())
            using (var agendamentoSalaCirurgicaDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaDisponibilidadeAppService>())
            {
                var dayOfWeek = (int)date.DayOfWeek;
                var start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                var end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

                var agendamentos = await agendamentoSalaCirurgicaDisponibilidadeAppService.Object.ListarPorData(date).ConfigureAwait(false);
                var marcacoes = await agendamentoSalaCirurgicaAppService.Object.ListarPorData(start, end).ConfigureAwait(false);
                var agendamentosData = agendamentos.Where(m => m.SalaCirurgicaId == salaId && m.TipoCirurgiaId == tipoCirurgiaId).DistinctBy(m => m.Id);
                var horarios = new List<SelectListItem>();

                string Horario;

                foreach (var item in agendamentosData)
                {
                    var _marcacoes = marcacoes.Where(
                        m => m.AgendamentoSalaCirurgicaDisponibilidade?.SalaCirurgicaId == item.SalaCirurgicaId
                             && m.AgendamentoSalaCirurgicaDisponibilidade?.TipoCirurgiaId == item.TipoCirurgiaId
                             && m.AgendamentoConsulta.StatusId != 4 // não considera agendamento cancelado
                    ).ToList();
                    var horariosMarcados = _marcacoes.Select(m => m.AgendamentoConsulta.HoraAgendamento).ToList();
                    var disponivel = false;
                    var horaIni = new DateTime(date.Year, date.Month, date.Day, item.HoraInicio.Hour, item.HoraInicio.Minute, item.HoraInicio.Second);
                    var horaFim = new DateTime(date.Year, date.Month, date.Day, item.HoraFim.Hour, item.HoraFim.Minute, item.HoraFim.Second);
                    var intervaloMinutos = item.Intervalo.IntervaloMinutos;
                    var horaLoop = horaIni;
                    var agendamentoAtual = false;

                    var horarioComplementar = 0;

                    while (horaLoop <= horaFim)
                    {
                        if (horarioComplementar == 0)
                        {
                            if (horaLoop < horaFim)
                            {
                                if (id > 0 && !agendamentoAtual)
                                {
                                    var agendamento = _marcacoes.SingleOrDefault(m => m.AgendamentoConsulta.Id == id && m.AgendamentoConsulta.HoraAgendamento == horaLoop);
                                    if (agendamento != null)
                                    {
                                        disponivel = true;
                                        agendamentoAtual = true;
                                        horarioComplementar = agendamento.AgendamentoConsulta.QuantidadeHorarios != 0 ? agendamento.AgendamentoConsulta.QuantidadeHorarios - 1 : 0;
                                    }
                                    else
                                    {
                                        disponivel = !horaLoop.IsIn(horariosMarcados.ToArray()); //await _agendamentoConsultaAppService.ChecarDisponibilidade(item.Id, horaLoop, id);
                                    }
                                }
                                else
                                {
                                    disponivel = !horaLoop.IsIn(horariosMarcados.ToArray()); //await _agendamentoConsultaAppService.ChecarDisponibilidade(item.Id, horaLoop, id);

                                    var agendamento = _marcacoes.FirstOrDefault(m => m.AgendamentoConsulta.HoraAgendamento == horaLoop);
                                    if (agendamento != null)
                                    {
                                        horarioComplementar = agendamento.AgendamentoConsulta.QuantidadeHorarios != 0 ? agendamento.AgendamentoConsulta.QuantidadeHorarios - 1 : 0;
                                    }
                                }

                                if (disponivel)
                                {
                                    horarios.Add(new SelectListItem { Value = item.Id.ToString(), Text = horaLoop.ToString("HH:mm"), Selected = horaLoop.Equals(dataHora) });
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

                var viewModel = new MontarComboHorariosViewModel
                {
                    Horario = string.Format("{0:HH:mm}", dataHora),

                    Horarios = new SelectList(horarios, "Value", "Text")
                };
                return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_MontarComboHorarios.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> _MontarComboQuantidadeHorarios(DateTime date, long salaId, long? tipoCirurgiaId, long id = 0, DateTime? dataHora = null, int quantidadeHorarios = 0)
        {
            var quantidesHorarios = new List<SelectListItem>();

            //var horaLoop = dataHora;
            var start = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            var end = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            using (var agendamentoSalaCirurgicaDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaDisponibilidadeAppService>())
            using (var agendamentoConsultaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaAppService>())
            {
                var agendamento =
                    (await agendamentoSalaCirurgicaDisponibilidadeAppService.Object.ListarPorSalaTipoCirurgia(date, salaId, tipoCirurgiaId ?? 0).ConfigureAwait(false))
                    .FirstOrDefault();

                var marcacoes = await agendamentoConsultaAppService.Object.ListarPorDataSalaTipoCirurgia(start, end, salaId, tipoCirurgiaId ?? 0)
                                    .ConfigureAwait(false);
                var qtd = 1;

                var horaIni = new DateTime(
                    date.Year,
                    date.Month,
                    date.Day,
                    agendamento.HoraInicio.Hour,
                    agendamento.HoraInicio.Minute,
                    agendamento.HoraInicio.Second);
                var horaFim = new DateTime(
                    date.Year,
                    date.Month,
                    date.Day,
                    agendamento.HoraFim.Hour,
                    agendamento.HoraFim.Minute,
                    agendamento.HoraFim.Second);
                var intervaloMinutos = agendamento.Intervalo.IntervaloMinutos;
                //var horaLoop = horaIni;

                var horaLoop = dataHora ?? horaFim;


                while (horaLoop <= horaFim)
                {
                    var ishora = marcacoes.ToList()
                        .FirstOrDefault(w => w.Id != id && w.HoraAgendamento == horaLoop);

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

                var viewModel = new MontarComboQuantidadeHorariosViewModel();

                viewModel.QuantidadeHorarios = new SelectList(quantidesHorarios, "Value", "Text");
                viewModel.QuantidadeHorario = quantidadeHorarios != 0 ? quantidadeHorarios.ToString() : "1";

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoConsultas/_MontarComboQuantidadeHorarios.cshtml",
                    viewModel);
            }
        }

        public async Task<PartialViewResult> _EditarModal(long id)
        {
            using (var _agendamentoSalaCirurgicaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaAppService>())
            {
                var model = await _agendamentoSalaCirurgicaAppService.Object.Obter(id).ConfigureAwait(false);
                //var pacientes = await _pacienteAppService.ListarTodos();
                //var convenios = await _convenioAppService.ListarTodos();
                //var planos = await _planoAppService.ListarTodos();
                var viewModel = new AgendamentoCirurgiasViewModel(model);

                var listCirurgias = new List<GenericoRelacionamento>();
                long idGrid = 0;
                foreach (var item in viewModel.Cirurgias.Where(w => w.IsCirurgica))
                {
                    listCirurgias.Add(
                        new GenericoRelacionamento
                        {
                            IdGrid = idGrid++,
                            Id = item.Id,
                            RelacionadoId = item.FaturamentoItemId,
                            Descricao = string.Concat(
                                    item.FaturamentoItem?.Codigo,
                                    " - ",
                                    item.FaturamentoItem?.Descricao,
                                    " - ",
                                    item.FaturamentoItem?.CodTuss)
                        });
                }

                viewModel.CirurgiasJson = JsonConvert.SerializeObject(listCirurgias);

                var listMateriaisOPME = new List<AgendamentoMaterialOPMEJson>();

                idGrid = 0;

                foreach (var item in model.MateriaisOPME)
                {
                    var material = new AgendamentoMaterialOPMEJson();

                    material.IdGrid = idGrid++;
                    material.Id = item.Id;
                    material.DataPrevista = item.DataPrevista;
                    material.DataRecebimento = item.DataRecebimento;
                    material.FornecedorId = item.FornecedorId;
                    material.FornecedorDescricao = item.Fornecedor?.NomeFantasia;
                    material.IsCobraPeloHospital = item.IsCobrarPeloHospital;
                    material.Material = item.Material;
                    material.NumeroNota = item.NumeroNotaFiscal;
                    material.QuantidadeMaterial = item.Quantidade;
                    material.ValorNota = item.ValorNotaFiscal;

                    listMateriaisOPME.Add(material);
                }

                viewModel.MateriaisOPMEJson = JsonConvert.SerializeObject(listMateriaisOPME);


                var listMaterial = new List<AgendamentoMaterialJson>();
                idGrid = 0;
                foreach (var item in viewModel.Cirurgias.Where(
                    w => (w.FaturamentoItem != null && w.FaturamentoItem.IsAgendaMaterial)))
                {
                    listMaterial.Add(
                        new AgendamentoMaterialJson
                        {
                            IdGrid = idGrid++,
                            Id = item.Id,
                            Descricao = item.FaturamentoItem.Descricao,
                            FaturamentoItemId = item.FaturamentoItemId,
                            QuantidadeMaterial = item.Quantidade
                        });
                }


                viewModel.MateriaisJson = JsonConvert.SerializeObject(listMaterial);


                viewModel.IsEditMode = true;

                //viewModel.Pacientes = new SelectList(pacientes.Items, "Id", "NomeCompleto", model.PacienteId);
                //viewModel.Convenios = new SelectList(convenios.Items, "Id", "NomeFantasia");
                //viewModel.Planos = new SelectList(planos.Items, "Id", "Descricao");

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/_CriarOuEditarModal.cshtml",
                    viewModel);
            }
        }


        //public async Task<PartialViewResult> ConfirmarAtendimento(long id)
        //{
        //    var agendamento = await _agendamentoConsultaAppService.Obter(id);
        //    if (agendamento != null)
        //    {
        //        var atendimentoId = await _atendimentoAppService.ObterAtendindimentoAbertoPaciente((long)agendamento.PacienteId);

        //        if(atendimentoId==null)
        //        {
        //            //Construir tela para inserir dados do atendimento

        //            //Confirmar com o Márcio
        //        }
        //        else
        //        {
        //          var retorno = await _agendamentoConsultaAppService.InserirItensFaturamentoAtendimentoExistente(agendamento.Id, (long)atendimentoId);
        //        }
        //    }
        //    return null;

        //}


        public async Task<ActionResult> IndexRelatorio()
        {
            var viewModel = new FiltroModel();


            return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/IndexRelatorioAgendamento.cshtml", viewModel);
        }

        public async Task<ActionResult> IndexRelatorioOrcamento(long? convenioId, long? planoId, long? disponibilidadeId, string listItemFaturamento, string listItemMateriais, long? pacienteId, string dataHoraAgendamento, string pacienteReservante, long agendamentoId)
        {
            var viewModel = new FiltroAgendamentoOrcamento
            {
                AgendamentoId = agendamentoId,
                ConvenioId = convenioId,
                PlanoId = planoId,
                DisponibilidadeId = disponibilidadeId,
                ListItemFaturamento = listItemFaturamento,
                ListItemMateriais = listItemMateriais,
                PacienteId = pacienteId,
                DataHoraAgendamento = dataHoraAgendamento,
                PacienteReservante = pacienteReservante
            };

            return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/IndexRelatorioOrcamento.cshtml", viewModel);
        }


        public async Task<ActionResult> ImprimirAgendamentoSalaCirurgiaPorDia(DateTime? dataIni, DateTime? dataFim, long? tipoCirurgiaId, long? medicoId)
        {
            try
            {
                var dataAtual = DateTime.Now;

                if (dataIni == null)
                {
                    dataIni = dataAtual;
                }

                if (dataFim == null)
                {
                    dataFim = dataAtual;
                }


                //dataIni = ((DateTime)dataIni).AddDays(-30);
                //dataFim = ((DateTime)dataFim).AddDays(30);

                using (var agendamentoSalaCirurgicaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaAppService>())
                using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
                using (var tipoCirurgiaAppService = IocManager.Instance.ResolveAsDisposable<ITipoCirurgiaAppService>())
                using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
                {
                    var agendamentoDia = await agendamentoSalaCirurgicaAppService.Object
                                             .ObterAgendamentosDia(dataIni, dataFim, tipoCirurgiaId, medicoId)
                                             .ConfigureAwait(false);



                    var relDS = new AgendamentosDia();

                    // Logotipo
                    var tabela = this.ConvertToDataTable(
                        agendamentoDia.AgendamentosDia /*dados.Contas*/,
                        relDS.Tables["Agendamento"]);
                    // DataRow row = tabela.NewRow();
                    // row["Logotipo"] = atendimento.Empresa.Logotipo;
                    // tabela.Rows.Add(row);
                    // fim - logotipo

                    var dataSource = new ReportDataSource("AgendamentoDia", tabela);
                    var agendamento = new ReportViewer();
                    agendamento.LocalReport.DataSources.Add(dataSource);

                    var scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(agendamento);

                    agendamento.LocalReport.ReportPath = string.Concat(
                        this.Server.MapPath("~"),
                        @"Relatorios\Atendimento\Agendamento\AgendamentoDia.rdlc");

                    var usuario = await userAppService.Object.GetUser().ConfigureAwait(false);


                    ReportParameter nomeHospital = null;

                    if (agendamentoDia.AgendamentosDia != null && agendamentoDia.AgendamentosDia.Count > 0)
                    {

                        var empresa = await empresaAppService.Object
                                          .Obter(agendamentoDia.AgendamentosDia[0].EmpresaId ?? 0)
                                          .ConfigureAwait(false);

                        if (empresa != null)
                        {
                            nomeHospital = new ReportParameter("NomeHospital", empresa.NomeFantasia);
                            var tabeResumo = this.ConvertToDataTable(
                                new List<string>() /*dados.Contas*/,
                                relDS.Tables["Geral"]);
                            var row = tabeResumo.NewRow();
                            row["Logotipo"] = empresa.Logotipo;
                            tabeResumo.Rows.Add(row);
                            // fim - logotipo

                            var dataSourceResumo = new ReportDataSource("Geral", tabeResumo);
                            // ReportViewer GuiaSpsadt = new ReportViewer();
                            agendamento.LocalReport.DataSources.Add(dataSourceResumo);
                        }
                    }
                    else
                    {
                        nomeHospital = new ReportParameter("NomeHospital", "");


                        var tabeResumo = this.ConvertToDataTable(
                            new List<string>() /*dados.Contas*/,
                            relDS.Tables["Geral"]);
                        var row = tabeResumo.NewRow();
                        row["Logotipo"] = null;
                        ;
                        tabeResumo.Rows.Add(row);
                        // fim - logotipo

                        var dataSourceResumo = new ReportDataSource("Geral", tabeResumo);
                        agendamento.LocalReport.DataSources.Add(dataSourceResumo);

                    }

                    var nomeUsuario = new ReportParameter("NomeUsuario", usuario?.Name);
                    var titulo = new ReportParameter("Titulo", "Mapa de Agendamento");
                    var dataHora = new ReportParameter(
                        "DataHora",
                        string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now));
                    var filtroData = new ReportParameter("FiltroData", string.Format("{0:dd/MM/yyyy}", dataIni));

                    ReportParameter tipoCirurgia = null;

                    if (tipoCirurgiaId != null)
                    {

                        var _tipoCirurgia = await tipoCirurgiaAppService.Object.Obter((long)tipoCirurgiaId)
                                                .ConfigureAwait(false);

                        if (_tipoCirurgia != null)
                        {
                            tipoCirurgia = new ReportParameter("TipoCirurgia", _tipoCirurgia.Descricao);
                        }
                        else
                        {
                            tipoCirurgia = new ReportParameter("TipoCirurgia", "Todas");
                        }
                    }
                    else
                    {
                        tipoCirurgia = new ReportParameter("TipoCirurgia", "Todas");
                    }

                    agendamento.LocalReport.SetParameters(
                        new ReportParameter[]
                            {
                                    nomeHospital, nomeUsuario, titulo, dataHora, filtroData, tipoCirurgia
                            });



                    Warning[] warnings;
                    string[] streamIds;
                    var mimeType = string.Empty;
                    var encoding = string.Empty;
                    var extension = "pdf";
                    var pdfBytes = agendamento.LocalReport.Render(
                        "PDF",
                        null,
                        out mimeType,
                        out encoding,
                        out extension,
                        out streamIds,
                        out warnings);

                    var absPath = string.Concat(this.Server.MapPath("/"), @"temp\");
                    var path = string.Empty;
                    var file = string.Empty;
                    var pathReturn = string.Empty;

                    file = string.Concat("RelatorioInternado-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                    path = string.Concat(absPath, file);
                    pathReturn = this.Url.Content("~/temp/" + file);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                    {
                        fs.Write(pdfBytes, 0, pdfBytes.Length);
                    }


                    agendamento.LocalReport.Refresh();

                    this.Response.Headers.Add(
                        "Content-Disposition",
                        string.Format(
                            "inline; filename=AtendimentoResumido-{0}.pdf",
                            DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));

                    return this.Content(pathReturn);

                }
            }
            catch (Exception ex)
            {

            }

            return null;


            //return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Ficha/_ModalFichaAmbulatorio.cshtml", viewModel);
        }

        class MaterialQuantidade
        {
            public long FaturamentoItemId { get; set; }
            public int QuantidadeMaterial { get; set; }
        }

        public async Task<ActionResult> ImprimirOrcamento(long? convenioId, long? planoId, long? disponibilidadeId, string listItemFaturamento, string listItemMateriais, long? pacienteId, string dataHoraAgendamento, string pacienteReservante)
        {

            try
            {
                var nomeConvenio = string.Empty;
                var nomePlano = string.Empty;
                var nomePaciente = string.Empty;


                if (convenioId != null)
                {

                    using (var convenioAppService = IocManager.Instance.ResolveAsDisposable<IConvenioAppService>())
                    {
                        var convenio = await convenioAppService.Object.Obter((long)convenioId).ConfigureAwait(false);

                        if (convenio != null)
                        {
                            nomeConvenio = convenio.NomeFantasia;
                        }
                    }
                }

                if (planoId != null)
                {
                    using (var planoAppService = IocManager.Instance.ResolveAsDisposable<IPlanoAppService>())
                    {
                        var plano = await planoAppService.Object.Obter((long)planoId).ConfigureAwait(false);


                        if (plano != null)
                        {
                            nomePlano = plano.Descricao;
                        }
                    }
                }

                if (pacienteId != null)
                {
                    using (var _pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
                    {
                        var paciente = await _pacienteAppService.Object.Obter((long)pacienteId).ConfigureAwait(false);

                        if (paciente != null)
                        {
                            nomePaciente = paciente.NomeCompleto;
                        }
                    }
                }
                else
                {
                    nomePaciente = pacienteReservante;
                }


                var orcamentos = new List<AgendamentoOrcamentoDto>();

                var itensId = JsonConvert.DeserializeObject<List<long>>(listItemFaturamento);

                var marteriasIds = JsonConvert.DeserializeObject<List<MaterialQuantidade>>(listItemMateriais);

                itensId.ForEach(f => marteriasIds.Add(new MaterialQuantidade { FaturamentoItemId = f, QuantidadeMaterial = 1 }));

                using (var agendamentoSalaCirurgicaDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaDisponibilidadeAppService>())
                using (var faturamentoItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemAppService>())
                using (var faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
                using (var _userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
                {
                    var disponibilidade = await agendamentoSalaCirurgicaDisponibilidadeAppService.Object.Obter(disponibilidadeId ?? 0).ConfigureAwait(false);
                    if (disponibilidade != null)
                    {

                        foreach (var item in marteriasIds)
                        {
                            var faturamentoItem = await faturamentoItemAppService.Object.Obter(item.FaturamentoItemId)
                                                      .ConfigureAwait(false);
                            if (faturamentoItem != null)
                            {
                                var faturamentoContaItemDto = new FaturamentoContaItemDto();
                                faturamentoContaItemDto.FaturamentoItem = faturamentoItem;
                                faturamentoContaItemDto.FaturamentoItemId = item.FaturamentoItemId;

                                var valor = await faturamentoContaItemAppService.Object.CalcularValorUnitarioItem(
                                                disponibilidade.EmpresaId ?? 0,
                                                convenioId ?? 0,
                                                planoId ?? 0,
                                                faturamentoContaItemDto).ConfigureAwait(false);

                                var orcamento = new AgendamentoOrcamentoDto();

                                orcamento.FaturamentoItem = faturamentoItem.Descricao;
                                orcamento.Qtd = item.QuantidadeMaterial.ToString();
                                orcamento.Valor = string.Format("{0:###,##0.00}", valor);
                                orcamento.ValorDec = (decimal)valor * item.QuantidadeMaterial;

                                orcamentos.Add(orcamento);
                            }
                        }
                    }




                    var relDS = new AgendamentoOrcamento();

                    var tabela = this.ConvertToDataTable(orcamentos /*dados.Contas*/, relDS.Tables["Orcamento"]);


                    var dataSource = new ReportDataSource("Orcamento", tabela);
                    using (var rvOrcamento = new ReportViewer())
                    {
                        rvOrcamento.LocalReport.DataSources.Add(dataSource);

                        var scriptManager = new ScriptManager();
                        scriptManager.RegisterPostBackControl(rvOrcamento);

                        rvOrcamento.LocalReport.ReportPath = string.Concat(
                            this.Server.MapPath("~"),
                            @"Relatorios\Atendimento\Agendamento\AgendamentoOrcamento.rdlc");


                        var usuario = Task.Run(() => _userAppService.Object.GetUser()).Result;

                        var relResumo = new AgendamentosDia();

                        var tabeResumo = this.ConvertToDataTable(
                            new List<string>() /*dados.Contas*/,
                            relResumo.Tables["Geral"]);
                        var row = tabeResumo.NewRow();
                        row["Logotipo"] = disponibilidade.Empresa.Logotipo;
                        tabeResumo.Rows.Add(row);
                        // fim - logotipo

                        var dataSourceResumo = new ReportDataSource("Geral", tabeResumo);
                        // ReportViewer GuiaSpsadt = new ReportViewer();
                        rvOrcamento.LocalReport.DataSources.Add(dataSourceResumo);



                        var nomeUsuario = new ReportParameter("NomeUsuario", usuario?.Name);
                        var titulo = new ReportParameter("Titulo", "Orçamento de Cirurgia");
                        var dataHora = new ReportParameter(
                            "DataHora",
                            string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now));
                        // ReportParameter filtroData = new ReportParameter("FiltroData", string.Format("{0:dd/MM/yyyy}", dataIni));
                        var nomeHospital = new ReportParameter("NomeHospital", disponibilidade.Empresa.NomeFantasia);
                        var convenio = new ReportParameter("Convenio", nomeConvenio);
                        var plano = new ReportParameter("Plano", nomePlano);
                        var paciente = new ReportParameter("Paciente", nomePaciente);
                        var dataHoraAgendamentoParan = new ReportParameter("DataHoraAgendamento", dataHoraAgendamento);

                        rvOrcamento.LocalReport.SetParameters(
                            new ReportParameter[]
                                {
                                    nomeHospital, nomeUsuario, titulo, dataHora, convenio, plano, paciente,
                                    dataHoraAgendamentoParan
                                    //filtroData
                                });




                        Warning[] warnings;
                        string[] streamIds;
                        var mimeType = string.Empty;
                        var encoding = string.Empty;
                        var extension = "pdf";
                        var pdfBytes = rvOrcamento.LocalReport.Render(
                            "PDF",
                            null,
                            out mimeType,
                            out encoding,
                            out extension,
                            out streamIds,
                            out warnings);

                        var absPath = string.Concat(this.Server.MapPath("/"), @"temp\");
                        var path = string.Empty;
                        var file = string.Empty;
                        var pathReturn = string.Empty;

                        file = string.Concat("AgendamentoOrcamento-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                        path = string.Concat(absPath, file);
                        pathReturn = this.Url.Content("~/temp/" + file);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                        fs.Write(pdfBytes, 0, pdfBytes.Length);
                        fs.Close();


                        rvOrcamento.LocalReport.Refresh();

                        this.Response.Headers.Add(
                            "Content-Disposition",
                            string.Format(
                                "inline; filename=AgendamentoOrcamento-{0}.pdf",
                                DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));

                        return this.Content(pathReturn);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return null;


        }


        public async Task<ActionResult> ImprimirOrcamentoPorAgendamento(long? agendamentoId)
        {
            // var itensId = JsonConvert.DeserializeObject<List<long>>(listItemFaturamento);

            //  var marteriasIds = JsonConvert.DeserializeObject<List<MaterialQuantidade>>(listItemMateriais);

            //  itensId.ForEach(f => marteriasIds.Add(new MaterialQuantidade { FaturamentoItemId = f, QuantidadeMaterial = 1 }));

            //  var _agendamentoSalaCirurgicaDisponibilidadeAppService = this.iocResolver.Resolve<IAgendamentoSalaCirurgicaDisponibilidadeAppService>();
            // var disponibilidade = await _agendamentoSalaCirurgicaDisponibilidadeAppService.Obter(agendamento.AgendamentoConsultaMedicoDisponibilidadeId ?? 0);
            //if (disponibilidade != null)
            //{
            using (var agendamentoSalaCirurgicaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaAppService>())
            using (var _faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            using (var _userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            {
                var agendamento = await agendamentoSalaCirurgicaAppService.Object.Obter(agendamentoId ?? 0).ConfigureAwait(false);



                var orcamentos = new List<AgendamentoOrcamentoDto>();
                foreach (var item in agendamento.Cirurgias)
                {
                    //var _faturamentoItemAppService = this.iocResolver.Resolve<IFaturamentoItemAppService>();

                    // var faturamentoItem = await _faturamentoItemAppService.Obter(item.FaturamentoItemId);
                    if (item.FaturamentoItem != null)
                    {
                        var faturamentoContaItemDto = new FaturamentoContaItemDto
                        {
                            FaturamentoItem = item.FaturamentoItem,
                            FaturamentoItemId = item.FaturamentoItemId
                        };

                        var valor = await _faturamentoContaItemAppService.Object.CalcularValorUnitarioItem(
                                        agendamento.AgendamentoSalaCirurgicaDisponibilidade.EmpresaId ?? 0,
                                        agendamento.ConvenioId ?? 0,
                                        agendamento.PlanoId ?? 0,
                                        faturamentoContaItemDto).ConfigureAwait(false);

                        var orcamento = new AgendamentoOrcamentoDto
                        {
                            FaturamentoItem = item.FaturamentoItem.Descricao,
                            Qtd = item.Quantidade.ToString(),
                            Valor = string.Format("{0:###,##0.00}", valor),
                            ValorDec = (decimal)valor * item.Quantidade
                        };


                        orcamentos.Add(orcamento);
                    }
                }

                var relDS = new AgendamentoOrcamento();

                var tabela = this.ConvertToDataTable(orcamentos /*dados.Contas*/, relDS.Tables["Orcamento"]);


                var dataSource = new ReportDataSource("Orcamento", tabela);
                using (var rvOrcamento = new ReportViewer())
                {
                    rvOrcamento.LocalReport.DataSources.Add(dataSource);

                    var scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(rvOrcamento);

                    rvOrcamento.LocalReport.ReportPath = string.Concat(
                        this.Server.MapPath("~"),
                        @"Relatorios\Atendimento\Agendamento\AgendamentoOrcamento.rdlc");


                    var usuario = await _userAppService.Object.GetUser().ConfigureAwait(false);

                    var relResumo = new AgendamentosDia();

                    var tabeResumo = this.ConvertToDataTable(
                        new List<string>() /*dados.Contas*/,
                        relResumo.Tables["Geral"]);
                    var row = tabeResumo.NewRow();
                    row["Logotipo"] = agendamento.AgendamentoSalaCirurgicaDisponibilidade.Empresa.Logotipo;
                    tabeResumo.Rows.Add(row);
                    // fim - logotipo

                    var dataSourceResumo = new ReportDataSource("Geral", tabeResumo);
                    // ReportViewer GuiaSpsadt = new ReportViewer();
                    rvOrcamento.LocalReport.DataSources.Add(dataSourceResumo);



                    var nomeUsuario = new ReportParameter("NomeUsuario", usuario?.Name);
                    var titulo = new ReportParameter("Titulo", "Orçamento de Cirurgia");
                    var dataHora = new ReportParameter("DataHora", string.Format("{0:dd/MM/yyyy HH:mm}", DateTime.Now));
                    // ReportParameter filtroData = new ReportParameter("FiltroData", string.Format("{0:dd/MM/yyyy}", dataIni));
                    var nomeHospital = new ReportParameter(
                        "NomeHospital",
                        agendamento.AgendamentoSalaCirurgicaDisponibilidade.Empresa.NomeFantasia);
                    var convenio = new ReportParameter("Convenio", agendamento.Convenio.Descricao);
                    var plano = new ReportParameter("Plano", agendamento.Plano.Descricao);
                    var paciente = new ReportParameter("Paciente", agendamento.Paciente.NomeCompleto);
                    var dataHoraAgendamentoParan = new ReportParameter(
                        "DataHoraAgendamento",
                        string.Format("{0:dd/MM/yyyy HH:mm}", agendamento.HoraAgendamento));

                    rvOrcamento.LocalReport.SetParameters(
                        new ReportParameter[]
                            {
                                nomeHospital, nomeUsuario, titulo, dataHora, convenio, plano, paciente,
                                dataHoraAgendamentoParan
                                //filtroData
                            });

                    Warning[] warnings;
                    string[] streamIds;
                    var mimeType = string.Empty;
                    var encoding = string.Empty;
                    var extension = "pdf";
                    var pdfBytes = rvOrcamento.LocalReport.Render(
                        "PDF",
                        null,
                        out mimeType,
                        out encoding,
                        out extension,
                        out streamIds,
                        out warnings);

                    var absPath = string.Concat(this.Server.MapPath("/"), @"temp\");
                    var path = string.Empty;
                    var file = string.Empty;
                    var pathReturn = string.Empty;

                    file = string.Concat("AgendamentoOrcamento-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                    path = string.Concat(absPath, file);
                    pathReturn = this.Url.Content("~/temp/" + file);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                    {
                        fs.Write(pdfBytes, 0, pdfBytes.Length);
                    }


                    rvOrcamento.LocalReport.Refresh();

                    this.Response.Headers.Add(
                        "Content-Disposition",
                        string.Format(
                            "inline; filename=AgendamentoOrcamento-{0}.pdf",
                            DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));

                    return this.Content(pathReturn);
                }
            }
        }

        public async Task<ActionResult> ExibirDescontoAgendamento(long id)
        {
            var viewModel = new AgendamentoDescontoViewModel { Id = id };

            using (var agendamentoSalaCirurgicaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaAppService>())
            {
                var procedimentos = await agendamentoSalaCirurgicaAppService.Object.ObterProcedimentos(new AgendamentoProcedimentoInput { Id = id })
                                        .ConfigureAwait(false);

                viewModel.DescontoJson = JsonConvert.SerializeObject(procedimentos.Items);

                var valorTotal = procedimentos.Items.Sum(s => s.ValorSemDesconto);
                var valorComDesconto = procedimentos.Items.Sum(s => s.ValorComDesconto);

                viewModel.ValorSemDesconto = string.Format("{0:###,##0.00}", valorTotal);
                viewModel.ValorDescontoTotal = valorTotal - valorComDesconto;


                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoCirurgias/Desconto.cshtml",
                    viewModel);
            }
        }

    }
}