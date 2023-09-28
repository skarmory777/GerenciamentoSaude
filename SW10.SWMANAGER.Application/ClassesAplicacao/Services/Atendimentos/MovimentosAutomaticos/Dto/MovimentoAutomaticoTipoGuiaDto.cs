using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto
{
    public class MovimentoAutomaticoTipoGuiaDto : CamposPadraoCRUDDto
    {
        public long MovimentoAutomaticoId { get; set; }
        public long FaturamentoGuiaId { get; set; }

        public MovimentoAutomaticoDto MovimentoAutomatico { get; set; }
        public FaturamentoGuiaDto FaturamentoGuia { get; set; }

        public static MovimentoAutomaticoTipoGuiaDto Mapear(MovimentoAutomaticoTipoGuia movimentoAutomaticoTipoGuia)
        {
            MovimentoAutomaticoTipoGuiaDto movimentoAutomaticoTipoGuiaDto = new MovimentoAutomaticoTipoGuiaDto();

            movimentoAutomaticoTipoGuiaDto.Id = movimentoAutomaticoTipoGuia.Id;
            movimentoAutomaticoTipoGuiaDto.Codigo = movimentoAutomaticoTipoGuia.Codigo;
            movimentoAutomaticoTipoGuiaDto.Descricao = movimentoAutomaticoTipoGuia.Descricao;
            movimentoAutomaticoTipoGuiaDto.MovimentoAutomaticoId = movimentoAutomaticoTipoGuia.MovimentoAutomaticoId;
            movimentoAutomaticoTipoGuiaDto.FaturamentoGuiaId = movimentoAutomaticoTipoGuia.FaturamentoGuiaId;

            if (movimentoAutomaticoTipoGuia.FaturamentoGuia != null)
            {
                movimentoAutomaticoTipoGuiaDto.FaturamentoGuia = FaturamentoGuiaDto.Mapear(movimentoAutomaticoTipoGuia.FaturamentoGuia);
            }

            return movimentoAutomaticoTipoGuiaDto;
        }

        public static List<MovimentoAutomaticoTipoGuiaDto> Mapear(List<MovimentoAutomaticoTipoGuia> movimentoAutomaticoTipoGuia)
        {
            List<MovimentoAutomaticoTipoGuiaDto> movimentoAutomaticoTipoGuiaDto = new List<MovimentoAutomaticoTipoGuiaDto>();

            foreach (var item in movimentoAutomaticoTipoGuia)
            {
                movimentoAutomaticoTipoGuiaDto.Add(Mapear(item));
            }

            return movimentoAutomaticoTipoGuiaDto;
        }
    }
}
