using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios
{
    public class PainelVerificaExamesDto
    {
        public long? ResultadoId { get; set; }
        public List<long> ResultadoExameIds { get; set; }
    }
}