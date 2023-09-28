using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstPreMovimentoItemKitLoteValidade")]
    public class EstoquePreMovimentoItemKitLoteValidade: CamposPadraoCRUD
    {
       public long EstoquePreMovimentoItemKitId { get; set; }

        [ForeignKey("EstoquePreMovimentoItemKitId")]
        public EstoquePreMovimentoItemKit EstoquePreMovimentoItemKit { get; set; }

        public long LoteValidadeId { get; set; }

        [ForeignKey("LoteValidadeId")]
        public LoteValidade LoteValidade { get; set; }

        public decimal Quantidade { get; set; }
    }
}
