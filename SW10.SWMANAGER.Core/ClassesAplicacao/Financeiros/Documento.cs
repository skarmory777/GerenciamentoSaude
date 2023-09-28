using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinDocumento")]
    public class Documento : CamposPadraoCRUD
    {
        public long TipoDocumentoId { get; set; }

        [ForeignKey("TipoDocumentoId")]
        public TipoDocumento TipoDocumento { get; set; }

        public long EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        public long? PessoaId { get; set; }

        [ForeignKey("PessoaId")]
        public SisPessoa Pessoa { get; set; }

        public string Numero { get; set; }
        [Index("Fin_Idx_DataEmissao")]
        public DateTimeOffset? DataEmissao { get; set; }

        [Index("Fin_Idx_IsCredito")]
        public bool IsCredito { get; set; }
        
        public decimal? ValorDocumento { get; set; }
        
        public decimal? ValorAcrescimoDecrescimo { get; set; }
        
        public decimal? ValorDesconto { get; set; }

        public string Observacao { get; set; }
        
        public int QuantidadeParcelas { get; set; }

        public List<Lancamento> LancamentoDocumentos { get; set; }
        
        public List<DocumentoRateio> Rateios { get; set; }

        public long? FatContaId { get; set; }

        [ForeignKey("FatContaId")]
        public FaturamentoConta FaturamentoConta { get; set; }

        public long? FatEntregaLoteId { get; set; }

        [ForeignKey("FatEntregaLoteId")]
        public FaturamentoEntregaLote FaturamentoEntregaLote { get; set; }

        public Guid? AnexoListaId { get; set; }

        public long? PreMovimentoId { get; set; }
        [ForeignKey("PreMovimentoId")]
        public EstoquePreMovimento EstoquePreMovimento { get; set; }
    }
}
