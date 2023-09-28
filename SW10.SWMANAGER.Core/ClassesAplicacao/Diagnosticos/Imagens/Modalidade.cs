using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens
{
    [Table("LauModalidade")]
    public class Modalidade : CamposPadraoCRUD
    {
        #region Key/Index Property
        [Index("IX_LauModalidade_Codigo")]
        [StringLength(10)]
        public override string Codigo { get; set; }
        #endregion

        #region ForeignKey Property
        #endregion

        #region Foreign Property
        #endregion

        #region Property
        public bool IsParecer { get; set; }
        #endregion

        #region NotMapped Property
        #endregion
    }
}
