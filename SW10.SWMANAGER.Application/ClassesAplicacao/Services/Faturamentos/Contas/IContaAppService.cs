using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Pacote;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas
{
    public interface IContaAppService : IApplicationService
    {

        Task<FaturamentoContaDto> CriarOuEditar(FaturamentoContaDto input);

        Task<PagedResultDto<FaturamentoContaItemTableDto>> ListarItems(FaturamentoContaItemTableFilterDto input);

        Task<PagedResultDto<FaturamentoContaLoteDto>> ListarContasParaLotes(FaturamentoContaLoteFilterDto input);

        Task<DefaultReturn<ResumoContaDto>> ResumoContaAberta(FaturamentoResumoContaFilterDto input);

        Task<DefaultReturn<ResumoContaDto>> ResumoContaFechada(FaturamentoResumoContaFilterDto input);

        Task<DefaultReturn<DefaultReturnBool>> VerificaPacote(CriarOuEditarPacoteModalInputDto input);

        Task<int> VerificaFluxo(long id);
        Task<int> VerificaFluxoVolta(long id);

        Task<long> AlteraStatusConta(long contaMedicaId, int statusId);

        


        Task<DefaultReturn<DefaultReturnBool>> VerificaRemoverItensKit(long contaMedicaId, List<long> itemIds);

        Task<DefaultReturn<DefaultReturnBool>> RemoverItensKit(long contaMedicaId, List<long> itemIds);
        
        Task<User> ObterUsuarioLogadoAsync();
        
        Task ConferirContas(ConferirContasInput input);
        
        Task<PagedResultDto<ContaMedicaViewModel>> Listar(ListarContasInput input);
        
        Task<PagedResultDto<ContaMedicaViewModel>> ListarParaAtendimento(ListarContasInput input);
        
        Task<PagedResultDto<ContaMedicaViewModel>> ListarParaExame(ListarContasInput input);
        
        Task<float> CalcularTotalConta(CalcularTotalContaInput input);
        
        //  Task<float> CalcularValorUnitarioContaItemViaFront(long contaItemId);
        
        // Task<float> CalcularValorUnitarioContaItemPorContaViaFront(long contaId);
        
        Task<bool> VerificarCadastroPrecoItem(VerificarCadastroPrecoInput input);

       

        Task Excluir(FaturamentoContaDto input);
        
        Task<FaturamentoContaDto> Obter(long id);
        
        Task<long> ObterUltimaContaAtendimentoId(long id);
        
        Task<ContaMedicaViewModel> ObterViewModel(long id);
        
        Task<ContaMedicaReportModel> ObterReportModel(long id, long atendimentoId = 0);
        
        Task<FileDto> ListarParaExcel(ListarContasInput input);
        
        Task<PagedResultDto<FaturamentoContaItemViewModel>> ListarItensVM(ListarFaturamentoContaItensInput input);
        
        //  Task<float> CalcularValorUnitarioContaItem (CalculoContaItemInput input);
        
        // Entrega de contas
        Task<PagedResultDto<ContaMedicaViewModel>> ListarNaoConferidas(ListarContasInput input);
        
        Task<PagedResultDto<ContaMedicaViewModel>> ListarParaEntrega(ListarContasInput input);
        
        Task EditarComUsuarioConferencia(FaturamentoContaDto input);
        
        Task<float> ObterValorTotalConta(long contaId);
        
        Task<float> ObterValorContaRegistrado(long contaId);
        
        Task RecalcularValores(long contaId);
    }
}
