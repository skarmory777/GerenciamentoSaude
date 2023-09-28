using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ElementosHtml.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ElementosHtml
{
    [AutoMap(typeof(ElementoHtmlDto))]
    public class CriarOuEditarElementoHtmlViewModel : ElementoHtmlDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public CriarOuEditarElementoHtmlViewModel(ElementoHtmlDto output)
        {
            output.MapTo(this);
        }
    }
}