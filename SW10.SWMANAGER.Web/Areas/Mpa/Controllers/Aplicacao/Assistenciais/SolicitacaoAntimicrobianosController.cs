using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class SolicitacaoAntimicrobianosController : SWMANAGERControllerBase
    {

        public async Task<ActionResult> Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacaoAntimicrobianos/index.cshtml", null);
        }

        public async Task<ActionResult> CriarOuEditarModal(long id)
        {
            using (var tipoSolicitacaoAntimicrobianosIndicacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoSolicitacaoAntimicrobianosIndicacao, long>>())
            using (var tipoSolicitacaoAntimicrobianosResultadoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoSolicitacaoAntimicrobianosResultado, long>>())
            using (var tipoSolicitacaoAntimicrobianosCulturasRepository = IocManager.Instance.ResolveAsDisposable<IRepository<TipoSolicitacaoAntimicrobianosCultura, long>>())
            using (var solicitacaoAntimicrobianosAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoAntimicrobianoAppService>())
            {
                var solicitacaoAntimicrobiano = await solicitacaoAntimicrobianosAppService.Object.ObterSolicitacaoPorId(id).ConfigureAwait(false);

                var viewModel = new SolicitacaoAntimicrobianosViewModel
                {
                    AtendimentoId = solicitacaoAntimicrobiano.Atendimento.Id,
                    CodigoAtendimento = solicitacaoAntimicrobiano.Atendimento.Codigo,
                    Leito = solicitacaoAntimicrobiano.Atendimento.Leito?.Descricao,
                    NomePaciente = solicitacaoAntimicrobiano.Atendimento.Paciente?.NomeCompleto,
                    UnidadeOrganizacional = solicitacaoAntimicrobiano.Atendimento.UnidadeOrganizacional.Descricao
                };

                viewModel.SolicitacaoAntimicrobianos = new List<SolicitacaoAntimicrobianoDto>() { solicitacaoAntimicrobiano };

                viewModel.TipoIndicacoes = tipoSolicitacaoAntimicrobianosIndicacaoRepository.Object.GetAll().ToList().Select(x => CamposPadraoCRUDDto.MapearBase<TipoSolicitacaoAntimicrobianosIndicacaoDto>(x)).ToList();
                viewModel.TipoResultados = tipoSolicitacaoAntimicrobianosResultadoRepository.Object.GetAll().ToList().Select(x => CamposPadraoCRUDDto.MapearBase<TipoSolicitacaoAntimicrobianosResultadoDto>(x)).ToList();

                viewModel.TipoCulturas = tipoSolicitacaoAntimicrobianosCulturasRepository.Object.GetAll().ToList().Select(x => CamposPadraoCRUDDto.MapearBase<TipoSolicitacaoAntimicrobianosCulturaDto>(x)).ToList();

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacaoAntimicrobianos/_CriarOuEditar.cshtml", viewModel);

            }
        }

        // GET: Mpa/Prorrogacao
        public async Task<ActionResult> SolicitacaoAntimicrobianoModal(long atendimentoId, long? prescricaoId)
        {
            using (var solicitacaoAntimicrobianosAppService =
                IocManager.Instance.ResolveAsDisposable<ISolicitacaoAntimicrobianoAppService>())
            {
                var viewModel = await solicitacaoAntimicrobianosAppService.Object
                    .SolicitacaoAntimicrobianoModal(atendimentoId, prescricaoId)
                    .ConfigureAwait(false);
                
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacaoAntimicrobianos/_CriarOuEditar.cshtml", viewModel);
            }
        }
    }
}