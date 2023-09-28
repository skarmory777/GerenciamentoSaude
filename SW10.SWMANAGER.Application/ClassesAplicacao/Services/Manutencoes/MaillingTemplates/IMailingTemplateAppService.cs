using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates
{
    public interface IMailingTemplateAppService : IApplicationService
    {

        Task<PagedResultDto<MailingTemplateDto>> Listar(ListarMailingTemplateInput input);

        Task<ListResultDto<MailingTemplateDto>> ListarTodos();

        Task<MailingTemplateDto> Obter(long id);

        Task CriarOuEditar(MailingTemplateDto input);

        Task Excluir(MailingTemplateDto input);
    }
}
