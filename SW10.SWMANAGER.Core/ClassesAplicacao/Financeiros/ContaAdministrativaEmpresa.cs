using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinContaAdministrativaEmpresa")]
    public class ContaAdministrativaEmpresa : CamposPadraoCRUD
    {
        public long? ContaAdministrativaId { get; set; }

        [ForeignKey("ContaAdministrativaId")]
        public ContaAdministrativa ContaAdministrativa { get; set; }

        public long? EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }


    }
}
