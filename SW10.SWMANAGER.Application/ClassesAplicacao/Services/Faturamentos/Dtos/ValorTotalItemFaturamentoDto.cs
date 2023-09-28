using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Dtos
{
    public class ValorTotalItemFaturamentoDto
    {
        public ValorTotalItemFaturamentoDto()
        {
            
        }
        
        public ValorTotalItemFaturamentoDto(
            long contaMedicaId, 
            DateTime data, 
            long faturamentoItemId, 
            double qtd, 
            double percentual = 0,
            long? unidadeOrganizacionalId = 0,
            long? terceirizadoId = 0,
            long? centroCustoId = 0,
            long? turnoId = 0,
            long? tipoLeitoId = 0,
            ResumoDetalhamentoHonorarios honorarios = null)
        {
            ContaMedicaId = contaMedicaId;
            Data = data;
            FaturamentoItemId = faturamentoItemId;
            Qtd = qtd;
            Percentual = percentual;
            UnidadeOrganizacionalId = unidadeOrganizacionalId;
            TerceirizadoId = terceirizadoId;
            CentroCustoId = centroCustoId;
            TurnoId = turnoId;
            TipoLeitoId = tipoLeitoId;
            Honorarios = honorarios;
        }
        
        public long ContaMedicaId { get; set; }
        public DateTime Data { get; set; }
        public long FaturamentoItemId { get; set; }
        public double Qtd { get; set; }
        public double Percentual { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public long? TerceirizadoId { get; set; }

        public long? CentroCustoId { get; set; }

        public long? TurnoId { get; set; }

        public long? TipoLeitoId { get; set; }

        public ResumoDetalhamentoHonorarios Honorarios { get; set; }
    }
}