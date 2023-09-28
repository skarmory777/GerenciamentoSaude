using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormulasEstoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormulasExamesImagens;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormulasExamesLaboratoriais;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.FormulasFaturamentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ADORepositorio.Base;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class PrescricoesItensController : SWMANAGERControllerBase
    {
        //private readonly IPrescricaoItemAppService _prescricaoItemAppService;
        //private readonly IFormulaEstoqueAppService _formulaEstoqueAppService;
        ////private readonly IFormulaEstoqueItemAppService _formulaEstoqueItemAppService;
        //private readonly IFormulaFaturamentoAppService _formulaFaturamentoAppService;
        //private readonly IProdutoAppService _produtoAppService;
        //private readonly IFormulaEstoqueKitAppService _formulaEstoqueKitAppService;

        //public PrescricoesItensController(
        //    IPrescricaoItemAppService tipoControleAppService,
        //    IFormulaEstoqueAppService formulaEstoqueAppService,
        //    //IFormulaEstoqueItemAppService formulaEstoqueItemAppService,
        //    IFormulaFaturamentoAppService formulaFaturamentoAppService,
        //    IProdutoAppService produtoAppService,
        //    IFormulaEstoqueKitAppService formulaEstoqueKitAppService
        //    )
        //{
        //    _prescricaoItemAppService = tipoControleAppService;
        //    _formulaEstoqueAppService = formulaEstoqueAppService;
        //    //_formulaEstoqueItemAppService = formulaEstoqueItemAppService;
        //    _formulaFaturamentoAppService = formulaFaturamentoAppService;
        //    _produtoAppService = produtoAppService;
        //    _formulaEstoqueKitAppService = formulaEstoqueKitAppService;
        //}



        public ActionResult Index()
        {
            var model = new PrescricaoItemViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/Index.cshtml", model);
        }

        [OutputCache(Duration = 1, VaryByParam = "*")]
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Edit)]
        public ViewResult CriarOuEditar(long? id)
        {
            ViewBag.Id = id;
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/CriarOuEditar.cshtml");
        }

        //[OutputCache(Duration = 1, VaryByParam = "*")]
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarPartial(long? id)
        {
            CriarOuEditarPrescricaoItemViewModel viewModel;
            if (id.HasValue)
            {
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var formulaEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IFormulaEstoqueAppService>())
                using (var formulaFaturamentoAppService = IocManager.Instance.ResolveAsDisposable<IFormulaFaturamentoAppService>())
                using (var formulaEstoqueKitAppService = IocManager.Instance.ResolveAsDisposable<IFormulaEstoqueKitAppService>())
                {
                    var output = await prescricaoItemAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPrescricaoItemViewModel(output);
                    var formulaEstoqueList = await formulaEstoqueAppService.Object.ListarPorPrescricaoItem(id.Value).ConfigureAwait(false);
                    var formulaFaturamentoList =
                        await formulaFaturamentoAppService.Object.ListarFaturamentoPorPrescricaoItem(id.Value).ConfigureAwait(false);
                    var formulaExameLaboratorialList =
                        await formulaFaturamentoAppService.Object.ListarExameLaboratorialPorPrescricaoItem(id.Value).ConfigureAwait(false);
                    var formulaExameImagemList =
                        await formulaFaturamentoAppService.Object.ListarExameImagemPorPrescricaoItem(id.Value).ConfigureAwait(false);
                    viewModel.FormulaEstoqueList = JsonConvert.SerializeObject(formulaEstoqueList.Items.ToList());

                    viewModel.FormulaEstoqueKitJson = JsonConvert.SerializeObject(formulaEstoqueKitAppService.Object.ListarPorPrescricaoItem(id.Value));


                    viewModel.FormulaFaturamentoList =
                        JsonConvert.SerializeObject(formulaFaturamentoList.Items.ToList());
                    viewModel.FormulaExameLaboratorialList =
                        JsonConvert.SerializeObject(formulaExameLaboratorialList.Items.ToList());
                    viewModel.FormulaExameImagemList =
                        JsonConvert.SerializeObject(formulaExameImagemList.Items.ToList());
                }
            }
            else
            {
                viewModel = new CriarOuEditarPrescricaoItemViewModel(new PrescricaoItemDto())
                {
                    FormulaEstoqueList = JsonConvert.SerializeObject(new List<FormulaEstoqueDto>()),
                    FormulaFaturamentoList =
                                        JsonConvert.SerializeObject(new List<FormulaFaturamentoDto>()),
                    FormulaExameLaboratorialList =
                                        JsonConvert.SerializeObject(new List<FormulaFaturamentoDto>()),
                    FormulaExameImagemList =
                                        JsonConvert.SerializeObject(new List<FormulaFaturamentoDto>()),
                    FormulaEstoqueKitJson =
                                        JsonConvert.SerializeObject(new List<FormulaEstoqueKitDto>())
                };
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarModal.cshtml", viewModel);
        }

        //[OutputCache(Duration = 1, VaryByParam = "*")]
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Edit)]
        public PartialViewResult _CriarOuEditar(long? produtoId)
        {
            var model = new PrescricaoItemDto();

            if (produtoId.HasValue && produtoId.Value > 0)
            {
                using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                {
                    model = Task.Run(() => prescricaoItemAppService.Object.ObterPorProduto(produtoId.Value)).Result;
                    if (model == null)
                    {
                        model = new PrescricaoItemDto
                        {
                            ProdutoId = produtoId.Value,
                            Produto = Task.Run(() => produtoAppService.Object.Obter(produtoId.Value)).Result
                        };
                    }
                }
            }

            var viewModel = new CriarOuEditarPrescricaoItemViewModel(model);

            if (model.Id > 0)
            {
                using (var formulaEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IFormulaEstoqueAppService>())
                using (var formulaFaturamentoAppService = IocManager.Instance.ResolveAsDisposable<IFormulaFaturamentoAppService>())
                {
                    var formulaEstoqueList = Task.Run(() => formulaEstoqueAppService.Object.ListarPorPrescricaoItem(model.Id))
                        .Result;
                    var formulaFaturamentoList = Task.Run(
                        () => formulaFaturamentoAppService.Object.ListarFaturamentoPorPrescricaoItem(model.Id)).Result;
                    var formulaExameLaboratorialList = Task.Run(
                        () => formulaFaturamentoAppService.Object.ListarExameLaboratorialPorPrescricaoItem(model.Id)).Result;
                    var formulaExameImagemList = Task.Run(
                        () => formulaFaturamentoAppService.Object.ListarExameImagemPorPrescricaoItem(model.Id)).Result;
                    viewModel.FormulaEstoqueList = JsonConvert.SerializeObject(formulaEstoqueList.Items.ToList());
                    viewModel.FormulaFaturamentoList =
                        JsonConvert.SerializeObject(formulaFaturamentoList.Items.ToList());
                    viewModel.FormulaExameLaboratorialList =
                        JsonConvert.SerializeObject(formulaExameLaboratorialList.Items.ToList());
                    viewModel.FormulaExameImagemList =
                        JsonConvert.SerializeObject(formulaExameImagemList.Items.ToList());
                }
            }
            else
            {
                viewModel.FormulaEstoqueList = JsonConvert.SerializeObject(new List<FormulaEstoqueDto>());
                viewModel.FormulaFaturamentoList = JsonConvert.SerializeObject(new List<FormulaFaturamentoDto>());
                viewModel.FormulaExameLaboratorialList = JsonConvert.SerializeObject(new List<FormulaFaturamentoDto>());
                viewModel.FormulaExameImagemList = JsonConvert.SerializeObject(new List<FormulaFaturamentoDto>());
            }
            viewModel.IsCadastroProduto = true;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarModal.cshtml", viewModel);
        }

        [ChildActionOnly]
        public PartialViewResult _IndexFormulaEstoque(long prescricaoItemId)
        {
            var model = new FormulaEstoqueViewModel();
            model.PrescricaoItemId = prescricaoItemId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_IndexFormulaEstoque.cshtml", model);
        }

        [ChildActionOnly]
        public PartialViewResult _IndexFormulaFaturamento(long prescricaoItemId)
        {
            var model = new FormulaFaturamentoViewModel();
            model.PrescricaoItemId = prescricaoItemId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_IndexFormulaFaturamento.cshtml", model);
        }

        [ChildActionOnly]
        public PartialViewResult _IndexFormulaExameLaboratorial(long prescricaoItemId)
        {
            var model = new FormulaExameLaboratorialViewModel();
            model.PrescricaoItemId = prescricaoItemId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_IndexFormulaExameLaboratorial.cshtml", model);
        }

        [ChildActionOnly]
        public PartialViewResult _IndexFormulaExameImagem(long prescricaoItemId)
        {
            var model = new FormulaExameImagemViewModel();
            model.PrescricaoItemId = prescricaoItemId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_IndexFormulaExameImagem.cshtml", model);
        }
        
        [ChildActionOnly]
        public PartialViewResult _IndexSubItemPrescricao(long prescricaoItemId)
        {
            var model = new CriarOuEditarSubItensPrescricaoViewModel {PrescricaoItemId = prescricaoItemId};
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_IndexSubItensPrescricao.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarSubItensPrescricaoModal(long prescricaoItemId, long? subItemPrescricaoId)
        {
            using (var presricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
            using (var presricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem,long>>())
            {
                var model = new CriarOuEditarSubItensPrescricaoViewModel {PrescricaoItemId = prescricaoItemId, SubPrescricaoItemId = subItemPrescricaoId};

                if (subItemPrescricaoId.HasValue)
                {
                    model.SubPrescricaoItem = await presricaoItemAppService.Object.Obter(model.SubPrescricaoItemId.Value).ConfigureAwait(false);
                }

                model.PrescricaoItemDescricao = (await presricaoItemRepository.Object.GetAll()
                    .Where(x => x.Id == prescricaoItemId).FirstOrDefaultAsync().ConfigureAwait(false))?.Descricao;
                
                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarSubItensPrescricaoModal.cshtml",
                    model);
            }
        }
        
       

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaEstoque_Edit)]
        public PartialViewResult _CriarOuEditarFormulaEstoqueModal(long prescricaoItemId, long? id, long? idGrid)
        {
            CriarOuEditarFormulaEstoqueViewModel viewModel;
            if (id.HasValue)
            {
                using (var formulaEstoqueAppService = IocManager.Instance.ResolveAsDisposable<IFormulaEstoqueAppService>())
                {
                    var output = Task.Run(() => formulaEstoqueAppService.Object.Obter(id.Value)).Result;
                    viewModel = new CriarOuEditarFormulaEstoqueViewModel(output);
                }
            }
            else
            {
                viewModel = new CriarOuEditarFormulaEstoqueViewModel(new FormulaEstoqueDto())
                {
                    PrescricaoItemId = prescricaoItemId
                };
            }
            viewModel.PrescricaoItemId = prescricaoItemId;
            if (idGrid.HasValue)
            {
                viewModel.IdGridFormulasEstoque = idGrid.Value;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarFormulaEstoqueModal.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaFaturamento_Edit)]
        //public PartialViewResult _CriarOuEditarFormulaFaturamentoModal(long prescricaoItemId, long? id, long? idGrid)
        public PartialViewResult _CriarOuEditarFormulaFaturamentoModal(long prescricaoItemId, long? id, long? idGrid)
        {
            CriarOuEditarFormulaFaturamentoViewModel viewModel;
            if (id.HasValue)
            {
                using (var formulaFaturamentoAppService = IocManager.Instance.ResolveAsDisposable<IFormulaFaturamentoAppService>())
                {
                    var output = Task.Run(() => formulaFaturamentoAppService.Object.Obter(id.Value)).Result;

                    viewModel = new CriarOuEditarFormulaFaturamentoViewModel(output);
                }
            }
            else
            {
                viewModel = new CriarOuEditarFormulaFaturamentoViewModel(new FormulaFaturamentoDto())
                {
                    PrescricaoItemId = prescricaoItemId
                };
            }
            viewModel.PrescricaoItemId = prescricaoItemId;
            if (idGrid.HasValue)
            {
                viewModel.IdGridFormulasFaturamento = idGrid.Value;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarFormulaFaturamentoModal.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameLaboratorial_Edit)]
        public PartialViewResult _CriarOuEditarFormulaExameLaboratorialModal(long prescricaoItemId, long? id, long? idGrid)
        {
            CriarOuEditarFormulaExameLaboratorialViewModel viewModel;
            if (id.HasValue && id.Value > 0)
            {
                using (var formulaFaturamentoAppService = IocManager.Instance.ResolveAsDisposable<IFormulaFaturamentoAppService>())
                {
                    var query = Task.Run(() => formulaFaturamentoAppService.Object.Obter(id.Value)).Result;
                    var output = new FormulaExameLaboratorialDto
                    {
                        Codigo = query.Codigo,
                        CreationTime = query.CreationTime,
                        CreatorUserId = query.CreatorUserId,
                        DeleterUserId = query.DeleterUserId,
                        DeletionTime = query.DeletionTime,
                        Descricao = query.Descricao,
                        FaturamentoItemId = query.FaturamentoItemId,
                        Id = query.Id,
                        IdGridFormulasExameLaboratorial = query.IdGridFormulasExameLaboratorial,
                        IsDeleted = query.IsDeleted,
                        IsFatura = query.IsFatura,
                        IsSistema = query.IsSistema,
                        LastModificationTime = query.LastModificationTime,
                        LastModifierUserId = query.LastModifierUserId,
                        MaterialId = query.MaterialId,
                        PrescricaoItemId = query.PrescricaoItemId,
                        FaturamentoItem = query.FaturamentoItem,
                        Material = query.Material
                    };
                    //PrescricaoItem = query.PrescricaoItem
                    viewModel = new CriarOuEditarFormulaExameLaboratorialViewModel(output);
                }
            }
            else
            {
                viewModel = new CriarOuEditarFormulaExameLaboratorialViewModel(new FormulaExameLaboratorialDto());
            }
            viewModel.PrescricaoItemId = prescricaoItemId;
            if (idGrid.HasValue)
            {
                viewModel.IdGridFormulasExameLaboratorial = idGrid.Value;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarFormulaExameLaboratorialModal.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_FormulaExameImagem_Edit)]
        public PartialViewResult _CriarOuEditarFormulaExameImagemModal(long prescricaoItemId, long? id, long? idGrid)
        {
            CriarOuEditarFormulaExameImagemViewModel viewModel;
            if (id.HasValue && id.Value > 0)
            {
                using (var formulaFaturamentoAppService = IocManager.Instance.ResolveAsDisposable<IFormulaFaturamentoAppService>())
                {
                    var query = Task.Run(() => formulaFaturamentoAppService.Object.Obter(id.Value)).Result;
                    var output = new FormulaExameImagemDto
                    {
                        Codigo = query.Codigo,
                        CreationTime = query.CreationTime,
                        CreatorUserId = query.CreatorUserId,
                        DeleterUserId = query.DeleterUserId,
                        DeletionTime = query.DeletionTime,
                        Descricao = query.Descricao,
                        FaturamentoItemId = query.FaturamentoItemId,
                        Id = query.Id,
                        IdGridFormulasExameLaboratorial = query.IdGridFormulasExameLaboratorial,
                        IsDeleted = query.IsDeleted,
                        IsFatura = query.IsFatura,
                        IsSistema = query.IsSistema,
                        LastModificationTime = query.LastModificationTime,
                        LastModifierUserId = query.LastModifierUserId,
                        MaterialId = query.MaterialId,
                        PrescricaoItemId = query.PrescricaoItemId,
                        FaturamentoItem = query.FaturamentoItem,
                        Material = query.Material
                    };
                    viewModel = new CriarOuEditarFormulaExameImagemViewModel(output);
                }
            }
            else
            {
                viewModel = new CriarOuEditarFormulaExameImagemViewModel(new FormulaExameImagemDto());
            }
            viewModel.PrescricaoItemId = prescricaoItemId;
            if (idGrid.HasValue)
            {
                viewModel.IdGridFormulasExameImagem = idGrid.Value;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_CriarOuEditarFormulaExameImagemModal.cshtml", viewModel);
        }


       

        public PartialViewResult _FormCriarOuEditar(long? produtoId)
        {
            var model = new PrescricaoItemDto();
            if (produtoId.HasValue && produtoId.Value > 0)
            {
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                {
                    model = Task.Run(() => prescricaoItemAppService.Object.ObterPorProduto(produtoId.Value)).Result;
                    if (model == null)
                    {
                        model = new PrescricaoItemDto();
                    }
                }
            }
            var viewModel = new CriarOuEditarPrescricaoItemViewModel(model);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_FormCriarOuEditar.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Edit, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Delete)]
        public PartialViewResult _Produtos()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_Produtos.cshtml");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Edit, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Delete)]
        public PartialViewResult _Laboratorios()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_Laboratorios.cshtml");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Edit, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_PrescricaoItem_Delete)]
        public PartialViewResult _Imagens()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/PrescricoesItens/_Imagens.cshtml");
        }
    }

}