using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades
{
    public interface INaturalidadeAppService : IApplicationService
    {
        Task<PagedResultDto<NaturalidadeDto>> Listar(ListarNaturalidadesInput input);

        Task<ListResultDto<NaturalidadeDto>> ListarTodos();

        Task<ListResultDto<NaturalidadeDto>> ListarAutoComplete(string input);

        Task CriarOuEditar(CriarOuEditarNaturalidade input);

        Task Excluir(CriarOuEditarNaturalidade input);

        Task<CriarOuEditarNaturalidade> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarNaturalidadesInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

    }
}
