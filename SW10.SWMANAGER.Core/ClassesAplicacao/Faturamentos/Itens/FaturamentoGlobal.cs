using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens
{
    [Table("FatGlobal")]
    public class FaturamentoGlobal : CamposPadraoCRUD
    {
        public override string Codigo { get; set; }

        public override string Descricao { get; set; }
    }

}


