using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    [Table("AssFormulaEstoqueKit")]
    public class FormulaEstoqueKit : CamposPadraoCRUD
    {
        [ForeignKey("PrescricaoItem"), Column("AssPrescricaoItemId")]
        public long PrescricaoItemId { get; set; }

        public PrescricaoItem PrescricaoItem { get; set; }

        [ForeignKey("EstoqueKit"), Column("AssEstoqueKitId")]
        public long EstoqueKitId { get; set; }

        public EstoqueKit EstoqueKit { get; set; }



    }
}
