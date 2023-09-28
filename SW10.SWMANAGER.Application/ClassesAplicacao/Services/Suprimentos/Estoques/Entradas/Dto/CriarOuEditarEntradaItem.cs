using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Entradas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Entradas.Dto
{
    [AutoMap(typeof(EntradaItem))]
    public class CriarOuEditarEntradaItem : CamposPadraoCRUDDto
    {
        public long EntradaId { get; set; }
        public long ProdutoId { get; set; }
        public long Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }
        // public virtual Entrada Entrada { get; set; }
        public virtual ProdutoDto Produto { get; set; }
    }
}
