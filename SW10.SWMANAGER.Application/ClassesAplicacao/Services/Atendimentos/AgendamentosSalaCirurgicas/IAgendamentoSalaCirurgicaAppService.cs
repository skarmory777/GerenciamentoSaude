using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentosSalaCirurgicas
{
    public interface IAgendamentoSalaCirurgicaAppService : IApplicationService
    {
        Task<ICollection<AgendamentoCirurgico>> ListarPorSala(long? salaId, long? tipoCirurgiaId, DateTime start, DateTime end, long? empresaId, long? medicoId);
        Task<ICollection<AgendamentoCirurgico>> ListarPorData(DateTime start, DateTime end);
        Task CriarOuEditar(AgendamentoCirurgicoDto input);
        Task<AgendamentoCirurgicoDto> Obter(long id);
        Task<AgendamentoCirurgicoDto> ObterCirurgico(long id);
        Task<DefaultReturn<AgendamentoCirurgicoDto>> RecalcularQuantidadeHorarios(string cirurgiasJson, long agendamentoId, long? disponibilidadeId, DateTime dataAgendamento, DateTime horaAgendamento, long? salaId);
        Task<RelatorioAgendamentoDto> ObterAgendamentosDia(DateTime? inicio, DateTime? fim, long? tipoCirurgiaId, long? medicoId);
        Task<PagedResultDto<ListagemAgendamentoDto>> obterListagem(ListarAgendamentoInput input);
        Task<PagedResultDto<ProcedimentoDescontoDto>> ObterProcedimentos(AgendamentoProcedimentoInput input);
        Task<DefaultReturn<AgendamentoCirurgicoDto>> AtualizarDesconto(long agendamentoId, decimal? valorDesconto);// List<ProcedimentoDescontoDto> procedimentos);
    }
}
