using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Religioes.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Base.Religioes
{
    public interface IReligiaoAppService : IApplicationService
    {
        Task<ListResultDto<ReligiaoDto>> ListarTodos();

        Task CriarOuEditar(ReligiaoDto input);

        Task Excluir(ReligiaoDto input);

        Task<ReligiaoDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
