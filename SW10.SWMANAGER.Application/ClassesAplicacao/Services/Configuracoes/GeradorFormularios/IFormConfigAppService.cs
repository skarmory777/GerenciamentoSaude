using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    using System.Collections.Generic;

    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;

    public interface IFormConfigAppService : IApplicationService
    {
        Task<PagedResultDto<FormConfigDto>> Listar(ListarInput input);

        Task<ListResultDto<FormConfigDto>> ListarTodos();

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task CriarOuEditar(FormConfigDto input);

        Task Excluir(FormConfigDto input);

        Task<FormConfigDto> Obter(long id);

        Task<FormConfigDto> ObterDapper(long id);

        Task<FormConfigDto> Clonar(long id);

        Task<ResultDropdownList> ListarRelacionadosDropdown(DropdownInput dropdownInput);

        Task<IEnumerable<FormConfigDto>> ListarRelacionados(long operacaoId, long unidadeOrganizacionalId, long? especialidadeId, long atendimentoId);

        // Reservados
        Task<long> CriarReservadoAndGetId();

        Task<long> ObterReservadoId();

        Task<FormConfigDto> ListarReservados();

        string ObterValorUltimoLancamento(string colConfigName, long? atendimentoId);

    }
}
