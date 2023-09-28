using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    public interface IViewModelAppService : IApplicationService
    {
        Task<PagedResultDto<VWFaturamentoAbertoSeisMesesDto>> ListarFaturamentoAbertoSeisMeses(ListarInput input);
        Task<ListResultDto<VWFaturamentoAbertoSeisMesesDto>> ListarTodosFaturamentoAbertoSeisMeses();
        Task<PagedResultDto<VWEmpresaDto>> ListarEmpresas(ListarInput input);
    }
}
