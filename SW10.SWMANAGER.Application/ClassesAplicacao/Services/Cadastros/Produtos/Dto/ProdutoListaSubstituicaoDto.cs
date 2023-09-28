using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    [AutoMap(typeof(ProdutoListaSubstituicao))]
    public class ProdutoListaSubstituicaoDto : CamposPadraoCRUDDto
    {
        public long ProdutoId { get; set; }
        public long ProdutoSubstituicaoId { get; set; }

        [ForeignKey("ProdutoId")]
        public virtual ProdutoDto Produto { get; set; }
    }
}
