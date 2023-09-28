using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs
{
    public class ListarQuitacoesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public long? PessoaId { get; set; }
        public long? EmpresaId { get; set; }
        public long? ContaCorrenteId { get; set; }
        public long? MeioPagamentoId { get; set; }
        public DateTime? MovimentoDe { get; set; }
        public DateTime? MovimentoAte { get; set; }
        public bool IgnorarDataMovimento { get; set; }
        public DateTime? ConciliacaoDe { get; set; }
        public DateTime? ConciliacaoAte { get; set; }
        public bool IgnorarDataConciliacao { get; set; }

        public void Normalize()
        {
        }
    }
}
