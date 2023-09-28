using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Web.Mvc.Authorization;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Home;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Resultados;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Castle.Core.Internal;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class ResultadosController : SWMANAGERControllerBase
    {
        public async Task<ActionResult> Index()
        {
            var userId = AbpSession.UserId.Value;

            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            {
                //kernel.Bind<IUserAppService>().To<UserAppService>();

                var userEmpresas = await userAppService.Object.GetUserEmpresas(userId).ConfigureAwait(false);
                var empresa = TempData.Peek("Empresa") as EmpresaDto;

                var model = new AtendimentoDto();
                var viewModel = new AssistenciaisViewModel(model);
                if (empresa == null || (empresa != null && empresa.Id == 0))
                {
                    viewModel.Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia");
                }
                else
                {
                    viewModel.Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia", empresa.Id);
                    viewModel.EmpresaId = empresa.Id;
                    viewModel.Empresa = empresa;
                }

                viewModel.IsAmbulatorioEmergencia = true;
                viewModel.IsInternacao = false;
                return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Resultados/Index.cshtml", viewModel);
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Resultado_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Resultado_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long? atendimentoId)
        {
            CriarOuEditarResultadoModalViewModel viewModel;
            var ambulatorioInternacao = new List<GenericoIdNome>();

            ambulatorioInternacao.Add(new GenericoIdNome { Id = 1, Nome = "Ambulatório" });
            ambulatorioInternacao.Add(new GenericoIdNome { Id = 2, Nome = "Internação" });

            if (id.HasValue)
            {
                using (var resultadoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoAppService>())
                using (var resultadoExameAppService = IocManager.Instance.ResolveAsDisposable<IResultadoExameAppService>())
                {
                    var output = await resultadoAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    viewModel = new CriarOuEditarResultadoModalViewModel(output);

                    var list = await resultadoExameAppService.Object.ListarPorResultado(id.Value).ConfigureAwait(false);
                    viewModel.ResultadosExamesList = JsonConvert.SerializeObject(list.Items.ToList());
                    viewModel.IsSolicitacao = true;

                    viewModel.AmbulatorioInternacao = output.Atendimento.IsAmbulatorioEmergencia ? 1 : 2;

                    viewModel.ListaAmbulatorioInternacao = new SelectList(
                        ambulatorioInternacao,
                        "Id",
                        "Nome",
                        viewModel.AmbulatorioInternacao);
                }
            }
            else
            {
                viewModel = new CriarOuEditarResultadoModalViewModel(new ResultadoDto());
                if (atendimentoId.HasValue)
                {
                    viewModel.AtendimentoId = atendimentoId.Value;
                }
                viewModel.ResultadosExamesList = JsonConvert.SerializeObject(new List<ResultadoExameIndexCrudDto>());
                viewModel.ListaAmbulatorioInternacao = new SelectList(ambulatorioInternacao, "Id", "Nome");
            }
            
            viewModel.PacienteNome = viewModel.Atendimento?.Paciente?.NomeCompleto;            
            TempData["AtendimentoLab"] = viewModel.Atendimento;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Resultados/_CriarOuEditarModal.cshtml", viewModel);
        }
        
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Resultado_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Resultado_Edit)]
        public async Task<ActionResult> CriarOuEditarModalPorSolicitacao(long? id, long? atendimentoId, long? solicitacaoExameId, List<long> solicitacaoExameItems)
        {
            CriarOuEditarResultadoModalViewModel viewModel;
            var ambulatorioInternacao = new List<GenericoIdNome>
            {
                new GenericoIdNome { Id = 1, Nome = "Ambulatório" },
                new GenericoIdNome { Id = 2, Nome = "Internação" }
            };

            if (id.HasValue)
            {
                using (var resultadoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoAppService>())
                using (var resultadoExameAppService = IocManager.Instance.ResolveAsDisposable<IResultadoExameAppService>())
                {
                    var output = await resultadoAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    viewModel = new CriarOuEditarResultadoModalViewModel(output);

                    var list = await resultadoExameAppService.Object.ListarPorResultado(id.Value).ConfigureAwait(false);
                    viewModel.ResultadosExamesList = JsonConvert.SerializeObject(list.Items.ToList());
                    viewModel.IsSolicitacao = true;

                    viewModel.AmbulatorioInternacao = output.Atendimento.IsAmbulatorioEmergencia ? 1 : 2;

                    viewModel.ListaAmbulatorioInternacao = new SelectList(
                        ambulatorioInternacao,
                        "Id",
                        "Nome",
                        viewModel.AmbulatorioInternacao);
                }
            }
            else
            {
                viewModel = new CriarOuEditarResultadoModalViewModel(new ResultadoDto());
                using(var medicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico,long>>())
                using (var solicitacaoExameAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoExameAppService>())
                {
                    var solicitacao = await solicitacaoExameAppService.Object.Obter(solicitacaoExameId ?? 0);
                    if (solicitacao != null)
                    {
                        if (!solicitacaoExameItems.IsNullOrEmpty())
                        {
                            var resultadoExamesList = solicitacao.SolicitacaoItens
                                .Where(x => solicitacaoExameItems.Contains(x.Id)).Select(x => ResultadoExameIndexCrudDto.Mapear(x, atendimentoId));
                            viewModel.ResultadosExamesList = JsonConvert.SerializeObject(resultadoExamesList);
                        }

                        viewModel.PacienteNome = solicitacao.Atendimento?.Paciente?.NomeCompleto;
                        viewModel.AtendimentoId = solicitacao.AtendimentoId;
                        viewModel.AmbulatorioInternacao = solicitacao.Atendimento.IsAmbulatorioEmergencia ? 1 : 2;
                        viewModel.IsUrgente = solicitacao.Prioridade == SolicitacaoExamePrioridadeDto.Urgencia;
                        viewModel.Atendimento = solicitacao.Atendimento;
                        viewModel.IsSolicitacao = true;
                        viewModel.ConvenioId = solicitacao.Atendimento.ConvenioId;
                        viewModel.Convenio = solicitacao.Atendimento.Convenio;
                        viewModel.MedicoSolicitante = solicitacao.MedicoSolicitante;
                        viewModel.MedicoSolicitanteId = solicitacao.MedicoSolicitanteId;
                        viewModel.SolicitacaoExameId = solicitacaoExameId;
                        if (viewModel.MedicoSolicitanteId.HasValue)
                        {
                            viewModel.MedicoSolicitante = MedicoDto.Mapear(await medicoRepository.Object.GetAll().Include(x=> x.SisPessoa)
                                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == viewModel.MedicoSolicitanteId));
                        }
                        
                        if (solicitacao.Atendimento.IsAmbulatorioEmergencia)
                        {
                            viewModel.LocalUtilizacao = solicitacao.Atendimento.UnidadeOrganizacional;
                            viewModel.LocalUtilizacaoId = solicitacao.Atendimento.UnidadeOrganizacionalId;
                        }
                        else
                        {
                            viewModel.TipoAcomodacao = solicitacao.Atendimento.TipoAcomodacao;
                            viewModel.TipoAcomodacaoId = solicitacao.Atendimento.TipoAcomodacaoId;
                            viewModel.LeitoAtual = solicitacao.Atendimento.Leito;
                            viewModel.LeitoAtualId = solicitacao.Atendimento.LeitoId;
                            viewModel.LocalUtilizacao = solicitacao.Atendimento.Leito?.UnidadeOrganizacional;
                            viewModel.LocalUtilizacaoId = solicitacao.Atendimento.Leito?.UnidadeOrganizacionalId;
                        }

                        viewModel.ListaAmbulatorioInternacao = new SelectList(ambulatorioInternacao, "Id", "Nome");
                    }
                    else
                    {
                       
                        if (atendimentoId.HasValue)
                        {
                            viewModel.AtendimentoId = atendimentoId.Value;
                        }

                        viewModel.ResultadosExamesList =
                            JsonConvert.SerializeObject(new List<ResultadoExameIndexCrudDto>());

                        viewModel.ListaAmbulatorioInternacao = new SelectList(ambulatorioInternacao, "Id", "Nome");
                    }
                }
            }
            
            viewModel.PacienteNome = viewModel.Atendimento?.Paciente?.NomeCompleto;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Resultados/_CriarOuEditarModalPorSolicitacao.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            using (var resultadoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoAppService>())
            {
                var query = await resultadoAppService.Object.ListarAutoComplete(term).ConfigureAwait(false);
                var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
                return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            }

            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> CriarColetaDeExamesSolicitado(string listIds)
        {
            var ids = JsonConvert.DeserializeObject<List<long>>(listIds);


            var ambulatorioInternacao = new List<GenericoIdNome>
            {
                new GenericoIdNome { Id = 1, Nome = "Ambulatório" },
                new GenericoIdNome { Id = 2, Nome = "Internação" }
            };


            CriarOuEditarResultadoModalViewModel viewModel = null;

            try
            {
                if (ids != null)
                {
                    using (var solicitacaoExameItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExameItem, long>>())
                    {
                        var solicitacoesExamesEntidades = solicitacaoExameItemRepository.Object.GetAll()
                            .Include(i => i.Solicitacao).Include(i => i.Solicitacao.MedicoSolicitante)
                            .Include(i => i.Solicitacao.MedicoSolicitante.SisPessoa)
                            .Include(i => i.Solicitacao.Atendimento).Include(i => i.Solicitacao.Atendimento.Convenio)
                            .Include(i => i.Solicitacao.Atendimento.Convenio.SisPessoa)
                            .Include(i => i.Solicitacao.Atendimento.Leito)
                            .Include(i => i.Solicitacao.Atendimento.Paciente)
                            .Include(i => i.Solicitacao.Atendimento.Paciente.SisPessoa)
                            .Include(i => i.Solicitacao.Atendimento.Medico)
                            .Include(i => i.Solicitacao.Atendimento.Medico.SisPessoa).Include(i => i.FaturamentoItem)
                            .Include(i => i.FaturamentoItem.Material).Where(m => ids.Any(a => a == m.Id)).ToList();

                        var solicitacoesExames = SolicitacaoExameItemDto.Mapear(solicitacoesExamesEntidades);

                        if (solicitacoesExames != null && solicitacoesExames.Count > 0)
                        {
                            var solicitacao = solicitacoesExames[0];
                            var resultadoDto = new ResultadoDto
                            {
                                DataColeta = DateTime.Now,
                                Atendimento = solicitacao.Solicitacao.Atendimento,
                                AtendimentoId = solicitacao.Solicitacao.Atendimento.Id,
                                PacienteNome = solicitacao.Solicitacao.Atendimento.Paciente.NomeCompleto
                            };

                            if (solicitacao.Solicitacao.MedicoSolicitante != null)
                            {
                                resultadoDto.MedicoSolicitante = new MedicoDto
                                {
                                    Id = solicitacao.Solicitacao.MedicoSolicitanteId.Value,
                                    NomeCompleto = solicitacao.Solicitacao.Atendimento.Medico.SisPessoa.NomeCompleto,
                                    SisPessoa = solicitacao.Solicitacao.MedicoSolicitante.SisPessoa
                                };

                                solicitacao.Solicitacao.MedicoSolicitanteId = solicitacao.Solicitacao.MedicoSolicitante.Id;

                                resultadoDto.NomeMedicoSolicitante = solicitacao.Solicitacao.MedicoSolicitante.SisPessoa.NomeCompleto;
                                resultadoDto.CRMSolicitante = solicitacao.Solicitacao.MedicoSolicitante.NumeroConselho.ToString();
                            }

                            resultadoDto.LeitoAtual = solicitacao.Solicitacao.Atendimento.Leito; //.MapTo<LeitoDto>();
                            resultadoDto.LeitoAtualId = solicitacao.Solicitacao.Atendimento.LeitoId;

                            if (solicitacao.Solicitacao.Atendimento.Paciente.Nascimento != null)
                            {
                                var idade = DateDifference.GetExtendedDifference((DateTime)solicitacao.Solicitacao.Atendimento.Paciente.Nascimento);

                                resultadoDto.IsRn = (idade.Ano == 0 && idade.Mes == 0 && idade.Dia <= 30);
                            }

                            if (solicitacao.Solicitacao.Atendimento.Convenio != null)
                            {
                                resultadoDto.ConvenioId = solicitacao.Solicitacao.Atendimento.ConvenioId;
                                resultadoDto.Convenio = solicitacao.Solicitacao.Atendimento.Convenio;
                            }

                            List<ResultadoExameIndexCrudDto> exames = new List<ResultadoExameIndexCrudDto>();

                            long idGrid = 0;
                            foreach (var item in solicitacoesExames)
                            {
                                ResultadoExameIndexCrudDto resultadoExame = new ResultadoExameIndexCrudDto
                                {
                                    SolicitacaoItemId = item.Id,
                                    FaturamentoItemId = item.FaturamentoItem?.Id,
                                    Exame = item.FaturamentoItem?.Descricao,
                                    MaterialId = item.FaturamentoItem?.MaterialId,
                                    Material = item.FaturamentoItem?.Material?.Descricao,
                                    Quantidade = item.FaturamentoItem.QtdFatura,
                                    IdGridResultadoExame = ++idGrid
                                };
                                exames.Add(resultadoExame);
                            }

                            viewModel = new CriarOuEditarResultadoModalViewModel(resultadoDto)
                            {
                                Atendimento = solicitacao.Solicitacao.Atendimento
                            };

                            TempData["AtendimentoLab"] = viewModel.Atendimento;


                            viewModel.ResultadosExamesList = JsonConvert.SerializeObject(exames);
                            viewModel.IsSolicitacao = true;

                            viewModel.AmbulatorioInternacao = resultadoDto.Atendimento.IsAmbulatorioEmergencia ? 1 : 2;

                            viewModel.ListaAmbulatorioInternacao = new SelectList(ambulatorioInternacao, "Id", "Nome", viewModel.AmbulatorioInternacao);
                        }
                    }
                }
                else
                {
                    viewModel = new CriarOuEditarResultadoModalViewModel(new ResultadoDto());
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Resultados/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}