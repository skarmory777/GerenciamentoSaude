using Abp.Domain.Uow;
using SW10.SWMANAGER.Web.Controllers;
using SW10.SWMANAGER.Web.Extensions;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao
{
    public class VisualAsaImportExportController : SWMANAGERControllerBase
    {
        private IUnitOfWorkManager _unitOfWorkManagerAmerican;
        private IUnitOfWorkManager _unitOfWorkManagerLipp;
        VisualAsaImportExportWorkerAmerican _servicoAmerican;
        VisualAsaImportExportWorkerLipp _servicoLipp;

        public VisualAsaImportExportController(
            VisualAsaImportExportWorkerAmerican servicoAmerican,
            VisualAsaImportExportWorkerLipp servicoLipp,
            IUnitOfWorkManager unitOfWorkManagerAmerican,
            IUnitOfWorkManager unitOfWorkManagerLipp
        )
        {
            _unitOfWorkManagerAmerican = unitOfWorkManagerAmerican;
            _unitOfWorkManagerLipp = unitOfWorkManagerLipp;
            _servicoAmerican = servicoAmerican;
            _servicoAmerican.UnitOfWorkManager = _unitOfWorkManagerAmerican;
            _servicoLipp = servicoLipp;
            _servicoLipp.UnitOfWorkManager = _unitOfWorkManagerLipp;
        }
        // GET: Mpa/Assistenciais
        public ActionResult Index()
        {
            if (AbpSession.TenantId == 7)
            {
                if (_servicoAmerican.IsRunning)
                {
                    TempData["VisualAsaImportExport"] = "Started";
                }
                else
                {
                    TempData["VisualAsaImportExport"] = "Stopped";
                }
            }
            else
            {
                if (_servicoLipp.IsRunning)
                {
                    TempData["VisualAsaImportExport"] = "Started";
                }
                else
                {
                    TempData["VisualAsaImportExport"] = "Stopped";
                }
            }
            return View("~/Areas/Mpa/Views/Aplicacao/VisualAsaImportExport/Index.cshtml");
        }

        [UnitOfWork]
        public ContentResult AlternarServico()
        {
            var result = string.Empty;
            if (AbpSession.TenantId == 7)
            {
                if (_servicoAmerican.IsRunning)
                {
                    _servicoAmerican.Stop();
                    result = "Stopped";
                }
                else
                {
                    _servicoAmerican.Start();
                    result = "Started";
                }
            }
            else
            {
                if (_servicoLipp.IsRunning)
                {
                    _servicoLipp.Stop();
                    result = "Stopped";
                }
                else
                {
                    _servicoLipp.Start();
                    result = "Started";
                }
            }
            return Content(result);
        }
    }
}