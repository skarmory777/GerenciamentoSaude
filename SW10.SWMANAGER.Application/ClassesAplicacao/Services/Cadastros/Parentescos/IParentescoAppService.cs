using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos
{
    public interface IParentescoAppService : IApplicationService
    {
        Task<CriarOuEditarParentesco> Obter(long id);

        Task<PagedResultDto<ParentescoDto>> Listar(ListarParentescosInput input);

        Task<ListResultDto<ParentescoDto>> ListarTodos();

        Task<FileDto> ListarParaExcel(ListarParentescosInput input);

        Task CriarOuEditar(CriarOuEditarParentesco input);

        Task Excluir(CriarOuEditarParentesco input);

    }
}
