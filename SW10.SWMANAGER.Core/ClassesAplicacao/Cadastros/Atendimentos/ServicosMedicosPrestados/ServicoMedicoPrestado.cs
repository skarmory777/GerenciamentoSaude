using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.ServicosMedicosPrestados
{
    [Table("ServicoMedicoPrestado")]
    public class ServicoMedicoPrestado : CamposPadraoCRUD
    {
        public string ModeloAnamnese { get; set; }

        public bool Caucao { get; set; }

        [ForeignKey("Especialidade"), Column("SisEspecialidadeId")]
        public long? EspecialidadeId { get; set; }

        public Especialidade Especialidade { get; set; }
    }
}

