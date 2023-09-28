using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinRateioCentroCusto")]
    public class RateioCentroCusto : CamposPadraoCRUD
    {
        public List<RateioCentroCustoItem> RateioCentroCustoItens { get; set; }
    }
}
