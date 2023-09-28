using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes
{
	[AutoMapFrom(typeof(PacienteConvenioDto))]
	public class CriarOuEditarPacienteConvenioModalViewModel : PacienteConvenioDto
	{
		public virtual CriarOuEditarPaciente Paciente { get; set; }

		public virtual CriarOuEditarConvenio Convenio { get; set; }

		public SelectList Convenios { get; set; }

		public bool IsEditMode
		{
			get { return this.Id > 0; }
		}

		public CriarOuEditarPacienteConvenioModalViewModel(PacienteConvenioDto output)
		{
			output.MapTo(this);
		}
	}
}