using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    //   [AutoMap(typeof(SisMoedaCotacaoItem))]
    public class SisMoedaCotacaoItemDto : CamposPadraoCRUDDto
    {
        public virtual FaturamentoCotacaoMoedaDto SisMoedaCotacao { get; set; }
        public long? SisMoedaCotacaoId { get; set; }

        public virtual FaturamentoItem Item { get; set; }
        public long? ItemId { get; set; }
    }

}