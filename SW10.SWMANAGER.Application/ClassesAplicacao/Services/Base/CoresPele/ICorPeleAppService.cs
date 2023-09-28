using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.CoresPele
{
    public interface ICorPeleAppService : IApplicationService
    {
        Task<ListResultDto<CorPeleDto>> ListarTodos();

        Task CriarOuEditar(CorPeleDto input);

        Task Excluir(CorPeleDto input);

        Task<CorPeleDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
