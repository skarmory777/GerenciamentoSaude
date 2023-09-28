using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades
{
    public interface INacionalidadeAppService : IApplicationService
    {
        Task<PagedResultDto<NacionalidadeDto>> Listar(ListarNacionalidadesInput input);

        Task<ListResultDto<NacionalidadeDto>> ListarTodos();
        // Task<ListResultDto<EstoquePreMovimentoDto>> ListarTodos();

        Task<ListResultDto<NacionalidadeDto>> ListarAutoComplete(string input);

        Task CriarOuEditar(CriarOuEditarNacionalidade input);

        Task Excluir(CriarOuEditarNacionalidade input);

        Task<CriarOuEditarNacionalidade> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarNacionalidadesInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
