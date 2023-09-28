using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposSanguineos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto
{
    [AutoMap(typeof(TipoSanguineo))]
    public class TipoSanguineoDto : CamposPadraoCRUDDto
    {
    }
}
