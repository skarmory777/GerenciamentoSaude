using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Escolaridades
{
    public interface IEscolaridadeAppService : IApplicationService
    {
        Task<ListResultDto<EscolaridadeDto>> ListarTodos();

        Task CriarOuEditar(EscolaridadeDto input);

        Task Excluir(EscolaridadeDto input);

        Task<EscolaridadeDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
