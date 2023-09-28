using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosTiposUnidade
{
    [AutoMap(typeof(ProdutoTipoUnidadeDto))]
    public class CriarOuEditarProdutoTipoUnidadeModalViewModel : ProdutoTipoUnidadeDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList ProdutosTiposUnidade { get; set; }

        public CriarOuEditarProdutoTipoUnidadeModalViewModel(ProdutoTipoUnidadeDto output)
        {
            output.MapTo(this);
        }
    }
}