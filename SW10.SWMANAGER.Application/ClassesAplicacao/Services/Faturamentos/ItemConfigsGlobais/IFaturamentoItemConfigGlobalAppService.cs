using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs
{
    public interface IFaturamentoItemConfigGlobalAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoItemConfigGlobalDto>> Listar(ListarFaturamentoItemConfigGlobaisInput input);

        Task<PagedResultDto<FaturamentoItemConfigGlobalDto>> ListarPorConvenio(ListarFaturamentoItemConfigGlobaisInput convenioId);

        Task CriarOuEditar(FaturamentoItemConfigGlobalDto input);

        Task CriarTodosGrupos(FaturamentoItemConfigGlobalDto input);

        Task Excluir(FaturamentoItemConfigGlobalDto input);

        Task<FaturamentoItemConfigGlobalDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoItemConfigGlobaisInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        //Task<PagedResultDto<FaturamentoItemConfigGlobalDto>> ListarPorItem (ListarFaturamentoItemConfigGlobaisInput input);

        Task<PagedResultDto<FatItemConfigGlobalGenerico>> ListarPorItem(ListarFaturamentoItemConfigGlobaisInput input);
    }
}
