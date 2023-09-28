using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.SalasCirurgicas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas;

using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoCirurgias
{
    public class ListarSalasCirurgicasViewModel
    {
        public long? SalaCirurgicaId { get; set; }
        public SalaCirurgicaDto SalaCirurgica { get; set; }
        public string DiasSemana { get; set; }
        public List<AgendamentoViewModel> Agendamentos { get; set; }

    }
}