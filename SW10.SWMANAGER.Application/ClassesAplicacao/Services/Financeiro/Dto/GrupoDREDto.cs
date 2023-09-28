using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(GrupoDRE))]
    public class GrupoDREDto : CamposPadraoCRUDDto
    {
    }
}
