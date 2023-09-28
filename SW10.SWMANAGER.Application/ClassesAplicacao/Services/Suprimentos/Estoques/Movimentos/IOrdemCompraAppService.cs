using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface IOrdemCompraAppService : IApplicationService
    {
        Task<ListResultDto<OrdemCompraDto>> ListarTodos();
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
