using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Conselhos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Conselhos
{
    public interface IConselhoAppService : IApplicationService
    {
        Task<PagedResultDto<ConselhoDto>> Listar(ListarConselhosInput input);

        Task CriarOuEditar(CriarOuEditarConselho input);

        Task Excluir(CriarOuEditarConselho input);

        Task<CriarOuEditarConselho> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
