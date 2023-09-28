using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Orcamentos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Orcamentos
{
    public interface IOrcamentoAppService : IApplicationService
    {
        Task CriarOuEditar(CriarOuEditarOrcamento input);

        Task Excluir(long id);

        Task<CriarOuEditarOrcamento> Obter(long id);

        //Task<FileDto> ListarParaExcel(ListarOrcamentosInput input);

        Task<PagedResultDto<OrcamentoDto>> ListarTodos();
    }
}
