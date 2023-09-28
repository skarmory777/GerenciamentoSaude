using Newtonsoft.Json;
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
    public class CentralAutorizacoesController : SWMANAGERControllerBase
    {

        private readonly IAutorizacaoProcedimentoAppService _autorizacaoProcedimentoAppService;
        private readonly IStatusSolicitacaoProcedimentoAppService _statusSolicitacaoProcedimentoAppService;

        public CentralAutorizacoesController(IAutorizacaoProcedimentoAppService autorizacaoProcedimentoAppService
                                            , IStatusSolicitacaoProcedimentoAppService statusSolicitacaoProcedimentoAppService)
        {
            _autorizacaoProcedimentoAppService = autorizacaoProcedimentoAppService;
            _statusSolicitacaoProcedimentoAppService = statusSolicitacaoProcedimentoAppService;
        }

        // GET: Mpa/AtendimentoClinico
        public ActionResult Index()
        {
            var viewModel = new AutorizacaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/CentralAutorizacao/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id, long? itemId)
        {
            var viewModel = new AutorizacaoViewModel();
            var userId = AbpSession.UserId.Value;
            var status = await _statusSolicitacaoProcedimentoAppService.ListarTodos();
            var listaItens = new List<AutorizacaoProcedimentoItemDto>();

            if (id.HasValue) //edição
            {
                var autorizacao = await _autorizacaoProcedimentoAppService.Obter((long)id);
                if (autorizacao != null)
                {
                    viewModel = new AutorizacaoViewModel(autorizacao);
                    var itensDto = await _autorizacaoProcedimentoAppService.ObterItens((long)id);


                    foreach (var item in itensDto)
                    {
                        listaItens.Add(new AutorizacaoProcedimentoItemDto
                        {
                            Id = item.Id,
                            AutorizacaoProcedimentoId = item.AutorizacaoProcedimentoId,
                            FaturamentoItemStr = item.FaturamentoItem.Descricao,
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
            }
            viewModel.Itens = JsonConvert.SerializeObject(listaItens);
            viewModel.Status = new SelectList(status.Items, "Id", "Descricao");
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/CentralAutorizacao/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}