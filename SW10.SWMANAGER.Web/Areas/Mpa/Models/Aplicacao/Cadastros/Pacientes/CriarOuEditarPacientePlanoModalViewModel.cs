using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes
{
	[AutoMapFrom(typeof(PacientePlanoDto))]
	public class CriarOuEditarPacientePlanoModalViewModel : PacientePlanoDto
	{
		public virtual CriarOuEditarPaciente Paciente { get; set; }

		public virtual CriarOuEditarPlano Plano { get; set; }

		public SelectList Planos { get; set; }

		public bool IsEditMode
		{
			get { return this.Id > 0; }
		}

		public CriarOuEditarPacientePlanoModalViewModel(PacientePlanoDto output)
		{
			output.MapTo(this);
		}
	}
}