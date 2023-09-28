using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios
{
    public interface IFaturamentoConfigConvenioGlobalAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoConfigConvenioGlobalDto>> Listar(ListarFaturamentoConfigConvenioGlobaisInput input);

        Task<PagedResultDto<FaturamentoConfigConvenioGlobalDto>> ListarPorConvenio(ListarFaturamentoConfigConvenioGlobaisInput convenioId);

        Task CriarOuEditar(FaturamentoConfigConvenioGlobalDto input);

        Task CriarTodosGrupos(FaturamentoConfigConvenioGlobalDto input);

        Task Excluir(FaturamentoConfigConvenioGlobalDto input);

        Task<FaturamentoConfigConvenioGlobalDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoConfigConvenioGlobaisInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
