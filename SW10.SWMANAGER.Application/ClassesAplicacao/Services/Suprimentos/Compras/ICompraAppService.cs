using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Compras
{
    public interface ICompraAppService : IApplicationService
    {
        #region ↓ Metodos

        #region → Basico - CRUD
        //Task<CompraRequisicaoDto> CriarOuEditar(CompraRequisicaoDto input);

        //Task Excluir(CompraRequisicaoDto input);

        Task<CompraMotivoPedidoDto> ObterMotivoPedido(long id);

        #endregion

        #region → Basico - Listar
        Task<PagedResultDto<CompraMotivoPedidoDto>> ListarMotivosPedidos(ListarInput input);

        Task<ListResultDto<CompraMotivoPedidoDto>> ListarTodosMotivosPedidos();
        #endregion

        #region → Gets
        #endregion

        #region → Dropdowns
        Task<IResultDropdownList<long>> ListarMotivoPedidoDropdown(DropdownInput dropdownInput);

        #endregion

        #endregion
    }
}
