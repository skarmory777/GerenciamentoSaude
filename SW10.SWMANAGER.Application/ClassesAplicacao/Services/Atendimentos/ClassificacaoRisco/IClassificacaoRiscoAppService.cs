using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco
{
    public interface IClassificacaoRiscoAppService : IApplicationService
    {
        Task CriarOuEditar(CriarOuEditarClassificacaoRisco input);

        Task Excluir(long id);

        Task<CriarOuEditarClassificacaoRisco> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarClassificacoesRiscoInput input);

        Task<PagedResultDto<ClassificacaoRiscoDto>> ListarTodos();
    }
}
