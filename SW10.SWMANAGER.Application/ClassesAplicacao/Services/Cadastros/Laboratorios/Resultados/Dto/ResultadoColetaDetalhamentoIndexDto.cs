using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto
{
    public class ResultadoColetaDetalhamentoIndexDto:CamposPadraoCRUDDto
    {
        public DateTime? DataColeta { get; set; }
        public long StatusId { get; set; }
        public string StatusDescricao { get; set; }
        public string StatusCor { get; set; }
        public DateTime? DataColetaBaixa { get; set; }
        public string UsuarioColetaBaixa { get; set; }
        public DateTime? DataConferido { get; set; }
        public string UsuarioConferido { get; set; }
        public DateTime? DataDigitado { get; set; }
        public string UsuarioDigitado { get; set; }
        public string ExameCodigo { get; set; }
        public string Exame { get; set; }
        public string ExameDescricao { get; set; }
        public string ExameMneumonico { get; set; }
        public string DescricaoMaterial { get; set; }
        public string CodigoMaterial { get; set; }
        public string MotivoPendencia { get; set; }
        public bool IsPendencia { get; set; }
        public string UsuarioPendencia { get; set; }
        public string PendenciaDateTime { get; set; }
        public string Observacao { get; set; }
    }
}