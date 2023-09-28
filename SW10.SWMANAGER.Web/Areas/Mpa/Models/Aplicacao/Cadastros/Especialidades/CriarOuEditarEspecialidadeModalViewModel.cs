using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Especialidades
{
    [AutoMap(typeof(EspecialidadeDto))]
    public class CriarOuEditarEspecialidadeModalViewModel : EspecialidadeDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }
        public CriarOuEditarEspecialidadeModalViewModel(EspecialidadeDto output)
        {
            output.MapTo(this);
        }
    }
}