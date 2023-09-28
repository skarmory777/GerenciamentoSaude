using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID
{
    public interface ICapituloCIDAppService : IApplicationService
    {
        Task<PagedResultDto<CapituloCIDDto>> Listar(ListarCapitulosCIDInput input);

        Task CriarOuEditar(CapituloCIDDto input);

        Task Excluir(CapituloCIDDto input);

        Task<CapituloCIDDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarCapitulosCIDInput input);

    }
}
