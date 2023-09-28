using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TabelaConvenioCodigos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TabelaConvenioCodigos
{
    public interface ITabelaConvenioCodigoAppService : IApplicationService
    {
        Task<PagedResultDto<TabelaConvenioCodigoDto>> Listar(ListarInput input);

        Task CriarOuEditar(TabelaConvenioCodigoDto input);

        Task Excluir(TabelaConvenioCodigoDto input);

        Task<TabelaConvenioCodigoDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
