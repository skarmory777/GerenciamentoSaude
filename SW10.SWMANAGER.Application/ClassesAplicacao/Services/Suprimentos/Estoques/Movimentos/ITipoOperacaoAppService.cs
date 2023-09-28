using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface ITipoOperacaoAppService : IApplicationService
    {
        Task<PagedResultDto<TipoOperacaoDto>> Listar();
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
