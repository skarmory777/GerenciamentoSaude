using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class ListarAgendamentoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long? PacienteId { get; set; }
        public long? MedicoId { get; set; }
        public long? SalaId { get; set; }
        public long? ProcedimentoId { get; set; }
        public DateTime? Inicio { get; set; }
        public DateTime? Fim { get; set; }
        public long? TipoCirurgiaId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id";
            }
        }
    }
}
