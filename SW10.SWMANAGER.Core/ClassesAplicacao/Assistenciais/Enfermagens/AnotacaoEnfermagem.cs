using SW10.SWMANAGER.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Enfermagens
{
    [Table("AssAnotacaoEnfermagem")]
    public class AnotacaoEnfermagem : CamposPadraoCRUD
    {
        [Index("Ass_Idx_DataAnotacao")]
        public DateTime DataAnotacao { get; set; }

        [ForeignKey("User"), Column("SisUserId")]
        public long UserId { get; set; }

        public User Prestador { get; set; }

    }
}
