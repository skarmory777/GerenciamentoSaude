using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto
{
    public class SenhaIndex
    {
        public long SenhaMovimentoId { get; set; }
        public int NumeroSenha { get; set; }
        public string NomePaciente { get; set; }
        public string TipoLocalChamada { get; set; }
        public DateTime? Data { get; set; }
        public long? TipoLocalChamadaId { get; set; }
        public string Hospital { get; set; }
    }
}
