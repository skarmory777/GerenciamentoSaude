using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto
{
    public class FaturamentoKitItemDto : CamposPadraoCRUDDto
    {
        public long? FatKitId { get; set; }
        public FaturamentoKitDto FatKit { get; set; }

        public long? FatItemId { get; set; }
        public FaturamentoItemDto FatItem { get; set; }

        public decimal Quantidade { get; set; }

        public static FaturamentoKitItemDto Mapear(FaturamentoKitItem faturamentoKitItem)
        {
            var faturamentoKitItemDto = new FaturamentoKitItemDto
            {
                Id = faturamentoKitItem.Id,
                Codigo = faturamentoKitItem.Codigo,
                Descricao = faturamentoKitItem.Descricao,
                Quantidade = faturamentoKitItem.Quantidade,
                FatKitId = faturamentoKitItem.FatKitId,
                FatItemId = faturamentoKitItem.FatItemId
            };

            if (faturamentoKitItem.FatItem != null)
            {
                faturamentoKitItemDto.FatItem = FaturamentoItemDto.Mapear(faturamentoKitItem.FatItem);
            }


            return faturamentoKitItemDto;
        }

        public static FaturamentoKitItem Mapear(FaturamentoKitItemDto faturamentoKitItemDto)
        {
            var faturamentoKitItem = new FaturamentoKitItem
            {
                Id = faturamentoKitItemDto.Id,
                Codigo = faturamentoKitItemDto.Codigo,
                Descricao = faturamentoKitItemDto.Descricao,
                Quantidade = faturamentoKitItemDto.Quantidade,
                FatKitId = faturamentoKitItemDto.FatKitId,
                FatItemId = faturamentoKitItemDto.FatItemId
            };

            if (faturamentoKitItemDto.FatItem != null)
            {
                faturamentoKitItem.FatItem = FaturamentoItemDto.Mapear(faturamentoKitItemDto.FatItem);
            }

            return faturamentoKitItem;
        }

        public static List<FaturamentoKitItemDto> Mapear(List<FaturamentoKitItem> itens)
        {
            var itensDto = new List<FaturamentoKitItemDto>();

            if (itens == null) return itensDto;
            foreach (var item in itens)
            {
                itensDto.Add(Mapear(item));
            }
            return itensDto;
        }

        public static List<FaturamentoKitItem> Mapear(List<FaturamentoKitItemDto> itensDto)
        {
            var itens = new List<FaturamentoKitItem>();
            if (itensDto == null) return itens;
            foreach (var itemDto in itensDto)
            {
                itens.Add(Mapear(itemDto));
            }
            return itens;
        }
    }
}
