using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.BancoAgencias
{

    public interface IBancoAgenciasAppService : IApplicationService
    {
        Task<PagedResultDto<BancoDto>> Listar(BancoAgenciasInput input);
        Task<BancoDto> Obter(long id);
        Task<DefaultReturn<BancoDto>> CriarOuEditar(BancoDto input);
        Task<DefaultReturn<BancoDto>> Excluir(BancoDto input);
        Task<ResultDropdownList> ListarDropdownBanco(DropdownInput dropdownInput);
        Task<ResultDropdownList> ListarDropdownAgencia(DropdownInput dropdownInput);
    }

}
