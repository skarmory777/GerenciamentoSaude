using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto
{
    [AutoMap(typeof(ProdutoUnidade))]
    public class ProdutoUnidadeDto : CamposPadraoCRUDDto
    {

        public bool IsAtivo { get; set; }
       
        public bool IsPrescricao { get; set; }
        
        public  ProdutoDto Produto { get; set; }
        public long ProdutoId { get; set; }

        public  UnidadeDto Unidade { get; set; }
        public long UnidadeId { get; set; }
      
        public  UnidadeTipoDto Tipo { get; set; }
        public long UnidadeTipoId { get; set; }

    }
}
