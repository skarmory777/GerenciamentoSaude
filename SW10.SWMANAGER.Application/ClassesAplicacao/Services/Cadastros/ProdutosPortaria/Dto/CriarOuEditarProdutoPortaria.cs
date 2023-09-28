using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPortaria;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Dto
{
    [AutoMap(typeof(ProdutoPortaria))]
    public class CriarOuEditarProdutoPortaria : CamposPadraoCRUDDto
    {
    }
}