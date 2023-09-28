using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas
{
    [Table("ConsultorTabelaCampo")]
    public class ConsultorTabelaCampo : CamposPadraoCRUD
    {
        [StringLength(70)]
        public string Campo { get; set; }

        [StringLength(10)]
        public string Ele { get; set; }

        public string Tamanho { get; set; }

        public string Observacao { get; set; }

        public long? ConsultorTabelaId { get; set; }

        public long? ConsultorTipoDadoNFId { get; set; }

        public long? ConsultorOcorrenciaId { get; set; }

        [ForeignKey("ConsultorTipoDadoNFId")]
        public ConsultorTipoDadoNF ConsultorTipoDadoNF { get; set; } // Tabela Auxiliar

        [ForeignKey("ConsultorTabelaId")]
        public ConsultorTabela ConsultorTabela { get; set; }

        [ForeignKey("ConsultorOcorrenciaId")]
        public ConsultorOcorrencia ConsultorOcorrencia { get; set; } // Multiplicidade
    }
}