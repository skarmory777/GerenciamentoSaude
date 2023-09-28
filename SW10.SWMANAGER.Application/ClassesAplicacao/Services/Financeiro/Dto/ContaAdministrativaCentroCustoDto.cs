using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(ContaAdministrativaCentroCusto))]
    public class ContaAdministrativaCentroCustoDto : CamposPadraoCRUDDto
    {
        public long? ContaAdministrativaId { get; set; }
        public ContaAdministrativaDto ContaAdministrativa { get; set; }
        public long CentroCustoId { get; set; }
        public CentroCustoDto CentroCusto { get; set; }
        public decimal Percentual { get; set; }
    }
}
