using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Input
{
    public class LaudoResultadoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long ExameId { get; set; }
        public long ResultadoExameId { get; set; }
        public long? ColetaId { get; set; }

        public virtual void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
