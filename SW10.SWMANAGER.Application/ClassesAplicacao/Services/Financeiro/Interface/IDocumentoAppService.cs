using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public interface IDocumentoAppService : IApplicationService
    {
        Task<ListResultDto<DocumentoDto>> Listar(ListarDocumentoInput input);
        Task<DocumentoDto> Obter(long id);
        DefaultReturn<DocumentoDto> CriarOuEditar(DocumentoDto input);
        Task Excluir(DocumentoDto input);
        Task<ListResultDto<LancamentoIndex>> ListarLancamento(ListarDocumentoInput input);
        Task<DocumentoDto> ObterPorLancamento(long id);
    }
}
