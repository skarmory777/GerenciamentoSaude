using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.TipoConta
{
    public interface ITipoContaCorrenteAppService : IApplicationService
    {
        Task<PagedResultDto<TipoContaCorrenteDto>> Listar(TipoContaCorrenteInput input);
        Task<TipoContaCorrenteDto> Obter(long id);
        Task<DefaultReturn<TipoContaCorrenteDto>> CriarOuEditar(TipoContaCorrenteDto input);
        Task<DefaultReturn<TipoContaCorrenteDto>> Excluir(TipoContaCorrenteDto input);
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
