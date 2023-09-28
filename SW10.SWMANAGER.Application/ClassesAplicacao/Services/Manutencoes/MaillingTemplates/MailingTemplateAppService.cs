using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Manutencoes.MailingTemplates;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates
{
    public class MailingTemplateAppService : SWMANAGERAppServiceBase, IMailingTemplateAppService
    {
        private readonly IRepository<MailingTemplate, long> _mailingTemplateRepository;
        //private readonly IListarControleProducaosExcelExporter _listarControleProducaosExcelExporter;

        public MailingTemplateAppService(
            IRepository<MailingTemplate, long> mailingTemplateRepository
            )
        {
            _mailingTemplateRepository = mailingTemplateRepository;
        }

        public async Task CriarOuEditar(MailingTemplateDto input)
        {
            try
            {
                //propriedade requerida que o form ñ consegue tratar
                //---acertar na pagina depois (pablo 08/08/2017)------
                if (input.ContentTemplate.IsNullOrWhiteSpace())
                {
                    input.ContentTemplate = ".";
                }
                //---------------------fim´------------------------------

                var output = input.MapTo<MailingTemplate>();
                if (input.Id > 0)
                {
                    await _mailingTemplateRepository.UpdateAsync(output);
                }
                else
                {
                    await _mailingTemplateRepository.InsertAsync(output);
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("ErroSalvar", ex);
            }
        }

        public async Task Excluir(MailingTemplateDto input)
        {
            try
            {
                await _mailingTemplateRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("ErroSalvar", ex);
            }
        }

        public async Task<PagedResultDto<MailingTemplateDto>> Listar(ListarMailingTemplateInput input)
        {
            try
            {
                var pacientes = await _mailingTemplateRepository
                    .GetAll()
                    .AsNoTracking()
                    .ToListAsync();

                var mailingTemplatesDto = pacientes
                    .MapTo<List<MailingTemplateDto>>();

                return new PagedResultDto<MailingTemplateDto> { Items = mailingTemplatesDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<MailingTemplateDto>> ListarTodos()
        {
            try
            {
                var mailingTemplate = await _mailingTemplateRepository
                    .GetAll()
                    .AsNoTracking()
                    .ToListAsync();

                var mailingTemplatesDto = mailingTemplate
                    .MapTo<List<MailingTemplateDto>>();

                return new ListResultDto<MailingTemplateDto> { Items = mailingTemplatesDto };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<MailingTemplateDto> Obter(long id)
        {
            try
            {
                var query = await _mailingTemplateRepository
                    .GetAsync(id);

                var mailingTemplate = query.MapTo<MailingTemplateDto>();

                return mailingTemplate;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


    }
}
