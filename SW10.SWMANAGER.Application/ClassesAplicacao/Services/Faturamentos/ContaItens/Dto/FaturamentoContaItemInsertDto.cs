using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto
{
    public class FaturamentoContaItemInsertDto
    {
        public long Id { get; set; }
        public long AtendimentoId { get; set; }
        public long? CentroCustoId { get; set; }
        public DateTime? Data { get; set; }
        public long? MedicoId { get; set; }
        public string Obs { get; set; }
        public float? Qtd { get; set; }
        public long? TurnoId { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public long? ContaId { get; set; }

        public List<FaturamentoContaItemDto> ItensFaturamento { get; set; }
    }
}
