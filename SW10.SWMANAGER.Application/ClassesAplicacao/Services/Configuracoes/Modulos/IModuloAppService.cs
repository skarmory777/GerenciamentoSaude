using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos
{
    public interface IModuloAppService : IApplicationService
    {
        Task<PagedResultDto<ModuloDto>> Listar(ListarInput input);

        Task<ListResultDto<ModuloDto>> ListarTodos();

        Task CriarOuEditar(ModuloDto input);

        Task Excluir(ModuloDto input);

        Task<ModuloDto> Obter(long id);

        //Task<FileDto> ListarParaExcel(ListarInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
