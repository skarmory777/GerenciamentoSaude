using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Dto;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos
{
    [AutoMap(typeof(ProdutoEmpresaDto))]
    public class CriarOuEditarProdutoEmpresaModalViewModel : ProdutoEmpresaDto
    {
        public UserEditDto UpdateUser { get; set; }

        public bool IsEditMode
        {
            get { return this.Id > 0; }
        }

        public SelectList Empresas { get; set; }

        public CriarOuEditarProdutoEmpresaModalViewModel(ProdutoEmpresaDto output)
        {
            output.MapTo(this);
        }
    }
}