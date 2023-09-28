using Abp.Runtime.Session;

using FluentEmail;

using SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Desenvolvimento.Projetos;
using SW10.SWMANAGER.Web.Controllers;

using System.Collections.Generic;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Desenvolvimento.Projetos
{
    public class ProjetosController : SWMANAGERControllerBase
    {
        private readonly IProjetoAppService _projetoAppService;

        public ProjetosController(
            IProjetoAppService projetoAppService
            )
        {
            _projetoAppService = projetoAppService;
        }

        public ActionResult Index()
        {
            var model = new ProjetosViewModel();
            var usuarioLogadoId = AbpSession.GetUserId();
            var usuarioLogado = _projetoAppService.UsuarioLogado(usuarioLogadoId);
            model.UsuarioLogado = new KeyValuePair<long, string>(usuarioLogadoId, usuarioLogado.Nome);
            return View("~/Areas/Mpa/Views/Aplicacao/Desenvolvimento/Projetos/Index.cshtml", model);
        }

        public void EnviarEmail(int responsavelId, string descricao, string dataPrevistaInicio)
        {
            var usuarioLogadoId = AbpSession.GetUserId();
            var usuarioLogado = _projetoAppService.UsuarioLogado(usuarioLogadoId);
            var responsavelTarefa = _projetoAppService.UsuarioLogado(responsavelId);

            System.Text.StringBuilder corpoEmail = new System.Text.StringBuilder();

            corpoEmail.Append("<head><meta charset='utf-8'></head><html>");
            corpoEmail.Append("<br> " + responsavelTarefa.Nome);
            corpoEmail.Append("<br>Você recebeu uma tarefa: 'URGENTE AGORA' ");
            corpoEmail.Append("<br>");
            corpoEmail.Append("<br>");
            corpoEmail.Append(string.Format("Descrição da tarefa:  " + descricao));
            corpoEmail.Append("<br>Previsão de ínico:  " + dataPrevistaInicio);
            corpoEmail.Append("<strong><br>");
            corpoEmail.Append("<br>");
            corpoEmail.Append("<br>");
            corpoEmail.Append("<br>");
            //corpoEmail.Append("<div style='font-family:Arial, Helvetica, sans-serif;color:blue'>swmanager.smwe.com.br</div></html>");
            corpoEmail.Append("<div style='font-family:Arial, Helvetica, sans-serif;color:#337ab7'>");
            corpoEmail.Append("<img src='http://swmanager.smwe.com.br/Common/Images/logo-sw-white.jpg' alt='SMWELogo'>");
            corpoEmail.Append("<a href='http://swmanager.smwe.com.br'> www.smwe.com.br </a></div></html>");

            var mailClient = new Email(usuarioLogado.Email, usuarioLogado.Nome);
            // mailClient.Body("Tarefa Urgente");
            mailClient.To(responsavelTarefa.Email);
            mailClient.Subject("Tarefa Urgente");
            mailClient.Body(corpoEmail.ToString());
            mailClient.BodyAsHtml();
            mailClient.Send();
        }

    }
}