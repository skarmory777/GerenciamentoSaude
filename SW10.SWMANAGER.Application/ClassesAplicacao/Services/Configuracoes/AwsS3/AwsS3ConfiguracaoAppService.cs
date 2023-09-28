using Abp.AutoMapper;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.AwsS3;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.AwsS3.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.AwsS3
{
    public class AwsS3ConfiguracaoAppService : SWMANAGERAppServiceBase, IAwsS3ConfiguracaoAppService
    {
        private readonly IRepository<AwsS3Configuracao, long> _awsS3Configuracao;

        public AwsS3ConfiguracaoAppService(IRepository<AwsS3Configuracao, long> awsS3Configuracao)
        {
            _awsS3Configuracao = awsS3Configuracao;
        }

        public async Task<AwsS3ConfiguracaoDto> ObterConfiguracao(long empresaId)
        {
            var configuracao = await _awsS3Configuracao.FirstOrDefaultAsync(x => x.EmpresaId.Equals(empresaId));
            var configuracaoDto = configuracao.MapTo<AwsS3ConfiguracaoDto>();

            return configuracaoDto;
        }
    }
}
