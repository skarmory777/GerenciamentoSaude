using Abp.AutoMapper;
using SW10.SWMANAGER.Authorization.Users.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Produtos
{
    [AutoMap(typeof(ProdutoSaldoDto))]
    public class CriarOuEditarProdutoSaldoViewModel : ProdutoSaldoDto
    {
        public UserEditDto UpdateUser { get; set; }
        public CriarOuEditarProdutoSaldoViewModel(ProdutoSaldoDto output)
        {
            output.MapTo(this);
        }
    }
}