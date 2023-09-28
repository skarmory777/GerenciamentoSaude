using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos
{
    [Table("FatFaturamentoItemAutorizacao")]
    public class FaturamentoItemAutorizacao : CamposPadraoCRUD
    {
        public long ConvenioId { get; set; }
        public long? FaturamentoItemId { get; set; }
        public long? FaturamentoGrupoId { get; set; }
        public long? FaturamentoSubGrupoId { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }

        [ForeignKey("FaturamentoItemId")]
        public FaturamentoItem FaturamentoItem { get; set; }

        [ForeignKey("FaturamentoGrupoId")]
        public FaturamentoGrupo FaturamentoGrupo { get; set; }

        [ForeignKey("FaturamentoSubGrupoId")]
        public FaturamentoSubGrupo FaturamentoSubGrupo { get; set; }
    }
}
