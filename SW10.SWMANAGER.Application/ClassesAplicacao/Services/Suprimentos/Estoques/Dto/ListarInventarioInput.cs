using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class ListarInventarioInput : ListarInput //PagedAndSortedInputDto, IShouldNormalize
    {
        public long? EstoqueId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id";
            }
        }
    }
}
