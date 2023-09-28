using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContas.Dto
{
    public class ContasQuitacaoPacienteInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime? StartDateEntrega { get; set; }
        public DateTime? EndDateEntrega { get; set; }

        public long? ConvenioId { get; set; }

        public long? PacienteId { get; set; }

        public string? ModalidadeAtendimento { get; set; }

        public string? Situacao { get; set; }
        public void Normalize()
        {
        }
    }
}
