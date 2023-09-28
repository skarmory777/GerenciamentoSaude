using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras
{
    [Table("CmpCotacaoItem")]
    public class CompraCotacaoItem : CamposPadraoCRUD
    {
        [ForeignKey("CompraCotacao"), Column("CmpCompraCotacaoId")]
        public long CompraCotacaoId { get; set; }
        public CompraCotacao CompraCotacao { get; set; }

        [ForeignKey("RequisicaoItem"), Column("CmpRequisicaoItemId")]
        public long RequisicaoItemId { get; set; }
        public CompraRequisicaoItem RequisicaoItem { get; set; }

        public decimal ValorUnitario { get; set; }

        public decimal Quantidade { get; set; }

        public long? LaboratorioId { get; set; }
        [ForeignKey("LaboratorioId")]
        public EstoqueLaboratorio EstoqueLaboratorio { get; set; }

        public bool OpcaoComprador { get; set; }

        public int? PrazoEntregaEmDias { get; set; }
    }
}