using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EnvioEmail.Models
{
    public class MailingTemplate
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Nome")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Título")]
        public string Titulo { get; set; }

        [Required]
        [DisplayName("E-mail de saída")]
        public string EmailSaida { get; set; }

        [Required]
        [DisplayName("Nome de saída")]
        public string NomeSaida { get; set; }

        [Required]
        [System.Web.Mvc.AllowHtml]
        [DisplayName("Conteúdo do Template")]
        public string ContentTemplate { get; set; }

        [Required]
        [DisplayName("Campos disponíveis")]
        public string CamposDisponiveis { get; set; }
    }
}