namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoCirurgias
{
    public class FiltroAgendamentoOrcamento
    {
        public long? AgendamentoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? PlanoId { get; set; }
        public long? DisponibilidadeId { get; set; }
        public string ListItemFaturamento { get; set; }
        public string ListItemMateriais { get; set; }
        public long? PacienteId { get; set; }
        public string DataHoraAgendamento { get; set; }
        public string PacienteReservante { get; set; }

    }
}