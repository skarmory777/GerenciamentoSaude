using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits
{
    [Table("FatKit")]
    public class FaturamentoKit : CamposPadraoCRUD
    {
        [StringLength(10)]
        public override string Codigo { get; set; }

        [StringLength(255)]
        public override string Descricao { get; set; }
        
        public FaturamentoItem FaturamentoItem { get; set; }
        
        public long? FaturamentoItemId { get; set; }

        public List<FaturamentoKitItem> FatItens { get; set; }

        [StringLength(255)]
        public string Observacao { get; set; }
    }

}


