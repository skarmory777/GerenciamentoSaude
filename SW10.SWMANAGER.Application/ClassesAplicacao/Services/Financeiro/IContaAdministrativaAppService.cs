using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface IContaAdministrativaAppService : IApplicationService
    {
        Task<ListResultDto<ContaAdministrativaDto>> Listar(ListarContaAdministrativaInput input);
        Task<ContaAdministrativaDto> Obter(long id);
        DefaultReturn<ContaAdministrativaDto> CriarOuEditar(ContaAdministrativaDto input);
        Task Excluir(ContaAdministrativaDto input);
        Task<IResultDropdownList<long>> ListarContaAdministrivaPorEmpresaDropdown(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarContaAdministrivaDespesaDropdown(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarContaAdministrivaDespesaTodasEmpresasDropdown(DropdownInput dropdownInput);


    }
}
