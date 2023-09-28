using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto
{
    public class EstoqueEtiquetaDto : CamposPadraoCRUDDto
    {
        public long? ProdutoId { get; set; }
        public long? LoteValidadeId { get; set; }

        public ProdutoDto Produto { get; set; }

        public LoteValidadeDto LoteValidade { get; set; }

        public long? UnidadeProdutoId { get; set; }

        public UnidadeDto UnidadeProduto { get; set; }

        public long? EstoqueKitId { get; set; }

        public EstoqueKitDto EstoqueKit { get; set; }

        public EnumTipoEtiquetaCodigoBarra TipoEtiquetaCodigoBarra { get; set; }
    }
}
