using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos
{
    [AutoMap(typeof(ProdutoRelacaoPalavraChaveDto))]
    public class CriarOuEditarProdutoRelacaoPalavraChaveModalViewModel : ProdutoRelacaoPalavraChaveDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList PalavrasChaves { get; set; }

        public CriarOuEditarProdutoRelacaoPalavraChaveModalViewModel(ProdutoRelacaoPalavraChaveDto output)
        {
            output.MapTo(this);
        }
    }
}