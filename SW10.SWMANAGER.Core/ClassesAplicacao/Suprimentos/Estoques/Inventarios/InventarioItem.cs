using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios
{
    [Table("EstInventarioItem")]
    public class InventarioItem : CamposPadraoCRUD
    {
        public long InventarioId { get; set; }
        [ForeignKey("InventarioId")]
        public Inventario Inventario { get; set; }

        public long ProdutoId { get; set; }
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        public long? LoteValidadeId { get; set; }
        [ForeignKey("LoteValidadeId")]
        public LoteValidade LoteValidade { get; set; }

        public long StatusInventarioItemId { get; set; }
        [ForeignKey("StatusInventarioItemId")]
        public StatusInventarioItem StatusInventarioItem { get; set; }

        public string NumeroSerie { get; set; }

        public decimal? QuantidadeEstoque { get; set; }
        public decimal? QuantidadeContagem { get; set; }
        public decimal? QuantidadeContagem2 { get; set; }
        public decimal? QuantidadeContagem3 { get; set; }

        public long? UsuarioContegemId { get; set; }
        public long? UsuarioContegemId2 { get; set; }
        public long? UsuarioContegemId3 { get; set; }

        [ForeignKey("UsuarioContegemId")]
        public User UsuarioContegem { get; set; }

        [ForeignKey("UsuarioContegemId2")]
        public User UsuarioContegem2 { get; set; }

        [ForeignKey("UsuarioContegemId3")]
        public User UsuarioContegem3 { get; set; }
        [Index("Est_Idx_DataContagem")]
        public DateTime? DataContagem { get; set; }
        [Index("Est_Idx_DataContagem2")]
        public DateTime? DataContagem2 { get; set; }
        [Index("Est_Idx_DataContagem3")]
        public DateTime? DataContagem3 { get; set; }
        [Index("Est_Idx_TemDivergencia")]
        public bool TemDivergencia { get; set; }
        
        public long? UsuarioDivergenciaId { get; set; }
        [ForeignKey("UsuarioDivergenciaId")]
        public User UsuarioDivergencia { get; set; }

        public bool DivergenciaResolvida { get; set; }
        [Index("Est_Idx_DataDivergenciaResolvida")]
        public DateTime? DataDivergenciaResolvida { get; set; }

        
    }
}
