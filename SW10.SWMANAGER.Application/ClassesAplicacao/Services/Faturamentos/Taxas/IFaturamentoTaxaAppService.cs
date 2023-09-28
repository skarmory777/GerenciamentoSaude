using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto;
using SW10.SWMANAGER.Dto;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas
{
    public interface IFaturamentoTaxaAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoTaxaDto>> Listar(ListarFaturamentoTaxasInput input);

        Task<PagedResultDto<TaxaJTable>> ListarParaJTable(ListarFaturamentoTaxasInput input);

        Task<long> CriarOuEditar(TaxaCrudInput input);

        Task Excluir(FaturamentoTaxaDto input);

        Task<FaturamentoTaxaDto> Obter(long id);

        Task<TaxaCrudInput> ObterTaxaEmpresa(long id);

        Task<FileDto> ListarParaExcel(ListarFaturamentoTaxasInput input);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
    }
}

