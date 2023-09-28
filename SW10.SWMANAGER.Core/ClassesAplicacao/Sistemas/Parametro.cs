using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Sistemas
{
    [Table("SisParametro")]
    public class Parametro : CamposPadraoCRUD
    {
        [Index("UK_Parametro", IsUnique = true, Order = 1)]
        public override string Codigo { get; set; }

        [Index("UK_Parametro", IsUnique = true, Order = 2)]
        public long? EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
    }
}
