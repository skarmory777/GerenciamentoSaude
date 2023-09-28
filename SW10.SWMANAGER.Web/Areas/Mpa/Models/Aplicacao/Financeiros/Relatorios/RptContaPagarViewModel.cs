using System;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros.Relatorios
{
    public class RptContaPagarViewModel
    {
        public long? EmpresaId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TipoRel { get; set; }
        public int TipoData { get; set; }
        public int Situacao { get; set; }
        public long? TipoDocumentoId { get; set; }
        public long? TipoPessoaId { get; set; }
        public long? PessoaId { get; set; }
        public int SituacaoNotaFiscal { get; set; }
        public int MeioPagamentoId { get; set; }
        public bool IsCredito { get; set; }
    }
}