using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TabelasDominio.Dto
{
    public class ListarTabelasDominioInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public long TipoTabelaId { get; set; }

        public long VersaoTissId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Codigo";
            }
        }
    }
}
