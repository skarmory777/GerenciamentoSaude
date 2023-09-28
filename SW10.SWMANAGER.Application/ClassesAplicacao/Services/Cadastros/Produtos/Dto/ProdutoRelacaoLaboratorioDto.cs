using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    [AutoMap(typeof(ProdutoRelacaoLaboratorio))]
    public class ProdutoRelacaoLaboratorioDto : CamposPadraoCRUDDto
    {
        public long ProdutoId { get; set; }
        public long ProdutoLaboratorioId { get; set; }

        public virtual ProdutoLaboratorioDto ProdutoLaboratorio { get; set; }

        // public virtual ProdutoDto Produto { get; set; }
    }
}
