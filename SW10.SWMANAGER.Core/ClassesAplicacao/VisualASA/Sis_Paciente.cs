using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.VisualASA
{
    [Table("Sis_Paciente")]
    public class Sis_Paciente : CamposPadraoCRUD
    {

        public int? IDPaciente { get; set; }
        public int? IDSW { get; set; }
        public string CodPaciente { get; set; }
        public string Pai { get; set; }
        public string Mae { get; set; }
        public string CNS { get; set; }
        public byte[] Observacao { get; set; }
        public DateTime? DataUltimaMalaDir { get; set; }
        public string RacaCor { get; set; }
        public int? IDEtnia { get; set; }
        public bool? IsSUS { get; set; }
        public string UsuarioWeb { get; set; }
        public string SenhaWeb { get; set; }
        public bool? IsRecebeEmail { get; set; }
        public string ValorEscala { get; set; }
        public int? IDReligiao { get; set; }
        public string Matricula { get; set; }
        public string Categoria { get; set; }
        public string GrauDependente { get; set; }
        public int? Escolaridade { get; set; }
        public bool? IsCartao { get; set; }
        public string NumDeclNascVivo { get; set; }
        public string JustificativaNumDeclNascVivo { get; set; }
        public string UsuarioAgendaWeb { get; set; }
        public string SenhaAgendaWeb { get; set; }
        public int? IDEmpresaPac { get; set; }
        public int? IDExterno { get; set; }
        //tenant id para identificar a origem do registro
        public int? TenantId { get; set; }

    }
}
