using Abp.Application.Services;
using Abp.Application.Services.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds
{
    public interface IUltimoIdAppService : IApplicationService
    {
        Task<ListResultDto<UltimoIdDto>> ListarTodos();

        Task CriarOuEditar(UltimoIdDto input);

        //Task Excluir(UltimoIdDto input);

        Task<UltimoIdDto> Obter(long id);

        Task<string> ObterProximoCodigo(string nomeTabela);
    }
}
