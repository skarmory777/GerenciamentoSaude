using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes
{
    [Table("PacienteAlergias")]
    public class PacienteAlergias : CamposPadraoCRUD
    {
        [Index("Idx_DataCadastro")]
        public DateTime DataCadastro { get; set; }

        public string Alergia { get; set; }

        /*
         * //TODO: Fazer depois poder pegar de uma lista de alergias.
         * Caso seja Principio ativo deverá buscar na lista dos principios dos medicamentos
         * Caso seja Outros ou alimento o usuário poderá cadastrar uma alergia caso não tenha na lista.
         */

        //public long? PrincipioAtivo { get; set; }
        // public PrincipoAtivo {get;set;}

        //public long? AlergiaId {get;set;
        //public Alergias Alergia { get; set; }

        [ForeignKey("Paciente")]
        public long PacienteId { get; set; }

        public Paciente Paciente { get; set; }

        [ForeignKey("Atendimento")]
        public long? AtendimentoId { get; set; }

        public Atendimento Atendimento { get; set; }
    }


}
