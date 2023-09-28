using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasApresentacoes
{
    [Table("FatBrasApresentacao")]
    public class FaturamentoBrasApresentacao : CamposPadraoCRUD
    {
        public float Quantidade { get; set; }
    }

}


