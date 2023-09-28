using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface ITipoFreteAppService : IApplicationService
    {
        Task<ListResultDto<TipoFreteDto>> ListarTodos();
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        Task<TipoFreteDto> Obter(long id);
    }
}
