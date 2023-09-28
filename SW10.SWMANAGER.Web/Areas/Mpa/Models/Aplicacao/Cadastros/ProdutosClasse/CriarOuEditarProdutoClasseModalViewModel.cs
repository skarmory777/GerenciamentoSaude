using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosClasse
{
    [AutoMapFrom(typeof(CriarOuEditarGrupoClasse))]
    public class CriarOuEditarProdutoClasseModalViewModel : CriarOuEditarGrupoClasse
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList ProdutosClasse { get; set; }

        public CriarOuEditarProdutoClasseModalViewModel(CriarOuEditarGrupoClasse output)
        {
            output.MapTo(this);
        }
    }
}