using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Tabelas;
using SW10.SWMANAGER.ClassesAplicacao.SisMoedas;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ItensTabela
{
    [Table("FatItemTabela")]
    public class FaturamentoItemTabela : CamposPadraoCRUD
    {
        [StringLength(20)]
        public override string Codigo { get; set; }

        public override string Descricao { get; set; }

        [ForeignKey("TabelaId")]
        public FaturamentoTabela Tabela { get; set; }
        public long? TabelaId { get; set; }

        [ForeignKey("ItemId")]
        public FaturamentoItem Item { get; set; }
        public long? ItemId { get; set; }

        [ForeignKey("SisMoedaId")]
        public SisMoeda SisMoeda { get; set; }
        public long? SisMoedaId { get; set; }

        [Index("Fat_Idx_VigenciaDataInicio")]
        [DataType(DataType.DateTime)]
        public DateTime VigenciaDataInicio { get; set; }

        public float? COCH { get; set; }

        public float? HMCH { get; set; }

        public float? ValorTotal { get; set; }

        [Index("Fat_Idx_IsAtivo")]
        public bool IsAtivo { get; set; }

        public int? Auxiliar { get; set; }

        public int? Porte { get; set; }

        public float? Filme { get; set; }

        public float Preco { get; set; }

        public bool IsInclusaoManual { get; set; }
    }

}


