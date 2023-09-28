using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios
{
    [Table("SisFormConfigEspecialidade")]
    public class FormConfigEspecialidade : CamposPadraoCRUD
    {
        [ForeignKey("FormConfig"), Column("SisFormConfigId")]
        public long? FormConfigId { get; set; }

        [ForeignKey("Especialidade"), Column("SisEspecialidadeId")]
        public long? EspecialidadeId { get; set; }

        public FormConfig FormConfig { get; set; }

        public Especialidade Especialidade { get; set; }
    }
}
