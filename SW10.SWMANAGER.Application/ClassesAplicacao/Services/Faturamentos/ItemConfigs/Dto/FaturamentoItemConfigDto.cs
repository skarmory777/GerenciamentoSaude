using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Planos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto
{
    [AutoMap(typeof(FaturamentoItemConfig))]
    public class FaturamentoItemConfigDto : CamposPadraoCRUDDto
    {
        public long? ConvenioId { get; set; }
        public virtual ConvenioDto Convenio { get; set; }

        public long? PlanoId { get; set; }
        public virtual PlanoDto Plano { get; set; }

        public long? ItemId { get; set; }
        public virtual FaturamentoItemDto Item { get; set; }

        public long? ItemCobrarId { get; set; }
        public virtual FaturamentoItemDto ItemCobrar { get; set; }

        public static FaturamentoItemConfigDto Mapear(FaturamentoItemConfig faturamentoItemConfig)
        {
            if (faturamentoItemConfig == null)
            {
                return null;
            }

            var faturamentoItemConfigDto = MapearBase<FaturamentoItemConfigDto>(faturamentoItemConfig);

            faturamentoItemConfigDto.ConvenioId = faturamentoItemConfig.ConvenioId;
            faturamentoItemConfigDto.Convenio = ConvenioDto.Mapear(faturamentoItemConfig.Convenio);

            faturamentoItemConfigDto.PlanoId = faturamentoItemConfig.PlanoId;
            faturamentoItemConfigDto.Plano = PlanoDto.Mapear(faturamentoItemConfig.Plano);

            faturamentoItemConfigDto.ItemId = faturamentoItemConfig.ItemId;
            faturamentoItemConfigDto.Item = FaturamentoItemDto.Mapear(faturamentoItemConfig.Item);

            faturamentoItemConfigDto.ItemCobrarId = faturamentoItemConfig.ItemCobrarId;
            faturamentoItemConfigDto.ItemCobrar = FaturamentoItemDto.Mapear(faturamentoItemConfig.ItemCobrar);

            return faturamentoItemConfigDto;
        }

        public static FaturamentoItemConfig Mapear(FaturamentoItemConfigDto faturamentoItemConfigDto)
        {
            if (faturamentoItemConfigDto == null)
            {
                return null;
            }

            var faturamentoItemConfig = MapearBase<FaturamentoItemConfig>(faturamentoItemConfigDto);

            faturamentoItemConfig.ConvenioId = faturamentoItemConfigDto.ConvenioId;
            faturamentoItemConfig.Convenio = ConvenioDto.Mapear(faturamentoItemConfigDto.Convenio);

            faturamentoItemConfig.PlanoId = faturamentoItemConfigDto.PlanoId;
            faturamentoItemConfig.Plano = PlanoDto.Mapear(faturamentoItemConfigDto.Plano);

            faturamentoItemConfig.ItemId = faturamentoItemConfigDto.ItemId;
            faturamentoItemConfig.Item = FaturamentoItemDto.Mapear(faturamentoItemConfigDto.Item);

            faturamentoItemConfig.ItemCobrarId = faturamentoItemConfigDto.ItemCobrarId;
            faturamentoItemConfig.ItemCobrar = FaturamentoItemDto.Mapear(faturamentoItemConfigDto.ItemCobrar);

            return faturamentoItemConfig;
        }
    }
}
