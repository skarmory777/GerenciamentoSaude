using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto
{
    public class ListarProdutosUnidadeInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Nome";
            }
        }
    }
}
