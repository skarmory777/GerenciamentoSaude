using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse
{
    public interface IGrupoSubClasseAppService : IApplicationService
    {

        Task<PagedResultDto<GrupoSubClasseDto>> Listar(ListarGruposSubClasseInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string term, long id);

        Task<ListResultDto<GrupoSubClasseDto>> ListarTodos();

        Task CriarOuEditar(CriarOuEditarGrupoSubClasse input);

        Task Excluir(CriarOuEditarGrupoSubClasse input);

        Task<CriarOuEditarGrupoSubClasse> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarGruposSubClasseInput input);

        Task<ListResultDto<GrupoSubClasseDto>> ObterPorClasse(long id);

        Task<ListResultDto<GrupoSubClasseDto>> ListarPorClasse(long id);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
