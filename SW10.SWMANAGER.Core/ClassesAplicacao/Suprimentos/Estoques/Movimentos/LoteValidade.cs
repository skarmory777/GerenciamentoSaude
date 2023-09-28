using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("LoteValidade")]
    public class LoteValidade : CamposPadraoCRUD
    {
        public string Lote { get; set; }

        [Index("Est_Idx_Validade")]
        public DateTime Validade { get; set; }

        public long ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("EstoqueLaboratorio"), Column("EstEstoqueLaboratorioId")]
        public long? EstLaboratorioId { get; set; }
        public EstoqueLaboratorio EstoqueLaboratorio { get; set; }

    }
}
