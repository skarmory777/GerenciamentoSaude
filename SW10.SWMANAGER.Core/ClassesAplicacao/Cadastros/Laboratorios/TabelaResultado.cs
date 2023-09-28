using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabTabelaResultado")]
    public class TabelaResultado : CamposPadraoCRUD
    {
        public bool IsAlterado { get; set; }
        public long? TabelaId { get; set; }

        [ForeignKey("TabelaId")]
        public Tabela Tabela { get; set; }


        //public IList<TabelaResultadoItem> LabTabelaResultadoItens { get; set; }
        //public IList<Informacao> LabInformacoes { get; set; }
    }
}