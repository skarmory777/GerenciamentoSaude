using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaLotes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Lote;
using SW10.SWMANAGER.Web.Controllers;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturamentoLotesController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Lotes/Index.cshtml");
        }

        public ActionResult Lote(long loteId)
        {
            using (var _loteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoEntregaLote, long>>())
            {
                var entity = _loteRepository.Object.
                    GetAll()
                    .Include(x=> x.Convenio)
                    .Include(x=> x.Convenio.SisPessoa)
                    .Include(x => x.Empresa)
                    .FirstOrDefault(x=> x.Id == loteId);
                if (entity != null)
                {
                    var lote = new FaturamentoEntregaLoteViewModel(FaturamentoEntregaLoteDto.Mapear(entity));
                    return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Lotes/Lote/Index.cshtml", lote);
                }
                return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Lotes/Lote/Index.cshtml");
            }
        }

        public ActionResult Criar()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Lotes/Criar/Index.cshtml");
        }

        public ActionResult ReenviarConta()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Lotes/CriarOuEditar/Index.cshtml");
        }

        public async Task<FileResult> GerarLote(long fatEntregaLoteId)
        {
            using(var faturamentoEntregaLote = IocManager.Instance.ResolveAsDisposable<IFaturamentoEntregaLoteAppService>() )
            {
                var result = await faturamentoEntregaLote.Object.GerarLote(fatEntregaLoteId);
                return result.ReturnObject.GerarArquivo();
            }
        }
    }
}