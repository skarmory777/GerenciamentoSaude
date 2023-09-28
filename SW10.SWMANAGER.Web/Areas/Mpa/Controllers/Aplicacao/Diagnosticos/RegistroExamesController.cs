using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Diagnosticos
{
    public class RegistroExamesController : SWMANAGERControllerBase
    {
        private readonly IRegistroExemesAppService _registroExameService;
        private readonly ISolicitacaoExameItemAppService _solicitacaoExameItemAppService;

        public RegistroExamesController(IRegistroExemesAppService registroExameService
            , ISolicitacaoExameItemAppService solicitacaoExameItemAppService)
        {
            _registroExameService = registroExameService;
            _solicitacaoExameItemAppService = solicitacaoExameItemAppService;
        }

        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new LaudoMovimentoViewModel(new LaudoMovimentoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/Registro/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var ambulatorioInternacao = new List<GenericoIdNome>();

            ambulatorioInternacao.Add(new GenericoIdNome { Id = 1, Nome = "Ambulatório" });
            ambulatorioInternacao.Add(new GenericoIdNome { Id = 2, Nome = "Internação" });

            var ionico = new List<GenericoIdNome>();

            ionico.Add(new GenericoIdNome { Id = 1, Nome = "Sim" });
            ionico.Add(new GenericoIdNome { Id = 2, Nome = "Não" });


            var aplicacao = new List<GenericoIdNome>();

            aplicacao.Add(new GenericoIdNome { Id = 1, Nome = "Bomba insufora" });
            aplicacao.Add(new GenericoIdNome { Id = 2, Nome = "Manual" });

            LaudoMovimentoViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new LaudoMovimentoViewModel(new LaudoMovimentoDto());
                viewModel.ExamesJson = JsonConvert.SerializeObject(new List<RegistroExameDto>());
                viewModel.DataRegistro = DateTime.Now;
            }
            else
            {
                var laudoMovimentoDto = await _registroExameService.Obter((long)id);

                viewModel = new LaudoMovimentoViewModel(laudoMovimentoDto);

                var listaExames = new List<RegistroExameDto>();

                long idGrid = 0;
                foreach (var item in laudoMovimentoDto.LaudoMovimentoItensDto)
                {
                    var exame = new RegistroExameDto
                    {
                        Id = item.Id,
                        ExameId = item.FaturamentoItem.Id,
                        ExameDescricao = item.FaturamentoItem.Descricao,
                        AccessNumber = SolicitacaoExameItemAppService.FormatAccessNumber(item.AccessNumber),
                        IdGrid = idGrid++
                    };

                    listaExames.Add(exame);
                }

                viewModel.Ionico = viewModel.IsIonico ? 1 : 2;
                viewModel.Aplicacao = viewModel.IsBombaInsufora ? 1 : 2;

                viewModel.ExamesJson = JsonConvert.SerializeObject(listaExames);

                viewModel.AmbulatorioInternacao = laudoMovimentoDto.Atendimento.IsAmbulatorioEmergencia ? 1 : 2;

                viewModel.ListaAmbulatorioInternacao = new SelectList(ambulatorioInternacao, "Id", "Nome", viewModel.AmbulatorioInternacao);

            }

            viewModel.ListaAmbulatorioInternacao = new SelectList(ambulatorioInternacao, "Id", "Nome");
            viewModel.ListaIonico = new SelectList(ionico, "Id", "Nome");
            viewModel.ListaAplicacao = new SelectList(aplicacao, "Id", "Nome");

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/Registro/_CriarOuEditarModal.cshtml", viewModel);
        }


        public async Task<ActionResult> CriarRegistroDeExameSolicitado(List<long> ids)
        {
            LaudoMovimentoViewModel viewModel = null;


            var ambulatorioInternacao = new List<GenericoIdNome>();

            ambulatorioInternacao.Add(new GenericoIdNome { Id = 1, Nome = "Ambulatório" });
            ambulatorioInternacao.Add(new GenericoIdNome { Id = 2, Nome = "Internação" });

            var ionico = new List<GenericoIdNome>();

            ionico.Add(new GenericoIdNome { Id = 1, Nome = "Sim" });
            ionico.Add(new GenericoIdNome { Id = 2, Nome = "Não" });


            var aplicacao = new List<GenericoIdNome>();

            aplicacao.Add(new GenericoIdNome { Id = 1, Nome = "Bomba insufora" });
            aplicacao.Add(new GenericoIdNome { Id = 2, Nome = "Manual" });




            if (ids != null)
            {
                var solicitacoesExames = await _solicitacaoExameItemAppService.ObterPorLista(ids);


                if (solicitacoesExames != null && solicitacoesExames.Count > 0)
                {
                    var solicitacaoExame = solicitacoesExames[0];

                    var laudoMovimentoDto = new LaudoMovimentoDto();

                    laudoMovimentoDto.AtendimentoId = (long)solicitacaoExame.Solicitacao.Atendimento.Id;
                    laudoMovimentoDto.Atendimento = solicitacaoExame.Solicitacao.Atendimento;
                    laudoMovimentoDto.ConvenioId = solicitacaoExame.Solicitacao.Atendimento.ConvenioId;
                    laudoMovimentoDto.Convenio = solicitacaoExame.Solicitacao.Atendimento.Convenio;
                    laudoMovimentoDto.DataRegistro = DateTime.Now;
                    laudoMovimentoDto.LeitoId = solicitacaoExame.Solicitacao.Atendimento.LeitoId;
                    laudoMovimentoDto.Leito = solicitacaoExame.Solicitacao.Atendimento.Leito;
                    laudoMovimentoDto.Obs = solicitacaoExame.Solicitacao.Observacao;
                    laudoMovimentoDto.PacienteId = solicitacaoExame.Solicitacao.Atendimento.PacienteId;
                    laudoMovimentoDto.MedicoSolicitante = solicitacaoExame.Solicitacao.Atendimento.Medico.NomeCompleto;

                    viewModel = new LaudoMovimentoViewModel(laudoMovimentoDto);

                    var listaExames = new List<RegistroExameDto>();

                    long idGrid = 0;
                    foreach (var item in solicitacoesExames)
                    {
                        var exame = new RegistroExameDto();

                        exame.ExameId = item.FaturamentoItem.Id;
                        exame.ExameDescricao = item.FaturamentoItem.Descricao;
                        exame.SolicitacaoExameId = item.Id;
                        exame.AccessNumber = SolicitacaoExameItemAppService.FormatAccessNumber(item.AccessNumber);
                        exame.IdGrid = idGrid++;
                        listaExames.Add(exame);
                    }

                    viewModel.AmbulatorioInternacao = solicitacaoExame.Solicitacao.Atendimento.IsAmbulatorioEmergencia ? 1 : 2;

                    viewModel.ListaAmbulatorioInternacao = new SelectList(ambulatorioInternacao, "Id", "Nome", viewModel.AmbulatorioInternacao);
                    viewModel.ListaIonico = new SelectList(ionico, "Id", "Nome");
                    viewModel.ListaAplicacao = new SelectList(aplicacao, "Id", "Nome");

                    viewModel.ExamesJson = JsonConvert.SerializeObject(listaExames);

                }
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/Registro/_CriarOuEditarModal.cshtml", viewModel);
        }


        public async Task<ActionResult> CriarPorExameFaturadosNaoRegistrados(List<long> ids)
        {
            LaudoMovimentoViewModel viewModel = null;


            var ambulatorioInternacao = new List<GenericoIdNome>();

            ambulatorioInternacao.Add(new GenericoIdNome { Id = 1, Nome = "Ambulatório" });
            ambulatorioInternacao.Add(new GenericoIdNome { Id = 2, Nome = "Internação" });

            var ionico = new List<GenericoIdNome>();

            ionico.Add(new GenericoIdNome { Id = 1, Nome = "Sim" });
            ionico.Add(new GenericoIdNome { Id = 2, Nome = "Não" });


            var aplicacao = new List<GenericoIdNome>();

            aplicacao.Add(new GenericoIdNome { Id = 1, Nome = "Bomba insufora" });
            aplicacao.Add(new GenericoIdNome { Id = 2, Nome = "Manual" });




            if (ids != null)
            {
                var examesFaturados = await _registroExameService.ObterExamesFaturadosSemregistros(ids);

                if (examesFaturados != null)
                {
                    // var solicitacaoExame = solicitacoesExames[0];
                    var laudoMovimentoDto = new LaudoMovimentoDto();

                    laudoMovimentoDto.AtendimentoId = (long)examesFaturados.Atendimento?.Id;
                    laudoMovimentoDto.Atendimento = examesFaturados.Atendimento;
                    laudoMovimentoDto.ConvenioId = examesFaturados.Atendimento?.Convenio?.Id;
                    laudoMovimentoDto.Convenio = examesFaturados.Atendimento?.Convenio;
                    laudoMovimentoDto.DataRegistro = DateTime.Now;
                    laudoMovimentoDto.LeitoId = examesFaturados.Atendimento?.Leito?.Id;
                    laudoMovimentoDto.Leito = examesFaturados.Atendimento?.Leito;
                    //laudoMovimentoDto.Obs = solicitacaoExame.Observacao;
                    laudoMovimentoDto.PacienteId = examesFaturados.Atendimento?.Paciente?.Id;
                    laudoMovimentoDto.MedicoSolicitante = examesFaturados.Atendimento?.Medico?.NomeCompleto;

                    viewModel = new LaudoMovimentoViewModel(laudoMovimentoDto);

                    var listaExames = new List<RegistroExameDto>();

                    long idGrid = 0;
                    foreach (var item in examesFaturados.LaudoMovimentoItensDto)
                    {
                        var exame = new RegistroExameDto();

                        exame.ExameId = item.FaturamentoItem.Id;
                        exame.ExameDescricao = item.FaturamentoItem.Descricao;
                        exame.FaturamentoContaItemId = item.FaturamentocontaItemId;
                        exame.AccessNumber = SolicitacaoExameItemAppService.FormatAccessNumber(item.AccessNumber);
                        exame.IdGrid = idGrid++;
                        listaExames.Add(exame);
                    }

                    var ambulatorioInternacaoId = examesFaturados.Atendimento.IsAmbulatorioEmergencia ? 1 : 2;

                    viewModel.ListaAmbulatorioInternacao = new SelectList(ambulatorioInternacao, "Id", "Nome", ambulatorioInternacaoId);
                    viewModel.ListaIonico = new SelectList(ionico, "Id", "Nome");
                    viewModel.ListaAplicacao = new SelectList(aplicacao, "Id", "Nome");

                    viewModel.ExamesJson = JsonConvert.SerializeObject(listaExames);

                }
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Diagnosticos/Imagens/Registro/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}