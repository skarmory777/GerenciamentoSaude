using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos
{
    public interface ILeitoCaracteristicaAppService : IApplicationService
    {
        Task<PagedResultDto<LeitoCaracteristicaDto>> Listar(ListarLeitoCaracteristicasInput input);

        Task CriarOuEditar(CriarOuEditarLeitoCaracteristica input);

        Task Excluir(CriarOuEditarLeitoCaracteristica input);

        Task<CriarOuEditarLeitoCaracteristica> Obter(long id);

        //	Task<FileDto> ListarParaExcel (ListarLeitoCaracteristicasInput input);
    }
}
