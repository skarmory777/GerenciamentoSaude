using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasItens;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens.Dto
{
    [AutoMap(typeof(FaturamentoBrasItem))]
    public class FaturamentoBrasItemDto : CamposPadraoCRUDDto
    {

        public static FaturamentoBrasItemDto Mapear(FaturamentoBrasItem faturamentoBrasItem)
        {
            return MapearBase<FaturamentoBrasItemDto>(faturamentoBrasItem);
        }

        public static FaturamentoBrasItem Mapear(FaturamentoBrasItemDto faturamentoBrasItemDto)
        {
            return MapearBase<FaturamentoBrasItem>(faturamentoBrasItemDto);
        }
    }
}
