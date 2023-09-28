using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Tarefas.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public interface ITarefaAppService : IApplicationService
    {

        Task<PagedResultDto<TarefaDto>> ListarTodos();

        Task<PagedResultDto<TarefaDto>> ListarFiltrando(ListarTarefasInput input);

        Task<PagedResultDto<TarefaDto>> ListarTarefasExecutando();

        Task<PagedResultDto<TarefaDto>> ListarTarefasHorasRealizadas(ListarInput input);

        Task CriarOuEditar(TarefaDto input);

        Task Excluir(TarefaDto input);

        Task<TarefaDto> Obter(long id);

        Task<bool> IniciarContagemIntervalo(long tarefaId);

        Task<bool> PararContagemIntervalo(long tarefaId);

        Task<string> CalcularTempoDecorrido(long tarefaId);

        //      Task<PagedResultDto<TarefaIntervaloDto>> ListarIntervalosPorUsuarioPeriodo (ListarInput input);

        Task<ProducaoResponsavel> CalcularProducaoPorReponsavel();

        Task<PagedResultDto<TarefaIntervaloDto>> ListarIntervalosExecutando();

        Task<PagedResultDto<TarefaIntervaloDto>> ListarIntervalosHorasRealizadas(ListarInput input);
    }
}
