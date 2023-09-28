using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs
{
    public interface IFaturamentoItemConfigAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoItemConfigDto>> Listar(ListarFaturamentoItemConfigsInput input);

        Task<PagedResultDto<FaturamentoItemConfigDto>> ListarPorConvenio(ListarFaturamentoItemConfigsInput convenioId);

        Task CriarOuEditar(FaturamentoItemConfigDto input);

        Task CriarTodosGrupos(FaturamentoItemConfigDto input);

        Task Excluir(FaturamentoItemConfigDto input);

        Task<FaturamentoItemConfigDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoItemConfigsInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<PagedResultDto<FaturamentoItemConfigDto>> ListarPorItem(ListarFaturamentoItemConfigsInput input);
    }
}
