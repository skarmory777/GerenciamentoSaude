using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(ContaAdministrativa))]
    public class ContaAdministrativaDto : CamposPadraoCRUDDto
    {

        public bool IsNaoContabilizarPagarGerencial { get; set; }
        public bool IsNaoContabilizarReceberGerencial { get; set; }
        public bool IsReceita { get; set; }
        public bool IsDespesa { get; set; }
        public long? RateioCentroCustoId { get; set; }
        public RateioCentroCustoDto RateioCentroCusto { get; set; }
        public long SubGrupoContaAdministrativaId { get; set; }
        public SubGrupoContaAdministrativaDto SubGrupoContaAdministrativa { get; set; }
        public string CentrosCustos { get; set; }
        public string Empresas { get; set; }
        public string Exibir { get; set; }


        public List<ContaAdministrativaCentroCustoDto> ContaAdministrativaCustos { get; set; }
        public List<ContaAdministrativaEmpresaDto> ContasAdministrativaEmpresas { get; set; }

    }
}
