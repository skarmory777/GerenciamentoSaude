using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto
{
    [AutoMap(typeof(FaturamentoItemConfigGlobal))]
    public class FaturamentoItemConfigGlobalDto : CamposPadraoCRUDDto
    {
        public long? GlobalId { get; set; }
        public FaturamentoGlobal Global { get; set; }

        public long? ItemId { get; set; }
        public FaturamentoItem Item { get; set; }

        public long? ItemCobrarId { get; set; }
        public FaturamentoItem ItemCobrar { get; set; }
    }
}
