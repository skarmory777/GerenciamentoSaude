using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades
{
    [Table("SisEspecialidade")]
    public class Especialidade : CamposPadraoCRUD
    {
        public string Nome { get; set; }
        public string Cbo { get; set; }
        public string CboSus { get; set; }

        public bool IsAtivo { get; set; }

        [ForeignKey("SisCbo"), Column("SisCboId")]
        public long? CboId { get; set; }
        public Cbo SisCbo { get; set; }

        public ICollection<MedicoEspecialidade> MedicoEspecialidades { get; set; }
    }
}
