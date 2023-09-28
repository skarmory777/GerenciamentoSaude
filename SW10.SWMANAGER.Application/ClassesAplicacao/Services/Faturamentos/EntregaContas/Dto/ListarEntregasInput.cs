using Abp.Extensions;
using Abp.Runtime.Validation;
using SW10.SWMANAGER.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto
{
    public class ListarEntregasInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public string Filtro { get; set; }
        public long? LoteId { get; set; }
        public string EmpresaId { get; set; }
        public string PacienteId { get; set; }
        public string ConvenioId { get; set; }
        public string AtendimentoId { get; set; }
        public string GuiaId { get; set; }
        public string MedicoId { get; set; }
        public string NumeroGuia { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsEmergencia { get; set; }
        public bool IsInternacao { get; set; }
        public long? TipoGuiaId { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Codigo";
            }
        }
    }
}
