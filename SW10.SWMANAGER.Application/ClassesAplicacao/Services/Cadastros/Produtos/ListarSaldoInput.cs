using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos
{
    public class ListarSaldoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public long? Id { get; set; }

        public long? EstoqueId { get; set; }

        public virtual void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id";
            }
        }
    }
}
