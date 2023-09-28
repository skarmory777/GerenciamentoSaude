using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.BIs.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.BIs
{
    public interface IBiAppService : IApplicationService
    {

        Task<PagedResultDto<BIDto>> Listar(ListarInput input);

        Task<ListResultDto<BIDto>> ListarTodos();

        Task<BIDto> Obter(long id);

        Task<BIDto> ObterPorOperacao(long id);

        Task CriarOuEditar(BIDto input);

        Task Excluir(BIDto input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
