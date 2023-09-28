using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Regioes;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes.Dto
{
    [AutoMap(typeof(Regiao))]
    public class CriarOuEditarRegiao : CamposPadraoCRUDDto
    {

    }
}
