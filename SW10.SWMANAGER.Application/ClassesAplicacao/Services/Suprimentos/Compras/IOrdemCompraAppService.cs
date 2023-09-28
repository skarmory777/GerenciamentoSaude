using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Inputs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras
{
    public interface IOrdemCompraAppService : IApplicationService
    {
        #region ↓ Atributos

        #region → Basico - Listar
        Task<PagedResultDto<OrdemCompraIndexDto>> Listar(ListarOrdensCompraInput input);
        Task<IResultDropdownList<long>> ListarOrdemCompraStatusDropdown(DropdownInput dropdownInput);
        Task<OrdemCompraDto> Obter(long id);
        Task<ListResultDto<OrdemCompraItemDto>> ListarRequisicaoItem(long id);
        Task<PagedResultDto<OrdemCompraItemDto>> ListarItensJson(List<OrdemCompraItemDto> list);
        #endregion

        #region → CRUD
        Task<OrdemCompraDto> CriarOuEditar(OrdemCompraDto input);
        Task Excluir(long id);
        #endregion

        #endregion
    }
}
