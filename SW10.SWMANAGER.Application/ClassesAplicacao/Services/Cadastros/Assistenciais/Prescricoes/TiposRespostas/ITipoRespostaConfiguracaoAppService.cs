using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    public interface ITipoRespostaConfiguracaoAppService : IApplicationService
    {
        Task<PagedResultDto<TipoRespostaConfiguracaoDto>> Listar(ListarInput input);

        Task<ListResultDto<TipoRespostaConfiguracaoDto>> ListarTodos();

        Task<ListResultDto<TipoRespostaConfiguracaoDto>> ListarFiltro(string filtro);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<TipoRespostaConfiguracaoDto> CriarOuEditar(TipoRespostaConfiguracaoDto input);

        Task Excluir(TipoRespostaConfiguracaoDto input);

        Task<TipoRespostaConfiguracaoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarInput input);
    }
}
