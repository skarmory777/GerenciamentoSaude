using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface IGrupoContaAdministrativaAppService : IApplicationService
    {
        Task<ListResultDto<GrupoContaAdministrativaDto>> Listar(ListarGrupoContaAdministrativaInput input);
        Task<GrupoContaAdministrativaDto> Obter(long id);
        DefaultReturn<GrupoContaAdministrativaDto> CriarOuEditar(GrupoContaAdministrativaDto input);
        Task Excluir(GrupoContaAdministrativaDto input);
    }
}
