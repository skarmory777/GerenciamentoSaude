using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos
{
    [Table("FatGrupoConvenio")]
    public class FaturamentoGrupoConvenio : CamposPadraoCRUD
    {
        [ForeignKey("Convenio"), Column("ConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("Grupo"), Column("GrupoId")]
        public long? GrupoId { get; set; }
        public FaturamentoGrupo Grupo { get; set; }

        public bool IsOutraDespesa { get; set; }

        public override string Codigo { get; set; }

        public bool? IsCobrancaDia { get; set; }
    }
}


