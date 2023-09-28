using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas
{
    [Table("FatEntregaConta")]
    public class FaturamentoEntregaConta : CamposPadraoCRUD
    {
        [ForeignKey("ContaMedica"), Column("FatContaMedicaId")]
        public long? ContaMedicaId { get; set; }
        public FaturamentoConta ContaMedica { get; set; }

        [ForeignKey("EntregaLote"), Column("FatEntregaLoteId")]
        public long? EntregaLoteId { get; set; }
        public FaturamentoEntregaLote EntregaLote { get; set; }

        public float ValorConta { get; set; }
        public float ValorTaxas { get; set; }
        public float ValorFranquia { get; set; }
        public float ValorProduzido { get; set; }
        public float ValorProduzidoTaxas { get; set; }
        public float ValorRecebido { get; set; }
        public float ValorRecebidoTemp { get; set; }
        public bool IsGlosa { get; set; }
        public bool IsRecebe { get; set; }
        public bool IsRecebeTudo { get; set; }
        public bool IsErroGuia { get; set; }
        public float? ValorGlosaRecuperavel { get; set; }
        public float? ValorGlosaRecuperavelTemp { get; set; }
        public float? ValorGlosaIrrecuperavel { get; set; }
        public float? ValorGlosaIrrecuperavelTemp { get; set; }

        [Index("Fat_Idx_DataEntrega")]
        [DataType(DataType.DateTime)]
        public DateTime? DataEntrega { get; set; }
        [Index("Fat_Idx_DataFinalEntrega")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFinalEntrega { get; set; }
        [Index("Fat_Idx_DataUsuarioEntrega")]
        [DataType(DataType.DateTime)]
        public DateTime? DataUsuarioEntrega { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DataUsuarioTemp { get; set; }

        public long? UsuarioTempId { get; set; }
        public long? UsuarioEntregaId { get; set; }
    }

}


