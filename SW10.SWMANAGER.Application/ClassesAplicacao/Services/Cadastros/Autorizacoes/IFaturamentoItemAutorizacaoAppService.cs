using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Autorizacoes.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Autorizacoes
{
    public interface IFaturamentoItemAutorizacaoAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarItemFaturamentoAutorizacaoPorConvenioDropdown(DropdownInput dropdownInput);
        Task<PagedResultDto<FaturamentoItemAutorizacaoDto>> Listar(ListarFaturamentoItemAutorizacaoInput input);
        Task<FaturamentoItemAutorizacaoDto> Obter(long id);
        DefaultReturn<FaturamentoItemAutorizacaoDto> CriarOuEditar(FaturamentoItemAutorizacaoDto input);
    }
}
