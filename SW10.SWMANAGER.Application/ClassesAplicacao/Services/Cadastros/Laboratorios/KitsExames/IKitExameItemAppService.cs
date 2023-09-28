using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames
{
    public interface IKitExameItemAppService : IApplicationService
    {
        Task<PagedResultDto<KitExameItemDto>> Listar(ListarInput input);

        Task<ListResultDto<KitExameItemDto>> ListarFiltro(string filtro);

        Task<ListResultDto<KitExameItemDto>> ListarTodos();

        Task<PagedResultDto<IndexKitExameDto>> ListarPorKit(ListarInput input);

        Task<KitExameItemDto> CriarOuEditar(KitExameItemDto input);

        Task Excluir(long id);

        Task<KitExameItemDto> Obter(long id);

        Task<ResultDropdownList> ListarDropDown(DropdownInput input);

    }
}
