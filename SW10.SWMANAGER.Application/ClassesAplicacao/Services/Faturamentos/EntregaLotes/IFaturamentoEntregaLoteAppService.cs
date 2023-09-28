using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.DomainServices;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaLotes
{
    public interface IFaturamentoEntregaLoteAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoEntregaLoteListOutputDto>> ListarLotes(FaturamentoEntregaLoteInputDto input);

        Task<PagedResultDto<FaturamentoContaLoteDto>> ListarContasPorLote(FaturamentoEntregaLoteListarContasPorLoteInputDto input);

        Task<PagedResultDto<FaturamentoEntregaLoteDto>> Listar(ListarEntregasInput input);

        Task<DefaultReturn<EntregaTissLoteGerado>> GerarLote(long EntregaLoteId);

        Task<DefaultReturn<string>> CriarLote(CriaLoteInput input);

        Task<DefaultReturn<string>> CriarOuEditar(CrudEntregaLoteContaInput input);

        Task Excluir(FaturamentoEntregaLoteDto input);

        Task<FaturamentoEntregaLoteDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarEntregasInput input);

        Task<ListResultDto<FaturamentoEntregaLoteDto>> ListarTodos();

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);


    }
}

