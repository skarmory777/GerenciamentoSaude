using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames
{
    public interface IKitExameAppService : IApplicationService
    {
        Task<PagedResultDto<KitExameDto>> Listar(ListarKitExamesInput input);

        //Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(KitExameDto input);

        Task Excluir(KitExameDto input);

        Task<KitExameDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarKitExamesInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<ListResultDto<KitExameDto>> ListarFiltro(string filtro);

        Task<ListResultDto<KitExameDto>> ListarTodos();
    }
}
