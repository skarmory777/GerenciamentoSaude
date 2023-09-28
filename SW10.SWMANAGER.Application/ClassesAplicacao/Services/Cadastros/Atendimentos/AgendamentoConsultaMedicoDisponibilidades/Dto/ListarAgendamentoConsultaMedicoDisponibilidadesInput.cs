using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Dto
{
    public class ListarAgendamentoConsultaMedicoDisponibilidadesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataFim";
            }
        }
    }
}
