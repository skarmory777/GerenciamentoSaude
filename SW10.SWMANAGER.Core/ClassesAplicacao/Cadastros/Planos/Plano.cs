using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos
{
    [Table("SisPlano")]
    public class Plano : CamposPadraoCRUD
    {
        public Convenio Convenio { get; set; }

        [ForeignKey("Convenio"), Column("SisConvenioId")]
        public long? ConvenioId { get; set; }

        public bool IsDespesasAcompanhante { get; set; }

        public bool IsValidadeCarteiraIndeterminada { get; set; }

        //CONFIRMAR RELACIONAMENTO 
        //public long? TipoAcamodacaoId { get; set; }
        //[ForeignKey("CapituloCid")]
        //public virtual TipoAcamodacao TiposAcamodacoes { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsPlanoEmpresa { get; set; }

        public object this[string propertyName]
        {
            get { return this.GetType().GetProperty(propertyName).GetValue(this, null); }
            set { this.GetType().GetProperty(propertyName).SetValue(this, value, null); }
        }


        //public string NomeConvenio { get { return Convenio == null ? string.Empty : Convenio.Nome; } set {; } }
        //public virtual ICollection<Paciente> Pacientes { get; set; }
    }
}