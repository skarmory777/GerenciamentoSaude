using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes
{
	public class PacientePlanosViewModel
	{
		public ICollection<PacientePlanoDto> PacientePlanos { get; set; }

		public ICollection<PlanoDto> Planos { get; set; }
	}
}