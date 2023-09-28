using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento
{
    public interface IMotivoCancelamentoAppService : IApplicationService
    {
        Task<PagedResultDto<MotivoCancelamentoDto>> Listar(ListarMotivosCancelamentoInput input);

        Task CriarOuEditar(CriarOuEditarMotivoCancelamento input);

        Task Excluir(CriarOuEditarMotivoCancelamento input);

        Task<CriarOuEditarMotivoCancelamento> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarMotivosCancelamentoInput input);
    }
}
