using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos
{
    public interface IIntervaloAppService : IApplicationService
    {
        Task<PagedResultDto<IntervaloDto>> Listar(ListarIntervalosInput input);

        Task<ListResultDto<IntervaloDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarIntervalo input);

        Task Excluir(CriarOuEditarIntervalo input);

        Task<CriarOuEditarIntervalo> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarIntervalosInput input);
    }
}
