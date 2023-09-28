using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    public class AtendimentoInternacaoIndexDto : CamposPadraoCRUDDto
    {
        public string Unidade { get; set; }
        public string CodigoAtendimento { get; set; }
        public string CodigoPaciente { get; set; }
        public string Paciente { get; set; }
        public DateTime? DataIternacao { get; set; }
        public DateTime? DataAlta { get; set; }
        public string Convenio { get; set; }
        public string Medico { get; set; }
        public string TipoLeito { get; set; }
        public string Leito { get; set; }
        public string Empresa { get; set; }
    }
}
