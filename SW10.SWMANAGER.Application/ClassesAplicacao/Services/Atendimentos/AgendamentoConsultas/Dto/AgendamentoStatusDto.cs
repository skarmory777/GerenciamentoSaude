using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class AgendamentoStatusDto : CamposPadraoCRUDDto
    {
        public static AgendamentoStatusDto Mapear(AgendamentoStatus status)
        {
            AgendamentoStatusDto agendamentoStatusDto = new AgendamentoStatusDto();

            agendamentoStatusDto.Id = status.Id;
            agendamentoStatusDto.Codigo = status.Codigo;
            agendamentoStatusDto.Descricao = status.Descricao;

            return agendamentoStatusDto;
        }
    }
}
