using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.SisMoedas
{
    [Table("SisMoedaCotacaoPlano")]
    public class SisMoedaCotacaoPlano : CamposPadraoCRUD
    {

        [ForeignKey("SisMoedaCotacaoId")]
        public SisMoedaCotacao SisMoedaCotacao { get; set; }
        public long? SisMoedaCotacaoId { get; set; }

        [ForeignKey("PlanoId")]
        public Plano Plano { get; set; }
        public long? PlanoId { get; set; }

    }
}

