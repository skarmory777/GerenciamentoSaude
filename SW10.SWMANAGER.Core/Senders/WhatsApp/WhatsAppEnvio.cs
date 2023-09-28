

namespace SW10.SWMANAGER.Senders.WhatsApp
{
    using RestSharp;
    using System.Configuration;
    using System.Threading.Tasks;

    /// <inheritdoc />
    public class WhatsAppEnvio : IEnvio<WhatsAppEnvioInformacoes>
    {
        /// <summary>
        /// WhatsApp Envio Informacao
        /// </summary>
        public WhatsAppEnvioInformacoes whatsAppEnvioInformacao { get; set; }

        /// <inheritdoc />
        public WhatsAppEnvio()
        {
        }

        /// <inheritdoc />
        public WhatsAppEnvio(WhatsAppEnvioInformacoes whatsAppEnvioInformacao)
        {
            this.whatsAppEnvioInformacao = whatsAppEnvioInformacao;
        }

        /// <summary>
        /// Get Http Request
        /// </summary>
        /// <returns>Rest Client</returns>
        private RestClient GetRestClient()
        {
            var url = ConfigurationManager.AppSettings["whatsApp::Url"].ToString();
            var token = ConfigurationManager.AppSettings["whatsApp::Token"].ToString();
            return new RestClient($"{url}/{token}");
        }

        /// <inheritdoc />
        private RestRequest GetRestRequest(WhatsAppEnvioInformacoes envioInformacao)
        {
            var request = new RestRequest();
            request.AddParameter("nome", envioInformacao.NomePaciente);
            request.AddParameter("celular", envioInformacao.Telefone);
            request.AddParameter("message", envioInformacao.Mensagem);
            return request;
        }

        /// <inheritdoc />
        public async Task EnviarAsync()
        {
            await this.EnviarAsync(this.whatsAppEnvioInformacao).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task EnviarAsync(WhatsAppEnvioInformacoes envioInformacao)
        {
            var client = this.GetRestClient();
            await client.PostAsync<SimpleChatResult>(this.GetRestRequest(envioInformacao)).ConfigureAwait(false);
        }


        /// <inheritdoc />
        public void Enviar(WhatsAppEnvioInformacoes envioInformacao)
        {
            var client = this.GetRestClient();
            var result = client.Post<SimpleChatResult>(this.GetRestRequest(envioInformacao));
        }

        /// <inheritdoc />
        public void Enviar()
        {
            this.Enviar(this.whatsAppEnvioInformacao);
        }

        private class SimpleChatParameters
        {
            public long Celular { get; set; }

            public string Nome { get; set; }

            public string message { get; set; }
        }

        private class SimpleChatResult
        {
            public bool Success { get; set; }
        }
    }
}
