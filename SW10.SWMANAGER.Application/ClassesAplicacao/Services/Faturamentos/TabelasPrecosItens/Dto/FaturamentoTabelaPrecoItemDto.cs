using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaPrecoItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TabelasPrecosItens.Dto
{
    public class FaturamentoTabelaPrecoItemDto : CamposPadraoCRUDDto
    {
        public FaturamentoItemDto Item { get; set; }
        public long ItemId { get; set; }

        #region Mapeamento
        public static FaturamentoTabelaPrecoItemDto Mapear(FaturamentoTabelaPrecoItem input)
        {
            var result = new FaturamentoTabelaPrecoItemDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.ItemId = input.ItemId;

            if (input.Item != null)
            {
                result.Item = FaturamentoItemDto.Mapear(input.Item);
            }

            return result;
        }

        public static FaturamentoTabelaPrecoItem Mapear(FaturamentoTabelaPrecoItemDto input)
        {
            var result = new FaturamentoTabelaPrecoItem();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.ItemId = input.ItemId;

            return result;
        }

        public static IEnumerable<FaturamentoTabelaPrecoItemDto> Mapear(List<FaturamentoTabelaPrecoItem> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }

        public static IEnumerable<FaturamentoTabelaPrecoItem> Mapear(List<FaturamentoTabelaPrecoItemDto> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        #endregion

    }
}
