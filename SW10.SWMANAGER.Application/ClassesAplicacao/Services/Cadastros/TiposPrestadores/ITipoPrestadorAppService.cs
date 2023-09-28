using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores
{
    public interface ITipoPrestadorAppService : IApplicationService
    {
        //ListResultDto<TipoPrestadorDto> GetTiposPrestadores(GetTiposPrestadoresInput input);
        Task<PagedResultDto<TipoPrestadorDto>> Listar(ListarTiposPrestadoresInput input);

        Task CriarOuEditar(CriarOuEditarTipoPrestador input);

        Task Excluir(CriarOuEditarTipoPrestador input);

        Task<CriarOuEditarTipoPrestador> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposPrestadoresInput input);
    }
}
