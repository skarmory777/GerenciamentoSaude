using EnvioEmail.Core;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.MailingTemplate;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Manutencoes
{
    //[RoutePrefix("api/v1/EnviarEmail")]
    //public class EnviarEmailController : ApiController
    public class EnviarEmailController : SWMANAGERControllerBase
    {
        private readonly IMailingTemplateAppService _mailingTemplateAppService;
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IMedicoAppService _medicoAppService;
        private readonly IEmpresaAppService _empresaAppService;

        public EnviarEmailController(
            IMailingTemplateAppService mailingTemplateAppService,
            IPacienteAppService pacienteAppService,
            IMedicoAppService medicoAppService,
            IEmpresaAppService empresaAppService
            )
        {
            _mailingTemplateAppService = mailingTemplateAppService;
            _pacienteAppService = pacienteAppService;
            _medicoAppService = medicoAppService;
            _empresaAppService = empresaAppService;
        }

        [HttpPost]
        //[Route("Enviar")]
        public async Task<string> Enviar(EnvioModel model)
        {
            if (await ProcessarEnvio(model))
            {
                return "OK";
            }
            else
            {
                return "Não";
            }
        }

        private async Task<bool> ProcessarEnvio(EnvioModel model)
        {
            MailingTemplateDto templateDto = await _mailingTemplateAppService.Obter(model.MailingTemplateId);
            var template = MailingTemplateDto.Mapear(templateDto);
            var type = model.DestinatarioTipo;
            //var destinatario = new object();

            //typeof(model.DestinatarioTipo.GetType()) paciente = null;
            if (model.DestinatarioTipo.Equals("Paciente"))
            {
                var destinatario = new PacienteDto();
                destinatario = await _pacienteAppService.Obter(model.DestinatarioId);
                if (template != null && destinatario != null)
                {
                    var configCore = new MailingCore();
                    await configCore.ProcessarEnvioAsync(model, destinatario, template, destinatario.Email);
                }

                return template != null && destinatario != null;
            }
            else if (model.DestinatarioTipo.Equals("Empresa"))
            {
                var destinatario = new EmpresaDto();
                destinatario = await _empresaAppService.Obter(model.DestinatarioId);
                if (template != null && destinatario != null)
                {
                    var configCore = new MailingCore();
                    await configCore.ProcessarEnvioAsync(model, destinatario, template, destinatario.Email);
                }

                return template != null && destinatario != null;
            }
            else
            {
                var destinatario = new MedicoDto();
                destinatario = await _medicoAppService.Obter(model.DestinatarioId);
                if (template != null && destinatario != null)
                {
                    var configCore = new MailingCore();
                    await configCore.ProcessarEnvioAsync(model, destinatario, template, destinatario.Email);
                }

                return template != null && destinatario != null;
            }

        }

        /*
         * CUIDADO com o envio em lote, é recomendado usar alguma tecnologica como Fila para evitar gargalo no servidor
         */
        //[Route("Enviar/Lote")]
        public async Task<string> Lote(string emais)
        {
            IList<EnvioModel> malote = Newtonsoft.Json.JsonConvert.DeserializeObject<IList<EnvioModel>>(emais);
            foreach (var model in malote)
            {
                await ProcessarEnvio(model);
            }

            return "OK";
        }

    }
}
