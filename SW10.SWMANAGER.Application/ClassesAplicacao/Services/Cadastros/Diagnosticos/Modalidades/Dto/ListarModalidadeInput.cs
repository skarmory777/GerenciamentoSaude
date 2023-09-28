using Abp.Extensions;
using Abp.Runtime.Validation;

using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Dto
{
    public class ListarModalidadeInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public string IsParecer { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }

}
