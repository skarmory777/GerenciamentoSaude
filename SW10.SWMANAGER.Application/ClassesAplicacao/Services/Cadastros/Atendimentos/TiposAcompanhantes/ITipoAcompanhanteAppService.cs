using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAcompanhantes
{
    public interface ITipoAcompanhanteAppService : IApplicationService
    {
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
