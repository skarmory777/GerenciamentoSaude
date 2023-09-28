using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCentroCusto
{
    [Table("GrupoCentroCusto")]
    public class GrupoCentroCusto : CamposPadraoCRUD
    {
        //public long? TipoGrupoCentroCustosId { get; set; }

        //[ForeignKey("TipoGrupoCentroCustosId")]
        //public virtual TipoGrupoCentroCusto TipoGrupoCentroCustos { get; set; }
    }
}
