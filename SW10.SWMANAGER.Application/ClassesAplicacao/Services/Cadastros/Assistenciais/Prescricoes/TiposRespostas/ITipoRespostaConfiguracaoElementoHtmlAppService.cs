using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    public interface ITipoRespostaConfiguracaoElementoHtmlAppService : IApplicationService
    {
        Task<PagedResultDto<TipoRespostaConfiguracaoElementoHtmlDto>> Listar(ListarInput input);

        Task<ListResultDto<TipoRespostaConfiguracaoElementoHtmlDto>> ListarTodos();

        Task<ListResultDto<TipoRespostaConfiguracaoElementoHtmlDto>> ListarFiltro(string filtro);

        Task<ListResultDto<TipoRespostaConfiguracaoElementoHtmlDto>> ListarPorTipoRespostaConfiguracao(ListarTipoRespostaConfiguracaoElementoHtmlInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<TipoRespostaConfiguracaoElementoHtmlDto> CriarOuEditar(TipoRespostaConfiguracaoElementoHtmlDto input);

        Task Excluir(TipoRespostaConfiguracaoElementoHtmlDto input);

        Task<TipoRespostaConfiguracaoElementoHtmlDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarInput input);
    }
}
