using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposTelefone
{
    public interface ITipoTelefoneAppService : IApplicationService
    {
        Task<ListResultDto<TipoTelefoneDto>> ListarTodos();

        Task CriarOuEditar(TipoTelefoneDto input);

        Task Excluir(TipoTelefoneDto input);

        Task<TipoTelefoneDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
