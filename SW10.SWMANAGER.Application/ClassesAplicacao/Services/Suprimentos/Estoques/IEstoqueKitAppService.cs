using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public interface IEstoqueKitAppService : IApplicationService
    {
        EstoqueKitDto ObterPeloId(long id);
        EstoqueKitDto Obter(long id);
        Task<ListResultDto<EstoqueKitDto>> ListarTodos();   
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        List<EstoqueKitItemDto> ObterItensKit(long id);
        Task<PagedResultDto<EstoqueKitDto>> Listar(ListarEstoqueKitInput input);
        Task<long?> CriarOuEditar(EstoqueKitDto input);
        Task Excluir(EstoqueKitDto input);
    }
}
