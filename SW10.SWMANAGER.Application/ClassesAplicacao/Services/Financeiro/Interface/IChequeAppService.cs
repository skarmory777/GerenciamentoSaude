using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface
{
    public interface IChequeAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarChequeNaoUtilziadoPorContaCorrenteDropdown(DropdownInput dropdownInput);
    }
}
