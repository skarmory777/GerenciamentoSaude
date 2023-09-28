using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios
{
    public interface IFormConfigOperacaoAppService : IApplicationService
    {
        Task<PagedResultDto<FormConfigOperacaoDto>> Listar(ListarInput input);

        Task<ListResultDto<FormConfigOperacaoDto>> ListarTodos();

        Task<ListResultDto<FormConfigOperacaoDto>> ListarTodosPorOperacao(long operacaoId);

        Task<IEnumerable<FormConfigDto>> ListarTodosPorOperacaoDapper(long operacaoId);

        Task<ListResultDto<FormConfigOperacaoDto>> ListarPorForm(long id);

        Task<ListResultDto<FormConfigOperacaoDto>> ListarPorOperacao(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task CriarOuEditar(FormConfigOperacaoDto input);

        Task Excluir(FormConfigOperacaoDto input);

        Task<FormConfigOperacaoDto> Obter(long id);

        Task<PagedResultDto<OperacaoDto>> ListarOperacaoPorForm(ListarInput input);

        Task<PagedResultDto<OperacaoDto>> ListarOperacaoSemForm(ListarInput input);

        Task<FormConfigOperacaoDto> Obter(long formConfigId, long operacaoId);
    }
}
