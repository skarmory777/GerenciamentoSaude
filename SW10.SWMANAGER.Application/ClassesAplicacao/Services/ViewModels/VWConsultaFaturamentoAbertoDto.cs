using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    [AutoMap(typeof(VWConsultaFaturamentoAberto))]
    public class VWConsultaFaturamentoAbertoDto : EntityDto<long>
    {
        public string AnoMesVenc { get; set; }
        public string ValorDifEntregaGlosa { get; set; }
        public string ValorEntrega { get; set; }
        public string ValorQuitacaoAmbulatorio { get; set; }
        public string ValorQuitacaoInternacao { get; set; }
        public string ValorLancamentoAberto { get; set; }
        public string ValorGlosa { get; set; }
    }
}