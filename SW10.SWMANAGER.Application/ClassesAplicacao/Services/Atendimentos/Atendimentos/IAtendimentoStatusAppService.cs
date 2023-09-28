using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos
{
    public interface IAtendimentoStatusAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
