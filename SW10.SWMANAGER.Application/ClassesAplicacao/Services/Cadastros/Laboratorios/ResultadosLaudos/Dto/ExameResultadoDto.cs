using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto
{
    public class ExameResultadoDto
    {
        public DateTime? DataColeta { get; set; }
        public string Exame { get; set; }
        public string Resultado { get; set; }
        public long? TabelaId { get; set; }
        public long? TipoResultadoId { get; set; }
    }
}
