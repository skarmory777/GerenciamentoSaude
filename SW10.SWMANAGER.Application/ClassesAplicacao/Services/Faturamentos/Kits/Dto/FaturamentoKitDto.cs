using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits;
using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto
{
    [AutoMap(typeof(FaturamentoKit))]
    public class FaturamentoKitDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }

        public List<FaturamentoKitItemDto> Itens { get; set; }

        public long[] itensIds { get; set; }
        public string Observacao { get; set; }
        
        public FaturamentoItemDto FaturamentoItem { get; set; }
        
        public long? FaturamentoItemId { get; set; }

        public List<FaturamentoItemQuantidade> ItensQuantidade { get; set; }

        public string StrItensQtds { get; set; }

        #region Mapear

        public static FaturamentoKit Mapear(FaturamentoKitDto faturamentoKitDto)
        {
            var faturamentoKit = new FaturamentoKit
            {
                Id = faturamentoKitDto.Id,
                Codigo = faturamentoKitDto.Codigo,
                Descricao = faturamentoKitDto.Descricao,
                Observacao = faturamentoKitDto.Observacao,
                FaturamentoItem = FaturamentoItemDto.Mapear(faturamentoKitDto.FaturamentoItem),
                FaturamentoItemId = faturamentoKitDto.FaturamentoItemId,
                FatItens = FaturamentoKitItemDto.Mapear(faturamentoKitDto.Itens)
            };

            return faturamentoKit;
        }

        public static FaturamentoKitDto Mapear(FaturamentoKit faturamentoKit)
        {
            var faturamentoKitDto = new FaturamentoKitDto
            {
                Id = faturamentoKit.Id,
                Codigo = faturamentoKit.Codigo,
                Descricao = faturamentoKit.Descricao,
                Observacao = faturamentoKit.Observacao,
                FaturamentoItem = FaturamentoItemDto.Mapear(faturamentoKit.FaturamentoItem),
                FaturamentoItemId = faturamentoKit.FaturamentoItemId,
                Itens = FaturamentoKitItemDto.Mapear(faturamentoKit.FatItens)
            };

            return faturamentoKitDto;
        }

        #endregion

    }
}
