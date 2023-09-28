
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.dtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.FaturarAtendimento.Pacote
{
    public class CriarOuEditarPacoteModalInputDto : CriarOuEditarContaBaseModalInputDto
    {
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public long? PacoteId { get; set; }
        public FaturamentoItemDto Pacote { get; set; }

        public string UnidadeOrganizacionalDescricao { get; set; }
        public string TerceirizadoDescricao { get; set; }

        public string CentroCustoDescricao { get; set; }

        public string TurnoDescricao { get; set; }

        public string TipoLeitoDescricao { get; set; }
    }


}