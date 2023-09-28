using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposTelefone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Configuracoes
{
    public class EmpresasController : SWMANAGERControllerBase
    {
        private readonly IEmpresaAppService _empresaAppService;
        private readonly IEstadoAppService _estadoAppService;
        private readonly ICidadeAppService _cidadeAppService;
        private readonly IPaisAppService _paisAppService;
        private readonly ITipoTelefoneAppService _tipoTelefoneAppService;
        private readonly ITipoLogradouroAppService _tipoLogradouroAppService;
        private readonly IConvenioAppService _convenioAppService;
        private readonly IPlanoAppService _planoAppService;
        private readonly IEstoqueAppService _estoqueAppService;

        public EmpresasController(
            IEmpresaAppService empresaAppService,
            IEstadoAppService estadoAppService,
            ICidadeAppService cidadeAppService,
            IPaisAppService paisAppService,
            ITipoTelefoneAppService tipoTelefoneAppService,
            ITipoLogradouroAppService tipoLogradouroAppService,
            IConvenioAppService convenioAppService,
            IPlanoAppService planoAppService,
            IEstoqueAppService estoqueAppService
            )
        {
            _empresaAppService = empresaAppService;
            _estadoAppService = estadoAppService;
            _cidadeAppService = cidadeAppService;
            _paisAppService = paisAppService;
            _tipoTelefoneAppService = tipoTelefoneAppService;
            _tipoLogradouroAppService = tipoLogradouroAppService;
            _convenioAppService = convenioAppService;
            _planoAppService = planoAppService;
            _estoqueAppService = estoqueAppService;
        }

        public ActionResult Index()
        {
            var model = new EmpresasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/Empresas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Configuracoes_Empresa_Create, AppPermissions.Pages_Tenant_Configuracoes_Empresa_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var tiposTelefone = await _tipoTelefoneAppService.ListarTodos();
            CriarOuEditarEmpresaModalViewModel viewModel;

            var convenios = await _convenioAppService.ListarTodos();
            var planos = await _planoAppService.ListarTodos();
            var estoques = await _estoqueAppService.ListarTodos();

            if (id.HasValue)
            {
                var output = await _empresaAppService.Obter((long)id);
                viewModel = new CriarOuEditarEmpresaModalViewModel(output);


                var estados = await _estadoAppService.ListarPorPais(output.PaisId);
                var cidades = await _cidadeAppService.ListarPorEstado(output.EstadoId);
                var paises = await _paisAppService.Listar(new ListarPaisesInput { MaxResultCount = 1000 });
                var tiposLogradouro = await _tipoLogradouroAppService.ListarTodos();
                if (output.TipoLogradouroId.HasValue)
                {
                    var tipoLogradouro = await _tipoLogradouroAppService.Obter(output.TipoLogradouroId.Value);
                    viewModel.TiposLogradouro = new SelectList(tiposLogradouro.Items.Select(m => new { Id = m.Id, Descricao = m.Descricao }), "Id", "Descricao", tipoLogradouro.Id);
                }
                else
                {
                    viewModel.TiposLogradouro = new SelectList(tiposLogradouro.Items.Select(m => new { Id = m.Id, Descricao = m.Descricao }), "Id", "Descricao");
                }

                viewModel.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1}) - {2} ({3})", m.Nome, m.Uf, m.Pais.Nome, m.Pais.Sigla) }), "Id", "Nome", output.EstadoId);
                viewModel.Cidades = new SelectList(cidades.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}/{1} ({2})", m.Nome, m.Estado.Nome, m.Estado.Uf) }), "Id", "Nome", output.CidadeId);
                viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome", output.PaisId);
                viewModel.Convenios = new SelectList(convenios.Items.Select(m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }), "Id", "NomeFantasia", output.ConvenioId);
                viewModel.Planos = new SelectList(planos.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao", output.PlanoId);

                if (output.EstadoId.HasValue)
                {
                    viewModel.EstoquesMaster = new SelectList(estoques.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao", output.EstoqueId);
                }
                else
                {
                    viewModel.EstoquesMaster = new SelectList(estoques.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                }
            }
            else
            {
                var estados = await _estadoAppService.ListarPorPais(null);
                var cidades = await _cidadeAppService.ListarPorEstado(null);
                var paises = await _paisAppService.Listar(new ListarPaisesInput());
                var tiposLogradouro = await _tipoLogradouroAppService.ListarTodos();
                viewModel = new CriarOuEditarEmpresaModalViewModel(new EmpresaDto());
                viewModel.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1}) - {2} ({3})", m.Nome, m.Uf, m.Pais?.Nome, m.Pais?.Sigla) }), "Id", "Nome");
                viewModel.Cidades = new SelectList(cidades.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}/{1} ({2})", m.Nome, m.Estado.Nome, m.Estado.Uf) }), "Id", "Nome");
                viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome");
                viewModel.TiposLogradouro = new SelectList(tiposLogradouro.Items.Select(m => new { Id = m.Id, Nome = m.Descricao }), "Id", "Nome");
                viewModel.Convenios = new SelectList(convenios.Items.Select(m => new { Id = m.Id, NomeFantasia = string.Format("{0}", m.NomeFantasia) }), "Id", "NomeFantasia");
                viewModel.Planos = new SelectList(planos.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                viewModel.EstoquesMaster = new SelectList(estoques.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
            }
            viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Configuracoes/Empresas/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var input = new ListarEmpresasInput();
            input.Filtro = term;
            input.MaxResultCount = 50;
            input.Sorting = "NomeFantasia";
            var query = await _empresaAppService.Listar(input);
            var result = query.Items.Select(m => new { m.Id, m.NomeFantasia }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AutoCompleteDescricao(string term)
        {
            var input = new ListarEmpresasInput();
            input.Filtro = term;
            input.MaxResultCount = 50;
            input.Sorting = "NomeFantasia";
            var query = await _empresaAppService.Listar(input);
            var result = query.Items.Select(m => new { Id = m.Id, Nome = m.NomeFantasia }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public async Task SetEmpresaTempData(long? empresaId)
        {
            var empresa = await _empresaAppService.Obter(empresaId.Value);
            if (empresa == null || (empresa != null && empresa.Id == 0))
            {
                TempData.Remove("Empresa");
            }
            else
            {
                TempData["Empresa"] = empresa;
            }
        }

    }
}