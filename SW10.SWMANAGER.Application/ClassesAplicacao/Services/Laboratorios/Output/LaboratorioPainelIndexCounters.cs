using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios
{
    public class LaboratorioPainelIndexCounters
    {
        public double UrgenteValor { get; set; }
        
        public IEnumerable<LaboratorioPainelIndexCounterUnidade> UnidadesUrgente { get; set; }
        public IEnumerable<LaboratorioPainelIndexCounterStatus> StatusUrgente { get; set; }

        public double RotinaValor { get; set; }
        
        public IEnumerable<LaboratorioPainelIndexCounterUnidade> UnidadesRotina { get; set; }
        
        public IEnumerable<LaboratorioPainelIndexCounterStatus> StatusRotina { get; set; }
        
        public double PendenteValor { get; set; }
        
        public IEnumerable<LaboratorioPainelIndexCounterUnidade> UnidadesPendente { get; set; }
        public IEnumerable<LaboratorioPainelIndexCounterStatus> StatusPendente { get; set; }
        
        public double CulturaValor { get; set; }
        
        public IEnumerable<LaboratorioPainelIndexCounterUnidade> UnidadesCultura { get; set; }
        public IEnumerable<LaboratorioPainelIndexCounterStatus> StatusCultura { get; set; }
        
    }
}