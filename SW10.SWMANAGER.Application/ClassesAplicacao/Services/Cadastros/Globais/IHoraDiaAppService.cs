using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Globais.HorasDias
{
    public interface IHoraDiaAppService : IApplicationService
    {
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
