using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    public class ListarSisMoedaCotacoesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public string ConvenioId { get; set; }
        public bool IsUco { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
