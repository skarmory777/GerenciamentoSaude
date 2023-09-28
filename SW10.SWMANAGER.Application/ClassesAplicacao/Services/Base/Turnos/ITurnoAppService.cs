using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Turnos
{
    public interface ITurnoAppService : IApplicationService
    {
        Task<ListResultDto<TurnoDto>> ListarTodos();

        Task CriarOuEditar(TurnoDto input);

        Task Excluir(TurnoDto input);

        Task<TurnoDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
