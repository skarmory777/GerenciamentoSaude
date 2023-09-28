using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas
{
    public interface IFaturamentoTabelaAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoTabelaDto>> Listar(ListarFaturamentoTabelasInput input);

        Task<long?> CriarOuEditar(FaturamentoTabelaDto input);

        Task Excluir(FaturamentoTabelaDto input);

        Task<FaturamentoTabelaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoTabelasInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
