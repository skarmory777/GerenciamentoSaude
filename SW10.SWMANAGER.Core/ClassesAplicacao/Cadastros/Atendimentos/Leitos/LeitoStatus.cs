using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos
{
    [Table("LeitoStatus")]
    public class LeitoStatus : CamposPadraoCRUD
    {
        [StringLength(7)]
        public string Cor { get; set; }

        public bool IsBloqueioAtendimento { get; set; }
    }
}
