using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Tabelas
{
    [Table("FatTabela")]
    public class FaturamentoTabela : CamposPadraoCRUD
    {
        public bool IsAtualizaBrasindice { get; set; }

        //[ForeignKey("TabelaTissItemId")]
        //public virtual TabelaTissItem TabelaTissItem { get; set; }
        public long? TabelaTissItemId { get; set; }

        public bool IsCBHPM { get; set; }
    }

}


