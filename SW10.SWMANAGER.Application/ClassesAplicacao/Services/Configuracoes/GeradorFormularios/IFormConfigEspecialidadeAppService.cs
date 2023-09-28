using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    public interface IFormConfigEspecialidadeAppService : IApplicationService
    {
        Task<PagedResultDto<FormConfigEspecialidadeDto>> Listar(ListarInput input);

        Task<ListResultDto<FormConfigEspecialidadeDto>> ListarTodos();

        Task<ListResultDto<FormConfigEspecialidadeDto>> ListarPorForm(long id);

        Task<ListResultDto<FormConfigEspecialidadeDto>> ListarPorEspecialidade(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task CriarOuEditar(FormConfigEspecialidadeDto input);

        Task Excluir(FormConfigEspecialidadeDto input);

        Task<FormConfigEspecialidadeDto> Obter(long id);

        Task<PagedResultDto<EspecialidadeDto>> ListarEspecialidadePorForm(ListarInput input);

        Task<PagedResultDto<EspecialidadeDto>> ListarEspecialidadeSemForm(ListarInput input);

        Task<FormConfigEspecialidadeDto> Obter(long formConfigId, long operacaoId);
    }
}
