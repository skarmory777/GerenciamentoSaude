using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos
{
    public interface IPreAtendimentoAppService : IApplicationService
    {
        Task CriarOuEditar(CriarOuEditarPreAtendimento input);

        Task<long> CriarGetId(CriarOuEditarPreAtendimento input);

        Task Excluir(long id);

        Task<CriarOuEditarPreAtendimento> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarPreAtendimentosInput input);

        Task<PagedResultDto<PreAtendimentoDto>> ListarTodos();

        Task<PagedResultDto<ListarPreAtendimentosIndex>> ListarParaIndex(ListarPreAtendimentosInput input);
    }
}
