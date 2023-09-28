using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface ISituacaoLancamentoAppService : IApplicationService
    {
        Task<ListResultDto<SituacaoLancamentoDto>> Listar(ListarSituacaoLancamentoInput input);
        Task<SituacaoLancamentoDto> Obter(long id);
        DefaultReturn<SituacaoLancamentoDto> CriarOuEditar(SituacaoLancamentoDto input);
        Task Excluir(SituacaoLancamentoDto input);
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
