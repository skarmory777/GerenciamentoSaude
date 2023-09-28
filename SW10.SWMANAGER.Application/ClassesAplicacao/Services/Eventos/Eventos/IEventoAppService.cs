using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.Eventos.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.Eventos
{
    public interface IEventoAppService : IApplicationService
    {
        Task<long> CriarOuEditar(EventoDto input);

        Task Excluir(long id);

        Task<EventoDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarEventosInput input);

        Task<PagedResultDto<EventoDto>> ListarTodos();

        Task<PagedResultDto<EventoDto>> ListarFiltro(ListarAtendimentosInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
