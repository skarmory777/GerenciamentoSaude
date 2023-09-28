using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface IRateioCentroCustoAppService : IApplicationService
    {
        Task<ListResultDto<RateioCentroCustoDto>> Listar(ListarRateioCentroCustoInput input);
        Task<RateioCentroCustoDto> Obter(long id);
        DefaultReturn<RateioCentroCustoDto> CriarOuEditar(RateioCentroCustoDto input);
        Task Excluir(RateioCentroCustoDto input);
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        string Obter2(long id);
    }
}
