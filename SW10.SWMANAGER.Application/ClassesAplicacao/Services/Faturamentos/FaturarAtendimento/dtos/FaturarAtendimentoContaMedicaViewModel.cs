using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos
{
    public class FaturarAtendimentoContaMedicaViewModel
    {
        public long AtendimentoId { get; set; }
        public bool IsAmbulatorioEmergencia { get; set; }

        public long ContaMedicaId { get; set; }
        public FaturamentoContaDto ContaMedica { get; set; }
    }
}