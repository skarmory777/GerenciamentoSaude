using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos.Dto
{
    public class ListarModeloTextoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public virtual void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
