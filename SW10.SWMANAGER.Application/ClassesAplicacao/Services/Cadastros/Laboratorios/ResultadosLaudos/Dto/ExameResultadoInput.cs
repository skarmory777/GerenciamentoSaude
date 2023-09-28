using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto
{
    public class ExameResultadoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long exameId { get; set; }
        public long pacienteId { get; set; }


        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataColeta";
            }
        }
    }
}
