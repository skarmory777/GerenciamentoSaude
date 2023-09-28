using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Receituarios
{
    /// <summary>
    /// Receituário Médico
    /// </summary>
    [Table("AssReceituario")]
    public class ReceituarioMedico : CamposPadraoCRUD
    {
        /// <summary>
        /// Gets or sets the atend id.
        /// </summary>
        [ForeignKey("Atendimento")]
        public long AtendimentoId { get; set; }

        /// <summary>
        /// Gets or sets the atendimento.
        /// </summary>
        public Atendimento Atendimento { get; set; }

        /// <summary>
        /// Gets or sets the data receituário.
        /// </summary>
        [Index("Ass_Idx_DataReceituario")]
        public DateTime DataReceituario { get; set; }

        /// <summary>
        ///  Gets or sets the Médico Id
        /// </summary>
        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long MedicoId { get; set; }
        public Medico Medico { get; set; }

        public string PrescricaoMemedId { get; set; }
    }
}
