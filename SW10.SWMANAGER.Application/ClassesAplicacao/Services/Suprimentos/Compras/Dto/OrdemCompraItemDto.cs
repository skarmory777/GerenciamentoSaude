using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    [AutoMap(typeof(OrdemCompraItem))]
    public class OrdemCompraItemDto : CamposPadraoCRUDDto
    {
        public long OrdemCompraId { get; set; }
        public long? RequisicaoItemId { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorTotal { get; set; }
        public long UnidadeId { get; set; }
        public UnidadeDto Unidade { get; set; }
        public long ProdutoId { get; set; }
        public ProdutoDto Produto { get; set; }
        /// <summary>
        /// Id artificial para manipulacao dinamica
        /// </summary>
        public long? IdGrid { get; set; }
    }
}
