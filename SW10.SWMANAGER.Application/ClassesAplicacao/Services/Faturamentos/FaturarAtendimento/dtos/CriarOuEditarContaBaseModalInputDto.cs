using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos
{
    public class CriarOuEditarContaBaseModalInputDto : CamposPadraoCRUDDto
    {
        public long ContaMedicaId { get; set; }

        public float? Qtde { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public long? TerceirizadoId { get; set; }

        public long? CentroCustoId { get; set; }

        public long? TurnoId { get; set; }

        public long? TipoLeitoId { get; set; }

        public DateTime? HoraIncio { get; set; }
        public DateTime? HoraFim { get; set; }

    }
}