using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes
{
    public interface IProfissaoAppService : IApplicationService
    {
        Task<PagedResultDto<ProfissaoDto>> Listar(ListarProfissoesInput input);

        Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input);

        Task<ListResultDto<ProfissaoDto>> ListarPorNome(string input);

        Task CriarOuEditar(CriarOuEditarProfissao input);

        Task Excluir(CriarOuEditarProfissao input);

        Task<CriarOuEditarProfissao> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarProfissoesInput input);

        Task<ListResultDto<ProfissaoDto>> ListarTodos();

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

    }
}
