using Abp.Dependency;
using Microsoft.AspNet.SignalR;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using SW10.SWMANAGER.SignalR;

namespace SW10.SWMANAGER.SMWeMensageria
{
    public class SMWEHubWeb : BaseHub, ITransientDependency
    {
        #region Cabecalho

        //private readonly IBuscaAppService _buscaAppService;
        //private readonly IColetorAppService _coletorAppService;

        public SMWEHubWeb(
            //IBuscaAppService buscaAppService,
            //IColetorAppService coletorAppService
            )
        {
            //_buscaAppService = buscaAppService;
            //_coletorAppService = coletorAppService;
        }

        #endregion cabecalho.

        public void GetTarefasExecutando()
        {
            var clientes = GlobalHost.ConnectionManager.GetHubContext("SMWEHubApp").Clients;
            clientes.All.srGetTarefasExecutando();
        }

        public void ApenderNovoComentario(ComentarioDto novoComentario)
        {
            //var hubClient = ObterHubClient(Context.ConnectionId);
            //hubClient.apenderNovoComentario(novoComentario);


            var clientes = ObterHubContext().Clients;
            clientes.All.apenderNovoComentario(novoComentario);
            //   enviar uma notificacao para o ícone de notificação.
            //await _appNotifier
            //    .SendMessageAsync(
            //        AbpSession.ToUserIdentifier(),
            //        L("SincronizacaoConcluida"),
            //        "Success".ToPascalCase(CultureInfo.InvariantCulture).ToEnum<NotificationSeverity>()
            //    );

            //Type thisType = GetType();
            //MethodInfo theMethod = thisType.GetMethod(TheCommandString);
            //theMethod.Invoke(this, userParameters);
        }

        private static dynamic ObterHubClient(string hubConnectionId)
        {
            var hubContext = ObterHubContext();
            return hubContext.Clients.Client(hubConnectionId);
        }

        private static IHubContext ObterHubContext()
        {
            return GlobalHost.ConnectionManager.GetHubContext("SMWEHubWeb");
        }
    }
}