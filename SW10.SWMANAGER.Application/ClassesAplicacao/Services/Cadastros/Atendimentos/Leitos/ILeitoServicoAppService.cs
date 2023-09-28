using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos
{
    public interface ILeitoServicoAppService : IApplicationService
    {
        Task<PagedResultDto<LeitoServicoDto>> Listar(ListarLeitoServicosInput input);

        Task CriarOuEditar(CriarOuEditarLeitoServico input);

        Task Excluir(CriarOuEditarLeitoServico input);

        Task<CriarOuEditarLeitoServico> Obter(long id);

        //	Task<FileDto> ListarParaExcel (ListarLeitoServicosInput input);
    }
}
