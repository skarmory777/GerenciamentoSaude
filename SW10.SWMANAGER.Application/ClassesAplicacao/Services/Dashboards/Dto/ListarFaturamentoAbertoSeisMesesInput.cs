using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Dashboard.Dto
{
    public class ListarFaturamentoAbertoSeisMesesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public long? EmpresaId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "LancamentoAberto desc";
            }
        }
    }
}
