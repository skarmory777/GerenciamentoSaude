#region Usings
using Abp.Dependency;
using Abp.Threading;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ContasMedicas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Relatorios;
using SW10.SWMANAGER.Web.Controllers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class ContasMedicasController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            var model = new ContasMedicasViewModel();
            using (var faturamentoContaStatusAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaStatusAppService>())
            {
                model.ListaStatus = faturamentoContaStatusAppService.Object.ListarTodos();

                return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContasMedicas/Index.cshtml", model);
            }
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarContaMedicaModalViewModel viewModel;
            if (id.HasValue && id != 0)
            {
                using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var configConvenioAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoConfigConvenioAppService>())
                {
                    var output = await contaMedicaAppService.Object.ObterViewModel((long)id);
                    viewModel = new CriarOuEditarContaMedicaModalViewModel(output);
                    var atendimento = await atendimentoAppService.Object.Obter(viewModel.AtendimentoId.Value);// ta demorando este
                    viewModel.Atendimento = atendimento;
                    viewModel.DataInicio = atendimento.DataRegistro;
                    viewModel.DataFim = atendimento.DataAlta;
                    viewModel.GuiaNumero = atendimento.GuiaNumero;
                    viewModel.NumeroGuia = atendimento.NumeroGuia;
                    viewModel.EmpresaNome = atendimento.Empresa?.NomeFantasia;
                    viewModel.Plano = atendimento.Plano != null ? atendimento.Plano : null;
                    viewModel.Convenio = atendimento.Convenio != null ? atendimento.Convenio : null;
                    viewModel.PlanoId = viewModel.Plano?.Id;
                    viewModel.ConvenioId = viewModel.Convenio?.Id;

                    //try
                    //{
                    //    viewModel.ValorTotal = output.ContaItensDto.Where(w => (w.FaturamentoPacoteId == null || w.FaturamentoItem.Grupo.TipoGrupoId == 4)).Sum(s => (s.ValorItem * s.Qtde));

                    //    foreach (var item in output.ContaItensDto)
                    //    {
                    //        var tabela = item.FaturamentoConfigConvenioDto.TabelaId;



                    //    }


                    //}
                    //catch(Exception ex)
                    //{

                    viewModel.ValorTotal = await contaMedicaAppService.Object.ObterValorContaRegistrado((long)id);

                    ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput();
                    configConvenioInput.Filtro = output.ConvenioId.ToString();
                    var configsConvenio = await configConvenioAppService.Object.ListarPorConvenio(configConvenioInput);

                    // Filtrar por empresa
                    var configsPorEmpresa = configsConvenio.Items
                        .Where(c => c.EmpresaId == output.EmpresaId);

                    viewModel.configsPorEmpresa = configsPorEmpresa.ToArray();

                    // Filtrar por plano
                    var configsPorPlano = configsPorEmpresa
                        .Where(x => x.PlanoId != null &&
                               x.PlanoId == output.PlanoId);

                    viewModel.configsPorPlano = configsPorPlano.ToArray();
                }
            }
            else
            {
                viewModel = new CriarOuEditarContaMedicaModalViewModel(new FaturamentoContaDto())
                {
                    Atendimento = new ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto.AtendimentoDto(),
                    EmpresaId = 0, // CORRIGIR PARA EMPRESA LOGADA
                    ConvenioId = 0, // CORRIGIR ?
                    PlanoId = 0 // CORRIGIR ?
                };
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContasMedicas/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<ActionResult> ContasMedicasPorAtendimentoModal(long? id, long? atendimentoId = null, bool viaAtendimento = false)
        {
            CriarOuEditarContaMedicaModalViewModel viewModel;
            if (id.HasValue && id != 0)
            {
                using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var configConvenioAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoConfigConvenioAppService>())
                {
                    var output = await contaMedicaAppService.Object.ObterViewModel((long)id);
                    viewModel = new CriarOuEditarContaMedicaModalViewModel(output);
                    viewModel.Atendimento = await atendimentoAppService.Object.Obter((long)viewModel.AtendimentoId);// ta demorando este

                    ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput();
                    configConvenioInput.Filtro = output.ConvenioId.ToString();
                    var configsConvenio = await configConvenioAppService.Object.ListarPorConvenio(configConvenioInput);

                    // Filtrar por empresa
                    var configsPorEmpresa = configsConvenio.Items
                        .Where(c =>
                            c.EmpresaId == output.EmpresaId);

                    viewModel.configsPorEmpresa = configsPorEmpresa.ToArray();

                    // Filtrar por plano
                    var configsPorPlano = configsPorEmpresa
                        .Where(x => x.PlanoId != null)
                        .Where(c => c.PlanoId == output.PlanoId);

                    viewModel.configsPorPlano = configsPorPlano.ToArray();
                }
            }
            else
            {
                viewModel = new CriarOuEditarContaMedicaModalViewModel(new FaturamentoContaDto());

                if (atendimentoId.HasValue && atendimentoId != 0)
                {
                    using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                    {
                        viewModel.Atendimento = await atendimentoAppService.Object.Obter((long)atendimentoId);

                        viewModel.EmpresaId = viewModel.Atendimento.EmpresaId ?? 0;
                        viewModel.EmpresaNome = viewModel.Atendimento.Empresa?.NomeFantasia;
                        viewModel.ConvenioId = viewModel.Atendimento.ConvenioId ?? 0;
                        viewModel.PlanoId = viewModel.Atendimento.PlanoId ?? 0;
                        viewModel.PacienteNome = viewModel.Atendimento.Paciente?.NomeCompleto;
                        viewModel.AtendimentoId = atendimentoId;
                        viewModel.DataInicio = viewModel.Atendimento.DataRegistro;
                        viewModel.DataFim = viewModel.Atendimento.DataAlta;
                        viewModel.NumeroGuia = viewModel.Atendimento.GuiaNumero;
                        viewModel.FatGuia = viewModel.Atendimento.FatGuia;
                        viewModel.TipoAcomodacaoId = viewModel.Atendimento.TipoAcomodacaoId;
                        viewModel.TipoAcomodacaoDescricao = viewModel.Atendimento.TipoAcomodacao?.Descricao;
                    }
                }
                else
                {
                    viewModel.Atendimento = new ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto.AtendimentoDto();
                    viewModel.EmpresaId = 0; // CORRIGIR PARA EMPRESA LOGADA
                    viewModel.ConvenioId = 0; // CORRIGIR ?
                    viewModel.PlanoId = 0; // CORRIGIR ?
                }
            }

            viewModel.selecionarPrimeiraConta = viaAtendimento;

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContasMedicas/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<ActionResult> _ContaMedicas(long? id, bool viaAtendimento = false)
        {
            ContasAtendimentoViewModel viewModel;
            if (id.HasValue)
            {
                viewModel = new ContasAtendimentoViewModel();
                viewModel.AtendimentoId = id;
            }
            else
            {
                viewModel = new ContasAtendimentoViewModel();
                viewModel.AtendimentoId = id;
            }

            viewModel.selecionarPrimeiraConta = viaAtendimento;

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContasMedicas/_Contas.cshtml", viewModel);
        }

        public async Task<ActionResult> ContaMedica(long? id)
        {
            CriarOuEditarContaMedicaModalViewModel viewModel;
            if (id.HasValue)
            {
                using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
                {
                    var output = await contaMedicaAppService.Object.ObterViewModel((long)id);
                    viewModel = new CriarOuEditarContaMedicaModalViewModel(output);
                }
            }
            else
            {
                viewModel = new CriarOuEditarContaMedicaModalViewModel(new FaturamentoContaDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContasMedicas/ContaMedica/Index.cshtml", viewModel);
        }

        public async Task<ActionResult> VisualizarRelatorio(long contaMedicaId)
        {
            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            using (var contaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            {

                var contaMedica = AsyncHelper.RunSync(() => contaMedicaAppService.Object.ObterReportModel(contaMedicaId));

                var dados = new FiltroModel();
                dados.Contas = new List<ContaMedicaReportModel>();
                dados.Contas.Add(contaMedica);

                var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations();

                dados.Titulo = string.Concat("Conta Médica - ", contaMedica.DataIncio?.ToString("dd/MM/yy"), " a ", contaMedica.DataFim?.ToString("dd/MM/yy"));

                dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                dados.DataHora = Convert.ToString(DateTime.Now);

                var x = dados.Contas[0];
                dados.Paciente = x.PacienteNome;
                dados.Nascimento = x.PacienteNascimento;

                dados.NomeHospital = x.EmpresaNome;
                dados.Convenio = x.ConvenioNome;
                dados.Plano = x.PlanoNome;
                dados.Matricula = x.Matricula;
                dados.Titular = x.Titular;
                dados.ValidCarteira = x.ValidadeCarteira?.ToString("dd/MM/yy");
                dados.DataInternacao = string.Format("{0:dd/MM/yyyy}", x.DataIncio);
                dados.Senha = x.SenhaAutorizacao;
                dados.Guia = x.GuiaNumero;
                dados.Especialidade = "";
                dados.Medico = x.MedicoNome;
                dados.CRM = x.CRM;
                dados.TipoAlta = x.TipoAlta;

                var input = new ListarFaturamentoContaItensInput();
                input.Filtro = contaMedicaId.ToString();

                dados.Itens = AsyncHelper.RunSync(() => contaItemAppService.Object.ListarReportModel(input)).Items.ToList();

                // Separando contaItens por grupos
                dados.ListaTotal = new Dictionary<string, IList<Dictionary<string, string>>>();

                var total = dados.Itens.GroupBy(g => g.Grupo);

                foreach (var grupo in total)
                {
                    dados.ListaTotal.Add(grupo.Key, new List<Dictionary<string, string>>());

                    foreach (var item in grupo)
                    {
                        var valorTotal = item.Qtde * item.ValorItem;

                        var totalPercentual = valorTotal * item.Percentual / 100;

                        var novo = new Dictionary<string, string>
                        {
                            {"Codigo", item.FaturamentoItemCodigo },
                            {"Descricao", item.FaturamentoItemDescricao },
                            {"Quantidade", item.Qtde.ToString() },
                            {"PrecoUnitario",  item.ValorItem > 0 ? string.Format("{0:#,##0.00}", item.ValorItem): "" },
                            {"ValorTotal",   valorTotal > 0 ? string.Format("{0:#,##0.00}", valorTotal): "" },
                            {"Percentual", item.Percentual.ToString() },
                            {"TotalPercentual",  totalPercentual > 0 ? string.Format("{0:#,##0.00}", totalPercentual): ""  },
                            {"Medico", item.MedicoNome }
                        };

                        dados.ListaTotal[grupo.Key].Add(novo);
                    }
                }

                return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Relatorios/RelatorioContaMedica.aspx", dados);
            }
        }

        // PARA CALCULO DE ITEM DE CONTA
        [AcceptVerbs("GET", "POST", "PUT")]
        public async Task<bool> VerificarCadastroPrecoItem(long contaId, long itemId)
        {
            try
            {
                using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
                using (var itemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoItemAppService>())
                using (var configConvenioAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoConfigConvenioAppService>())
                {
                    var output = await contaMedicaAppService.Object.ObterViewModel(contaId);

                    ListarFaturamentoConfigConveniosInput configConvenioInput = new ListarFaturamentoConfigConveniosInput
                    {
                        Filtro = output.ConvenioId.ToString()
                    };
                    var configsConvenio = await configConvenioAppService.Object.ListarPorConvenio(configConvenioInput);

                    // Filtrar por empresa
                    var configsPorEmpresa = configsConvenio.Items
                        .Where(c => c.EmpresaId == output.EmpresaId);

                    // Filtrar por plano
                    var configsPorPlano = configsPorEmpresa
                        .Where(x => x.PlanoId != null)
                        .Where(c => c.PlanoId == output.PlanoId);

                    ContaCalculoItem contaCalculoItem = new ContaCalculoItem
                    {
                        EmpresaId = (long)output.EmpresaId,
                        ConvenioId = (long)output.ConvenioId,
                        PlanoId = (long)output.PlanoId
                    };

                    VerificarCadastroPrecoInput verificarInput = new VerificarCadastroPrecoInput
                    {
                        configsPorEmpresa = configsPorEmpresa.ToArray(),
                        configsPorPlano = configsPorPlano.ToArray(),
                        conta = contaCalculoItem
                    };
                    var fatContaItem = new FaturamentoContaItemDto
                    {
                        FaturamentoItem = await itemAppService.Object.Obter(itemId)
                    };
                    verificarInput.FatContaItemDto = fatContaItem;
                    var te = await contaMedicaAppService.Object.VerificarCadastroPrecoItem(verificarInput);

                    return te;
                }
            }
            catch (Exception ex)
            {

            }
            return true;
        }



        //public async Task<ActionResult> VisualizarSpsadt ()
        //{
        //    var dados = new GuiaSpsadtModel();
        //    dados.Titulo = "Guia SPSADT";
        //    dados.Contas = new List<ContaMedicaReportModel>();

        //    var x = new ContaMedicaReportModel();
        //    x.AtendimentoCodigo = "12313212";
        //    dados.Contas.Add(x);

        //    try
        //    {
        //        Guias relDS = new Guias();
        //        DataTable tabela = this.ConvertToDataTable(dados.Contas, relDS.Tables["Spsadt"]);
        //        ReportDataSource dataSource = new ReportDataSource("Spsadt", tabela);
        //        ReportViewer GuiaSpsadt = new ReportViewer();
        //        GuiaSpsadt.LocalReport.DataSources.Add(dataSource);
        //        ScriptManager scriptManager = new ScriptManager();
        //        scriptManager.RegisterPostBackControl(GuiaSpsadt);
        //        GuiaSpsadt.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\Spsadt\guia_spsadt.rdlc");
        //        SetParametros(GuiaSpsadt, dados);
        //        GuiaSpsadt.LocalReport.Refresh();

        //        Warning[] warnings;
        //        string[] streamIds;
        //        string mimeType = string.Empty;
        //        string encoding = string.Empty;
        //        string extension = "pdf";

        //        byte[] pdfBytes = GuiaSpsadt.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
        //        Response.Headers.Add("Content-Disposition", "inline; filename=teste.pdf");
        //        return File(pdfBytes, "application/pdf");
        //    }
        //    catch(Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //    return null;
        //}


        //public void SetParametros (ReportViewer rv, GuiaSpsadtModel dados)
        //{
        //    ReportParameter NumeroGuiaPrestador = new ReportParameter("NumeroGuiaPrestador", string.IsNullOrEmpty(dados.NumeroGuiaPrestador) ? "NumGuiaPrest" : dados.NumeroGuiaPrestador);
        //    ReportParameter RegistroAns = new ReportParameter("RegistroAns", string.IsNullOrEmpty(dados.RegistroAns) ? "registro ans" : dados.RegistroAns);
        //    ReportParameter NumeroGuiaPrincipal = new ReportParameter("NumeroGuiaPrincipal", string.IsNullOrEmpty(dados.NumeroGuiaPrincipal) ? "num guia principal" : dados.NumeroGuiaPrincipal);
        //    ReportParameter DataAutorizacao = new ReportParameter("DataAutorizacao", string.IsNullOrEmpty(dados.DataAutorizacao) ? "data aut" : dados.DataAutorizacao);
        //    ReportParameter Senha = new ReportParameter("Senha", string.IsNullOrEmpty(dados.Senha) ? "12345687" : dados.Senha);
        //    ReportParameter DataValidadeSenha = new ReportParameter("DataValidadeSenha", string.IsNullOrEmpty(dados.DataValidadeSenha) ? "data senha" : dados.DataValidadeSenha);
        //    ReportParameter NumeroGuiaOperadora = new ReportParameter("NumeroGuiaOperadora", string.IsNullOrEmpty(dados.NumeroGuiaOperadora) ? "num guia oper" : dados.NumeroGuiaOperadora);
        //    ReportParameter NumeroCarteira = new ReportParameter("NumeroCarteira", string.IsNullOrEmpty(dados.NumeroCarteira) ? "num carte" : dados.NumeroCarteira);
        //    ReportParameter ValidadeCarteira = new ReportParameter("ValidadeCarteira", string.IsNullOrEmpty(dados.ValidadeCarteira) ? "valid cart" : dados.ValidadeCarteira);
        //    ReportParameter NomePaciente = new ReportParameter("NomePaciente", string.IsNullOrEmpty(dados.NomePaciente) ? "nome pac" : dados.NomePaciente);
        //    ReportParameter CartaoNacionalSaude = new ReportParameter("CartaoNacionalSaude", string.IsNullOrEmpty(dados.NumeroCns) ? "num cns" : dados.NumeroCns);
        //    ReportParameter AtendimentoRn = new ReportParameter("AtendimentoRn", string.IsNullOrEmpty(dados.AtendimentoRn) ? "atend rn" : dados.AtendimentoRn);
        //    ReportParameter CodigoOperadora = new ReportParameter("CodigoOperadora", string.IsNullOrEmpty(dados.CodigoOperadora) ? "cod oper" : dados.CodigoOperadora);
        //    ReportParameter NomeContratado = new ReportParameter("NomeContratado", string.IsNullOrEmpty(dados.NomeContratado) ? "nome contrat" : dados.NomeContratado);
        //    ReportParameter NomeProfissionalSolicitante = new ReportParameter("NomeProfissionalSolicitante", string.IsNullOrEmpty(dados.NomeProfissionalSolicitante) ? "nome prof solic" : dados.NomeProfissionalSolicitante);
        //    ReportParameter ConselhoProfissional = new ReportParameter("ConselhoProfissional", string.IsNullOrEmpty(dados.ConselhoProfissional) ? "cons prof" : dados.ConselhoProfissional);
        //    ReportParameter NumeroConselho = new ReportParameter("NumeroConselho", string.IsNullOrEmpty(dados.NumeroConselho) ? "num conselho" : dados.NumeroConselho);
        //    ReportParameter UF = new ReportParameter("UF", string.IsNullOrEmpty(dados.UF) ? "uf" : dados.UF);
        //    ReportParameter CodigoCbo = new ReportParameter("CodigoCbo", string.IsNullOrEmpty(dados.CodigoCbo) ? "cod cbo" : dados.CodigoCbo);
        //    ReportParameter AssinaturaProfissionalSolicitante = new ReportParameter("AssinaturaProfissionalSolicitante", string.IsNullOrEmpty(dados.AssinaturaProfissionalSolicitante) ? "assin prof" : dados.AssinaturaProfissionalSolicitante);
        //    ReportParameter CaraterAtendimento = new ReportParameter("CaraterAtendimento", string.IsNullOrEmpty(dados.CaraterAtendimento) ? "carater atend" : dados.CaraterAtendimento);
        //    ReportParameter DataSolicitacao = new ReportParameter("DataSolicitacao", string.IsNullOrEmpty(dados.DataSolicitacao) ? "data solic" : dados.DataSolicitacao);
        //    ReportParameter IndicacaoClinica = new ReportParameter("IndicacaoClinica", string.IsNullOrEmpty(dados.IndicacaoClinica) ? "indic clinica" : dados.IndicacaoClinica);
        //    ReportParameter CodigoCne = new ReportParameter("CodigoCne", string.IsNullOrEmpty(dados.CodigoCne) ? "cod cne" : dados.CodigoCne);
        //    ReportParameter TipoAtendimento = new ReportParameter("TipoAtendimento", string.IsNullOrEmpty(dados.TipoAtendimento) ? "tipo atend" : dados.TipoAtendimento);
        //    ReportParameter IndicacaoAcidente = new ReportParameter("IndicacaoAcidente", string.IsNullOrEmpty(dados.IndicacaoAcidente) ? "indic acid" : dados.IndicacaoAcidente);
        //    ReportParameter TipoConsulta = new ReportParameter("TipoConsulta", string.IsNullOrEmpty(dados.TipoConsulta) ? "tipo consulta" : dados.TipoConsulta);
        //    ReportParameter MotivoEncerramentoAtendimento = new ReportParameter("MotivoEncerramentoAtendimento", string.IsNullOrEmpty(dados.MotivoEncerramentoAtendimento) ? "motivo encer" : dados.MotivoEncerramentoAtendimento);
        //    // Identificacao Equipe
        //    ReportParameter SequenciaRef1 = new ReportParameter("SequenciaRef1", string.IsNullOrEmpty(dados.SequenciaRef1) ? "12345687" : dados.SequenciaRef1);
        //    ReportParameter SequenciaRef2 = new ReportParameter("SequenciaRef2", string.IsNullOrEmpty(dados.SequenciaRef2) ? "12345687" : dados.SequenciaRef2);
        //    ReportParameter SequenciaRef3 = new ReportParameter("SequenciaRef3", string.IsNullOrEmpty(dados.SequenciaRef3) ? "12345687" : dados.SequenciaRef3);
        //    ReportParameter SequenciaRef4 = new ReportParameter("SequenciaRef4", string.IsNullOrEmpty(dados.SequenciaRef4) ? "12345687" : dados.SequenciaRef4);
        //    ReportParameter GrauPart1 = new ReportParameter("GrauPart1", string.IsNullOrEmpty(dados.GrauPart1) ? "12345687" : dados.GrauPart1);
        //    ReportParameter GrauPart2 = new ReportParameter("GrauPart2", string.IsNullOrEmpty(dados.GrauPart2) ? "12345687" : dados.GrauPart2);
        //    ReportParameter GrauPart3 = new ReportParameter("GrauPart3", string.IsNullOrEmpty(dados.GrauPart3) ? "12345687" : dados.GrauPart3);
        //    ReportParameter GrauPart4 = new ReportParameter("GrauPart4", string.IsNullOrEmpty(dados.GrauPart4) ? "12345687" : dados.GrauPart4);
        //    ReportParameter CodigoOperadoraCpf1 = new ReportParameter("CodigoOperadoraCpf1", string.IsNullOrEmpty(dados.CodigoOperadoraCpf1) ? "12345687" : dados.CodigoOperadoraCpf1);
        //    ReportParameter CodigoOperadoraCpf2 = new ReportParameter("CodigoOperadoraCpf2", string.IsNullOrEmpty(dados.CodigoOperadoraCpf2) ? "12345687" : dados.CodigoOperadoraCpf2);
        //    ReportParameter CodigoOperadoraCpf3 = new ReportParameter("CodigoOperadoraCpf3", string.IsNullOrEmpty(dados.CodigoOperadoraCpf3) ? "12345687" : dados.CodigoOperadoraCpf3);
        //    ReportParameter CodigoOperadoraCpf4 = new ReportParameter("CodigoOperadoraCpf4", string.IsNullOrEmpty(dados.CodigoOperadoraCpf4) ? "12345687" : dados.CodigoOperadoraCpf4);
        //    ReportParameter NomeProfissional1 = new ReportParameter("NomeProfissional1", string.IsNullOrEmpty(dados.NomeProfissional1) ? "12345687" : dados.NomeProfissional1);
        //    ReportParameter NomeProfissional2 = new ReportParameter("NomeProfissional2", string.IsNullOrEmpty(dados.NomeProfissional2) ? "12345687" : dados.NomeProfissional2);
        //    ReportParameter NomeProfissional3 = new ReportParameter("NomeProfissional3", string.IsNullOrEmpty(dados.NomeProfissional3) ? "12345687" : dados.NomeProfissional3);
        //    ReportParameter NomeProfissional4 = new ReportParameter("NomeProfissional4", string.IsNullOrEmpty(dados.NomeProfissional4) ? "12345687" : dados.NomeProfissional4);
        //    ReportParameter ConselhoProfissional1 = new ReportParameter("ConselhoProfissional1", string.IsNullOrEmpty(dados.ConselhoProfissional1) ? "12345687" : dados.ConselhoProfissional1);
        //    ReportParameter ConselhoProfissional2 = new ReportParameter("ConselhoProfissional2", string.IsNullOrEmpty(dados.ConselhoProfissional2) ? "12345687" : dados.ConselhoProfissional2);
        //    ReportParameter ConselhoProfissional3 = new ReportParameter("ConselhoProfissional3", string.IsNullOrEmpty(dados.ConselhoProfissional3) ? "12345687" : dados.ConselhoProfissional3);
        //    ReportParameter ConselhoProfissional4 = new ReportParameter("ConselhoProfissional4", string.IsNullOrEmpty(dados.ConselhoProfissional4) ? "12345687" : dados.ConselhoProfissional4);
        //    ReportParameter NumeroConselho1 = new ReportParameter("NumeroConselho1", string.IsNullOrEmpty(dados.NumeroConselho1) ? "12345687" : dados.NumeroConselho1);
        //    ReportParameter NumeroConselho2 = new ReportParameter("NumeroConselho2", string.IsNullOrEmpty(dados.NumeroConselho2) ? "12345687" : dados.NumeroConselho2);
        //    ReportParameter NumeroConselho3 = new ReportParameter("NumeroConselho3", string.IsNullOrEmpty(dados.NumeroConselho3) ? "12345687" : dados.NumeroConselho3);
        //    ReportParameter NumeroConselho4 = new ReportParameter("NumeroConselho4", string.IsNullOrEmpty(dados.NumeroConselho4) ? "12345687" : dados.NumeroConselho4);
        //    ReportParameter Uf1 = new ReportParameter("Uf1", string.IsNullOrEmpty(dados.Uf1) ? "12345687" : dados.Uf1);
        //    ReportParameter Uf2 = new ReportParameter("Uf2", string.IsNullOrEmpty(dados.Uf2) ? "12345687" : dados.Uf2);
        //    ReportParameter Uf3 = new ReportParameter("Uf3", string.IsNullOrEmpty(dados.Uf3) ? "12345687" : dados.Uf3);
        //    ReportParameter Uf4 = new ReportParameter("Uf4", string.IsNullOrEmpty(dados.Uf4) ? "12345687" : dados.Uf4);
        //    ReportParameter CodigoCbo1 = new ReportParameter("CodigoCbo1", string.IsNullOrEmpty(dados.CodigoCbo1) ? "12345687" : dados.CodigoCbo1);
        //    ReportParameter CodigoCbo2 = new ReportParameter("CodigoCbo2", string.IsNullOrEmpty(dados.CodigoCbo2) ? "12345687" : dados.CodigoCbo2);
        //    ReportParameter CodigoCbo3 = new ReportParameter("CodigoCbo3", string.IsNullOrEmpty(dados.CodigoCbo3) ? "12345687" : dados.CodigoCbo3);
        //    ReportParameter CodigoCbo4 = new ReportParameter("CodigoCbo4", string.IsNullOrEmpty(dados.CodigoCbo4) ? "12345687" : dados.CodigoCbo4);
        //    // Datas e Assinaturas (procedimentos em serie)
        //    ReportParameter DataRealizacaoProcedimentoSerie1 = new ReportParameter("DataRealizacaoProcedimentoSerie1", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie1) ? "12345687" : dados.DataRealizacaoProcedimentoSerie1);
        //    ReportParameter DataRealizacaoProcedimentoSerie2 = new ReportParameter("DataRealizacaoProcedimentoSerie2", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie2) ? "12345687" : dados.DataRealizacaoProcedimentoSerie2);
        //    ReportParameter DataRealizacaoProcedimentoSerie3 = new ReportParameter("DataRealizacaoProcedimentoSerie3", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie3) ? "12345687" : dados.DataRealizacaoProcedimentoSerie3);
        //    ReportParameter DataRealizacaoProcedimentoSerie4 = new ReportParameter("DataRealizacaoProcedimentoSerie4", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie4) ? "12345687" : dados.DataRealizacaoProcedimentoSerie4);
        //    ReportParameter DataRealizacaoProcedimentoSerie5 = new ReportParameter("DataRealizacaoProcedimentoSerie5", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie5) ? "12345687" : dados.DataRealizacaoProcedimentoSerie5);
        //    ReportParameter DataRealizacaoProcedimentoSerie6 = new ReportParameter("DataRealizacaoProcedimentoSerie6", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie6) ? "12345687" : dados.DataRealizacaoProcedimentoSerie6);
        //    ReportParameter DataRealizacaoProcedimentoSerie7 = new ReportParameter("DataRealizacaoProcedimentoSerie7", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie7) ? "12345687" : dados.DataRealizacaoProcedimentoSerie7);
        //    ReportParameter DataRealizacaoProcedimentoSerie8 = new ReportParameter("DataRealizacaoProcedimentoSerie8", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie8) ? "12345687" : dados.DataRealizacaoProcedimentoSerie8);
        //    ReportParameter DataRealizacaoProcedimentoSerie9 = new ReportParameter("DataRealizacaoProcedimentoSerie9", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie9) ? "12345687" : dados.DataRealizacaoProcedimentoSerie9);
        //    ReportParameter DataRealizacaoProcedimentoSerie10 = new ReportParameter("DataRealizacaoProcedimentoSerie10", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie10) ? "12345687" : dados.DataRealizacaoProcedimentoSerie10);
        //    ReportParameter ObservacaoJustificativa = new ReportParameter("ObservacaoJustificativa", string.IsNullOrEmpty(dados.ObservacaoJustificativa) ? "12345687" : dados.ObservacaoJustificativa);
        //    ReportParameter TotalProcedimentos = new ReportParameter("TotalProcedimentos", string.IsNullOrEmpty(dados.TotalProcedimentos) ? "12345687" : dados.TotalProcedimentos);
        //    ReportParameter TotalTaxasAlugueis = new ReportParameter("TotalTaxasAlugueis", string.IsNullOrEmpty(dados.TotalTaxasAlugueis) ? "12345687" : dados.TotalTaxasAlugueis);
        //    ReportParameter TotalMateriais = new ReportParameter("TotalMateriais", string.IsNullOrEmpty(dados.TotalMateriais) ? "12345687" : dados.TotalMateriais);
        //    ReportParameter TotalOpme = new ReportParameter("TotalOpme", string.IsNullOrEmpty(dados.TotalOpme) ? "12345687" : dados.TotalOpme);
        //    ReportParameter TotalMedicamentos = new ReportParameter("TotalMedicamentos", string.IsNullOrEmpty(dados.TotalMedicamentos) ? "12345687" : dados.TotalMedicamentos);
        //    ReportParameter TotalGeral = new ReportParameter("TotalGeral", string.IsNullOrEmpty(dados.TotalGeral) ? "12345687" : dados.TotalGeral);

        //    rv.LocalReport.SetParameters(new ReportParameter[] {
        //                  //   Titulo ,
        //                     NumeroGuiaPrestador ,
        //                     // Guia
        //                     RegistroAns ,
        //                     NumeroGuiaPrincipal ,
        //                     DataAutorizacao ,
        //                     Senha ,
        //                     NumeroGuiaOperadora ,
        //                     NumeroCarteira ,
        //                     ValidadeCarteira ,
        //                //     NomePaciente ,
        //                     CartaoNacionalSaude ,
        //                     AtendimentoRn ,
        //                     CodigoOperadora ,
        //                     NomeContratado ,
        //                     NomeProfissionalSolicitante ,
        //                     ConselhoProfissional ,
        //                     NumeroConselho ,
        //                     UF ,
        //                     CodigoCbo ,
        //                     AtendimentoRn,
        //                 //    AssinaturaProfissionalSolicitante ,
        //                     CaraterAtendimento ,
        //                     DataSolicitacao ,
        //                     IndicacaoClinica ,
        //                //     CodigoCne ,
        //                     TipoAtendimento ,
        //                     IndicacaoAcidente ,
        //                     TipoConsulta ,
        //                     MotivoEncerramentoAtendimento ,
        //                     // Identificacao Equipe
        //                     //SequenciaRef1 ,
        //                     //SequenciaRef2 ,
        //                     //SequenciaRef3 ,
        //                     //SequenciaRef4 ,
        //                     //GrauPart1 ,
        //                     //GrauPart2 ,
        //                     //GrauPart3 ,
        //                     //GrauPart4 ,
        //                     //CodigoOperadoraCpf1 ,
        //                     //CodigoOperadoraCpf2 ,
        //                     //CodigoOperadoraCpf3 ,
        //                     //CodigoOperadoraCpf4 ,
        //                     //NomeProfissional1 ,
        //                     //NomeProfissional2 ,
        //                     //NomeProfissional3 ,
        //                     //NomeProfissional4 ,
        //                     //ConselhoProfissional1 ,
        //                     //ConselhoProfissional2 ,
        //                     //ConselhoProfissional3 ,
        //                     //ConselhoProfissional4 ,
        //                     //NumeroConselho1 ,
        //                     //NumeroConselho2 ,
        //                     //NumeroConselho3 ,
        //                     //NumeroConselho4 ,
        //                     //Uf1 ,
        //                     //Uf2 ,
        //                     //Uf3 ,
        //                     //Uf4 ,
        //                     //CodigoCbo1 ,
        //                     //CodigoCbo2 ,
        //                     //CodigoCbo3 ,
        //                     //CodigoCbo4 ,
        //                     //// Datas e Assinaturas (procedimentos em serie)
        //                     //DataRealizacaoProcedimentoSerie1 ,
        //                     //DataRealizacaoProcedimentoSerie2 ,
        //                     //DataRealizacaoProcedimentoSerie3 ,
        //                     //DataRealizacaoProcedimentoSerie4 ,
        //                     //DataRealizacaoProcedimentoSerie5 ,
        //                     //DataRealizacaoProcedimentoSerie6 ,
        //                     //DataRealizacaoProcedimentoSerie7 ,
        //                     //DataRealizacaoProcedimentoSerie8 ,
        //                     //DataRealizacaoProcedimentoSerie9 ,
        //                     //DataRealizacaoProcedimentoSerie10 ,
        //                     //ObservacaoJustificativa ,
        //                     //TotalProcedimentos ,
        //                     //TotalTaxasAlugueis ,
        //                     //TotalMateriais ,
        //                     //TotalOpme ,
        //                     //TotalMedicamentos ,
        //                     //TotalGeral
        //                });
        //}

        //public DataTable ConvertToDataTable<T> (IList<T> data, DataTable table)
        //{
        //    PropertyDescriptorCollection properties =
        //       TypeDescriptor.GetProperties(typeof(T));

        //    if (data != null)
        //    {
        //        var x = table.NewRow();
        //        x["RegistroAns"] = "teste";
        //        table.Rows.Add(x);
        //        //foreach (T item in data)
        //        //{
        //        //    try
        //        //    {
        //        //        DataRow row = table.NewRow();
        //        //        foreach (PropertyDescriptor prop in properties)
        //        //            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //        //        table.Rows.Add(row);
        //        //    }
        //        //    catch { }
        //        //}
        //    }

        //    return table;

        //}

    }


}