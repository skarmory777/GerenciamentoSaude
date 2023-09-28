using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Threading;
using Abp.UI;
using Castle.Core.Internal;
using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Dtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Kit;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Pacote;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Home;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.FaturamentoAtendimento;
using SW10.SWMANAGER.Web.Controllers;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturarAtendimentoController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/Index.cshtml", null);
        }

        public ActionResult AuditoriaInterna()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/AuditoriaInterna/Index.cshtml", null);
        }

        public ActionResult AuditoriaExterna()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/AuditoriaExterna/Index.cshtml", null);
        }

        public async Task<ActionResult> ContaMedica(long atendimentoId, long contaMedicaId)
        {
            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            {
                var contaMedicaViewModel = new FaturarAtendimentoContaMedicaModel
                {
                    AtendimentoId = atendimentoId,
                    ContaMedicaId = contaMedicaId,
                    ContaMedica = await contaMedicaAppService.Object.Obter(contaMedicaId).ConfigureAwait(false)
                };
                return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ContaMedica/ContaMedica.cshtml", contaMedicaViewModel);
            }

        }

        public async Task<ActionResult> ImpressaoContaMedica(long atendimentoId, long contaMedicaId)
        {
            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            {
                var contaMedicaViewModel = new FaturarAtendimentoContaMedicaModel
                {
                    AtendimentoId = atendimentoId,
                    ContaMedica = await contaMedicaAppService.Object.Obter(contaMedicaId).ConfigureAwait(false)
                };
                return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ImpressaoContaMedica/ImpressaoContaMedica.cshtml", contaMedicaViewModel);
            }
        }

        public async Task<ActionResult> CriarOuEditarContaMedicaModal(long atendimentoId, long? contaMedicaId)
        {
            using (var AtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            {
                var contaMedicaViewModel = new FaturarAtendimentoContaMedicaModel
                {
                    AtendimentoId = atendimentoId,
                    ContaMedicaId = contaMedicaId ?? 0,
                    ContaMedica = contaMedicaId.HasValue
                        ? await contaMedicaAppService.Object.Obter(contaMedicaId.Value).ConfigureAwait(false)
                        : new FaturamentoContaDto()
                };
                var atendimento = await AtendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);
                contaMedicaViewModel.IsAmbulatorioEmergencia = atendimento.IsAmbulatorioEmergencia;
                if (!contaMedicaId.HasValue)
                {
                    
                    MapearAtendimentoContaMedica(contaMedicaViewModel.ContaMedica, atendimento);
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ContaMedica/_CriarOuEditarModal.cshtml", contaMedicaViewModel);
            }
        }

        private static void MapearAtendimentoContaMedica(FaturamentoContaDto contaMedica, AtendimentoDto atendimento)
        {
            if (atendimento == null)
            {
                return;
            }

            contaMedica.DataInicio = atendimento.DataRegistro;
            contaMedica.DataFim = atendimento.DataAlta;
            contaMedica.ConvenioId = atendimento.ConvenioId;
            contaMedica.PlanoId = atendimento.PlanoId;
            contaMedica.GuiaId = atendimento.GuiaId;
            contaMedica.FatGuiaId = atendimento.FatGuiaId;
            contaMedica.Titular = atendimento.Titular;
            contaMedica.CodDependente = atendimento.CodDependente;
            contaMedica.NumeroGuia = atendimento.GuiaNumero;
            contaMedica.OrigemTitular = atendimento.OrigemTitular;
            contaMedica.SenhaAutorizacao = atendimento.Senha;
            contaMedica.ValidadeCarteira = atendimento.ValidadeCarteira;
            contaMedica.MedicoId = atendimento.MedicoId;
            contaMedica.Matricula = atendimento.Matricula;
            contaMedica.UnidadeOrganizacionalId = atendimento.UnidadeOrganizacionalId;
            contaMedica.TipoAcomodacaoId = atendimento.Leito?.TipoAcomodacaoId;
        }

        public async Task<ActionResult> CriarOuEditarKitModal(CriarOuEditarKitModalInputDto input)
        {
            using (var fatKitAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoKitAppService>())
            using (var fatContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            {
                if (!input.KitId.HasValue)
                {
                    throw new UserFriendlyException("Não é possível cadastrar kit sem kit selecionado.");
                }

                var viewModel = new CriarOuEditarKitModalViewModel()
                {
                    KitId = input.KitId,
                    Data = input.Data,
                    ContaMedicaId = input.ContaMedicaId,
                    CentroCustoId = input.CentroCustoId,
                    TerceirizadoId = input.TerceirizadoId,
                    TurnoId = input.TurnoId,
                    TipoLeitoId = input.TipoLeitoId,
                    UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                    Qtde = input.Qtde
                };

                if (input.Id != 0)
                {
                    var kit = await fatKitAppService.Object.ObterDapper(input.KitId.Value).ConfigureAwait(false);
                    viewModel.Kit = kit;
                    viewModel.KitId = input.KitId;
                    viewModel.Items = (await fatContaItemAppService.Object.ObterPorContaKit(input.Id, input.ContaMedicaId).ConfigureAwait(false)).ToList();


                }
                else
                {
                    var kit = await fatKitAppService.Object.ObterDapper(input.KitId.Value).ConfigureAwait(false);

                    if (!kit.Itens.IsNullOrEmpty())
                    {
                        viewModel.Kit = kit;
                        viewModel.Items = kit.Itens.Select(kitItem =>
                            CriarOuEditarKitModalViewModel.MapearKitItemParaFatContaItem(kitItem, input)).ToList();

                        foreach (var item in viewModel.Items.Where(x => x.FaturamentoItemId.HasValue))
                        {
                            var honorarios = ResumoDetalhamentoExtensions.MapearHonorarios(item);
                            var dto = new ValorTotalItemFaturamentoDto(
                                input.ContaMedicaId, 
                                input.Data, 
                                item.FaturamentoItemId.Value, 
                                (input.Qtde ?? 0) * item.Qtde, 
                                item.Percentual == 0 ? 1 : item.Percentual,
                                item.UnidadeOrganizacionalId,
                                item.TerceirizadoId,
                                item.CentroCustoId,
                                item.TurnoId,
                                item.TipoLeitoId,
                                honorarios);
                            var result = await fatContaItemAppService.Object
                                .CalcularValorTotalItemFaturamento(dto).ConfigureAwait(false);
                            item.ResumoDetalhamento = result.ReturnObject.ResumoDetalhamento;
                        }
                    }
                }
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ContaMedica/Kit/CriarOuEditarKitModal.cshtml", viewModel);
            }

        }



        public async Task<ActionResult> CriarOuEditarPacoteModal(CriarOuEditarPacoteModalInputDto input)
        {
            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            using (var fatItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemAppService>())
            using (var fatPacoteAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoPacoteAppService>())
            using (var fatContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            {
                if (!input.PacoteId.HasValue)
                {
                    throw new UserFriendlyException("Não é possível cadastrar pacote sem pacote selecionado.");
                }

                var viewModel = new CriarOuEditarPacoteModalViewModel()
                {
                    PacoteId = input.PacoteId,
                    DataInicio = input.DataInicio,
                    DataFim = input.DataFim,
                    ContaMedicaId = input.ContaMedicaId,
                    CentroCustoId = input.CentroCustoId,
                    TerceirizadoId = input.TerceirizadoId,
                    TurnoId = input.TurnoId,
                    TipoLeitoId = input.TipoLeitoId,
                    UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                    Qtde = input.Qtde,
                    Input = input
                };

                if (viewModel.Input != null)
                {
                    using (var unidadeOrganizacionalRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                    using (var terceirizadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Terceirizado, long>>())
                    using (var tipoLeitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoAcomodacao, long>>())
                    using (var turnoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Turno, long>>())
                    {
                        if (viewModel.Input.UnidadeOrganizacionalId.HasValue && viewModel.Input.UnidadeOrganizacionalId != 0)
                        {
                            viewModel.Input.UnidadeOrganizacionalDescricao = unidadeOrganizacionalRepository
                                .Object.GetAll().AsNoTracking()
                                .FirstOrDefault(x => x.Id == viewModel.Input.UnidadeOrganizacionalId)?.Descricao;
                        }

                        if (viewModel.Input.TerceirizadoId.HasValue && viewModel.Input.TerceirizadoId != 0)
                        {
                            viewModel.Input.TerceirizadoDescricao = terceirizadoRepository
                                .Object.GetAll().AsNoTracking().Include(x => x.SisPessoa)
                                .FirstOrDefault(x => x.Id == viewModel.Input.TerceirizadoId)?.SisPessoa?.NomeCompleto;
                        }

                        if (viewModel.Input.TipoLeitoId.HasValue && viewModel.Input.TipoLeitoId != 0)
                        {
                            viewModel.Input.TipoLeitoDescricao = tipoLeitoRepository
                                .Object.GetAll().AsNoTracking()
                                .FirstOrDefault(x => x.Id == viewModel.Input.TipoLeitoId)?.Descricao;
                        }

                        if (viewModel.Input.TurnoId.HasValue && viewModel.Input.TurnoId != 0)
                        {
                            viewModel.Input.TurnoDescricao = turnoRepository
                                .Object.GetAll().AsNoTracking()
                                .FirstOrDefault(x => x.Id == viewModel.Input.TurnoId)?.Descricao;
                        }
                    }
                }

                if (input.Id != 0)
                {
                    //var kit = await fatPacoteAppService.Object.Obter(input.KitId.Value).ConfigureAwait(false);
                    //viewModel.Kit = kit;
                    //viewModel.KitId = input.KitId;
                    //viewModel.Items = (await fatContaItemAppService.Object.ObterPorContaKit(input.Id, input.ContaMedicaId).ConfigureAwait(false)).ToList();


                }
                else
                {
                    var pacote = await fatItemAppService.Object.Obter(input.PacoteId ?? 0).ConfigureAwait(false);
                    var items = (await contaMedicaAppService.Object.ListarItems(new FaturamentoContaItemTableFilterDto()
                    {
                        ContaMedicaId = input.ContaMedicaId,
                        EnablePaginate = false
                    }).ConfigureAwait(false)).Items;

                    if (!items.IsNullOrEmpty())
                    {
                        viewModel.Pacote = pacote;
                        viewModel.Items = items
                            .Where(x => x.Data.HasValue && x.Data.Value.Date >= input.DataInicio && x.Data.Value.Date <= input.DataFim)
                            .Where(x => x.FaturamentoPacoteId == null && x.TipoGrupoId != 4).Select(x => CriarOuEditarPacoteModalViewModel.MapearPacoteItemParaFatContaItem(x, input)).ToList();

                        foreach (var item in viewModel.Items.Where(x => x.FaturamentoItemId.HasValue))
                        {
                            var honorarios = ResumoDetalhamentoExtensions.MapearHonorarios(item);
                            var dto = new ValorTotalItemFaturamentoDto(
                                input.ContaMedicaId,
                                item.Data.Value.DateTime, 
                                item.FaturamentoItemId.Value,
                                (input.Qtde ?? 0) * item.Qtde, 
                                item.Percentual == 0 ? 1 : item.Percentual,
                                item.UnidadeOrganizacionalId,
                                item.TerceirizadoId,
                                item.CentroCustoId,
                                item.TurnoId,
                                item.TipoLeitoId,
                                honorarios);
                            var result = await fatContaItemAppService.Object
                                .CalcularValorTotalItemFaturamento(dto).ConfigureAwait(false);
                            item.ResumoDetalhamento = result.ReturnObject.ResumoDetalhamento;
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("Não é possível cadastrar pacote sem nenhum item possível.");
                    }
                }
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ContaMedica/Pacote/CriarOuEditarPacoteModal.cshtml", viewModel);
            }

        }



        public ActionResult historicoItemModal()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ContaMedica/historicoItemModal.cshtml", null);
        }



        public FileResult ImprimirConta(long atendimentoId, long contaMedicaId,DateTime? dataInicial = null, DateTime? dataFinal = null, 
            List<long> grupoIds = null, List<long> centroDeCustoIds = null, 
            List<long> localUtilizacaoIds = null, List<long> terceirizadoIds = null, 
            List<long> turnoIds = null, 
            string tipo = "Aberta")
        {
            grupoIds = !grupoIds.IsNullOrEmpty() ? grupoIds.Where(x => x != 0).ToList() : null;
            centroDeCustoIds = !centroDeCustoIds.IsNullOrEmpty() ? centroDeCustoIds.Where(x => x != 0).ToList() : null;
            localUtilizacaoIds = !localUtilizacaoIds.IsNullOrEmpty() ? localUtilizacaoIds.Where(x => x != 0).ToList() : null;
            terceirizadoIds = !terceirizadoIds.IsNullOrEmpty() ? terceirizadoIds.Where(x => x != 0).ToList() : null;
            turnoIds = !turnoIds.IsNullOrEmpty() ? turnoIds.Where(x => x != 0).ToList() : null;

            var context = ViewRenderer.CreateController<EmptyController>().ControllerContext;
            var renderer = new ViewRenderer(context);

            using (var atendimentoService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var contaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            {
                var baseUrl = ConfigurationManager.AppSettings.Get("baseUrl");
                var model = new RptFaturarAtendimentoResumoContaMedicaModel(atendimentoId, contaMedicaId, tipo)
                {
                    Url = baseUrl
                };

                var modelHeader = AsyncHelper.RunSync(() => atendimentoService.Object.Obter(atendimentoId));

                var viewModelHeader = new AssistenciaisViewModel(modelHeader, new HeaderAtendimentoPacienteNavBarOptions(true))
                {
                    IsAmbulatorioEmergencia = !modelHeader.IsInternacao,
                    IsInternacao = modelHeader.IsInternacao
                };

                //model.HeaderHtml =  renderer.RenderViewToString("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Home/_headerAtendimentoPacienteNavBar.cshtml", viewModelHeader);


                if (tipo == FaturarAtendimentoResumoContaMedicaModel.TipoAberta)
                {
                    model.ResumoConta = AsyncHelper.RunSync(() => contaAppService.Object.ResumoContaAberta(new FaturamentoResumoContaFilterDto
                    {
                        Id = contaMedicaId.ToString(),
                        DataInicial = dataInicial,
                        DataFinal = dataFinal,
                        GrupoIds =grupoIds,
                        CentroDeCustoIds = centroDeCustoIds,
                        LocalUtilizacaoIds = localUtilizacaoIds,
                        TerceirizadoIds = terceirizadoIds,
                        TurnoIds = turnoIds
                    })).ReturnObject;
                } else if (tipo == FaturarAtendimentoResumoContaMedicaModel.TipoFechada)
                {
                    model.ResumoConta = AsyncHelper.RunSync(() => contaAppService.Object.ResumoContaFechada(new FaturamentoResumoContaFilterDto
                    {
                        Id = contaMedicaId.ToString(),
                        DataInicial = dataInicial,
                        DataFinal = dataFinal,
                        GrupoIds = grupoIds,
                        CentroDeCustoIds = centroDeCustoIds,
                        LocalUtilizacaoIds = localUtilizacaoIds,
                        TerceirizadoIds = terceirizadoIds,
                        TurnoIds = turnoIds
                    })).ReturnObject;
                }

                var htmlRender = renderer.RenderPartialViewToString("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturarAtendimento/ImpressaoContaMedica/RptImpressaoContaMedica.cshtml", model);
                
                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter
                {
                    CustomWkHtmlArgs = "--viewport-size 1024x768 -T 5mm -B 5mm -L 5mm -R 5mm",
                    PageFooterHtml =
                           $@"<div style=""width:100%;text-align:right;font-size:10px !important""><span style=""text-align:left;left:0px;top:5px"">SWManager - TSW Tecnologia em Saúde</span> <span></span> <span class=""page""></span>/<span class=""topage""></span></div>"
                };

                var pdfBytes = htmlToPdf.GeneratePdf(htmlRender);

                return File(pdfBytes, "application/pdf", "DownloadName.pdf");

            }
        }

    }
}