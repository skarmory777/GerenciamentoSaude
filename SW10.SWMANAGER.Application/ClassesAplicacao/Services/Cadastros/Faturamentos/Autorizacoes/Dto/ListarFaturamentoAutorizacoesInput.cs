using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes.Dto
{
    public class ListarAutorizacoesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public void Normalize()
        {
            Sorting = "Descricao";
        }
    }
}
