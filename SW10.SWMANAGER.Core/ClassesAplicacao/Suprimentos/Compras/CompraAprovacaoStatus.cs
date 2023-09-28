using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras
{
    [Table("CmpAprovacaoStatus")]
    public class CompraAprovacaoStatus : CamposPadraoCRUD
    {   
        public bool? IsStatusRequisicao { get; set; }
        public bool? IsStatusCotacao { get; set; }
    }
}
