using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas
{
    public interface IEntradaAppService : IApplicationService
    {
        Task<PagedResultDto<EntradaDto>> Listar(ListarEntradasInput input);

        Task<ListResultDto<EntradaDto>> ListarTodos();

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(CriarOuEditarEntrada input);

        Task Excluir(CriarOuEditarEntrada input);

        Task<CriarOuEditarEntrada> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarEntradasInput input);

        Task<PagedResultDto<CriarOuEditarEntradaItem>> ListarItens(ListarEntradasInput input);

    }
}
