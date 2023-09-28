using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios
{
    [Table("SisIdentificacaoPrestadorNaOperadora")]
    public class IdentificacaoPrestadorNaOperadora : CamposPadraoCRUD
    {
        public long ConvenioId { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }

        public long EmpresaId { get; set; }
        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
    }
}
