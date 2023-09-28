using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ElementosHtmlTipos
{
    [AutoMap(typeof(ElementoHtmlTipoDto))]
    public class CriarOuEditarElementoHtmlTipoViewModel : ElementoHtmlTipoDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarElementoHtmlTipoViewModel(ElementoHtmlTipoDto output)
        {
            output.MapTo(this);
        }
    }
}