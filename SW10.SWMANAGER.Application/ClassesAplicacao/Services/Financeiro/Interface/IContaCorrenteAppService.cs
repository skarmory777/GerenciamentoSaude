using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.ContaTesouraria;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface
{
    public interface IContaCorrenteAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarPorEmpresaDropdown(DropdownInput dropdownInput);
        Task<PagedResultDto<ContaCorrenteDto>> Listar(ContaCorrenteInput input);
        Task<ContaCorrenteDto> Obter(long id);
        Task<DefaultReturn<ContaCorrenteDto>> CriarOuEditar(ContaCorrenteDto input);
        Task<DefaultReturn<ContaCorrenteDto>> Excluir(ContaCorrenteDto input);
    }
}
