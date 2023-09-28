using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class ListarEstoqueKitItemInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long EstoqueKitId { get; set; }
        public string Filtro { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
