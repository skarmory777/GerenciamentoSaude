using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(SubGrupoContaAdministrativa))]
    public class SubGrupoContaAdministrativaDto : CamposPadraoCRUDDto
    {
        public long GrupoContaAdministrativaId { get; set; }

        public GrupoContaAdministrativaDto GrupoContaAdministrativa { get; set; }

        public bool IsSubGrupoContaNaoOperacional { get; set; }
        public bool IsUtilizadoCalculoSalario { get; set; }
        public bool IsSomandoDespesas { get; set; }
        public bool IsUsarFormula { get; set; }
        public bool IsNaoDetalharContaAdministrativa { get; set; }

        public long? IdGrid { get; set; }
    }
}
