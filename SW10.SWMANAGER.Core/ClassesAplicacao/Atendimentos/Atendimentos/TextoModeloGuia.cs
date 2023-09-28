using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos
{
    [Table("AteTextoGuia")]
    public class TextoModeloGuia : CamposPadraoCRUD
    {
        public long? TextoId { get; set; }

        [ForeignKey("TextoId")]
        public TextoModelo TextoModelo { get; set; }

        public long? FatGuiaId { get; set; }

        [ForeignKey("FatGuiaId")]
        public FaturamentoGuia FatGuia { get; set; }
    }
}
