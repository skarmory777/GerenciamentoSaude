using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus
{
    [Table("AssPrescricaoItemStatus")]
    public class PrescricaoItemStatus : CamposPadraoCRUD
    {
        public string Cor { get; set; }
    }
}
