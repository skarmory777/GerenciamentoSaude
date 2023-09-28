using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces
{
    public interface IFilaAppService : IApplicationService
    {
        Task<PagedResultDto<FilaIndex>> Listar(ListaFilaInput input);
        Task<FilaDto> Obter(long id);
        Task Excluir(long id);
        Task<DefaultReturn<FilaDto>> CriarOuEditar(FilaDto input);
    }
}
