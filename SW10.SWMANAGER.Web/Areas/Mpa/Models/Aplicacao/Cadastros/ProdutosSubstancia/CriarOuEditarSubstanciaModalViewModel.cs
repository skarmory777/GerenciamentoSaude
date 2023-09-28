using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosSubstancia
{
    [AutoMapFrom(typeof(CriarOuEditarProdutoSubstancia))]
    public class CriarOuEditarProdutoSubstanciaModalViewModel : CriarOuEditarProdutoSubstancia
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Substancias { get; set; }

        public CriarOuEditarProdutoSubstanciaModalViewModel(CriarOuEditarProdutoSubstancia output)
        {
            output.MapTo(this);
        }
    }
}