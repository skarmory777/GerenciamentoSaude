using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Avisos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.DisparoDeMensagem.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Avisos
{
    public interface IAvisosAppService : IApplicationService
    {
        Task<PagedResultDto<AvisoDto>> Listar(IndexFiltroAvisoViewModel input);
        
        Task<AvisoDto> CriarOuEditar(AvisoDto input);

        AvisoDto Obter(long id);
        
        void Excluir(long id);
    }
}