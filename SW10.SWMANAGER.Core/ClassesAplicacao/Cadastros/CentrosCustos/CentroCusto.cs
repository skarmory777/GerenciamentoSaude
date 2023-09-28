using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCentroCusto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos
{
    [Table("CentroCusto")]
    public class CentroCusto : CamposPadraoCRUD
    {
        public long? GrupoCentroCustoId { get; set; }
        [ForeignKey("GrupoCentroCustoId")]
        public GrupoCentroCusto GrupoCentroCusto { get; set; }

        public string CodigoCentroCusto { get; set; }

        public ICollection<UnidadeOrganizacional> UnidadesOrganizacionais { get; set; }

        public bool IsReceberLancamento { get; set; }

        public bool IsFaturamento { get; set; }

        public bool IsAtivo { get; set; }

    }
}


