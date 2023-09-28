using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Grupos
{
    [AutoMap(typeof(GrupoDto))]
    public class CriarOuEditarGrupoModalViewModel : GrupoDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Grupo { get; set; }

        public CriarOuEditarGrupoModalViewModel(GrupoDto output)
        {
            output.MapTo(this);
        }
    }
}