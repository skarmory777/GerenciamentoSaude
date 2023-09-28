using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.PreAtendimentos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ClassificacoesRisco
{
    [Table("ClassificacaoRisco")]
    public class ClassificacaoRisco : CamposPadraoCRUD
    {
        public string Senha { get; set; }

        public int Prioridade { get; set; } // tabela auxiliar

        public long PreAtendimentoId { get; set; }

        public long PacienteId { get; set; }

        public string FilaId { get; set; } // temporariamente string, depois sera chave estrangeira da classe ClasssificacaoFila

        public string SetorId { get; set; } // temporariamente string, depois sera chave estrangeira da classe Setor

        public long? EspecialidadeId { get; set; }

        //[ForeignKey("ClassificacaoFilaId")]
        //public virtual ClassificacaoFilaDto ClassificacaoFila { get; set; }

        //[ForeignKey("SetorId")]
        //public virtual SetorDto Setor { get; set; }

        [ForeignKey("PreAtendimentoId")]
        public PreAtendimento PreAtendimento { get; set; }

        [ForeignKey("PacienteId")]
        public Paciente Paciente { get; set; }

        [ForeignKey("EspecialidadeId")]
        public Especialidade Especialidade { get; set; }
    }
}
