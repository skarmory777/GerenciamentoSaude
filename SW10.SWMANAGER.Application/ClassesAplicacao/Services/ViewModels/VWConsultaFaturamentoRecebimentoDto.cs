using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    [AutoMap(typeof(VWConsultaFaturamentoRecebimento))]
    public class VWConsultaFaturamentoRecebimentoDto : EntityDto<long>
    {
        public string AnoMesPG { get; set; }
        public string ValorQuitacaoAmbulatorio { get; set; }
        public string ValorQuitacaoInternacao { get; set; }
        public string ValorQuitacaoSemIdentificacao { get; set; }
        public string ValorQuitacaoLancamento { get; set; }
    }
}