using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCentroCusto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Dto
{
    [AutoMap(typeof(GrupoCentroCusto))]
    public class GrupoCentroCustoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        //public long? TipoGrupoCentroCustosId { get; set; }

        //public virtual TipoGrupoCentroCustoDto TipoGrupoCentroCustos { get; set; }
    }
}
