using Abp.Extensions;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Cidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto;
using SW10.SWMANAGER.CorreiosService;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Cep;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class CepsController : SWMANAGERControllerBase
    {
        private readonly ICepAppService _cepAppService;
        private readonly IPaisAppService _paisAppService;
        private readonly IEstadoAppService _estadoAppService;
        private readonly ICidadeAppService _cidadeAppService;
        private readonly ITipoLogradouroAppService _tipoLogradouroAppService;

        public CepsController(ICepAppService cepAppService, IPaisAppService paisAppService, IEstadoAppService estadoAppService, ICidadeAppService cidadeAppService, ITipoLogradouroAppService tipoLogradouroAppService)
        {
            _cepAppService = cepAppService;
            _paisAppService = paisAppService;
            _estadoAppService = estadoAppService;
            _cidadeAppService = cidadeAppService;
            _tipoLogradouroAppService = tipoLogradouroAppService;
        }

        // GET: Mpa/Cep
        [AcceptVerbs("GET", "POST")]
        public async Task<ActionResult> Index()
        {
            var paises = await _paisAppService.Listar(new ListarPaisesInput { MaxResultCount = 50 });
            var estados = await _estadoAppService.Listar(new ListarEstadosInput { MaxResultCount = 50 });
            var cidades = await _cidadeAppService.Listar(new ListarCidadesInput { MaxResultCount = 50 });
            var tiposLogradouro = await _tipoLogradouroAppService.Listar(new ListarTiposLogradouroInput { MaxResultCount = 50 });

            var model = new CepsViewModel();
            model.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = m.Nome }));
            model.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Uf) }), "Id", "Nome");
            model.Cidades = new SelectList(cidades.Items.Select(m => new { Id = m.Id, Nome = m.Nome }));
            model.TiposLogradouro = new SelectList(tiposLogradouro.Items.Select(m => new { Id = m.Id, Nome = m.Descricao }));


            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Ceps/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cep_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Cep_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarCepModalViewModel viewModel;

            var paises = await _paisAppService.ListarTodos();
            var estados = await _estadoAppService.ListarPorPais(0);
            var cidades = await _cidadeAppService.ListarPorEstado(0);
            var tiposLogradouro = await _tipoLogradouroAppService.ListarTodos();




            if (id.HasValue)
            {
                var output = await _cepAppService.Obter((long)id);
                viewModel = new CriarOuEditarCepModalViewModel(output);
                viewModel.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Uf) }), "Id", "Nome", output.EstadoId);
                viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome", output.PaisId);
                viewModel.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Uf) }), "Id", "Nome", output.EstadoId);
                viewModel.Cidades = new SelectList(cidades.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Id, m.Nome) }), "Id", "Nome", output.CidadeId);
                viewModel.TiposLogradouro = new SelectList(tiposLogradouro.Items.Select(m => new { Id = m.Id, Nome = m.Descricao }), "Id", "Nome", output.TipoLogradouroId);
            }
            else
            {
                viewModel = new CriarOuEditarCepModalViewModel(new CriarOuEditarCep());
                viewModel.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Uf) }), "Id", "Nome");
                viewModel.Paises = new SelectList(paises.Items.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome");
                viewModel.Estados = new SelectList(estados.Items.Select(m => new { Id = m.Id, Nome = string.Format("{0} ({1})", m.Nome, m.Uf) }), "Id", "Nome");
                viewModel.Cidades = new SelectList(cidades.Items.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome");
                viewModel.TiposLogradouro = new SelectList(tiposLogradouro.Items.Select(m => new { Id = m.Id, Nome = m.Descricao }), "Id", "Nome");
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Ceps/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> ConsultaCep(string cep)
        {
            var cepCorreios = new CepCorreios();
            try
            {
                // VERIFICAR SE EXISTE O CEP NO BANCO LOCAL

                var cepNaBase = await _cepAppService.Obter(cep);

                if (cepNaBase != null)
                {
                    //cepCorreios.Bairro = cepNaBase.Bairro;
                    //cepCorreios.Cep = cepNaBase.CEP;
                    //cepCorreios.Cidade = cepNaBase.Cidade.Nome;
                    //cepCorreios.CidadeId = cepNaBase.CidadeId;
                    //cepCorreios.Complemento = cepNaBase.Complemento;
                    //cepCorreios.Complemento2 = cepNaBase.Complemento2;
                    //cepCorreios.CreationTime = cepNaBase.CreationTime;
                    //cepCorreios.CreatorUserId = cepNaBase.CreatorUserId;
                    //cepCorreios.DeleterUserId = cepNaBase.DeleterUserId;
                    //cepCorreios.DeletionTime = cepNaBase.DeletionTime;
                    //cepCorreios.End = string.Format("{0} {1}", cepNaBase.TipoLogradouro.Descricao, cepNaBase.Logradouro);
                    //cepCorreios.EstadoId = cepNaBase.EstadoId;
                    //cepCorreios.Id = cepNaBase.Id;
                    //cepCorreios.IsDeleted = cepNaBase.IsDeleted;
                    //cepCorreios.IsSistema = cepNaBase.IsSistema;
                    //cepCorreios.LastModificationTime = cepNaBase.LastModificationTime;
                    //cepCorreios.LastModifierUserId = cepNaBase.LastModifierUserId;
                    //cepCorreios.PaisId = cepNaBase.PaisId;
                    //cepCorreios.Uf = cepNaBase.Estado.Uf;
                    //cepCorreios.UnidadesPostagem = cepNaBase.UnidadePostagem;
                    return Json(cepNaBase, "application/json; charset=UTF-8", JsonRequestBehavior.AllowGet);
                }
                else
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
                        var inputEstado = new EstadoDto();
                        inputEstado.PaisId = 1; //Brasil
                        inputEstado.Nome = cepCorreios.Uf;
                        inputEstado.Uf = cepCorreios.Uf;
                        inputEstado.CreatorUserId = AbpSession.UserId;

                        await _estadoAppService.CriarOuEditar(inputEstado);
                        estado = await _estadoAppService.Obter(cepCorreios.Uf);
                    }
                    cepCorreios.EstadoId = estado.Id;
                    cepCorreios.PaisId = estado.PaisId;

                    //procurando a cidade
                    var cidade = await _cidadeAppService.ObterComEstado(cepCorreios.Cidade, estado.Id);
                    if (cidade == null)
                    {
                        //se não existir, incluir
                        var inputCidade = new CidadeDto();
                        inputCidade.Capital = false;
                        inputCidade.EstadoId = estado.Id;
                        inputCidade.Nome = cepCorreios.Cidade;
                        inputCidade.CreatorUserId = AbpSession.UserId;
                        await _cidadeAppService.CriarOuEditar(inputCidade);
                        cidade = await _cidadeAppService.ObterComEstado(cepCorreios.Cidade, estado.Id);
                    }
                    cepCorreios.CidadeId = cidade.Id;
                    var end = cepCorreios.End.Split(' ');
                    var logradouro = string.Empty;
                    for (int i = 1; i < end.Length; i++)
                    {
                        logradouro += string.Format("{0}{1}", i > 1 ? " " : string.Empty, end[i]);
                    }
                    var tipoLogradouro = await _tipoLogradouroAppService.Obter(end[0].ToString());
                    if (tipoLogradouro == null)
                    {
                        var inputTipoLogradouro = new CriarOuEditarTipoLogradouroDto();
                        inputTipoLogradouro.Descricao = end[0];
                        inputTipoLogradouro.Abreviacao = end[0].Substring(0, end[0].Length >= 4 ? 4 : end[0].Length);
                        inputTipoLogradouro.CreatorUserId = AbpSession.UserId;
                        tipoLogradouro = await _tipoLogradouroAppService.CriarOuEditar(inputTipoLogradouro);
                        //tipoLogradouro = await _tipoLogradouroAppService.Obter(end[0].ToString());
                    }

                    var inputCep = new CriarOuEditarCep();
                    inputCep.TipoLogradouroId = tipoLogradouro.Id;
                    inputCep.PaisId = cepCorreios.PaisId;
                    inputCep.EstadoId = cepCorreios.EstadoId;
                    inputCep.CidadeId = cepCorreios.CidadeId;
                    inputCep.CreatorUserId = AbpSession.UserId;
                    inputCep.Bairro = cepCorreios.Bairro;
                    inputCep.CEP = cep;
                    inputCep.Complemento = cepCorreios.Complemento;
                    inputCep.Complemento2 = cepCorreios.Complemento2;
                    inputCep.Logradouro = logradouro;
                    var _cep = await _cepAppService.CriarOuEditar(inputCep);
                    var cepResult = await _cepAppService.Obter(_cep.Id);
                    return Json(cepResult, "application/json; charset=UTF-8", JsonRequestBehavior.AllowGet);
                }
            }
            catch (System.Exception ex)
            {
                return Json(string.Format("ERRO: {0}", ex.Message), "application/json; charset=UTF-8", JsonRequestBehavior.AllowGet);
            }
            //return Json(cepCorreios, "application/json; charset=UTF-8", JsonRequestBehavior.AllowGet);
        }

        //public async Task<JsonResult> AutoComplete(string term, long? estadoId)
        //{
        //    var query = await _cepAppService.ListarAutoComplete(term, estadoId);
        //    //var result = query.Items.Select (m => new { m.Id,m.Nome }).ToList ();
        //    //return Json (result.ToArray (),JsonRequestBehavior.AllowGet);
        //    return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        //}
    }
}