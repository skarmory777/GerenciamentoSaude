using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto
{
    public class ListarFormRespostaInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public long FormId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id";
            }
        }
    }
}
