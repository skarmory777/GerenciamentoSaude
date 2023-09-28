using Abp.Dependency;
using Abp.UI;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.AtestadosMedicos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.FichasPacientes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Home;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Prescricoes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.SolicitacoesExames;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.ProntuarioEletronico;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Receituarios;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Threading;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto.BalancoHidricos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SolicitacaoAutorizacoes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Home;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Assistenciais
{
    using Abp.Application.Services.Dto;
    using Abp.Collections.Extensions;
    using Abp.Domain.Repositories;
    using ClassesAplicacao;
    using ClassesAplicacao.Services.Configuracoes.Empresas;
    using Microsoft.Reporting.WebForms;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.PacienteAlergias;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.PacienteDiagnosticos;
    using System.Data.Entity;
    using System.Globalization;
    using System.IO;
    using System.Web;
    using System.Web.Http;
    using System.Web.UI;
    using Web.Relatorios.Assistenciais.SolicitacaoExames;

    public class AssistenciaisController : SWMANAGERControllerBase
    {

        // GET: Mpa/Assistenciais
        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AmbulatoriosEmergencias(long? id)
        {
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var localChamadaAppService = IocManager.Instance.ResolveAsDisposable<ILocalChamadasAppService>())
            using (var medicoAppService = IocManager.Instance.ResolveAsDisposable<IMedicoAppService>())
            using (var atendimentoService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var user = await userAppService.Object.GetUser().ConfigureAwait(false);
                var model = new AtendimentoDto();

                if (id.HasValue)
                {
                    model = await atendimentoService.Object.Obter(id.Value).ConfigureAwait(false);
                }

                var viewModel = new AssistenciaisViewModel(model)
                {
                    IsAmbulatorioEmergencia = true,
                    IsInternacao = false
                };

                var coockie = this.Request.Cookies["localChamada"];
                var localChamadaId = 0L;

                if (coockie != null && !string.IsNullOrWhiteSpace(coockie.Value))
                {
                    if (long.TryParse(coockie.Value, out localChamadaId))
                    {
                        var localChamada = await localChamadaAppService.Object.Obter(localChamadaId).ConfigureAwait(false);
                        this.ViewBag.LocalChamadaId = localChamadaId;
                        this.ViewBag.LocalChamada = localChamada;
                        this.ViewBag.TipoLocalChamadaId = localChamada.TipoLocalChamadaId;
                        this.ViewBag.TipoLocalChamada = localChamada.TipoLocalChamada;
                    }
                }

                if (user.MedicoId.HasValue)
                {
                    this.ViewBag.UserMedicoId = user.MedicoId;
                    this.ViewBag.UserMedico = await medicoAppService.Object.Obter(user.MedicoId.Value).ConfigureAwait(false);
                }

                return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Home/Index.cshtml", viewModel);
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> Internacoes(long? id)
        {
            var model = new AtendimentoDto();
            var viewModel = new AssistenciaisViewModel(model)
            {
                IsAmbulatorioEmergencia = false,
                IsInternacao = true
            };

            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var medicoAppService = IocManager.Instance.ResolveAsDisposable<IMedicoAppService>())
            using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
            using (var atendimentoStatusRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoStatus, long>>())
            {
                if (id.HasValue)
                {
                    model = await atendimentoAppService.Object.Obter(id.Value).ConfigureAwait(false) ?? new AtendimentoDto();

                    viewModel = new AssistenciaisViewModel(model);
                }

                var user = await userAppService.Object.GetUser().ConfigureAwait(false);

                if (user.MedicoId.HasValue)
                {
                    ViewBag.UserMedicoId = user.MedicoId;

                    ViewBag.UserMedico = await medicoAppService.Object.Obter(user.MedicoId.Value).ConfigureAwait(false);
                }

                viewModel.UnidadeOrganizacionais = (await unidadeOrganizacionalAppService.Object.ListarParaInternacao().ConfigureAwait(false)).Items.ToList();

                viewModel.ListaAtendimentoStatus = (await atendimentoStatusRepository.Object.GetAllListAsync().ConfigureAwait(false)).ToList().Select(AtendimentoStatusDto.Mapear).ToList();

                // var listaStatus = await _prescricaoStatusAppService.ListarTodos();

                // if (listaStatus != null && listaStatus.Items != null)
                // {
                // viewModel.ListaStatus = listaStatus.Items.ToList();
                // }
                // else
                // {
                // viewModel.ListaStatus = new List<PrescricaoStatusDto>();
                // }
                return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Home/IndexInternacao.cshtml", viewModel);
            }
        }


        public async Task<ViewResult> DetalhamentoQuantidadeExames(long? id, string tipo)
        {
            var viewModel = new DetalhamentoQuantidadeExamesViewModel();
            viewModel.Tipo = tipo == "Lab" ? "Laboratório" : "Imagem";
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(id.Value);

                viewModel.Paciente = atendimento?.Paciente?.NomeCompleto;

                viewModel.TotalResultado = await atendimentoAppService.Object.TotalExamesResultados(id.Value, tipo);

                viewModel.TotalSolicitado = await atendimentoAppService.Object.TotalExamesSolicitados(id.Value, tipo);

                return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Home/DetalhamentoQuantidadeExames.cshtml", viewModel);
            }
        }


        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ViewResult> FichaPaciente(long? id)
        {
            try
            {
                var model = new PacienteDto();
                using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
                {
                    if (id.HasValue)
                    {
                        model = await pacienteAppService.Object.Obter((long)id).ConfigureAwait(false);
                    }
                    else
                    {
                        var paciente = this.TempData.Peek("Paciente") as PacienteDto;
                        if (paciente != null)
                        {
                            model = paciente;
                        }
                    }

                    var viewModel = new FichasPacientesViewModel
                    {
                        CriarOuEditarPacienteModalViewModel = new CriarOuEditarPacienteModalViewModel(model)
                    };

                    // viewModel;

                    return this.View(
                        "~/Areas/Mpa/Views/Aplicacao/Assistenciais/FichasPacientes/Index.cshtml",
                        viewModel);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroOcorrido", ex.Message.ToString()));
            }
        }


        public async Task<ActionResult> AtestadosMedicos(long id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var mailingTemplateAppService = IocManager.Instance.ResolveAsDisposable<IMailingTemplateAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(id).ConfigureAwait(false);
                var paciente = this.TempData.Peek("Paciente") as PacienteDto;
                var medico = this.TempData.Peek("Medico") as MedicoDto;
                var empresa = this.TempData.Peek("Empresa") as EmpresaDto;

                var viewModel = new AtestadoMedicoViewModel();
                var templates = await mailingTemplateAppService.Object.ListarTodos().ConfigureAwait(false);
                viewModel.Templates = templates.Items.Where(m => m.Name.Contains("Atestado Médico")).ToList();
                if (paciente != null)
                {
                    viewModel.Paciente = paciente;
                }

                if (medico != null)
                {
                    viewModel.Medico = medico;
                }

                if (empresa != null)
                {
                    viewModel.Empresa = empresa;
                }

                return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/AtestadosMedicos/Index.cshtml", viewModel);
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<PartialViewResult> _LerAtendimento(long id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var prescricaoStatusAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoStatusAppService>())
            {
                var atendimento = await atendimentoAppService.Object.ObterAssistencial(id).ConfigureAwait(false);
                //this.TempData["Atendimento"] = atendimento;

                var listaStatus = await prescricaoStatusAppService.Object.ListarTodos().ConfigureAwait(false);

                if (listaStatus != null && listaStatus.Items != null)
                {
                    atendimento.ListaStatus = listaStatus.Items.ToList();
                }
                else
                {
                    atendimento.ListaStatus = new List<PrescricaoStatusDto>();
                }


                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Home/_LerAtendimento.cshtml",
                    atendimento);
            }
        }
        
        [ChildActionOnly]
        public ActionResult _headerAtendimentoPacienteNavBar(long atendimentoId, HeaderAtendimentoPacienteNavBarOptions options)
        {
            using (var atendimentoService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                return AsyncHelper.RunSync(async() =>
                {
                    var model = await atendimentoService.Object.Obter(atendimentoId).ConfigureAwait(false);

                    var viewModel = new AssistenciaisViewModel(model, options)
                    {
                        IsAmbulatorioEmergencia = !model.IsInternacao,
                        IsInternacao = model.IsInternacao
                    };

                    return this.PartialView(
                        "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Home/_headerAtendimentoPacienteNavBar.cshtml",
                        viewModel);
                });
            }
        }
        



        public async Task<PartialViewResult> PendenciaFinalizarAtendimento(long atendimentoId)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.ObterAssistencial(atendimentoId).ConfigureAwait(false);

                var viewModel = new PendenteModalViewModel
                {
                    AtendimentoId = atendimento.Id,
                    IsPendenteExames = atendimento.IsPendenteExames,
                    IsPendenteProcedimento = atendimento.IsPendenteProcedimento,
                    IsPendenteMedicacao = atendimento.IsPendenteMedicacao,
                };

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Atendimentos/Altas/_PendenteModal.cshtml",
                    viewModel);
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> HeaderAtendimento(long id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(id).ConfigureAwait(false);

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_HeaderPaciente.cshtml",
                    atendimento);
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        private async Task<ActionResult> IndexProntuarioEletronico(long? idAtendimento, string viewBagTitle, string activePage = null)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var operacaoAppService = IocManager.Instance.ResolveAsDisposable<IOperacaoAppService>())
            {
                activePage = activePage ?? this.TempData.Peek("ActivePage").ToString();
                var operacao = await operacaoAppService.Object.ObterPorNome(activePage).ConfigureAwait(false);
                this.TempData["OperacaoId"] = operacao.Id;
                this.ViewBag.Title = viewBagTitle;
                var viewModel = new AssistencialAtendimentoViewModel
                {
                    Permission = operacao.Name
                };

                if (idAtendimento.HasValue)
                {
                    viewModel.Atendimento =
                        await atendimentoAppService.Object.Obter(idAtendimento.Value).ConfigureAwait(false);
                }

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/Index.cshtml",
                    viewModel);
            }
        }

        #region enfermagem
        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> EnfermagemAdmissao(long? id)
        {
            return await this.IndexProntuarioEletronico(id, "EnfermagemAdmissao").ConfigureAwait(false);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> EnfermagemEvolucao(long? id)
        {
            return await this.IndexProntuarioEletronico(id, "EnfermagemEvolucao").ConfigureAwait(false);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> EnfermagemPassagemPlantao(long? id)
        {
            return await this.IndexProntuarioEletronico(id, "EnfermagemPassagemPlantao").ConfigureAwait(false);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult EnfermagemPrescricao()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Enfermagens/Prescricoes/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult EnfermagemSinalVital()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Enfermagens/SinaisVitais/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult EnfermagemChecagem()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Enfermagens/Checagens/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult EnfermagemControleBalancoHidrico()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Enfermagens/ControlesBalancoHidrico/Index.cshtml");
        }

        #endregion

        #region medico
        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoReceituario(long? id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var viewModel = new AssistencialAtendimentoViewModel();

                if (id.HasValue)
                {
                    viewModel.Atendimento = await atendimentoAppService.Object.Obter(id.Value).ConfigureAwait(false);
                }

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Receituarios/Index.cshtml",
                    viewModel);
            }
        }

        [HttpPost]
        public async Task<PartialViewResult> CriarOuEditarMedicoReceituario(long atendimentoId, long receituarioId)
        {
            using (var receituarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ReceituarioMedico, long>>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var receituario = await receituarioRepository.Object.GetAll().AsNoTracking().Include(x => x.Medico).Where(m => m.Id == receituarioId).FirstOrDefaultAsync().ConfigureAwait(false);
                var atendimentoDto = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);

                ViewBag.pacienteId = atendimentoDto.PacienteId;
                ViewBag.receituarioId = receituario.Id;
                ViewBag.atendimentoId = atendimentoId;
                ViewBag.medicoPrescritorMemedToken = receituario.Medico.PrescritorMemedToken;

                ViewBag.pacienteNome = atendimentoDto.Paciente.NomeCompleto;
                ViewBag.pacienteCpf = atendimentoDto.Paciente.Cpf;
                ViewBag.pacienteTelefone = atendimentoDto.Paciente.Telefone1;

                return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Receituarios/CriarOuEditarMedicoReceituario.cshtml");
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoAdmissao(long? id)
        {
            return await this.IndexProntuarioEletronico(id, "MedicoAdmissao").ConfigureAwait(false);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoAlta()
        {


            // var atendimentoDto = (AtendimentoDto)TempData["Atendimento"];
            // long atendimentoId = atendimentoDto.Id;

            ////  long.TryParse(TempData["Atendimento"].ToString(), out atendimentoId);


            // var atendimento = await _atendimentoAppService.Obter(atendimentoId);

            // AltaModalViewModel viewModel = new AltaModalViewModel(); ;

            // viewModel.AtendimentoId = atendimentoId;
            // viewModel.Data = (DateTime)(atendimento.DataAlta != null ? atendimento.DataAlta : DateTime.Now);
            // viewModel.DataAltaMedica = (DateTime)(atendimento.DataAltaMedica != null ? atendimento.DataAltaMedica : DateTime.Now);
            // viewModel.PrevisaoAlta = (DateTime)(atendimento.DataPrevistaAlta != null ? atendimento.DataPrevistaAlta : DateTime.Now);
            // if (atendimento.LeitoId.HasValue)
            // {
            // viewModel.LeitoId = atendimento.Leito.Id;
            // viewModel.Leito = atendimento.Leito;
            // }
            ////ViewBag.IsConsulta = false;

            // ViewBag.Title = "Alta";

            // return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Altas/Alta/_CriarOuEditarModal.cshtml", viewModel);

            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Altas/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoAnamnese(long? id)
        {
            return await this.IndexProntuarioEletronico(id, "MedicoAnamnese").ConfigureAwait(false);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoEvolucao(long? id)
        {
            return await this.IndexProntuarioEletronico(id, "MedicoEvolucao").ConfigureAwait(false);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoParecerEspecialista(long? id)
        {
            return await this.IndexProntuarioEletronico(id, "MedicoParecerEspecialista").ConfigureAwait(false);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoPrescricao(long? id)
        {
            var viewModel = new AssistencialAtendimentoViewModel();

            if (id.HasValue)
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var prescricaoStatusAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoStatusAppService>())
                {
                    viewModel.Atendimento = await atendimentoAppService.Object.Obter(id.Value).ConfigureAwait(false);

                    var listaStatus = await prescricaoStatusAppService.Object.ListarTodos().ConfigureAwait(false);

                    if (listaStatus != null && listaStatus.Items != null)
                    {
                        viewModel.Atendimento.ListaStatus = listaStatus.Items.ToList();
                    }
                    else
                    {
                        viewModel.Atendimento.ListaStatus = new List<PrescricaoStatusDto>();
                    }
                }
            }


            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/Index.cshtml", viewModel);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoSolicitacaoExame(long? id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var viewModel = new AssistencialAtendimentoViewModel();

                if (id.HasValue)
                {
                    viewModel.Atendimento = await atendimentoAppService.Object.Obter(id.Value).ConfigureAwait(false);
                }

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacoesExames/Index.cshtml",
                    viewModel);
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoResultadoExame(long? id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var viewModel = new AssistencialAtendimentoViewModel();

                if (id.HasValue)
                {
                    viewModel.Atendimento = await atendimentoAppService.Object.Obter(id.Value).ConfigureAwait(false);
                }

                return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/ExamesImagem/Index.cshtml", viewModel);
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> MedicoResumoAlta(long? id)
        {
            return await this.IndexProntuarioEletronico(id, "MedicoResumoAlta").ConfigureAwait(false);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult MedicoDescricaoAtoCirurgico()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/DescricoesAtosCirurgicos/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult MedicoDescricaoAtoAnestesico()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/DescricoesAtosAnestesicos/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult MedicoFolhaGastoCentroCirurgico()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/FolhasGastosCentroCirurgicos/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult MedicoPartograma()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Partogramas/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<PartialViewResult> CriarOuEditarAdmissaoMedica(long? atendimentoId, long? id)
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            using (var operacaoAppService = IocManager.Instance.ResolveAsDisposable<IOperacaoAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var prontuarioAppService = IocManager.Instance.ResolveAsDisposable<IProntuarioEletronicoAppService>())
            {
                if (!atendimentoId.HasValue)
                {
                    throw new UserFriendlyException("id do atendimento obrigatório.");
                }

                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId.Value).ConfigureAwait(false);
                ProntuarioEletronicoDto output;
                var formResposta = new FormResposta();
                if (id.HasValue)
                {
                    output = await prontuarioAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    formResposta = output.FormResposta;
                    if (output.FormRespostaId.HasValue)
                    {
                        if (output.FormResposta != null && formResposta.FormConfig != null)
                        {
                            this.ViewBag.FormName = formResposta.FormConfig.Nome;
                        }
                        else
                        {

                            var formConfig = await formConfigAppService.Object.Obter(output.FormResposta.FormConfigId)
                                                 .ConfigureAwait(false);
                            this.ViewBag.FormName = formConfig.Nome;
                        }
                    }
                    else
                    {
                        this.ViewBag.FormName = this.L("SelecionarLista");
                    }
                }
                else
                {
                    // CriarOuEditarAssistencialAtendimento
                    output = new ProntuarioEletronicoDto { AtendimentoId = atendimento.Id };
                }

                var viewModel = new CriarOuEditarProntuarioEletronicoViewModel(output);
                var activePage = this.TempData.Peek("ActivePage").ToString();
                var operacao = await operacaoAppService.Object.ObterPorNome(activePage).ConfigureAwait(false);
                var especialidadeId = atendimento.EspecialidadeId;
                var formsDisp = await formConfigAppService.Object.ListarRelacionados(
                                    operacao.Id,
                                    atendimento.UnidadeOrganizacionalId.Value,
                                    especialidadeId,
                                    atendimento.Id).ConfigureAwait(false);

                if (formsDisp.Any())
                {
                    viewModel.FormConfig = formsDisp.FirstOrDefault();
                    viewModel.FormConfigId = viewModel.FormConfig?.Id;
                }

                viewModel.Atendimento = atendimento;

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/ProntuarioEletronico/CriarOuEditarProntuarioEletronicoViewModel.cshtml",
                    viewModel);
            }
        }

        ////[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> CriarOuEditarSolicitacaoExameModal(long atendimentoId, long? id, bool? dataFutura)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var solicitacaoExamePrioridadeAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoExamePrioridadeAppService>())
            using (var solicitacaoExameAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoExameAppService>())
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var medicoAppService = IocManager.Instance.ResolveAsDisposable<IMedicoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);
                SolicitacaoExameDto output;


                if (id.HasValue)
                {
                    output = await solicitacaoExameAppService.Object.Obter(id.Value)
                                 .ConfigureAwait(
                                     false); // AsyncHelper.RunSync(() => _admissaoMedicaAppService.Obter(id.Value)); //await _admissaoMedicaAppService.Obter(id.Value);
                    var prioriade = await solicitacaoExamePrioridadeAppService.Object.Obter(output.Prioridade)
                                        .ConfigureAwait(false);
                    this.ViewBag.NomePrioridade = prioriade.Descricao;
                }
                else
                {
                    var prioridadeList =
                        await solicitacaoExamePrioridadeAppService.Object.ListarTodos().ConfigureAwait(false);
                    var prioridadeId = prioridadeList.Items.FirstOrDefault(c => string.Equals(c.Codigo, "2")) != null
                                           ? prioridadeList.Items.FirstOrDefault(c => string.Equals(c.Codigo, "2")).Id
                                           : 0;
                    
                    output = new SolicitacaoExameDto
                    {
                        DataSolicitacao = dataFutura.HasValue && dataFutura.Value ? DateTime.Today.AddDays(1) : DateTime.Now,
                        AtendimentoId = atendimento.Id,
                        Atendimento = atendimento,
                        Prioridade = dataFutura.HasValue && dataFutura.Value && !atendimento.IsAmbulatorioEmergencia ? 1 :(int)prioridadeId
                    };

                    var user = await userAppService.Object.GetUser().ConfigureAwait(false);

                    if (user.MedicoId.HasValue)
                    {
                        output.MedicoSolicitanteId = user.MedicoId;
                        output.MedicoSolicitante =
                            await medicoAppService.Object.Obter(user.MedicoId.Value).ConfigureAwait(false);
                    }

                    if (output.Prioridade != 0)
                    {
                        var prioriade = await solicitacaoExamePrioridadeAppService.Object.Obter(output.Prioridade)
                                            .ConfigureAwait(false);
                        this.ViewBag.NomePrioridade = prioriade.Descricao;
                    }
                }

                var viewModel = new CriarOuEditarSolicitacaoExameViewModel(output) { Atendimento = atendimento, DataFutura = dataFutura.HasValue && dataFutura.Value };


                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacoesExames/CriarOuEditarSolicitacaoExame.cshtml",
                    viewModel);
            }
        }

        ////[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<PartialViewResult> CriarOuEditarSolicitacaoExameItemModal(long atendimentoId, long solicitacaoId, long? id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var solicitacaoExameAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoExameAppService>())
            using (var kitExameAppService = IocManager.Instance.ResolveAsDisposable<IKitExameAppService>())
            using (var faturamentoItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemAppService>())
            using (var solicitacaoExameItemAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoExameItemAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);
                SolicitacaoExameItemDto output;

                if (id.HasValue)
                {
                    // output = await _solicitacaoExameItemAppService.ObterParaEdicao(id.Value);
                    output = await solicitacaoExameItemAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    if (output.FaturamentoItemId.HasValue)
                    {
                        output.FaturamentoItem = await faturamentoItemAppService.Object.Obter(output.FaturamentoItemId.Value)
                                                     .ConfigureAwait(false);
                    }

                    if (output.KitExameId.HasValue)
                    {
                        output.KitExame =
                            await kitExameAppService.Object.Obter(output.KitExameId.Value).ConfigureAwait(false);
                    }

                    output.Solicitacao = await solicitacaoExameAppService.Object.Obter(output.SolicitacaoExameId)
                                             .ConfigureAwait(false);

                    if (output.Solicitacao.AtendimentoId.HasValue)
                    {
                        if (output.Solicitacao.AtendimentoId.Value == atendimento.Id)
                        {
                            output.Solicitacao.Atendimento = atendimento;
                        }
                        else
                        {
                            output.Solicitacao.Atendimento =
                                await atendimentoAppService.Object.Obter(output.Solicitacao.AtendimentoId.Value)
                                    .ConfigureAwait(false);
                        }

                        atendimento = output.Solicitacao.Atendimento;
                    }
                }
                else
                {
                    var solicitacao = await solicitacaoExameAppService.Object.Obter(solicitacaoId).ConfigureAwait(false);
                    output = new SolicitacaoExameItemDto
                    {
                        DataValidade = DateTime.Now,
                        SolicitacaoExameId = solicitacao.Id,
                        Solicitacao = solicitacao
                    };
                }

                var vm = new CriarOuEditarSolicitacaoExameItemViewModel(output) { Atendimento = atendimento };
                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/SolicitacoesExames/CriarOuEditarSolicitacaoExameItem.cshtml",
                    vm);
            }
        }

        ////[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<PartialViewResult> _CriarOuEditarMedicoPrescricao(long atendimentoId, long? id, bool? dataFutura)
        {
            using (var prescricaoMedicaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoMedicaAppService>())
            using (var divisaoAppService = IocManager.Instance.ResolveAsDisposable<IDivisaoAppService>())
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
            using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
            {
                CriarOuEditarPrescricaoViewModel viewModel;
                if (id.HasValue)
                {
                    var output = await prescricaoMedicaAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    if (output.MedicoId.HasValue)
                    {
                        output.Medico = MedicoDto.Mapear(await medicoRepository.Object.GetAll().AsNoTracking().Include(x => x.SisPessoa).FirstOrDefaultAsync(x => x.Id == output.MedicoId.Value).ConfigureAwait(false));
                    }

                    viewModel = new CriarOuEditarPrescricaoViewModel(output);
                }
                else
                {
                    viewModel = new CriarOuEditarPrescricaoViewModel(new PrescricaoMedicaDto())
                    {
                        DataPrescricao = dataFutura.HasValue && dataFutura.Value ? DateTime.Today.AddDays(1) : DateTime.Now,
                        DataFuturaPrescricao = dataFutura.HasValue && dataFutura.Value ? DateTime.Today.AddDays(1) : (DateTime?)null
                    };
                }

                var atendimento = await atendimentoRepository.Object.GetAll().AsNoTracking()
                    .Select(x => new { Id = x.Id, UnidadeOrganizacionalId = x.UnidadeOrganizacionalId, LeitoId = x.LeitoId })
                    .FirstOrDefaultAsync(x => x.Id == atendimentoId).ConfigureAwait(false);
                viewModel.AtendimentoId = atendimento.Id;
                viewModel.UnidadeOrganizacionalId = atendimento.UnidadeOrganizacionalId;
                viewModel.AtendimentoLeitoId = atendimento.LeitoId;

                if (!id.HasValue)
                {
                    viewModel.LeitoId = atendimento.LeitoId;
                }

                var divisoes = await divisaoAppService.Object.ListarTodos().ConfigureAwait(false);
                viewModel.Divisoes = divisoes.Items.OrderBy(m => m.Ordem).ToList();
                // viewModel.Atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);

                var usuario = await usuarioRepository.Object.GetAll().Include(i => i.Medico).Include(i => i.Medico.MedicoEspecialidades).AsNoTracking().FirstOrDefaultAsync(w => w.Id == this.AbpSession.UserId).ConfigureAwait(false);

                if (usuario != null && usuario.MedicoId.HasValue)
                {
                    var medico = await medicoRepository.Object.GetAll().AsNoTracking().Include(x => x.SisPessoa).FirstOrDefaultAsync(x => x.Id == usuario.MedicoId.Value).ConfigureAwait(false);
                    viewModel.MedicoCorrente = MedicoDto.Mapear(medico);
                    if (viewModel.Id == 0)
                    {
                        viewModel.MedicoId = viewModel.MedicoCorrente.Id;
                        viewModel.Medico = viewModel.MedicoCorrente;
                    }
                }

                return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/_CriarOuEditar.cshtml", viewModel);
            }
        }


        [HttpPost]
        public async Task<PartialViewResult> _PrescricaoCompleta(long atendimentoId, [FromBody] long prescricaoMedicaId)
        {

            using (var divisaoAppService = IocManager.Instance.ResolveAsDisposable<IDivisaoAppService>())
            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
            using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var prescricaoMedicaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoMedicaAppService>())
            {
                var viewModel = new PrescricaoCompletaViewModel
                {
                    AtendimentoId = atendimentoId,
                    PrescricaoItemRespostas = await prescricaoMedicaAppService.Object.ListarRespostasPorPrescricaoCompleta(prescricaoMedicaId).ConfigureAwait(false),
                    Divisoes = (await divisaoAppService.Object.ListarTodos().ConfigureAwait(false)).Items.OrderBy(m => m.Ordem).ToList(),
                    PrescricaoMedicaId = prescricaoMedicaId
                };
                var usuario = await usuarioRepository.Object.GetAll().Include(i => i.Medico)
                           .Include(i => i.Medico.MedicoEspecialidades)
                           .AsNoTracking()
                           .FirstOrDefaultAsync(w => w.Id == this.AbpSession.UserId);

                if (usuario != null && usuario.MedicoId.HasValue)
                {
                    var medico = await medicoRepository.Object.GetAll().AsNoTracking().Include(x => x.SisPessoa).FirstOrDefaultAsync(x => x.Id == usuario.MedicoId.Value).ConfigureAwait(false);
                    viewModel.MedicoCorrente = MedicoDto.Mapear(medico);
                }
                return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/_PrescricaoCompleta.cshtml", viewModel);
            }
        }

        [HttpPost]
        public async Task<PartialViewResult> ImpressaoAcrescimosESuspensoes(long atendimentoId, long prescricaoMedicaId)
        {
            using (var divisaoAppService = IocManager.Instance.ResolveAsDisposable<IDivisaoAppService>())
            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<User, long>>())
            using (var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var prescricaoMedicaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoMedicaAppService>())
            {
                var viewModel = new PrescricaoCompletaViewModel
                {
                    AtendimentoId = atendimentoId,
                    PrescricaoItemRespostas = await prescricaoMedicaAppService.Object.ListarRespostasPorPrescricaoCompleta(prescricaoMedicaId).ConfigureAwait(false),
                    Divisoes = (await divisaoAppService.Object.ListarTodos().ConfigureAwait(false)).Items.OrderBy(m => m.Ordem).ToList(),
                    PrescricaoMedicaId = prescricaoMedicaId
                };
                var usuario = await usuarioRepository.Object.GetAll().Include(i => i.Medico)
                    .Include(i => i.Medico.MedicoEspecialidades)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(w => w.Id == this.AbpSession.UserId);

                if (usuario != null && usuario.MedicoId.HasValue)
                {
                    var medico = await medicoRepository.Object.GetAll().AsNoTracking().Include(x => x.SisPessoa).FirstOrDefaultAsync(x => x.Id == usuario.MedicoId.Value).ConfigureAwait(false);
                    viewModel.MedicoCorrente = MedicoDto.Mapear(medico);
                }
                return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/_impressaoAcrescimosESuspensoesModal.cshtml", viewModel);
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<PartialViewResult> _TevMovimento(long atendimentoId)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/_TevMovimento.cshtml",
                    new AssistencialAtendimentoViewModel { Atendimento = atendimento });
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<PartialViewResult> _ControlaTev(long atendimentoId)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Home/_ControlaTev.cshtml",
                    new AssistencialAtendimentoViewModel { Atendimento = atendimento });
            }
        }

        #endregion

        #region administrativo
        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoCAT()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/CATs/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoAlergia(long? id)
        {
            return await this.IndexProntuarioEletronico(id, "AdministrativoAlergia").ConfigureAwait(false);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoDocumentacaoPaciente()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/DocumentacaoPacientes/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoConfirmacaoAgendaConsulta()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/ConfirmacoesAgendaConsultas/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoConfirmacaoAgendaExame()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/ConfirmacoesAgendaExames/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoConfirmacaoAgendaCirurgia()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/ConfirmacoesAgendaCirurgias/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoTranferenciaLeito()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/TransferenciasLeitos/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoTransferenciaMedicoResponsavel()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/TransferenciasMedicosResponsaveis/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoTransferenciaSetor()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/TransferenciasSetores/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoAlta()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/Altas/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoAlteracaoAtendimento()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/AlteracoesAtendimentos/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoPassagemPlantaoEnfermagem()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/PassagensPlantoesEnfermagem/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoSolicitacaoProrrogacao()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/SolicitacoesProrrogacoes/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoSolicitacaoProdutoSetor()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/SolicitacoesProdutosSetores/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoSolicitacaoProdutoSOS()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/SolicitacoesProdutosSOS/Index.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> AdministrativoLiberacaoInterdicaoLeito()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Administrativos/LiberacoesInterdicoesLeitos/Index.cshtml");
        }

        #endregion

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> ListarRegistroArquivos(long id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimentoDto = await atendimentoAppService.Object.Obter(id).ConfigureAwait(false);

                var model = new AtendimentoDto
                {
                    Id = id,
                    Codigo = atendimentoDto.Codigo,
                    Paciente = atendimentoDto.Paciente
                };
                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/RegistrosArquivos/Index.cshtml",
                    model);
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        [System.Web.Mvc.HttpPost]
        public string VisualizarImagemRegistroArquivo(long id)
        {
            try
            {
                using (var registroArquivoAppService = IocManager.Instance.ResolveAsDisposable<IRegistroArquivoAppService>())
                {
                    var registro = registroArquivoAppService.Object.ObterPorId(id);
                    var arquivo = registro.Arquivo;
                    var base64 = Convert.ToBase64String(arquivo);
                    var imgSrc = string.Format("data:{0};base64,{1}", "image/png", base64);
                    return imgSrc;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> VisualizarPorId(long id)
        {
            using (var registroArquivoAppService = IocManager.Instance.ResolveAsDisposable<IRegistroArquivoAppService>())
            {
                var registroArquivo = registroArquivoAppService.Object.ObterPorId(id);

                try
                {
                    this.Response.Headers.Add("Content-Disposition", "inline; filename=desctino.pdf");
                    return this.File(registroArquivo.Arquivo, "application/pdf");
                }
                catch (Exception ex)
                {
                    throw new UserFriendlyException(L("ErroPesquisar"), ex);
                }
            }
        }

        /// <summary>
        /// The balanco hidrico.
        /// </summary>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> BalancoHidrico(long id, DateTime? date)
        {
            const int HoraIntervalo = 6;
            if (!date.HasValue)
            {
                date = DateTime.Today;
            }

            if (DateTime.Now.Hour < HoraIntervalo)
            {
                date = date.Value.AddDays(-1);
            }

            var viewModel = await BalancoHidricoAppService.BalancoHidricoGetData(id, date).ConfigureAwait(false);

            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/BalancoHidrico/index.cshtml", viewModel);
        }
        
        private string TransporteValor(IEnumerable<string> valoresTransp, string totalTransp)
        {
            var valorSoma = valoresTransp.Sum(x => {
                double val;
                if (string.IsNullOrEmpty(x) || !double.TryParse(x, out val))
                {
                    return 0;
                }

                return val;
            });
            
            double valorTransp;
            if (!double.TryParse(totalTransp, out valorTransp))
            {
                valorTransp = 0;
            }

            return (valorSoma - valorTransp).ToString();
        }
        
        

        /// <summary>
        /// The balanco hidrico partial.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> BalancoHidricoPartial(long id, DateTime? date)
        {
            var viewModel = await BalancoHidricoAppService.BalancoHidricoGetData(id, date).ConfigureAwait(false);

            return this.View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/BalancoHidrico/partialBalancoHidrico.cshtml", viewModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="date"></param>
        /// <param name="copiarSolucoes"></param>
        /// <returns></returns>
        public async Task<bool> BalancoHidricoCopiarSolucoes(long id, DateTime? date, bool copiarSolucoes = false)
        {
            var viewModel = await BalancoHidricoAppService.BalancoHidricoGetData(id, date).ConfigureAwait(false);

            using (var balancoHidricoAppService = IocManager.Instance.ResolveAsDisposable<IBalancoHidricoAppService>())
            {
                if (copiarSolucoes && viewModel.BalancoHidricoAnteriorId != 0)
                {
                    var modelAtual = viewModel.Model;

                    var modelAnterior = await balancoHidricoAppService.Object.ObterIdAsync(viewModel.BalancoHidricoAnteriorId).ConfigureAwait(false);

                    if (modelAtual.BalancoHidricoSolucoes.IsNullOrEmpty())
                    {
                        modelAtual.BalancoHidricoSolucoes = new List<BalancoHidricoSolucoesDto>();
                    }

                    foreach (var solucaoAnterior in modelAnterior.BalancoHidricoSolucoes)
                    {
                        var solucaoAtual = modelAtual.BalancoHidricoSolucoes.FirstOrDefault(x => x.IndiceSolucao == solucaoAnterior.IndiceSolucao);
                        if (solucaoAtual == null)
                        {
                            solucaoAtual = new BalancoHidricoSolucoesDto { IndiceSolucao = solucaoAnterior.IndiceSolucao };
                        }
                        solucaoAtual.Valor = solucaoAnterior.Valor;

                        if (modelAtual.BalancoHidricoSolucoes.All(x => x.IndiceSolucao != solucaoAnterior.IndiceSolucao))
                        {
                            modelAtual.BalancoHidricoSolucoes.Add(solucaoAtual);
                        }
                    }

                    await balancoHidricoAppService.Object.UpSertBalancoHidricoAsync(modelAtual).ConfigureAwait(false);
                    return true;
                }
            }

            return false;
        }



        /// <summary>
        /// The balanco hidrico relatorio.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="date">
        /// The date.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<FileResult> BalancoHidricoRelatorio(long id, DateTime? date)
        {
            var arquivo = await BalancoHidricoAppService.BalancoHidricoRelatorio(id, date).ConfigureAwait(false);
            return this.File(arquivo, "application/pdf");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> CadastroPacienteAlergias(long id, long? atendimentoId)
        {
            using (var pacienteAlergias = IocManager.Instance.ResolveAsDisposable<IPacienteAlergiasAppService>())
            using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var viewModel = new PacienteAlergiasViewModel();

                if (atendimentoId.HasValue)
                {
                    viewModel.Atendimento =
                        await atendimentoAppService.Object.Obter(atendimentoId.Value).ConfigureAwait(false);
                }


                viewModel.Paciente = await pacienteAppService.Object.Obter(id).ConfigureAwait(false);

                viewModel.Alergias =
                    await pacienteAlergias.Object.AlergiasPorPaciente(id, atendimentoId).ConfigureAwait(false);

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/PacienteAlergias/PacienteAlergiasModal.cshtml",
                    viewModel);
            }
        }

        public async Task<ActionResult> SelecionarModelo(long? id)
        {
            var viewModel = id;

            return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/ModelosPrescricoes/SelecionarModelePrescricaoModal.cshtml",
                    viewModel);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> CadastroPacienteDiagnosticos(long id, long? atendimentoId)
        {
            using (var pacienteDiagnosticos = IocManager.Instance.ResolveAsDisposable<IPacienteDiagnosticoAppService>())
            using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var viewModel = new PacienteDiagnosticosViewModel();

                if (atendimentoId.HasValue)
                {
                    viewModel.Atendimento =
                        await atendimentoAppService.Object.Obter(atendimentoId.Value).ConfigureAwait(false);
                }

                viewModel.Paciente = await pacienteAppService.Object.Obter(id).ConfigureAwait(false);

                viewModel.Diagnosticos = await pacienteDiagnosticos.Object.DiagnosticosPorPaciente(id, atendimentoId)
                                             .ConfigureAwait(false);

                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/PacienteDiagnosticos/PacienteDiagnosticosModal.cshtml",
                    viewModel);
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> CriarOuEditarPacienteDiagnosticosModal(long pacienteId, long? id)
        {
            CriarOuEditarPacienteDiagnosticosModalViewModel viewModel;
            if (id.HasValue)
            {
                using (var pacienteDiagnosticoAppService = IocManager.Instance.ResolveAsDisposable<IPacienteDiagnosticoAppService>())
                {
                    viewModel = new CriarOuEditarPacienteDiagnosticosModalViewModel(
                        await pacienteDiagnosticoAppService.Object.ObterAsync((long)id).ConfigureAwait(false));
                }
            }
            else
            {
                viewModel = new CriarOuEditarPacienteDiagnosticosModalViewModel(new PacienteDiagnosticosDto())
                {
                    PacienteId = pacienteId
                };
            }

            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/PacienteDiagnosticos/PacienteDiagnosticosCriarOuEditarModal.cshtml", viewModel);
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public async Task<ActionResult> CriarOuEditarPacienteAlergiasModal(long pacienteId, long? id)
        {
            CriarOuEditarPacienteAlergiasModalViewModel viewModel;
            if (id.HasValue)
            {
                using (var pacienteAlergiasAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAlergiasAppService>())
                {
                    viewModel = new CriarOuEditarPacienteAlergiasModalViewModel(
                        await pacienteAlergiasAppService.Object.ObterAsync((long)id).ConfigureAwait(false));
                }
            }
            else
            {
                viewModel = new CriarOuEditarPacienteAlergiasModalViewModel(new PacienteAlergiasDto())
                {
                    PacienteId = pacienteId
                };
            }

            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/PacienteAlergias/PacienteAlergiasCriarOuEditarModal.cshtml", viewModel);
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarPacienteAlergia(PacienteAlergiasDto pacienteAlergia)
        {
            using (var pacienteAlergiasAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAlergiasAppService>())
            {
                await pacienteAlergiasAppService.Object.UpsertPacienteAlergia(pacienteAlergia).ConfigureAwait(false);
                return this.Content(this.L("Sucesso"));
            }
        }

        public async Task<ActionResult> ExcluirPacienteAlergia(long id)
        {
            using (var pacienteAlergiasAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAlergiasAppService>())
            {
                var pacienteAlergia = await pacienteAlergiasAppService.Object.ObterAsync(id).ConfigureAwait(false);
                await pacienteAlergiasAppService.Object.Excluir(pacienteAlergia).ConfigureAwait(false);
                return this.Content(this.L("Sucesso"));
            }
        }

        public async Task<ActionResult> ExcluirPacienteDiagnostico(long id)
        {
            using (var pacienteDiagnosticoAppService = IocManager.Instance.ResolveAsDisposable<IPacienteDiagnosticoAppService>())
            {
                var pacienteDiagnostico = await pacienteDiagnosticoAppService.Object.ObterAsync(id).ConfigureAwait(false);
                await pacienteDiagnosticoAppService.Object.Excluir(pacienteDiagnostico).ConfigureAwait(false);
                return this.Content(this.L("Sucesso"));
            }
        }

        [System.Web.Mvc.HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarPacienteDiagnostico(PacienteDiagnosticosDto pacienteDiagnostico)
        {
            using (var pacienteDiagnosticoAppService = IocManager.Instance.ResolveAsDisposable<IPacienteDiagnosticoAppService>())
            {
                await pacienteDiagnosticoAppService.Object.UpsertPacienteDiagnostico(pacienteDiagnostico)
                    .ConfigureAwait(false);
                return this.Content(this.L("Sucesso"));
            }
        }

        public async Task<ActionResult> SolicitacoesModal(long atendimentoId, long? prescricaoId)
        {

            using (var solicitacaoAutorizacaoAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoAutorizacaoAppService>())
            using (var solicitacaoAntimicrobianosAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoAntimicrobianoAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);
                var viewModel = new SolicitacoesViewModel
                {
                    HeaderPaciente = new AssistenciaisViewModel(atendimento),
                    SolicitacaoAntimicrobianos = await solicitacaoAntimicrobianosAppService.Object
                        .SolicitacaoAntimicrobianoModal(atendimentoId, prescricaoId)
                        .ConfigureAwait(false),
                    SolicitacaoAutorizacoes = await solicitacaoAutorizacaoAppService.Object
                        .SolicitacaoAutorizacaoModal(atendimentoId, prescricaoId)
                        .ConfigureAwait(false)
                };
                viewModel.HeaderPaciente.HabilitarAcoes = false;



                return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/Solicitacoes/_CriarOuEditar.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> SelecionarSubItemPrescricaoModal()
        {
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Medicos/Prescricoes/_SelecionarSubItemPrescricaoModal.cshtml", new CriarOuEditarSubItensPrescricaoViewModel());
        }
    }
}