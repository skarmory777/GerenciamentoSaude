using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstPreMovimentoItemKit")]
    public class EstoquePreMovimentoItemKit : CamposPadraoCRUD
    {
        public long EstoquePreMovimentoItemId { get; set; }

        [ForeignKey("EstoquePreMovimentoItemId")]
        public EstoquePreMovimentoItem EstoquePreMovimentoItem { get; set; }

        public long EstoqueKitItemId { get; set; }

        [ForeignKey("EstoqueKitItemId")]
        public EstoqueKitItem EstoqueKitItem { get; set; }

        public string NumeroSerie { get; set; }
    }
}
