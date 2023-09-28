using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    [AutoMap(typeof(VWConsultaFaturamentoEntrega))]
    public class VWConsultaFaturamentoEntregaDto : EntityDto<long>
    {
        public string AnoMes { get; set; }
        public string Ano { get; set; }
        public string Mes { get; set; }
        public string ValorTotalEntregue { get; set; }
        public string ValorTotalRecebido { get; set; }
        //public string AnoMesEnt { get; set; }
        //public int QtdConta { get; set; }
        //public int QtdContaInternacao { get; set; }
        //public int QtdContaEmergencia { get; set; }
        //public string ValorTotal { get; set; }
        //public string ValorTotalInternacao { get; set; }
        //public string ValorTotalEmergencia { get; set; }
        //public string ValorMedioConta { get; set; }
        //public string ValorMedioContaInternacao { get; set; }
        //public string ValorMedioContaEmergencia { get; set; }
        //public string ValorMedioDia { get; set; }
        //public string ValorMedioDiaInternacao { get; set; }
        //public string ValorMedioDiaEmergencia { get; set; }
        //public int Dias { get; set; }
        //public int DiasInternacao { get; set; }
        //public int DiasEmergencia { get; set; }
    }
}