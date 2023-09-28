using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    /// <summary>
    /// Entidade de relacionamento entre Estoque e um Grupo de Produtos.
    /// </summary>
    [Table("Est_EstoqueGrupo")]
    public class EstoqueGrupo : CamposPadraoCRUD
    {
        /// <summary>
        /// Estoque.
        /// </summary>
        [ForeignKey("EstoqueId")]
        public Estoque Estoque { get; set; }
        public long? EstoqueId { get; set; }

        /// <summary>
        /// Grupo de Produtos.
        /// </summary>
        [ForeignKey("GrupoId")]
        public Grupo Grupo { get; set; }
        public long? GrupoId { get; set; }

        public bool? IsTodosItens { get; set; }

    }
}
