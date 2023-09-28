using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados
{
    public interface IModeloAtestadoAppService : IApplicationService
    {
        Task<PagedResultDto<ModeloAtestadoDto>> Listar(ListarInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task<ListResultDto<ModeloAtestadoDto>> ListarTodos();

        Task CriarOuEditar(ModeloAtestadoDto input);

        Task Excluir(ModeloAtestadoDto input);

        Task<ModeloAtestadoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarInput input);
    }
}
