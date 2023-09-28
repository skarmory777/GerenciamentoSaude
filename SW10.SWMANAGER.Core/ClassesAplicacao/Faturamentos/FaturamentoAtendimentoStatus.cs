using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos
{
    [Table("FatAtendimentoStatus")]
    public class FaturamentoAtendimentoStatus: CamposPadraoCRUD
    {
        public static long Pendente = 1;
        public static long Parcial = 2;
        public static long Glosado = 3;
        public static long Finalizado = 4;
        public string Cor { get; set; }
    }
}