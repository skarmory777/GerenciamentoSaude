using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.ClassificacoesRisco;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto
{
    [AutoMap(typeof(ClassificacaoRisco))]
    public class CriarOuEditarClassificacaoRisco : CamposPadraoCRUDDto
    {
        public string Senha { get; set; }

        public int Prioridade { get; set; } // tabela auxiliar

        public long PreAtendimentoId { get; set; }

        public long PacienteId { get; set; }

        public string FilaId { get; set; } // temporariamente string, depois sera chave estrangeira da classe ClasssificacaoFila

        public string SetorId { get; set; } // temporariamente string, depois sera chave estrangeira da classe Setor

        public long? EspecialidadeId { get; set; }

        [ForeignKey("PreAtendimentoId")]
        public virtual PreAtendimentoDto PreAtendimento { get; set; }

        [ForeignKey("PacienteId")]
        public virtual PacienteDto Paciente { get; set; }

        //[ForeignKey("ClassificacaoFilaId")]
        //public virtual ClassificacaoFilaDto ClassificacaoFila { get; set; }

        //[ForeignKey("SetorId")]
        //public virtual SetorDto Setor { get; set; }

        [ForeignKey("EspecialidadeId")]
        public virtual EspecialidadeDto Especialidade { get; set; }
    }
}
