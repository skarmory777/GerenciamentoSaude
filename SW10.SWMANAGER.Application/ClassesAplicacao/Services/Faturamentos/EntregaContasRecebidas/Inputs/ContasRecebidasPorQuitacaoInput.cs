using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas.Inputs
{
    public class ContasRecebidasPorQuitacaoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long QuitacaoId { get; set; }
        public void Normalize()
        {
        }
    }
}
