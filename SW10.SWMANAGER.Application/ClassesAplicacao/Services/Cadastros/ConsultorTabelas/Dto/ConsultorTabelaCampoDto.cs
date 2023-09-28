using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ConsultorTabelas.Dto
{
    [AutoMap(typeof(ConsultorTabelaCampo))]
    public class ConsultorTabelaCampoDto : CamposPadraoCRUDDto //CampoRelacaoDto
    {
        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(70)]
        public string Campo { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        [StringLength(10)]
        public string Ele { get; set; }

        public long? ConsultorTabelaId { get; set; }

        public long? ConsultorTipoDadoNFId { get; set; }

        public long? ConsultorOcorrenciaId { get; set; }

        public string Tamanho { get; set; }

        public string Observacao { get; set; }

        [ForeignKey("ConsultorTipoDadoNFId")]
        public virtual ConsultorTipoDadoNFDto ConsultorTipoDadoNF { get; set; } // Tabela Auxiliar

        [ForeignKey("ConsultorOcorrenciaId")]
        public virtual ConsultorOcorrenciaDto ConsultorOcorrencia { get; set; } // Multiplicidade

        [ForeignKey("ConsultorTabelaId")]
        public virtual ConsultorTabelaDto ConsultorTabela { get; set; }
    }
}
