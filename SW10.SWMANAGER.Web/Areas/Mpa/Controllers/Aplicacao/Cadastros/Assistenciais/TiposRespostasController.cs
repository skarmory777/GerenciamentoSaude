using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class TiposRespostasController : SWMANAGERControllerBase
    {
        private readonly ITipoRespostaAppService _tipoRespostaAppService;
        private readonly ITipoRespostaTipoRespostaConfiguracaoAppService _tipoRespostaTipoRespostaConfiguracaoAppService;
        private readonly IDivisaoAppService _divisaoAppService;
        private readonly IFormaAplicacaoAppService _formaAplicacaoAppService;
        private readonly IFrequenciaAppService _frequenciaAppService;
        private readonly IMedicoAppService _medicoAppService;
        private readonly IPrescricaoItemAppService _prescricaoItemAppService;
        private readonly IUnidadeAppService _unidadeAppService;
        private readonly IUnidadeOrganizacionalAppService _unidadeOrganizacionalAppService;
        private readonly IVelocidadeInfusaoAppService _velocidadeInfusaoAppService;

        public TiposRespostasController(
            ITipoRespostaAppService tipoRespostaAppService,
            ITipoRespostaTipoRespostaConfiguracaoAppService tipoRespostaTipoRespostaConfiguracaoAppService,
            IDivisaoAppService divisaoAppService,
            IFormaAplicacaoAppService formaAplicacaoAppService,
            IFrequenciaAppService frequenciaAppService,
            IMedicoAppService medicoAppService,
            IPrescricaoItemAppService prescricaoItemAppService,
            IUnidadeAppService unidadeAppService,
            IUnidadeOrganizacionalAppService unidadeOrganizacionalAppService,
            IVelocidadeInfusaoAppService velocidadeInfusaoAppService

            )
        {
            _tipoRespostaAppService = tipoRespostaAppService;
            _tipoRespostaTipoRespostaConfiguracaoAppService = tipoRespostaTipoRespostaConfiguracaoAppService;
            _divisaoAppService = divisaoAppService;
            _formaAplicacaoAppService = formaAplicacaoAppService;
            _frequenciaAppService = frequenciaAppService;
            _medicoAppService = medicoAppService;
            _prescricaoItemAppService = prescricaoItemAppService;
            _unidadeAppService = unidadeAppService;
            _unidadeOrganizacionalAppService = unidadeOrganizacionalAppService;
            _velocidadeInfusaoAppService = velocidadeInfusaoAppService;

        }
        // GET: Mpa/TipoResposta
        public ActionResult Index()
        {
            var model = new TipoRespostaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostas/Index.cshtml", model);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_TipoRespostaMedico_Create, AppPermissions.Pages_Tenant_Assistencial_TipoRespostaMedico_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoRespostaViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoRespostaAppService.Obter(id.Value); //_TiposRespostaservice.GetTiposRespostas(new GetTiposRespostasInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoRespostaViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoRespostaViewModel(new TipoRespostaDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostas/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<PartialViewResult> _CriarOuEditarRelacaoModal(long tipoRespostaPrincipalId, long? id)
        {
            CriarOuEditarTipoRespostaTipoRespostaConfiguracaoViewModel viewModel;
            var output = new TipoRespostaTipoRespostaConfiguracaoDto();
            if (id.HasValue)
            {
                output = await _tipoRespostaTipoRespostaConfiguracaoAppService.Obter(id.Value);
            }
            output.TipoRespostaId = tipoRespostaPrincipalId;
            viewModel = new CriarOuEditarTipoRespostaTipoRespostaConfiguracaoViewModel(output);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostas/_CriarOuEditarRelacaoModal.cshtml", viewModel);
        }

    }
}