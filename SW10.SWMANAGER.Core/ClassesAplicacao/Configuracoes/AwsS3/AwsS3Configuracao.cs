using Abp.Domain.Entities;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.AwsS3
{
    [Table("AwsS3Configuracao")]
    public class AwsS3Configuracao : Entity<long>
    {
        [ForeignKey("Empresa")]
        public long EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }

        public string AcessKey { get; set; }

        public string SecretKey { get; set; }

        public string BucketName { get; set; }

        public string BucketRegion { get; set; }
    }
}
