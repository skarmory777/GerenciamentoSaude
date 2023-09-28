using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Operacoes;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios
{
    [Table("SisFormConfigOperacao")]
    public class FormConfigOperacao : CamposPadraoCRUD
    {
        [ForeignKey("FormConfig"), Column("SisFormConfigId")]
        public long? FormConfigId { get; set; }

        [ForeignKey("Operacao"), Column("SisOperacaoId")]
        public long? OperacaoId { get; set; }

        public FormConfig FormConfig { get; set; }

        public Operacao Operacao { get; set; }
    }
}
