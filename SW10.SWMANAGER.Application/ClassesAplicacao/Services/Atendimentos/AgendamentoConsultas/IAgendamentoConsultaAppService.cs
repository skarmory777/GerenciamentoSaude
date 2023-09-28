using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas
{
    public interface IAgendamentoConsultaAppService : IApplicationService
    {
        Task<PagedResultDto<AgendamentoConsultaDto>> Listar(ListarAgendamentoConsultasInput input);

        Task CriarOuEditar(CriarOuEditarAgendamentoConsulta input);

        Task Excluir(long id);

        Task<CriarOuEditarAgendamentoConsulta> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarAgendamentoConsultasInput input);

        Task<ICollection<AgendamentoConsulta>> ListarPorMedico(long? medicoId, long? medicoEspecialidadeId, DateTime start, DateTime end, EnumTipoAgendamento tipoAgendamento);

        Task<ListResultDto<AgendamentoConsultaDto>> ListarTodos();

        Task<bool> ChecarDisponibilidade(long medicoDisponibiliadeId, DateTime hora, long id = 0);

        Task<ICollection<AgendamentoConsulta>> ListarPorData(DateTime start, DateTime end);

        Task<ICollection<AgendamentoConsulta>> ListarPorDataMedicoEspecialidade(DateTime start, DateTime end, long medicoId, long medicoEspecialidadeId);

        Task<DefaultReturn<string>> AlterarAgendamento(long id, int dias, int horas, int minutos);

        Task<DefaultReturn<FaturamentoContaItemInsertDto>> InserirItensFaturamentoAtendimentoExistente(long agendamentoId, long atendimentoId);

        Task<DefaultReturn<string>> ConfirmarAtendimento(long id);

        Task<ICollection<AgendamentoConsulta>> ListarPorDataSalaTipoCirurgia(DateTime start, DateTime end, long salaId, long tipoCirurgiaId);


        byte[] RetornaArquivoAgendamentoCirurgico(DateTime dataInicial, DateTime dataFinal);

    }
}
