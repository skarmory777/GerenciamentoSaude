using Abp.Extensions;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.CoresPele;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Escolaridades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.EstadosCivis;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Religioes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Sexos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposTelefone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.Web.Controllers;
using SW10.SWMANAGER.Web.CorreiosService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class MedicosController : SWMANAGERControllerBase
    {
        private readonly IMedicoAppService _medicoAppService;
        private readonly IPaisAppService _paisAppService;
        private readonly IEstadoAppService _estadoAppService;
        private readonly ICidadeAppService _cidadeAppService;
        private readonly IProfissaoAppService _profissaoAppService;
        private readonly INaturalidadeAppService _naturalidadeAppService;
        private readonly IEspecialidadeAppService _especialidadeAppService;
        private readonly ISexoAppService _sexoAppService;
        private readonly IEscolaridadeAppService _escolaridadeAppService;
        private readonly ICorPeleAppService _corPeleAppService;
        private readonly IReligiaoAppService _religiaoAppService;
        private readonly IEstadoCivilAppService _estadoCivilAppService;
        private readonly ITipoTelefoneAppService _tipoTelefoneAppService;
        private readonly IMedicoEspecialidadeAppService _medicoEspecialidadeAppService;
        private readonly ITipoLogradouroAppService _tipoLogradouroAppService;
        private readonly IMailingTemplateAppService _mailingTemplateAppService;

        public MedicosController(
            IMedicoAppService medicoAppService,
            IPaisAppService paisAppService,
            IEstadoAppService estadoAppService,
            IProfissaoAppService profissaoAppService,
            INaturalidadeAppService naturalidadeAppService,
            ICidadeAppService cidadeAppService,
            IEspecialidadeAppService especialidadeAppService,
            ISexoAppService sexoAppService,
            IEscolaridadeAppService escolaridadeAppService,
            ICorPeleAppService corPeleAppService,
            IReligiaoAppService religiaoAppService,
            IEstadoCivilAppService estadoCivilAppService,
            ITipoTelefoneAppService tipoTelefoneAppService,
            IMedicoEspecialidadeAppService medicoEspecialidadeAppService,
            ITipoLogradouroAppService tipoLogradouroAppService,
            IMailingTemplateAppService mailingTemplateAppService
            )
        {
            _medicoAppService = medicoAppService;
            _paisAppService = paisAppService;
            _estadoAppService = estadoAppService;
            _cidadeAppService = cidadeAppService;
            _profissaoAppService = profissaoAppService;
            _naturalidadeAppService = naturalidadeAppService;
            _especialidadeAppService = especialidadeAppService;
            _sexoAppService = sexoAppService;
            _escolaridadeAppService = escolaridadeAppService;
            _corPeleAppService = corPeleAppService;
            _religiaoAppService = religiaoAppService;
            _estadoCivilAppService = estadoCivilAppService;
            _tipoTelefoneAppService = tipoTelefoneAppService;
            _medicoEspecialidadeAppService = medicoEspecialidadeAppService;
            _tipoLogradouroAppService = tipoLogradouroAppService;
            _mailingTemplateAppService = mailingTemplateAppService;
        }


        public async Task<ActionResult> Index()
        {
            var query = await _mailingTemplateAppService.ListarTodos();
            var lista = query.Items.ToList();
            ViewBag.Templates = lista;

            var model = new MedicosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Medico_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Medico_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            //var sexosAll = await _sexoAppService.ListarTodos();
            //var sexoAmbos = sexosAll.Items.FirstOrDefault(p => p.Descricao == "Ambos");
            //List<SexoDto> sexs = new List<SexoDto>();
            //sexs.Add(sexoAmbos);
            //var sexos = sexosAll.Items;//.Except(sexs).ToList();

            //var coresPele = await _corPeleAppService.ListarTodos();
            //var escolaridades = await _escolaridadeAppService.ListarTodos();
            //var religioes = await _religiaoAppService.ListarTodos();
            //var estadosCivis = await _estadoCivilAppService.ListarTodos();
            //var tiposTelefone = await _tipoTelefoneAppService.ListarTodos();
            //var tiposLogradouro = await _tipoLogradouroAppService.ListarTodos();

            CriarOuEditarMedicoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _medicoAppService.Obter(id.Value);
                viewModel = new CriarOuEditarMedicoModalViewModel(output);
                var medicosEspecialidadesList = await _medicoEspecialidadeAppService.ListarMedicoEspecialidadePorMedico(id.Value);


                var lista = new List<EspecialidadeMedicoDto>();

                long idGrid = 0;
                foreach (var item in medicosEspecialidadesList.Items.ToList())
                {
                    var itemDto = new EspecialidadeMedicoDto();

                    itemDto.Id = item.Id;
                    itemDto.IdEspecialidade = item.Especialidade.Id;
                    itemDto.Descricao = item.Especialidade.Descricao;
                    itemDto.IdGridMedicoEspecialidade = ++idGrid;
                    lista.Add(itemDto);
                }
                viewModel.MedicoEspecialidadeList = JsonConvert.SerializeObject(lista);

                //                var medicoEspecialidadeList = await _medicoEspecialidadeAppService.Listar((long)id);
                //var especialidades = await _especialidadeAppService.ListarPorMedico(output.Id);

                //viewModel.Especialidades = especialidades;
                //viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao", output.Sexo);
                //viewModel.Sexos = new SelectList(sexos, "Id", "Descricao", output.Sexo);
                //viewModel.Escolaridades = new SelectList(escolaridades.Items, "Id", "Descricao", output.Escolaridade);
                //viewModel.CoresPele = new SelectList(coresPele.Items, "Id", "Descricao", output.CorPele);
                //viewModel.Religioes = new SelectList(religioes.Items, "Id", "Descricao", output.Religiao);
                //viewModel.EstadosCivis = new SelectList(estadosCivis.Items, "Id", "Descricao", output.EstadoCivil);
                //viewModel.TiposLogradouro = new SelectList(tiposLogradouro.Items, "Id", "Descricao", output.TipoLogradouroId);
                //viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");
            }
            else
            {
                viewModel = new CriarOuEditarMedicoModalViewModel(new MedicoDto());
                viewModel.MedicoEspecialidadeList = JsonConvert.SerializeObject(new List<MedicoEspecialidadeDto>());

                //viewModel.Sexos = new SelectList(sexos.Items, "Id", "Descricao");
                //viewModel.Sexos = new SelectList(sexos, "Id", "Descricao");
                //viewModel.CoresPele = new SelectList(coresPele.Items, "Id", "Descricao");
                //viewModel.Escolaridades = new SelectList(escolaridades.Items, "Id", "Descricao");
                //viewModel.Religioes = new SelectList(religioes.Items, "Id", "Descricao");
                //viewModel.EstadosCivis = new SelectList(estadosCivis.Items, "Id", "Descricao");
                //viewModel.TiposTelefone = new SelectList(tiposTelefone.Items, "Id", "Descricao");
                //viewModel.TiposLogradouro = new SelectList(tiposLogradouro.Items, "Id", "Descricao");
            }

            viewModel.Nascimento = viewModel.Nascimento ?? DateTime.Now;

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/_CriarOuEditarModal.cshtml", viewModel);
        }

        public ActionResult ObterIdade(DateTime data)
        {
            var idade = DateDifference.GetExtendedDifference(data);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/_ObterIdade.cshtml", idade);
        }

        public PartialViewResult _CarregarFoto()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/_CarregarFoto.cshtml");
        }

        public async Task<ActionResult> _CriarOuEditarMedicoEspecialidadeModal(long medicoId, long? id)
        {
            CriarOuEditarMedicoEspecialidadeModalViewModel viewModel; //=new CriarOuEditarMedicoEspecialidadeModalViewModel()
            var medico = await _medicoAppService.Obter(medicoId);
            var especialidades = await _especialidadeAppService.ListarTodos();
            //var medicoEspecialidades = medico.MedicoEspecialidades;
            //var especialidadesCadastradas = medicoEspecialidades.Select(m => m.EspecialidadeId);

            //var especialidadesMedico = especialidades.Items.Where(m => m.Id.IsIn(especialidadesCadastradas.ToArray()));
            //var especialidadesDisponiveis = especialidades.Items.Except(especialidadesMedico).ToList();
            if (id.HasValue)
            {
                var output = await _medicoEspecialidadeAppService.Obter((long)id);
                viewModel = new CriarOuEditarMedicoEspecialidadeModalViewModel(output);
                var especialidade = await _especialidadeAppService.Obter(output.EspecialidadeId);
                //viewModel.Especialidade = especialidade;
                viewModel.EspecialidadeId = output.EspecialidadeId;
                //viewModel.Especialidades = new SelectList(especialidadesDisponiveis, "Id", "Nome", viewModel.EspecialidadeId);
            }
            else
            {
                viewModel = new CriarOuEditarMedicoEspecialidadeModalViewModel(new MedicoEspecialidadeDto());
                //viewModel.Especialidades = new SelectList(especialidadesDisponiveis, "Id", "Nome");
            }
            //viewModel.Medico = medico;
            viewModel.MedicoId = medicoId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/_CriarOuEditarMedicoEspecialidadeModal.cshtml", viewModel);
        }

        public async Task<ActionResult> _MedicoEspecialidades(long id)
        {
            var especialidades = await _especialidadeAppService.ListarTodos();
            var result = await _medicoAppService.Obter(id);
            //var medicoEspecialidades = result.MedicoEspecialidades.ToList();
            var viewModel = new MedicoEspecialidadesViewModel();
            viewModel.Especialidades = especialidades.Items.ToList();
            //viewModel.MedicoEspecialidades = medicoEspecialidades;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Medicos/_MedicoEspecialidades.cshtml", viewModel);
        }

        public async Task<FileContentResult> ObterFotoMedico(long id)
        {
            var medicoDto = await _medicoAppService.Obter(id);
            if (medicoDto.Foto.Length > 0)
            {
                return File(medicoDto.Foto, medicoDto.FotoMimeType);
            }
            else
            {
                return null;
            }
        }

        public async Task<JsonResult> ConsultaCep(string cep)
        {
            var cepCorreios = new CepCorreios();
            try
            {
                var service = new AtendeClienteClient();
                var endereco = await service.consultaCEPAsync(cep);
                service.Close();
                if (endereco.@return.end.IsNullOrEmpty())
                {
                    throw new UserFriendlyException(L("CepInvalido"));
                }
                cepCorreios.Bairro = endereco.@return.bairro;
                cepCorreios.Cidade = endereco.@return.cidade;
                cepCorreios.Uf = endereco.@return.uf;
                cepCorreios.Cep = endereco.@return.cep;
                cepCorreios.Complemento = endereco.@return.complemento;
                cepCorreios.Complemento2 = endereco.@return.complemento2;
                cepCorreios.End = endereco.@return.end;
                cepCorreios.Bairro = endereco.@return.bairro;

                //procurando o estado
                var estado = await _estadoAppService.Obter(cepCorreios.Uf);

                if (estado == null)
                {
                    //estado não existe, cadastrar
                    var input = new EstadoDto();
                    input.PaisId = 1; //Brasil
                    input.Nome = cepCorreios.Uf;
                    input.Uf = cepCorreios.Uf;
                    await _estadoAppService.CriarOuEditar(input);
                    estado = await _estadoAppService.Obter(cepCorreios.Uf);
                }
                cepCorreios.EstadoId = estado.Id;
                cepCorreios.PaisId = estado.PaisId;

                //procurando a cidade
                var cidade = await _cidadeAppService.ObterComEstado(cepCorreios.Cidade, estado.Id);
                if (cidade == null)
                {
                    //se não existir, incluir
                    var input = new CidadeDto();
                    input.Capital = false;
                    input.EstadoId = estado.Id;
                    input.Nome = cepCorreios.Cidade;
                    await _cidadeAppService.CriarOuEditar(input);
                    cidade = await _cidadeAppService.ObterComEstado(cepCorreios.Cidade, estado.Id);
                }
                cepCorreios.CidadeId = cidade.Id;
            }
            catch (System.Exception ex)
            {
                return Json(string.Format("ERRO: {0}", ex.Message), "application/json; charset=UTF-8", JsonRequestBehavior.AllowGet);
            }
            return Json(cepCorreios, "application/json; charset=UTF-8", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> MedicosPorEspecialidade(long id)
        {
            var medicos = await _medicoAppService.ListarPorEspecialidade(id);
            return Json(medicos, "application/json;charset=UTF-8", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarMedicoEspecialidade(MedicoEspecialidadeDto medicoEspecialidade)
        {
            await _medicoEspecialidadeAppService.CriarOuEditar(medicoEspecialidade);
            return Content(L("Sucesso"));
        }

        public async Task<ActionResult> ExcluirMedicoEspecialidade(long id)
        {
            var medicoEspecialidade = await _medicoEspecialidadeAppService.Obter(id);
            await _medicoEspecialidadeAppService.Excluir(medicoEspecialidade);
            return Content(L("Sucesso"));
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _medicoAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}