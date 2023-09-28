using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto
{
    public class ListarResultadoExamesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public long ColetaId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
