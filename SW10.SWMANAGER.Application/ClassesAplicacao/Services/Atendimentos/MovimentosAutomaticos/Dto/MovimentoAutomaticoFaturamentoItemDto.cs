using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto
{
    public class MovimentoAutomaticoFaturamentoItemDto : CamposPadraoCRUDDto
    {
        public long MovimentoAutomaticoId { get; set; }
        public long FaturamentoItemId { get; set; }

        public MovimentoAutomaticoDto MovimentoAutomatico { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }

        public static MovimentoAutomaticoFaturamentoItemDto Mapear(MovimentoAutomaticoFaturamentoItem movimentoAutomaticoFaturamentoItem)
        {
            MovimentoAutomaticoFaturamentoItemDto movimentoAutomaticoFaturamentoItemDto = new MovimentoAutomaticoFaturamentoItemDto();

            movimentoAutomaticoFaturamentoItemDto.Id = movimentoAutomaticoFaturamentoItem.Id;
            movimentoAutomaticoFaturamentoItemDto.Codigo = movimentoAutomaticoFaturamentoItem.Codigo;
            movimentoAutomaticoFaturamentoItemDto.Descricao = movimentoAutomaticoFaturamentoItem.Descricao;
            movimentoAutomaticoFaturamentoItemDto.MovimentoAutomaticoId = movimentoAutomaticoFaturamentoItem.MovimentoAutomaticoId;
            movimentoAutomaticoFaturamentoItemDto.FaturamentoItemId = movimentoAutomaticoFaturamentoItem.FaturamentoItemId;

            if (movimentoAutomaticoFaturamentoItem.FaturamentoItem != null)
            {
                movimentoAutomaticoFaturamentoItemDto.FaturamentoItem = FaturamentoItemDto.Mapear(movimentoAutomaticoFaturamentoItem.FaturamentoItem);
            }

            return movimentoAutomaticoFaturamentoItemDto;
        }

        public static List<MovimentoAutomaticoFaturamentoItemDto> Mapear(List<MovimentoAutomaticoFaturamentoItem> movimentoAutomaticoFaturamentoItem)
        {
            List<MovimentoAutomaticoFaturamentoItemDto> movimentoAutomaticoFaturamentoItemDto = new List<MovimentoAutomaticoFaturamentoItemDto>();

            foreach (var item in movimentoAutomaticoFaturamentoItem)
            {
                movimentoAutomaticoFaturamentoItemDto.Add(Mapear(item));
            }

            return movimentoAutomaticoFaturamentoItemDto;
        }
    }
}
