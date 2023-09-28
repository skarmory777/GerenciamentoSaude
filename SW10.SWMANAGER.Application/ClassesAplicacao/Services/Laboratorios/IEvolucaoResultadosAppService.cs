using Abp.Application.Services;
using Abp.Application.Services.Dto;

using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios.Input;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios
{
    using SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios.Dto;

    public interface IEvolucaoResultadosAppService : IApplicationService
    {
        Task<PagedResultDto<IndexPacientesOutputDto>> ListarNaoConferido(ListarPacientesInput input);
        Task<PagedResultDto<ResultadoLaudoDto>> Obter(EvolucaoResultadoInput input);

        Task<ListaEvolucaoResultado> ListaEvolucaoResultado(EvolucaoResultadoInput input);

        Task<PagedResultDto<EvolucaoResultadoDto>> ListaEvolucaoResultadoPorColeta(EvolucaoResultadoInput input);
    }
}
