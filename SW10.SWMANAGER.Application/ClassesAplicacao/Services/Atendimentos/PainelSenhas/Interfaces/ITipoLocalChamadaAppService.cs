using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces
{
    public interface ITipoLocalChamadaAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarTipoLocalChamadaDropdown(DropdownInput dropdownInput);
    }
}
