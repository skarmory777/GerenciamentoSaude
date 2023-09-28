using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Input
{
    public class ResultadoColetaInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long ColetaId { get; set; }

        public virtual void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
