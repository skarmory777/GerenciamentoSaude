using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes
{
    public interface ITipoParticipacaoAppService : IApplicationService
    {
        Task<PagedResultDto<TipoParticipacaoDto>> Listar(ListarTiposParticipacoesInput input);

        Task<ListResultDto<TipoParticipacaoDto>> ListarTodos();

        Task CriarOuEditar(TipoParticipacaoDto input);

        Task Excluir(TipoParticipacaoDto input);

        Task<TipoParticipacaoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposParticipacoesInput input);
    }
}
