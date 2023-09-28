using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposPrestadores
{
    [Table("SisTipoPrestador")]
    public class TipoPrestador : CamposPadraoCRUD
    {
        //ACERTAR REFERENCIA
        //public long? ConselhoId { get; set; }
        //[ForeignKey("ConselhoId")]
        //public virtual Conselho Consenho { get; set; }

        public bool IsDescricao { get; set; }

        public bool IsEvolucaoEnfermagem { get; set; }

        public bool IsSolicitaIntervencao { get; set; }

        public bool IsTecnicoExame { get; set; }

        public bool IsAssinaLaudo { get; set; }

        public bool IsNumeroConselhoObrigatorio { get; set; }

    }

}
