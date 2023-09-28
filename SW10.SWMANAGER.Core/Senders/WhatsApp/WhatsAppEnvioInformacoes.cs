
namespace SW10.SWMANAGER.Senders.WhatsApp
{
    public class WhatsAppEnvioInformacoes : IEnvioInformacoes
    {
        public WhatsAppEnvioInformacoes()
        {

        }

        public WhatsAppEnvioInformacoes(string nomePaciente, long telefone, string mensagem)
        {
            this.NomePaciente = nomePaciente;
            this.Telefone = telefone;
            this.Mensagem = mensagem;
        }

        public string NomePaciente { get; set; }
        public long Telefone { get; set; }
        public string Mensagem { get; set; }
    }
}
