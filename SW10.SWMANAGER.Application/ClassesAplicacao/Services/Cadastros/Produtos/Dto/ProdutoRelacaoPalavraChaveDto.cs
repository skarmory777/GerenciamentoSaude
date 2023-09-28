using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    [AutoMap(typeof(ProdutoRelacaoPalavraChave))]
    public class ProdutoRelacaoPalavraChaveDto : CamposPadraoCRUDDto
    {
        public long ProdutoId { get; set; }
        public long ProdutoPalavraChaveId { get; set; }

        //public virtual ProdutoDto Produto { get; set; }
        public virtual ProdutoPalavraChaveDto ProdutoPalavraChave { get; set; }
    }
}
