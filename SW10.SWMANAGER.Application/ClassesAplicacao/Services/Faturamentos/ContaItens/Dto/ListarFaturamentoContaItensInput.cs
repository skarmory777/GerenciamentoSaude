using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto
{
    public class ListarFaturamentoContaItensInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public CalculoContaItemInput CalculoContaItemInput { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Data";
            }
        }
    }
}
