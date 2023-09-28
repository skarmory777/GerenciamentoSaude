using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Autorizacoes
{
    [Table("FatAutorizacao")]
    public class FaturamentoAutorizacao : CamposPadraoCRUD
    {
        public string Mensagem { get; set; }
        [Index("Fat_Idx_DataInicial")]
        public DateTime DataInicial { get; set; }
        [Index("Fat_Idx_DataFinal")]
        public DateTime? DataFinal { get; set; }
        public bool IsAmbulatorio { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsAutorizacao { get; set; }
        public bool IsLiberacao { get; set; }
        public bool IsJustificativa { get; set; }
        public bool IsBloqueio { get; set; }

        public List<FaturamentoAutorizacaoDetalhe> Detalhe { get; set; }
    }
}


