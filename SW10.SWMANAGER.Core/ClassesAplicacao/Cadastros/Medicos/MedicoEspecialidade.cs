using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos
{
    [Table("SisMedicoEspecialidade")]
    //public class MedicoEspecialidade : Entity<long>
    public class MedicoEspecialidade : CamposPadraoCRUD
    {
        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long? MedicoId { get; set; }
        public Medico Medico { get; set; }

        [ForeignKey("Especialidade"), Column("SisEspecialidadeId")]
        public long EspecialidadeId { get; set; }
        public Especialidade Especialidade { get; set; }

    }
}
