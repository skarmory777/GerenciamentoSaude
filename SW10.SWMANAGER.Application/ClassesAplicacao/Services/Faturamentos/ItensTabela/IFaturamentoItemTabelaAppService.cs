using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela
{
    public interface IFaturamentoItemTabelaAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoItemTabelaDto>> Listar(ListarFaturamentoItensTabelaInput input);

        Task<PagedResultDto<FaturamentoItemTabelaDto>> ListarParaFatItem(ListarFaturamentoItensTabelaInput input);

        Task<PagedResultDto<FaturamentoItemTabelaDto>> ListarParaFatTabela(ListarFaturamentoItensTabelaInput input);

        Task CriarOuEditar(FaturamentoItemTabelaDto input);

        Task Excluir(FaturamentoItemTabelaDto input);

        Task<FaturamentoItemTabelaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoItensTabelaInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);


    }
}
