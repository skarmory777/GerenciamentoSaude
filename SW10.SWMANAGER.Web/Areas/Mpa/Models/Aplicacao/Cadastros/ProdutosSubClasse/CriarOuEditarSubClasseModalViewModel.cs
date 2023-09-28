using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosSubClasse
{
    [AutoMapFrom(typeof(CriarOuEditarGrupoSubClasse))]
    public class CriarOuEditarGrupoSubClasseModalViewModel : CriarOuEditarGrupoSubClasse
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList SubClasses { get; set; }

        public CriarOuEditarGrupoSubClasseModalViewModel(CriarOuEditarGrupoSubClasse output)
        {
            output.MapTo(this);
        }
    }
}