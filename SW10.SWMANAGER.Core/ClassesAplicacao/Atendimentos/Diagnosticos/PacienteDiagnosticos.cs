using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCID;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Diagnosticos
{
    [Table("PacienteDiagnosticos")]
    public class PacienteDiagnosticos : CamposPadraoCRUD
    {
        [Index("Idx_DataDiagnostico")]
        public DateTime DataDiagnostico { get; set; }

        [ForeignKey("GrupoCID")]
        public long GrupoCIDId { get; set; }

        public GrupoCID GrupoCID { get; set; }

        [ForeignKey("Paciente")]
        public long PacienteId { get; set; }

        public Paciente Paciente { get; set; }

        [ForeignKey("Atendimento")]
        public long AtendimentoId { get; set; }

        public Atendimento Atendimento { get; set; }
    }
}
