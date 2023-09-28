using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TiposGrupo;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupos
{
    [AutoMap(typeof(FaturamentoTipoGrupo))]
    public class FaturamentoTipoGrupoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}