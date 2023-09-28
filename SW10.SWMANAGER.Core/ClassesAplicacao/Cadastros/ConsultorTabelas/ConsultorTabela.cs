using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ConsultorTabelas
{
    [Table("ConsultorTabela")]
    public class ConsultorTabela : CamposPadraoCRUD
    {

        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(500)]
        public string Observacao { get; set; }

        [StringLength(50)]
        public string ItemMenu { get; set; }

        public ICollection<ConsultorTabelaCampo> ConsultorTabelaCampos { get; set; }
    }
}
