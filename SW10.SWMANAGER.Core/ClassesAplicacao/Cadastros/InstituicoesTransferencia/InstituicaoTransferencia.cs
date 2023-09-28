using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.InstituicoesTransferencia
{
    [Table("InstituicaoTransferencia")]
    public class InstituicaoTransferencia : CamposPadraoCRUD
    {
        [StringLength(10)]
        public string CodigoSUS { get; set; }
    }
}
