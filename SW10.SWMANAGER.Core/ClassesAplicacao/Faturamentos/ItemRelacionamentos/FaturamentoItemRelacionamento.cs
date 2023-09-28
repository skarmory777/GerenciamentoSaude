using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Tabelas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Relacionamento
{
    [Table("FatItemRelacionamento")]
    public class FaturamentoItemRelacionamento : CamposPadraoCRUD
    {
        [ForeignKey("ItemOrigem"), Column("FatItemOrigemId")]
        public long? ItemOrigemId { get; set; }
        public FaturamentoItem ItemOrigem { get; set; }

        [ForeignKey("Convenio"), Column("SisConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("TabelaRelacionamento"), Column("FatTabelaRelacionamentoId")]
        public long? TabelaRelacionamentoId { get; set; }
        public FaturamentoTabelaRelacionamento TabelaRelacionamento { get; set; }

        [ForeignKey("ItemDestino"), Column("FatItemDestinoId")]
        public long? ItemDestinoId { get; set; }
        public FaturamentoItem ItemDestino { get; set; }
    }

}


