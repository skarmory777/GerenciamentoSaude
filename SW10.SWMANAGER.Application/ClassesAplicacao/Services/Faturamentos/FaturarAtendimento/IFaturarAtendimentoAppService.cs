using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento
{
    public interface IFaturarAtendimentoAppService: IApplicationService
    {
        Task<PagedResultDto<FaturarAtendimentoIndexDto>> ListarAtendimentoFaturamento(FaturarAtendimentoInputDto input);

        Task<PagedResultDto<FaturarAtendimentoIndexDto>> ListarAtendimentoFaturamentoAuditoriaInterna(FaturarAtendimentoInputDto input);

        Task<PagedResultDto<FaturarAtendimentoIndexDto>> ListarAtendimentoFaturamentoAuditoriaExterna(FaturarAtendimentoInputDto input);

        Task<PagedResultDto<FaturarAtendimentoContaMedicaIndexDto>> ListarContaMedica(ListarContaMedicaInputDto input);

        Task<PagedResultDto<FaturarItemAtendimentoIndexDto>> ListarFaturamentoItem(ListarFaturamentoItemInputDto input);
    }
}