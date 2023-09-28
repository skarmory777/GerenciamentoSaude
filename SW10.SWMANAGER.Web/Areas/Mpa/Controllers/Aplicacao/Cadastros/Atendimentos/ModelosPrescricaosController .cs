using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.MovimentosAutomaticos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.ModelosAtestados;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Atendimentos
{
    public class ModelosPrescricaosController : SWMANAGERControllerBase
    {
        public ModelosPrescricaosController()
        {
        }

        public ActionResult Index()
        {
            var model = new ModeloAtestadoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/ModelosPrescricoes/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            AssistencialAtendimentoViewModel viewModel = new AssistencialAtendimentoViewModel();

            if (id.HasValue)
            {
                using (var modeloPrescricaoAppService = IocManager.Instance.ResolveAsDisposable<IModeloPrescricaoAppService>())
                using (var prescricaoStatusAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoStatusAppService>())
                {
                   var modelo = await modeloPrescricaoAppService.Object.Obter(id.Value).ConfigureAwait(false);

                    if (modelo != null)
                    {
                        viewModel.Atendimento = modelo.PrescricaoMedica.Atendimento;
                        viewModel.ModeloPrescricao = new ModeloPrescricaoDto { Id = modelo.Id, Codigo = modelo.Codigo, Descricao = modelo.Descricao };
                    }
                    else
                    {
                        viewModel.ModeloPrescricao = new ModeloPrescricaoDto();
                        viewModel.Atendimento = new AtendimentoDto();
                    }

                    var listaStatus = await prescricaoStatusAppService.Object.ListarTodos().ConfigureAwait(false);

                    if (listaStatus != null && listaStatus.Items != null)
                    {
                        viewModel.Atendimento.ListaStatus = listaStatus.Items.ToList();
                    }
                    else
                    {
                        viewModel.Atendimento.ListaStatus = new List<PrescricaoStatusDto>();
                    }
                }
            }
            else
            {
                viewModel.ModeloPrescricao = new ModeloPrescricaoDto();
                viewModel.Atendimento = new AtendimentoDto { Id = 12475 };
                viewModel.Atendimento.ListaStatus = new List<PrescricaoStatusDto>();
            }

            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/ModelosPrescricoes/ModeloPrescricao.cshtml", viewModel);
        }


    }
}