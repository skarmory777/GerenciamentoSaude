#region Usings
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaKits.Dto;
using System.Threading.Tasks;
#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaKits
{
    public interface IFaturamentoContaKitAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoContaKitDto>> Listar(ListarFaturamentoContaKitsInput input);

        Task<PagedResultDto<FaturamentoContaKitContaMedicaDto>> ListarParaContaMedica(FaturamentoContaKitFilterDto input);
        
        Task<PagedResultDto<FaturamentoContaKitViewModel>> ListarVM(ListarFaturamentoContaKitsInput input);

        Task<long> CriarOuEditar(FaturamentoContaKitDto input);

        Task Excluir(FaturamentoContaKitDto input);

        Task<DefaultReturn<DefaultReturnBool>> RemoverKit(long contaMedicaId, long contaKitId);

        Task ExcluirVM(long id);

        Task<FaturamentoContaKitDto> Obter(long id);

        Task<FaturamentoContaKitViewModel> ObterViewModel(long id);
    }
}
