using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes.Dto;
using System.Threading.Tasks;
using SW10.SWMANAGER.Authorization.Users;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes
{
    public interface IParametrizacoesAppService : IApplicationService
    {
        Task<ParametrizacoesDto> GetParametrizacoes();
        
        ParametrizacoesDto GetParametrizacoesSync();
        
        Task<ParametrizacoesDto> CriarOuEditar(ParametrizacoesDto input);
        
    }
}
