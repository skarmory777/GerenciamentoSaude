using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.DisparoDeMensagem
{
    [Table("SisDisparoDeMensagemItemTipo")]
    public class DisparoDeMensagemItemTipo : CamposPadraoCRUD
    {
        public static long Email = 1;
        public static long WhatsApp = 2;
    }


}
