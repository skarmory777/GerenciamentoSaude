using Abp.Application.Services.Dto;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TransferenciasController : Controller // Web.Controllers.SWMANAGERControllerBase
    {
        private readonly IEstoqueAppService _produtoEstoqueAppService;
        private readonly IEstoquePreMovimentoAppService _preMovimentoAppService;
        private readonly IProdutoAppService _produtoAppService;
        private readonly IEstoquePreMovimentoItemAppService _estoquePreMovimentoItemAppService;
        private readonly IUnidadeAppService _unidadeAppService;
        private readonly IProdutoLaboratorioAppService _produtoLaboratorioAppService;
        private readonly IEstoqueLoteValidadeAppService _estoqueLoteValidadeAppService;

        public TransferenciasController(IEstoqueAppService produtoEstoqueAppService,
                                        IEstoquePreMovimentoAppService preMovimentoAppService,
                                        IProdutoAppService produtoAppService,
                                        IEstoquePreMovimentoItemAppService estoquePreMovimentoItemAppService,
                                        IUnidadeAppService unidadeAppService,
                                        IProdutoLaboratorioAppService produtoLaboratorioAppService,
                                        IEstoqueLoteValidadeAppService estoqueLoteValidadeAppService)
        {
            _produtoEstoqueAppService = produtoEstoqueAppService;
            _preMovimentoAppService = preMovimentoAppService;
            _produtoAppService = produtoAppService;
            _estoquePreMovimentoItemAppService = estoquePreMovimentoItemAppService;
            _unidadeAppService = unidadeAppService;
            _produtoLaboratorioAppService = produtoLaboratorioAppService;
            _estoqueLoteValidadeAppService = estoqueLoteValidadeAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new PreMovimentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Transferencias/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            // var estoques = await _produtoEstoqueAppService.ListarTodos();

            EstoqueTransferenciaProdutoViewModel viewModel;

            if (id.HasValue) //edição
            {
                EstoqueTransferenciaProdutoDto output = await _preMovimentoAppService.ObterTransferencia((long)id);

                viewModel = new EstoqueTransferenciaProdutoViewModel();
                viewModel.EstoqueSaidaId = (long)output.PreMovimentoSaida.EstoqueId;
                viewModel.EstoqueEntradaId = (long)output.PreMovimentoEntrada.EstoqueId;

                //viewModel.EstoquesSaida = new SelectList(estoques.Items, "Id", "Descricao", output.PreMovimentoSaida.EstoqueId);
                if (output.PreMovimentoSaida.EstoqueId != null && output.PreMovimentoSaida.EstoqueId != 0)
                {
                    viewModel.PreMovimentoSaida = new EstoquePreMovimentoDto();
                    viewModel.PreMovimentoSaida.Estoque = await _produtoEstoqueAppService.Obter((long)output.PreMovimentoSaida.EstoqueId);
                }


                //viewModel.EstoquesEntrada = new SelectList(estoques.Items, "Id", "Descricao", output.PreMovimentoEntrada.EstoqueId);

                if (output.PreMovimentoEntrada.EstoqueId != null && output.PreMovimentoEntrada.EstoqueId != 0)
                {
                    viewModel.PreMovimentoEntrada = new EstoquePreMovimentoDto();
                    viewModel.PreMovimentoEntrada.Estoque = await _produtoEstoqueAppService.Obter((long)output.PreMovimentoEntrada.EstoqueId);
                }

                viewModel.Documento = output.Documento;
                viewModel.PreMovimentoSaidaId = output.PreMovimentoSaidaId;
                viewModel.PreMovimentoEntradaId = output.PreMovimentoEntradaId;
                viewModel.Id = output.Id;
                viewModel.PreMovimentoEstadoId = output.PreMovimentoEntrada.PreMovimentoEstadoId;
                viewModel.Movimento = output.PreMovimentoEntrada.Movimento;

            }
            else //Novo
            {
                viewModel = new EstoqueTransferenciaProdutoViewModel();

                // long var;

                //viewModel.EstoquesSaida = FuncoesGlobais.SelecionarSelectListUnitario<EstoqueDto>(estoques.Items, "Id", "Descricao", out var);
                //viewModel.EstoqueSaidaId = var;

                //viewModel.EstoquesEntrada = FuncoesGlobais.SelecionarSelectListUnitario<EstoqueDto>(estoques.Items, "Id", "Descricao", out var);
                //viewModel.EstoqueEntradaId = var;
                viewModel.Movimento = DateTime.Now;

            }



            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Transferencias/_CriarOuEditarModal.cshtml", viewModel);
        }





        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Edit)]
        public async Task<ActionResult> CriarOuEditarTransferenciaItemModal(long? id, long transferenciaId = 0, long estoqueId = 0, long transferenciaItemId = 0)
        {
            CriarOuEditarPreMovimentoItemModalViewModel viewModel;

            // var produtos = await _produtoAppService.ListarTodosParaMovimento();
            var laboratorios = await _produtoLaboratorioAppService.ListarTodos();

            long preMovimentoId = 0;

            EstoqueTransferenciaProdutoDto transferencia = await _preMovimentoAppService.ObterTransferencia((long)transferenciaId);



            if (id.HasValue && id.Value != 0)
            {
                var transferenciaItem = await _estoquePreMovimentoItemAppService.ObterTransferenciaItem((long)id);



                var model = await _estoquePreMovimentoItemAppService.Obter(transferenciaItem.PreMovimentoSaidaItemId);
                viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(model);
                if (model != null)
                {
                    viewModel.Hidden = (model.Produto.IsValidade || model.Produto.IsLote) ? "" : "hidden";

                    //viewModel.Produtos = new SelectList(produtos.Items, "Id", "Descricao", model.ProdutoId);

                    if (model.ProdutoId != 0)
                    {
                        viewModel.Produto = await _produtoAppService.Obter(model.ProdutoId);
                    }

                    var unidades = await _produtoAppService.ObterUnidadePorProduto(model.ProdutoId);
                    viewModel.Unidades = new SelectList(unidades.Items, "Id", "Descricao", model.ProdutoUnidadeId);

                    if (model.ProdutoUnidadeId != 0)
                    {
                        var unidade = await _unidadeAppService.Obter((long)model.ProdutoUnidadeId);
                        if (unidade != null)
                        {
                            viewModel.Quantidade = model.Quantidade / unidade.Fator;
                        }
                    }

                    var EstoquePreMovimentoLoteValidadeResult = await _estoqueLoteValidadeAppService.ListarPorPreMovimentoItem(new ListarEstoquePreMovimentoInput { Filtro = model.Id.ToString() });
                    if (EstoquePreMovimentoLoteValidadeResult != null && EstoquePreMovimentoLoteValidadeResult.Items.Count() > 0)
                    {
                        var loteValidate = EstoquePreMovimentoLoteValidadeResult.Items[0].LoteValidade;
                        viewModel.LaboratorioId = loteValidate.ProdutoLaboratorioId;
                        viewModel.Lote = loteValidate.Lote;
                        viewModel.Validade = loteValidate.Validade;
                        viewModel.Laboratorios = new SelectList(laboratorios.Items, "Id", "Descricao", loteValidate.ProdutoLaboratorioId);
                        viewModel.LoteValidadeId = loteValidate.Id;
                        viewModel.EstoquePreMovimentoLoteValidadeId = EstoquePreMovimentoLoteValidadeResult.Items[0].Id;

                        var lotesValidades = await _estoqueLoteValidadeAppService.ObterPorProdutoEstoque(model.ProdutoId, estoqueId, preMovimentoId);

                        viewModel.LotesValidades = new SelectList(lotesValidades, "Id", "Nome", loteValidate.Id);
                    }
                    else
                    {
                        viewModel.Laboratorios = new SelectList(laboratorios.Items, "Id", "Nome");
                        viewModel.LotesValidades = new SelectList(new ListResultDto<LoteValidadeDto>().Items, "Id", "Nome");
                    }
                }
            }
            else
            {
                viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto());
                // viewModel.Produtos = new SelectList(produtos.Items, "Id", "Descricao");
                viewModel.Unidades = new SelectList(new ListResultDto<Unidade>().Items, "Id", "Descricao");
                viewModel.Laboratorios = new SelectList(laboratorios.Items, "Id", "Descricao");
                viewModel.LotesValidades = new SelectList(new ListResultDto<LoteValidadeDto>().Items, "Id", "Nome");
            }

            if (transferencia != null)
            {
                preMovimentoId = transferencia.PreMovimentoSaidaId;
                viewModel.TransferenciaId = transferencia.Id;
                viewModel.TransferenciaItemId = transferenciaItemId;
                //viewModel.PreMovimentoSaidaId = transferencia.PreMovimentoSaidaId;
                //viewModel.PreMovimentoEntradaId = transferencia.PreMovimentoEntradaId;
            }


            viewModel.Id = id.Value;
            viewModel.PreMovimentoId = preMovimentoId;





            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Transferencias/_CriarOuEditarTransferenciaItemModal.cshtml", viewModel);
        }

    }
}