using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(GrupoContaAdministrativa))]
    public class GrupoContaAdministrativaDto : CamposPadraoCRUDDto
    {
        public bool IsValorIncideResultadoOperacionalEmpresa { get; set; }
        public long GrupoDREId { get; set; }
        public GrupoDREDto GrupoDRE { get; set; }
        public string SubGrupos { get; set; }


        public List<SubGrupoContaAdministrativaDto> SubGruposCntAdm { get; set; }
    }
}
