using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposPrestadores;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Dto
{
    [AutoMap(typeof(TipoPrestador))]
    public class CriarOuEditarTipoPrestador : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

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
