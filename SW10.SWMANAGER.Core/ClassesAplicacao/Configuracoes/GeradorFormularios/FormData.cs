using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios
{
    [Table("SisFormData")]
    public class FormData : CamposPadraoCRUD
    {
        //public string Valor { get; set; }

        //[Column("ColConfigId")]
        //public long ColConfigId { get; set; }

        //[ForeignKey("ColConfigId")]
        //public virtual ColConfig Coluna { get; set; }

        //[Column("FormRespostaId")]
        //public long FormRespostaId { get; set; }

        //[ForeignKey("FormRespostaId")]
        //public virtual FormResposta Resposta { get; set; }

        //public virtual bool IsChecked
        //{
        //    get
        //    {
        //        var result = false;
        //        if (Coluna != null && Coluna.MultiOption != null)
        //        {
        //            var selected = Coluna.MultiOption.Where(m => m.Opcao.ToUpper().Equals(Valor.ToUpper())).ToList();
        //            result = selected.Count() > 0;
        //        }
        //        return result;
        //    }
        //}
        public string Valor { get; set; }

        [Column("ColConfigId")]
        public long ColConfigId { get; set; }

        [ForeignKey("ColConfigId")]
        public ColConfig Coluna { get; set; }

        [Column("FormRespostaId")]
        public long FormRespostaId { get; set; }

        //[ForeignKey("FormRespostaId")]
        //public FormResposta Resposta { get; set; }

        [Column("Sequencial")]
        public int? Sequencial { get; set; }

    }
}