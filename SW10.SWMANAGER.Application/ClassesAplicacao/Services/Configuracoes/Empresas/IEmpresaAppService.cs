using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas
{
    public interface IEmpresaAppService : IApplicationService
    {
        Task<PagedResultDto<EmpresaDto>> Listar(ListarEmpresasInput input);

        Task<ListResultDto<EmpresaDto>> ListarTodos();

        Task CriarOuEditar(EmpresaDto input);

        Task Excluir(EmpresaDto input);

        Task<EmpresaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarEmpresasInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<ResultDropdownList> ListarDropdownPorUsuario(DropdownInput dropdownInput);
    }
}
