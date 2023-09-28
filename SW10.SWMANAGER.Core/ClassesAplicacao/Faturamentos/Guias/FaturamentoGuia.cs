using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos
{
    [Table("FatGuia")]
    public class FaturamentoGuia : CamposPadraoCRUD
    {
        [StringLength(10)]
        public override string Codigo { get; set; }

        [StringLength(100)]
        public override string Descricao { get; set; }

        public bool IsAmbulatorio { get; set; }

        public bool IsInternacao { get; set; }
    }

}


