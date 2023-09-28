using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Laboratorios
{
    public class ResultadoExameViewModel
    {
        public string Paciente { get; set; }
        public long ColetaId { get; set; }
        public string DataColeta { get; set; }
        public string Codigo { get; set; }
        public bool IsUrgente { get; set; }
        public string Medico { get; set; }
        public string Leito { get; set; }
        public string Tecnico { get; set; }
        public bool IsRN { get; set; }
        public string Exame { get; set; }
        public long? ExameId { get; set; }
        public string ItensJson { get; set; }
        public long? ResultadoExameId { get; set; }
        public long? RegistroArquivoId { get; set; }
        public long? PacienteId { get; set; }

        public List<ExameStatusDto> ExamesStatus { get; set; }
    }
}