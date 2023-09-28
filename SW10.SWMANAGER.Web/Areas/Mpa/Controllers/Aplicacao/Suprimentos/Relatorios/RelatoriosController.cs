using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using Microsoft.Reporting.WebForms;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Relatorios;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.OData.Edm;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Relatorios.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos.Relatorios
{
    public partial class RelatoriosController : Controller
    {
        public async Task<ActionResult> ConsumoPorPaciente()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexConsumoPorPaciente.cshtml", null);
        }
        
        public async Task<ActionResult> ConsumoPorSetor()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexConsumoPorSetor.cshtml", null);
        }
        
        public async Task<ActionResult> DevolucaoPorEstoque()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexDevolucaoPorEstoque.cshtml", null);
        }
        
        public async Task<ActionResult> DevolucaoPorPaciente()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexDevolucaoPorPaciente.cshtml", null);
        }
        
        public async Task<ActionResult> PerdaPorEstoque()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexPerdaPorEstoque.cshtml", null);
        }
        
        public async Task<ActionResult> UltimasCompras()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexUltimasCompras.cshtml", null);
        }
        
        public async Task<ActionResult> UltimasComprasVsAtual()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexUltimasComprasVsAtual.cshtml", null);
        }

        public async Task<ActionResult> MapaDispensacao()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexMapaDispensacao.cshtml", null);
        }

        public async Task<FileResult> RetornaConsumoPorPaciente(DateTime dataInicio, DateTime dataFinal, long? pacienteId, long? empresaId)
        {
            using (var relatorioSuprimentoAppService =  IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var relatorioName = $"relatorio-consumo-por-paciente-{dataInicio.ToString("dd-MM-yyyy")}-{dataFinal.ToString("dd-MM-yyyy")}";
                if (pacienteId.HasValue)
                {
                    relatorioName += $"-{(pacienteId.Value)}";
                }

                return this.File(
                    await relatorioSuprimentoAppService.Object
                        .RetornaConsumoPorPaciente(dataInicio, dataFinal, pacienteId, empresaId).ConfigureAwait(false),
                    "application/pdf", $"{relatorioName}.pdf");
            }
        }
        
        public async Task<FileResult> RetornaConsumoPorSetor(DateTime dataInicio, DateTime dataFinal, long? unidadeOrganizacionalId, long? empresaId)
        {
            using (var relatorioSuprimentoAppService =  IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var relatorioName = $"relatorio-consumo-por-setor-{dataInicio.ToString("dd-MM-yyyy")}-{dataFinal.ToString("dd-MM-yyyy")}";
                if (unidadeOrganizacionalId.HasValue)
                {
                    relatorioName += $"-{(unidadeOrganizacionalId.Value)}";
                }

                return this.File(
                    await relatorioSuprimentoAppService.Object
                        .RetornaConsumoPorSetor(dataInicio, dataFinal, unidadeOrganizacionalId, empresaId).ConfigureAwait(false),
                    "application/pdf", $"{relatorioName}.pdf");
            }
        }
        
        public async Task<FileResult> RetornaDevolucaoPorEstoque(DateTime dataInicio, DateTime dataFinal, long? estoqueId, long? empresaId)
        {
            using (var relatorioSuprimentoAppService =  IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var relatorioName = $"relatorio-devolucao-por-estoque-{dataInicio.ToString("dd-MM-yyyy")}-{dataFinal.ToString("dd-MM-yyyy")}";
                if (estoqueId.HasValue)
                {
                    relatorioName += $"-{(estoqueId.Value)}";
                }

                return this.File(
                    await relatorioSuprimentoAppService.Object
                        .RetornaDevolucaoPorEstoque(dataInicio, dataFinal, estoqueId, empresaId).ConfigureAwait(false),
                    "application/pdf", $"{relatorioName}.pdf");
            }
        }
        
        public async Task<FileResult> RetornaDevolucaoPorPaciente(DateTime dataInicio, DateTime dataFinal, long? estoqueId, long? empresaId, long? pacienteId)
        {
            using (var relatorioSuprimentoAppService =  IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var relatorioName = $"relatorio-devolucao-por-paciente-{dataInicio.ToString("dd-MM-yyyy")}-{dataFinal.ToString("dd-MM-yyyy")}";
                if (estoqueId.HasValue)
                {
                    relatorioName += $"-{(estoqueId.Value)}";
                }

                return this.File(
                    await relatorioSuprimentoAppService.Object
                        .RetornaDevolucaoPorPaciente(dataInicio, dataFinal, estoqueId, empresaId,pacienteId).ConfigureAwait(false),
                    "application/pdf", $"{relatorioName}.pdf");
            }
        }
        
        public async Task<FileResult> RetornaPerdaPorEstoque(DateTime dataInicio, DateTime dataFinal, long? estoqueId, long? empresaId)
        {
            using (var relatorioSuprimentoAppService =  IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var relatorioName = $"relatorio-perda-por-estoque-{dataInicio.ToString("dd-MM-yyyy")}-{dataFinal.ToString("dd-MM-yyyy")}";
                if (estoqueId.HasValue)
                {
                    relatorioName += $"-{(estoqueId.Value)}";
                }

                return this.File(
                    await relatorioSuprimentoAppService.Object
                        .RetornaPerdaPorEstoque(dataInicio, dataFinal, estoqueId, empresaId).ConfigureAwait(false),
                    "application/pdf", $"{relatorioName}.pdf");
            }
        }
        
        public async Task<FileResult> RetornaUltimasCompras(RelatorioUltimasComprasDto input)
        {
            using (var relatorioSuprimentoAppService =  IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var relatorioName = $"relatorio-ultimas-compras-{input.DataInicio.ToString("dd-MM-yyyy")}-{input.DataFinal.ToString("dd-MM-yyyy")}";
                if (input.EstoqueId.HasValue)
                {
                    relatorioName += $"-{(input.EstoqueId.Value)}";
                }
                
                if (input.ProdutoId.HasValue)
                {
                    relatorioName += $"-{(input.ProdutoId.Value)}";
                }
                
                if (input.Rank.HasValue)
                {
                    relatorioName += $"-{(input.Rank.Value)}";
                }

                return this.File(
                    await relatorioSuprimentoAppService.Object.RetornaUltimasCompras(input).ConfigureAwait(false),"application/pdf", $"{relatorioName}.pdf");
            }
        }
        
        public async Task<FileResult> RetornaUltimasComprasVsAtual(RelatorioUltimasComprasVsAtualDto input)
        {
            using (var relatorioSuprimentoAppService =  IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var relatorioName = $"relatorio-ultimas-compras-vs-atual-{input.DataInicioAtual.ToString("dd-MM-yyyy")}-{input.DataFimAtual.ToString("dd-MM-yyyy")}";
                

                return this.File(
                    await relatorioSuprimentoAppService.Object.RetornaUltimasComprasVsAtual(input).ConfigureAwait(false),"application/pdf", $"{relatorioName}.pdf");
            }
        }

        public async Task<FileResult> RetornaAcuracia(RelatorioAcuraciaDto input)
        {
            using (var relatorioSuprimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var relatorioName = $"relatorio-acuracia-{input.DataInicio.ToString("dd-MM-yyyy")}-{input.DataFinal.ToString("dd-MM-yyyy")}";
                if (input.EstoqueId.HasValue)
                {
                    relatorioName += $"-{(input.EstoqueId.Value)}";
                }

                return this.File(
                    await relatorioSuprimentoAppService.Object.RetornaAcuracia(input).ConfigureAwait(false), "application/pdf", $"{relatorioName}.pdf");
            }
        }

        public async Task<FileResult> RetornaMapaDispensacao(DateTime dataInicio, DateTime dataFinal, long? unidadeId, long? empresaId)
        {
            using (var relatorioSuprimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var relatorioName = $"relatorio-mapa-dispensacao-{dataInicio.ToString("dd-MM-yyyy")}-{dataFinal.ToString("dd-MM-yyyy")}";
                return this.File(
                    await relatorioSuprimentoAppService.Object.RetornaMapaDispensacao(dataInicio, dataFinal, unidadeId, empresaId).ConfigureAwait(false),
                    "application/pdf", $"{relatorioName}.pdf");
            }
        }



        /// <summary>
        /// Entrada para filtro de visualização do Report de produtos
        /// </summary>
        /// <returns></returns>
        //GET: Mpa/Relatorios/SaldoProduto
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto)]
        public async Task<ActionResult> SaldoProduto()
        {
            FiltroModel result = await CarregarIndex();
            result.EhMovimentacao = false;
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/Index.csHtml", result);
        }

        /// <summary>
        /// Entrada para filtro e visualização do Report de Movimentação
        /// </summary>
        /// <returns></returns>
        //GET: Mpa/Relatorios/MovimentacaoProduto
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        public async Task<ActionResult> MovimentacaoProduto()
        {
            FiltroModel result = await CarregarIndex();
            result.EhMovimentacao = true;
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexMovimentacao.csHtml", result);
        }

        public async Task<ActionResult> Acuracia()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/IndexAcuracia.cshtml", null);
        }

        /// <summary>
        /// PartialView que renderiza o relatório com os filtros selecionados no formulário
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        public ActionResult Visualizar(FiltroModel filtro)
        {
            if (filtro.EhMovimentacao)
            {
                ProcessarDadosMovimentacao(filtro);
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/Movimentacao.aspx", filtro);
            }
            else
            {
                ProcessarDadosProduto(filtro);
                return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/SaldoProduto.aspx", filtro);
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        public ActionResult Exportar(FiltroModel filtro, string formato)
        {
            if (formato != "Word" && formato != "Excel" && formato != "PDF")
            {
                throw new Exception();
            }

            ReportDataSource rd;
            LocalReport report;
            string mimeType;
            string encoding;
            string filenameExtension;
            string[] streams;
            Warning[] warnings;

            if (filtro.EhMovimentacao)
            {
                ProcessarDadosMovimentacao(filtro);
                report = new LocalReport()
                {
                    ReportPath = "Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/Movimentacao.rdlc"
                };
                rd = new ReportDataSource("DataSet1", filtro.DadosMovimentacao);
            }
            else
            {
                ProcessarDadosProduto(filtro);
                report = new LocalReport()
                {
                    ReportPath = "Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/ExtratoProdutos.rdlc"
                };
                rd = new ReportDataSource("DataSet1", filtro.Dados);
            }

            report.DataSources.Add(rd);
            var bytes = report.Render(formato, "", out mimeType, out encoding, out filenameExtension, out streams, out warnings);
            string extensionType = string.Empty;
            switch (formato)
            {
                case "Word":
                    extensionType = ".doc";
                    break;
                case "Excel":
                    extensionType = ".xls";
                    break;
                default:
                    extensionType = ".pdf";
                    break;
            }
            string fileName = string.Concat("RELATORIO", extensionType);
            return File(bytes, mimeType, fileName);
        }

        /// <summary>
        /// Listar GrupoClasse com base do Grupo Selecionado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        public async Task<JsonResult> ListarGrupoClasse(int id)
        {
            using (var relatorioSuprimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var result = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "Selecione" } };

                result.AddRange((await relatorioSuprimentoAppService.Object.Listar(new Grupo { Id = id }))
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Nome
                    }));

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        public async Task<JsonResult> ListarGrupoSubClasse(int id)
        {
            using (var relatorioSuprimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var result = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "Selecione" } };

                result.AddRange((await relatorioSuprimentoAppService.Object.Listar(new GrupoClasse { Id = id }))
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Nome
                    }));

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private async Task<FiltroModel> CarregarIndex()
        {
            using (var relatorioSuprimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioSuprimentoAppService>())
            {
                var result = new FiltroModel
                {
                    Grupos = (await relatorioSuprimentoAppService.Object.Listar())
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Nome
                    }).ToList()
                };
                var padrao = new SelectListItem { Text = "Selecione", Value = "0" };
                result.Grupos.Insert(0, padrao);
                result.Default = new List<SelectListItem> { padrao };
                return result;
            }
        }

        private void ProcessarDadosProduto(FiltroModel filtro)
        {
            var db = new Core.DataSetReportsTableAdapters.ReportProdutosTableAdapter();

            var grupo = filtro.GrupoProduto.GetValueOrDefault();
            var classe = filtro.Classe.GetValueOrDefault();
            var subClasse = filtro.SubClasse.GetValueOrDefault();
            if (grupo != 0 && classe == 0 && subClasse == 0)
            {
                filtro.Dados = db.GetDataByGrupoId(grupo).ToList();
            }
            else if (grupo != 0 && classe != 0 && subClasse == 0)
            {
                filtro.Dados = db.GetDataByGrupoClasseId(grupo, classe).ToList();
            }
            else if (grupo != 0 && classe != 0 && subClasse != 0)
            {
                filtro.Dados = db.GetDataBy(grupo, classe, subClasse).ToList();
            }
            else
            {
                filtro.Dados = db.GetData().ToList();
            }
        }

        private void ProcessarDadosMovimentacao(FiltroModel filtro)
        {
            var db = new Core.DataSetReportsTableAdapters.RelatorioMovimentoAdapter();

            var grupo = filtro.GrupoProduto.GetValueOrDefault();
            var classe = filtro.Classe.GetValueOrDefault();
            var subClasse = filtro.SubClasse.GetValueOrDefault();
            var query = db.GetData().Where(w => w.GrupoId == grupo);

            if (classe != 0)
            {
                query = query.Where(w => w.GrupoClasseId == classe);
            }

            if (subClasse != 0)
            {
                query = query.Where(w => w.GrupoSubClasseId == subClasse);
            }

            filtro.DadosMovimentacao = query.ToList();
        }

        public PartialViewResult _Viewer(string path)
        {
            //VisualizarSaldoProdutoPDF(1, 0, 0);
            ViewBag.FilePath = path; //Url.Content("~/areas/mpa/views/aplicacao/relatorios/pdfs/SaldoProduto.pdf").ToString(); //@"C:\Temp\SaldoProduto.pdf"; //path;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Relatorios/_Viewer.cshtml");
        }
    }
}