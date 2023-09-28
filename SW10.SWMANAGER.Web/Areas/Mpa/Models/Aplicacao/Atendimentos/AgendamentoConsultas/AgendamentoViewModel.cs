using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    public class AgendamentoViewModel
    {
        public Especialidade Especialidade { get; set; }
        public string Horarios { get; set; }
        public int IntervaloMinutos { get; set; }
        public string DiasSemana { get; set; }
        public TipoCirurgiaDto TipoCirurgia { get; set; }
    }
}