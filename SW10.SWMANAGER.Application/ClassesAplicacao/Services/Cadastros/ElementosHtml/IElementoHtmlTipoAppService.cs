using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml
{
    public interface IElementoHtmlTipoAppService : IApplicationService
    {
        Task<PagedResultDto<ElementoHtmlTipoDto>> Listar(ListarInput input);

        Task<ListResultDto<ElementoHtmlTipoDto>> ListarTodos();

        Task<ListResultDto<ElementoHtmlTipoDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<ElementoHtmlTipoDto> CriarOuEditar(ElementoHtmlTipoDto input);

        Task Excluir(ElementoHtmlTipoDto input);

        Task<ElementoHtmlTipoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarInput input);
    }
}
