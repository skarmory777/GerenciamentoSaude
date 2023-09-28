using Abp.Dependency;
using Microsoft.AspNet.SignalR;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;

namespace SW10.SWMANAGER.SMWEMensageria
{
    public class SMWEHubApp : Hub, ITransientDependency
    {
        // 
        public static void ApenderNovoComentario(ComentarioDto novoComentario)
        {
            var clientes = GlobalHost.ConnectionManager.GetHubContext("SMWEHubApp").Clients;
            clientes.All.apenderNovoComentario(novoComentario);
        }

        public static void GetTarefasExecutando()
        {
            var clientes = GlobalHost.ConnectionManager.GetHubContext("SMWEHubApp").Clients;
            clientes.All.srGetTarefasExecutando();
        }

        //private static dynamic ObterHubClient ()
        //{
        //    IHubContext hubContext = ObterHubContext();
        //    return hubContext.Clients.Client(HubConnectionId);
        //}

        //private static IHubContext ObterHubContext ()
        //{
        //    return GlobalHost.ConnectionManager.GetHubContext("SMWEHub");
        //}

        //public static void LogErro (string erro)
        //{
        //    var hubClient = ObterHubClient();
        // //   hubClient.exibirErro(erro);
        //}
    }
}