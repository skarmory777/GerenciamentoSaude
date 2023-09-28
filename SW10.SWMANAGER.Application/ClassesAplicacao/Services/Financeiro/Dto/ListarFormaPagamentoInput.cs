using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    public class ListarFormaPagamentoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public void Normalize()
        {
            Sorting = "Descricao";
        }
    }
}
