using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    [Table("AssFormulaFaturamento")]
    public class FormulaFaturamento : CamposPadraoCRUD, IDescricao
    {
        [ForeignKey("FaturamentoItem"), Column("FatItemId")]
        public long? FaturamentoItemId { get; set; }

        public FaturamentoItem FaturamentoItem { get; set; }

        [ForeignKey("PrescricaoItem"), Column("AssPrescricaoItemId")]
        public long PrescricaoItemId { get; set; }

        public PrescricaoItem PrescricaoItem { get; set; }

        [ForeignKey("Material"), Column("LabMaterialId")]
        public long? MaterialId { get; set; }

        public Material Material { get; set; }

    }
}
