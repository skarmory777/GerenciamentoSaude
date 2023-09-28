using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Vistantes.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Visitantes
{
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Visitantes.Dto;

    public interface IVisitanteAppService : IApplicationService
    {
        Task CriarOuEditar(VisitanteDto input);

        Task Excluir(long id);

        Task<PagedResultDto<VisitanteDto>> ListarFiltro(ListarVisitantesInput input);

        Task<PagedResultDto<VisitanteIndexOut>> IndexVisitanteFiltro(ListarVisitantesInput input);

        Task<VisitanteDto> Obter(long id);

        VisitanteDto ObterVisitantePorAtendimentoId(long atendimentoId);

        Task<ResultDropdownList> ListarDropdownModalVisitantePaciente(DropdownInput dropdownInput);

        Task<ResultDropdownList> ListarDropdownModalVisitantePaciente2(DropdownInput dropdownInput);

        //Task<FileDto> ListarParaExcel(ListarVisitantesInput input);

        //Task<PagedResultDto<VisitanteDto>> ListarFiltro(ListarVisitantesInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
