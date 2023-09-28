using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface ISolicitacaoExamePrioridadeAppService : IApplicationService
    {
        Task<PagedResultDto<SolicitacaoExamePrioridadeDto>> Listar(ListarInput input);

        Task<ListResultDto<SolicitacaoExamePrioridadeDto>> ListarFiltro(string filtro);

        Task<ListResultDto<SolicitacaoExamePrioridadeDto>> ListarTodos();

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<SolicitacaoExamePrioridadeDto> CriarOuEditar(SolicitacaoExamePrioridadeDto input);

        Task Excluir(long id);

        Task<SolicitacaoExamePrioridadeDto> Obter(long id);

    }
}
