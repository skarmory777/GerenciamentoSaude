using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios
{
    [Table("SisFormConfigUnidadeOrganizacional")]
    public class FormConfigUnidadeOrganizacional : CamposPadraoCRUD
    {
        [ForeignKey("FormConfig"), Column("SisFormConfigId")]
        public long? FormConfigId { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }

        public FormConfig FormConfig { get; set; }

        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }
    }
}
