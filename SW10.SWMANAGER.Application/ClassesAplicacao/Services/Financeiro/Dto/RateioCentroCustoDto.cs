using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(RateioCentroCusto))]
    public class RateioCentroCustoDto : CamposPadraoCRUD
    {
        public string CentrosCustos { get; set; }

        public List<RateioCentroCustoItemDto> RateioCentroCustoItensDto { get; set; }
    }
}
