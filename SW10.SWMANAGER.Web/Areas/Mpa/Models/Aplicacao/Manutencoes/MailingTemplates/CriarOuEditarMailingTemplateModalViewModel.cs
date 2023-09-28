using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates.Dto;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Manutencoes.MailingTemplates
{
    [AutoMap(typeof(MailingTemplateDto))]
    public class CriarOuEditarMailingTemplateModalViewModel : MailingTemplateDto
    {
        public bool IsEditMode { get { return Id > 0; } }

        public SelectList Classes { get; set; }

        public CriarOuEditarMailingTemplateModalViewModel(MailingTemplateDto output)
        {
            output.MapTo(this);
        }
    }
}