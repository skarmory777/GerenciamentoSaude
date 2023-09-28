using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito
{
    public interface IMotivoTransferenciaLeitoAppService : IApplicationService
    {
        Task<PagedResultDto<MotivoTransferenciaLeitoDto>> Listar(ListarMotivosTransferenciaLeitoInput input);

        Task CriarOuEditar(CriarOuEditarMotivoTransferenciaLeito input);

        Task Excluir(CriarOuEditarMotivoTransferenciaLeito input);

        Task<CriarOuEditarMotivoTransferenciaLeito> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarMotivosTransferenciaLeitoInput input);
    }
}
