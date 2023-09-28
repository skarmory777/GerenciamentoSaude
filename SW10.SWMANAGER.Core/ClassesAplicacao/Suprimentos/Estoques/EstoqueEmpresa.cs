using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    /// <summary>
    /// Entidade de relacionamento entre Estoques e Empresas.
    /// </summary>
    [Table("Est_EstoqueEmpresa")]
    public class EstoqueEmpresa : CamposPadraoCRUD
    {
        /// <summary>
        /// Estoque.
        /// </summary>
        [ForeignKey("EstoqueId")]
        public Estoque Estoque { get; set; }
        public long? EstoqueId { get; set; }

        /// <summary>
        /// Empresa.
        /// </summary>
        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
        public long? EmpresaId { get; set; }

        /// <summary>
        /// ProcessoCota -> 1 - CotaTotal / 2 - CompletarCota
        /// </summary>
        public long? ProcessoCota { get; set; }

        public bool? IsCotaLimitarTransferencia { get; set; }

        public bool IsPrincipal { get; set; }

    }
}
