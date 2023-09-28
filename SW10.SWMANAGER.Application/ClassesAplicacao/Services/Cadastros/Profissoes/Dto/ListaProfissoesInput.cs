using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto
{
    public class ListarProfissoesInput : PagedAndSortedInputDto, IShouldNormalize
    {
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
