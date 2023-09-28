using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.AwsS3;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.AwsS3.Dto
{
    [AutoMapTo(typeof(AwsS3Configuracao))]
    public class AwsS3ConfiguracaoDto
    {
        public long EmpresaId { get; set; }

        public string AcessKey { get; set; }

        public string SecretKey { get; set; }

        public string BucketName { get; set; }

        public string BucketRegion { get; set; }
    }
}
