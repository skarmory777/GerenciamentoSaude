using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto
{
    public class ResultadoColetaIndexDto
    {
        public long ResultadoExameId { get; set; }
        public DateTime DataColeta { get; set; }
        public string Exame { get; set; }
        public string Cor { get; set; }
    }
}
