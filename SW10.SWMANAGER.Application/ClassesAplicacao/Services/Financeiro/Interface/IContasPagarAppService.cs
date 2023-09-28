using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface IContasPagarAppService : IApplicationService
    {
        DocumentoDto ObterPorPessoaNumero(long pessoaId, string numero);

        Task<ListResultDto<VWRptContaPagarDetalhadoDto>> ListarContaPagarDetalhadoReport(VWRptContaPagarInput input);

        byte[] GerarRelatorio(RelatorioContasApagarDto input);

        byte[] GerarRelatorioQuitacao(RelatorioContasApagarDto input);

        byte[] GerarRelatorioGroupNome(RelatorioContasApagarDto input);

        Task<DefaultReturn<DocumentoDto>> Excluir(long id);

        Task<DocumentoDto> ObterPorLancamento(long id);

        DefaultReturn<DocumentoDto> CriarOuEditar(DocumentoDto input);

        Task<ListResultDto<LancamentoIndex>> ListarLancamento(ListarDocumentoInput input);
        Task<long?> ObterFornecedorId(long pessoaId);
    }
}
