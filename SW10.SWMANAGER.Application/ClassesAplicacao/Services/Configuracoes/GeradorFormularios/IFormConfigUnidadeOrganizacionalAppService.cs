using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    public interface IFormConfigUnidadeOrganizacionalAppService : IApplicationService
    {
        Task<PagedResultDto<FormConfigUnidadeOrganizacionalDto>> Listar(ListarInput input);

        Task<ListResultDto<FormConfigUnidadeOrganizacionalDto>> ListarTodos();

        Task<ListResultDto<FormConfigUnidadeOrganizacionalDto>> ListarTodosPorUnidade(long unidadeOrganizacional);

        Task<IEnumerable<FormConfigDto>> ListarTodosPorUnidadeDapper(long unidadeOrganizacional);

        Task<ListResultDto<FormConfigUnidadeOrganizacionalDto>> ListarPorForm(long id);

        Task<PagedResultDto<UnidadeOrganizacionalDto>> ListarUnidadeOrganizacionalPorForm(ListarInput input);

        Task<PagedResultDto<UnidadeOrganizacionalDto>> ListarUnidadeOrganizacionalSemForm(ListarInput input);

        Task<ListResultDto<FormConfigUnidadeOrganizacionalDto>> ListarPorUnidadeOrganizacional(long id);

        Task<FormConfigUnidadeOrganizacionalDto> Obter(long formConfigId, long unidadeOrganizacionalId);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task CriarOuEditar(FormConfigUnidadeOrganizacionalDto input);

        Task Excluir(FormConfigUnidadeOrganizacionalDto input);

        Task<FormConfigUnidadeOrganizacionalDto> Obter(long id);

    }
}
