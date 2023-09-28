using Abp.Extensions;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto
{
    public class FaturamentoContaLoteFilterDto : ListarInput
    {
        public DateTime? DataInicial { get; set; }

        public DateTime? DataFinal { get; set; }

        public long? ConvenioId { get; set; }

        public long? PacienteId { get; set; }

        public long? TipoInternacao { get; set; }

        public long FatContaStatusId { get; set; } = FaturamentoContaStatusDto.Conferido;

        public string Periodo { get; set; }

        public override void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "DataInicio desc";
            }
        }
    }


    public class FaturamentoContaLoteDto : CamposPadraoCRUDDto
    {
        public long AtendimentoId { get; set; }
        public long FatContaMedicaId { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public DateTime? DataRegistro { get; set; }
        public DateTime? DataAlta { get; set; }
        public string Matricula { get; set; }
        public string NumeroGuia { get; set; }
        public long? PacienteId { get; set; }
        public string PacienteNomeCompleto { get; set; }
        public long? MedicoId { get; set; }
        public string MedicoNomeCompleto { get; set; }
        public long? ConvenioId { get; set; }
        public string ConvenioNomeFantasia { get; set; }
        public long? PlanoId { get; set; }
        public string PlanoDescricao { get; set; }
        public bool IsAmbulatorioEmergencia { get; set; }
        public string TipoDeGuia { get; set; }

        public decimal ValorConta { get; set; }

    }


}