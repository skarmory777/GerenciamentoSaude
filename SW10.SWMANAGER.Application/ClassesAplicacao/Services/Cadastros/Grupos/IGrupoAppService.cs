using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos
{
    public interface IGrupoAppService : IApplicationService
    {
        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        //ListResultDto<TipoAtendimentoDto> GetTiposAtendimento(GetTiposAtendimentoInput input);
        Task<PagedResultDto<GrupoDto>> Listar(ListarGruposInput input);

        Task<ListResultDto<GrupoDto>> ListarTodos();

        Task<GrupoDto> CriarOuEditar(GrupoDto input);

        Task Excluir(GrupoDto input);

        Task<GrupoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarGruposInput input);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownGruposPorEstoque(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownGruposPorEstoqueIdObrigatorio(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarPorEstoqueDropdown(DropdownInput dropdownInput);
    }
}
