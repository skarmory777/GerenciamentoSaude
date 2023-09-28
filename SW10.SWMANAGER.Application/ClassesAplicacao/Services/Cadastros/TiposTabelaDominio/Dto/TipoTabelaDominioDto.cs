using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposTabelaDominio;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto
{
    [AutoMap(typeof(TipoTabelaDominio))]
    public class TipoTabelaDominioDto : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }
    }
}
