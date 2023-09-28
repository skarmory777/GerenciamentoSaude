using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ExamesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos;
using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Laboratorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.Core.Internal;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Laboratorios
{
    public class LaboratorioPainelController : Controller
    {


        // GET: Mpa/LaboratorioPainel
        public async Task<ActionResult> Index()
        {
            using (var unidadeOrganizacionalAppService =
                IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
            {
                var viewModel = new LaboratorioPainelIndexViewModel
                {
                    UnidadeOrganizacionais = new List<UnidadeOrganizacionalDto> { await unidadeOrganizacionalAppService.Object.Obter(7).ConfigureAwait(false) }
                };

                viewModel.UnidadeOrganizacionais.AddRange((await unidadeOrganizacionalAppService.Object.ListarParaInternacao().ConfigureAwait(false)).Items
                    .ToList());
                
                return View("~/Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Index.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> Detalhamento(long id)
        {
            using(var resultadoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoAppService>())
            using(var ResultadoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame,long>>())
            using (var solicitacaoExameAppService =
                IocManager.Instance.ResolveAsDisposable<ISolicitacaoExameAppService>())
            {
                var solicitacaoExame = await solicitacaoExameAppService.Object.Obter(id);
                var labResultado = await resultadoAppService.Object.ObterPorSolicitacaoExameId(id);
                var viewModel = new LaboratorioIndexDetalhamentoViewModel
                {
                    SolicitacaoExame = solicitacaoExame,
                    AtendimentoId = solicitacaoExame.AtendimentoId ?? 0,
                    SolicitacaoExameId = id,
                    PacienteId = solicitacaoExame.Atendimento?.PacienteId ?? 0,
                    LabResultado = labResultado,
                    LabResultadoId = labResultado?.Id,
                    QtdExamesColeta = (labResultado!= null && labResultado.Id != 0) ? ResultadoExameRepository.Object.Count(x=> x.ResultadoId == labResultado.Id) : 0
                };
                

                return View("~/Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Detalhamento.cshtml", viewModel);
            }
        }
        
        public async Task<ActionResult> BaixaExames(long resultadoId, List<long> resultadoExameIds)
        {
            using (var resultadoExameAppService = IocManager.Instance.ResolveAsDisposable<IResultadoExameAppService>())
            {
                var viewModel = new LaboratorioBaixaDetalhamentoViewModel
                {
                    ResultadoId = resultadoId,
                    Items = await resultadoExameAppService.Object.ObterResultadoExames(resultadoId, resultadoExameIds)
                };
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Baixas/BaixasModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> DetalhamentoExame(long resultadoId, long resultadoExameId)
        {
            using (var resultadoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoAppService>())
            using (var resultadoExameAppService = IocManager.Instance.ResolveAsDisposable<IResultadoExameAppService>())
            {
                var viewModel = new LaboratorioDetalhamentoExameViewModel
                {
                    ResultadoId = resultadoId,
                    Resultado = await resultadoAppService.Object.Obter(resultadoId),
                    ResultadoExameId = resultadoExameId,
                    ResultadoExame = await resultadoExameAppService.Object.Obter(resultadoExameId) 
                };
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/DetalhamentoExame/DetalhamentoExameModal.cshtml", viewModel);
            }
            
        }
        
        public async Task<ActionResult> AdicionarPendencia(long resultadoId, List<long> resultadoExameIds)
        {
            using (var resultadoExameAppService = IocManager.Instance.ResolveAsDisposable<IResultadoExameAppService>())
            {
                var viewModel = new LaboratorioBaixaDetalhamentoViewModel
                {
                    ResultadoId = resultadoId,
                    Items = await resultadoExameAppService.Object.ObterResultadoExames(resultadoId, resultadoExameIds)
                };
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Pendencias/PendenciaModal.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> ImprimirEtiquetas(long resultadoId, List<long> resultadoExameIds)
        {
            using (var resultadoExameAppService = IocManager.Instance.ResolveAsDisposable<IResultadoExameAppService>())
            {
                var viewModel = new LaboratorioImprimirEtiquetaDetalhamentoViewModel
                {
                    ResultadoId = resultadoId,
                    Items = await resultadoExameAppService.Object.ObterResultadoExames(resultadoId, resultadoExameIds)
                };
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Etiquetas/ImprimirEtiqueta.cshtml", viewModel);
            }
        }
        
        public async Task<ActionResult> ImprimirEtiquetasPorSolicitacao(long resultadoId, List<long> solicitacaoExameItems)
        {
            using (var resultadoExameAppService = IocManager.Instance.ResolveAsDisposable<IResultadoExameAppService>())
            {
                var viewModel = new LaboratorioImprimirEtiquetaDetalhamentoViewModel
                {
                    ResultadoId = resultadoId,
                    Items = await resultadoExameAppService.Object.ObterResultadoExamesPorSolicitacaoExames(resultadoId, solicitacaoExameItems)
                };
                return PartialView("~/Areas/Mpa/Views/Aplicacao/Laboratorios/Painel/Etiquetas/ImprimirEtiqueta.cshtml", viewModel);
            }
        }


    }
}