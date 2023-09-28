using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    [AutoMap(typeof(ProdutoRelacaoAcaoTerapeutica))]
    public class ProdutoRelacaoAcaoTerapeuticaDto : CamposPadraoCRUDDto
    {
        public long ProdutoId { get; set; }
        public long ProdutoAcaoTerapeuticaId { get; set; }

        //public virtual ProdutoDto Produto { get; set; }
        public virtual ProdutoAcaoTerapeuticaDto ProdutoAcaoTerapeutica { get; set; }
    }
}
