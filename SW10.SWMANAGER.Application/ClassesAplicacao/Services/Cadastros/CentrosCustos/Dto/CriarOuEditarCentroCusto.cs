using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto
{

    [AutoMap(typeof(CentroCusto))]
    public class CriarOuEditarCentroCusto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public long GrupoCentroCustoId { get; set; }

        public string CodigoCentroCusto { get; set; }

        public long UnidadeOrganizacionalId { get; set; }

        public bool IsReceberLancamento { get; set; }

        public bool IsFaturamento { get; set; }

        public bool IsAtivo { get; set; }
    }
}
