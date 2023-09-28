using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos
{
    public interface ICentroCustoAppService : IApplicationService
    {
        Task<CriarOuEditarCentroCusto> Obter(long id);

        Task<PagedResultDto<CentroCustoDto>> Listar(ListarCentrosCustosInput input);

        Task<ListResultDto<CentroCustoDto>> ListarTodos();

        Task<FileDto> ListarParaExcel(ListarCentrosCustosInput input);

        Task CriarOuEditar(CriarOuEditarCentroCusto input);

        Task Excluir(CriarOuEditarCentroCusto input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarDropdownCodigoCentroCusto(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarDropdownCodigoCentroCustoReceberLancamento(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarDropdownCodigoCentroCustoPorContaAdministrativa(DropdownInput dropdownInput);
    }
}
