using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    [AutoMap(typeof(ProdutoRelacaoPortaria))]
    public class ProdutoRelacaoPortariaDto : CamposPadraoCRUDDto
    {
        public long ProdutoId { get; set; }
        public long ProdutoPortariaId { get; set; }

        //public virtual ProdutoDto Produto { get; set; }
        public virtual ProdutoPortariaDto ProdutoPortaria { get; set; }
    }
}
