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
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposPessoa;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.TiposTelefone;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Enderecos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Naturalidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pessoas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Profissoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.Web.Controllers;
using SW10.SWMANAGER.Web.CorreiosService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FornecedoresController : SWMANAGERControllerBase
    {
        private readonly IFornecedorAppService _fornecedorAppService;
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
        private readonly ITipoPessoaAppService _tipoPessoaAppService;
        private readonly IConvenioAppService _convenioAppService;
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IMedicoAppService _medicoAppService;
        private readonly IEmpresaAppService _empresaAppService;
        private readonly ITipoLogradouroAppService _tipoLogradouroAppService;

        public FornecedoresController(
            IFornecedorAppService fornecedorAppService,
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
            ITipoPessoaAppService tipoPessoaAppService,
            IPacienteAppService pacienteAppService,
            IConvenioAppService convenioAppService,
            IMedicoAppService medicoAppService,
            IEmpresaAppService empresaAppService,
            ITipoLogradouroAppService tipoLogradouroAppService
            )
        {
            _fornecedorAppService = fornecedorAppService;
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
            _tipoPessoaAppService = tipoPessoaAppService;
            _pacienteAppService = pacienteAppService;
            _convenioAppService = convenioAppService;
            _medicoAppService = medicoAppService;
            _empresaAppService = empresaAppService;
            _tipoLogradouroAppService = tipoLogradouroAppService;
        }

        public ActionResult Index()
        {
            var model = new FornecedoresViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Fornecedor_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarFornecedorModalViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new CriarOuEditarFornecedorModalViewModel(new SisFornecedorDto() { SisPessoa = new SisPessoaDto() });
                viewModel.Enderecos = JsonConvert.SerializeObject(new List<EnderecoDto>());
            }
            else
            {
                var fornecedorDto = await _fornecedorAppService.Obter((long)id);

                viewModel = new CriarOuEditarFornecedorModalViewModel(fornecedorDto);
                viewModel.Enderecos = JsonConvert.SerializeObject(fornecedorDto.SisPessoa.Enderecos);
            }


            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_CriarOuEditarModal.cshtml", viewModel);
        }

        public ActionResult ObterIdade(DateTime data)
        {
            var idade = DateDifference.GetExtendedDifference(data);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_ObterIdade.cshtml", idade);
        }

        public PartialViewResult _CarregarFoto()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_CarregarFoto.cshtml");
        }

        //public async Task<ActionResult> _CriarOuEditarFornecedorEspecialidadeModal(long fornecedorId,long? id)
        //{
        //	CriarOuEditarFornecedorEspecialidadeModalViewModel viewModel; //=new CriarOuEditarFornecedorEspecialidadeModalViewModel()
        //	var fornecedor = await _fornecedorAppService.Obter(fornecedorId);
        //	var especialidades = await _especialidadeAppService.Listar();
        //	var fornecedorEspecialidades = fornecedor.FornecedorEspecialidades;
        //	var especialidadesCadastradas = fornecedorEspecialidades.Select(m => m.EspecialidadeId);

        //	var especialidadesFornecedor = especialidades.Items.Where(m => m.Id.IsIn(especialidadesCadastradas.ToArray()));
        //	var especialidadesDisponiveis = especialidades.Items.Except(especialidadesFornecedor).ToList();
        //	if(id.HasValue)
        //	{
        //		var output = await _fornecedorEspecialidadeAppService.Obter((long)id);
        //		viewModel = new CriarOuEditarFornecedorEspecialidadeModalViewModel(output);
        //		var especialidade = await _especialidadeAppService.Obter(output.EspecialidadeId);
        //		viewModel.Especialidade = especialidade;
        //		viewModel.EspecialidadeId = output.EspecialidadeId;
        //		//var include = especialidade.MapTo<EspecialidadeDto>();
        //		//especialidadesDisponiveis.Add(especialidades.Items.Where(m=>m.Id==output.EspecialidadeId).FirstOrDefault());
        //		viewModel.Especialidades = new SelectList(especialidadesDisponiveis,"Id","Nome",viewModel.EspecialidadeId);
        //	}
        //	else
        //	{
        //		viewModel = new CriarOuEditarFornecedorEspecialidadeModalViewModel(new FornecedorEspecialidadeDto());
        //		viewModel.Especialidades = new SelectList(especialidadesDisponiveis,"Id","Nome");
        //	}
        //	viewModel.Fornecedor = fornecedor;
        //	viewModel.FornecedorId = fornecedorId;
        //	return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_CriarOuEditarFornecedorEspecialidadeModal.cshtml",viewModel);
        //}

        //public async Task<ActionResult> _FornecedorEspecialidades(long id)
        //{
        //	var especialidades = await _especialidadeAppService.Listar();
        //	var result = await _fornecedorAppService.Obter(id);
        //	var fornecedorEspecialidades = result.FornecedorEspecialidades.ToList();
        //	var viewModel = new FornecedorEspecialidadesViewModel();
        //	viewModel.Especialidades = especialidades.Items.ToList();
        //	viewModel.FornecedorEspecialidades = fornecedorEspecialidades;
        //	return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_FornecedorEspecialidades.cshtml",viewModel);
        //}

        //public async Task<FileContentResult> ObterFotoFornecedor(long id)
        //{
        //	var fornecedorDto = await _fornecedorAppService.Obter(id);
        //	var fornecedor = fornecedorDto.MapTo<Fornecedor>();
        //	if(fornecedor.Foto.Length > 0)
        //	{
        //		return File(fornecedor.Foto,fornecedor.FotoMimeType);
        //	}
        //	else
        //	{
        //		return null;
        //	}
        //}

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

        //public async Task<JsonResult> FornecedoresPorEspecialidade(long id)
        //{
        //	var fornecedores = await _fornecedorAppService.ListarPorEspecialidade(id);
        //	return Json(fornecedores,"application/json;charset=UTF-8",JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> SalvarFornecedorEspecialidade(FornecedorEspecialidadeDto fornecedorEspecialidade)
        //{
        //	await _fornecedorEspecialidadeAppService.CriarOuEditar(fornecedorEspecialidade);
        //	return Content(L("Sucesso"));
        //}
        //public async Task<ActionResult> ExcluirFornecedorEspecialidade(long id)
        //{
        //	var fornecedorEspecialidade = await _fornecedorEspecialidadeAppService.Obter(id);
        //	var input = fornecedorEspecialidade.MapTo<FornecedorEspecialidadeDto>();
        //	await _fornecedorEspecialidadeAppService.Excluir(input);
        //	return Content(L("Sucesso"));
        //}

        public async Task<PartialViewResult> ExibirFornecedorPessoaFisica(long? fornecedorId)
        {
            CriarOuEditarFornecedorModalViewModel viewModel;


            /*

			var sexos = await _sexoAppService.ListarTodos();
			var coresPele = await _corPeleAppService.ListarTodos();
			var escolaridades = await _escolaridadeAppService.ListarTodos();
			var religioes = await _religiaoAppService.ListarTodos();
			var estadosCivis = await _estadoCivilAppService.ListarTodos();
			var tiposTelefone = await _tipoTelefoneAppService.ListarTodos();

			// Se for apenas criacao, nao precisa mais do if
			if(fornecedorId.HasValue)
			{
				var output = await _fornecedorAppService.Obter((long)fornecedorId);
				viewModel = new CriarOuEditarFornecedorModalViewModel(output);

				viewModel.Sexos = new SelectList(sexos.Items,"Id","Descricao");
				viewModel.CoresPele = new SelectList(coresPele.Items,"Id","Descricao");
				viewModel.Escolaridades = new SelectList(escolaridades.Items,"Id","Descricao");
				viewModel.Religioes = new SelectList(religioes.Items,"Id","Descricao");
				viewModel.EstadosCivis = new SelectList(estadosCivis.Items,"Id","Descricao");
				viewModel.TiposTelefone = new SelectList(tiposTelefone.Items,"Id","Descricao");
			}
			else
			{
				viewModel = new CriarOuEditarFornecedorModalViewModel(new CriarOuEditarFornecedor());

				viewModel.Sexos = new SelectList(sexos.Items,"Id","Descricao");
				viewModel.CoresPele = new SelectList(coresPele.Items,"Id","Descricao");
				viewModel.Escolaridades = new SelectList(escolaridades.Items,"Id","Descricao");
				viewModel.Religioes = new SelectList(religioes.Items,"Id","Descricao");
				viewModel.EstadosCivis = new SelectList(estadosCivis.Items,"Id","Descricao");
				viewModel.TiposTelefone = new SelectList(tiposTelefone.Items,"Id","Descricao");

				var pessoaFisica = new FornecedorPessoaFisicaDto();

				//pessoaFisica.Profissao = new ProfissaoDto();
				//pessoaFisica.Profissao.Id = 0;
				//pessoaFisica.Naturalidade = new NaturalidadeDto();
				//pessoaFisica.Naturalidade.Id = 0;
				pessoaFisica.Cidade = new CidadeDto();
				pessoaFisica.Cidade.Id = 0;
				pessoaFisica.Cidade.Estado = new EstadoDto();
				pessoaFisica.Cidade.Estado.Id = 0;
				pessoaFisica.Cidade.Estado.Pais = new PaisDto();
				pessoaFisica.Cidade.Estado.Pais.Id = 0;
				pessoaFisica.Estado = new EstadoDto();
				pessoaFisica.Estado.Id = 0;
				pessoaFisica.Pais = new PaisDto();
				pessoaFisica.Pais.Id = 0;
				pessoaFisica.CidadeId = 0;
				pessoaFisica.EstadoId = 0;
				pessoaFisica.PaisId = 0;
				pessoaFisica.ProfissaoId = 0;
				pessoaFisica.NaturalidadeId = 0;
				pessoaFisica.Bairro = string.Empty;
				pessoaFisica.Cep = string.Empty;

				pessoaFisica.Complemento = string.Empty;
				pessoaFisica.CorPele = 0;
				pessoaFisica.Cpf = string.Empty;
				pessoaFisica.Email = string.Empty;
				pessoaFisica.Emissao = DateTime.MinValue;
				pessoaFisica.Emissor = string.Empty;
				pessoaFisica.Logradouro = string.Empty;
				pessoaFisica.Escolaridade = 0;
				pessoaFisica.EstadoCivil = 0;
				pessoaFisica.Nascimento = DateTime.MinValue;
				pessoaFisica.Naturalidade = new NaturalidadeDto();
				pessoaFisica.NomeCompleto = string.Empty;
				pessoaFisica.NomeMae = pessoaFisica.NomePai = pessoaFisica.Numero = string.Empty;
				pessoaFisica.Religiao = 0;
				pessoaFisica.Rg = string.Empty;
				pessoaFisica.Sexo = 0;
				pessoaFisica.Telefone1 = pessoaFisica.Telefone2 = pessoaFisica.Telefone3 = pessoaFisica.Telefone4 = string.Empty;
				pessoaFisica.TipoTelefone1 = pessoaFisica.TipoTelefone2 = pessoaFisica.TipoTelefone3 = pessoaFisica.TipoTelefone4 = 0;

				viewModel.FornecedorPessoaFisica = pessoaFisica;
			}
            */

            viewModel = new CriarOuEditarFornecedorModalViewModel(new SisFornecedorDto());

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_FornecedorPessoaFisica.cshtml", viewModel);
        }

        public async Task<PartialViewResult> ExibirFornecedorPessoaJuridica(long? fornecedorId)
        {
            CriarOuEditarFornecedorModalViewModel viewModel;
            viewModel = new CriarOuEditarFornecedorModalViewModel(new SisFornecedorDto());

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_FornecedorPessoaJuridica.cshtml", viewModel);
        }

        public async Task<PartialViewResult> ExibirPessoa(long pessoaId, long tipoCadastro)
        {
            CriarOuEditarFornecedorModalViewModel viewModel;
            viewModel = new CriarOuEditarFornecedorModalViewModel(new SisFornecedorDto());
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Fornecedores/_FornecedorPessoaJuridica.cshtml", viewModel);
        }
    }
}