using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto
{
    [AutoMap(typeof(CentroCusto))]
    public class CentroCustoDto : CamposPadraoCRUDDto
    {

        //public string Descricao { get; set; }

        public long? GrupoCentroCustoId { get; set; }

        public virtual GrupoCentroCustoDto GrupoCentroCusto { get; set; }

        public string CodigoCentroCusto { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public virtual UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        public bool IsReceberLancamento { get; set; }

        public bool IsAtivo { get; set; }


        public static CentroCusto Mapear(CentroCustoDto centroCustoDto)
        {
            CentroCusto centroCusto = new CentroCusto();

            centroCusto.Id = centroCustoDto.Id;
            centroCusto.Codigo = centroCustoDto.Codigo;
            centroCusto.Descricao = centroCustoDto.Descricao;

            return centroCusto;
        }

        public static CentroCustoDto Mapear(CentroCusto centroCusto)
        {
            CentroCustoDto centroCustoDto = new CentroCustoDto();

            centroCustoDto.Id = centroCusto.Id;
            centroCustoDto.Codigo = centroCusto.Codigo;
            centroCustoDto.Descricao = centroCusto.Descricao;

            return centroCustoDto;
        }

    }
}
