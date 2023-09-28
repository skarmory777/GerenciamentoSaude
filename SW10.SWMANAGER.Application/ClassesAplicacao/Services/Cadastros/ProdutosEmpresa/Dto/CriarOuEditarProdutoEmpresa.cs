using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto
{
    [AutoMap(typeof(ProdutoEmpresa))]
    public class CriarOuEditarProdutoEmpresa : CamposPadraoCRUDDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long ProdutoId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long EmpresaId { get; set; }

    }
}
