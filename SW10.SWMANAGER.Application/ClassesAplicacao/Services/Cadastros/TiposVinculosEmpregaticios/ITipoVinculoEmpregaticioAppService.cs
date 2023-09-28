using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios
{
    public interface ITipoVinculoEmpregaticioAppService : IApplicationService
    {
        Task<PagedResultDto<TipoVinculoEmpregaticioDto>> Listar(ListarTiposVinculosEmpregaticiosInput input);

        Task<ListResultDto<TipoVinculoEmpregaticioDto>> ListarTodos();

        Task CriarOuEditar(TipoVinculoEmpregaticioDto input);

        Task Excluir(TipoVinculoEmpregaticioDto input);

        Task<TipoVinculoEmpregaticioDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposVinculosEmpregaticiosInput input);
    }
}
