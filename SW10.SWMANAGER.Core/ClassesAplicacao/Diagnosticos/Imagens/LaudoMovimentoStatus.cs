using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens
{
    [Table("LauMovimentoStatus")]
    public class LaudoMovimentoStatus : CamposPadraoCRUD
    {
        #region Key/Index Property
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [Index("IX_LauMovimentoStatus_Codigo")]
        [StringLength(10)]
        public override string Codigo { get; set; }
        #endregion

        #region ForeignKey Property
        #endregion

        #region Foreign Property
        #endregion


        #region NotMapped Property
        #endregion
    }
}
