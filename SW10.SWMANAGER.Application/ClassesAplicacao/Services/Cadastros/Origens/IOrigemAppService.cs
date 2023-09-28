using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens
{
    public interface IOrigemAppService : IApplicationService
    {
        Task<CriarOuEditarOrigem> Obter(long id);

        Task<PagedResultDto<OrigemDto>> Listar(ListarOrigensInput input);

        Task<ListResultDto<OrigemDto>> ListarTodos();

        Task<FileDto> ListarParaExcel(ListarOrigensInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(CriarOuEditarOrigem input);

        Task Excluir(CriarOuEditarOrigem input);

        ListResultDto<OrigemDto> ListarDropdown();

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

    }
}
