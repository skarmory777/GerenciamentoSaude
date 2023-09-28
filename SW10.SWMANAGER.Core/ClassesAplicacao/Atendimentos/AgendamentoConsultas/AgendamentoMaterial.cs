using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas
{
    [Table("AteAgendamentoMaterial")]
    public class AgendamentoMaterial : CamposPadraoCRUD
    {
        public long? AgendamentoCirurgicoId { get; set; }

        [ForeignKey("AgendamentoCirurgicoId")]
        public AgendamentoCirurgico AgendamentoCirurgico { get; set; }


        public long? FaturamentoItemId { get; set; }

        [ForeignKey("FaturamentoItemId")]
        public FaturamentoItem FaturamentoItem { get; set; }

        public decimal Quantidade { get; set; }

        [Index("Ate_Idx_DataRecebimento")]
        public DateTime? DataRecebimento { get; set; }

        [Index("Ate_Idx_DataPrevista")]
        public DateTime? DataPrevista { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public decimal ValorNotaFiscal { get; set; }

        public long FornecedorId { get; set; }

        [ForeignKey("FornecedorId")]
        public SisFornecedor Fornecedor { get; set; }

        public bool IsCobrarPeloHospital { get; set; }

        public string Material { get; set; }

    }
}
