
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;

using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.PainelSenhas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Filas;
using SW10.SWMANAGER.Web.Controllers;

using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class MonitorPainelSenhasController : SWMANAGERControllerBase
    {
        // GET: Mpa/Alta
        public ActionResult Index()
        {
            var viewModel = new PainelSenhaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/PainelSenhas/Index.cshtml", viewModel);
        }

        public PartialViewResult _ChamarSenha(ChamarSenhaModalViewModel input)
        {
            var viewModel = new ChamarSenhaModalViewModel();

            using (var localChamadaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LocalChamada, long>>())
            using (var movimentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SenhaMovimentacao, long>>())
            {
                if (input.LocalChamadaId.HasValue)
                {
                    var local = localChamadaRepository.Object.GetAll().Include(i => i.TipoLocalChamada)
                        .FirstOrDefault(w => w.Id == input.LocalChamadaId);

                    if (local != null)
                    {
                        var localChamataDto = LocalChamadaDto.Mapear(local);
                        viewModel.LocalChamada = localChamataDto;
                        viewModel.TipoLocalChamada = localChamataDto.TipoLocalChamada;
                    }
                }

                var ultimaMovimentacao = movimentacaoRepository.Object.GetAll().Where(w => w.SenhaId == input.SenhaId).ToList();

                viewModel.SenhaId = ultimaMovimentacao.OrderBy(o => o.Id).LastOrDefault()?.Id;

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/PainelSenhas/_ChamarSenha.cshtml",
                    viewModel);
            }
        }
    }
}