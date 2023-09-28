using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public interface ITipoMovimentoAppService : IApplicationService
    {
        Task<PagedResultDto<TipoMovimentoDto>> Listar(bool isEntrada);
        Task<PagedResultDto<TipoMovimentoDto>> ListarTodos();
        Task<IResultDropdownList<long>> ListarPorEntradaOuSaida(DropdownInput input);
        Task<PagedResultDto<TipoMovimentoDto>> ListarTipoMovimentoDevolucao();
        Task<ResultDropdownList> ListarDropdownEntrada(DropdownInput dropdownInput);
        Task<ResultDropdownList> ListarDropdownSaida(DropdownInput dropdownInput);
        Task<ResultDropdownList> ListarDropdownDevolucao(DropdownInput dropdownInput);
        Task<ResultDropdownList> ListarDropdownSolicitacaoSaida(DropdownInput dropdownInput);
    }
}
