using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Runtime.Session;
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using Microsoft.Reporting.WebForms;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Sexos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.JasperRelatorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Relatorios;
using SW10.SWMANAGER.Web.Relatorios.Suprimento;
using SW10.SWMANAGER.Web.Relatorios.Suprimento.SaldoProduto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using SW10.SWMANAGER.Helpers;
using System.Configuration;
using System.Web;
using HeyRed.Mime;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var model = new ProdutosViewModel();

            #region filtroPrincipal
            List<GenericoIdNome> filtroPrincipal = new List<GenericoIdNome>();
            var opcaoPrincipal1 = new GenericoIdNome();
            opcaoPrincipal1.Id = 1;
            opcaoPrincipal1.Nome = "Sim";
            filtroPrincipal.Add(opcaoPrincipal1);

            var opcaoPrincipal2 = new GenericoIdNome();
            opcaoPrincipal2.Id = 2;
            opcaoPrincipal2.Nome = "Não";
            filtroPrincipal.Add(opcaoPrincipal2);
            #endregion

            #region filtroStatus
            List<GenericoIdNome> filtroStatus = new List<GenericoIdNome>();
            var opcaoStatus1 = new GenericoIdNome();
            opcaoStatus1.Id = 1;
            opcaoStatus1.Nome = "Ativo";
            filtroStatus.Add(opcaoStatus1);

            var opcaoStatus2 = new GenericoIdNome();
            opcaoStatus2.Id = 2;
            opcaoStatus2.Nome = "Inativo";
            filtroStatus.Add(opcaoStatus2);
            #endregion

            model.FiltroPrincipais = new SelectList(filtroPrincipal, "Id", "Nome");
            model.FiltroStatus = new SelectList(filtroStatus, "Id", "Nome");

            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/Index.cshtml", model);
        }

        /// <summary>
        /// Prepara E exibe cadastro para Novo/Ediçao de Produto
        /// </summary>
        [AcceptVerbs("GET", "POST", "PUT")]
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Suprimentos_Produto_Create, AppPermissions.Pages_Tenant_Cadastros_Suprimentos_Produto_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoModalViewModel viewModel;


            using (var _unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            using (var _produtoGrupoAppService = IocManager.Instance.ResolveAsDisposable<IGrupoAppService>())
            using (var _produtoClasseAppService = IocManager.Instance.ResolveAsDisposable<IGrupoClasseAppService>())
            using (var _produtoSubClasseAppService = IocManager.Instance.ResolveAsDisposable<IGrupoSubClasseAppService>())
            using (var _sexoAppService = IocManager.Instance.ResolveAsDisposable<ISexoAppService>())
            using (var _produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            {
                var generos = await _sexoAppService.Object.ListarTodos();
                var unidadesreferenciais = await _unidadeAppService.Object.ListarUnidadesReferenciais();
                var unidadesgerenciais = new ListResultDto<UnidadeDto>();
                var grupos = await _produtoGrupoAppService.Object.ListarTodos();
                var classes = new ListResultDto<GrupoClasseDto>();
                var subClasses = new ListResultDto<GrupoSubClasseDto>();

                #region etiquetas
                List<EtiquetaDto> etiquetas = new List<EtiquetaDto>();
                var lista = new EtiquetaDto();
                lista.Id = 1;
                lista.Descricao = "Sistema";
                etiquetas.Add(lista);

                var lista2 = new EtiquetaDto();
                lista2.Id = 2;
                lista2.Descricao = "Fornecedor";
                etiquetas.Add(lista2);

                var lista3 = new EtiquetaDto();
                lista3.Id = 3;
                lista3.Descricao = "Não utilizar";
                etiquetas.Add(lista3);


                #endregion
                //-----------------------------------------------------------------------------------------------------

                if (id.HasValue)
                {
                    #region EdicaoProduto




                    var output = await _produtoAppService.Object.Obter((long)id);
                    viewModel = new CriarOuEditarProdutoModalViewModel(output);

                    //var produtoAtual = produtosprincipais.Items.FirstOrDefault(p => p.Id == id);
                    //List<GenericoIdNome> prods = new List<GenericoIdNome>();
                    //prods.Add(produtoAtual);
                    //var produtosMestresEdicao = produtosprincipais.Items.Except(prods);

                    ////viewModel.EtiquetaId = 2;
                    //viewModel.EstoqueLocalizacaoId = 1;
                    //viewModel.DCBs = new SelectList(dcbs.Items, "Id", "Nome", output.DCBId);
                    viewModel.Etiquetas = new SelectList(etiquetas, "Id", "Descricao", output.EtiquetaId);
                    viewModel.Generos = new SelectList(generos.Items, "Id", "Descricao", output.Genero);
                    //viewModel.ProdutosPrincipais = new SelectList(produtosMestresEdicao, "Id", "Nome", output.ProdutoPrincipalId);

                    viewModel.Grupos = new SelectList(grupos.Items, "Id", "Descricao", output.GrupoId);
                    if (viewModel.GrupoId != null)
                    {
                        classes = await _produtoClasseAppService.Object.ListarPorGrupo(viewModel.GrupoId);
                        viewModel.Classes = new SelectList(classes.Items, "Id", "Descricao", output.GrupoClasseId);
                    }

                    if (viewModel.GrupoClasseId != null)
                    {
                        subClasses = await _produtoSubClasseAppService.Object.ListarPorClasse((long)viewModel.GrupoClasseId);
                        viewModel.SubClasses = new SelectList(subClasses.Items, "Id", "Descricao", output.GrupoSubClasseId);
                    }
                    else
                    {
                        viewModel.SubClasses = new SelectList(subClasses.Items, "Id", "Descricao");
                    };

                    viewModel.UnidadeReferencial = await _produtoAppService.Object.ObterUnidadePorTipo(viewModel.Id, 1);
                    if (viewModel.UnidadeReferencial != null)
                    {
                        viewModel.UnidadeReferencialId = viewModel.UnidadeReferencial.Id;
                    }
                    viewModel.UnidadesReferenciais = new SelectList(unidadesreferenciais.Items, "Id", "Descricao", viewModel.UnidadeReferencialId);
                    //viewModel.UnidadesReferenciais = new SelectList(unidadesreferenciais.Items, "Sigla", "Descricao", viewModel.UnidadeReferencialId);

                    viewModel.UnidadeGerencial = await _produtoAppService.Object.ObterUnidadePorTipo(viewModel.Id, 2);
                    viewModel.UnidadeGerencialId = viewModel.UnidadeGerencial.Id;
                    unidadesgerenciais = await _unidadeAppService.Object.ListarPorReferencial(viewModel.UnidadeReferencialId, true);
                    viewModel.UnidadesGerenciais = new SelectList(unidadesgerenciais.Items, "Id", "Descricao", viewModel.UnidadeGerencialId);

                    #endregion EdicaoProduto
                }
                else
                {
                    viewModel = new CriarOuEditarProdutoModalViewModel(new ProdutoDto());
                    viewModel.EstoqueLocalizacaoId = 1;
                    viewModel.Generos = new SelectList(generos.Items, "Id", "Descricao");
                    viewModel.Etiquetas = new SelectList(etiquetas, "Id", "Descricao");
                    viewModel.Grupos = new SelectList(grupos.Items, "Id", "Descricao");
                    viewModel.Classes = new SelectList(classes.Items, "Id", "Descricao");
                    viewModel.SubClasses = new SelectList(subClasses.Items, "Id", "Descricao");
                    viewModel.UnidadesReferenciais = new SelectList(unidadesreferenciais.Items, "Id", "Descricao");
                    unidadesgerenciais = await _unidadeAppService.Object.ListarPorReferencial(viewModel.Id, true);
                    viewModel.UnidadesGerenciais = new SelectList(unidadesgerenciais.Items.Select(m => new { UnidadeReferencialId = m.Id, Descricao = m.Descricao }), "UnidadeReferencialId", "Descricao");
                }

                return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        /// <summary>
        /// Retona Json com Id e Descricao de Classes para um determinado Grupo
        /// </summary>
        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult GetClasses(long id)
        {
            try
            {
                if (id > 0)
                {

                    using (var produtoClasseAppService = IocManager.Instance.ResolveAsDisposable<IGrupoClasseAppService>())
                    {

                        var classes = AsyncHelper.RunSync(() => produtoClasseAppService.Object.ListarPorGrupo(id));

                        var lista = classes.Items.ToList().Select(
                            c => new { DisplayText = c.Id, Value = c.Descricao });

                        return Json(new { Result = "OK", Options = lista }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Result = "OK", Options = "0" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Retona Json com Id e Descricao de SubClasses para uma determinada Classe
        /// </summary>
        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult GetSubClasses(long id)
        {
            try
            {
                if (id > 0)
                {
                    using (var produtoSubClasseAppService = IocManager.Instance.ResolveAsDisposable<IGrupoSubClasseAppService>())
                    {
                        var subClasses = AsyncHelper.RunSync(() => produtoSubClasseAppService.Object.ListarPorClasse(id));

                        var lista = subClasses.Items.ToList().Select(
                            c => new { DisplayText = c.Id, Value = c.Descricao });

                        return Json(new { Result = "OK", Options = lista }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Result = "OK", Options = "0" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Retorna Json com Id e Descricao de Unidades. Parametro Id = id da classe referencial
        /// </summary>
        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult GetUnidadesPorReferencia(long id, bool addPai)
        {
            try
            {
                if (id > 0)
                {
                    using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                    {
                        var unidades = AsyncHelper.RunSync(() => unidadeAppService.Object.ListarPorReferencial(id, addPai));
                        var lista = unidades.Items.ToList().Select(
                            c => new { DisplayText = c.Id, Value = c.Descricao });

                        return Json(new { Result = "OK", Options = lista }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Result = "OK", Options = "0" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        //Controllers das Abas
        //------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<ActionResult> CriarOuEditarProdutoEstoqueModal(long? id, long idProduto = 0)
        {
            var _id = id != null ? (long)id : 0;

            using (var produtoEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IProdutoEstoqueAppService>())
            {
                var estoques = produtoEstoqueAppService.Object.ListarTodosNaoRelacionadosProdutos(idProduto, _id);//_estoqueAppService.ListarTodos();

                CriarOuEditarProdutoEstoqueModalViewModel viewModel = new CriarOuEditarProdutoEstoqueModalViewModel(new ProdutoEstoqueDto());

                if (id.HasValue)
                {
                    var output = await produtoEstoqueAppService.Object.Obter((long)id);

                    viewModel = new CriarOuEditarProdutoEstoqueModalViewModel(output)
                    {
                        Estoques = new SelectList(estoques.Items, "Id", "Descricao", output.EstoqueId)
                    };
                }
                else
                {
                    viewModel = new CriarOuEditarProdutoEstoqueModalViewModel(new ProdutoEstoqueDto());
                    viewModel.ProdutoId = idProduto;
                    viewModel.Estoques = new SelectList(estoques.Items, "Id", "Descricao");
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoEstoqueModal.cshtml", viewModel);
            }


        }

        public async Task<ActionResult> CriarOuEditarProdutoUnidadeModal(long? id, long? unidadeReferencialId, long? unidadeGerencialId, long idProduto)
        {
            //-------------------------------------------------------------------------------------------------------------
            //popula combo de unidades. Lista unidades relacionadas a uma unidade referencial

            using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            using (var tipoUnidadeAppService = IocManager.Instance.ResolveAsDisposable<ITipoUnidadeAppService>())
            using (var produtoUnidadeAppService = IocManager.Instance.ResolveAsDisposable<IProdutoUnidadeAppService>())
            {
                var unidadesPrincipais = await unidadeAppService.Object.ListarPorReferencial(unidadeReferencialId, true);
                var unidadesLista = unidadesPrincipais.Items.ToList();


                var unidadesUtilizadas = await produtoAppService.Object.ObterUnidadePorProduto(idProduto, false);
                List<UnidadeDto> unidadesUtilizadasList = new List<UnidadeDto>();
                unidadesUtilizadasList = unidadesUtilizadas.Items.ToList();
                List<UnidadeDto> unidades = new List<UnidadeDto>();

                //------------

                var tiposunidades = await tipoUnidadeAppService.Object.ListarTodos();

                //------------

                CriarOuEditarProdutoUnidadeModalViewModel viewModel;

                var _unidadeReferencialId = (long)unidadeReferencialId;

                if (id.HasValue)
                {

                    var output = await produtoUnidadeAppService.Object.Obter((long)id);
                    viewModel = new CriarOuEditarProdutoUnidadeModalViewModel(output);
                    viewModel.UnidadeReferencial = await unidadeAppService.Object.ObterUnidadeDto(_unidadeReferencialId);

                    unidades = unidadesLista
                                .Where(m => !m.Id.IsIn(unidadesUtilizadasList.Select(p => p.Id).ToArray()))
                                .ToList();

                    //viewModel.IsAtivo = output.IsAtivo;
                    unidades.Add(new UnidadeDto { Id = output.UnidadeId, Descricao = output.Unidade?.Descricao });

                    viewModel.Unidades = new SelectList(unidades, "Id", "Descricao", output.UnidadeId);
                    viewModel.TiposUnidades = new SelectList(tiposunidades.Items, "Id", "Descricao", output.UnidadeTipoId);
                }
                else
                {
                    unidades = unidadesLista.Where(m => !m.Id.IsIn(unidadesUtilizadasList.Select(p => p.Id).ToArray())).ToList();

                    viewModel = new CriarOuEditarProdutoUnidadeModalViewModel(new ProdutoUnidadeDto());
                    viewModel.ProdutoId = idProduto;
                    viewModel.UnidadeReferencial = await unidadeAppService.Object.ObterUnidadeDto(_unidadeReferencialId);

                    viewModel.Unidades = new SelectList(unidades, "Id", "Descricao");
                    viewModel.TiposUnidades = new SelectList(tiposunidades.Items, "Id", "Descricao");
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoUnidadeModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> CriarOuEditarProdutoEmpresaModal(long? id, long idProduto)
        {
            //-------------------------------------------------------------------------------------------------------------
            using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var produtoEmpresaAppService = IocManager.Instance.ResolveAsDisposable<IProdutoEmpresaAppService>())
            {
                var userId = abpSession.Object.UserId.Value;
                var userEmpresas = await userAppService.Object.GetUserEmpresas(userId);
                CriarOuEditarProdutoEmpresaModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await produtoEmpresaAppService.Object.Obter((long)id);
                    viewModel = new CriarOuEditarProdutoEmpresaModalViewModel(output)
                    {
                        Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia", output.EmpresaId)
                    };
                }
                else
                {
                    viewModel = new CriarOuEditarProdutoEmpresaModalViewModel(new ProdutoEmpresaDto())
                    {
                        Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia"),
                        ProdutoId = idProduto
                    };
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoEmpresaModal.cshtml", viewModel);
            }
        }

        /// <summary>
        ///  id = id do produto        
        /// </summary>
        public async Task<ActionResult> ProdutoSaldoDetalhadoModal(long id, long idEstoque)
        {
            using (var estoqueAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueAppService>())
            {
                var estoque = await estoqueAppService.Object.Obter(idEstoque);

                CriarOuEditarProdutoSaldoViewModel viewModel;
                viewModel = new CriarOuEditarProdutoSaldoViewModel(new ProdutoSaldoDto())
                {
                    ProdutoId = id,
                    EstoqueId = idEstoque,
                    Estoque = estoque
                };

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_SaldoDetalhado.cshtml", viewModel);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<ActionResult> CriarOuEditarProdutoListaSubstituicaoModal(long? id, long idProduto)
        {
            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            using (var produtoListaSubstituicaoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoListaSubstituicaoAppService>())
            {
                var produtos = await produtoAppService.Object.ListarTodos();

                ProdutoListaSubstituicaoModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await produtoListaSubstituicaoAppService.Object.Obter((long)id);

                    viewModel = new ProdutoListaSubstituicaoModalViewModel(output)
                    {
                        Produtos = new SelectList(produtos.Items, "Id", "Descricao", output.ProdutoSubstituicaoId)
                    };
                }
                else
                {
                    viewModel = new ProdutoListaSubstituicaoModalViewModel(new ProdutoListaSubstituicaoDto())
                    {
                        ProdutoId = idProduto,
                        Produtos = new SelectList(produtos.Items, "Id", "Descricao")
                    };
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoListaSubstituicaoModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> CriarOuEditarProdutoRelacaoLaboratorioModal(long? id, long idProduto)
        {
            using (var produtoLaboratorioAppService = IocManager.Instance.ResolveAsDisposable<IProdutoLaboratorioAppService>())
            using (var produtoRelacaoLaboratorioAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoLaboratorioAppService>())
            {
                var laboratorios = await produtoLaboratorioAppService.Object.ListarTodos();

                CriarOuEditarProdutoRelacaoLaboratorioModalViewModel viewModel;

                if (id.HasValue)
                {

                    var output = await produtoRelacaoLaboratorioAppService.Object.Obter((long)id);

                    viewModel = new CriarOuEditarProdutoRelacaoLaboratorioModalViewModel(output)
                    {
                        Laboratorios = new SelectList(laboratorios.Items, "Id", "Descricao", output.ProdutoLaboratorio)
                    };
                }
                else
                {
                    viewModel = new CriarOuEditarProdutoRelacaoLaboratorioModalViewModel(new ProdutoRelacaoLaboratorioDto())
                    {
                        ProdutoId = idProduto,
                        Laboratorios = new SelectList(laboratorios.Items, "Id", "Descricao")
                    };
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoRelacaoLaboratorioModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> CriarOuEditarProdutoRelacaoPalavraChaveModal(long? id, long idProduto)
        {
            using (var produtoPalavraChaveAppService = IocManager.Instance.ResolveAsDisposable<IProdutoPalavraChaveAppService>())
            using (var produtoRelacaoPalavraChaveAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoPalavraChaveAppService>())
            {
                var palavraschaves = await produtoPalavraChaveAppService.Object.ListarTodos();

                CriarOuEditarProdutoRelacaoPalavraChaveModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await produtoRelacaoPalavraChaveAppService.Object.Obter((long)id);

                    viewModel = new CriarOuEditarProdutoRelacaoPalavraChaveModalViewModel(output);
                    viewModel.PalavrasChaves = new SelectList(palavraschaves.Items, "Id", "Palavra", output.ProdutoPalavraChave);
                }
                else
                {
                    viewModel = new CriarOuEditarProdutoRelacaoPalavraChaveModalViewModel(new ProdutoRelacaoPalavraChaveDto());
                    viewModel.ProdutoId = idProduto;
                    viewModel.PalavrasChaves = new SelectList(palavraschaves.Items, "Id", "Palavra");
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoRelacaoPalavraChaveModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> CriarOuEditarProdutoRelacaoAcaoTerapeuticaModal(long? id, long idProduto)
        {
            using (var produtoAcaoTerapeuticaAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAcaoTerapeuticaAppService>())
            using (var produtoRelacaoAcaoTerapeuticaAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoAcaoTerapeuticaAppService>())
            {
                var acoesterapeuticas = await produtoAcaoTerapeuticaAppService.Object.ListarTodos();

                CriarOuEditarProdutoRelacaoAcaoTerapeuticaModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await produtoRelacaoAcaoTerapeuticaAppService.Object.Obter((long)id);

                    viewModel = new CriarOuEditarProdutoRelacaoAcaoTerapeuticaModalViewModel(output)
                    {
                        AcoesTerapeuticas = new SelectList(acoesterapeuticas.Items, "Id", "Descricao", output.ProdutoAcaoTerapeutica)
                    };
                }
                else
                {
                    viewModel = new CriarOuEditarProdutoRelacaoAcaoTerapeuticaModalViewModel(new ProdutoRelacaoAcaoTerapeuticaDto())
                    {
                        ProdutoId = idProduto,
                        AcoesTerapeuticas = new SelectList(acoesterapeuticas.Items, "Id", "Descricao")
                    };
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Produtos/_CriarOuEditarProdutoRelacaoAcaoTerapeuticaModal.cshtml", viewModel);
            }
        }
        //------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<JsonResult> AutoComplete(string term)
        {
            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            {
                var query = await produtoAppService.Object.ListarAutoComplete(term);
                return Json(query.Items, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SalvarProduto(ProdutoDto input)
        {
            using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
            {
                var produto = produtoAppService.Object.CriarGetId(input);
                return Json(produto, JsonRequestBehavior.AllowGet);
            }
        }

        #region Unidade
        //------------------------------------------------------------------------------------------------------------------------
        // Unidade
        //------------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        public JsonResult EditarProdutoUnidadeTipo(ProdutoUnidadeDto input)
        {
            try
            {
                using (var produtoUnidadeTipoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoUnidadeTipoAppService>())
                {
                    AsyncHelper.RunSync(() => produtoUnidadeTipoAppService.Object.Editar(input));
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult CriarProdutoUnidadeTipo(ProdutoUnidadeDto input)
        {
            try
            {
                using (var produtoUnidadeTipoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoUnidadeTipoAppService>())
                {
                    var objUnidadeTipo = AsyncHelper.RunSync(() => produtoUnidadeTipoAppService.Object.CriarOuEditar(input, input.Id));
                    return Json(new { Result = "OK", Record = objUnidadeTipo }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }



        [HttpPost]
        public JsonResult ExcluirProdutoUnidadeTipo(ProdutoUnidadeDto input)
        {
            try
            {
                //input. ProdutoListaSubstituicaoDto
                using (var produtoUnidadeTipoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoUnidadeTipoAppService>())
                {
                    AsyncHelper.RunSync(() => produtoUnidadeTipoAppService.Object.Excluir(input));
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        public async Task<JsonResult> InserirUnidadeReferencia(int produtoId, int unidadeId, int tipoUnidade)
        {
            using (var produtoUnidadeTipoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoUnidadeTipoAppService>())
            {
                var produtoTipo = AsyncHelper.RunSync(() => produtoUnidadeTipoAppService.Object.ObterPorUnidadeProduto(unidadeId, produtoId));

                if (produtoTipo == null)
                {
                    ProdutoUnidadeDto input = new ProdutoUnidadeDto
                    {
                        ProdutoId = produtoId,
                        UnidadeId = unidadeId
                    };
                    AsyncHelper.RunSync(() => produtoUnidadeTipoAppService.Object.CriarOuEditar(input, 0));

                    return Json(input, JsonRequestBehavior.AllowGet);

                }
                return Json(produtoTipo, JsonRequestBehavior.AllowGet);
            }
        }


        #endregion

        //------------------------------------------------------------------------------------------------------------------------
        // Substituicao
        //------------------------------------------------------------------------------------------------------------------------
        #region Substituição
        [HttpPost]
        public JsonResult EditarProdutoListaSubstituicao(ProdutoListaSubstituicaoDto input)
        {
            try
            {
                using (var produtoListaSubstituicaoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoListaSubstituicaoAppService>())
                {
                    AsyncHelper.RunSync(() => produtoListaSubstituicaoAppService.Object.Editar(input));
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult CriarProdutoListaSubstituicao(ProdutoListaSubstituicaoDto input)
        {
            try
            {
                using (var produtoListaSubstituicaoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoListaSubstituicaoAppService>())
                {
                    var objResult = AsyncHelper.RunSync(() => produtoListaSubstituicaoAppService.Object.CriarOuEditar(input));
                    return Json(new { Result = "OK", Record = objResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public async Task<ActionResult> SalvarProdutoSubstituicao(ProdutoListaSubstituicaoDto produtoSubstituicao)
        {
            using (var produtoListaSubstituicaoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoListaSubstituicaoAppService>())
            {
                await produtoListaSubstituicaoAppService.Object.CriarOuEditar(produtoSubstituicao);
                return Content("Sucesso");
            }
        }

        [HttpPost]
        public JsonResult ExcluirProdutoSubstituicao(long id)
        {
            try
            {
                using (var produtoListaSubstituicaoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoListaSubstituicaoAppService>())
                {
                    AsyncHelper.RunSync(() => produtoListaSubstituicaoAppService.Object.Excluir(id));
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Portaria
        //------------------------------------------------------------------------------------------------------------------------
        // Portaria
        //------------------------------------------------------------------------------------------------------------------------
        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult EditarProdutoRelacaoPortaria(ProdutoRelacaoPortariaDto input)
        {
            try
            {
                using (var produtoRelacaoPortariaAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoPortariaAppService>())
                {
                    var objResult = AsyncHelper.RunSync(() => produtoRelacaoPortariaAppService.Object.CriarOuEditar(input));

                    //return Json(new { Result = "OK", Record = objTeste }, JsonRequestBehavior.AllowGet);
                    return Json(new { Result = "OK", Record = objResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult CriarProdutoRelacaoPortaria(ProdutoRelacaoPortariaDto input)
        {
            try
            {
                using (var produtoRelacaoPortariaAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoPortariaAppService>())
                {
                    var objResult = AsyncHelper.RunSync(() => produtoRelacaoPortariaAppService.Object.CriarOuEditar(input));
                    return Json(new { Result = "OK", Record = objResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public JsonResult ExcluirProdutoRelacaoPortaria(long id)
        {
            try
            {
                using (var produtoRelacaoPortariaAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoPortariaAppService>())
                {
                    AsyncHelper.RunSync(() => produtoRelacaoPortariaAppService.Object.Excluir(id));
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Laboratório

        //------------------------------------------------------------------------------------------------------------------------
        // Laboratorio
        //------------------------------------------------------------------------------------------------------------------------
        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult EditarProdutoRelacaoLaboratorio(ProdutoRelacaoLaboratorioDto input)
        {
            try
            {
                using (var produtoRelacaoLaboratorioAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoLaboratorioAppService>())
                {
                    var objResult = AsyncHelper.RunSync(() => produtoRelacaoLaboratorioAppService.Object.CriarOuEditar(input));
                    return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult CriarProdutoRelacaoLaboratorio(ProdutoRelacaoLaboratorioDto input)
        {
            try
            {
                using (var produtoRelacaoLaboratorioAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoLaboratorioAppService>())
                {
                    var objResult = AsyncHelper.RunSync(() => produtoRelacaoLaboratorioAppService.Object.CriarOuEditar(input));
                    return Json(new { Result = "OK", Record = objResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public JsonResult ExcluirProdutoRelacaoLaboratorio(long id)
        {
            try
            {
                using (var produtoRelacaoLaboratorioAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoLaboratorioAppService>())
                {
                    AsyncHelper.RunSync(() => produtoRelacaoLaboratorioAppService.Object.Excluir(id));
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        #region Palavra chave
        //------------------------------------------------------------------------------------------------------------------------
        // PalavraChave
        //------------------------------------------------------------------------------------------------------------------------
        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult EditarProdutoRelacaoPalavraChave(ProdutoRelacaoPalavraChaveDto input)
        {

            try
            {
                using (var produtoRelacaoPalavraChaveAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoPalavraChaveAppService>())
                {
                    var objResult = AsyncHelper.RunSync(() => produtoRelacaoPalavraChaveAppService.Object.CriarOuEditar(input));

                    return Json(new { Result = "OK", Record = objResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult CriarProdutoRelacaoPalavraChave(ProdutoRelacaoPalavraChaveDto input)
        {
            try
            {
                using (var produtoRelacaoPalavraChaveAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoPalavraChaveAppService>())
                {
                    var objResult = AsyncHelper.RunSync(() => produtoRelacaoPalavraChaveAppService.Object.CriarOuEditar(input));
                    return Json(new { Result = "OK", Record = objResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public JsonResult ExcluirProdutoRelacaoPalavraChave(long id)
        {
            try
            {
                using (var produtoRelacaoPalavraChaveAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoPalavraChaveAppService>())
                {
                    AsyncHelper.RunSync(() => produtoRelacaoPalavraChaveAppService.Object.Excluir(id));
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion



        #region Ação terapeutica
        //------------------------------------------------------------------------------------------------------------------------
        // AcaoTerapeutica
        //------------------------------------------------------------------------------------------------------------------------
        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult EditarProdutoRelacaoAcaoTerapeutica(ProdutoRelacaoAcaoTerapeuticaDto input)
        {
            try
            {
                using (var produtoRelacaoAcaoTerapeuticaAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoAcaoTerapeuticaAppService>())
                {
                    var objResult = AsyncHelper.RunSync(() => produtoRelacaoAcaoTerapeuticaAppService.Object.CriarOuEditar(input));

                    //return Json(new { Result = "OK", Record = objTeste }, JsonRequestBehavior.AllowGet);
                    return Json(new { Result = "OK", Record = objResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult CriarProdutoRelacaoAcaoTerapeutica(ProdutoRelacaoAcaoTerapeuticaDto input)
        {
            try
            {
                using (var produtoRelacaoAcaoTerapeuticaAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoAcaoTerapeuticaAppService>())
                {
                    var objResult = AsyncHelper.RunSync(() => produtoRelacaoAcaoTerapeuticaAppService.Object.CriarOuEditar(input));
                    return Json(new { Result = "OK", Record = objResult }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message, JsonRequestBehavior.AllowGet });
            }
        }

        [HttpPost]
        public JsonResult ExcluirProdutoRelacaoAcaoTerapeutica(long id)
        {
            try
            {
                using (var produtoRelacaoAcaoTerapeuticaAppService = IocManager.Instance.ResolveAsDisposable<IProdutoRelacaoAcaoTerapeuticaAppService>())
                {
                    AsyncHelper.RunSync(() => produtoRelacaoAcaoTerapeuticaAppService.Object.Excluir(id));
                    return Json(new { Result = "OK" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        #endregion

        //------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------------------

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult ListarNomeProdutosExcetoId(long id)
        {
            try
            {
                using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
                {
                    var unidades = AsyncHelper.RunSync(() => produtoAppService.Object.ListarProdutosExcetoId(id));
                    var lista = unidades.Items.ToList().Select(
                        c => new { DisplayText = c.DescricaoResumida, Value = c.Id });
                    return Json(new { Result = "OK", Options = lista }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult ListarCodigoProdutosExcetoId(long id)
        {
            try
            {
                if (id > 0)
                {
                    using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
                    {
                        var unidades = AsyncHelper.RunSync(() => produtoAppService.Object.ListarProdutosExcetoId(id));
                        var lista = unidades.Items.ToList().Select(
                            c => new { DisplayText = c.Id, Value = c.Id });
                        return Json(new { Result = "OK", Options = lista }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Result = "OK", Options = "0" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ContentResult VisualizarSaldoProdutoPDF(long empresaId, long grupoId, long estoqueId)
        {
            try
            {
                using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
                using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
                using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
                using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                {
                    var empresa = Task.Run(() => empresaAppService.Object.Obter(empresaId)).Result;
                    var usuarioEmpresas = Task.Run(() => userAppService.Object.GetUserEmpresas(abpSession.Object.UserId.Value)).Result;
                    var usuario = Task.Run(() => userAppService.Object.GetUser()).Result;
                    var loginInformations = Task.Run(() => sessionAppService.Object.GetCurrentLoginInformations()).Result;
                    //if (filtro.Respostas.Count() > 0)
                    //{
                    var dados = new Models.Aplicacao.Suprimentos.Relatorios.FiltroModel();
                    dados.Titulo = "Saldo por produto";
                    dados.NomeHospital = empresa.NomeFantasia;
                    dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                    dados.DataHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    //}

                    // Obtido do ASPX
                    ScriptManager scriptManager = new ScriptManager();
                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"\Relatorios\Suprimento\SaldoProduto\SaldoProduto.rdlc");

                    if (dados != null)
                    {
                        //parâmetros para o relatório
                        ReportParameter nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        ReportParameter nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                        ReportParameter titulo = new ReportParameter("Titulo", dados.Titulo);
                        ReportParameter data = new ReportParameter("Data", dados.DataHora);
                        ReportParameter dataHora = new ReportParameter("DataHora", dados.DataHora);

                        reportViewer.LocalReport.SetParameters(new ReportParameter[] {
                            nomeHospital,
                            nomeUsuario,
                            titulo,
                            data,
                            dataHora,
                        });

                        //fonte de dados para o relatório - datasource
                        var list = Task.Run(() => produtoAppService.Object.ListarProdutoSaldoReport(estoqueId, grupoId)).Result;
                        var listDto = list.Items;
                        var listDs = new List<ProdutoSaldoDsDto>();
                        foreach (var item in listDto)
                        {
                            var ds = new ProdutoSaldoDsDto();
                            ds.Produto = item.Produto;
                            ds.Grupo = item.Grupo;
                            ds.Estoque = item.Estoque;
                            ds.UnidadeGerencial = item.UnidadeGerencial;
                            ds.UnidadeReferencial = item.UnidadeReferencial;
                            ds.SaldoAtual = item.QuantidadeAtual.ToString(); //string.Concat(item.QuantidadeAtual, " ", item.UnidadeReferencia, " / ", item.QuantidadeGerencialAtual, " ", item.UnidadeGerencial);
                            ds.EntradaPendente = item.QuantidadeEntradaPendente.ToString(); //string.Concat(item.QuantidadeEntradaPendente, " ", item.UnidadeReferencia, " / ", item.QuantidadeGerencialEntradaPendente, " ", item.UnidadeGerencial);
                            ds.SaidaPendente = item.QuantidadeSaidaPendente.ToString(); //string.Concat(item.QuantidadeSaidaPendente, " ", item.UnidadeReferencia, " / ", item.QuantidadeGerencialSaidaPendente, " ", item.UnidadeGerencial);
                            ds.SaldoFuturo = item.SaldoFuturo.ToString(); //string.Concat(item.SaldoFuturo, " ", item.UnidadeReferencia, " / ", item.SaldoGerencialFuturo, " ", item.UnidadeGerencial);
                            listDs.Add(ds);
                        }
                        SaldoProduto relDS = new SaldoProduto();
                        DataTable tabela = ConvertToDataTable(listDs, relDS.Tables["SaldoProduto"]);


                        // Logotipo
                        if (tabela.Rows.Count > 0)
                        {
                            tabela.Rows[0]["Logotipo"] = empresa.Logotipo;
                        }
                        // fim - logotipo

                        ReportDataSource dataSource = new ReportDataSource("SaldoProduto", tabela);

                        reportViewer.LocalReport.DataSources.Add(dataSource);

                        scriptManager.RegisterPostBackControl(reportViewer);

                        // Gerando PDF
                        string mimeType = "application/pdf"; //string.Empty;
                        string encoding = string.Empty;
                        string extension = "pdf";

                        string[] streamIds;
                        Warning[] warnings;
                        byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                        //if (System.IO.File.Exists(@"C:\Temp\SaldoProduto.pdf"))
                        var absPath = string.Concat(Server.MapPath("/"), @"temp\");
                        var path = string.Empty;
                        var file = string.Empty;
                        var pathReturn = string.Empty;
                        //var path = string.Concat(Server.MapPath("/"), @"temp\SaldoProduto-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");

                        file = string.Concat("SaldoProduto-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                        path = string.Concat(absPath, file);
                        pathReturn = Url.Content("~/temp/" + file);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                        fs.Write(pdfBytes, 0, pdfBytes.Length);
                        fs.Close();

                        //RegistroArquivo registroArquivo = new RegistroArquivo();
                        //var seq = _registroArquivoAppService.
                        //registroArquivo.Arquivo = pdfBytes;
                        //registroArquivo.RegistroTabelaId = (long)EnumArquivoTabela.SaldoProduto;
                        //registroArquivo.RegistroId = 1; //(long)filtro.PrescricaoId;
                        //var id = _registroArquivoRepository.InsertAndGetId(registroArquivo);

                        reportViewer.LocalReport.Refresh();

                        Response.Headers.Add("Content-Disposition", string.Format("inline; filename=SaldoProduto-{0}.pdf", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));

                        return Content(pathReturn);
                        //return File(pdfBytes, "application/pdf");
                        //return File(pdfBytes, "application/pdf", @"c:\temp\SaldoProduto.pdf");
                        //return PartialView("~/areas/mpa/views/aplicacao/relatorios/_viewer.cshtml",)

                        // return View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Relatorios/PrescricoesMedicas/PrescricaoMedica.aspx", _filtro);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return null;
        }

        public ContentResult VisualizarEstoqueMovimentoResumidoPDF(EstoqueMovimentoViewModel input)
        {
            try
            {
                using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
                using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
                using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
                using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                {
                    var empresa = Task.Run(() => empresaAppService.Object.Obter(input.EmpresaId)).Result;
                    var usuarioEmpresas = Task.Run(() => userAppService.Object.GetUserEmpresas(abpSession.Object.UserId.Value)).Result;
                    var usuario = Task.Run(() => userAppService.Object.GetUser()).Result;
                    var loginInformations = Task.Run(() => sessionAppService.Object.GetCurrentLoginInformations()).Result;
                    var dados = new Models.Aplicacao.Suprimentos.Relatorios.FiltroModel();
                    dados.Titulo = "Movimentação dos produtos " + (input.TipoRel == 1 ? "resumido" : "detalhado");
                    dados.NomeHospital = empresa.NomeFantasia;
                    dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                    dados.DataHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    dados.StartDate = input.StartDate.ToString("dd/MM/yyyy");
                    dados.EndDate = input.EndDate.ToString("dd/MM/yyyy");
                    // Obtido do ASPX
                    ScriptManager scriptManager = new ScriptManager();
                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Suprimento\EstoqueMovimento\Resumido.rdlc");

                    if (dados != null)
                    {
                        //parâmetros para o relatório
                        ReportParameter nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        ReportParameter nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                        ReportParameter titulo = new ReportParameter("Titulo", dados.Titulo);
                        ReportParameter data = new ReportParameter("Data", dados.DataHora);
                        ReportParameter dataHora = new ReportParameter("DataHora", dados.DataHora);
                        ReportParameter _startDate = new ReportParameter("StartDate", dados.StartDate);
                        ReportParameter _endDate = new ReportParameter("EndDate", dados.EndDate);

                        reportViewer.LocalReport.SetParameters(new ReportParameter[] {
                            nomeHospital,
                            nomeUsuario,
                            titulo,
                            data,
                            dataHora,
                            _startDate,
                            _endDate
                        });

                        //fonte de dados para o relatório - datasource
                        var list = Task.Run(() => produtoAppService.Object.ListarEstoqueMovimentoResumidoReport(input.StartDate, input.EndDate, input.EstoqueId, input.GrupoId, input.Produto)).Result;
                        var listDto = list.Items;
                        var listDs = new List<EstoqueMovimentoResumidoDsDto>();
                        foreach (var item in listDto)
                        {
                            var _item = new EstoqueMovimentoResumidoDsDto();
                            _item.CodProduto = item.CodProduto;
                            _item.Estoque = item.Estoque;
                            _item.Grupo = item.Grupo;
                            _item.Produto = item.Produto;
                            _item.QtdSaldoInicial = item.QtdSaldoInicial.ToString();
                            _item.QtdEntrada = item.QtdEntrada.ToString();
                            _item.QtdSaida = item.QtdSaida.ToString();
                            _item.QtdFinal = item.QtdFinal.ToString();
                            _item.QtdEntradaApos = item.QtdEntradaApos.ToString();
                            _item.QtdSaidaApos = item.QtdSaidaApos.ToString();
                            _item.QtdSaldoAtual = item.QtdSaldoAtual.ToString();

                            listDs.Add(_item);
                        }

                        SuprimentoDS relDS = new SuprimentoDS();
                        DataTable tabela = ConvertToDataTable(listDs, relDS.Tables["Resumido"]);


                        // Logotipo
                        if (tabela.Rows.Count > 0)
                        {
                            tabela.Rows[0]["Logotipo"] = empresa.Logotipo;
                        }
                        // fim - logotipo

                        ReportDataSource dataSource = new ReportDataSource("Resumido", tabela);

                        reportViewer.LocalReport.DataSources.Add(dataSource);

                        scriptManager.RegisterPostBackControl(reportViewer);

                        // Gerando PDF
                        string mimeType = "application/pdf"; //string.Empty;
                        string encoding = string.Empty;
                        string extension = "pdf";

                        string[] streamIds;
                        Warning[] warnings;
                        byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                        //if (System.IO.File.Exists(@"C:\Temp\SaldoProduto.pdf"))
                        var absPath = string.Concat(Server.MapPath("/"), @"temp\");
                        var path = string.Empty;
                        var file = string.Empty;
                        var pathReturn = string.Empty;

                        file = string.Concat("EstoqueMovimentoResumido-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                        path = string.Concat(absPath, file);
                        pathReturn = Url.Content("~/temp/" + file);

                        //var path = string.Concat(Server.MapPath("/"), @"areas\mpa\views\aplicacao\relatorios\pdfs\EstoqueMovimentoResumido.pdf");
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                        fs.Write(pdfBytes, 0, pdfBytes.Length);
                        fs.Close();

                        //RegistroArquivo registroArquivo = new RegistroArquivo();
                        //var seq = _registroArquivoAppService.
                        //registroArquivo.Arquivo = pdfBytes;
                        //registroArquivo.RegistroTabelaId = (long)EnumArquivoTabela.SaldoProduto;
                        //registroArquivo.RegistroId = 1; //(long)filtro.PrescricaoId;
                        //var id = _registroArquivoRepository.InsertAndGetId(registroArquivo);

                        reportViewer.LocalReport.Refresh();

                        Response.Headers.Add("Content-Disposition", string.Format("inline; filename=EstoqueMovimentoResumido-{0}.pdf", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));

                        return Content(pathReturn);
                        //return File(pdfBytes, "application/pdf");
                        //return File(pdfBytes, "application/pdf", @"c:\temp\SaldoProduto.pdf");
                        //return PartialView("~/areas/mpa/views/aplicacao/relatorios/_viewer.cshtml",)

                        // return View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Relatorios/PrescricoesMedicas/PrescricaoMedica.aspx", _filtro);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return null;
        }

        public ContentResult VisualizarEstoqueMovimentoDetalhadoPDF(EstoqueMovimentoViewModel input)
        {
            try
            {
                using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
                using (var abpSession = IocManager.Instance.ResolveAsDisposable<IAbpSession>())
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
                using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
                {
                    var empresa = Task.Run(() => empresaAppService.Object.Obter(input.EmpresaId)).Result;
                    var usuarioEmpresas = Task.Run(() => userAppService.Object.GetUserEmpresas(abpSession.Object.UserId.Value)).Result;
                    var usuario = Task.Run(() => userAppService.Object.GetUser()).Result;
                    var loginInformations = Task.Run(() => sessionAppService.Object.GetCurrentLoginInformations()).Result;
                    var dados = new Models.Aplicacao.Suprimentos.Relatorios.FiltroModel();
                    dados.Titulo = "Movimentação dos produtos " + (input.TipoRel == 1 ? "resumido" : "detalhado");
                    dados.NomeHospital = empresa.NomeFantasia;
                    dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                    dados.DataHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    dados.StartDate = input.StartDate.ToString("dd/MM/yyyy");
                    dados.EndDate = input.EndDate.ToString("dd/MM/yyyy");
                    // Obtido do ASPX
                    ScriptManager scriptManager = new ScriptManager();
                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Suprimento\EstoqueMovimento\Detalhado.rdlc");

                    if (dados != null)
                    {
                        //parâmetros para o relatório
                        ReportParameter nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        ReportParameter nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                        ReportParameter titulo = new ReportParameter("Titulo", dados.Titulo);
                        ReportParameter dataHora = new ReportParameter("DataHora", dados.DataHora);
                        ReportParameter _startDate = new ReportParameter("StartDate", dados.StartDate);
                        ReportParameter _endDate = new ReportParameter("EndDate", dados.EndDate);

                        reportViewer.LocalReport.SetParameters(new ReportParameter[] {
                            nomeHospital,
                            nomeUsuario,
                            titulo,
                            dataHora,
                            _startDate,
                            _endDate
                        });

                        //fonte de dados para o relatório - datasource
                        var list = Task.Run(() => produtoAppService.Object.ListarEstoqueMovimentoDetalhadoReport(input.StartDate, input.EndDate, input.EstoqueId, input.GrupoId, input.Produto, input.Lote)).Result;
                        var listDto = list.Items;
                        var listDs = new List<EstoqueMovimentoDetalhadoDsDto>();
                        foreach (var item in listDto)
                        {
                            var _item = new EstoqueMovimentoDetalhadoDsDto();
                            _item.CodProduto = item.CodProduto;
                            _item.Estoque = item.Estoque;
                            _item.Grupo = item.Grupo;
                            _item.Produto = item.Produto;
                            _item.Unidade = item.Unidade;
                            _item.Documento = item.Documento;
                            _item.Data = item.Data.ToString("dd/MM/yyyy");
                            _item.QuantidadeEntrada = item.QuantidadeEntrada.ToString();
                            _item.QuantidadeSaida = item.QuantidadeSaida.ToString();
                            _item.TipoOperacao = item.TipoOperacao;
                            _item.CustoUnitario = item.CustoUnitario.ToString();
                            _item.Lote = item.Lote;
                            _item.Validade = item.Validade?.ToString("dd/MM/yyyy");
                            _item.NumeroSerie = item.NumeroSerie;
                            _item.SaldoInicial = item.SaldoInicial.ToString();
                            _item.SaldoFinal = item.SaldoFinal.ToString();
                            listDs.Add(_item);
                        }
                        SuprimentoDS relDS = new SuprimentoDS();
                        DataTable tabela = ConvertToDataTable(listDs, relDS.Tables["Detalhado"]);


                        // Logotipo
                        if (tabela.Rows.Count > 0)
                        {
                            tabela.Rows[0]["Logotipo"] = empresa.Logotipo;
                        }
                        // fim - logotipo

                        ReportDataSource dataSource = new ReportDataSource("Detalhado", tabela);

                        reportViewer.LocalReport.DataSources.Add(dataSource);

                        scriptManager.RegisterPostBackControl(reportViewer);

                        // Gerando PDF
                        string mimeType = "application/pdf"; //string.Empty;
                        string encoding = string.Empty;
                        string extension = "pdf";

                        string[] streamIds;
                        Warning[] warnings;
                        byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                        //if (System.IO.File.Exists(@"C:\Temp\SaldoProduto.pdf"))
                        var absPath = string.Concat(Server.MapPath("/"), @"temp\");
                        var path = string.Empty;
                        var file = string.Empty;
                        var pathReturn = string.Empty;
                        file = string.Concat("EstoqueMovimentoDetalhado-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                        path = string.Concat(absPath, file);
                        pathReturn = Url.Content("~/temp/" + file);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                        fs.Write(pdfBytes, 0, pdfBytes.Length);
                        fs.Close();

                        //RegistroArquivo registroArquivo = new RegistroArquivo();
                        //var seq = _registroArquivoAppService.
                        //registroArquivo.Arquivo = pdfBytes;
                        //registroArquivo.RegistroTabelaId = (long)EnumArquivoTabela.SaldoProduto;
                        //registroArquivo.RegistroId = 1; //(long)filtro.PrescricaoId;
                        //var id = _registroArquivoRepository.InsertAndGetId(registroArquivo);

                        reportViewer.LocalReport.Refresh();

                        Response.Headers.Add("Content-Disposition", string.Format("inline; filename=EstoqueMovimentoDetalhado-{0}.pdf", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));

                        return Content(pathReturn);
                        //return File(pdfBytes, "application/pdf");
                        //return File(pdfBytes, "application/pdf", @"c:\temp\SaldoProduto.pdf");
                        //return PartialView("~/areas/mpa/views/aplicacao/relatorios/_viewer.cshtml",)

                        // return View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Relatorios/PrescricoesMedicas/PrescricaoMedica.aspx", _filtro);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return null;
        }


        public async Task<string> RelatorioEstoqueMovimento(EstoqueMovimentoViewModel input)
        {
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
            {
                var jasperReport = JasperReportHelper.CreateJasperReport((EmpresaAppService)empresaAppService.Object, "MovimentacaoEstoque");

                jasperReport.SetMethod(RestSharp.Method.POST);

                jasperReport.AddParameter("dataInicial", input.StartDate.ToString("dd/MM/yyyy"));
                jasperReport.AddParameter("dataFinal", input.EndDate.ToString("dd/MM/yyyy"));

                var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["App.TempPath"].ToString());

                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }

                var empresa = await empresaAppService.Object.Obter(input.EmpresaId).ConfigureAwait(false);
                if (empresa != null)
                {
                    string urlPath = null;
                    if (!empresa.LogotipoMimeType.IsNullOrEmpty() && empresa.Logotipo != null)
                    {
                        var extension = MimeTypesMap.GetExtension(empresa.LogotipoMimeType);
                        var logoFileName = $"logo_empresa_{((EmpresaAppService)empresaAppService.Object).GetCurrentTenant().Id}_{empresa.Id}.{extension}";
                        System.IO.File.WriteAllBytes(Path.Combine(tempPath, logoFileName), empresa.Logotipo);
                        urlPath = $"http://{this.HttpContext.Request.Url.Authority}/{ConfigurationManager.AppSettings["App.TempPath"]}/{logoFileName}";
                    }
                    jasperReport.AddParameter("urlImagemCliente", urlPath);
                    jasperReport.AddParameter("nomeCliente", empresa.NomeFantasia);
                }
                else
                {
                    jasperReport.AddParameter("urlImagemCliente", null);
                    jasperReport.AddParameter("nomeCliente", null);
                }

                var usuario = await userAppService.Object.GetUser().ConfigureAwait(false);
                var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);
                jasperReport.AddParameter("usuarioImpressao", string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname));
                var whereCondition = " 1 = 1";

                if (input.EstoqueId != 0)
                {
                    whereCondition += $" AND EstoqueId = {input.EstoqueId}";
                }

                if (input.GrupoId != 0)
                {
                    whereCondition += $" AND GrupoId = {input.GrupoId}";
                }

                if (!input.Produto.IsNullOrEmpty())
                {
                    whereCondition += $" AND Produto like '%{input.Produto}%'";
                }

                if (!input.Lote.IsNullOrEmpty())
                {
                    whereCondition += $" AND Lote like '%{input.Lote}%'";
                }

                jasperReport.AddParameter("whereCondition", whereCondition);

                jasperReport.AddParameter("Dominio", ((UserAppService)userAppService.Object).GetConnectionStringName());

                var report = jasperReport.GenerateReport();

                

                var fileName = $"MovimentacaoEstoque_${Guid.NewGuid()}.pdf";

                System.IO.File.WriteAllBytes(Path.Combine(tempPath, fileName), report);

                return VirtualPathUtility.Combine("/", ConfigurationManager.AppSettings["App.TempPath"].ToString()) + "/" + fileName;

            }
        }

        private DataTable ConvertToDataTable<T>(IList<T> data, DataTable table)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

            if (data != null)
            {
                foreach (T item in data)
                {
                    try
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }

            return table;
        }

    }
}