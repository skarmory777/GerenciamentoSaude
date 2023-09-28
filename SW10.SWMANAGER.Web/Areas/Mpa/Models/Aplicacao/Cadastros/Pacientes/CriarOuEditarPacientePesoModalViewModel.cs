using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes
{
    [AutoMapFrom(typeof(PacientePesoDto))]
    public class CriarOuEditarPacientePesoModalViewModel : PacientePesoDto
    {
        public SelectList PacientePesos { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public CriarOuEditarPacientePesoModalViewModel(PacientePesoDto output)
        {
            output.MapTo(this);
        }
    }
}