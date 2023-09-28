using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.DocRotulos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public interface IDocRotuloAppService : IApplicationService
    {

        Task<PagedResultDto<DocRotuloDto>> ListarCapitulos();
        Task<PagedResultDto<DocRotuloDto>> ListarModulos(ListardocRotulosInput input);
        Task<PagedResultDto<DocRotuloDto>> ListarPrioridades();
        Task<PagedResultDto<DocRotuloDto>> ListarStatus();

        Task CriarOuEditar(DocRotuloDto input);

        Task Excluir(DocRotuloDto input);

        Task<DocRotuloDto> Obter(long id);

        Task<ResultDropdownList> ListarCapitulosDropdown(DropdownInput dropdownInput);
        Task<ResultDropdownList> ListarSessoesDropdown(DropdownInput dropdownInput);
        Task<ResultDropdownList> ListarAssuntosDropdown(DropdownInput dropdownInput);
        Task<ResultDropdownList> ListarStatusDropdown(DropdownInput dropdownInput);
        //    [AcceptVerbs("GET", "POST", "PUT")]
        Task<ResultDropdownList> ListarPrioridadesDropdown(DropdownInput dropdownInput);

        Task<ResultDropdownList> ListarModulosDropdown(DropdownInput dropdownInput);
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
