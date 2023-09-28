using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos
{
    [Table("FatSubGrupo")]
    public class FaturamentoSubGrupo : CamposPadraoCRUD
    {
        [StringLength(10)]
        public override string Codigo { get; set; }

        [StringLength(100)]
        public override string Descricao { get; set; }

        [ForeignKey("GrupoId")]
        public FaturamentoGrupo Grupo { get; set; }
        public long GrupoId { get; set; }

        public bool IsLaboratorio { get; set; }

        public bool IsLaudo { get; set; }
    }

}


