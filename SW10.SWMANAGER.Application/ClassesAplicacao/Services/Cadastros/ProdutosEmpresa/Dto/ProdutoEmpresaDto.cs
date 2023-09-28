using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEmpresa.Dto
{
    [AutoMap(typeof(ProdutoEmpresa))]
    public class ProdutoEmpresaDto : CamposPadraoCRUDDto
    {
        public ProdutoDto Produto { get; set; }
        public long ProdutoId { get; set; }

        public EmpresaDto Empresa { get; set; }
        public long EmpresaId { get; set; }
    }
}
