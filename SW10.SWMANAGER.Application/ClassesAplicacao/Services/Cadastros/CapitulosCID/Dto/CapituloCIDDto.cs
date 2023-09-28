using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CapitulosCID;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID.Dto
{
    [AutoMap(typeof(CapituloCID))]
    public class CapituloCIDDto : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
    }
}
