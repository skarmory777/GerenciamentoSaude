using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface
{
    public interface IContasReceberAppService : IApplicationService
    {
        Task<DefaultReturn<DocumentoDto>> Excluir(long id);
        Task<ListResultDto<LancamentoIndex>> ListarLancamento(ListarDocumentoInput input);
        DefaultReturn<DocumentoDto> CriarOuEditar(DocumentoDto input);
        Task<long?> ObterConvenioId(long pessoaId);
    }
}
