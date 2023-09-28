using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    public interface ITipoRespostaAppService : IApplicationService
    {
        Task<PagedResultDto<TipoRespostaDto>> Listar(ListarTipoRespostaInput input);

        Task<ListResultDto<TipoRespostaDto>> ListarTodos();

        Task<ListResultDto<TipoRespostaDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<TipoRespostaDto> CriarOuEditar(TipoRespostaDto input);

        Task Excluir(TipoRespostaDto input);

        Task<TipoRespostaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarTipoRespostaInput input);

    }
}
