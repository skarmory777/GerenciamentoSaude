using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    public interface ISisMoedaCotacaoAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoCotacaoMoedaDto>> Listar(ListarSisMoedaCotacoesInput input);

        Task<PagedResultDto<FaturamentoCotacaoMoedaDto>> ListarTable(ListarSisMoedaCotacoesInput input);

        Task<PagedResultDto<FaturamentoCotacaoMoedaDto>> ListarPorMoeda(ListarSisMoedaCotacoesInput input);

        Task<DefaultReturn<DefaultReturnBool>> CriarOuEditar(FaturamentoCotacaoMoedaDto input);

        Task Excluir(FaturamentoCotacaoMoedaDto input);

        Task<FaturamentoCotacaoMoedaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarSisMoedaCotacoesInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}
