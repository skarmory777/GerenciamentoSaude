using System;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto
{
	[AutoMap(typeof(PacientePlano))]
    public class PacientePlanoDto : CamposPadraoCRUDDto
    {
        public bool IsAtivo { get; set; }

        public string Matricula { get; set; }

        public DateTime? DataInicioContrato { get; set; }

        public DateTime? DataTerminoContrato { get; set; }

        public long PacienteId { get; set; }

        public long PlanoId { get; set; }

        //[ForeignKey("PlanoId")]
        //public virtual PlanoDto Plano { get; set; }

        //[ForeignKey("PacienteId")]
        //public virtual PacienteDto Paciente { get; set; }
    }
}
