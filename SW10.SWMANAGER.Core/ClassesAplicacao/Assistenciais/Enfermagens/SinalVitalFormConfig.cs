using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Enfermagens
{
    [Table("AssSinalVitalFormConfig")]
    public class SinalVitalFormConfig : CamposPadraoCRUD
    {
        [ForeignKey("SinalVital"), Column("AssSinalVitalId")]
        public long SinalVitalId { get; set; }

        public SinalVital SinalVital { get; set; }

        [ForeignKey("FormConfig"), Column("SisFormConfigId")]
        public long FormConfigId { get; set; }

        public FormConfig FormConfig { get; set; }
    }
}
