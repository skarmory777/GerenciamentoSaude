using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises
{
    public interface IPaisAppService : IApplicationService
    {
        Task<PagedResultDto<PaisDto>> Listar(ListarPaisesInput input);

        Task<ListResultDto<PaisDto>> ListarTodos();

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(CriarOuEditarPais input);

        Task Excluir(CriarOuEditarPais input);

        Task<CriarOuEditarPais> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarPaisesInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
