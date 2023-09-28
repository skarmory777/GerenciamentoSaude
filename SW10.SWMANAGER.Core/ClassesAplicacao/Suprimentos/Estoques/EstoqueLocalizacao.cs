using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_EstoqueLocalizacao")]
    public class EstoqueLocalizacao : CamposPadraoCRUD
    {
        /// <summary>
        /// Estoque
        /// </summary>
        public long EstoqueId { get; set; }
        [ForeignKey("EstoqueId")]
        public Estoque Estoque { get; set; }
    }
}
