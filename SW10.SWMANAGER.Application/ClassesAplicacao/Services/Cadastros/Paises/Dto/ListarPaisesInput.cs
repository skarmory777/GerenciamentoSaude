using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto
{
    public class ListarPaisesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public void Normalize()
        {
            Sorting = "Nome";
        }
    }
}
