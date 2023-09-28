using Abp.Dependency;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Enumeradores;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.CentralAutorizacao;
using SW10.SWMANAGER.Web.Controllers;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class ProrrogacoesController : SWMANAGERControllerBase
    {

        // GET: Mpa/Prorrogacao
        public ActionResult Index()
        {
            var viewModel = new AutorizacaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/CentralAutorizacao/Prorrogacoes/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id, long? itemId, long? atendimentoId)
        {
            using (var autorizacaoProcedimentoAppService = IocManager.Instance.ResolveAsDisposable<IAutorizacaoProcedimentoAppService>())
            using (var statusSolicitacaoProcedimentoAppService = IocManager.Instance.ResolveAsDisposable<IStatusSolicitacaoProcedimentoAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var viewModel = new AutorizacaoViewModel();
                var userId = AbpSession.UserId.Value;
                var status = await statusSolicitacaoProcedimentoAppService.Object.ListarTodos().ConfigureAwait(false);
                var listaItens = new List<AutorizacaoProcedimentoItemDto>();

                AutorizacaoProcedimentoDto autorizacao = null;

                if (atendimentoId != null)
                {
                    autorizacao = await autorizacaoProcedimentoAppService.Object.ObterProrrogacaoPorAtendimento((long)atendimentoId).ConfigureAwait(false);
                    if (autorizacao != null)
                    {
                        id = autorizacao.Id;
                    }
                }

                if (id.HasValue) //edição
                {
                    if (autorizacao == null)
                    {
                        autorizacao = await autorizacaoProcedimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    }

                    if (autorizacao != null)
                    {
                        viewModel = new AutorizacaoViewModel(autorizacao);
                        viewModel.FormaAutorizacao = autorizacao.Convenio?.FormaAutorizacao?.Descricao;
                        viewModel.DadosContato = autorizacao.Convenio?.DadosContato;
                        viewModel.NumeroGuia = autorizacao.Atendimento?.GuiaNumero;

                        var itensDto = await autorizacaoProcedimentoAppService.Object.ObterItens((long)id)
                                           .ConfigureAwait(false);


                        foreach (var item in itensDto)
                        {
                            listaItens.Add(
                                new AutorizacaoProcedimentoItemDto
                                {
                                    Id = item.Id,
                                    AutorizacaoProcedimentoId = item.AutorizacaoProcedimentoId,
                                    FaturamentoItemStr = item.FaturamentoItem?.Descricao,
                                    QuantidadeAutorizada = item.QuantidadeAutorizada,
                                    FaturamentoItemId = item.FaturamentoItemId,
                                    Senha = item.Senha,
                                    DataAutorizacao = item.DataAutorizacao,
                                    ItemSelecionado = item.Id == itemId,
                                    QuantidadeSolicitada = item.QuantidadeSolicitada,
                                    StatusId = item.StatusId,
                                    IsOrtese = item.IsOrtese,
                                    Observacao = item.Observacao,
                                    IdGrid = item.IdGrid,
                                    FaturamentoItemDescricao = item.FaturamentoItemDescricao,
                                    StatusDescricao = item.StatusDescricao,
                                    AutorizadoPor = item.AutorizadoPor
                                });
                        }

                    }
                }
                else //Novo
                {
                    viewModel.StatusId = (long)EnumStatusSolicitacao.EmAnalise;

                    if (atendimentoId != null)
                    {
                        var atendimento = await atendimentoAppService.Object.ObterParaProrrogacao((long)atendimentoId)
                                              .ConfigureAwait(false);

                        if (atendimento != null)
                        {
                            viewModel.Atendimento = atendimento;
                            viewModel.AtendimentoId = atendimento.Id;
                            viewModel.Solicitante = atendimento.Medico;
                            viewModel.SolicitanteId = atendimento.Medico.Id;
                            viewModel.NumeroGuia = atendimento.GuiaNumero;
                            viewModel.Convenio = atendimento.Convenio;
                            viewModel.ConvenioId = atendimento.Convenio.Id;
                            viewModel.FormaAutorizacao = atendimento.Convenio?.FormaAutorizacao?.Descricao;
                            viewModel.DadosContato = atendimento.Convenio?.DadosContato;
                        }
                    }
                }

                viewModel.Itens = JsonConvert.SerializeObject(listaItens);
                viewModel.Status = new SelectList(status.Items, "Id", "Descricao");
                return View(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/CentralAutorizacao/Prorrogacoes/_CriarOuEditarModal.cshtml",
                    viewModel);
            }
        }
    }
}