using Abp.AutoMapper;

namespace SW10.SWMANAGER.ClassesAplicacao.Services
{
    [AutoMapTo(typeof(GuiaTipo))]
    public class GuiaTipoDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }
    }
}
