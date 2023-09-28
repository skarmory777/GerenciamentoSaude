using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposDocumento.Dto;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposDocumento
{
	public interface ITipoDocumentoAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<TipoDocumentoDto>> Listar(ListarTiposDocumentoInput input);

        Task<ListResultDto<TipoDocumentoDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarTipoDocumento input);

        Task Excluir(CriarOuEditarTipoDocumento input);

        Task<CriarOuEditarTipoDocumento> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposDocumentoInput input);

        Task<ListResultDto<TipoDocumentoDto>> ListarPorEntradaSaida(bool isEntrada);
    }
}
