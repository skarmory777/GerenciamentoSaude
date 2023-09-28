using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.Authorization.Roles;
using SW10.SWMANAGER.Authorization.Roles.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Avisos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Avisos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.Aviso;
using SW10.SWMANAGER.Web.Controllers;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class AvisosController: SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/Avisos/Index.cshtml", new IndexAvisoViewModel());
        }

        public ActionResult IndexCriarOuEditar(long? id)
        {
            using(var avisoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Aviso, long>>())
            using (var roleManager = IocManager.Instance.ResolveAsDisposable<RoleManager>())
            {
                var roles = roleManager.Object.Roles.ToList();
                var model = new IndexCriarOuEditarAvisoViewModel
                {
                    Id = id,
                    Roles = roles.Select(x => new RoleListDto {Id = x.Id, DisplayName = x.DisplayName,}).ToList()
                };

                if (id.HasValue && id != 0)
                {
                    model.Aviso = AvisoDto.Mapear(avisoRepository.Object.GetAll().Include(x=> x.Grupos).FirstOrDefault(x=> x.Id == id.Value));
                }

                return View("~/Areas/Mpa/Views/Aplicacao/Apoios/Avisos/IndexCriarOuEditar.cshtml",model );
            }
        }
    }
}