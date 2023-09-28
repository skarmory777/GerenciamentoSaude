using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos
{
    [AutoMap(typeof(ProdutoUnidadeDto))]
    public class CriarOuEditarProdutoUnidadeModalViewModel : ProdutoUnidadeDto
    {
        public UserEditDto UpdateUser { get; set; }

        public UnidadeDto UnidadeReferencial { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Unidades { get; set; }

        public SelectList TiposUnidades { get; set; }

        public CriarOuEditarProdutoUnidadeModalViewModel(ProdutoUnidadeDto output)
        {
            output.MapTo(this);
        }
    }
}