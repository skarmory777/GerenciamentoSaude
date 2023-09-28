using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados
{
    public interface ITipoAtestadoAppService : IApplicationService
    {
        Task<PagedResultDto<TipoAtestadoDto>> Listar(ListarInput input);

        Task<ListResultDto<TipoAtestadoDto>> ListarTodos();

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(TipoAtestadoDto input);

        Task Excluir(TipoAtestadoDto input);

        Task<TipoAtestadoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarInput input);
    }
}
