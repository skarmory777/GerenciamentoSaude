using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposLocalChamada.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposLocalChamada
{

    public interface ITipoLocalChamadaCoreAppService : IApplicationService
    {
        //Task<PagedResultDto<LocalChamadaDto>> ListarLocalChamada(ListarLocalChamadaCoreInput input);
        Task<PagedResultDto<TipoLocalChamadaDto>> Listar(ListarLocalChamadaCoreInput input);
        Task<TipoLocalChamadaDto> Obter(long id);
        Task<DefaultReturn<TipoLocalChamadaDto>> CriarOuEditar(TipoLocalChamadaDto input);
        Task<DefaultReturn<TipoLocalChamadaDto>> Excluir(TipoLocalChamadaDto input);
    }

}
