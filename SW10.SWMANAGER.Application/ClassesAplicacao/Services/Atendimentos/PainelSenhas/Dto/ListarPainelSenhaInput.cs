using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto
{
    public class ListarPainelSenhaInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public long? TipoLocalChamadaId { get; set; }
        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
