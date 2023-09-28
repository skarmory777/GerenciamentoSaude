using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Manutencoes.MailingTemplates
{
    [Table("MailingTemplate")]
    public class MailingTemplate : CamposPadraoCRUD
    {
        //[DisplayName("Nome")]
        public string Name { get; set; }

        //[DisplayName("Título")]
        public string Titulo { get; set; }

        //[DisplayName("E-mail de saída")]
        public string EmailSaida { get; set; }

        //[DisplayName("Nome de saída")]
        public string NomeSaida { get; set; }

        //[System.Web.Mvc.AllowHtml]
        //[DisplayName("Conteúdo do Template")]
        public string ContentTemplate { get; set; }

        //[DisplayName("Campos disponíveis")]
        public string CamposDisponiveis { get; set; }
    }
}
