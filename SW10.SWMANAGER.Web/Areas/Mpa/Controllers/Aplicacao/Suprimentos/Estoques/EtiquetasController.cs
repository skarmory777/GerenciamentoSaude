using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Etiquetas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Controllers;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class EtiquetasController : SWMANAGERControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var model = new PreMovimentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Etiquetas/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarEtiqueta(long id, decimal? qtd)
        {
            var model = new EtiquetaViewModel();

            using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
            {
                var loteValidade = await estoqueLoteValidadeAppService.Object.ObterPorId(id).ConfigureAwait(false);

                if (loteValidade != null)
                {
                    model.Id = loteValidade.Id;
                    model.Validade = loteValidade.Validade;
                    model.Quantidade = qtd ?? 0;
                    model.Lote = loteValidade.Lote;
                    model.Laboratorio = loteValidade.ProdutoLaboratorio?.Descricao;
                    model.Produto = loteValidade.Produto.Descricao;
                }


                return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Etiquetas/_CriarOuEditar.cshtml", model);
            }
        }

        public ActionResult ImprimirEtiqueta(long loteValidadeId,  double qtd, DateTime dataFracionamento, string modelo)
        {
            using (var codigoBarraAppService = IocManager.Instance.ResolveAsDisposable<ICodigoBarraAppService>())
            {

                return this.File(codigoBarraAppService.Object.ImprimirEtiqueta(new CodigoBarraAppService.ImprimirEtiquetaDto(loteValidadeId, qtd, dataFracionamento, modelo))
                , "application/pdf", $"etiqueta-{loteValidadeId}-{qtd}-{modelo}.pdf");
            }
        }
    }
}