#region usings

using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentosSalaCirurgicas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Sexos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Atendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.ClassificacoesRisco;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.PreAtendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.AtendimentosLeitosMov.Altas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.ClassificacoesRisco;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Orcamentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.PreAtendimentos;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class AtendimentosController : SWMANAGERControllerBase
    {
        #region Index e Modais

        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_Edit)]
        public async Task<ActionResult> Index(long? id, int abaId, bool internacao = false, bool ambulatorioEmergencia = false, long? agendamentoId = null)
        {
            var userId = this.AbpSession.UserId;

            using (var localChamadasAppService = IocManager.Instance.ResolveAsDisposable<ILocalChamadasAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var agendamentoSalaCirurgicaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoSalaCirurgicaAppService>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var medicoAppService = IocManager.Instance.ResolveAsDisposable<IMedicoAppService>())
            {
                var user = await userAppService.Object.GetUser().ConfigureAwait(false);

                var medicoId = user.MedicoId;
                if (medicoId.HasValue)
                {
                    ViewBag.UserMedicoId = medicoId.Value;


                    ViewBag.UserMedico = await medicoAppService.Object.Obter(medicoId.Value).ConfigureAwait(false);
                }

                var empresas = await userAppService.Object.GetUserEmpresas(userId.Value).ConfigureAwait(false);
                var viewModel = new CriarOuEditarAtendimentoModalViewModel(new AtendimentoDto());

                if (id == 0)
                {
                    id = null;
                }

                #region Edicao

                if (id.HasValue)
                {
                    var output = await atendimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarAtendimentoModalViewModel(output)
                    {
                        Empresas = new SelectList(
                                            empresas.Items.Select(
                                                m => new
                                                {
                                                    Id = m.Id,
                                                    NomeFantasia = string.Format("{0}", m.NomeFantasia)
                                                }),
                                            "Id",
                                            "NomeFantasia"),
                        IsParticular = output.Convenio.IsParticular
                    };
                }

                #endregion edicao.

                #region Criacao

                else
                {
                    viewModel.Empresas = new SelectList(
                        empresas.Items.Select(
                            m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }),
                        "Id",
                        "NomeFantasia");

                    if (agendamentoId != null)
                    {

                        var agendamento = await agendamentoSalaCirurgicaAppService.Object.ObterCirurgico((long)agendamentoId).ConfigureAwait(false);
                        if (agendamento != null)
                        {
                            viewModel.Paciente = agendamento.Paciente;
                            viewModel.PacienteId = agendamento.PacienteId;
                            viewModel.Convenio = agendamento.Convenio;
                            viewModel.ConvenioId = agendamento.ConvenioId;
                            viewModel.Plano = agendamento.Plano;
                            viewModel.PlanoId = agendamento.PlanoId;
                            viewModel.AgendamentoId = agendamentoId;
                            viewModel.MedicoId = agendamento.MedicoId;
                            viewModel.Medico = agendamento.Medico;
                            viewModel.EspecialidadeId = agendamento.MedicoEspecialidade.EspecialidadeId;
                            viewModel.Especialidade = agendamento.MedicoEspecialidade?.Especialidade;
                        }
                    }

                    var dataAtual = DateTime.Now;
                    viewModel.DataUltimoPagamento = dataAtual;
                    viewModel.ValidadeCarteira = dataAtual;
                    viewModel.DataAutorizacao = dataAtual;

                    this.CarregarDadosCookie(viewModel, ambulatorioEmergencia);
                }

                #endregion criacao.

                viewModel.AbaId = abaId;
                viewModel.IsInternacao = internacao;
                viewModel.IsAmbulatorioEmergencia = ambulatorioEmergencia;

                var cookie = Request.Cookies.Get("localChamada");

                if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
                {
                    long localChamadaId;

                    if (long.TryParse(cookie.Value, out localChamadaId))
                    {
                        var localChamada = await localChamadasAppService.Object.Obter(localChamadaId).ConfigureAwait(false);

                        if (localChamada != null)
                        {
                            viewModel.LocalChamadaId = localChamada.Id;
                            viewModel.LocalChamada = localChamada;

                            viewModel.TipoLocalChamadaId = localChamada.TipoLocalChamadaId;
                        }
                    }
                }

                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/IndexParcial.cshtml", viewModel);
            }
        }


        void CarregarDadosCookie(CriarOuEditarAtendimentoModalViewModel viewModel, bool ambulatorioEmergencia)
        {
            var cookieUnidade = ambulatorioEmergencia ? "unidadeAtendimentoId" : "unidadeAtendimentoInternacaoId";

            var cookieUnidadeAtendimento = Request.Cookies.Get(cookieUnidade);

            if (cookieUnidadeAtendimento != null && !string.IsNullOrEmpty(cookieUnidadeAtendimento.Value))
            {
                long unidadeOrganizacionalId;

                if (long.TryParse(cookieUnidadeAtendimento.Value, out unidadeOrganizacionalId))
                {
                    viewModel.UnidadeOrganizacionalId = unidadeOrganizacionalId;
                    viewModel.UnidadeOrganizacional = new ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto.UnidadeOrganizacionalDto();
                    viewModel.UnidadeOrganizacional.Id = unidadeOrganizacionalId;

                    var cookieUnidadeDescricao = ambulatorioEmergencia ? "unidadeAtendimentoDescricao" : "unidadeAtendimentoDescricaoInternacao";

                    var retorno = HttpUtility.UrlDecode(Request.Cookies.Get(cookieUnidadeDescricao)?.Value);
                    viewModel.UnidadeOrganizacional.Descricao = retorno;
                }
            }

            var cookieOrigem = ambulatorioEmergencia ? "origemId" : "origemInternacaoId";

            var cookieorigem = Request.Cookies.Get(cookieOrigem);

            if (cookieorigem != null && !string.IsNullOrEmpty(cookieorigem.Value))
            {
                long cookieorigemId;

                if (long.TryParse(cookieorigem.Value, out cookieorigemId))
                {
                    viewModel.OrigemId = cookieorigemId;
                    viewModel.Origem = new OrigemDto();
                    viewModel.Origem.Id = cookieorigemId;

                    var cookieOrigemDescricao = ambulatorioEmergencia ? "origemDescricao" : "origemDescricaoInternacao";

                    var retorno = HttpUtility.UrlDecode(Request.Cookies.Get(cookieOrigemDescricao)?.Value);
                    viewModel.Origem.Descricao = retorno;
                }
            }

            var cookieTipoAtendimento = ambulatorioEmergencia ? "tipoAtendimentoId" : "tipoAtendimentoInternacaoId";
            var tipoAtendimento = Request.Cookies.Get(cookieTipoAtendimento);

            if (tipoAtendimento != null && !string.IsNullOrEmpty(tipoAtendimento.Value))
            {
                long tipoAtendimentoId;

                if (long.TryParse(tipoAtendimento.Value, out tipoAtendimentoId))
                {
                    viewModel.AtendimentoTipoId = tipoAtendimentoId;
                    viewModel.AtendimentoTipo = new TipoAtendimentoDto();
                    viewModel.AtendimentoTipo.Id = tipoAtendimentoId;

                    var cookieTipoAtendimentoDescricao = ambulatorioEmergencia ? "tipoAtendimentoDescricao" : "tipoAtendimentoDescricaoInternacao";

                    var retorno = HttpUtility.UrlDecode(Request.Cookies.Get(cookieTipoAtendimentoDescricao)?.Value);
                    viewModel.AtendimentoTipo.Descricao = retorno;
                }
            }

            var cookieIndicadorAcidente = ambulatorioEmergencia ? "indicadorAcidenteId" : "indicadorAcidenteInternacaoId";

            var indicadorAcidente = Request.Cookies.Get(cookieIndicadorAcidente);

            if (indicadorAcidente != null && !string.IsNullOrEmpty(indicadorAcidente.Value))
            {
                long indicadorAcidenteId;

                if (long.TryParse(indicadorAcidente.Value, out indicadorAcidenteId))
                {
                    viewModel.IndicacaoAcidenteId = indicadorAcidenteId;
                    viewModel.IndicacaoAcidente = new TabelaDominioDto();
                    viewModel.IndicacaoAcidente.Id = indicadorAcidenteId;

                    var cookieIndicadorAcidenteDescricao = ambulatorioEmergencia ? "indicadorAcidenteDescricao" : "indicadorAcidenteDescricaoInternacao";

                    var retorno = HttpUtility.UrlDecode(Request.Cookies.Get(cookieIndicadorAcidenteDescricao)?.Value);
                    viewModel.IndicacaoAcidente.Descricao = retorno;
                }
            }


            var cookieComboGuia = ambulatorioEmergencia ? "comboGuiaId" : "comboGuiaInternacaoId";

            var comboGuia = Request.Cookies.Get(cookieComboGuia);

            if (comboGuia != null && !string.IsNullOrEmpty(comboGuia.Value))
            {
                long comboGuiaId;

                if (long.TryParse(comboGuia.Value, out comboGuiaId))
                {
                    viewModel.FatGuiaId = comboGuiaId;
                    viewModel.FatGuia = new FaturamentoGuiaDto();
                    viewModel.FatGuia.Id = comboGuiaId;

                    var cookieComboGuiaDescricao = ambulatorioEmergencia ? "comboGuiaDescricao" : "comboGuiaDescricaoInternacao";

                    var retorno = HttpUtility.UrlDecode(Request.Cookies.Get(cookieComboGuiaDescricao)?.Value);
                    viewModel.FatGuia.Descricao = retorno;
                }
            }

            var cookieCaraterAtendimento = ambulatorioEmergencia ? "caraterAtendimentoId" : "caraterAtendimentoInternacaoId";

            var caraterAtendimento = Request.Cookies.Get(cookieCaraterAtendimento);

            if (caraterAtendimento != null && !string.IsNullOrEmpty(caraterAtendimento.Value))
            {
                long caraterAtendimentoId;

                if (long.TryParse(caraterAtendimento.Value, out caraterAtendimentoId))
                {
                    viewModel.CaraterAtendimentoId = caraterAtendimentoId;
                    viewModel.CaraterAtendimento = new TabelaDominioDto();
                    viewModel.CaraterAtendimento.Id = caraterAtendimentoId;

                    var cookieCaraterAtendimentoDescricao = ambulatorioEmergencia ? "caraterAtendimentoDescricao" : "caraterAtendimentoDescricaoInternacao";

                    var retorno = HttpUtility.UrlDecode(Request.Cookies.Get(cookieCaraterAtendimentoDescricao)?.Value);
                    viewModel.CaraterAtendimento.Descricao = retorno;
                }
            }

            var codDependente = Request.Cookies.Get("codDependente");

            if (codDependente != null && !string.IsNullOrEmpty(codDependente.Value))
            {
                viewModel.CodDependente = codDependente.Value;
            }

        }




        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var _pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
            {
                var pacientes = await _pacienteAppService.Object.Listar(new ListarPacientesInput()).ConfigureAwait(false);

                CriarOuEditarAtendimentoModalViewModel viewModel;

                if (id.HasValue)
                {

                    var output = await atendimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarAtendimentoModalViewModel(output)
                    {
                        Pacientes = new SelectList(
                        pacientes.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }),
                        "Id",
                        "Nome")
                    };
                }
                else
                {
                    viewModel = new CriarOuEditarAtendimentoModalViewModel(new AtendimentoDto());
                    viewModel.Pacientes = new SelectList(
                        pacientes.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeCompleto) }),
                        "Id",
                        "Nome");
                }

                viewModel.PreAtendimento = false;

                viewModel.FichaAmbulatorioInput = new Fichas.FichaAmbulatorioInput();
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_IndexParcial.cshtml", viewModel);
            }
        }

        public ActionResult ModalOrcamento()
        {
            var model = new OrcamentosViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/Orcamentos/Index.cshtml", model);
        }

        public ActionResult ModalPreAtendimento()
        {
            var model = new PreAtendimentosViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/PreAtendimentos/Index.cshtml", model);
        }

        public ActionResult ModalClassificacaoRisco()
        {
            var model = new ClassificacoesRiscoViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/ClassificacaoRiscos/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CancelarAtendimentoModal(long? id)
        {
            var viewModel = new CancelamentoViewModel();
            viewModel.AtendimentoId = id;

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Cancelamento/CancelarAtendimentoModal.cshtml", viewModel);
        }

        public async Task<PartialViewResult> ReativarAtendimentoModal(long? id)
        {
            var viewModel = new CancelamentoViewModel();
            viewModel.AtendimentoId = id;

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Cancelamento/ReativarAtendimentoModal.cshtml", viewModel);
        }

        #endregion

        #region Parciais

        //public PartialViewResult DropdownEspecialidades(int abaId, long? medicoId)
        //{
        //    var viewModel = new DropdownEspecialidadesViewModel();

        //    if (medicoId.HasValue)
        //    {
        //        var especialidades = AsyncHelper.RunSync(() => _especialidadeAppService.ListarPorMedico((long)medicoId));
        //        viewModel.Especialidades = new SelectList(especialidades.Select(e => new { Id = e.Id, Nome = string.Format("{0}", e.Nome) }), "Id", "Nome");
        //    }
        //    else
        //    {
        //        var especialidades = AsyncHelper.RunSync(() => _especialidadeAppService.ListarTodos());
        //        viewModel.Especialidades = new SelectList(especialidades.Items.Select(e => new { Id = e.Id, Nome = string.Format("{0}", e.Nome) }), "Id", "Nome");
        //    }

        //    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_DropdownEspecialidades.cshtml", viewModel);
        //}

        //public PartialViewResult DropdownLeitos(int abaId, long? unidadeId)
        //{
        //    var viewModel = new DropdownLeitosViewModel();

        //    if (unidadeId.HasValue)
        //    {
        //        var leitos = AsyncHelper.RunSync(() => _leitoAppService.ListarPorUnidadeParaDrop((long)unidadeId));
        //        viewModel.Leitos = new SelectList(leitos.Select(e => new { Id = e.Id, Descricao = string.Format("{0}", e.Descricao) }), "Id", "Descricao");
        //    }
        //    else
        //    {
        //        var leitos = AsyncHelper.RunSync(() => _leitoAppService.ListarTodos());
        //        viewModel.Leitos = new SelectList(leitos.Items.Select(e => new { Id = e.Leito.Id, Descricao = string.Format("{0}", e.Leito.Descricao) }), "Id", "Descricao");
        //    }

        //    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_DropdownLeitos.cshtml", viewModel);
        //}

        public async Task<PartialViewResult> ExibirGuiaParaImpressao(string nomeGuia)
        {
            var viewModel = new ExibirGuiaImpressaoViewModel();
            viewModel.Guia = nomeGuia;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_ExibirGuiaImpressao.cshtml", viewModel);
        }

        public PartialViewResult _PesquisarPreAtendimento()
        {
            var model = new PreAtendimentosViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_PesquisarPreAtendimentos.cshtml", model);
        }

        public PartialViewResult _PesquisarPaciente()
        {
            var model = new PacientesViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/_PesquisarPacientes.cshtml", model);
        }

        public PartialViewResult _SelecionarreAtendimentoModal(PreAtendimentosViewModel model)
        {
            ViewBag.DivisaoPrincipalId = model.Filtro;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/_SelecionarPreAtendimentoModal.cshtml", model);

            //public PartialViewResult _PreAtendimento()
            //{
            //    var model = new PreAtendimentosViewModel();
            //    //return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/PreAtendimentos/Index.cshtml", model);
            //    return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/PreAtendimentos/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _CriarOuEditarPreAtendimento()
        {
            CriarOuEditarPreAtendimentoModalViewModel viewModel;
            viewModel = new CriarOuEditarPreAtendimentoModalViewModel(new CriarOuEditarPreAtendimento());

            using (var sexoAppService = IocManager.Instance.ResolveAsDisposable<ISexoAppService>())
            {
                var sexos = await sexoAppService.Object.ListarTodos().ConfigureAwait(false);
                viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/PreAtendimentos/_CriarOuEditarModal.cshtml",
                    viewModel);
            }
        }

        public PartialViewResult _ClassificacaoRisco()
        {
            var model = new ClassificacoesRiscoViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/ClassificacoesRisco/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _CriarOuEditarClassificacaoRisco()
        {
            using (var _especialidadeAppService = IocManager.Instance.ResolveAsDisposable<IEspecialidadeAppService>())
            {

                var especialidades = await _especialidadeAppService.Object.ListarTodos().ConfigureAwait(false);

                CriarOuEditarClassificacaoRiscoModalViewModel viewModel;

                viewModel = new CriarOuEditarClassificacaoRiscoModalViewModel(new CriarOuEditarClassificacaoRisco());
                viewModel.Especialidades = new SelectList(especialidades.Items, "Id", "Nome");

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/ClassificacoesRisco/_CriarOuEditarModal.cshtml",
                    viewModel);
            }
        }

        public async Task<PartialViewResult> _IdentificacaoPaciente(long? id)
        {
            //var origens = await _origemAppService.Listar(new ListarOrigensInput());
            //  var sexos = await _sexoAppService.ListarTodos();
            // var coresPele = await _corPeleAppService.ListarTodos();
            // var escolaridades = await _escolaridadeAppService.ListarTodos();
            // var religioes = await _religiaoAppService.ListarTodos();
            // var estadosCivis = await _estadoCivilAppService.ListarTodos();
            //var tiposTelefone = await _tipoTelefoneAppService.ListarTodos();

            CriarOuEditarPacienteModalViewModel viewModel;

            if (id.HasValue)
            {
                using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
                {
                    var output = await pacienteAppService.Object.Obter2((long)id).ConfigureAwait(false);

                    viewModel = new CriarOuEditarPacienteModalViewModel(output);
                }

                //viewModel.Origens = new SelectList(origens.Items, "Id", "Descricao", output.OrigemId);
                //viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao", output.Sexo);
                //viewModel.Escolaridades = new SelectList(escolaridades.Items, "Id", "Descricao", output.Escolaridade);
                //viewModel.CoresPele = new SelectList(coresPele.Items, "Id", "Descricao", output.CorPele);
                //viewModel.Religioes = new SelectList(religioes.Items, "Id", "Descricao", output.Religiao);
                //     viewModel.EstadosCivis = new SelectList(estadosCivis.Items, "Id", "Descricao", output.EstadoCivil);
                //  viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");
            }
            else
            {
                viewModel = new CriarOuEditarPacienteModalViewModel(new PacienteDto());
                //viewModel.Origens = new SelectList(origens.Items, "Id", "Descricao");
                //viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
                //viewModel.CoresPele = new SelectList(coresPele.Items, "Id", "Descricao");
                //viewModel.Escolaridades = new SelectList(escolaridades.Items, "Id", "Descricao");
                //viewModel.Religioes = new SelectList(religioes.Items, "Id", "Descricao");
                //viewModel.EstadosCivis = new SelectList(estadosCivis.Items, "Id", "Descricao");
                //viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/Pacientes/_IdentificacaoPaciente.cshtml", viewModel);
            //return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Atendimentos/_IdentificacaoPaciente.cshtml", viewModel);
        }

        public async Task<PartialViewResult> _Agendamento()
        {

            using (var agendamentoConsultaMedicoDisponibilidadeAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaMedicoDisponibilidadeAppService>())
            using (var especialidadeAppService = IocManager.Instance.ResolveAsDisposable<IEspecialidadeAppService>())
            {
                var agendamentos = await agendamentoConsultaMedicoDisponibilidadeAppService.Object.ListarAtivos(0, 0)
                                       .ConfigureAwait(false);
                var especialidadesAgendadas =
                    agendamentos.Select(m => m.MedicoEspecialidade.EspecialidadeId).Distinct().ToList();
                var especialidades =
                    await especialidadeAppService.Object.Listar(especialidadesAgendadas).ConfigureAwait(false);
                var viewModel = new AgendamentoConsultasViewModel
                {
                    Especialidades = new SelectList(especialidades.Items, "Id", "Nome")
                };

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/Agendamentos/Index.cshtml",
                    viewModel);
            }
        }

        #endregion

        #region Metodos auxiliares

        [HttpPost]
        public async Task<long> SalvarAtendimento(AtendimentoDto atendimento)
        {
            //AtendimentoDto atend = new AtendimentoDto();
            long atend = new long();
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var retornoAtendimento = await atendimentoAppService.Object.CriarOuEditar(atendimento).ConfigureAwait(false);
                // return Content(L("Sucesso"));

                atend = retornoAtendimento.ReturnObject.Id;
                return atend;
            }
        }

        [HttpPost]
        public async Task<long> SalvarPreAtendimento(CriarOuEditarPreAtendimento preAtendimento)
        {
            using (var preAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IPreAtendimentoAppService>())
            {
                var preAtendimentoInserido =
                    await preAtendimentoAppService.Object.CriarGetId(preAtendimento).ConfigureAwait(false);
                return preAtendimentoInserido;
            }
        }

        //[HttpPost]
        //public async Task<string> ImprimirGuia(CriarOuEditarAtendimento at)
        //{
        //    var id = input.Id;
        //    var atendimento = await _atendimentoAppService.Obter(id);
        //    string guiaTemplate = Caminhos.Atendimentos.Guias.GuiasModelo + atendimento.Guia.Descricao + ".pdf";
        //    string guiaDestino = Caminhos.Atendimentos.Guias.GuiasModelo + atendimento.Guia.Descricao + "_" + atendimento.Id.ToString() + ".pdf";

        //    try
        //    {
        //        ByteBuffer.HIGH_PRECISION = true;
        //        PdfReader reader = new PdfReader(guiaTemplate);
        //        using (FileStream fs = new FileStream(guiaDestino, FileMode.Create, FileAccess.Write, FileShare.None))
        //        using (PdfStamper stamper = new PdfStamper(reader, fs))
        //        {
        //            PdfContentByte cb = stamper.GetOverContent(1);
        //            PdfLayer camada = new PdfLayer("CamadaDados", stamper.Writer);
        //            cb.BeginLayer(camada);
        //            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
        //            PdfGState gState = new PdfGState();
        //            cb.SetGState(gState);
        //            cb.SetColorFill(BaseColor.BLACK);
        //            cb.BeginText();



        //            Atendimento atd = atendimento.MapTo<Atendimento>();
        //            var atendimentoProps = atd.GetType().GetProperties();
        //            var guia = await _guiaAppService.Obter(atd.Guia.Id);
        //            CriarOuEditarGuia guiaConsulta = guia;

        //            foreach (var item in guiaConsulta.Campos)
        //            {
        //                guiaConsulta.NomesCampos.Add(item.GuiaCampo.Descricao);
        //            }

        //            foreach (PropertyInfo p in atendimentoProps)
        //            {
        //                // temp?
        //                if (p.PropertyType != typeof(Paciente) && p.PropertyType != typeof(Medico) && p.PropertyType != typeof(Plano) && p.PropertyType != typeof(Convenio))
        //                    continue;

        //                string nomeProp = p.Name;

        //                if (guiaConsulta.NomesCampos.Contains(nomeProp))
        //                {
        //                    // retirar os try/catch quando terminar os testes
        //                    try
        //                    {
        //                        var valor = atd[nomeProp];
        //                        if (!String.IsNullOrWhiteSpace(valor.ToString()))
        //                        {
        //                            List<RelacaoGuiaCampoDto> campos = null;// guiaConsulta.Campos.ToList();
        //                            var item = campos.Find(c => c.GuiaCampo.Descricao == nomeProp);
        //                            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX, item.CoordenadaY, 0f);
        //                        }
        //                    }
        //                    catch (Exception ex) { ex.ToString(); }
        //                }

        //                var pProps = p.PropertyType.GetProperties();

        //                foreach (PropertyInfo pp in pProps)
        //                {
        //                    // temp?
        //                    if (pp.PropertyType != typeof(string) && pp.PropertyType != typeof(DateTime) && pp.PropertyType != typeof(Cidade))
        //                        continue;

        //                    string nomeProp2 = nomeProp + "." + pp.Name;

        //                    if (guiaConsulta.NomesCampos.Contains(nomeProp2))
        //                    {
        //                        // retirar os try/catch quando terminar os testes
        //                        try
        //                        {
        //                            var valor = atd[nomeProp2];
        //                            if (!String.IsNullOrWhiteSpace(valor.ToString()))
        //                            {
        //                                List<RelacaoGuiaCampoDto> campos = null;//guiaConsulta.Campos.ToList();
        //                                var item = campos.Find(c => c.GuiaCampo.Descricao == nomeProp2);

        //                                var page = reader.GetPageSize(1);

        //                                //    double pag = page.Height * 1.3f;

        //                                //double pag = page.Height;

        //                                //double y = (item.CoordenadaY * page.Height) / pag;

        //                                //y = ((page.Height - y) * 1.1);

        //                                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX * 0.55f, (float)y, 0f);

        //                                var y = page.Height - item.CoordenadaY - 15;

        //                                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX, y, 0f);
        //                            }
        //                        }
        //                        catch (Exception ex) { ex.ToString(); }
        //                    }

        //                    var bases = guiaConsulta.PegarClassesBase(pp.PropertyType);

        //                    foreach (Type t in bases)
        //                    {
        //                        var ppProps = t.GetProperties();

        //                        foreach (var ppp in ppProps)
        //                        {
        //                            if (ppp.PropertyType != typeof(string) && ppp.PropertyType != typeof(DateTime))
        //                                continue;

        //                            string nomeProp3 = nomeProp2 + "." + ppp.Name;

        //                            if (guiaConsulta.NomesCampos.Contains(nomeProp3))
        //                            {
        //                                var valor = atd[nomeProp3];
        //                                if (!String.IsNullOrWhiteSpace(valor.ToString()))
        //                                {
        //                                    List<RelacaoGuiaCampoDto> campos = null;//guiaConsulta.Campos.ToList();
        //                                    var item = campos.Find(c => c.GuiaCampo.Descricao == nomeProp3);
        //                                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX, item.CoordenadaY, 0f);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            // Logo
        //            string logoPath = @"C:\Users\SWDev\Source\Repos\SW10.SWMANAGER\SW10.SWMANAGER.Web\Areas\Mpa\Views\Aplicacao\Atendimentos\Home\Arquivos\logo.png";
        //            Image logo = Image.GetInstance(logoPath);
        //            logo.ScalePercent(75.0f);
        //            logo.Alignment = Element.ALIGN_CENTER;
        //            logo.SetAbsolutePosition(50.0f, 740.0f);
        //            cb.AddImage(logo);

        //            cb.EndText();
        //            cb.EndLayer();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }


        //    string nomeArquivo = atendimento.Guia.Descricao + "_" + atendimento.Id.ToString() + ".pdf";
        //    TempData["guiaParaImprimir"] = guiaDestino;
        //    return nomeArquivo;
        //}


        // COMENTADO TEMPORARIAMENTE
        //[HttpPost]
        //public async Task<string> ImprimirGuia(CriarOuEditarAtendimento input)
        //{
        //    var id = input.Id;
        //    var criarOuEditarAtendimento = await _atendimentoAppService.Obter(id);
        //    string guiaTemplate = Caminhos.Atendimentos.Guias.GuiasModelo + criarOuEditarAtendimento.Guia.Descricao + ".pdf";
        //    string guiaDestino = Caminhos.Atendimentos.Guias.GuiasModelo + criarOuEditarAtendimento.Guia.Descricao + "_" + criarOuEditarAtendimento.Id.ToString() + ".pdf";

        //    try
        //    {
        //        ByteBuffer.HIGH_PRECISION = true;
        //        PdfReader reader = new PdfReader(guiaTemplate);
        //        using (FileStream fs = new FileStream(guiaDestino, FileMode.Create, FileAccess.Write, FileShare.None))
        //        using (PdfStamper stamper = new PdfStamper(reader, fs))
        //        {
        //            PdfContentByte cb = stamper.GetOverContent(1);
        //            PdfLayer camada = new PdfLayer("CamadaDados", stamper.Writer);
        //            cb.BeginLayer(camada);
        //            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
        //            PdfGState gState = new PdfGState();
        //            cb.SetGState(gState);
        //            cb.SetColorFill(BaseColor.BLACK);
        //            cb.BeginText();

        //            Atendimento atendimento = criarOuEditarAtendimento.MapTo<Atendimento>();
        //            var atendimentoProps = atendimento.GetType().GetProperties();
        //            var guia = await _guiaAppService.Obter(atendimento.Guia.Id);
        //            CriarOuEditarGuia guiaConsulta = guia;

        //            List<RelacaoGuiaCampoDto> campos = guiaConsulta.Campos.ToList();

        //            //List<Type> tiposValidos = new List<Type>();


        //            foreach (var item in guiaConsulta.Campos)
        //            {
        //                guiaConsulta.NomesCampos.Add(item.GuiaCampo.Descricao);
        //            }

        //            foreach (PropertyInfo p in atendimentoProps)
        //            {
        //                // temp?
        //                if (p.PropertyType != typeof(Paciente) && p.PropertyType != typeof(Medico) && p.PropertyType != typeof(Plano) && p.PropertyType != typeof(Convenio))
        //                    continue;

        //                string nomeProp = p.Name;

        //                // verificacao se campo contem '.' nao e propriedade (e apenas classe/conjunto) // temp
        //                if (nomeProp.Contains('.') && guiaConsulta.NomesCampos.Contains(nomeProp))
        //                {
        //                    // retirar os try/catch quando terminar os testes
        //                    try
        //                    {
        //                        var valor = atendimento[nomeProp];
        //                        if (!String.IsNullOrWhiteSpace(valor.ToString()))
        //                        {
        //                            var item = campos.FirstOrDefault(c => c.GuiaCampo.Descricao == nomeProp);// apenas First?
        //                            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX, item.CoordenadaY, 0f);
        //                            campos.FirstOrDefault(c => c.GuiaCampo.Descricao == nomeProp).GuiaCampo.Descricao = string.Empty; // evitando repeticao (temp)
        //                        }
        //                    }
        //                    catch (Exception ex) { ex.ToString(); }
        //                }

        //                var pProps = p.PropertyType.GetProperties();

        //                foreach (PropertyInfo pp in pProps)
        //                {
        //                    // temp?
        //                    if (pp.PropertyType != typeof(string) && pp.PropertyType != typeof(DateTime) && pp.PropertyType != typeof(Cidade))
        //                        continue;

        //                    string nomeProp2 = nomeProp + "." + pp.Name;

        //                    if (guiaConsulta.NomesCampos.Contains(nomeProp2))
        //                    {
        //                        // retirar os try/catch quando terminar os testes
        //                        try
        //                        {
        //                            var valor = atendimento[nomeProp2];
        //                            if (!String.IsNullOrWhiteSpace(valor.ToString()))
        //                            {
        //                                var item = campos.Find(c => c.GuiaCampo.Descricao == nomeProp2);
        //                                var page = reader.GetPageSize(1);
        //                                var y = page.Height - item.CoordenadaY - 15;
        //                                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX, y, 0f);
        //                                item.GuiaCampo.Descricao = string.Empty;
        //                            }
        //                        }
        //                        catch (Exception ex) { ex.ToString(); }
        //                    }

        //                    var bases = guiaConsulta.PegarClassesBase(pp.PropertyType);

        //                    foreach (Type t in bases)
        //                    {
        //                        var ppProps = t.GetProperties();

        //                        foreach (var ppp in ppProps)
        //                        {
        //                            if (ppp.PropertyType != typeof(string) && ppp.PropertyType != typeof(DateTime))
        //                                continue;

        //                            string nomeProp3 = nomeProp2 + "." + ppp.Name;

        //                            if (guiaConsulta.NomesCampos.Contains(nomeProp3))
        //                            {
        //                                var valor = atendimento[nomeProp3];
        //                                if (!String.IsNullOrWhiteSpace(valor.ToString()))
        //                                {
        //                                    var item = campos.Find(c => c.GuiaCampo.Descricao == nomeProp3);
        //                                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX, item.CoordenadaY, 0f);
        //                                    item.GuiaCampo.Descricao = string.Empty;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            try
        //            {
        //                string logoPath = Caminhos.Atendimentos.Guias.GuiasModelo + "logo.png";

        //                Image logo = Image.GetInstance(logoPath);
        //                logo.ScalePercent(75.0f);
        //                logo.Alignment = Element.ALIGN_CENTER;
        //                logo.SetAbsolutePosition(50.0f, 740.0f);
        //                cb.AddImage(logo);
        //            }
        //            catch {}

        //            cb.EndText();
        //            cb.EndLayer();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }

        //    string nomeArquivo = criarOuEditarAtendimento.Guia.Descricao + "_" + criarOuEditarAtendimento.Id.ToString() + ".pdf";
        //    TempData["guiaParaImprimir"] = guiaDestino;
        //    return nomeArquivo;
        //}
        // FIM COMENTADO TEMPORARIAMENTE


        //[HttpPost]
        //public async Task<string> ImprimirGuia(CriarOuEditarAtendimento at)
        //{
        //    var id = input.Id;
        //    var atendimento = await _atendimentoAppService.Obter(id);
        //    string guiaTemplate = Caminhos.Atendimentos.Guias.GuiasModelo + atendimento.Guia.Descricao + ".pdf";
        //    string guiaDestino = Caminhos.Atendimentos.Guias.GuiasModelo + atendimento.Guia.Descricao + "_" + atendimento.Id.ToString() + ".pdf";

        //    try
        //    {
        //        ByteBuffer.HIGH_PRECISION = true;
        //        PdfReader reader = new PdfReader(guiaTemplate);
        //        using (FileStream fs = new FileStream(guiaDestino, FileMode.Create, FileAccess.Write, FileShare.None))
        //        using (PdfStamper stamper = new PdfStamper(reader, fs))
        //        {
        //            PdfContentByte cb = stamper.GetOverContent(1);
        //            PdfLayer camada = new PdfLayer("CamadaDados", stamper.Writer);
        //            cb.BeginLayer(camada);
        //            cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
        //            PdfGState gState = new PdfGState();
        //            cb.SetGState(gState);
        //            cb.SetColorFill(BaseColor.BLACK);
        //            cb.BeginText();



        //            Atendimento atd = atendimento.MapTo<Atendimento>();
        //            var atendimentoProps = atd.GetType().GetProperties();
        //            var guia = await _guiaAppService.Obter(atd.Guia.Id);
        //            CriarOuEditarGuia guiaConsulta = guia;

        //            foreach (var item in guiaConsulta.Campos)
        //            {
        //                guiaConsulta.NomesCampos.Add(item.GuiaCampo.Descricao);
        //            }

        //            foreach (PropertyInfo p in atendimentoProps)
        //            {
        //                // temp?
        //                if (p.PropertyType != typeof(Paciente) && p.PropertyType != typeof(Medico) && p.PropertyType != typeof(Plano) && p.PropertyType != typeof(Convenio))
        //                    continue;

        //                string nomeProp = p.Name;

        //                if (guiaConsulta.NomesCampos.Contains(nomeProp))
        //                {
        //                    // retirar os try/catch quando terminar os testes
        //                    try
        //                    {
        //                        var valor = atd[nomeProp];
        //                        if (!String.IsNullOrWhiteSpace(valor.ToString()))
        //                        {
        //                            List<RelacaoGuiaCampoDto> campos = null;// guiaConsulta.Campos.ToList();
        //                            var item = campos.Find(c => c.GuiaCampo.Descricao == nomeProp);
        //                            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX, item.CoordenadaY, 0f);
        //                        }
        //                    }
        //                    catch (Exception ex) { ex.ToString(); }
        //                }

        //                var pProps = p.PropertyType.GetProperties();

        //                foreach (PropertyInfo pp in pProps)
        //                {
        //                    // temp?
        //                    if (pp.PropertyType != typeof(string) && pp.PropertyType != typeof(DateTime) && pp.PropertyType != typeof(Cidade))
        //                        continue;

        //                    string nomeProp2 = nomeProp + "." + pp.Name;

        //                    if (guiaConsulta.NomesCampos.Contains(nomeProp2))
        //                    {
        //                        // retirar os try/catch quando terminar os testes
        //                        try
        //                        {
        //                            var valor = atd[nomeProp2];
        //                            if (!String.IsNullOrWhiteSpace(valor.ToString()))
        //                            {
        //                                List<RelacaoGuiaCampoDto> campos = null;//guiaConsulta.Campos.ToList();
        //                                var item = campos.Find(c => c.GuiaCampo.Descricao == nomeProp2);

        //                                var page = reader.GetPageSize(1);

        //                                //    double pag = page.Height * 1.3f;

        //                                //double pag = page.Height;

        //                                //double y = (item.CoordenadaY * page.Height) / pag;

        //                                //y = ((page.Height - y) * 1.1);

        //                                //cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX * 0.55f, (float)y, 0f);

        //                                var y = page.Height - item.CoordenadaY - 15;

        //                                cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX, y, 0f);
        //                            }
        //                        }
        //                        catch (Exception ex) { ex.ToString(); }
        //                    }

        //                    var bases = guiaConsulta.PegarClassesBase(pp.PropertyType);

        //                    foreach (Type t in bases)
        //                    {
        //                        var ppProps = t.GetProperties();

        //                        foreach (var ppp in ppProps)
        //                        {
        //                            if (ppp.PropertyType != typeof(string) && ppp.PropertyType != typeof(DateTime))
        //                                continue;

        //                            string nomeProp3 = nomeProp2 + "." + ppp.Name;

        //                            if (guiaConsulta.NomesCampos.Contains(nomeProp3))
        //                            {
        //                                var valor = atd[nomeProp3];
        //                                if (!String.IsNullOrWhiteSpace(valor.ToString()))
        //                                {
        //                                    List<RelacaoGuiaCampoDto> campos = null;//guiaConsulta.Campos.ToList();
        //                                    var item = campos.Find(c => c.GuiaCampo.Descricao == nomeProp3);
        //                                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, valor.ToString(), item.CoordenadaX, item.CoordenadaY, 0f);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            // Logo
        //            string logoPath = @"C:\Users\SWDev\Source\Repos\SW10.SWMANAGER\SW10.SWMANAGER.Web\Areas\Mpa\Views\Aplicacao\Atendimentos\Home\Arquivos\logo.png";
        //            Image logo = Image.GetInstance(logoPath);
        //            logo.ScalePercent(75.0f);
        //            logo.Alignment = Element.ALIGN_CENTER;
        //            logo.SetAbsolutePosition(50.0f, 740.0f);
        //            cb.AddImage(logo);

        //            cb.EndText();
        //            cb.EndLayer();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }


        //    string nomeArquivo = atendimento.Guia.Descricao + "_" + atendimento.Id.ToString() + ".pdf";
        //    TempData["guiaParaImprimir"] = guiaDestino;
        //    return nomeArquivo;
        //}

        [HttpPost]
        public async Task<string> ImprimirGuia(long atendimentoId)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var _atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);
                var atendimento = AtendimentoDto.Mapear(_atendimento);

                using (var guiaAppService = IocManager.Instance.ResolveAsDisposable<IGuiaAppService>())
                {
                    var guia = await guiaAppService.Object.Obter((long)atendimento.GuiaId).ConfigureAwait(false);
                    var guiaTemplateBytes = guia.ModeloPDF;

                    string guiaDestino = @"C:\Users\SWDev\Documents\Sudano\teste.pdf";

                    try
                    {
                        ByteBuffer.HIGH_PRECISION = true;
                        PdfReader reader = new PdfReader(guiaTemplateBytes);
                        using (FileStream fs = new FileStream(
                            guiaDestino,
                            FileMode.Create,
                            FileAccess.Write,
                            FileShare.None))
                        using (PdfStamper stamper = new PdfStamper(reader, fs))
                        {
                            PdfContentByte cb = stamper.GetOverContent(1);
                            PdfLayer camada = new PdfLayer("CamadaDados", stamper.Writer);
                            cb.BeginLayer(camada);
                            cb.SetFontAndSize(
                                BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED),
                                10);
                            PdfGState gState = new PdfGState();
                            cb.SetGState(gState);
                            cb.SetColorFill(BaseColor.BLACK);
                            cb.BeginText();

                            var atendimentoProps = atendimento.GetType().GetProperties();


                            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer(new SimpleTypeResolver());
                            GuiaCampoDto[] campos = new GuiaCampoDto[] { };
                            campos = jsonSerializer.Deserialize<GuiaCampoDto[]>(guia.CamposJson);

                            foreach (var campo in campos)
                            {
                                if (!campo.Descricao.Contains("."))
                                {
                                    foreach (var subCampo in campo.SubConjuntos)
                                    {
                                        try
                                        {
                                            var valor = atendimento[subCampo.Descricao];
                                            if (!String.IsNullOrWhiteSpace(valor.ToString()))
                                            {
                                                var page = reader.GetPageSize(1);
                                                var y = page.Height - subCampo.CoordenadaY - 15;
                                                cb.ShowTextAligned(
                                                    PdfContentByte.ALIGN_LEFT,
                                                    valor.ToString(),
                                                    subCampo.CoordenadaX,
                                                    y,
                                                    0f);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            ex.ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        var valor = atendimento[campo.Descricao];
                                        if (!String.IsNullOrWhiteSpace(valor.ToString()))
                                        {
                                            var page = reader.GetPageSize(1);
                                            var y = page.Height - campo.CoordenadaY - 15;
                                            cb.ShowTextAligned(
                                                PdfContentByte.ALIGN_LEFT,
                                                valor.ToString(),
                                                campo.CoordenadaX,
                                                y,
                                                0f);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        ex.ToString();
                                    }
                                }
                            }

                            try
                            {
                                string logoPath = @"logo.png";

                                Image logo = Image.GetInstance(logoPath);
                                logo.ScalePercent(75.0f);
                                logo.Alignment = Element.ALIGN_CENTER;
                                logo.SetAbsolutePosition(50.0f, 740.0f);
                                cb.AddImage(logo);
                            }
                            catch
                            {
                            }

                            cb.EndText();
                            cb.EndLayer();
                        }
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }

                    //TempData["guiaParaImprimir"] = guiaDestino;// esta sendo usado
                    return guiaDestino;
                }
            }
        }



        [HttpPost]
        public async Task<string> ChecarControleAlta(long id)
        {
            using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
            {

                return await unidadeOrganizacionalAppService.Object.ChecarControlaAlta(id).ConfigureAwait(false);
            }
        }

        #endregion
    }
}