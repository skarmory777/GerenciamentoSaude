using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao
{
    public interface IMotivoCaucaoAppService : IApplicationService
    {
        //ListResultDto<MotivoCaucaoDto> GetMotivosCaucao(GetMotivosCaucaoInput input);
        Task<PagedResultDto<MotivoCaucaoDto>> Listar(ListarMotivosCaucaoInput input);

        Task CriarOuEditar(CriarOuEditarMotivoCaucao input);

        Task Excluir(CriarOuEditarMotivoCaucao input);

        Task<CriarOuEditarMotivoCaucao> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarMotivosCaucaoInput input);
    }
}
