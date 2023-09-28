using SW10.SWMANAGER.ClassesAplicacao;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.MultiTenancy
{
    [Table("TenantLogo")]
    public class TenantLogo : CamposPadraoCRUD
    {
        public byte[] Logotipo { get; set; }

        public string LogotipoMimeType { get; set; }

        public long? TenantId { get; set; }

    }
}
