using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs
{
    public interface IFaturamentoGlobalAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoGlobalDto>> Listar(ListarFaturamentoGlobaisInput input);

        Task<PagedResultDto<FaturamentoGlobalDto>> ListarPorConvenio(ListarFaturamentoGlobaisInput convenioId);

        Task CriarOuEditar(FaturamentoGlobalDto input);

        Task CriarTodosGrupos(FaturamentoGlobalDto input);

        Task Excluir(FaturamentoGlobalDto input);

        Task<FaturamentoGlobalDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoGlobaisInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<PagedResultDto<FaturamentoGlobalDto>> ListarPorItem(ListarFaturamentoGlobaisInput input);
    }
}
