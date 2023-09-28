using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade
{
    public interface ITipoUnidadeAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<TipoUnidadeDto>> Listar(ListarTiposUnidadeInput input);

        Task<ListResultDto<TipoUnidadeDto>> ListarTodos();

        Task<ListResultDto<TipoUnidadeDto>> ListarSemReferencialGerencial();

        Task CriarOuEditar(CriarOuEditarTipoUnidade input);

        Task Excluir(CriarOuEditarTipoUnidade input);

        Task<CriarOuEditarTipoUnidade> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposUnidadeInput input);
    }
}
