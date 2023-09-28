using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public interface IProjetoAppService : IApplicationService
    {

        Task<PagedResultDto<ProjetoDto>> ListarTodos();

        Task CriarOuEditar(ProjetoDto input);

        Task Excluir(ProjetoDto input);

        Task<ProjetoDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        UsuarioIdNome UsuarioLogado(long id);
    }
}
