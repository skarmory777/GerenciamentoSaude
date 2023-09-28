using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentoMovimento
{
    public class AtendimentoMovimentoDto : CamposPadraoCRUDDto
    {
        public long AtendimentoId { get; set; }

        public DateTime? DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }

        public long MedicoId { get; set; }

        public bool AssumirAtendimento { get; set; }

        public bool IniciarAtendimento { get; set; }
    }
}
