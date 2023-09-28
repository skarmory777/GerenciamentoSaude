using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras
{
    [Table("CmpCotacao")]
    public class CompraCotacao : CamposPadraoCRUD
    {
        [ForeignKey("Requisicao"), Column("CmpRequisicaoId")]
        public long RequisicaoId { get; set; }
        public CompraRequisicao Requisicao { get; set; }

        [ForeignKey("Fornecedor"), Column("SisFornecedorId")]
        public long? SisFornecedorId { get; set; }
        public SisFornecedor Fornecedor { get; set; }

        [ForeignKey("FinFormaPagamento"), Column("FinFormaPagamentoId")]
        public long? FinFormaPagamentoId { get; set; }
        public FormaPagamento FinFormaPagamento { get; set; }

        public int? PrazoEntregaEmDias { get; set; }
        [Index("Cmp_Idx_DataEnvioBionexo")]
        public DateTime? DataEnvioBionexo { get; set; }

        public long? UserIdEnvioBionexo { get; set; }

        public long? IdBionexo { get; set; }

        public string MensagemErroRetornoBionexo { get; set; }
        [Index("Cmp_Idx_DataRetornoBionexo")]
        public DateTime? DataRetornoBionexo { get; set; }

        [ForeignKey("CompraCotacaoStatus"), Column("CmpCompraCotacaoStatusId")]
        public long CompraCotacaoStatusId { get; set; }
        public CompraCotacaoStatus CompraCotacaoStatus { get; set; }

        public DateTime? DataStatusCancelado { get; set; }

        public long? UserIdStatusCancelado { get; set; }

        public DateTime? DataStatusFinalizado { get; set; }

        public long? UserIdStatusFinalizado { get; set; }
    }
}