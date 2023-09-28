using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface ITevMovimentoAppService : IApplicationService
    {
        Task<PagedResultDto<TevMovimentoDto>> Listar(ListarInput input);

        Task CriarOuEditar(TevMovimentoDto input);

        Task Excluir(long id);

        Task<TevMovimentoDto> Obter(long id);

        Task<TevMovimentoDto> ObterUltimo(long atendimentoId);

        Task<ListResultDto<TevMovimentoDto>> ListarTodos();

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarTevRiscoDropdown(DropdownInput dropdownInput);

    }
}
