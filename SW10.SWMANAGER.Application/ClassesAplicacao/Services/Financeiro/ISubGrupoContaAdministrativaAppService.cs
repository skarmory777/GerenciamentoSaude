using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface ISubGrupoContaAdministrativaAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
