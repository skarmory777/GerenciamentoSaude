using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada
{
    public interface ITipoEntradaAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<TipoEntradaDto>> Listar(ListarTiposEntradaInput input);

        Task<ListResultDto<TipoEntradaDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarTipoEntrada input);

        Task Excluir(CriarOuEditarTipoEntrada input);

        Task<CriarOuEditarTipoEntrada> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposEntradaInput input);
    }
}
