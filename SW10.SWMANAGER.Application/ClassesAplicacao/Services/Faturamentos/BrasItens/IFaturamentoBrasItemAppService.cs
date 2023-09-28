#region Usings
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens.Dto;
using SW10.SWMANAGER.Dto;
using System.Threading.Tasks;
#endregion usings.
namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens
{
    // teste merge arquivo tinha sumido
    public interface IFaturamentoBrasItemAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoBrasItemDto>> Listar(ListarFaturamentoBrasItensInput input);

        Task CriarOuEditar(FaturamentoBrasItemDto input);

        Task Excluir(FaturamentoBrasItemDto input);

        Task<FaturamentoBrasItemDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoBrasItensInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

    }
}
