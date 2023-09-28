using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto
{
    public class ResultadoExameIndexDto : CamposPadraoCRUDDto
    {
        public string Mneumonico { get; set; }
        public DateTime? DataColeta { get; set; }
        public string NumeroExame { get; set; }
        public string NomeExame { get; set; }
        public string UsuarioIncluidoId { get; set; }
        public DateTime? DataIncluido { get; set; }
        public string UsuarioDigitadoId { get; set; }
        public DateTime? DataDigitado { get; set; }
        public string UsuarioConferidoId { get; set; }
        public DateTime? DataConferido { get; set; }
        public DateTime? DataPendente { get; set; }
        public string UsuarioImpressoId { get; set; }
        public DateTime? DataImpresso { get; set; }
        public DateTime? DataEnvioEmail { get; set; }
        public long? AtendimentoId { get; set; }
        public long? EmpresaId { get; set; }
        public string Empresa { get; set; }
        public long? ExameStatusId { get; set; }
        public string ExameStatus { get; set; }
        public string ExameStatusCor { get; set; }
    }
}
