using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas
{
    public interface IEntradaItemAppService : IApplicationService
    {
        Task<EntradaItemDto> CriarOuEditar(CriarOuEditarEntradaItem input, long id);

        Task Editar(CriarOuEditarEntradaItem input);

        Task Excluir(long id);

        Task<PagedResultDto<EntradaItemDto>> Listar(long Id);

        Task<CriarOuEditarEntradaItem> Obter(long id);
    }
}
