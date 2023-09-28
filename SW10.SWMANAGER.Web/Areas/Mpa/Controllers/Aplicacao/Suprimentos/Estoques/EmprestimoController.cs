using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.TiposAtendimento;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class EmprestimosController : Controller
    {
        public async Task<ActionResult> Entradas()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Entrada/index.cshtml", null);
        }

        public async Task<ActionResult> Saidas()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Saida/index.cshtml", null);
        }

        public async Task<ActionResult> CriarOuEditarEntradaModal(long? id)
        {
            using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            using (var estMovimentoBaixaAppService = IocManager.Instance.ResolveAsDisposable<IEstMovimentoBaixaAppService>())
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
            {
                var userId = abpSession.Object.UserId.Value;

                CriarOuEditarPreMovimentoModalViewModel viewModel;

                if (id.HasValue) //edição
                {
                    EstoquePreMovimentoDto output = await preMovimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                    {
                        PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false),
                        PossuiNota = estMovimentoBaixaAppService.Object.PossuiNota(output.Id),
                        PossuiVales = estMovimentoBaixaAppService.Object.PossuiVales(output.Id),
                        PossuiItensConsignados = estMovimentoBaixaAppService.Object.PossuiItemConsignados(output.Id)
                    };
                }
                else //Novo
                {
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel())
                    {
                        IsEntrada = true,
                        TiposAtendimentos = new SelectList(new List<TiposAtendimentoViewModel>()),
                        MotivosPerda = new SelectList(new List<MotivoPerdaProdutoDto>()),
                        EstTipoMovimentoId = (long)EnumTipoMovimento.Emprestimo_Entrada,
                        PreMovimentoEstadoId = (long) EnumPreMovimentoEstado.Emprestado
                    };

                    viewModel.TipoMovimento = EstoqueTipoMovimentoDto.MapearBase<EstoqueTipoMovimentoDto>(
                        await tipoMovimentoRepository.Object.GetAll().AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == viewModel.EstTipoMovimentoId).ConfigureAwait(false)
                    );

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
                viewModel.Movimento = DateTime.Now;
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Entrada/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> CriarOuEditarDevolucaoModal(long id)
        {
            using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            using (var estMovimentoBaixaAppService = IocManager.Instance.ResolveAsDisposable<IEstMovimentoBaixaAppService>())
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
            {
                var userId = abpSession.Object.UserId.Value;

                CriarOuEditarPreMovimentoModalViewModel viewModel;

                EstoquePreMovimentoDto output = await preMovimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                {
                    PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false),
                    PossuiNota = estMovimentoBaixaAppService.Object.PossuiNota(output.Id),
                    PossuiVales = estMovimentoBaixaAppService.Object.PossuiVales(output.Id),
                    PossuiItensConsignados = estMovimentoBaixaAppService.Object.PossuiItemConsignados(output.Id)
                };

                viewModel.Movimento = DateTime.Now;
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Devolucao/_CriarOuEditarModal.cshtml", viewModel);

            }
        }

        public async Task<ActionResult> CriarOuEditarSaidaModal(long? id)
        {
            using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            using (var estMovimentoBaixaAppService = IocManager.Instance.ResolveAsDisposable<IEstMovimentoBaixaAppService>())
            using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
            using (var tipoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoMovimento, long>>())
            {
                var userId = abpSession.Object.UserId.Value;

                CriarOuEditarPreMovimentoModalViewModel viewModel;

                if (id.HasValue) //edição
                {
                    EstoquePreMovimentoDto output = await preMovimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                    {
                        PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false),
                        PossuiNota = estMovimentoBaixaAppService.Object.PossuiNota(output.Id),
                        PossuiVales = estMovimentoBaixaAppService.Object.PossuiVales(output.Id),
                        PossuiItensConsignados = estMovimentoBaixaAppService.Object.PossuiItemConsignados(output.Id)
                    };
                }
                else //Novo
                {
                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel())
                    {
                        IsEntrada = false,
                        TiposAtendimentos = new SelectList(new List<TiposAtendimentoViewModel>()),
                        EstTipoMovimentoId = (long)EnumTipoMovimento.Emprestimo_Saida
                    };

                    viewModel.TipoMovimento = EstoqueTipoMovimentoDto.MapearBase<EstoqueTipoMovimentoDto>(
                        await tipoMovimentoRepository.Object.GetAll().AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == viewModel.EstTipoMovimentoId).ConfigureAwait(false)
                    );

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
                viewModel.Movimento = DateTime.Now;
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Saida/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Entrada_Edit)]
        public async Task<ActionResult> CriarOuEditarPreMovimentoItemEntradaModal(long? id, long preMovimentoId = 0)
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
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Entrada/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida_Create, AppPermissions.Pages_Tenant_Suprimentos_Estoque_Emprestimo_Saida_Edit)]
        public async Task<ActionResult> CriarOuEditarPreMovimentoItemSaidaModal(long? id, long preMovimentoId = 0, long estoqueId = 0)
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

                        var EstoquePreMovimentoLoteValidadeResult = await estoqueLoteValidadeAppService.Object
                            .ListarPorPreMovimentoItem(new ListarEstoquePreMovimentoInput
                            {
                                Filtro = model.Id.ToString()
                            }).ConfigureAwait(false);

                        if (EstoquePreMovimentoLoteValidadeResult != null && EstoquePreMovimentoLoteValidadeResult.Items.Count > 0)
                        {
                            var loteValidate = EstoquePreMovimentoLoteValidadeResult.Items[0].LoteValidade;
                            viewModel.LaboratorioId = loteValidate.ProdutoLaboratorioId;
                            viewModel.Lote = loteValidate.Lote;
                            viewModel.Validade = loteValidate.Validade;
                            viewModel.EstoquePreMovimentoLoteValidadeId = EstoquePreMovimentoLoteValidadeResult.Items[0].Id;

                            var lotesValidades = await estoqueLoteValidadeAppService.Object.ObterPorProdutoEstoque(model.ProdutoId, estoqueId, preMovimentoId).ConfigureAwait(false);

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
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Saida/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> CriarOuEditarDevolucaoPreMovimentoItemModal(string item)
        {
            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            using (var produtoLaboratorioAppService = IocManager.Instance.ResolveAsDisposable<IProdutoLaboratorioAppService>())
            using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            {
                CriarOuEditarPreMovimentoItemModalViewModel viewModel;
                var produtos = await produtoAppService.Object.ListarTodosParaMovimento().ConfigureAwait(false);
                EstoquePreMovimentoItemSolicitacaoDto preMovimentoItem = new EstoquePreMovimentoItemSolicitacaoDto();

                if (!string.IsNullOrEmpty(item))
                {
                    preMovimentoItem = JsonConvert.DeserializeObject<EstoquePreMovimentoItemSolicitacaoDto>(item);
                }


                //if (preMovimentoItem.IdGrid != null && preMovimentoItem.IdGrid != 0)
                //{
                var model = new EstoquePreMovimentoItemDto
                {
                    ProdutoId = preMovimentoItem.ProdutoId,
                    ProdutoUnidadeId = preMovimentoItem.ProdutoUnidadeId,
                    QuantidadeAtendida = preMovimentoItem.QuantidadeAtendida
                };//  await _estoquePreMovimentoItemAppService.Obter(id.Value);

                viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(model);
                if (model != null)
                {
                    var produto = await produtoAppService.Object.Obter(preMovimentoItem.ProdutoId).ConfigureAwait(false);
                    viewModel.Produto = new ProdutoDto
                    {
                        Id = preMovimentoItem.ProdutoId,
                        Descricao = preMovimentoItem.Produto,
                        Codigo = preMovimentoItem.CodigoProduto,
                        IsLote = produto.IsLote,
                        IsValidade = produto.IsValidade,
                        IsSerie = produto.IsSerie
                    };

                    if (produto.IsValidade || produto.IsLote)
                    {
                        var laboratorios = await produtoLaboratorioAppService.Object.ListarTodos().ConfigureAwait(false);
                        viewModel.Laboratorios = new SelectList(laboratorios.Items, "Id", "Descricao");
                    }
                    else
                    {
                        viewModel.Laboratorios = new SelectList(new ListResultDto<ProdutoLaboratorioDto>().Items, "Id", "Descricao");
                    }

                    if (preMovimentoItem.ProdutoUnidadeId.HasValue)
                    {
                        var unidade = await unidadeAppService.Object.Obter((long)preMovimentoItem.ProdutoUnidadeId).ConfigureAwait(false);
                        viewModel.ProdutoUnidade = unidade;
                    }

                    //  viewModel.Unidades = new SelectList(unidades.Items, "Id", "Descricao", model.ProdutoUnidadeId);
                    viewModel.Quantidade = preMovimentoItem.Quantidade;
                    viewModel.QuantidadeSolicitada = preMovimentoItem.QuantidadeSolicitada;

                    viewModel.IdGrid = preMovimentoItem.IdGrid;
                    viewModel.LotesValidadesJson = preMovimentoItem.LotesValidadesJson;
                    viewModel.NumerosSerieJson = preMovimentoItem.NumerosSerieJson;
                }
                //}
                //else
                //{
                //    viewModel = new CriarOuEditarPreMovimentoItemModalViewModel(new EstoquePreMovimentoItemDto())
                //    {
                //        // viewModel.Produtos = new SelectList(produtos.Items, "Id", "Descricao");
                //        Unidades = new SelectList(new ListResultDto<Unidade>().Items, "Id", "Descricao"),
                //        LotesValidades = new SelectList(new ListResultDto<LoteValidadeDto>().Items, "Id", "Nome")
                //    };
                //}
                viewModel.Id = preMovimentoItem.Id;
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Devolucao/_CriarOuEditarPreMovimentoItemModal.cshtml", viewModel);
            }
        }
    }
}