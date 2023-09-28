using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using System.Threading.Tasks;
using static SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.CodigoBarraAppService;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public interface ICodigoBarraAppService : IApplicationService
    {
        Task<EstoqueEtiquetaDto> ObterValorEtiqueta(string codigoBarra);
        Task<DefaultReturn<EtiquetaReturn>> GerarEtiquetas(int? quantidade, long? produtoId, long? loteValidadeId, long? unidadeId);

        Task<DefaultReturn<EstoqueEtiquetaDto>> ObterEtiquetaEValidaSaldo(ObterEtiquetaEValidaSaldoInput input);

        byte[] ImprimirEtiqueta(ImprimirEtiquetaDto input);
    }
}
