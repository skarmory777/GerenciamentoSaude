#region Usings
using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.Itens;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoItensController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly IFaturamentoItemAppService _faturamentoItemAppService;
        private readonly IFaturamentoTipoGrupoAppService _faturamentoTipoGrupoAppService;
        private readonly IFaturamentoGrupoAppService _faturamentoGrupoAppService;
        private readonly IFaturamentoSubGrupoAppService _faturamentoSubGrupoAppService;
        private readonly IFaturamentoBrasItemAppService _faturamentoBrasItemAppService;
        private readonly IFaturamentoBrasApresentacaoAppService _faturamentoBrasApresentacaoAppService;
        private readonly IFaturamentoBrasLaboratorioAppService _faturamentoBrasLaboratorioAppService;
        private readonly ILaudoGrupoAppService _lauGrupoAppService;


        public FaturamentoItensController(
            IFaturamentoItemAppService faturamentoItemAppService,
            IFaturamentoGrupoAppService faturamentoGrupoAppService,
            IFaturamentoBrasItemAppService faturamentoBrasItemAppService,
            IFaturamentoBrasApresentacaoAppService faturamentoBrasApresentacaoAppService,
            IFaturamentoTipoGrupoAppService faturamentoTipoGrupoAppService,
            IFaturamentoBrasLaboratorioAppService faturamentoBrasLaboratorioAppService,
            IFaturamentoSubGrupoAppService faturamentoSubGrupoAppService,
            ILaudoGrupoAppService lauGrupoAppService
            )
        {
            _faturamentoItemAppService = faturamentoItemAppService;
            _faturamentoGrupoAppService = faturamentoGrupoAppService;
            _faturamentoBrasItemAppService = faturamentoBrasItemAppService;
            _faturamentoBrasLaboratorioAppService = faturamentoBrasLaboratorioAppService;
            _faturamentoBrasApresentacaoAppService = faturamentoBrasApresentacaoAppService;
            _faturamentoTipoGrupoAppService = faturamentoTipoGrupoAppService;
            _faturamentoSubGrupoAppService = faturamentoSubGrupoAppService;
            _lauGrupoAppService = lauGrupoAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new ItensViewModel();

            //var grupos = await _faturamentoGrupoAppService.Listar(new ListarFaturamentoGruposInput());
            //model.Grupos = new SelectList(grupos.Items, "Id", "Descricao");
            //var subGrupos = await _faturamentoSubGrupoAppService.Listar(new ListarFaturamentoSubGruposInput());
            //model.SubGrupos = new SelectList(subGrupos.Items, "Id", "Descricao");
            //var tipos = await _faturamentoTipoGrupoAppService.Listar(new ListarFaturamentoTiposGrupoInput());
            //model.Tipos = new SelectList(tipos.Items, "Id", "Descricao");

            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Itens/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_Item_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_Item_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarItemModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _faturamentoItemAppService.Obter((long)id);
                viewModel = new CriarOuEditarItemModalViewModel(output);

                if (viewModel.LaudoGrupo == null)
                {
                    viewModel.LaudoGrupo = new ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto.LaudoGrupoDto();
                    viewModel.LaudoGrupo.Descricao = "";
                    viewModel.LaudoGrupoId = 0;
                }
            }
            else
            {
                viewModel = new CriarOuEditarItemModalViewModel(new FaturamentoItemDto());
                viewModel.LaudoGrupo = new ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto.LaudoGrupoDto();
                viewModel.LaudoGrupo.Descricao = "";
                viewModel.LaudoGrupoId = 0;
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Itens/_CriarOuEditarModal.cshtml", viewModel);
        }


        // PARCIAIS
        // Honorarios
        public PartialViewResult ConfigHonorarios(long? itemId)
        {
            CriarOuEditarItemModalViewModel viewModel;
            if (itemId.HasValue)
            {
                var output = AsyncHelper.RunSync(() => _faturamentoItemAppService.Obter((long)itemId));
                viewModel = new CriarOuEditarItemModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarItemModalViewModel(new FaturamentoItemDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Itens/Configuracoes/_Honorarios.cshtml", viewModel);
        }

        // Servicos
        public PartialViewResult ConfigServicos(long? itemId)
        {
            CriarOuEditarItemModalViewModel viewModel;
            if (itemId.HasValue)
            {
                var output = _faturamentoItemAppService.Obter((long)itemId);
                viewModel = new CriarOuEditarItemModalViewModel(output.Result);
            }
            else
            {
                viewModel = new CriarOuEditarItemModalViewModel(new FaturamentoItemDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Itens/Configuracoes/_Servicos.cshtml", viewModel);
        }

        // Servicos
        public PartialViewResult ConfigProdutos(long? itemId)
        {
            CriarOuEditarItemModalViewModel viewModel;
            if (itemId.HasValue)
            {
                var output = _faturamentoItemAppService.Obter((long)itemId);
                viewModel = new CriarOuEditarItemModalViewModel(output.Result);
            }
            else
            {
                viewModel = new CriarOuEditarItemModalViewModel(new FaturamentoItemDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Itens/Configuracoes/_Produtos.cshtml", viewModel);
        }

        // Pacotes
        public PartialViewResult ConfigPacotes(long? itemId)
        {
            CriarOuEditarItemModalViewModel viewModel;
            if (itemId.HasValue)
            {
                var output = _faturamentoItemAppService.Obter((long)itemId);
                viewModel = new CriarOuEditarItemModalViewModel(output.Result);
            }
            else
            {
                viewModel = new CriarOuEditarItemModalViewModel(new FaturamentoItemDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Itens/Configuracoes/_Pacotes.cshtml", viewModel);
        }

    }
}