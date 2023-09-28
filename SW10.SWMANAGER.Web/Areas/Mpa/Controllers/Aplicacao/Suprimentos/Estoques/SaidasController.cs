using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class SaidasController : Controller // Web.Controllers.SWMANAGERControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var model = new PreMovimentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Saidas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            using (var motivoPerdaProdutoAppService = IocManager.Instance.ResolveAsDisposable<IMotivoPerdaProdutoAppService>())
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var userId = abpSession.Object.UserId.Value;

                var motivosPerdas = await motivoPerdaProdutoAppService.Object.ListarTodos().ConfigureAwait(false);
                var tiposAtendimentos = new ListResultDto<GenericoIdNome>
                {
                    Items = new List<GenericoIdNome> { new GenericoIdNome { Id = 0, Nome = "Ambulatório/Emergência" }, new GenericoIdNome { Id = 1, Nome = "Internação" }}
                };


                CriarOuEditarPreMovimentoModalViewModel viewModel;

                if (id.HasValue) //edição
                {
                    EstoquePreMovimentoDto output = await preMovimentoAppService.Object.Obter((long)id).ConfigureAwait(false);

                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                    {
                        PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false),
                        MotivosPerda = new SelectList(motivosPerdas.Items, "Id", "Descricao", output.MotivoPerdaProdutoId)
                    };

                    if (output.AtendimentoId != null && output.AtendimentoId != 0)
                    {
                        var atendimento = await atendimentoAppService.Object.Obter((long)output.AtendimentoId).ConfigureAwait(false);
                        viewModel.Atendimento = new AtendimentoDto { Id = atendimento.Id, Codigo = atendimento.Codigo, Paciente = new PacienteDto { NomeCompleto = atendimento.Paciente.NomeCompleto } };
                        viewModel.TiposAtendimentos = new SelectList(tiposAtendimentos.Items, "Id", "Nome", atendimento.IsAmbulatorioEmergencia ? 0 : 1);
                    }
                    else
                    {
                        viewModel.TiposAtendimentos = new SelectList(tiposAtendimentos.Items, "Id", "Nome");
                    }
                }
                else //Novo
                {
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel())
                    {
                        MotivosPerda = new SelectList(motivosPerdas.Items, "Id", "Descricao"),
                        TiposAtendimentos = new SelectList(tiposAtendimentos.Items, "Id", "Nome")
                    };
                }

                viewModel.Movimento = DateTime.Now;
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Saidas/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Edit)]
        public async Task<ActionResult> CriarOuEditarPreMovimentoKitEstoqueItemModal(long preMovimentoId, long estoqueId = 0)
        {
            CriarOuEditarPreMovimentoKitEstoqueItemModalViewModel viewModel;
            using (var kitEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueKitAppService>())
            {
                var kitsEstoque = await kitEstoqueAppService.Object.ListarTodos().ConfigureAwait(false);

                viewModel = new CriarOuEditarPreMovimentoKitEstoqueItemModalViewModel();
                viewModel.KitsEstoque = new SelectList(kitsEstoque.Items, "Id", "Descricao");
                viewModel.Quantidade = 1;
                viewModel.PreMovimentoId = preMovimentoId;

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Saidas/_CriarOuEditarPreMovimentoKitEstoqueItemModal.cshtml", viewModel);
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_SaidaProduto_Edit)]
        public async Task<ActionResult> CriarOuEditarPreMovimentoItemModal(long? id, long preMovimentoId = 0, long estoqueId = 0)
        {
            CriarOuEditarPreMovimentoItemModalViewModel viewModel;
            using (var produtoLaboratorioAppService = IocManager.Instance.ResolveAsDisposable<IProdutoLaboratorioAppService>())
            using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
            {
                var laboratorios = await produtoLaboratorioAppService.Object.ListarTodos().ConfigureAwait(false);

                if (id.HasValue && id.Value != 0)
                {
                    var model = await estoquePreMovimentoItemAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(model);
                    if (model != null)
                    {
                        viewModel.Hidden = (model.Produto.IsValidade || model.Produto.IsLote) ? "" : "hidden";

                        var unidades = await produtoAppService.Object.ObterUnidadePorProduto(model.ProdutoId).ConfigureAwait(false);
                        viewModel.Unidades = new SelectList(unidades.Items, "Id", "Descricao", model.ProdutoUnidadeId);

                        if (model.ProdutoUnidadeId != null)
                        {
                            var unidade = await unidadeAppService.Object.ObterUnidadeDto((long)model.ProdutoUnidadeId).ConfigureAwait(false);
                            if (unidade != null)
                            {
                                viewModel.Quantidade /= unidade.Fator;
                            }
                        }

                        var EstoquePreMovimentoLoteValidadeResult = await estoqueLoteValidadeAppService.Object.ListarPorPreMovimentoItem(new ListarEstoquePreMovimentoInput { Filtro = model.Id.ToString() }).ConfigureAwait(false);

                        if (EstoquePreMovimentoLoteValidadeResult != null && EstoquePreMovimentoLoteValidadeResult.Items.Count > 0)
                        {
                            var loteValidate = EstoquePreMovimentoLoteValidadeResult.Items[0].LoteValidade;
                            viewModel.LaboratorioId = loteValidate.ProdutoLaboratorioId;
                            viewModel.Lote = loteValidate.Lote;
                            viewModel.Validade = loteValidate.Validade;
                            viewModel.LoteValidadeId = loteValidate.Id;
                            viewModel.EstoquePreMovimentoLoteValidadeId = EstoquePreMovimentoLoteValidadeResult.Items[0].Id;

                            var lotesValidades = await estoqueLoteValidadeAppService.Object.ObterPorProdutoEstoquePorSaldo(model.ProdutoId, estoqueId, preMovimentoId).ConfigureAwait(false);
                            //var lotesValidades = await estoqueLoteValidadeAppService.Object.ObterPorProdutoEstoque(model.ProdutoId, estoqueId, preMovimentoId).ConfigureAwait(false);

                            viewModel.LotesValidades = new SelectList(lotesValidades, "Id", "Nome", loteValidate.Id);
                        }
                        else
                        {
                            viewModel.LotesValidades = new SelectList(new ListResultDto<LoteValidadeDto>().Items, "Id", "Nome");
                        }
                    }
                }
                else
                {
                    viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto())
                    {
                        Unidades = new SelectList(new ListResultDto<Unidade>().Items, "Id", "Descricao"),
                        Laboratorios = new SelectList(laboratorios.Items, "Id", "Descricao"),
                        LotesValidades = new SelectList(new ListResultDto<LoteValidadeDto>().Items, "Id", "Nome"),
                        Quantidade = 1
                    };
                }

                viewModel.Id = id.Value;
                viewModel.PreMovimentoId = preMovimentoId;
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Saidas/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
            }
        }


        public async Task<JsonResult> SelecionarLotesValidades(long produtoId, long estoqueId, long preMovimentoId)
        {
            using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
            {
                return Json(await estoqueLoteValidadeAppService.Object.ObterPorProdutoEstoquePorSaldo(produtoId, estoqueId, preMovimentoId).ConfigureAwait(false), JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> SelecionarAtendimento(long id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.ObterPacienteMedico(id).ConfigureAwait(false);
                return Json(atendimento, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> CarregarNumerosSeries(long produtoId, long estoqueId)
        {
            using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            {
                var numerosSeries = await estoquePreMovimentoItemAppService.Object.ObterNumerosSerieProduto(estoqueId, produtoId).ConfigureAwait(false);
                return Json(numerosSeries, JsonRequestBehavior.AllowGet);
            }
        }
    }
}