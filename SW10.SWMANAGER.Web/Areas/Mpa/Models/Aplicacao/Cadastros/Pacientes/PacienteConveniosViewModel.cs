using System;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes
{
	public class PacienteConveniosViewModel
	{
		public ICollection<PacienteConvenioDto> PacienteConvenios { get; set; }

		public ICollection<ConvenioDto> Convenios { get; set; }
	}
}