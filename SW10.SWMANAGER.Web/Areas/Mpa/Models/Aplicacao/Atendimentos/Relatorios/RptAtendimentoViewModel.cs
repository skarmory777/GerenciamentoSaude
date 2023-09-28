using System;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Relatorios
{
    public class RptAtendimentoViewModel
    {
        public long? EmpresaId { get; set; }
        public long? AtendimentoId { get; set; }
        public long? PacienteId { get; set; }
        public long? MedicoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? EspecialidadeId { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        /// <summary>
        /// 0 - Todos, 1 - Ambulatório/Emergência, 3 - Internação
        /// </summary>
        public int TipoAtendimento { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TipoRel { get; set; }
        /// <summary>
        /// 1 - Data do atendimento, 2 - Data da alta
        /// </summary>
        public int TipoPeriodo { get; set; }
        public string Filtrado { get; set; }
    }
}