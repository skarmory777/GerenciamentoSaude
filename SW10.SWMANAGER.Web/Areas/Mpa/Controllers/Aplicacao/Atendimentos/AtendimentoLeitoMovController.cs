#region usings
using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.AtendimentosLeitosMov;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentosLeitosMov;
using SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AtendimentosLeitosMov;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.AtendimentosLeitosMov.Altas;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendoimentos
{
    public class AtendimentoLeitoMovController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            var model = new AtendimentosLeitosMovViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AtendimentosLeitosMov/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_PreAtendimentos_Create, AppPermissions.Pages_Tenant_Atendimento_PreAtendimentos_Edit)]
        public PartialViewResult CriarOuEditarModal(AtendimentoLeitoMovDto data)
        {
            CriarOuEditarAtendimentoLeitoMovModalViewModel viewModel = new CriarOuEditarAtendimentoLeitoMovModalViewModel(data);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AtendimentosLeitosMov/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<PartialViewResult> CriarOuMapaLeitorModal(AtendimentoLeitoMovDto data)
        {
            using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var tipoAcomodacaoAppService = IocManager.Instance.ResolveAsDisposable<ITipoAcomodacaoAppService>())
            {
                CriarOuEditarAtendimentoLeitoMovModalViewModel viewModel = new CriarOuEditarAtendimentoLeitoMovModalViewModel(data);
                ListarTiposAcomodacaoInput teste = new ListarTiposAcomodacaoInput();
                var tiposLeitos = await tipoAcomodacaoAppService.Object.ListarComLeito(teste).ConfigureAwait(false);
                data.LeitoId = data.Leito.Id;

                var atendimentoId = data.AtendimentoLeitoMov.AtendimentoId.HasValue ? data.AtendimentoLeitoMov.AtendimentoId.Value : data.AtendimentoId;

                if (atendimentoId != null)
                {
                    data.AtendimentoLeitoMov.Atendimento = await atendimentoAppService.Object.Obter((long)atendimentoId);
                }
                else
                {
                    data.AtendimentoLeitoMov.Atendimento = new ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto.AtendimentoDto
                    {
                        Paciente = new ClassesAplicacao.Services.Cadastros.Pacientes.Dto.PacienteDto()
                    };
                }

                viewModel.TiposLeito = tiposLeitos.Items.ToList();

                viewModel.DataInicial = DateTime.Now; //data.AtendimentoLeitoMov.DataInicial;
                viewModel.DataFinal = data.AtendimentoLeitoMov.DataFinal;
                viewModel.DataInclusao = data.AtendimentoLeitoMov.DataInclusao;
                viewModel.UserId = 1;
                viewModel.AtendimentoLeitoMovId = data.AtendimentoLeitoMovId;
                viewModel.AtendimentoLeitoMov = data.AtendimentoLeitoMov;
                viewModel.AtendimentoId = data.AtendimentoId;
                viewModel.Atendimento = data.AtendimentoLeitoMov.Atendimento;
                viewModel.Atendimento.Paciente = data.AtendimentoLeitoMov.Atendimento.Paciente;
                viewModel.Atendimento.Paciente.NomeCompleto = data.AtendimentoLeitoMov.Atendimento.Paciente.NomeCompleto;
                viewModel.LeitoId = data.AtendimentoLeitoMov.LeitoId;
                viewModel.Leito = data.AtendimentoLeitoMov.Leito;
                viewModel.UnidadeOrganizacionalId = data.AtendimentoLeitoMov.Leito.UnidadeOrganizacionalId;
                viewModel.UnidadeOrganizacional = data.AtendimentoLeitoMov.Leito.UnidadeOrganizacional;

                var unidadesOrganizacionais = await unidadeOrganizacionalAppService.Object.ListarParaInternacao();

                viewModel.UnidadesOrganizacionais = unidadesOrganizacionais.Items.ToList();


                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/MapaLeitos/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> HistoricoLeitoModal(AtendimentoLeitoMovDto data)
        {
            try
            {

                using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var atendimentoLeitoMovAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoLeitoMovAppService>())
                using (var leitoAppService = IocManager.Instance.ResolveAsDisposable<ILeitoAppService>())
                using (var tipoAcomodacaoAppService = IocManager.Instance.ResolveAsDisposable<ITipoAcomodacaoAppService>())
                {
                    var mov = atendimentoLeitoMovAppService.Object.Obter(data.AtendimentoId);
                    var leito = new LeitoDto();
                    var atendimento = await atendimentoAppService.Object.ObterAtendimentoLeitoPaciente(data.AtendimentoId.Value).ConfigureAwait(false);
                    if (data.LeitoId.HasValue)
                    {
                        leito = await leitoAppService.Object.Obter(data.LeitoId.Value).ConfigureAwait(false);
                    }
                    else
                    {
                        leito = atendimento.Leito;
                    }
                    CriarOuEditarAtendimentoLeitoMovModalViewModel viewModel = new CriarOuEditarAtendimentoLeitoMovModalViewModel(mov);
                    ListarTiposAcomodacaoInput teste = new ListarTiposAcomodacaoInput();
                    var tiposLeitos = await tipoAcomodacaoAppService.Object.Listar(teste);
                    viewModel.TiposLeito = tiposLeitos.Items.ToList();
                    viewModel.AtendimentoId = data.AtendimentoId.Value;
                    viewModel.Atendimento = atendimento;
                    viewModel.LeitoId = data.LeitoId.HasValue ? data.LeitoId.Value : atendimento.LeitoId;
                    viewModel.Leito = leito;
                    viewModel.IsAlta = atendimento.DataAlta != null;
                    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/MapaLeitos/_HistoricoLeito.cshtml", viewModel);
                }
            }
            catch (Exception ex)
            {
                return PartialView(Url.Action("Erro", "Home"));

            }

        }

        [HttpPost]
        public async Task<ActionResult> SalvarAtendimentoLeitoMov(string AtendimentoId, string LeitoId, string UnidOrg, string DataInicial, string DataInclusao, int Edita, string DataFinal = null)
        {
            try
            {
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                using (var atendimentoLeitoMovAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoLeitoMovAppService>())
                {
                    //DADOS DO USUARIO LOGADO DANDO ERRO DE PERMISSÃO
                    var userId = AbpSession.UserId;

                    var atendimentoLeitoMovDto = new AtendimentoLeitoMovDto { CreatorUserId = userId};
                    long atendimentoId;
                    long leitoId;
                    long unidadeOrganizacionalId;
                    DateTime dataInicial;
                    DateTime dataInclusao;
                    DateTime dataFinal;
                    bool isEdit;
                    if (long.TryParse(AtendimentoId, out atendimentoId) && long.TryParse(LeitoId, out leitoId) && long.TryParse(UnidOrg, out unidadeOrganizacionalId))
                    {
                        atendimentoLeitoMovDto.AtendimentoId = atendimentoId;
                        atendimentoLeitoMovDto.LeitoId = leitoId;
                        atendimentoLeitoMovDto.UnidadeOrganizacionalId = unidadeOrganizacionalId;
                
                        if (!string.IsNullOrWhiteSpace(DataInicial) && DateTime.TryParse(DataInicial, out dataInicial))
                        {
                            atendimentoLeitoMovDto.DataInicial = dataInicial;
                        }

                        if (!string.IsNullOrWhiteSpace(DataInclusao) && DateTime.TryParse(DataInclusao, out dataInclusao))
                        {
                            atendimentoLeitoMovDto.DataInclusao = dataInclusao;
                        }

                        if (!string.IsNullOrWhiteSpace(DataFinal) && DateTime.TryParse(DataFinal, out dataFinal))
                        {
                            atendimentoLeitoMovDto.DataFinal = dataFinal;
                        }
                        
                        if (bool.TryParse(Edita.ToString(),out isEdit))
                        {
                            if (!isEdit)
                            {
                                await atendimentoLeitoMovAppService.Object.Criar(atendimentoLeitoMovDto)
                                    .ConfigureAwait(false);
                            }
                            else
                            {
                                await atendimentoLeitoMovAppService.Object.Editar(atendimentoLeitoMovDto).ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var erro = new System.Text.StringBuilder();
                erro.AppendLine(string.Concat("StackTrace:", ex.StackTrace, "Message:", ex.Message));
                if (ex.InnerException != null)
                {
                    erro.AppendLine(string.Concat("StackTrace:", ex.InnerException.StackTrace, "Message:", ex.InnerException.Message));
                }
                erro.AppendLine(string.Concat("data:", Newtonsoft.Json.JsonConvert.SerializeObject(new { AtendimentoId, LeitoId, UnidOrg, DataInicial, DataInclusao, Edita, DataFinal })));
                this.Logger.Error(erro.ToString(), ex);
                throw ex;

            }

            return Content(L("Sucesso"));
        }

        public async Task<ActionResult> AltaModal(AtendimentoLeitoMov input)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter((long)input.AtendimentoId).ConfigureAwait(false);

                var viewModel = new AltaModalViewModel
                {
                    AtendimentoId = input.AtendimentoId,
                    Data = (DateTime)(atendimento.DataAlta != null ? atendimento.DataAlta : new DateTime()),
                    DataAltaMedica = (DateTime)(atendimento.DataAltaMedica != null ? atendimento.DataAltaMedica : new DateTime()),
                    PrevisaoAlta = (DateTime)(atendimento.DataPrevistaAlta != null ? atendimento.DataPrevistaAlta : new DateTime()),
                    //viewModel.Data = (DateTime)(atendimento.DataAlta != null ? atendimento.DataAlta : DateTime.Now);
                    //viewModel.DataAltaMedica = (DateTime)(atendimento.DataAltaMedica != null ? atendimento.DataAltaMedica : DateTime.Now);
                    //viewModel.PrevisaoAlta = (DateTime)(atendimento.DataPrevistaAlta != null ? atendimento.DataPrevistaAlta : DateTime.Now);
                    GrupoCIDId = atendimento.AltaGrupoCIDId,
                    MotivoAltaId = atendimento.MotivoAltaId,
                    NumeroObito = atendimento.NumeroObito,
                    GrupoCID = atendimento.AltaGrupoCID,
                    MotivoAlta = atendimento.MotivoAlta,

                    DataTomadaDecisao = atendimento.DataTomadaDecisao
                };

                if (atendimento.LeitoId.HasValue)
                {
                    viewModel.LeitoId = atendimento.Leito.Id;
                    viewModel.Leito = atendimento.Leito;
                }
                ViewBag.IsConsulta = false;
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Altas/Alta/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> _AltaModal(long atendimentoId, bool isConsulta = false)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);

                var viewModel = new AltaModalViewModel
                {
                    AtendimentoId = atendimentoId,
                    Data = (DateTime)(atendimento.DataAlta != null ? atendimento.DataAlta : DateTime.MinValue),
                    DataAltaMedica = (DateTime)(atendimento.DataAltaMedica != null ? atendimento.DataAltaMedica : DateTime.MinValue),
                    PrevisaoAlta = (DateTime)(atendimento.DataPrevistaAlta != null ? atendimento.DataPrevistaAlta : DateTime.MinValue),
                    GrupoCIDId = atendimento.AltaGrupoCIDId,
                    MotivoAltaId = atendimento.MotivoAltaId,
                    GrupoCID = atendimento.AltaGrupoCID,
                    MotivoAlta = atendimento.MotivoAlta,
                    NumeroObito = atendimento.NumeroObito,
                    DataTomadaDecisao = atendimento.DataTomadaDecisao
                };

                if (atendimento.LeitoId.HasValue)
                {
                    viewModel.LeitoId = atendimento.Leito.Id;
                    viewModel.Leito = atendimento.Leito;
                }
                ViewBag.IsConsulta = isConsulta;
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Altas/Alta/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

    }
}