using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias
{
    public interface IFaturamentoGuiaAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoGuiaDto>> Listar(ListarGuiasInput input);

        Task<ListResultDto<FaturamentoGuiaDto>> ListarTodos();

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task CriarOuEditar(FaturamentoGuiaDto input);

        Task Excluir(FaturamentoGuiaDto input);

        Task<FaturamentoGuiaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarGuiasInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

    }
}
