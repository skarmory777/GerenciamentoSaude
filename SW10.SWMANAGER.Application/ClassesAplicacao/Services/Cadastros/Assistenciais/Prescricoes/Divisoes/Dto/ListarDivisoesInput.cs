using Abp.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes.Dto
{
    public class ListarDivisoesInput : ListarInput
    {
        public long DivisaoPrincipalId { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Ordem";
            }
        }
    }
}
