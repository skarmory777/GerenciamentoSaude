using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Sexos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class LeitosController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly ILeitoAppService _leitoAppService;
        private readonly ISexoAppService _sexoAppService;
        private readonly IUnidadeInternacaoAppService _unidadeInternacaoAppService;
        private readonly ITipoAcomodacaoAppService _tipoAcomodacaoAppService;
        private readonly ITabelaDominioAppService _tabelaDominioAppService;
        private readonly ILeitoStatusAppService _leitoStatusAppService;
        private readonly IUnidadeOrganizacionalAppService _unidadeOrganizacionalAppService;

        public LeitosController(
            ILeitoAppService leitoAppService,
            ISexoAppService sexoAppService,
            IUnidadeInternacaoAppService unidadeInternacaoAppService,
            ITipoAcomodacaoAppService tipoAcomodacaoAppService,
            ITabelaDominioAppService tabelaDominioAppService,
            ILeitoStatusAppService leitoStatusAppService,
            IUnidadeOrganizacionalAppService unidadeOrganizacionalAppService
            )
        {
            _leitoAppService = leitoAppService;
            _sexoAppService = sexoAppService;
            _unidadeInternacaoAppService = unidadeInternacaoAppService;
            _tipoAcomodacaoAppService = tipoAcomodacaoAppService;
            _tabelaDominioAppService = tabelaDominioAppService;
            _leitoStatusAppService = leitoStatusAppService;
            _unidadeOrganizacionalAppService = unidadeOrganizacionalAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new LeitosViewModel();
            var leitoStatus = await _leitoStatusAppService.Listar(new ListarLeitosStatusInput());
            List<LeitoStatusDto> leitoStatusList = leitoStatus.Items.ToList();
            model.LeitoStatus = leitoStatusList;
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Leitos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_Leitos_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_Leitos_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var sexos = await _sexoAppService.ListarTodos();
            var unidadesInternacao = await _unidadeOrganizacionalAppService.ListarTodos();
            var tiposAcomodacao = await _tipoAcomodacaoAppService.Listar(new ListarTiposAcomodacaoInput());
            var itensTabelaTiss = await _tabelaDominioAppService.Listar(new ListarTabelasDominioInput());
            var leitosStatus = await _leitoStatusAppService.Listar(new ListarLeitosStatusInput());

            CriarOuEditarLeitoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _leitoAppService.Obter((long)id);
                viewModel = new CriarOuEditarLeitoModalViewModel(output);
                viewModel.Sexos = new SelectList(sexos.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                viewModel.UnidadesInternacao = new SelectList(unidadesInternacao.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                viewModel.TiposAcomodacao = new SelectList(tiposAcomodacao.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                viewModel.ItensTabelaDominio = new SelectList(itensTabelaTiss.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                viewModel.LeitosStatus = ControllerHelper.ComboSelecionado(leitosStatus.Items, "Id", "Descricao", output.LeitoStatus != null ? output.LeitoStatus.Descricao : "Vago");
            }
            else
            {
                viewModel = new CriarOuEditarLeitoModalViewModel(new LeitoDto());
                viewModel.Sexos = new SelectList(sexos.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                viewModel.UnidadesInternacao = new SelectList(unidadesInternacao.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                viewModel.TiposAcomodacao = new SelectList(tiposAcomodacao.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                viewModel.ItensTabelaDominio = new SelectList(itensTabelaTiss.Items.Select(m => new { Id = m.Id, Descricao = string.Format("{0}", m.Descricao) }), "Id", "Descricao");
                viewModel.LeitosStatus = ControllerHelper.ComboSelecionado(leitosStatus.Items, "Id", "Descricao", "Vago");
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Leitos/_CriarOuEditarModal.cshtml", viewModel);
        }

        //[AcceptVerbs("GET", "POST", "PUT")]
        //public JsonResult ListarPorUnidade(long? id)
        //{
        //    try
        //    {
        //        var unidades = AsyncHelper.RunSync(() => _leitoAppService.ListarPorUnidade(id));

        //        var lista = unidades.Items.ToList();//.Select(c => new { DisplayText = c.Descricao, Value = c.Id });

        //        return Json(new { Result = "OK", Options = lista }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult OcuparLeito(long leitoId)
        {
            try
            {
                _leitoAppService.OcuparLeito(leitoId);
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult DesocuparLeito(long leitoId)
        {
            try
            {
                _leitoAppService.DesocuparLeito(leitoId);
                return Json(new { Result = "OK" }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}