using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto
{
    public class ListarConsultorTabelasInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public long CampoId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
