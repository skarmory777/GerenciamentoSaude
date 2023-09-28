using SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.Eventos;
using SW10.SWMANAGER.Web.Controllers;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Desenvolvimento.Projetos
{
    public class EventosController : SWMANAGERControllerBase
    {
        private readonly IEventoAppService _projetoAppService;

        public EventosController(
            IEventoAppService eventoAppService
            )
        {
            _projetoAppService = eventoAppService;
        }

        public ActionResult Index()
        {
            // var model = new CriaEditarEventosViewModel();
            //  var usuarioLogadoId = AbpSession.GetUserId();
            //  var usuarioLogado = _projetoAppService.UsuarioLogado(usuarioLogadoId);
            // model.UsuarioLogado = new KeyValuePair<long, string>(usuarioLogadoId,usuarioLogado.Nome);
            return View("~/Areas/Mpa/Views/Aplicacao/Desenvolvimento/Eventos/Index.cshtml");
        }


    }
}