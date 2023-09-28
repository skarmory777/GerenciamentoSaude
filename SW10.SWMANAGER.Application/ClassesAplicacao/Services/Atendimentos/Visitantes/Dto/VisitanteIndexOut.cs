using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Visitantes.Dto
{
    public class VisitanteIndexOut : CamposPadraoCRUDDto
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public bool IsAcompanhante { get; set; }
        public bool IsVisitante { get; set; }
        public bool IsMedico { get; set; }
        public bool IsEmergencia { get; set; }
        public bool IsInternado { get; set; }
        public bool IsFornecedor { get; set; }
        public bool IsSetor { get; set; }
        public DateTime? DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public long? AtendimentoId { get; set; }
        public long? LeitoId { get; set; }
        public long? FornecedorId { get; set; }
        public string NomePaciente { get; set; }

        public DateTime? AtendimentoDataRegistro { get; set; }
        public DateTime? AtendimentoDataAlta { get; set; }

        public string LeitoDescricao { get; set; }

        public string UnidadeOrganizacionalDescricao { get; set; }
    }
}
