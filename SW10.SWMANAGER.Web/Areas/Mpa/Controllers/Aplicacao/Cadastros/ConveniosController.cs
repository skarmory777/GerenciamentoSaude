#region Usings
using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposTelefone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.CodigosCredenciados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ConveniosController : SWMANAGERControllerBase
    {
        #region Cabecalho
        private readonly IConvenioAppService _convenioAppService;
        private readonly IPaisAppService _paisAppService;
        private readonly IEstadoAppService _estadoAppService;
        private readonly ICidadeAppService _cidadeAppService;
        private readonly ITipoTelefoneAppService _tipoTelefoneAppService;

        public ConveniosController(
            IConvenioAppService convenioAppService,
            IPaisAppService paisAppService,
            IEstadoAppService estadoAppService,
            ICidadeAppService cidadeAppService,
            ITipoTelefoneAppService tipoTelefoneAppService
            )
        {
            _convenioAppService = convenioAppService;
            _paisAppService = paisAppService;
            _estadoAppService = estadoAppService;
            _cidadeAppService = cidadeAppService;
            _tipoTelefoneAppService = tipoTelefoneAppService;
        }
        #endregion cabecalho.

        public ActionResult Index()
        {
            var model = new ConveniosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Convenios/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Convenio_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Convenio_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarConvenioModalViewModel viewModel;
            viewModel = new CriarOuEditarConvenioModalViewModel(new ConvenioDto());
            try
            {
                if (id.HasValue)
                {
                    var output = await _convenioAppService.Obter((long)id);
                    var estados = await _estadoAppService.ListarPorPais(output.PaisId);
                    var cidades = await _cidadeAppService.ListarPorEstado(output.EstadoId);
                    var paises = await _paisAppService.Listar(new ListarPaisesInput { MaxResultCount = 1000 });
                    var tiposTelefone = await _tipoTelefoneAppService.ListarTodos();

                    viewModel = new CriarOuEditarConvenioModalViewModel(output);

                    viewModel.IntervaloGuiasConveniosIndexJson = JsonConvert.SerializeObject(output.IntervaloGuiasConveniosIndex);
                    viewModel.CodigoCredenciadoConveniosIndexJson = JsonConvert.SerializeObject(output.CodigosCredenciadoIndex);
                    viewModel.FatGrupoConvenioIndexJson = JsonConvert.SerializeObject(output.FatGrupoConvenioIndex);
                    //viewModel.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1}) - {2} ({3})", m.Nome, m.Uf, m.Pais.Nome, m.Pais.Sigla) }), "Id", "Nome", output.EstadoId);
                    //viewModel.Cidades = new SelectList(cidades.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}/{1} ({2})", m.Nome, m.Estado.Nome, m.Estado.Uf) }), "Id", "Nome", output.CidadeId);
                    //viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome", output.PaisId);
                    //viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");

                }
                else
                {
                    var estados = await _estadoAppService.ListarPorPais(null);
                    var cidades = await _cidadeAppService.ListarPorEstado(null);
                    var paises = await _paisAppService.Listar(new ListarPaisesInput());
                    var tiposTelefone = await _tipoTelefoneAppService.ListarTodos();

                    viewModel = new CriarOuEditarConvenioModalViewModel(new ConvenioDto());
                    viewModel.IntervaloGuiasConveniosIndexJson = JsonConvert.SerializeObject(new List<IntervaloGuiasConvenioIndex>());
                    viewModel.CodigoCredenciadoConveniosIndexJson = JsonConvert.SerializeObject(new List<CodigoCredenciadoIndex>());
                    viewModel.FatGrupoConvenioIndexJson = JsonConvert.SerializeObject(new List<FaturamentoGrupoConvenioIndex>());
                    //viewModel.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1}) - {2} ({3})", m.Nome, m.Uf, m.Pais.Nome, m.Pais.Sigla) }), "Id", "Nome");
                    //viewModel.Cidades = new SelectList(cidades.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0}/{1} ({2})", m.Nome, m.Estado.Nome, m.Estado.Uf) }), "Id", "Nome");
                    //viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Sigla) }), "Id", "Nome");
                    //viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");

                }
            }
            catch (Exception ex)
            {

            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Convenios/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _convenioAppService.ListarAutoComplete(term);
            //var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            //return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}