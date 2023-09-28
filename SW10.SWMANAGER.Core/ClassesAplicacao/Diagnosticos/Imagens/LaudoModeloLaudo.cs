using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens
{
    [Table("LauModeloLaudo")]
    public class LaudoModeloLaudo : CamposPadraoCRUD
    {
        [StringLength(10)]
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }

        #region ForeignKey Property
        [Column("LauGrupoId"), ForeignKey("LaudoGrupo")]
        public long? LaudoGrupoId { get; set; }
        #endregion

        #region Foreign Property
        public LaudoGrupo LaudoGrupo { get; set; }
        #endregion

        #region Property
        public string Modelo { get; set; } // STRING? (O AssModeloAtestado possui duas strings, Titulo e Conteudo)

        #endregion

        #region NotMapped Property
        #endregion
    }
}
