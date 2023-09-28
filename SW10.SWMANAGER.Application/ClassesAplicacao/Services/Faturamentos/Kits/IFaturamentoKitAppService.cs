using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits
{
    public interface IFaturamentoKitAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoKitDto>> Listar(ListarFaturamentoKitsInput input);

        Task CriarOuEditar(FaturamentoKitDto input);

        Task Excluir(FaturamentoKitDto input);

        Task<FaturamentoKitDto> Obter(long id);

        Task<FaturamentoKitDto> ObterDapper(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoKitsInput input);

        //Task<ListResultDto<FaturamentoKitDto>> ListarPrevio (FaturamentoItemDto[] itens);
        Task<PagedResultDto<FaturamentoItemDto>> ListarPrevio(ListarItensKitFaturamentoInput input);

        Task<ListResultDto<FaturamentoKitDto>> ListarTodos();

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownKitContaMedica(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownKitContaMedicaPorKit(DropdownInput dropdownInput);
    }
}

