using Abp.Web.Mvc.Authorization;

using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Guias.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.Guias;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class GuiasFinalController : SWMANAGERControllerBase
    {
        #region Injecao e Construtor

        private readonly IGuiaAppService _guiaAppService;
        private readonly IGuiaCampoAppService _guiaCampoAppService;
        private readonly IRelacaoGuiaCampoAppService _relacaoGuiaCampoAppService;

        public GuiasFinalController(
            IGuiaAppService guiaAppService,
            IGuiaCampoAppService guiaCampoAppService,
            IRelacaoGuiaCampoAppService relacaoGuiaCampoAppService
            )
        {
            _guiaAppService = guiaAppService;
            _guiaCampoAppService = guiaCampoAppService;
            _relacaoGuiaCampoAppService = relacaoGuiaCampoAppService;
        }

        #endregion

        #region Index e Modal

        public ActionResult Index()
        {
            var model = new GuiasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {


            var guias = await _guiaAppService.ListarTodos();
            CriarOuEditarGuiaModalViewModel viewModel;


            if (id.HasValue)
            {
                var output = await _guiaAppService.Obter((long)id);
                viewModel = new CriarOuEditarGuiaModalViewModel(output);
                // HashSet<string> camposDescricoes = new HashSet<string>();
                // foreach (var campo in output.Campos)
                //{
                //    camposDescricoes.Add(campo.GuiaCampo.Descricao);
                //}
                // viewModel.Campos = output.Campos;
                // var propriedades = ListarPropriedades(camposDescricoes);
                // viewModel.Propriedades = new SelectList();
                viewModel.Guias = new SelectList(guias.Items, "Id", "Descricao", output.Descricao);
                viewModel.Contador = 1;
            }
            else
            {
                viewModel = new CriarOuEditarGuiaModalViewModel(new CriarOuEditarGuia());
                // HashSet<string> camposDescricoes = new HashSet<string>();
                // foreach (var campo in output.Campos)
                //{
                //    camposDescricoes.Add(campo.GuiaCampo.Descricao);
                //}
                // viewModel.Campos = output.Campos;
                // var propriedades = ListarPropriedades(camposDescricoes);
                // viewModel.Propriedades = new SelectList();
                viewModel.Guias = new SelectList(guias.Items, "Id", "Descricao");
                viewModel.Contador = 1;
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_GuiaTipos_Edit)]
        public async Task<ActionResult> CoordenadaModal(long? id)
        {
            var guias = await _guiaAppService.ListarTodos();
            CriarOuEditarGuiaModalViewModel viewModel;
            var output = await _guiaAppService.Obter((long)id);
            viewModel = new CriarOuEditarGuiaModalViewModel(output);
            viewModel.Guias = new SelectList(guias.Items, "Id", "Descricao", output.Descricao);
            viewModel.Contador = 1;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_CoordenadasModal.cshtml", viewModel);
        }

        #endregion

        #region Parciais

        public ActionResult _CampoNovo(int contador)
        {
            CampoViewModel viewModel;
            viewModel = new CampoViewModel(new GuiaCampoDto());
            var propriedades = ListarPropriedades();
            viewModel.Propriedades = new SelectList(propriedades.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome");
            viewModel.Contador = contador;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_CampoNovo.cshtml", viewModel);
        }

        public ActionResult _Campo(int contador, GuiaCampoDto campo)
        {
            CampoViewModel viewModel;
            viewModel = new CampoViewModel(campo);
            var propriedades = ListarPropriedades();
            viewModel.PropriedadeSelecionada = propriedades.FirstOrDefault(p => p.Nome == campo.Descricao);
            long? selecionadoId = viewModel.PropriedadeSelecionada.Id;
            if (selecionadoId.HasValue)
            {
                viewModel.Propriedades = new SelectList(propriedades.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome", selecionadoId);
            }
            else
            {
                viewModel.Propriedades = new SelectList(propriedades.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome");
            }
            viewModel.Contador = contador;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_Campo.cshtml", viewModel);
        }

        public ActionResult _SubCampo(int contador, GuiaCampoDto subCampo)
        {
            CampoViewModel viewModel;
            viewModel = new CampoViewModel(subCampo);
            var propriedades = ListarPropriedades();
            viewModel.PropriedadeSelecionada = propriedades.FirstOrDefault(p => p.Nome == subCampo.Descricao);
            viewModel.Propriedades = new SelectList(propriedades.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome", viewModel.PropriedadeSelecionada.Id);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_SubCampo.cshtml", viewModel);
        }

        public ActionResult _SubCampoNovo(int contador)
        {
            CampoViewModel viewModel;
            viewModel = new CampoViewModel(new GuiaCampoDto());
            var propriedades = ListarPropriedades();
            viewModel.Propriedades = new SelectList(propriedades.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome");
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_SubCampoNovo.cshtml", viewModel);
        }


        public ActionResult _CampoCoordenada(int contador, GuiaCampoDto campo)
        {
            CampoViewModel viewModel;
            viewModel = new CampoViewModel(campo);
            var propriedades = ListarPropriedades();
            viewModel.PropriedadeSelecionada = propriedades.FirstOrDefault(p => p.Nome == campo.Descricao);
            long? selecionadoId = viewModel.PropriedadeSelecionada.Id;
            if (selecionadoId.HasValue)
            {
                viewModel.Propriedades = new SelectList(propriedades.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome", selecionadoId);
            }
            else
            {
                viewModel.Propriedades = new SelectList(propriedades.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome");
            }
            viewModel.Contador = contador;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_CampoCoordenada.cshtml", viewModel);
        }

        public ActionResult _SubCampoCoordenada(int contador, GuiaCampoDto subCampo)
        {
            CampoViewModel viewModel;
            viewModel = new CampoViewModel(subCampo);
            var propriedades = ListarPropriedades();
            viewModel.PropriedadeSelecionada = propriedades.FirstOrDefault(p => p.Nome == subCampo.Descricao);
            viewModel.Propriedades = new SelectList(propriedades.Select(m => new { Id = m.Id, Nome = m.Nome }), "Id", "Nome", viewModel.PropriedadeSelecionada.Id);
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GuiasFinal/_SubCampoCoordenada.cshtml", viewModel);
        }

        #endregion

        #region Metodos auxiliares

        public List<GenericoIdNome> ListarPropriedades()
        {
            List<GenericoIdNome> propriedades = new List<GenericoIdNome>();
            Paciente paciente = new Paciente();
            Type pacienteTipo = paciente.GetType();
            Assembly assembly = Assembly.GetAssembly(pacienteTipo);
            IEnumerable<Type> classes = assembly.GetTypes().Where(t => t.IsClass);
            int contador = 0;
            foreach (var classe in classes)
            {
                // temp
                if (classe.Namespace == null || !classe.Namespace.Contains("ClassesAplicacao") || classe.Name.Contains("<") || classe.Name.Contains("CRUD") || classe.Name.Contains("Mailing"))
                    continue;

                var x = new GenericoIdNome();
                x.Id = contador++;
                x.Nome = classe.Name;

                propriedades.Add(x);

                foreach (var tipo in classe.GetProperties())
                {
                    string descricao = classe.Name + "." + tipo.Name;
                    var y = new GenericoIdNome();
                    y.Id = contador++;
                    y.Nome = descricao;
                    propriedades.Add(y);
                }
            }

            return propriedades;
        }

        #endregion
    }
}