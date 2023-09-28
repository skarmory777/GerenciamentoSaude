using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiros.TalaoCheque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.TalaoCheques
{
    public interface ITalaoChequeAppService : IApplicationService
    {
        //Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
        Task<PagedResultDto<TalaoChequeDto>> Listar(TalaoChequeInput input);
        Task<TalaoChequeDto> Obter(long id);
        Task<DefaultReturn<TalaoChequeDto>> CriarOuEditar(TalaoChequeDto input);
        Task<DefaultReturn<TalaoChequeDto>> Excluir(TalaoChequeDto input);
    }
}
