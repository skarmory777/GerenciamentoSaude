using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    [AutoMap(typeof(ProdutoRelacaoEstoque))]
    public class ProdutoRelacaoEstoqueDto : CamposPadraoCRUDDto
    {
        public long ProdutoId { get; set; }
        public long ProdutoEstoqueId { get; set; }
        public long ProdutoLocalizacaoId { get; set; }

        public long EstoqueMinimo { get; set; }
        public long EstoqueMaximo { get; set; }
        public long PontoPedido { get; set; }

        public virtual ProdutoLocalizacaoDto ProdutoLocalizacao { get; set; }
        public virtual ProdutoEstoqueDto ProdutoEstoque { get; set; }
    }
}
