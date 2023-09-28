using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios
{
    public interface IFaturamentoConfigConvenioAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoConfigConvenioDto>> Listar(ListarFaturamentoConfigConveniosInput input);

        Task<PagedResultDto<FaturamentoConfigConvenioDto>> ListarPorConvenio(ListarFaturamentoConfigConveniosInput convenioId);

        Task<PagedResultDto<FaturamentoConfigConvenioDto>> ListarPorConvenio2(ListarFaturamentoConfigConveniosInput convenioId);

        Task CriarOuEditar(FaturamentoConfigConvenioDto input);

        Task CriarTodosGrupos(FaturamentoConfigConvenioDto input);

        Task Excluir(FaturamentoConfigConvenioDto input);

        Task<FaturamentoConfigConvenioDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoConfigConveniosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<int> VerificarDuplicata(FaturamentoConfigConvenioDto input);

        void GerarLote();

    }
}
