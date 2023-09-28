namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Configuracoes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Abp.Collections.Extensions;
    using Abp.Dependency;

    using Newtonsoft.Json;

    using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.GeradorFormularios;
    using SW10.SWMANAGER.Web.Controllers;

    public class GeradorFormulariosController : SWMANAGERControllerBase
    {
        // GET: FormularioDinamico
        public ActionResult Index()
        {
            //var list = await _formConfigAppService.ListarTodos();
            var model = new GeradorFormulariosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/Index.cshtml", model);
        }

        public ActionResult CriarFormulario()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/CriarFormulario.cshtml");
        }

        public async Task<ActionResult> CamposReservados()
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                var idReservado = await formConfigAppService.Object.ObterReservadoId().ConfigureAwait(false);

                if (idReservado != 0)
                {
                    ViewBag.CloneId = idReservado;
                    return View(
                        "~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/Reservados/Reservados.cshtml");
                }
                else
                {
                    var id = await formConfigAppService.Object.CriarReservadoAndGetId().ConfigureAwait(false);
                    ViewBag.CloneId = id;
                    return View(
                        "~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/Reservados/Reservados.cshtml");
                }
            }
        }

        public async Task<ContentResult> ObterFormConfigReservado()
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                var result = new FormConfigDto();

                var reservadoId = await formConfigAppService.Object.ObterReservadoId().ConfigureAwait(false);

                result = await formConfigAppService.Object.Obter(reservadoId).ConfigureAwait(false);

                result.Linhas = result.Linhas.OrderBy(i => i.Ordem).ToList();

                var list = JsonConvert.SerializeObject(
                    result,
                    Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                //return Json(result, JsonRequestBehavior.AllowGet);
                return this.Content(list, "application/json");
            }
        }

        public async Task<ContentResult> ObterColConfigReservado(string colDesejada)
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                var reservadoId = await formConfigAppService.Object.ObterReservadoId().ConfigureAwait(false);

                var result = await formConfigAppService.Object.Obter(reservadoId).ConfigureAwait(false);

                result.Linhas = result.Linhas.OrderBy(i => i.Ordem).ToList();

                var cols = result.Linhas.SelectMany(x => x.ColConfigs);

                var col = cols.FirstOrDefault(c => c.Name == colDesejada);

                var list = JsonConvert.SerializeObject(
                    col,
                    Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                return Content(list, "application/json");
            }
        }


        public ActionResult ClonarFormulario(long id)
        {
            ViewBag.CloneId = id;
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/ClonarFormulario.cshtml");
        }

        public ActionResult EditarFormularioConfig(long id)
        {
            ViewBag.CloneId = id;
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/EditarFormulario.cshtml");
        }

        public async Task<ActionResult> ListarDados(long id)
        {
            var model = new FormRespostaViewModel { FormId = id };
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                var form = await formConfigAppService.Object.Obter(id).ConfigureAwait(false);
                model.FormName = form.Nome;
                return this.View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/ListarDados.cshtml", model);
            }
        }

        public async Task<PartialViewResult> _ListarDados(long id)
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                var model = new FormRespostaViewModel { FormId = id };
                var form = await formConfigAppService.Object.Obter(id).ConfigureAwait(false);
                model.FormName = form.Nome;
                return this.PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/_ListarDados.cshtml",
                    model);
            }
        }

        //public JsonResult ObterFormConfig(long id = 3)
        public async Task<ContentResult> ObterFormConfig(long id = 3, long atendimentoId = 0, long classeId = 0, long idResposta = 0)
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            using (var formRespostaAppService = IocManager.Instance.ResolveAsDisposable<IFormRespostaAppService>())
            {
                var result = await formConfigAppService.Object.Obter(id).ConfigureAwait(false);

                var reservados = await formConfigAppService.Object.ListarReservados();

                result.Linhas = result.Linhas.OrderBy(i => i.Ordem).ToList();
                if (atendimentoId != 0 && classeId != 0)
                {
                    FormRespostaDto formRespostaAtendimento = null;
                    FormRespostaDto formRespostaUltimoLancamento = null;
                    List<FormRespostaDto> formRespostaUltimoLancamentoPorAtendimento = null;
                    using (var prontuarioAppService =
                        IocManager.Instance.ResolveAsDisposable<IProntuarioEletronicoAppService>())
                    {
                        var prontuarioAtual = await prontuarioAppService.Object.Obter(classeId).ConfigureAwait(false);

                        var prontuario = await prontuarioAppService.Object
                                             .ObterUltimoProntuarioPorAtendimentoEFormulario(
                                                 atendimentoId,
                                                 id,
                                                 prontuarioAtual?.FormRespostaId ?? 0).ConfigureAwait(false);

                        if (prontuario?.FormRespostaId != null)
                        {
                            formRespostaAtendimento = FormRespostaDto.Mapear(prontuario.FormResposta);
                        }

                        formRespostaUltimoLancamento = await formRespostaAppService.Object
                                                           .ObterUltimoLancamentoPorFormConfig(
                                                               id,
                                                               prontuarioAtual?.FormRespostaId ?? 0)
                                                           .ConfigureAwait(false);
                        
                        formRespostaUltimoLancamentoPorAtendimento = await formRespostaAppService.Object
                            .ObterUltimoLancamentosPorFormConfig(
                                id,
                                prontuarioAtual?.FormRespostaId ?? 0,
                                atendimentoId)
                            .ConfigureAwait(false);
                    }

                    foreach (var linha in result.Linhas)
                    {
                        foreach (var colConfig in linha.ColConfigs)
                        {
                            switch (colConfig.Preenchimento)
                            {
                                case ColConfig.AtendimentoAtual:
                                {
                                    if (formRespostaAtendimento != null
                                        && !formRespostaAtendimento.ColRespostas.IsNullOrEmpty())
                                    {
                                        var atendimentoColConfig =
                                            formRespostaAtendimento.ColRespostas.FirstOrDefault(
                                                x => x.ColConfigId == colConfig.Id || (colConfig.ColConfigReservadoId.HasValue && x.Coluna.ColConfigReservadoId == colConfig.ColConfigReservadoId));

                                        if (!colConfig.MultiOption.IsNullOrEmpty() && atendimentoColConfig?.Coluna != null)
                                        {
                                            var multiOptions =
                                            formRespostaAtendimento.ColRespostas.Where(
                                                x => x.ColConfigId == colConfig.Id || (colConfig.ColConfigReservadoId.HasValue && x.Coluna.ColConfigReservadoId == colConfig.ColConfigReservadoId));
                                            foreach (var colMultiOption in colConfig.MultiOption)
                                            {
                                                colMultiOption.Selecionado = multiOptions.Any(x => x.Valor == colMultiOption.Opcao);
                                            }
                                        }

                                        colConfig.Value = atendimentoColConfig?.Valor;

                                        colConfig.Valores = new List<FormDataDto>
                                        {
                                            new FormDataDto
                                                {
                                                    Coluna = colConfig,
                                                    ColConfigId = colConfig.Id,
                                                    Valor = colConfig.Value
                                                }
                                        };
                                    }

                                    break;
                                }

                                case ColConfig.UltimoLancamento:
                                {
                                    if (formRespostaUltimoLancamento != null && !formRespostaUltimoLancamento.ColRespostas.IsNullOrEmpty())
                                    {
                                        var ultimoColConfig = formRespostaUltimoLancamento.ColRespostas.FirstOrDefault(x => x.ColConfigId == colConfig.Id || (colConfig.ColConfigReservadoId.HasValue && x.Coluna.ColConfigReservadoId == colConfig.ColConfigReservadoId));

                                        if (!colConfig.MultiOption.IsNullOrEmpty() && ultimoColConfig?.Coluna != null && !ultimoColConfig.Coluna.MultiOption.IsNullOrEmpty())
                                        {
                                            foreach (var colMultiOption in colConfig.MultiOption)
                                            {
                                                colMultiOption.Selecionado =
                                                    ultimoColConfig.Coluna.MultiOption
                                                        .FirstOrDefault(x => x.Opcao == colMultiOption.Opcao)
                                                        ?.Selecionado ?? false;
                                            }
                                        }

                                        colConfig.Value = ultimoColConfig?.Valor;

                                        colConfig.Valores = new List<FormDataDto>
                                        {
                                            new FormDataDto
                                                {
                                                    Coluna = colConfig,
                                                    ColConfigId = colConfig.Id,
                                                    Valor = colConfig.Value
                                                }
                                        };
                                    }

                                    break;
                                }
                                
                                case ColConfig.UltimoLancamentoPorAtendimento:
                                    {
                                        if (!formRespostaUltimoLancamentoPorAtendimento.IsNullOrEmpty())
                                        {
                                            var iQformRespostaUltimoLancamentoPorAtendimento = formRespostaUltimoLancamentoPorAtendimento.Where(item =>
                                                !item.ColRespostas.IsNullOrEmpty()
                                                && item.ColRespostas.Any(x =>
                                                    colConfig.ColConfigReservadoId.HasValue &&
                                                    x.Coluna.ColConfigReservadoId == colConfig.ColConfigReservadoId));
                                            if (!iQformRespostaUltimoLancamentoPorAtendimento.Any())
                                            {
                                                continue;
                                            }
                                        
                                            var ultimoColConfig =
                                                iQformRespostaUltimoLancamentoPorAtendimento.OrderByDescending(x=> x.DataResposta)
                                                    .FirstOrDefault()?.ColRespostas.FirstOrDefault(
                                                        x => x.ColConfigId == colConfig.Id || (colConfig.ColConfigReservadoId.HasValue && x.Coluna.ColConfigReservadoId == colConfig.ColConfigReservadoId));

                                            if (!colConfig.MultiOption.IsNullOrEmpty() && ultimoColConfig?.Coluna != null && !ultimoColConfig.Coluna.MultiOption.IsNullOrEmpty())
                                            {
                                                foreach (var colMultiOption in colConfig.MultiOption)
                                                {
                                                    colMultiOption.Selecionado =
                                                        ultimoColConfig.Coluna.MultiOption
                                                            .FirstOrDefault(x => x.Opcao == colMultiOption.Opcao)
                                                            ?.Selecionado ?? false;
                                                }
                                            }

                                            colConfig.Value = ultimoColConfig?.Valor;

                                            colConfig.Valores = new List<FormDataDto>
                                            {
                                                new FormDataDto
                                                    {
                                                        Coluna = colConfig,
                                                        ColConfigId = colConfig.Id,
                                                        Valor = colConfig.Value
                                                    }
                                            };
                                        }

                                        break;
                                    }

                                default:
                                    {
                                        if (!colConfig.MultiOption.IsNullOrEmpty())
                                        {
                                            colConfig.MultiOption.ForEach(x => x.Selecionado = false);
                                        }

                                        colConfig.Value = string.Empty;

                                        colConfig.Valores = null;
                                        break;
                                    }
                            }
                        }
                    }
                }

                var list = JsonConvert.SerializeObject(
                    result,
                    Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                //return Json(result, JsonRequestBehavior.AllowGet);
                return this.Content(list, "application/json");
            }
        }

        public async Task<ContentResult> ObterCloneFormConfig(long id = 3)
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                var result = await formConfigAppService.Object.Clonar(id).ConfigureAwait(false);
                var list = JsonConvert.SerializeObject(
                    result,
                    Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                return Content(list, "application/json");
            }
        }

        public async Task<ContentResult> ObterDados(long id = 2)
        {
            using (var formRespostaAppService = IocManager.Instance.ResolveAsDisposable<IFormRespostaAppService>())
            {
                var result = await formRespostaAppService.Object.Obter(id).ConfigureAwait(false);
                if (result?.FormConfig?.Linhas != null)
                {
                    result.FormConfig.Linhas = result.FormConfig.Linhas.OrderBy(x => x.Ordem).ToList();
                    var cols = result.FormConfig.Linhas.SelectMany(s => s.ColConfigs).ToList();

                    foreach (var item in cols)
                    {
                        if (item == null)
                        {
                            continue;
                        }

                        if (item.Type != "checkbox")
                        {
                            item.Value = result.ColRespostas?.Where(w => w.Coluna.Id == item.Id)?.FirstOrDefault()
                                ?.Valor;
                            continue;
                        }
                        var valores = result.ColRespostas?.Where(w => w.Coluna.Id == item.Id).ToList();

                        item.Valores = valores;
                        item.Value = valores.FirstOrDefault()?.Valor;

                        item.MultiOption?.ForEach(
                            e => { e.Selecionado = valores?.Any(a => a.Valor == e.Opcao) ?? false; });
                    }
                }

                var list = JsonConvert.SerializeObject(
                    result,
                    Formatting.None,
                    new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                return Content(list, "application/json");
            }
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult Preencher(long id)
        {
            ViewBag.formId = id;
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/Preencher.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ViewResult _Preencher(string nomeClasse, long? formConfigId = null, long? registroClasseId = null, long? atendimentoId = null, long? formRespostaId = null, long? leitoId = null, long? atendimentoLeitoId = null, bool habilitaAlteracaoLeito = false)
        {
            ViewBag.formId = formConfigId;
            ViewBag.nomeClasse = nomeClasse;
            ViewBag.registroClasseId = registroClasseId;
            ViewBag.atendimentoId = atendimentoId;
            ViewBag.dadosRespostaId = formRespostaId;
            ViewBag.leitoId = leitoId;
            ViewBag.atendimentoLeitoId = atendimentoLeitoId;
            ViewBag.habilitaAlteracaoLeito = habilitaAlteracaoLeito;

            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/_Preencher.cshtml");
        }

        //[OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult EditarPreenchimento(long id = 2)
        {
            ViewBag.dadosRespostaId = id;
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/EditarPreenchimento.cshtml");
        }

        public ActionResult EditarForm(string nomeClasse, long formRespostaId, long registroClasseId)
        {
            ViewBag.dadosRespostaId = formRespostaId;
            ViewBag.nomeClasse = nomeClasse;
            ViewBag.registroClasseId = registroClasseId;
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/EditarPreenchimento.cshtml");
        }


        public ActionResult PreencherForm(string nomeClasse, long formConfigId, long registroClasseId)
        {
            ViewBag.formId = formConfigId;
            ViewBag.nomeClasse = nomeClasse;
            ViewBag.registroClasseId = registroClasseId;
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/Preencher.cshtml");
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ViewResult _EditarPreenchimento(string nomeClasse, long formRespostaId, long registroClasseId, long? atendimentoId = null)
        {
            ViewBag.dadosRespostaId = formRespostaId;
            ViewBag.nomeClasse = nomeClasse;
            ViewBag.registroClasseId = registroClasseId;

            // Para Campos reservados - atendimento
            //var fr = AsyncHelper.RunSync(()=> _formRespostaAppService.Obter(formRespostaId));

            //var atd = _atdService.

            ViewBag.atendimentoId = atendimentoId;
            // Fim - campos reservados;

            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/_EditarPreenchimento.cshtml");
        }

        public ActionResult DetalharPreenchimento(long id)
        {
            ViewBag.dadosRespostaId = id;
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/DetalharPreenchimento.cshtml");
        }

        [HttpPost]
        public async Task<JsonResult> GravarConfig(FormConfigDto form)
        {
            if (form == null)
            {
                throw new Exception();
            }

            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                form.DataAlteracao = DateTime.Now;
                await formConfigAppService.Object.CriarOuEditar(form).ConfigureAwait(false);

                return Json(form);
            }
        }

        private static void ProcessarValor(ColConfig Col, List<FormData> dados, ColConfig coluna, List<ColMultiOption> multiSelected)
        {
            if (Col.Type == "checkbox" && multiSelected != null)
            {
                for (int i = 0; i < multiSelected.Count; i++)
                {
                    dados.Add(new FormData
                    {
                        Valor = multiSelected[i].Opcao + (i < (multiSelected.Count - 1) ? "," : string.Empty),
                        Coluna = coluna
                    });
                }
            }
            else
            {
                dados.Add(new FormData
                {
                    Valor = Col.Value,
                    Coluna = coluna
                });
            }
        }

        //Adicionando actions para relacionar o formulário com as Operações e as Unidades Operacionais

        public async Task<PartialViewResult> _AssociarUnidadeOrganizacional(long formId)
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                var model = new FormConfigUnidadeOrganizacionalDto
                {
                    FormConfigId = formId,
                    FormConfig = await formConfigAppService.Object.Obter(formId).ConfigureAwait(false)
                };
                var viewModel = new AssociarUnidadeOrganizacionalViewModel(model);

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/_AssociarUnidadeOrganizacional.cshtml",
                    viewModel);
            }
        }

        public async Task<PartialViewResult> _AssociarOperacao(long formId)
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                var model = new FormConfigOperacaoDto
                {
                    FormConfigId = formId,
                    FormConfig = await formConfigAppService.Object.Obter(formId).ConfigureAwait(false)
                };
                var viewModel = new AssociarOperacaoViewModel(model);

                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/_AssociarOperacao.cshtml",
                    viewModel);
            }
        }

        public async Task<PartialViewResult> _AssociarEspecialidade(long formId)
        {
            using (var formConfigAppService = IocManager.Instance.ResolveAsDisposable<IFormConfigAppService>())
            {
                var model = new FormConfigEspecialidadeDto
                {
                    FormConfigId = formId,
                    FormConfig = await formConfigAppService.Object.Obter(formId).ConfigureAwait(false)
                };
                var viewModel = new AssociarEspecialidadeViewModel(model);
                return PartialView(
                    "~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorFormularios/_AssociarEspecialidade.cshtml",
                    viewModel);
            }
        }
    }
}