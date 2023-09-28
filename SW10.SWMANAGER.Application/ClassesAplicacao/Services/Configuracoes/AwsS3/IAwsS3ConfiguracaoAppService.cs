using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.AwsS3.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.AwsS3
{
    public interface IAwsS3ConfiguracaoAppService
    {
        Task<AwsS3ConfiguracaoDto> ObterConfiguracao(long empresaId);
    }
}
