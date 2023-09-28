using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios
{
    public class LaboratorioPainelIndexInput : ListarInput, IShouldNormalize
    {
        // public DateTime DateStart { get; set; }
        // public DateTime DateEnd { get; set; }
        public long? PacienteId { get; set; }
        public long? MedicoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? UnidadeId { get; set; }
        public string TipoAtendimento { get; set; }
        public string StatusId { get; set; }
        
        public string Tipo { get; set; }
        
        public List<long> UnidadesOrganizacionais { get; set; }
        public List<string> LabResultadoStatus { get; set; }
    }
}
