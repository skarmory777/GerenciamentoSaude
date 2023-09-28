#region Usings
using Abp.Dependency;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Religioes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class PacientesController : SWMANAGERControllerBase
    {
        #region Dependencias
        //private readonly IPacienteAppService       _pacienteAppService;
        //private readonly ISexoAppService           _sexoAppService;
        //private readonly IEscolaridadeAppService   _escolaridadeAppService;
        //private readonly ICorPeleAppService        _corPeleAppService;
        //private readonly IReligiaoAppService       _religiaoAppService;
        //private readonly IEstadoCivilAppService    _estadoCivilAppService;
        //private readonly ITipoTelefoneAppService   _tipoTelefoneAppService;
        //private readonly IPacientePesoAppService   _pacientePesoAppService;
        //private readonly ITipoLogradouroAppService _tipoLogradouroAppService;
        //private readonly INacionalidadeAppService  _nacionalidadeAppService;
        //private readonly ITipoSanguineoAppService  _tipoSanguineoAppService;
        //private readonly INaturalidadeAppService   _naturalilidadeAppService;
        //private readonly IProfissaoAppService      _profissaoAppService;

        //public PacientesController(
        //    IPacienteAppService       pacienteAppService,
        //    ISexoAppService           sexoAppService,
        //    IEscolaridadeAppService   escolaridadeAppService,
        //    ICorPeleAppService        corPeleAppService,
        //    IReligiaoAppService       religiaoAppService,
        //    IEstadoCivilAppService    estadoCivilAppService,
        //    ITipoTelefoneAppService   tipoTelefoneAppService,
        //    IPacientePesoAppService   pacientePesoAppService,
        //    ITipoLogradouroAppService tipoLogradouroAppService,
        //    INacionalidadeAppService  nacionalidadeAppService,
        //    ITipoSanguineoAppService  tipoSanguineoAppService,
        //    INaturalidadeAppService   naturalilidadeAppService,
        //    IProfissaoAppService      profissaoAppService
        //    )
        //{
        //    _pacienteAppService       = pacienteAppService;
        //    _sexoAppService           = sexoAppService;
        //    _escolaridadeAppService   = escolaridadeAppService;
        //    _corPeleAppService        = corPeleAppService;
        //    _religiaoAppService       = religiaoAppService;
        //    _estadoCivilAppService    = estadoCivilAppService;
        //    _tipoTelefoneAppService   = tipoTelefoneAppService;
        //    _pacientePesoAppService   = pacientePesoAppService;
        //    _tipoLogradouroAppService = tipoLogradouroAppService;
        //    _nacionalidadeAppService  = nacionalidadeAppService;
        //    _tipoSanguineoAppService  = tipoSanguineoAppService;
        //    _naturalilidadeAppService = naturalilidadeAppService;
        //    _profissaoAppService      = profissaoAppService;
        //}
        #endregion dependencias.

        bool IsAtendimento { get; set; }

        public ActionResult Index()
        {
            var model = new PacientesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/Index.cshtml", model);
        }

        public ActionResult PacienteParcialModal()
        {
            var model = new PacientesViewModel();
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_PacienteParcial.cshtml", model);
        }


        public async Task<PartialViewResult> CriarOuEditarModalAtendimento(long? id, string nomePaciente = null, string cpf = null, DateTime? dataNascimento = null, string telefone = null, long? abaAtendimentoId = null)
        {

            IsAtendimento = true;

            return await this.CriarOuEditarModal(null, nomePaciente, cpf, dataNascimento, telefone, abaAtendimentoId).ConfigureAwait(false);
        }

        public async Task<PartialViewResult> EditarModalAtendimento(long id)
        {

            IsAtendimento = true;

            return await this.CriarOuEditarModal(id).ConfigureAwait(false);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Paciente_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Paciente_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id, string nomePaciente = null, string cpf = null, DateTime? dataNascimento = null, string telefone = null, long? abaAtendimentoId = null)
        {
            CriarOuEditarPacienteModalViewModel viewModel;

            if (id.HasValue && id != 0)
            {
                using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
                {

                    var output = await pacienteAppService.Object.Obter2((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPacienteModalViewModel(output);

                    if (output?.SisPessoa?.Enderecos.Count > 0)
                    {
                        viewModel.Cep = output.SisPessoa.Enderecos[0].Cep;
                        viewModel.Bairro = output.SisPessoa.Enderecos[0].Bairro;
                        viewModel.Cidade = output.SisPessoa.Enderecos[0].Cidade;
                        viewModel.CidadeId = output.SisPessoa.Enderecos[0].CidadeId;
                        viewModel.Estado = output.SisPessoa.Enderecos[0].Estado;
                        viewModel.EstadoId = output.SisPessoa.Enderecos[0].EstadoId;
                        viewModel.Pais = output.SisPessoa.Enderecos[0].Pais;
                        viewModel.PaisId = output.SisPessoa.Enderecos[0].PaisId;
                        viewModel.Numero = output.SisPessoa.Enderecos[0].Numero;
                        viewModel.Complemento = output.SisPessoa.Enderecos[0].Complemento;
                        viewModel.TipoLogradouro = output.SisPessoa.Enderecos[0].TipoLogradouro;
                        viewModel.TipoLogradouroId = output.SisPessoa.Enderecos[0].TipoLogradouroId;
                    }

                    if (viewModel.Nascimento == null)
                    {
                        viewModel.Nascimento = new DateTime();
                    }
                }

            }
            else
            {
                var pacienteDto = new PacienteDto
                {
                    Cidade = new CidadeDto(),
                    Estado = new EstadoDto(),
                    Pais = new PaisDto()
                };

                viewModel = new CriarOuEditarPacienteModalViewModel(pacienteDto);

                viewModel.Id = 0;
                viewModel.CodigoPaciente = 0;
                viewModel.Prontuario = 0;
                viewModel.Observacao = "";
                viewModel.IsDoador = false;
                viewModel.Cns = 0;
                viewModel.Indicacao = "";
                viewModel.TipoSanguineoId = 0;
                viewModel.TipoSanguineo = new TipoSanguineoDto();
                viewModel.PacientePesos = new List<PacientePesoDto>();
                viewModel.SisPessoaId = 0;
                viewModel.SisPessoa = new SisPessoaDto();
                viewModel.NomeCompleto = nomePaciente ?? "";
                viewModel.Nascimento = dataNascimento ?? new DateTime();
                viewModel.Sexo = new SexoDto();
                viewModel.SexoId = 0;
                viewModel.CorPele = new CorPeleDto();
                viewModel.CorPeleId = 0;
                viewModel.Profissao = new ProfissaoDto();
                viewModel.ProfissaoId = 0;
                viewModel.Escolaridade = new EscolaridadeDto();
                viewModel.EscolaridadeId = 0;
                viewModel.Rg = "";
                viewModel.Emissor = "";
                viewModel.Emissao = null;
                viewModel.Cpf = cpf ?? "";
                viewModel.Naturalidade = new NaturalidadeDto();
                viewModel.NaturalidadeId = 0;
                viewModel.NacionalidadeId = 0;
                viewModel.Nacionalidade = new NacionalidadeDto();
                viewModel.EstadoCivil = new EstadoCivilDto();
                viewModel.EstadoCivilId = 0;
                viewModel.NomeMae = "";
                viewModel.NomePai = "";
                viewModel.Religiao = new ReligiaoDto();
                viewModel.ReligiaoId = 0;
                viewModel.Foto = new byte[] { };
                viewModel.FotoMimeType = "";
                viewModel.Email = "";
                viewModel.Email2 = "";
                viewModel.Email3 = "";
                viewModel.Email4 = "";
                viewModel.Telefone1 = telefone ?? "";
                viewModel.TipoTelefone1 = new TipoTelefoneDto();
                viewModel.TipoTelefone1Id = 0;
                viewModel.DddTelefone1 = 0;
                viewModel.Telefone2 = "";
                viewModel.TipoTelefone2 = new TipoTelefoneDto();
                viewModel.TipoTelefone2Id = 0;
                viewModel.DddTelefone2 = 0;
                viewModel.Telefone3 = "";
                viewModel.TipoTelefone3 = new TipoTelefoneDto();
                viewModel.TipoTelefone3Id = 0;
                viewModel.DddTelefone3 = 0;
                viewModel.Telefone4 = "";
                viewModel.TipoTelefone4 = new TipoTelefoneDto();
                viewModel.TipoTelefone4Id = 0;
                viewModel.DddTelefone4 = 0;
                viewModel.Cep = "";
                viewModel.Cidade = new CidadeDto();
                viewModel.CidadeId = 0;
                viewModel.Complemento = "";
                viewModel.Estado = new EstadoDto();
                viewModel.EstadoId = 0;
                viewModel.Pais = new PaisDto();
                viewModel.PaisId = 0;
                viewModel.Logradouro = "";
                viewModel.Numero = "";
                viewModel.TipoLogradouroId = 0;
                viewModel.TipoLogradouro = new TipoLogradouroDto();
                viewModel.Bairro = "";
            }

            viewModel.IsAtendimento = IsAtendimento;
            viewModel.AbaAtendimentoId = abaAtendimentoId ?? 0;


            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_CriarOuEditarModal.cshtml", viewModel);
        }

        public ActionResult ObterIdade(DateTime data)
        {
            var idade = DateDifference.GetExtendedDifference(data);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_ObterIdade.cshtml", idade);
        }

        public PartialViewResult _CarregarFoto()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_CarregarFoto.cshtml");
        }

        public async Task<ActionResult> _CriarOuEditarPacientePesoModal(long pacienteId, long? id)
        {
            CriarOuEditarPacientePesoModalViewModel viewModel;
            if (id.HasValue)
            {
                using (var pacientePesoAppService = IocManager.Instance.ResolveAsDisposable<IPacientePesoAppService>())
                {
                    var output = await pacientePesoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPacientePesoModalViewModel(output);
                }
            }
            else
            {
                viewModel = new CriarOuEditarPacientePesoModalViewModel(new PacientePesoDto())
                {
                    PacienteId = pacienteId
                };
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_CriarOuEditarPacientePesoModal.cshtml", viewModel);
        }


        public async Task<ActionResult> PacientePesosCriarOuEditar(long pacienteId, long? id)
        {
            CriarOuEditarPacientePesoModalViewModel viewModel;
            if (id.HasValue)
            {
                using (var _pacientePesoAppService = IocManager.Instance.ResolveAsDisposable<IPacientePesoAppService>())
                {
                    var output = await _pacientePesoAppService.Object.Obter((long)id).ConfigureAwait(false);
                    viewModel = new CriarOuEditarPacientePesoModalViewModel(output);
                }
            }
            else
            {
                viewModel = new CriarOuEditarPacientePesoModalViewModel(new PacientePesoDto())
                {
                    PacienteId = pacienteId
                };
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/PacientePesosCriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<ActionResult> _PacientePesos(long id)
        {
            using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
            {
                var result = await pacienteAppService.Object.Obter(id).ConfigureAwait(false);
                var pacientePesos = result.PacientePesos.ToList();
                var viewModel = new PacientePesosViewModel { PacientePesos = pacientePesos, PacienteId = id };
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/_PacientePesos.cshtml", viewModel);
            }
        }


        public async Task<ActionResult> CadastroPacientePeso(long id)
        {
            using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
            {
                var output = await pacienteAppService.Object.Obter2(id).ConfigureAwait(false);
                var viewModel = new CriarOuEditarPacienteModalViewModel(output) { IsAtendimento = this.IsAtendimento };

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/PacientePesosModal.cshtml",
                    viewModel);
            }
        }

        public async Task<FileContentResult> ObterFotoPaciente(long id)
        {
            using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
            {
                var pacienteDto = await pacienteAppService.Object.Obter(id).ConfigureAwait(false);
                if (pacienteDto.Foto.Length > 0)
                {
                    return File(pacienteDto.Foto, pacienteDto.FotoMimeType);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            using (var pacienteAppService = IocManager.Instance.ResolveAsDisposable<IPacienteAppService>())
            {
                var query = await pacienteAppService.Object.ListarAutoComplete(term).ConfigureAwait(false);
                var result = query.Select(m => new { m.Id, m.NomeCompleto, m.Nascimento }).ToList();
                return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SalvarPacientePeso(PacientePesoDto pacientePeso)
        {
            using (var pacientePesoAppService = IocManager.Instance.ResolveAsDisposable<IPacientePesoAppService>())
            {
                await pacientePesoAppService.Object.CriarOuEditar(pacientePeso).ConfigureAwait(false);
                return Content(L("Sucesso"));
            }
        }

        public async Task<ActionResult> ExcluirPacientePeso(long id)
        {
            using (var pacientePesoAppService = IocManager.Instance.ResolveAsDisposable<IPacientePesoAppService>())
            {
                var pacientePeso = await pacientePesoAppService.Object.Obter(id).ConfigureAwait(false);
                await pacientePesoAppService.Object.Excluir(pacientePeso).ConfigureAwait(false);
                return Content(L("Sucesso"));
            }
        }

        public ActionResult ListarResumo()
        {
            var model = new VWTesteViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Pacientes/VWTeste.cshtml", model);
        }
    }
}