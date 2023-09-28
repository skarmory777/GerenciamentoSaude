namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos
{
    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AteTextoModelo")]
    public class TextoModelo : CamposPadraoCRUD
    {
        public string Texto { get; set; }

        [Index("Ate_Idx_IsAmbulatorioEmergencia")]
        public bool IsAmbulatorioEmergencia { get; set; }

        [Index("Ate_Idx_IsInternacao")]
        public bool IsInternacao { get; set; }

        public bool IsMostraAtendimento { get; set; }

        /// <summary>
        /// Gets or sets the tipo modelo id.
        /// </summary>
        public virtual long? TipoModeloId { get; set; }

        /// <summary>
        /// Gets or sets the tipo modelo.
        /// </summary>
        [ForeignKey("TipoModeloId")]
        public virtual TipoModelo TipoModelo { get; set; }

        /// <summary>
        /// Gets or sets the tamanho modelo id.
        /// </summary>
        public virtual long? TamanhoModeloId { get; set; }

        /// <summary>
        /// Gets or sets the tamanho modelo.
        /// </summary>
        [ForeignKey("TamanhoModeloId")]
        public virtual TamanhoModelo TamanhoModelo { get; set; }
    }
}
