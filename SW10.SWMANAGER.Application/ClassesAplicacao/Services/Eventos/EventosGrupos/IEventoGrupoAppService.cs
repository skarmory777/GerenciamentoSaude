using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosGrupos
{
    public interface IEventoGrupoAppService : IApplicationService
    {
        Task<long> CriarOuEditar(AtendimentoDto input);

        Task Excluir(long id);

        Task<AtendimentoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarAtendimentosInput input);

        Task<PagedResultDto<AtendimentoDto>> ListarTodos();

        Task<PagedResultDto<AtendimentoDto>> ListarFiltro(ListarAtendimentosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
