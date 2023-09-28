using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using NFe.Classes;
using NFe.Classes.Informacoes.Detalhe;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cfops.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NotasFiscais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Relatorios;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

#pragma warning disable CA3147 // Mark Verb Handlers With Validate Antiforgery Token
namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class PreMovimentosController : Controller // Web.Controllers.SWMANAGERControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var model = new PreMovimentoViewModel();

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            using (var contasPagarAppService = IocManager.Instance.ResolveAsDisposable<IContasPagarAppService>())
            using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            using (var estMovimentoBaixaAppService = IocManager.Instance.ResolveAsDisposable<IEstMovimentoBaixaAppService>())
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                var userId = abpSession.Object.UserId.Value;

                CriarOuEditarPreMovimentoModalViewModel viewModel;

                if (id.HasValue) //edição
                {
                    var output = await preMovimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                    {
                        PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false),
                        PossuiNota = estMovimentoBaixaAppService.Object.PossuiNota(output.Id),
                        PossuiVales = estMovimentoBaixaAppService.Object.PossuiVales(output.Id),
                        PossuiItensConsignados = estMovimentoBaixaAppService.Object.PossuiItemConsignados(output.Id),
                        Movimento = output.Movimento
                    };

                    if (output.EstTipoMovimentoId == (long)EnumTipoMovimento.NotaFiscal_Entrada)
                    {
                        if (viewModel.Fornecedor != null)
                        {
                            var documento = contasPagarAppService.Object.ObterPorPessoaNumero((long)viewModel.Fornecedor.SisPessoaId, output.Documento);

                            var lancamentos = new List<LancamentoIndex>();

                            if (documento != null)
                            {
                                foreach (var item in documento.LancamentosDto)
                                {
                                    var lancamento = new LancamentoIndex
                                    {
                                        AnoCompetencia = item.AnoCompetencia,
                                        MesCompetencia = item.MesCompetencia,
                                        CodigoBarras = item.CodigoBarras,
                                        Competencia = string.Concat(item.MesCompetencia.ToString().PadLeft(2, '0'), "/", item.AnoCompetencia),
                                        ValorAcrescimoDecrescimo = item.ValorAcrescimoDecrescimo,
                                        ValorLancamento = item.ValorLancamento,
                                        Juros = item.Juros,
                                        LinhaDigitavel = item.LinhaDigitavel,
                                        Multa = item.Multa,
                                        NossoNumero = item.NossoNumero,
                                        Parcela = item.Parcela,
                                        SituacaoLancamentoId = item.SituacaoLancamentoId,
                                        SituacaoDescricao = item.SituacaoDescricao,
                                        DataVencimento = item.DataVencimento,
                                        DataLancamento = item.DataLancamento,
                                        CorLancamentoFundo = item.CorLancamentoFundo,
                                        CorLancamentoLetra = item.CorLancamentoLetra,
                                        IdGrid = item.IdGrid,
                                        Id = item.Id
                                    };

                                    lancamentos.Add(lancamento);
                                }
                            }

                            viewModel.LancamentosJson = JsonConvert.SerializeObject(lancamentos);
                        }
                    }
                }
                else //Novo
                {
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel())
                    {
                        Serie = "1",
                        Movimento = DateTime.Now,
                        LancamentosJson = JsonConvert.SerializeObject(new List<LancamentoDto>())
                    };

                    using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
                    {
                        var userEmpresas = await userAppService.Object.GetUserEmpresas(userId).ConfigureAwait(false);

                        if (userEmpresas != null && userEmpresas.Items != null && userEmpresas.Items.Count == 1)
                        {
                            viewModel.Empresa = userEmpresas.Items[0];
                            viewModel.EmpresaId = userEmpresas.Items[0].Id;
                        }
                    }
                }
                
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Edit)]
        public async Task<ActionResult> CriarOuEditarPreMovimentoItemModal(long? id, long preMovimentoId = 0)
        {
            CriarOuEditarPreMovimentoItemModalViewModel viewModel;

            if (id.HasValue && id.Value != 0)
            {
                using (var _estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
                {
                    var model = await _estoquePreMovimentoItemAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(model);
                    if (model != null)
                    {
                        if (model.ProdutoUnidade != null)
                        {
                            viewModel.Quantidade /= model.ProdutoUnidade.Fator;
                        }
                        viewModel.CustoTotal = viewModel.Quantidade * viewModel.CustoUnitario;
                        //viewModel.ValorIPI = (viewModel.CustoTotal * viewModel.PerIPI) / 100;
                        viewModel.IsNumeroSerie = model.Produto.IsSerie;
                        viewModel.Produto = model.Produto;
                        viewModel.CodigoBarra = model.Produto.CodigoBarra;
                    }
                }
            }
            else
            {
                viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto());
            }
            viewModel.PreMovimentoId = preMovimentoId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Edit)]
        public async Task<ActionResult> InformarLoteValidadeModal(long preMovimentoId)
        {
            var viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto { PreMovimentoId = preMovimentoId });
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_InformarLoteValidade.cshtml", viewModel);
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Edit)]
        public async Task<ActionResult> InformarLoteValidadeProdutoModal(long preMovimentoItemId, long produtoId)
        {
            var viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto 
            { 
                PreMovimentoId = preMovimentoItemId, 
                ProdutoId = produtoId 
            });

            if (preMovimentoItemId != 0)
            {
                using (var _estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
                {
                    var preMovimentoItem = await _estoquePreMovimentoItemAppService.Object.Obter(preMovimentoItemId).ConfigureAwait(true);
                    if (preMovimentoItem != null)
                    {
                        using (var _preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                        {
                            preMovimentoItem.EstoquePreMovimento = await _preMovimentoAppService.Object.Obter(preMovimentoItem.PreMovimentoId).ConfigureAwait(false);
                            if (preMovimentoItem.EstoquePreMovimento != null)
                            {
                                viewModel.PreMovimentoEstadoId = preMovimentoItem.EstoquePreMovimento.PreMovimentoEstadoId;
                            }

                            viewModel.Quantidade = preMovimentoItem.Quantidade;
                        }
                    }
                }
            }

            if (produtoId != 0)
            {
                using (var _produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
                {
                    var produto = await _produtoAppService.Object.Obter(produtoId).ConfigureAwait(false);
                    if (produto != null)
                    {
                        viewModel.Produto = produto;
                    }
                }
            }
            
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_InformarLoteValidadeProduto.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Entrada_Edit)]
        public async Task<ActionResult> CriarOuEditarLoteValidadeModal(long preMovimentoItemId, long produtoLoteValidadeId = 0)
        {
            var viewModel = new CriarOuEditarEstoquePreMovimentoLoteValidadeDtoModalViewModel(new EstoquePreMovimentoLoteValidadeDto());

            using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            {
                var itemProduto = await estoquePreMovimentoItemAppService.Object.Obter(preMovimentoItemId).ConfigureAwait(false);

                if (itemProduto == null)
                {
                    return PartialView(
                        "~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_CriarOuEditarLoteValidadeModal.cshtml",
                        viewModel);
                }

                viewModel.Id = produtoLoteValidadeId;
                viewModel.ProdutoId = itemProduto.ProdutoId;
                if (itemProduto.Produto != null)
                {
                    viewModel.ProdutoDescricao = itemProduto.Produto.Descricao;
                    using (var estoquePreMovimentoLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoLoteValidadeAppService>())
                    {
                        var quantidade = await estoquePreMovimentoLoteValidadeAppService.Object.ObterQuantidadeRestanteLoteValidade(preMovimentoItemId).ConfigureAwait(false);
                        viewModel.Quantidade = quantidade;
                    }
                }

                if (produtoLoteValidadeId != 0)
                {
                    using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                    {
                        var loteValidade = estoqueLoteValidadeAppService.Object.Obter(produtoLoteValidadeId).Result;
                        if (loteValidade != null && loteValidade.LoteValidade != null)
                        {
                            viewModel.Lote = loteValidade.LoteValidade.Lote;
                            viewModel.Validade = loteValidade.LoteValidade.Validade;
                            viewModel.Quantidade = loteValidade.Quantidade;
                            viewModel.LaboratorioId = loteValidade.LoteValidade.ProdutoLaboratorioId;
                            viewModel.LoteValidade = loteValidade.LoteValidade;
                        }
                    }
                }

                viewModel.EstoquePreMovimentoItemId = preMovimentoItemId;
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_CriarOuEditarLoteValidadeModal.cshtml", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarPreMovimentacao(EstoquePreMovimentoDto input)
        {
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                var produto = preMovimentoAppService.Object.CriarOuEditar(input);
                return Json(produto, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarPreMovimentacaoItem(EstoquePreMovimentoItemDto input)
        {
            using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            {
                var produto = estoquePreMovimentoItemAppService.Object.CriarOuEditar(input);
                return Json(produto, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult EditarPreMovimentoItem(EstoquePreMovimentoItemDto input)
        {
            try
            {
                using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
                {
                    AsyncHelper.RunSync(() => estoquePreMovimentoItemAppService.Object.Editar(input));
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult ExcluirPreMovimentoItem(long id)
        {
            try
            {
                using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
                {
                    AsyncHelper.RunSync(() => estoquePreMovimentoItemAppService.Object.Excluir(id));
                    return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public async Task<JsonResult> SelecionarUnidades(int id)
        {
            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            {
                var unidades = await produtoAppService.Object.ObterUnidadePorProduto(id).ConfigureAwait(false);
                return Json(unidades, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public ActionResult Visualizar(long preMovimentoId)
        {
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            {
                var movimentoRelatorio = preMovimentoAppService.Object.ObterDadosRelatorioEntrada(preMovimentoId);
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/Entrada.aspx", new RelatorioEntradaModel(movimentoRelatorio));
            }
        }



        public async Task<ActionResult> BuscarLotesValidades(long preMovimentoId, string chave, long empresaId)
        {
            List<det> listDets;

            if (!string.IsNullOrEmpty(chave))
            {
                using (var _notaFiscalAppService = IocManager.Instance.ResolveAsDisposable<INotaFiscalAppService>())
                {
                    var nf = (await _notaFiscalAppService.Object.ObterNFeReceita(chave, empresaId).ConfigureAwait(false)).ReturnObject;
                    listDets = nf != null ? nf.NFe.infNFe.det : new List<det>();
                }
            }
            else
            {
                listDets = new List<det>();
            }

            using (var estoquePreMovimentoLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoLoteValidadeAppService>())
            {
                var lista = estoquePreMovimentoLoteValidadeAppService.Object.ObterLotesValidadesPreMovimento(preMovimentoId, listDets);

                var model = new InfornacaoLoteValidadeTodosModel
                {
                    InformacoesLoteValidade = lista
                };

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_InformacaoLoteValidadeTodosItens.cshtml", model);
            }
        }

        public async Task<JsonResult> BuscarNfe(string chave, long? empresaId, long? estoqueId, long? movimentoId)
        {
            var retornoPadrao = new DefaultReturn<EstoquePreMovimentoDto>();
            retornoPadrao.Errors = new List<ErroDto>();
            retornoPadrao.Warnings = new List<ErroDto>();


            if (empresaId == null || empresaId == 0)
            {
                retornoPadrao.Errors.Add(new ErroDto { Descricao = "Selecione uma empresa." });
            }

            if (string.IsNullOrEmpty(chave))
            {
                retornoPadrao.Errors.Add(new ErroDto { Descricao = "Chave da nota fiscal não informada." });
            }
            else
            {
                using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                {
                    var isChaveUtilizada = preMovimentoAppService.Object.ChaveNFeUtilizada(chave, movimentoId);

                    if (isChaveUtilizada)
                    {
                        retornoPadrao.Errors.Add(new ErroDto { Descricao = "Chave já utlizada." });
                    }
                }
            }

            if (estoqueId == null)
            {
                retornoPadrao.Errors.Add(new ErroDto { Descricao = "Selecione um estoque." });
            }

            if (retornoPadrao.Errors.Any())
            {
                return Json(retornoPadrao, JsonRequestBehavior.AllowGet);
            }
            try
            {
                using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
                using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
                using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                using (var estoqueImportacaoProdutoAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueImportacaoProdutoAppService>())
                using (var tipoFreteAppService = IocManager.Instance.ResolveAsDisposable<ITipoFreteAppService>())
                using (var fornecedorAppService = IocManager.Instance.ResolveAsDisposable<IFornecedorAppService>())
                using (var notaFiscalAppService = IocManager.Instance.ResolveAsDisposable<INotaFiscalAppService>())
                {
                    var retorno = await notaFiscalAppService.Object.ObterNFeReceita(chave, (long)empresaId).ConfigureAwait(false);
                    var preMovimento = new EstoquePreMovimentoDto();
                    if (retorno.ReturnObject == null || retorno.Errors.Count > 0)
                    {
                        retornoPadrao.Errors.AddRange(retorno.Errors);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(retorno.ReturnObject.NFe.infNFe.emit.CNPJ))
                        {
                            var fornecedorDto = await fornecedorAppService.Object.ObterPorCNPJ(retorno.ReturnObject.NFe.infNFe.emit.CNPJ).ConfigureAwait(false);
                            if (fornecedorDto == null)
                            {
                                retorno.Errors.Add(new ErroDto { Descricao = $"Fornecedor {retorno.ReturnObject.NFe.infNFe.emit.xNome}, CNPJ: {retorno.ReturnObject.NFe.infNFe.emit.CNPJ} não cadastrado." });
                                //return Json(retorno, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                preMovimento.FornecedorId = fornecedorDto.Id;
                                preMovimento.Fornecedor = new SisFornecedorDto { Id = fornecedorDto.Id, Descricao = fornecedorDto.SisPessoa.NomeFantasia };
                            }
                        }

                        var nf = retorno.ReturnObject;

                        preMovimento.Id = movimentoId ?? 0;

                        preMovimento.Documento = nf.NFe.infNFe.ide.nNF.ToString();
                        preMovimento.Emissao = nf.NFe.infNFe.ide.dhEmi;
                        preMovimento.Serie = nf.NFe.infNFe.ide.serie.ToString();
                        preMovimento.ValorICMS = nf.NFe.infNFe.total.ICMSTot.vICMS;
                        preMovimento.TotalDocumento = nf.NFe.infNFe.total.ICMSTot.vNF;
                        preMovimento.TotalProduto = nf.NFe.infNFe.total.ICMSTot.vProd;
                        preMovimento.DescontoPer = nf.NFe.infNFe.total.ICMSTot.vDesc;
                        preMovimento.AcrescimoDecrescimo = nf.NFe.infNFe.total.ICMSTot.vOutro;
                        preMovimento.ValorFrete = nf.NFe.infNFe.total.ICMSTot.vFrete;
                        preMovimento.TipoFreteId = (long?) nf.NFe.infNFe.transp.modFrete;

                        preMovimento.EstTipoMovimentoId = (long)EnumTipoMovimento.NotaFiscal_Entrada;
                        preMovimento.Movimento = DateTime.Now;
                        preMovimento.EmpresaId = empresaId;
                        preMovimento.Chave = chave;
                        preMovimento.EstoqueId = estoqueId;

                        if (nf.NFe.infNFe.transp != null && nf.NFe.infNFe.transp.transporta != null && nf.NFe.infNFe.transp.transporta.CNPJ != null)
                        {
                            var transportadora = await fornecedorAppService.Object.ObterPorCNPJ(nf.NFe.infNFe.transp.transporta.CNPJ).ConfigureAwait(false); ;
                            if (transportadora != null)
                            {
                                preMovimento.Frete_FornecedorId = transportadora.Id;
                                preMovimento.Frete_Forncedor = new FornecedorDto { Id = transportadora.Id, Descricao = transportadora.SisPessoa.NomeFantasia };
                            }
                        }
                        if (nf.NFe.infNFe.transp.modFrete.HasValue)
                        {
                            preMovimento.TipoFrete = await tipoFreteAppService.Object.Obter(((long)nf.NFe.infNFe.transp.modFrete)).ConfigureAwait(false); ;
                        }
                        preMovimento.CNPJNota = nf.NFe.infNFe.emit.CNPJ;


                        var importacaoProdutos = estoqueImportacaoProdutoAppService.Object.ObterListaImportacaoProduto(nf);

                        preMovimento.ImportacaoProdutos = importacaoProdutos.Where(w => w.ProdutoId == null).ToList();

                        if (nf.NFe.infNFe.det.Count > 0)
                        {
                            using (var _CFOPAppService = IocManager.Instance.ResolveAsDisposable<ICfopAppService>())
                            {
                                var cfop = await _CFOPAppService.Object.ObterPorNumero(nf.NFe.infNFe.det[0].prod.CFOP).ConfigureAwait(false); ;
                                if (cfop != null)
                                {
                                    preMovimento.CFOPId = cfop.Id;
                                    preMovimento.CFOP = new CfopDto { Id = cfop.Id, Descricao = cfop.Descricao };
                                }
                            }
                        }

                        retornoPadrao.Errors.AddRange(retorno.Errors);

                        long preMovimentoId = 0;

                        if (retornoPadrao.Errors.Count == 0 && preMovimento.ImportacaoProdutos.Count == 0)
                        {
                            preMovimentoId = preMovimentoAppService.Object.CriarGetIdEntrada(preMovimento).Id;
                            preMovimento.Id = preMovimentoId;

                            var items = new List<EstoquePreMovimentoItem>();

                            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                            {
                                items = estoquePreMovimentoItemRepository.Object.GetAll().AsNoTracking()
                                    .Include(x => x.EstoquePreMovimento)
                                    .Where(x => x.PreMovimentoId == preMovimento.Id).ToList();
                            }

                            foreach (var importacaoProduto in importacaoProdutos)
                            {
                                var preMovimentoItem = new EstoquePreMovimentoItemDto
                                {
                                    PreMovimentoId = preMovimentoId,
                                    Quantidade = importacaoProduto.Quantidade,
                                    ProdutoUnidadeId = importacaoProduto.UnidadeId,
                                    ProdutoId = (long)importacaoProduto.ProdutoId,
                                    CustoUnitario = importacaoProduto.CustoUnitario,
                                    EstoqueId = preMovimento.EstoqueId ?? 0,
                                    ValorIPI = importacaoProduto.ValorIPI,
                                    PerIPI = importacaoProduto.PercentualIPI,
                                    PerICMS = importacaoProduto.PercentualICMS,
                                    ValorICMS = importacaoProduto.ValorICMS
                                };

                                var preMovimentoDatabase = items.FirstOrDefault(x => x.PreMovimentoId == preMovimentoItem.PreMovimentoId
                                && x.Quantidade == preMovimentoItem.Quantidade
                                && x.ProdutoUnidadeId == preMovimentoItem.ProdutoUnidadeId
                                && x.ProdutoId == preMovimentoItem.ProdutoId
                                && x.CustoUnitario == preMovimentoItem.CustoUnitario
                                && preMovimento.EstoqueId != 0 && x.EstoquePreMovimento.EstoqueId == preMovimentoItem.EstoqueId
                                );

                                if (preMovimentoDatabase != null)
                                {
                                    preMovimento.Id = preMovimentoDatabase.Id;
                                }

                                var produto = await produtoAppService.Object.Obter((long)importacaoProduto.ProdutoId).ConfigureAwait(false);

                                if (produto != null && produto.IsSerie)
                                {
                                    preMovimentoItem.NumeroSerie = importacaoProduto.Serie;
                                }

                                var preMovimentoItemResult = await estoquePreMovimentoItemAppService.Object.CriarOuEditar(preMovimentoItem).ConfigureAwait(false);

                                retornoPadrao.Errors.AddRange(preMovimentoItemResult.Errors);


                                if (preMovimentoItemResult.ReturnObject != null && produto != null && (produto.IsValidade || produto.IsLote))
                                {
                                    await InserirLoteValidade(importacaoProduto, preMovimentoItemResult.ReturnObject).ConfigureAwait(false);
                                }
                            }
                        }

                        InserirFaturas(preMovimento, nf);

                        if (retornoPadrao.Errors.Count > 0)
                        {
                            await estoquePreMovimentoItemAppService.Object.ExcluirPorPreMovimento(preMovimentoId).ConfigureAwait(false);
                            await preMovimentoAppService.Object.Excluir(preMovimento).ConfigureAwait(false);
                        }
                    }
                    retornoPadrao.ReturnObject = preMovimento;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }
            return Json(retornoPadrao, JsonRequestBehavior.AllowGet);
        }

        async Task InserirLoteValidade(EstoqueImportacaoProdutoDto importacaoProduto, EstoquePreMovimentoItemDto preMovimentoItem)
        {
            if (importacaoProduto.Rastros != null)
            {
                using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                {
                    foreach (var item in importacaoProduto.Rastros)
                    {
                        var estoquePreMovimentoLoteValidade = new EstoquePreMovimentoLoteValidadeDto
                        {
                            EstoquePreMovimentoItemId = preMovimentoItem.Id,
                            Lote = item.Lote,
                            Validade = item.Validade,
                            ProdutoId = (long)importacaoProduto.ProdutoId,
                            Quantidade = item.Quantidade * (importacaoProduto.Fator ?? 1),
                        };

                        estoqueLoteValidadeAppService.Object.CriarOuEditar(estoquePreMovimentoLoteValidade);
                    }
                }
            }
        }

        void InserirFaturas(EstoquePreMovimentoDto preMovimento, nfeProc nfeProc)
        {
            var lancamentos = new List<LancamentoIndex>();
            var i = 1;
            if (nfeProc.NFe.infNFe.cobr == null || nfeProc.NFe.infNFe.cobr.dup.IsNullOrEmpty())
            {
                return;
            }
            foreach (var item in nfeProc.NFe.infNFe.cobr.dup)
            {
                var lancamento = new LancamentoIndex
                {
                    IdGrid = i,
                    AnoCompetencia = preMovimento.Movimento.Year,
                    MesCompetencia = preMovimento.Movimento.Month,
                    Competencia = string.Concat(preMovimento.Movimento.Month.ToString().PadLeft(2, '0'), "/", preMovimento.Movimento.Year),
                    ValorLancamento = item.vDup,
                    NossoNumero = string.Concat(nfeProc.NFe.infNFe.cobr.fat.nFat, "/", item.nDup),
                    Parcela = i++,
                    SituacaoLancamentoId = (long) EnumSituacaoLancamento.Aberto,
                    SituacaoDescricao = "Aberto",
                    DataVencimento = item.dVenc,
                    DataLancamento = preMovimento.Emissao
                };
                lancamentos.Add(lancamento);
            }

            preMovimento.LancamentosJson = JsonConvert.SerializeObject(lancamentos);
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        [ValidateInput(false)]
        public ActionResult CarregarRelacionarImportacaoProdutos(string importacaoProdutosRegistrados, long fornecedorId, string CNPJNota) //List<EstoqueImportacaoProdutoDto> importacaoProdutosRegistrados)
        {
            var importacaoProdutos = JsonConvert.DeserializeObject<List<EstoqueImportacaoProdutoDto>>(importacaoProdutosRegistrados);

            var i = 0;
            importacaoProdutos.ForEach(f => f.Index = i++);

            var importacaoProdutosViewModel = new ImportacaoProdutosViewModel
            {
                ImportacaoProdutos = importacaoProdutos, FornecedorId = fornecedorId, CNPJNota = CNPJNota
            };

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/PreMovimentos/_ImportacaoProdutos.cshtml", importacaoProdutosViewModel);//  importacaoProdutos);
        }

        public async Task<JsonResult> Salvar(string input)
        {
            try
            {
                var preMovimento = JsonConvert.DeserializeObject<EstoquePreMovimentoDto>(input);
                using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                {
                    var result = await preMovimentoAppService.Object.CriarOuEditar(preMovimento).ConfigureAwait(false);

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var mensagem = ex.Message;

                var retornoPadrao = new DefaultReturn<EstoquePreMovimentoDto>
                {
                    Warnings = new List<ErroDto>(),
                    Errors = new List<ErroDto>()
                };
                retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "EX0001", Descricao = mensagem });

                return Json(retornoPadrao, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
#pragma warning restore CA3147 // Mark Verb Handlers With Validate Antiforgery Token