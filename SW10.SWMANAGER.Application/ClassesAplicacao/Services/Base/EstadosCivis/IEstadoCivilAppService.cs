using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.EstadosCivis
{
    public interface IEstadoCivilAppService : IApplicationService
    {
        Task<ListResultDto<EstadoCivilDto>> ListarTodos();

        Task CriarOuEditar(EstadoCivilDto input);

        Task Excluir(EstadoCivilDto input);

        Task<EstadoCivilDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
