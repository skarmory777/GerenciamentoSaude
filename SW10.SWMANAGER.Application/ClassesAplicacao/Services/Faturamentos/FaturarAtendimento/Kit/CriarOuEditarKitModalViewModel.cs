using System.Collections.Generic;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Kit
{
    public class CriarOuEditarKitModalViewModel : CriarOuEditarKitModalInputDto
    {
        public List<FaturamentoContaItemDto> Items { get; set; }

        public static FaturamentoContaItemDto MapearKitItemParaFatContaItem(FaturamentoKitItemDto kitItem, CriarOuEditarKitModalInputDto input)
        {
            return new FaturamentoContaItemDto
            {
                Data = input.Data,
                FaturamentoContaId = input.ContaMedicaId,
                FaturamentoItemId = kitItem.FatItemId,
                Qtde = (float)kitItem.Quantidade,
                CentroCustoId = input.CentroCustoId,
                UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                TipoLeitoId = input.TipoLeitoId,
                ValorItem = (float)(input.ValorItem ?? 0),
                FaturamentoItem = kitItem.FatItem,
                TurnoId = input.TurnoId,
                TerceirizadoId = input.TerceirizadoId,
                FaturamentoContaKit = new FaturamentoContaKitDto
                {
                    Qtde = (float)kitItem.Quantidade,
                    Data = input.Data,
                    CentroCustoId = input.CentroCustoId,
                    UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                    TipoLeitoId = input.TipoLeitoId,
                    ValorItem = input.ValorItem,
                    FaturamentoKitId = input.KitId,
                    FaturamentoContaId = input.ContaMedicaId,
                }
            };
        }

        public FaturamentoContaItemDto GetDefinirParaTodos()
        {
            return new FaturamentoContaItemDto()
            {
                Data = Data,
                Qtde = (float)(Qtde ?? 0),
                TerceirizadoId = TerceirizadoId,
                CentroCustoId = CentroCustoId,
                TipoLeitoId = TipoLeitoId,
                TurnoId = TurnoId,
                UnidadeOrganizacionalId = UnidadeOrganizacionalId
            };
        }
    }


}