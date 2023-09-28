using Abp.Extensions;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    public class ListarAtendimentosInput : ListarInput //PagedAndSortedInputDto, IShouldNormalize
    {

        public long UnidadeOrganizacionalId { get; set; }

        public long MedicoId { get; set; }

        public long UserMedicoId { get; set; }

        public long ConvenioId { get; set; }

        public long PacienteId { get; set; }

        public bool Internados { get; set; }

        public bool? IsInternacao { get; set; }

        public bool? IsAmbulatorioEmergencia { get; set; }

        public long NacionalidadeResponsavelId { get; set; }

        // public DateTime startDate2 { get; set; }

        // public DateTime endDate2 { get; set; }

        public string FiltroData { get; set; }

        public string NomePaciente { get; set; }

        public bool FiltroDataAtendimento { get; set; }

        public string TipoAtendimento { get; set; }

        public object AtendimentoStatusId { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataRegistro desc";
            }
        }

        public bool IsAtendimento { get; set; }

        public List<long> UnidadeOrganizacionais { get; set; }
    }
}
