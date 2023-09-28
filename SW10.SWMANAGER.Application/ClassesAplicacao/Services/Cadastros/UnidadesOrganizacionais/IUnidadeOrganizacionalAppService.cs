using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais
{
    public interface IUnidadeOrganizacionalAppService : IApplicationService
    {
        Task<PagedResultDto<UnidadeOrganizacionalDto>> Listar(ListarUnidadesOrganizacionaisInput input);

        Task<ListResultDto<UnidadeOrganizacionalDto>> ListarTodos();


        Task<ListResultDto<UnidadeOrganizacionalDto>> ListarParaAmbulatorioEmergencia();

        Task<ListResultDto<UnidadeOrganizacionalDto>> ListarParaInternacao();

        Task<IResultDropdownList<long>> ListarDropdownLocalUtilizacao(DropdownInput dropdownInput);

        Task CriarOuEditar(UnidadeOrganizacionalDto input);

        Task Excluir(long id);

        Task<UnidadeOrganizacionalDto> Obter(long id);

        Task<IEnumerable<UnidadeOrganizacionalDto>> ObterIds(List<long> ids);

        Task<FileDto> ListarParaExcel(ListarUnidadesOrganizacionaisInput input);

        // Atendimento
        Task<string> ChecarControlaAlta(long id);

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownEstoque(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownUnidadeAtual(DropdownInput dropdownInput);

        Task<UnidadeOrganizacionalDto> ObterPorId(long id);

        UnidadeOrganizacionalDto ObterPorIdSync(long id);

        Task<IResultDropdownList<long>> ListarDropdownPorUsuario(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownPorUsuarioTipoAtendimento(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarDropdownTodosPorUsuario(DropdownInput dropdownInput);


        Task<IResultDropdownList<long>> ListarDropdownComAtendimentoPorUsuario(DropdownInput dropdownInput);
    }
}
