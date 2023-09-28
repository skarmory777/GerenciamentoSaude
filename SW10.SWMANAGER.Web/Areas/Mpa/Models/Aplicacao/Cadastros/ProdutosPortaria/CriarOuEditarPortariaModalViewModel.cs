using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosPortaria
{
    [AutoMap(typeof(CriarOuEditarProdutoPortaria))]
    public class CriarOuEditarProdutoPortariaModalViewModel : CriarOuEditarProdutoPortaria
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList ProdutosPortaria { get; set; }

        public CriarOuEditarProdutoPortariaModalViewModel(CriarOuEditarProdutoPortaria output)
        {
            output.MapTo(this);
        }
    }
}

