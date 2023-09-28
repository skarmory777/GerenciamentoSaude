using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios
{
    [Table("EstInventario")]
    public class Inventario : CamposPadraoCRUD
    {
        [Index("Est_Idx_DataInventario")]
        public DateTime DataInventario { get; set; }
        public long TipoInventarioId { get; set; }
        [ForeignKey("TipoInventarioId")]
        public TipoInventario TipoInventario { get; set; }

        public long StatusInventarioId { get; set; }
        [ForeignKey("StatusInventarioId")]
        public StatusInventario StatusInventario { get; set; }

        public long EstoqueId { get; set; }
        [ForeignKey("EstoqueId")]
        public Estoque Estoque { get; set; }

        public long? GrupoId { get; set; }
        [ForeignKey("GrupoId")]
        public Grupo Grupo { get; set; }

        public long? GrupoClasseId { get; set; }
        [ForeignKey("GrupoClasseId")]
        public GrupoClasse GrupoClasse { get; set; }

        public long? GrupoSubClasseId { get; set; }
        [ForeignKey("GrupoSubClasseId")]
        public GrupoSubClasse GrupoSubClasse { get; set; }

        public long? ProdutoId { get; set; }
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        public List<InventarioItem> Itens { get; set; }

    }
}
