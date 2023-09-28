using Abp.Application.Services.Dto;
using Abp.Dependency;
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
    public class DevolucoesController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new PreMovimentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Devolucoes/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarPreMovimentoModalViewModel viewModel;

            if (id.HasValue) //edição
            {
                using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                {
                    var output = await preMovimentoAppService.Object.Obter((long)id);

                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                    {
                        PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output)
                    };
                }
            }
            else //Novo
            {
                viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel());
            }

            viewModel.Movimento = DateTime.Now;

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Devolucoes/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarPreMovimentoItemModal(long? id, long preMovimentoId = 0, long estoqueId = 0)
        {
            CriarOuEditarPreMovimentoItemModalViewModel viewModel;

            if (id.HasValue && id.Value != 0)
            {

                using (var _estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
                using (var _estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                using (var _preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                {
                    var model = await _estoquePreMovimentoItemAppService.Object.Obter(id.Value);
                    viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(model);
                    if (model != null)
                    {
                        viewModel.Hidden = (model.Produto.IsValidade || model.Produto.IsLote) ? "" : "hidden";
                        var EstoquePreMovimentoLoteValidadeResult = await _estoqueLoteValidadeAppService.Object.ListarPorPreMovimentoItem(new ListarEstoquePreMovimentoInput { Filtro = model.Id.ToString() });
                        if (EstoquePreMovimentoLoteValidadeResult != null && EstoquePreMovimentoLoteValidadeResult.Items.Count() > 0)
                        {
                            var loteValidate = EstoquePreMovimentoLoteValidadeResult.Items[0].LoteValidade;
                            viewModel.LaboratorioId = loteValidate.ProdutoLaboratorioId;
                            viewModel.Lote = loteValidate.Lote;
                            viewModel.Validade = loteValidate.Validade;
                            viewModel.LoteValidadeId = loteValidate.Id;
                            viewModel.EstoquePreMovimentoLoteValidadeId = EstoquePreMovimentoLoteValidadeResult.Items[0].Id;

                            model.EstoquePreMovimento = await _preMovimentoAppService.Object.Obter(model.PreMovimentoId);

                            var lotesValidades = await _estoqueLoteValidadeAppService.Object.ObterPorProdutoEstoqueComSaida(model.ProdutoId, estoqueId, (long)model.EstoquePreMovimento.EstTipoMovimentoId, model.EstoquePreMovimento.UnidadeOrganizacionalId, model.EstoquePreMovimento.PacienteId);

                            viewModel.LotesValidades = new SelectList(lotesValidades, "Id", "Nome", loteValidate.Id);
                        }
                        else
                        {
                            viewModel.LotesValidades = new SelectList(new ListResultDto<LoteValidadeDto>().Items, "Id", "Nome");
                        }
                    }
                }
            }
            else
            {
                viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto());
                viewModel.Unidades = new SelectList(new ListResultDto<Unidade>().Items, "Id", "Descricao");
                viewModel.LotesValidades = new SelectList(new ListResultDto<LoteValidadeDto>().Items, "Id", "Nome");
            }
            viewModel.Id = id.Value;
            viewModel.PreMovimentoId = preMovimentoId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Devolucoes/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);

        }

        public async Task<JsonResult> SelecionarLotesValidades(long produtoId, long estoqueId, long tipoMovimentoId, long? unidadeOrganizacionalId, long? atendimentoId)
        {
            using (var _estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
            {
                var lotesValidades = await _estoqueLoteValidadeAppService.Object.ObterPorProdutoEstoqueComSaida(produtoId, estoqueId, tipoMovimentoId, unidadeOrganizacionalId, atendimentoId);
                return Json(lotesValidades, JsonRequestBehavior.AllowGet);
            }
        }

    }
}