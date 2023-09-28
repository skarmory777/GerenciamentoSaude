using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml
{
    public interface IElementoHtmlAppService : IApplicationService
    {
        Task<PagedResultDto<ElementoHtmlDto>> Listar(ListarInput input);

        Task<ListResultDto<ElementoHtmlDto>> ListarTodos();

        Task<ListResultDto<ElementoHtmlDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<ElementoHtmlDto> CriarOuEditar(ElementoHtmlDto input);

        Task Excluir(ElementoHtmlDto input);

        Task<ElementoHtmlDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarInput input);
    }
}
