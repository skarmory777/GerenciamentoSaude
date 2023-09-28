using SW10.SWMANAGER.ClassesAplicacao;

namespace SW10.SWMANAGER.MultiTenancy
{
    public class TenantImportConfig : CamposPadraoCRUD
    {
        public long? TenantId { get; set; }
        public string ConnectionStringNameSw { get; set; }
        public string ConnectionStringNameAsa { get; set; }
    }
}
