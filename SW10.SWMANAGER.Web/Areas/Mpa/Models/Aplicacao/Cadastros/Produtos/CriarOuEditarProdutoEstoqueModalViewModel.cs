using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos
{
    [AutoMap(typeof(ProdutoEstoqueDto))]
    public class CriarOuEditarProdutoEstoqueModalViewModel : ProdutoEstoqueDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Estoques { get; set; }
        public SelectList Localizacoes { get; set; }

        public CriarOuEditarProdutoEstoqueModalViewModel(ProdutoEstoqueDto output)
        {
            output.MapTo(this);
        }
    }
}