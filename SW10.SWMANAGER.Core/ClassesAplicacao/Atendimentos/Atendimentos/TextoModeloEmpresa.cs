using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos
{
    [Table("AteTextoEmpresa")]
    public class TextoModeloEmpresa : CamposPadraoCRUD
    {
        public long? TextoId { get; set; }

        [ForeignKey("TextoId")]
        public TextoModelo TextoModelo { get; set; }

        public long? EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }
    }
}
