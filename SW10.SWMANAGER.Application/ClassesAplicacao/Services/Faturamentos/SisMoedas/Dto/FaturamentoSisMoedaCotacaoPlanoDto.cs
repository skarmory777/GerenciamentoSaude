using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    //   [AutoMap(typeof(SisMoedaCotacaoPlano))]
    public class SisMoedaCotacaoPlanoDto : CamposPadraoCRUDDto
    {
        public virtual FaturamentoCotacaoMoedaDto SisMoedaCotacao { get; set; }
        public long? SisMoedaCotacaoId { get; set; }

        public virtual PlanoDto Plano { get; set; }
        public long? PlanoId { get; set; }
    }
}
