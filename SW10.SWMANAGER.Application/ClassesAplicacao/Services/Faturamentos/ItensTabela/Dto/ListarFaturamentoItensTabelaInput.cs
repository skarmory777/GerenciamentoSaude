using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto
{
    public class ListarFaturamentoItensTabelaInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public string ItemId { get; set; }

        public string MoedaId { get; set; }

        public string TabelaId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Descricao";
            }
        }
    }
}
