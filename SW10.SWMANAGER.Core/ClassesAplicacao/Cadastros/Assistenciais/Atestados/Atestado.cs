using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Atestados
{
    [Table("AssAtestado")]
    public class Atestado : CamposPadraoCRUD
    {
        [Index("Ass_Idx_DataAtendimento")]
        public DateTime DataAtendimento { get; set; }

        public string Conteudo { get; set; }

        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long? MedicoId { get; set; }

        [ForeignKey("Paciente"), Column("SisPacienteId")]
        public long? PacienteId { get; set; }

        [ForeignKey("TipoAtestado"), Column("AssTipoAtestadoId")]
        public long? TipoAtestadoId { get; set; }

        [ForeignKey("ModeloAtestado"), Column("AssModeloAtestadoId")]
        public long? ModeloAtestadoId { get; set; }

        public Medico Medico { get; set; }

        public Paciente Paciente { get; set; }

        public TipoAtestado TipoAtestado { get; set; }

        public ModeloAtestado ModeloAtestado { get; set; }

    }
}
