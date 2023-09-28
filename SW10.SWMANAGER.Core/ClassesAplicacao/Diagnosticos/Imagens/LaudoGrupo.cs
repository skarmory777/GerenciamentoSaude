using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens
{
    [Table("LauGrupo")]
    public class LaudoGrupo : CamposPadraoCRUD
    {
        [StringLength(10)]
        public override string Codigo { get; set; }

        public override string Descricao { get; set; }

        public long? ModalidadeId { get; set; }

        [ForeignKey("ModalidadeId")]
        public Modalidade Modalidade { get; set; }

        public List<FaturamentoItem> Exames { get; set; }
    }
}
