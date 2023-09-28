using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas
{
    [Table("FatContaStatus")]
    public class FaturamentoContaStatus : CamposPadraoCRUD
    {
        public const long Inicial = 1;
        public const long Conferido = 2;
        public const long Entregue = 3;
        public const long Lote = 4;
        public const long Pendente = 5;
        public const int AuditoriaInterna = 6;
        public const int AuditoriaExterna = 7;

        [StringLength(10)]
        public override string Codigo { get; set; }

        [StringLength(255)]
        public override string Descricao { get; set; }

        [StringLength(7)]
        public string Cor { get; set; }
    }
}
