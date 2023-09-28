using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Tabelas
{
    [Table("FatConvenioRelacionamento")]
    public class FaturamentoConvenioRelacionamento : CamposPadraoCRUD
    {
        [ForeignKey("Convenio"), Column("SisConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("Grupo"), Column("FatGrupoId")]
        public long? GrupoId { get; set; }
        public FaturamentoGrupo Grupo { get; set; }

        [ForeignKey("SubGrupo"), Column("FatSubGrupoId")]
        public long? SubGrupoId { get; set; }
        public FaturamentoSubGrupo SubGrupo { get; set; }

        [ForeignKey("TabelaRelacionamento"), Column("FatTabelaRelacionamentoId")]
        public long? TabelaRelacionamentoId { get; set; }
        public FaturamentoTabelaRelacionamento TabelaRelacionamento { get; set; }
    }
}


