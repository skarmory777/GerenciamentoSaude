using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios.Input
{
    public class EvolucaoResultadoInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public long? Id { get; set; }
        public long? AtendimentoId { get; set; }
        public string CodigoPaciente { get; set; }
        public string NomePaciente { get; set; }
        /// <summary>
        /// Filtros
        /// </summary>
        public string Filtro { get; set; }
        public DateTime? DateStart { get; set; }

        public long? PacienteId { get; set; }

        public DateTime? DateEnd { get; set; }

        public bool IsAmbulatorioEmergencia { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsDesseAtendimento { get; set; }
        public bool IsDessePaciente { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Referencia";
            }
        }
    }
}
