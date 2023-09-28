using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Medicos
{
    [AutoMap(typeof(MedicoEspecialidadeDto))]
    public class CriarOuEditarMedicoEspecialidadeModalViewModel : MedicoEspecialidadeDto
    {
        //public virtual MedicoDto Medico { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        //public SelectList Especialidades { get; set; }

        public CriarOuEditarMedicoEspecialidadeModalViewModel(MedicoEspecialidadeDto output)
        {
            output.MapTo(this);
        }
    }
}