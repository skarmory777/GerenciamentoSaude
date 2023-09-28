using SW10.SWMANAGER.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.PrescricoesLogs
{
    [Table("AssProntuarioLog")]
    public class ProntuarioLog : CamposPadraoCRUD
    {
        [ForeignKey("Prontuario"), Column("AssProntuarioId")]
        public long? ProntuarioId { get; set; }

        public Prontuario Prontuario { get; set; }

        [ForeignKey("User"), Column("ApbUserId")]
        public long? UserId { get; set; }

        public User User { get; set; }

        public string Anterior { get; set; }

        public string Lancamento { get; set; }

    }
}
