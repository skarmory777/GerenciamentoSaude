using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosPalavrasChave
{
    [AutoMap(typeof(ProdutoPalavraChaveDto))]
    public class CriarOuEditarProdutoPalavraChaveModalViewModel : ProdutoPalavraChaveDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList ProdutosPalavrasChave { get; set; }

        public CriarOuEditarProdutoPalavraChaveModalViewModel(ProdutoPalavraChaveDto output)
        {
            output.MapTo(this);
        }
    }
}