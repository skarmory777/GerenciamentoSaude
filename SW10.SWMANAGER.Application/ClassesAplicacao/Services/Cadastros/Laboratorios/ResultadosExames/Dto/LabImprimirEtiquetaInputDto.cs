using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto
{
    public class LabImprimirEtiquetaInputDto
    {
        public long ResultadoId { get; set; }
        
        public List<LabImprimirEtiquetaDto> Etiquetas { get; set; }
        
        public string Impressora { get; set; } 
    }

    public class LabImprimirEtiquetaDto
    {
        public long SetorId { get; set; }
        public List<long> ResultadoExameIds { get; set; }
        public string Modelo { get; set; }
        public int Qtd { get; set; }
    }
}