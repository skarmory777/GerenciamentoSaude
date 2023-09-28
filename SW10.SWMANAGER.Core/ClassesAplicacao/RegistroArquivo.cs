using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    [Table("SisRegistroArquivo")]
    public class RegistroArquivo : CamposPadraoCRUD
    {
        [Index("Sis_Idx_RegistroId")]
        public long RegistroId { get; set; }
        public long? RegistroTabelaId { get; set; }
        [MaxLength(500)]
        [Index("Sis_Idx_Campo")]
        public string Campo { get; set; }
        public byte[] Arquivo { get; set; }

        /// <summary>
        /// Gets or sets the arquivo nome.
        /// </summary>
        public string ArquivoNome { get; set; }

        /// <summary>
        /// Gets or sets the arquivo tipo.
        /// </summary>
        public string ArquivoTipo { get; set; }

        [ForeignKey("RegistroTabelaId")]
        public RegistroTabela RegistroTabela { get; set; }

        public long? AtendimentoId { get; set; }

        [ForeignKey("AtendimentoId")]
        public Atendimento Atendimento { get; set; }

    }
}
