using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Manutencoes.MailingTemplates;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates.Dto
{
    [AutoMap(typeof(MailingTemplate))]
    public class MailingTemplateDto : CamposPadraoCRUDDto
    {
        public string Name { get; set; }
        public string Titulo { get; set; }
        public string EmailSaida { get; set; }
        public string NomeSaida { get; set; }
        public string ContentTemplate { get; set; }
        public string CamposDisponiveis { get; set; }

        public static MailingTemplateDto Mapear(MailingTemplate mailingTemplate)
        {
            if (mailingTemplate == null)
            {
                return null;
            }

            var mailingTemplateDto = MapearBase<MailingTemplateDto>(mailingTemplate);

            mailingTemplateDto.Name = mailingTemplate.Name;
            mailingTemplateDto.Titulo = mailingTemplate.Titulo;
            mailingTemplateDto.EmailSaida = mailingTemplate.EmailSaida;
            mailingTemplateDto.NomeSaida = mailingTemplate.NomeSaida;
            mailingTemplateDto.ContentTemplate = mailingTemplate.ContentTemplate;
            mailingTemplateDto.CamposDisponiveis = mailingTemplate.CamposDisponiveis;

            return mailingTemplateDto;
        }

        public static MailingTemplate Mapear(MailingTemplateDto mailingTemplateDto)
        {
            if (mailingTemplateDto == null)
            {
                return null;
            }

            var mailingTemplate = MapearBase<MailingTemplate>(mailingTemplateDto);

            mailingTemplate.Name = mailingTemplateDto.Name;
            mailingTemplate.Titulo = mailingTemplateDto.Titulo;
            mailingTemplate.EmailSaida = mailingTemplateDto.EmailSaida;
            mailingTemplate.NomeSaida = mailingTemplateDto.NomeSaida;
            mailingTemplate.ContentTemplate = mailingTemplateDto.ContentTemplate;
            mailingTemplate.CamposDisponiveis = mailingTemplateDto.CamposDisponiveis;

            return mailingTemplate;
        }

    }
}
