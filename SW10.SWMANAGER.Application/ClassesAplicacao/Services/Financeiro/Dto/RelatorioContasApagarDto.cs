using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    public class RelatorioContasApagarDto
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool IsCredito { get; set; }
        public long? EmpresaId { get; set; }
        
        public long? PessoaId { get; set; }
        public long? SituacaoLancamentoId { get; set; }
    }
}