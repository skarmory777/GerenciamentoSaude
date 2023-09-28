using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto
{
    [AutoMap(typeof(TipoTabelaDominio))]
    public class CriarOuEditarTipoTabelaDominio : CamposPadraoCRUDDto
    {

    }
}
