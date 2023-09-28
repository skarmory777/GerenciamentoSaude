using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes
{
    public interface IOperacaoAppService : IApplicationService
    {
        Task<PagedResultDto<OperacaoDto>> Listar(ListarInput input);

        Task<PagedResultDto<OperacaoDto>> ListarPorModulo(ListarOperacaoInput input);

        Task<ListResultDto<OperacaoDto>> ListarTodos();

        Task CriarOuEditar(OperacaoDto input);

        Task Excluir(OperacaoDto input);

        Task<OperacaoDto> Obter(long id);

        Task<OperacaoDto> ObterPorNome(string name);

        //Task<FileDto> ListarParaExcel(ListarInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<ResultDropdownList> ListarPorModuloDropdown(DropdownInput dropdownInput);

        ResultDropdownList ListarPermissoesDropdown(DropdownInput dropdownInput);
    }
}
