using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.CoresPele;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Escolaridades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.EstadosCivis;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Religioes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Sexos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposTelefone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Orcamentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Orcamentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Orcamentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.PreAtendimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Orcamentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.PreAtendimentos;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Orcamentos
{
    public class OrcamentosController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            //TempData["Orcamento"] = new OrcamentoDto();
            //TempData["OrcamentoId"] = 0;
            var model = new OrcamentosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Orcamentos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Orcamentos, AppPermissions.Pages_Tenant_Atendimento_Orcamentos_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            using (var planoAppService = IocManager.Instance.ResolveAsDisposable<IPlanoAppService>())
            using (var convenioAppService = IocManager.Instance.ResolveAsDisposable<IConvenioAppService>())
            using (var orcamentoAppService = IocManager.Instance.ResolveAsDisposable<IOrcamentoAppService>())
            {
                var planos = await planoAppService.Object.ListarTodos();
                var convenios = await convenioAppService.Object.Listar(new ListarConveniosInput());

                CriarOuEditarOrcamentoModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await orcamentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarOrcamentoModalViewModel(output);
                    viewModel.Planos = new SelectList(planos.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} - {1}", m.Descricao, m.Convenio.NomeFantasia) }), "Id", "Nome", output.PlanoId);
                    viewModel.Convenios = new SelectList(convenios.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeFantasia) }), "Id", "Nome", output.ConvenioId);
                }
                else
                {
                    viewModel = new CriarOuEditarOrcamentoModalViewModel(new CriarOuEditarOrcamento());
                    viewModel.Planos = new SelectList(planos.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} - {1}", m.Descricao, m.Convenio.NomeFantasia) }), "Id", "Nome");
                    viewModel.Convenios = new SelectList(convenios.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeFantasia) }), "Id", "Nome");
                }
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Orcamentos/_CriarOuEditarModal.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> _CriarOuEditarOrcamento()
        {
            using (var planoAppService = IocManager.Instance.ResolveAsDisposable<IPlanoAppService>())
            using (var convenioAppService = IocManager.Instance.ResolveAsDisposable<IConvenioAppService>())
            using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
            {
                var planos = await planoAppService.Object.Listar(new ListarPlanosInput()).ConfigureAwait(false);
                var convenios = await convenioAppService.Object.Listar(new ListarConveniosInput()).ConfigureAwait(false);
                var empresas = await empresaAppService.Object.Listar(new ListarEmpresasInput()).ConfigureAwait(false);
                CriarOuEditarOrcamentoModalViewModel viewModel;
                viewModel = new CriarOuEditarOrcamentoModalViewModel(new CriarOuEditarOrcamento());
                viewModel.Planos = new SelectList(planos.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} - {1}", m.Descricao, m.Convenio.NomeFantasia) }), "Id", "Nome");
                viewModel.Convenios = new SelectList(convenios.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeFantasia) }), "Id", "Nome");
                viewModel.Empresas = new SelectList(empresas.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}", m.NomeFantasia) }), "Id", "Nome");
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Orcamentos/_CriarOuEditarModal.cshtml", viewModel);
            }
        }


        public PartialViewResult _PesquisarPreAtendimento()
        {
            var model = new PreAtendimentosViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Orcamentos/_PesquisarPreAtendimentos.cshtml", model);
        }

        public async Task<PartialViewResult> _PreAtendimento(long? id)
        {
            using (var sexoAppService = IocManager.Instance.ResolveAsDisposable<ISexoAppService>())
            using (var preAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IPreAtendimentoAppService>())
            {
                var sexos = await sexoAppService.Object.ListarTodos().ConfigureAwait(false);

                CriarOuEditarPreAtendimentoModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await preAtendimentoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPreAtendimentoModalViewModel(output);
                    viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao", output.Sexo);
                }
                else
                {
                    viewModel = new CriarOuEditarPreAtendimentoModalViewModel(new CriarOuEditarPreAtendimento());
                    viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
                }
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/Orcamentos/_CriarOuEditarPreAtendimento.cshtml", viewModel);
            }
        }

        public PartialViewResult _PesquisarPaciente()
        {
            var model = new PacientesViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Home/Pacientes/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _IdentificacaoPaciente(long? id)
        {
            using (var origemAppService = IocManager.Instance.ResolveAsDisposable<IOrigemAppService>())
            using (var sexoAppService = IocManager.Instance.ResolveAsDisposable<ISexoAppService>())
            using (var corPeleAppService = IocManager.Instance.ResolveAsDisposable<ICorPeleAppService>())
            using (var escolaridadeAppService = IocManager.Instance.ResolveAsDisposable<IEscolaridadeAppService>())
            using (var religiaoAppService = IocManager.Instance.ResolveAsDisposable<IReligiaoAppService>())
            using (var estadoCivilAppService = IocManager.Instance.ResolveAsDisposable<IEstadoCivilAppService>())
            using (var tipoTelefoneAppService = IocManager.Instance.ResolveAsDisposable<ITipoTelefoneAppService>())
            using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
            {
                var origens = await origemAppService.Object.Listar(new ListarOrigensInput()).ConfigureAwait(false);
                var sexos = await sexoAppService.Object.ListarTodos().ConfigureAwait(false);
                var coresPele = await corPeleAppService.Object.ListarTodos().ConfigureAwait(false);
                var escolaridades = await escolaridadeAppService.Object.ListarTodos().ConfigureAwait(false);
                var religioes = await religiaoAppService.Object.ListarTodos().ConfigureAwait(false);
                var estadosCivis = await estadoCivilAppService.Object.ListarTodos().ConfigureAwait(false);
                var tiposTelefone = await tipoTelefoneAppService.Object.ListarTodos().ConfigureAwait(false);

                CriarOuEditarPacienteModalViewModel viewModel;

                if (id.HasValue)
                {
                    var output = await pacienteAppService.Object.Obter2((long)id).ConfigureAwait(false);

                    viewModel = new CriarOuEditarPacienteModalViewModel(output);
                    //viewModel.Origens = new SelectList(origens.Items, "Id", "Descricao", output.OrigemId);
                    //viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao", output.Sexo);
                    //viewModel.Escolaridades = new SelectList(escolaridades.Items, "Id", "Descricao", output.Escolaridade);
                    //viewModel.CoresPele = new SelectList(coresPele.Items, "Id", "Descricao", output.CorPele);
                    //viewModel.Religioes = new SelectList(religioes.Items, "Id", "Descricao", output.Religiao);
                    //viewModel.EstadosCivis = new SelectList(estadosCivis.Items, "Id", "Descricao", output.EstadoCivil);
                    //viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");
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
            }
        }

        [HttpPost]
        public async Task<long> SalvarPreAtendimento(CriarOuEditarPreAtendimento preAtendimento)
        {
            using (var preAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IPreAtendimentoAppService>())
            {
                var preAtendimentoInserido = await preAtendimentoAppService.Object.CriarGetId(preAtendimento).ConfigureAwait(false);
                return preAtendimentoInserido;
            }
        }

        [HttpPost]
        public async Task<ActionResult> SalvarOrcamento(CriarOuEditarOrcamento orcamento)
        {
            using (var orcamentoAppService = IocManager.Instance.ResolveAsDisposable<IOrcamentoAppService>())
            {
                OrcamentoDto relacao = new OrcamentoDto();
                await orcamentoAppService.Object.CriarOuEditar(orcamento).ConfigureAwait(false);
                return Content(L("Sucesso"));
            }
        }
    }
}