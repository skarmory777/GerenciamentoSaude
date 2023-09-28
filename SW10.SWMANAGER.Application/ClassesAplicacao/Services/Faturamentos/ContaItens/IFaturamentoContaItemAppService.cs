using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Dtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes.Dtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Pacote;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss
{
    public interface IFaturamentoContaItemAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoContaItemDto>> Listar(ListarFaturamentoContaItensInput input);

        Task<PagedResultDto<FaturamentoContaItemDto>> ListarPorConta(ListarFaturamentoContaItensInput input);

        Task<PagedResultDto<FaturamentoContaItemViewModel>> ListarVM(ListarFaturamentoContaItensInput input);

        Task<PagedResultDto<FaturamentoContaItemReportModel>> ListarReportModel(ListarFaturamentoContaItensInput input);

        Task CriarOuEditar(FaturamentoContaItemDto input);

        Task IncluirItemsDoKit(IncluirItemsDoKitInputDto input);

        Task<DefaultReturn<DefaultReturnBool>> IncluirPacoteAvulso(CriarOuEditarPacoteModalInputDto input);
        Task<DefaultReturn<FaturamentoPacoteDto>> IncluirPacote(FaturamentoIncluirPacoteDto input);

        Task ExcluirItens(long contaMedicaId, List<long> itemIds);
        Task Excluir(FaturamentoContaItemDto input);

        Task ExcluirVM(long id);

        Task<FaturamentoContaItemDto> Obter(long id);

        Task<FaturamentoContaItemDto> ObterPorCodigo(string codigo);

        Task<FaturamentoContaItemViewModel> ObterViewModel(long id);

        Task<FaturamentoContaItemReportModel> ObterReportModel(long id);

        //Inserido por Marcus em 03/04/2018
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<float> CalcularValorUnitarioContaItem(CalculoContaItemInput input);
        Task<float> CalcularValorUnitarioContaItemViaFront(long contaItemId);

        Task<float> CalcularValorUnitarioItem(long empresaId, long convenioId, long planoId, FaturamentoContaItemDto fatContaItemDto);

        Task<DefaultReturn<FaturamentoContaItemInsertDto>> InserirItensContaFaturamento(FaturamentoContaItemInsertDto itensConta);

        Task<ValorCodigoTabela> CalcularValorItemFaturamento(long contaId, long faturamentoItemId);

        Task<DefaultReturn<ValorCodigoTabela>> CalcularValorTotalItemFaturamento(ValorTotalItemFaturamentoDto input);

        void ExcluirPacote(long contaItemId);


        public Task<IEnumerable<FaturamentoContaItemDto>> ObterPorContaKit(long contaKitId, long contaId);
    }
}
