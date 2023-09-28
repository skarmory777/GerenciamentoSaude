using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Dashboard.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Dashboards
{
    public interface IDashboardAppService : IApplicationService
    {
        Task<ListResultDto<VWConsultaFaturamentoAbertoDto>> ListarFaturamentoAberto();
        Task<ListResultDto<VWConsultaFaturamentoEntregaDto>> ListarFaturamentoEntrega();
        Task<ListResultDto<VWConsultaFaturamentoRecebimentoDto>> ListarFaturamentoRecebimento();
        Task<PagedResultDto<VWFaturamentoAbertoSeisMesesDto>> ListarFaturamentoAbertoSeisMeses(ListarFaturamentoAbertoSeisMesesInput input);
        Task<PagedResultDto<VWEmpresaDto>> ListarEmpresas(ListarInput input);
        Task<ListResultDto<VWEmpresaDto>> ListarTodasEmpresas();
        //Task<VWTesteDto> Obter(long id);
    }
}