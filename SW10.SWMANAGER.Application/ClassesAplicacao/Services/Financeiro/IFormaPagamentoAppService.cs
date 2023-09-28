using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface IFormaPagamentoAppService : IApplicationService
    {
        Task<ListResultDto<FormaPagamentoDto>> Listar(ListarFormaPagamentoInput input);
        Task<FormaPagamentoDto> Obter(long id);
        DefaultReturn<FormaPagamentoDto> CriarOuEditar(FormaPagamentoDto input);
        Task Excluir(FormaPagamentoDto input);
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
