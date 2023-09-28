using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse
{
    public interface IGrupoClasseAppService : IApplicationService
    {
        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);

        Task<PagedResultDto<GrupoClasseDto>> Listar(ListarGruposClasseInput input);

        Task<PagedResultDto<GrupoClasseDto>> ListarJson(List<GrupoClasseDto> list);

        Task<ListResultDto<GrupoClasseDto>> ListarTodos();

        Task<GrupoClasseDto> CriarOuEditar(GrupoClasseDto input);

        Task Excluir(GrupoClasseDto input);

        Task<CriarOuEditarGrupoClasse> Obter(long id);

        Task<ListResultDto<GrupoClasseDto>> ObterPorGrupo(long id);

        Task<ListResultDto<GrupoClasseDto>> ListarPorGrupo(long id);
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
    }
}
