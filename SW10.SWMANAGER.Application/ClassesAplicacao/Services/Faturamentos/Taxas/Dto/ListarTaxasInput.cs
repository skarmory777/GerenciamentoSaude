using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto
{
    public class ListarFaturamentoTaxasInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public long ConvenioId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Percentual";
            }
        }
    }
}
