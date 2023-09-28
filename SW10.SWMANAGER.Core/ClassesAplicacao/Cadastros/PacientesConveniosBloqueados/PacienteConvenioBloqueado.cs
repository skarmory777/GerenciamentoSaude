using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.PacientesConveniosBloqueados
{
    [Table("PacienteConvenioBloqueado")]
    public class PacienteConvenioBloqueado : CamposPadraoCRUD
    {
        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }
        public long? ConvenioId { get; set; }

        public string Matricula { get; set; }

        [Index("Idx_DataImportacao")]
        public DateTime DataImportacao { get; set; }


        //CONFIRMAR RELACIONAMENTO
        //public long? UsuarioImportacaoId { get; set; }
        //[ForeignKey("UsuarioImportacaoId")]
        //public virtual UsuarioImportacao UsuarioImportacao { get; set; }

        public bool IsReativaCarteira { get; set; }

        public string Justificativa { get; set; }

        public string UsuarioReativado { get; set; }

        [Index("Idx_DataUsuarioReativado")]
        public DateTime DataUsuarioReativado { get; set; }


    }
}
