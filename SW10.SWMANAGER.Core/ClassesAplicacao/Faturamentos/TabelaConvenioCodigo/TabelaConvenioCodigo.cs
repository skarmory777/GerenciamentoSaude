using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaPrecoItens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaConvenioCodigo
{
    [Table("FatTabelaConvenioCodigo")]
    public class TabelaConvenioCodigo : CamposPadraoCRUD
    {
        [Key, Column(Order = 0), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override long Id
        {
            get
            {
                return base.Id;
            }

            set
            {
                base.Id = value;
            }
        }

        public bool IsFromTuss { get; set; }

        [ForeignKey("Convenio"), Key, Column("SisConvenioId", Order = 1), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("TabelaPrecoItem"), Key, Column("FatTabelaPrecoItemId", Order = 2), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long? TabelaPrecoItemId { get; set; }
        public FaturamentoTabelaPrecoItem TabelaPrecoItem { get; set; }
    }
}
