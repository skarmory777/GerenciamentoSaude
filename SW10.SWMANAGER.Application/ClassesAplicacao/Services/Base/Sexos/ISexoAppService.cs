using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Sexos
{
    public interface ISexoAppService : IApplicationService
    {
        Task<ListResultDto<SexoDto>> ListarTodos();

        Task CriarOuEditar(SexoDto input);

        Task Excluir(SexoDto input);

        Task<SexoDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
