using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class AgendamentoItemFaturamentoDto : CamposPadraoCRUDDto
    {
        public long AgendamentoCirurgicoId { get; set; }
        public long FaturamentoItemId { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public AgendamentoCirurgicoDto AgendamentoCirurgico { get; set; }

        public decimal Quantidade { get; set; }
        public bool IsCirurgica { get; set; }

        public static AgendamentoItemFaturamentoDto Mapear(AgendamentoItemFaturamento agendamentoItemFaturamento)
        {
            AgendamentoItemFaturamentoDto agendamentoItemFaturamentoDto = new AgendamentoItemFaturamentoDto();

            agendamentoItemFaturamentoDto.Id = agendamentoItemFaturamento.Id;
            agendamentoItemFaturamentoDto.AgendamentoCirurgicoId = agendamentoItemFaturamento.AgendamentoCirurgicoId ?? 0;
            agendamentoItemFaturamentoDto.FaturamentoItemId = agendamentoItemFaturamento.FaturamentoItemId ?? 0;

            agendamentoItemFaturamentoDto.Quantidade = agendamentoItemFaturamento.Quantidade;
            agendamentoItemFaturamentoDto.IsCirurgica = agendamentoItemFaturamento.IsCirurgica;

            if (agendamentoItemFaturamento.FaturamentoItem != null)
            {
                agendamentoItemFaturamentoDto.FaturamentoItem = FaturamentoItemDto.Mapear(agendamentoItemFaturamento.FaturamentoItem);
            }

            return agendamentoItemFaturamentoDto;
        }


        public static List<AgendamentoItemFaturamentoDto> Mapear(List<AgendamentoItemFaturamento> listAgendamentoItemFaturamento)
        {
            List<AgendamentoItemFaturamentoDto> listAgendamentoItemFaturamentoDto = new List<AgendamentoItemFaturamentoDto>();

            if (listAgendamentoItemFaturamento != null)
            {
                foreach (var item in listAgendamentoItemFaturamento)
                {
                    listAgendamentoItemFaturamentoDto.Add(Mapear(item));
                }
            }

            return listAgendamentoItemFaturamentoDto;
        }

    }
}
