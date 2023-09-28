using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_ProdutoEmpresa")]
    public class ProdutoEmpresa : CamposPadraoCRUD
    {
        /// <Summary>
        /// Produto
        /// </Summary>
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
        public long ProdutoId { get; set; }

        /// <summary>
        /// Empresa
        /// </summary>
        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
        public long EmpresaId { get; set; }

    }
}
