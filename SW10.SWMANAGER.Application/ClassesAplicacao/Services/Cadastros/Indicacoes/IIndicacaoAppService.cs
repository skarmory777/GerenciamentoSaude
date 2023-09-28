using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes
{
    public interface IIndicacaoAppService : IApplicationService
    {
        Task<ListResultDto<IndicacaoDto>> ListarTodos();

        Task<PagedResultDto<IndicacaoDto>> Listar(ListarIndicacoesInput input);

        Task CriarOuEditar(CriarOuEditarIndicacao input);

        Task Excluir(CriarOuEditarIndicacao input);

        Task<CriarOuEditarIndicacao> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarIndicacoesInput input);

    }
}
