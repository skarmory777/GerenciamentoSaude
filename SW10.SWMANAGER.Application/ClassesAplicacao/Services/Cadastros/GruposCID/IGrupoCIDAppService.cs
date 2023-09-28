using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID
{
    public interface IGrupoCIDAppService : IApplicationService
    {
        Task<ListResultDto<GrupoCIDDto>> ListarTodos();

        Task<PagedResultDto<GrupoCIDDto>> Listar(ListarGruposCIDInput input);

        Task CriarOuEditar(GrupoCIDDto input);

        Task Excluir(GrupoCIDDto input);

        Task<GrupoCIDDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarGruposCIDInput input);
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
