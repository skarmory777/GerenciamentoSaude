using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs
{
    public class ListarDocumentoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public long? PessoaId { get; set; }
        public DateTime? EmissaoDe { get; set; }
        public DateTime? EmissaoAte { get; set; }
        public long? EmpresaId { get; set; }
        public long? SituacaoLancamentoId { get; set; }
        public string Documento { get; set; }
        public long? ContaAdministrativaId { get; set; }
        public long? CentroCustoId { get; set; }
        public DateTime? VencimentoDe { get; set; }
        public DateTime? VencimentoAte { get; set; }
        public long? MeioPagamentoId { get; set; }
        public long? TipoDocumentoId { get; set; }
        public bool IsCredito { get; set; }
        public bool IgnorarVencimento { get; set; }
        public bool IgnorarEmissao { get; set; }

        public void Normalize()
        {
        }
    }
}
