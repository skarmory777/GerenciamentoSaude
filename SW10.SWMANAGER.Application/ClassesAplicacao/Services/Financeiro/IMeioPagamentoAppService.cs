using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface IMeioPagamentoAppService : IApplicationService
    {
        Task<ListResultDto<MeioPagamentoDto>> Listar(ListarMeioPagamentoInput input);
        Task<MeioPagamentoDto> Obter(long id);
        DefaultReturn<MeioPagamentoDto> CriarOuEditar(MeioPagamentoDto input);
        Task Excluir(MeioPagamentoDto input);
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
