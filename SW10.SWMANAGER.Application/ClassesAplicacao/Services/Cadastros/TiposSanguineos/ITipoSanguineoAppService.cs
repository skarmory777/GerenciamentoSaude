using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos
{
    public interface ITipoSanguineoAppService : IApplicationService
    {
        Task<PagedResultDto<TipoSanguineoDto>> Listar(ListarTiposSanguineosInput input);

        Task<ListResultDto<TipoSanguineoDto>> ListarTodos();

        Task CriarOuEditar(TipoSanguineoDto input);

        Task Excluir(TipoSanguineoDto input);

        Task<TipoSanguineoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTiposSanguineosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
