using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    public interface ITipoRespostaTipoRespostaConfiguracaoAppService : IApplicationService
    {
        Task<PagedResultDto<TipoRespostaTipoRespostaConfiguracaoDto>> Listar(ListarInput input);

        Task<ListResultDto<TipoRespostaTipoRespostaConfiguracaoDto>> ListarTodos();

        Task<ListResultDto<TipoRespostaTipoRespostaConfiguracaoDto>> ListarFiltro(string filtro);

        Task<ListResultDto<TipoRespostaTipoRespostaConfiguracaoDto>> ListarPorTipoResposta(ListarTipoRespostaTipoRespostaConfiguracaoInput input);

        Task<TipoRespostaTipoRespostaConfiguracaoDto> CriarOuEditar(TipoRespostaTipoRespostaConfiguracaoDto input);

        Task Excluir(TipoRespostaTipoRespostaConfiguracaoDto input);

        Task<TipoRespostaTipoRespostaConfiguracaoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
