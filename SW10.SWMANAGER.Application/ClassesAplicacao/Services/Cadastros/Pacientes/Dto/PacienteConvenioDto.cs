using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using System;
using Abp.Application.Services.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto
{
	[AutoMap(typeof(PacienteConvenio))]
	public class PacienteConvenioDto : EntityDto<long>
	{
		public long ConvenioId { get; set; }

		public long PacienteId { get; set; }

		public string Matricula { get; set; }

		public DateTime? DataInicioContrato { get; set; }

		public DateTime? DataTerminoContrato { get; set; }

		public bool IsAtivo { get; set; }

		//[ForeignKey("ConvenioId")]
		//public virtual ConvenioDto Convenio { get; set; }
		//[ForeignKey("PacienteId")]
		//public virtual PacienteDto Paciente { get; set; }
	}
}
