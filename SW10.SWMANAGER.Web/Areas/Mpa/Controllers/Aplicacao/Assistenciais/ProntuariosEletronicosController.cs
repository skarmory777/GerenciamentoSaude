using Abp.Runtime.Session;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Assistenciais
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Abp.Dependency;

    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.ProntuarioEletronico;
    using SW10.SWMANAGER.Web.Controllers;

    public class ProntuariosEletronicosController : SWMANAGERControllerBase
    {

        // GET: Mpa/ProntuariosEletronicos
        public async Task<ActionResult> Index(long? atendimentoId)
        {
            var viewModel = new AssistencialAtendimentoViewModel();

            if (atendimentoId.HasValue)
            {
                using (var atendimento = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    viewModel.Atendimento = await atendimento.Object.Obter(atendimentoId.Value).ConfigureAwait(false);
                }
            }

            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> CriarOuEditarProntuarioEletronico(long? id, long? atendimentoId = null)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var prontuarioEletronicoAppService = IocManager.Instance.ResolveAsDisposable<IProntuarioEletronicoAppService>())
            using (var operacaoAppService = IocManager.Instance.ResolveAsDisposable<IOperacaoAppService>())
            {
                CriarOuEditarProntuarioEletronicoViewModel viewModel;
                if (id.HasValue)
                {
                    var output = await prontuarioEletronicoAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    viewModel = new CriarOuEditarProntuarioEletronicoViewModel(output);
                }
                else
                {
                    viewModel = new CriarOuEditarProntuarioEletronicoViewModel(new ProntuarioEletronicoDto());
                    var pageName = TempData.Peek("ActivePage") as string;
                    var operacao = await operacaoAppService.Object.ObterPorNome(pageName).ConfigureAwait(false);
                    if (operacao != null)
                    {
                        viewModel.OperacaoId = operacao.Id;
                        viewModel.Operacao = operacao;
                    }
                    else
                    {
                        Logger.Error($" ProntuariosEletronicosController.CriarOuEditarProntuarioEletronico :: TenantId {this.AbpSession.GetTenantId()}  pageName {pageName} ID: {id} atendimentoId: {atendimentoId} ");
                    }

                    viewModel.DataAdmissao = DateTime.Now;
                    viewModel.AtendimentoId = atendimentoId ?? 0;
                }

                if (atendimentoId.HasValue)
                {
                    viewModel.Atendimento = await atendimentoAppService.Object.Obter(atendimentoId.Value).ConfigureAwait(false);
                    viewModel.LeitoId = viewModel.Atendimento != null && !id.HasValue ? viewModel.Atendimento.LeitoId: null;
                    viewModel.AtendimentoLeitoId = viewModel.Atendimento != null ? viewModel.Atendimento.LeitoId : null;
                }

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/CriarOuEditarProntuarioEletronicoViewModel.cshtml",
                    viewModel);
            }
        }

        public async Task<ActionResult> ReativacaoProntuarioEletronico(long? operacaoId, long? atendimentoId = null)
        {
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/ModalReativacao/ModalReativacaoViewModel.cshtml");
        }

        public async Task<ActionResult> JustificativaProntuarioEletronico(long? prontuarioEletronicoId)
        {
            var viewModel = new CriarOuEditarProntuarioEletronicoViewModel(new ProntuarioEletronicoDto());
            using (var prontuarioEletronicoAppService = IocManager.Instance.ResolveAsDisposable<IProntuarioEletronicoAppService>())
            {
                if (prontuarioEletronicoId.HasValue)
                {
                    viewModel = new CriarOuEditarProntuarioEletronicoViewModel(
                        await prontuarioEletronicoAppService.Object.Obter(prontuarioEletronicoId.Value).ConfigureAwait(false));
                }
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/ModalJustificativa/ModalJustificativaViewModel.cshtml", viewModel);
        }
    }
}