using System.Threading;
using System.Web.Http;
using Castle.Core.Internal;
using Microsoft.OData.Edm;
using SW10.SWMANAGER.Helper;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Assistenciais.Relatorios
{
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Extensions;
    using Abp.UI;
    using Abp.Web.Mvc.Authorization;
    using Microsoft.Reporting.WebForms;
    using SW10.SWMANAGER.Authorization;
    using SW10.SWMANAGER.Authorization.Users;
    using SW10.SWMANAGER.ClassesAplicacao;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Divisoes;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Enumeradores;
    using SW10.SWMANAGER.Sessions;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Relatorios;
    using SW10.SWMANAGER.Web.Controllers;
    using SW10.SWMANAGER.Web.Relatorios.Assistenciais;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.UI;

    using SW10.SWMANAGER.ClassesAplicacao.Services.RegistroArquivos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
    using System.Data.Entity;

    public class AssistenciaisRelatoriosController : SWMANAGERControllerBase
    {
        private readonly IIocManager _iocManager;

        public AssistenciaisRelatoriosController(IIocManager iocManager)
        {
            this._iocManager = iocManager;
        }

        /// <summary>
        /// Entrada para filtro de visualização do Report de produtos
        /// </summary>
        /// <returns></returns>

        // GET: Mpa/Relatorios/SaldoProduto
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao)]
        public async Task<PartialViewResult> PrescricaoMedica()
        {
            var result = await this.CarregarIndex().ConfigureAwait(false);
            return this.PartialView(
                "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Relatorios/PrescricoesMedicas/IndexPrescricaoMedica.cshtml",
                result);
        }

        /// <summary>
        /// PartialView que renderiza o relatório com os filtros selecionados no formulário
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao)]
        public async Task<ActionResult> Visualizar(long atendimentoId, FiltroModel filtro)
        {
            using (var atendimentoAppService = this._iocManager.ResolveAsDisposable<IAtendimentoAppService>())
            using (var userAppService = this._iocManager.ResolveAsDisposable<IUserAppService>())
            using (var empresaAppService = this._iocManager.ResolveAsDisposable<IEmpresaAppService>())
            using (var medicoAppService = this._iocManager.ResolveAsDisposable<IMedicoAppService>())
            using (var sessionAppService = this._iocManager.ResolveAsDisposable<ISessionAppService>())
            using (var pacienteAppService = this._iocManager.ResolveAsDisposable<IPacienteAppService>())
            using (var convenioAppService = this._iocManager.ResolveAsDisposable<IConvenioAppService>())
            using (var unidadeOrganizacionalAppService = this._iocManager.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
            using (var leitoAppService = this._iocManager.ResolveAsDisposable<ILeitoAppService>())
            using (var prescricaoMedicaAppService = this._iocManager.ResolveAsDisposable<IPrescricaoMedicaAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);

                var lista = this.TempData.Peek("Respostas") as List<RespostaDto>;

                if (lista == null)
                {
                    lista = new List<RespostaDto>();
                }

                filtro.Respostas = lista;
                var usuarioEmpresas = await userAppService.Object.GetUserEmpresas(this.AbpSession.UserId.Value)
                                          .ConfigureAwait(false);
                var usuario = await userAppService.Object.GetUser().ConfigureAwait(false);
                var relatorioMedicoPrescricao = filtro.Respostas;
                var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);
                var _filtro = new FiltroModel();
                if (filtro.Respostas.Any())
                {
                    long medicoId = 0;
                    var medico = new MedicoDto();
                    if (atendimento.MedicoId.HasValue)
                    {
                        medicoId = atendimento.MedicoId.Value;
                    }
                    else if (usuario.MedicoId.HasValue)
                    {
                        medicoId = usuario.MedicoId.Value;
                    }

                    if (medicoId > 0)
                    {
                        medico = await medicoAppService.Object.Obter(medicoId).ConfigureAwait(false);
                        _filtro.Medico = medico.NomeCompleto;
                        _filtro.CRM = medico.NumeroConselho.ToString();
                    }
                    else
                    {
                        _filtro.Medico = string.Empty;
                        _filtro.CRM = string.Empty;
                    }

                    _filtro.Titulo = string.Concat("Prescrição Médica - ", Convert.ToString(atendimento.DataRegistro));
                    var empresa = await empresaAppService.Object.Obter(atendimento.EmpresaId.Value).ConfigureAwait(false);
                    var paciente = await pacienteAppService.Object.Obter(atendimento.PacienteId.Value).ConfigureAwait(false);

                    var convenio = atendimento.ConvenioId.HasValue
                                       ? await convenioAppService.Object.Obter(atendimento.ConvenioId.Value)
                                             .ConfigureAwait(false)
                                       : new ConvenioDto();
                    var unidadeOrganizacional = atendimento.UnidadeOrganizacionalId.HasValue
                                                    ? await unidadeOrganizacionalAppService
                                                          .Object.ObterPorId(atendimento.UnidadeOrganizacionalId.Value)
                                                          .ConfigureAwait(false)
                                                    : new UnidadeOrganizacionalDto();
                    var leito = atendimento.LeitoId.HasValue
                                    ? await leitoAppService.Object.Obter(atendimento.LeitoId.Value).ConfigureAwait(false)
                                    : new LeitoDto();
                    _filtro.NomeHospital = empresa.NomeFantasia;
                    _filtro.NomeUsuario = string.Concat(
                        loginInformations.User.Name,
                        " ",
                        loginInformations.User.Surname);
                    _filtro.DataHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                    _filtro.Paciente = string.Concat(paciente.Codigo, " - ", paciente.NomeCompleto);
                    if (paciente.Nascimento.HasValue)
                    {
                        var _idade = DateDifference.GetExtendedDifference(paciente.Nascimento.Value);
                        _filtro.Nascimento = string.Format(
                            "{0} ({1}a {2}m {3}d)",
                            paciente.Nascimento.Value.ToString("dd/MM/yyyy"),
                            _idade.Ano,
                            _idade.Mes,
                            _idade.Dia);
                    }
                    else
                    {
                        _filtro.Nascimento = string.Empty;
                    }

                    _filtro.Convenio = convenio.NomeFantasia.IsNullOrWhiteSpace() ? " " : convenio.NomeFantasia;
                    _filtro.Atendimento = atendimento.Codigo;
                    _filtro.UnidadeOrganizacional = unidadeOrganizacional.Descricao.IsNullOrWhiteSpace()
                                                        ? " "
                                                        : unidadeOrganizacional.Descricao;
                    _filtro.Leito = leito.Descricao.IsNullOrWhiteSpace() ? " " : leito.Descricao;
                    if (filtro.PrescricaoId.HasValue && filtro.PrescricaoId.Value > 0)
                    {
                        var prescricao = await prescricaoMedicaAppService.Object.Obter(filtro.PrescricaoId.Value)
                                             .ConfigureAwait(false);
                        _filtro.Prescricao = prescricao.Codigo;
                    }
                    else
                    {
                        _filtro.Prescricao = string.Empty;
                    }

                    _filtro.Respostas = filtro.Respostas;
                    _filtro.Internacao = atendimento.DataRegistro.ToString("dd/MM/yyyy");
                }

                return this.View(
                    "~/Areas/Mpa/Views/Aplicacao/Assistenciais/Relatorios/PrescricoesMedicas/PrescricaoMedica.aspx",
                    _filtro);
            }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_AmbulatorioEmergencia_Medico_Prescricao)]
        public async Task<ActionResult> VisualizarPDF(long atendimentoId, long prescricaoId)
        {
            try
            {
                using (var atendimentoAppService = this._iocManager.ResolveAsDisposable<IAtendimentoAppService>())
                using (var userAppService = this._iocManager.ResolveAsDisposable<IUserAppService>())
                using (var empresaAppService = this._iocManager.ResolveAsDisposable<IEmpresaAppService>())
                using (var medicoAppService = this._iocManager.ResolveAsDisposable<IMedicoAppService>())
                using (var sessionAppService = this._iocManager.ResolveAsDisposable<ISessionAppService>())
                using (var pacienteAppService = this._iocManager.ResolveAsDisposable<IPacienteAppService>())
                using (var convenioAppService = this._iocManager.ResolveAsDisposable<IConvenioAppService>())
                using (var unidadeOrganizacionalAppService = this._iocManager.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                using (var leitoAppService = this._iocManager.ResolveAsDisposable<ILeitoAppService>())
                using (var prescricaoMedicaAppService = this._iocManager.ResolveAsDisposable<IPrescricaoMedicaAppService>())
                using (var registroArquivoRepository = this._iocManager.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
                {
                    var filtro = new FiltroModel();


                    filtro.Respostas = this.MapearItemResposta((await prescricaoMedicaAppService.Object.ListarRespostasPorPrescricao(prescricaoId).ConfigureAwait(false)).Items.ToList()).ToList();

                    var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);


                    if (filtro.Respostas == null)
                    {
                        filtro.Respostas = new List<RespostaDto>();
                    }


                    var usuarioEmpresas = await userAppService.Object.GetUserEmpresas(this.AbpSession.UserId.Value).ConfigureAwait(false);
                    var usuario = await userAppService.Object.GetUser().ConfigureAwait(false);
                    var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);
                    if (filtro.Respostas.Any())
                    {
                        long medicoId = 0;
                        var medico = new MedicoDto();
                        if (atendimento.MedicoId.HasValue)
                        {
                            medicoId = atendimento.MedicoId.Value;
                        }
                        else if (usuario.MedicoId.HasValue)
                        {
                            medicoId = usuario.MedicoId.Value;
                        }

                        if (medicoId > 0)
                        {
                            medico = await medicoAppService.Object.Obter(medicoId).ConfigureAwait(false);
                            filtro.Medico = medico.NomeCompleto;
                            filtro.CRM = medico.NumeroConselho.ToString();
                        }
                        else
                        {
                            filtro.Medico = string.Empty;
                            filtro.CRM = string.Empty;
                        }

                        filtro.Titulo = string.Concat("Prescrição Médica - ", Convert.ToString(atendimento.DataRegistro));
                        var empresa = await empresaAppService.Object.Obter(atendimento.EmpresaId.Value).ConfigureAwait(false);
                        var paciente = await pacienteAppService.Object.Obter(atendimento.PacienteId.Value).ConfigureAwait(false);
                        var convenio = atendimento.ConvenioId.HasValue
                                           ? await convenioAppService.Object.Obter(atendimento.ConvenioId.Value)
                                                 .ConfigureAwait(false)
                                           : new ConvenioDto();
                        var unidadeOrganizacional = atendimento.UnidadeOrganizacionalId.HasValue
                                                        ? await unidadeOrganizacionalAppService
                                                              .Object.ObterPorId(atendimento.UnidadeOrganizacionalId.Value)
                                                              .ConfigureAwait(false)
                                                        : new UnidadeOrganizacionalDto();
                        var leito = atendimento.LeitoId.HasValue
                                        ? await leitoAppService.Object.Obter(atendimento.LeitoId.Value).ConfigureAwait(false)
                                        : new LeitoDto();
                        filtro.NomeHospital = empresa.NomeFantasia;
                        filtro.NomeUsuario = string.Concat(
                            loginInformations.User.Name,
                            " ",
                            loginInformations.User.Surname);
                        filtro.DataHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                        filtro.Paciente = string.Concat(paciente.CodigoPaciente, " - ", paciente.NomeCompleto);
                        if (paciente.Nascimento.HasValue)
                        {
                            var _idade = DateDifference.GetExtendedDifference(paciente.Nascimento.Value);
                            filtro.Nascimento = string.Format(
                                "{0} ({1}a {2}m {3}d)",
                                paciente.Nascimento.Value.ToString("dd/MM/yyyy"),
                                _idade.Ano,
                                _idade.Mes,
                                _idade.Dia);
                        }
                        else
                        {
                            filtro.Nascimento = string.Empty;
                        }

                        filtro.Convenio = convenio.NomeFantasia.IsNullOrWhiteSpace() ? " " : convenio.NomeFantasia;
                        filtro.Atendimento = atendimento.Codigo;
                        filtro.UnidadeOrganizacional = unidadeOrganizacional.Descricao.IsNullOrWhiteSpace()
                                                           ? " "
                                                           : unidadeOrganizacional.Descricao;
                        filtro.Leito = leito.Descricao.IsNullOrWhiteSpace() ? " " : leito.Descricao;

                        filtro.PrescricaoId = filtro.Respostas[0].PrescricaoId;

                        // }
                        if (filtro.PrescricaoId.HasValue && filtro.PrescricaoId.Value > 0)
                        {
                            var prescricao = await prescricaoMedicaAppService.Object.Obter(filtro.PrescricaoId.Value)
                                                 .ConfigureAwait(false);
                            filtro.Prescricao = prescricao.Codigo;
                        }
                        else
                        {
                            filtro.Prescricao = string.Empty;
                        }

                        filtro.Internacao = atendimento.DataRegistro.ToString("dd/MM/yyyy");
                    }

                    var reportViewer = new ReportViewer();
                    // Obtido do ASPX
                    var scriptManager = new ScriptManager();

                    reportViewer.LocalReport.ReportPath = string.Concat(
                        this.Server.MapPath("~"),
                        @"\Relatorios\Assistenciais\PrescricoesMedicas\PrescricaoMedica.rdlc");
                    var dados = filtro;

                    if (dados != null)
                    {
                        var a = dados.Atendimento;

                        // parâmetros para o relatório
                        var nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        var titulo = new ReportParameter("Titulo", dados.Titulo);
                        var nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                        var dataHora = new ReportParameter("DataHora", dados.DataHora);
                        var paciente = new ReportParameter("Paciente", dados.Paciente);
                        var atendimentoParam = new ReportParameter("Atendimento", dados.Atendimento);
                        var convenio = new ReportParameter("Convenio", dados.Convenio);
                        var internacao = new ReportParameter("Internacao", dados.Internacao);
                        var leito = new ReportParameter("Leito", dados.Leito);
                        var nascimento = new ReportParameter("Nascimento", dados.Nascimento);
                        var prescricao = new ReportParameter("Prescricao", dados.Prescricao);
                        var medico = new ReportParameter("Medico", dados.Medico);
                        var crm = new ReportParameter("CRM", dados.CRM);
                        var unidadeOrganizacional = new ReportParameter(
                            "unidadeOrganizacional",
                            dados.UnidadeOrganizacional);

                        reportViewer.LocalReport.SetParameters(
                            new[]
                                {
                                    nomeHospital, titulo, nomeUsuario, dataHora, paciente, atendimentoParam, convenio,
                                    internacao, leito, nascimento, prescricao, medico, crm, unidadeOrganizacional
                                });

                        // fonte de dados para o relatório - datasource
                        var relDS = new Assistencial();
                        var tabela = this.ConvertToDataTable(dados.Respostas, relDS.Tables["PrescricaoMedica"]);

                        // Logotipo
                        // tabela.Rows[tabela.Rows.Count - 1]["Logotipo"] = atendimento.Empresa.Logotipo;
                        tabela.Rows[0]["Logotipo"] = atendimento.Empresa.Logotipo;

                        // fim - logotipo
                        var dataSource = new ReportDataSource("PrescricaoMedica", tabela);

                        reportViewer.LocalReport.DataSources.Add(dataSource);

                        scriptManager.RegisterPostBackControl(reportViewer);

                        // Gerando PDF
                        var mimeType = "application/pdf"; // string.Empty;
                        var encoding = string.Empty;
                        var extension = "pdf";

                        string[] streamIds;
                        Warning[] warnings;
                        var pdfBytes = reportViewer.LocalReport.Render(
                            "PDF",
                            null,
                            out mimeType,
                            out encoding,
                            out extension,
                            out streamIds,
                            out warnings);

                        var registroArquivo = new RegistroArquivo
                        {
                            Arquivo = pdfBytes,
                            RegistroTabelaId = (long)EnumArquivoTabela.PrescricaoMedica,
                            RegistroId = (long)filtro.PrescricaoId
                        };

                        var id = registroArquivoRepository.Object.InsertAndGetId(registroArquivo);

                        reportViewer.LocalReport.Refresh();
                        //reportViewer.Dispose();

                        this.Response.Headers.Add(
                            "Content-Disposition",
                            string.Format(
                                "inline; filename=prescricao-{0}-{1}.pdf",
                                atendimento.Codigo,
                                DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));
                        return this.File(pdfBytes, "application/pdf");

                        // return View("~/Areas/Mpa/Views/Aplicacao/Assistenciais/Relatorios/PrescricoesMedicas/PrescricaoMedica.aspx", _filtro);
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return null;
        }

        private async Task<FiltroModel> CarregarIndex()
        {
            using (var userAppService = this._iocManager.ResolveAsDisposable<IUserAppService>())
            {
                var userId = this.AbpSession.UserId;

                var result = new FiltroModel();
                var empresas = await userAppService.Object.GetUserEmpresas(userId.Value).ConfigureAwait(false);
                result.Empresas = empresas.Items
                    .Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.NomeFantasia }).ToList();

                // var padrao = new SelectListItem { Text = "Selecione", Value = "0" };
                // result.Empresas.Insert(0, padrao);
                return result;
            }
        }

        public async Task AtualizarTempData(List<PrescricaoItemRespostaDto> list)
        {
            using (var prescricaoItemRespostaAppService = this._iocManager.ResolveAsDisposable<IPrescricaoItemRespostaAppService>())
            using (var prescricaoItemAppService = this._iocManager.ResolveAsDisposable<IPrescricaoItemAppService>())
            using (var prescricaoItemHoraAppService = this._iocManager.ResolveAsDisposable<IPrescricaoItemHoraAppService>())
            using (var divisaoAppService = this._iocManager.ResolveAsDisposable<IDivisaoAppService>())
            {
                var _list = new List<RespostaDto>();
                foreach (var item in list)
                {
                    var _item = item;
                    if (item.Id > 0)
                    {
                        _item = await prescricaoItemRespostaAppService.Object.Obter(item.Id).ConfigureAwait(false);
                        _item.IdGridPrescricaoItemResposta = item.IdGridPrescricaoItemResposta;
                    }

                    if (_item.DivisaoId.HasValue && _item.Divisao == null)
                    {
                        _item.Divisao = await divisaoAppService.Object.Obter(_item.DivisaoId.Value).ConfigureAwait(false);
                    }

                    if (_item.PrescricaoItemId.HasValue)
                    {
                        _item.PrescricaoItem = await prescricaoItemAppService.Object.Obter(_item.PrescricaoItemId.Value)
                                                   .ConfigureAwait(false);
                    }

                    var resposta = new RespostaDto
                    {
                        DataInicial =
                                               _item.DataInicial.HasValue
                                                   ? string.Concat(
                                                       _item.DataInicial.Value.Day.ToString("00"),
                                                       "/",
                                                       _item.DataInicial.Value.Month.ToString("00"),
                                                       "/",
                                                       _item.DataInicial.Value.Year.ToString("0000"))
                                                   : string.Empty,
                        Divisao = _item.Divisao != null ? _item.Divisao.Descricao : string.Empty,
                        DivisaoId = _item.DivisaoId.ToString(),
                        FormaAplicacao =
                                               _item.FormaAplicacao != null
                                                   ? _item.FormaAplicacao.Descricao
                                                   : string.Empty,
                        Frequencia =
                                               _item.Frequencia != null ? _item.Frequencia.Descricao : string.Empty,
                        Observacao = _item.Observacao,
                        PrescricaoItem = _item.PrescricaoItem != null ? string.Concat(_item.PrescricaoItem.Descricao, " - ", _item.VelocidadeInfusao?.Descricao) : string.Empty,
                        PrescricaoItemId = _item.PrescricaoItemId.ToString(),
                        Quantidade = _item.Quantidade.ToString(),
                        Unidade = _item.Unidade != null ? _item.Unidade.Descricao : string.Empty,
                        VelocidadeInfusao =
                                               _item.VelocidadeInfusao != null
                                                   ? _item.VelocidadeInfusao.Descricao
                                                   : string.Empty,
                        Prescricao =
                                               _item.PrescricaoMedica != null
                                                   ? _item.PrescricaoMedica.Codigo
                                                   : string.Empty,
                        UnidadeOrganizacional =
                                               _item.UnidadeOrganizacional != null
                                                   ? _item.UnidadeOrganizacional.Descricao
                                                   : string.Empty,
                        DataCriacao = _item.CreationTime.ToString("dd/MM/yyyy HH:mm:ss"),
                        PrescricaoId = (long)item.PrescricaoMedicaId,
                        Diluente = string.Concat(item.Diluente?.Descricao, item.VolumeDiluente != null ? (string.Concat(" - ", item.VolumeDiluente.ToString(), "ml")) : string.Empty)
                    };


                    if (_item.FrequenciaId.HasValue)
                    {
                        var horarios = await prescricaoItemHoraAppService.Object.ListarPorItem(_item.Id)
                                           .ConfigureAwait(false);
                        if (horarios != null && horarios.Items.Any())
                        {
                            var _d0 = horarios.Items.Where(m => m.DiaMedicamento == 0).Select(s => s.Hora).ToList();
                            var d0 = string.Empty;
                            foreach (var hora in _d0)
                            {
                                d0 += hora + ":00 ";
                            }

                            if (d0.Length > 1)
                            {
                                d0 = d0.Substring(0, d0.Length - 1);
                                resposta.D0 = d0;
                            }

                            var _d1 = horarios.Items.Where(m => m.DiaMedicamento == 1).Select(s => s.Hora).ToList();
                            var d1 = string.Empty;
                            foreach (var hora in _d1)
                            {
                                d1 += hora + ":00 ";
                            }

                            if (d1.Length > 0)
                            {
                                d1 = d1.Substring(0, d1.Length - 1);
                                resposta.D1 = d1;
                            }
                        }
                    }

                    _list.Add(resposta);
                }

                this.TempData["Respostas"] = _list;
            }
        }


        private DataTable ConvertToDataTable<T>(IList<T> data, DataTable table)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));

            if (data != null)
            {
                foreach (var item in data)
                {
                    try
                    {
                        var row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }

            return table;
        }

        public async Task<ActionResult> VisualizarPrescricao(long prescricaoId)
        {
            using (var registroArquivoAppService = this._iocManager.ResolveAsDisposable<IRegistroArquivoAppService>())
            {
                var registro = registroArquivoAppService.Object.ObterPorRegistro(prescricaoId, (long)EnumArquivoTabela.PrescricaoMedica);

                // if (registro == null)
                // {
                //     var attempts = 10;
                //     while (true)
                //     {
                //         if (attempts == 0)
                //         {
                //             break;
                //         }
                //         
                //         registro = registroArquivoAppService.Object.ObterPorRegistro(prescricaoId, (long)EnumArquivoTabela.PrescricaoMedica);
                //         if (registro != null)
                //         {
                //             break;
                //         }
                //
                //         attempts--;
                //         Thread.Sleep(TimeSpan.FromSeconds(1));
                //     }
                // }

                byte[] arquivo = null;

                if (registro != null)
                {
                    arquivo = registro.Arquivo;
                }

                if (arquivo == null)
                {
                    return null;
                }

                this.Response.Headers.Add("Content-Disposition", string.Format("inline; filename=prescricao-{0}-{1}.pdf", "Prescrição", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));
                return this.File(arquivo, "application/pdf");
            }
        }

        [HttpGet]
        public async Task<ActionResult> VisualizarProntuario(long prontuarioId, long operacaoId)
        {
            using (var registroArquivoAppService = this._iocManager.ResolveAsDisposable<IRegistroArquivoAppService>())
            {
                var registro = registroArquivoAppService.Object.ObterPorRegistro(prontuarioId, RegistroArquivoDto.ObterTabelaRegistroFormularioDinamico(operacaoId));



                if (registro?.Arquivo == null)
                {
                    return null;
                }

                this.Response.Headers.Add(
                    "Content-Disposition",
                    string.Format(
                        "inline; filename=prontuario-{0}-{1}.pdf",
                        "Prontuario",
                        DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));
                return this.File(registro.Arquivo, "application/pdf");
            }
        }

        private IEnumerable<RespostaDto> MapearItemResposta(List<PrescricaoItemRespostaDto> list)
        {
            using (var prescricaoItemHoraAppService = this._iocManager.ResolveAsDisposable<IPrescricaoItemHoraAppService>())
            {
                foreach (PrescricaoItemRespostaDto item in list)
                {
                    var resposta = new RespostaDto
                    {
                        DataInicial =
                                               item.DataInicial.HasValue
                                                   ? string.Concat(
                                                       item.DataInicial.Value.Day.ToString("00"),
                                                       "/",
                                                       item.DataInicial.Value.Month.ToString("00"),
                                                       "/",
                                                       item.DataInicial.Value.Year.ToString("0000"))
                                                   : string.Empty,
                        Divisao = item.Divisao != null ? item.Divisao.Descricao : string.Empty,
                        DivisaoId = item.DivisaoId.ToString(),
                        FormaAplicacao =
                                               item.FormaAplicacao != null
                                                   ? item.FormaAplicacao.Descricao
                                                   : string.Empty,
                        Frequencia = item.Frequencia != null ? item.Frequencia.Descricao : string.Empty,
                        Observacao = item.Observacao,
                        PrescricaoItem = item.PrescricaoItem != null ? string.Concat(item.PrescricaoItem.Descricao, " - ", item.VelocidadeInfusao?.Descricao) : string.Empty,
                        PrescricaoItemId = item.PrescricaoItemId.ToString(),
                        Quantidade = item.Quantidade.ToString(),
                        Unidade = item.Unidade != null ? item.Unidade.Descricao : string.Empty,
                        VelocidadeInfusao = item.VelocidadeInfusao != null ? item.VelocidadeInfusao.Descricao : string.Empty,
                        Prescricao = item.PrescricaoMedica != null ? item.PrescricaoMedica.Codigo : string.Empty,
                        UnidadeOrganizacional = item.UnidadeOrganizacional != null ? item.UnidadeOrganizacional.Descricao : string.Empty,
                        DataCriacao = item.CreationTime.ToString("dd/MM/yyyy HH:mm:ss"),
                        PrescricaoId = (long)item.PrescricaoMedicaId,
                        Diluente = string.Concat(item.Diluente?.Descricao, item.VolumeDiluente != null ? (string.Concat(" - ", item.VolumeDiluente.ToString(), "ml")) : string.Empty)
                    };


                    if (item.FrequenciaId.HasValue)
                    {
                        var horarios = prescricaoItemHoraAppService.Object.ListarPorItem(item.Id).GetAwaiter().GetResult();
                        if (horarios != null && horarios.Items.Any())
                        {
                            var _d0 = horarios.Items.Where(m => m.DiaMedicamento == 0).Select(s => s.Hora).ToList();
                            var d0 = string.Empty;
                            foreach (var hora in _d0)
                            {
                                d0 += hora + ":00 ";
                            }

                            if (d0.Length > 1)
                            {
                                d0 = d0.Substring(0, d0.Length - 1);
                                resposta.D0 = d0;
                            }

                            var _d1 = horarios.Items.Where(m => m.DiaMedicamento == 1).Select(s => s.Hora).ToList();
                            var d1 = string.Empty;
                            foreach (var hora in _d1)
                            {
                                d1 += hora + ":00 ";
                            }

                            if (d1.Length > 0)
                            {
                                d1 = d1.Substring(0, d1.Length - 1);
                                resposta.D1 = d1;
                            }
                        }
                    }

                    yield return resposta;
                }
            }
        }

        public async Task<ActionResult> GerarArquivoPrescricao(long atendimentoId, long prescricaoId)
        {
            try
            {
                using (var atendimentoAppService = this._iocManager.ResolveAsDisposable<IAtendimentoAppService>())
                using (var userAppService = this._iocManager.ResolveAsDisposable<IUserAppService>())
                using (var empresaAppService = this._iocManager.ResolveAsDisposable<IEmpresaAppService>())
                using (var medicoAppService = this._iocManager.ResolveAsDisposable<IMedicoAppService>())
                using (var sessionAppService = this._iocManager.ResolveAsDisposable<ISessionAppService>())
                using (var pacienteAppService = this._iocManager.ResolveAsDisposable<IPacienteAppService>())
                using (var convenioAppService = this._iocManager.ResolveAsDisposable<IConvenioAppService>())
                using (var unidadeOrganizacionalAppService = this._iocManager.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                using (var leitoAppService = this._iocManager.ResolveAsDisposable<ILeitoAppService>())
                using (var prescricaoMedicaRepository = this._iocManager.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var prescricaoMedicaAppService = this._iocManager.ResolveAsDisposable<IPrescricaoMedicaAppService>())
                using (var registroArquivoRepository = this._iocManager.ResolveAsDisposable<IRepository<RegistroArquivo, long>>())
                {


                    var filtro = new FiltroModel();

                    var atendimento = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);
                    filtro.Respostas = this.MapearItemResposta((await prescricaoMedicaAppService.Object.ListarRespostasPorPrescricao(prescricaoId).ConfigureAwait(false)).Items.ToList()).ToList();

                    if (filtro.Respostas == null)
                    {
                        filtro.Respostas = new List<RespostaDto>();
                    }

                    var usuarioEmpresas = await userAppService.Object.GetUserEmpresas(this.AbpSession.UserId.Value)
                                              .ConfigureAwait(false);
                    var usuario = await userAppService.Object.GetUser().ConfigureAwait(false);
                    var loginInformations =
                        await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);
                    if (filtro.Respostas.Any())
                    {
                        long medicoId = 0;
                        var medico = new MedicoDto();
                        PrescricaoMedica prescricao = null;

                        if (prescricaoId != null || prescricaoId != 0)
                        {
                            prescricao = await prescricaoMedicaRepository
                                .Object.GetAll()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.Id == prescricaoId).ConfigureAwait(false);

                            medicoId = prescricao.MedicoId ?? 0;
                        }

                        //if (atendimento.MedicoId.HasValue)
                        //{
                        //    medicoId = atendimento.MedicoId.Value;
                        //}
                        //else if (usuario.MedicoId.HasValue)
                        //{
                        //    medicoId = usuario.MedicoId.Value;
                        //}

                        if (medicoId > 0)
                        {
                            medico = await medicoAppService.Object.Obter(medicoId).ConfigureAwait(false);
                            filtro.Medico = medico.NomeCompleto;
                            filtro.CRM = medico.NumeroConselho.ToString();
                        }
                        else
                        {
                            filtro.Medico = string.Empty;
                            filtro.CRM = string.Empty;
                        }

                        filtro.Titulo = string.Concat(
                            "Prescrição Médica - ",
                            atendimento.DataRegistro.ToString("dd/MM/yyyy hh:mm:ss"));
                        var empresa = await empresaAppService.Object.Obter(atendimento.EmpresaId.Value).ConfigureAwait(false);
                        var paciente = await pacienteAppService.Object.Obter(atendimento.PacienteId.Value)
                                           .ConfigureAwait(false);
                        var convenio = atendimento.ConvenioId.HasValue
                                           ? await convenioAppService.Object.Obter(atendimento.ConvenioId.Value)
                                                 .ConfigureAwait(false)
                                           : new ConvenioDto();
                        var unidadeOrganizacional = atendimento.UnidadeOrganizacionalId.HasValue
                                                        ? await unidadeOrganizacionalAppService
                                                              .Object.ObterPorId(atendimento.UnidadeOrganizacionalId.Value)
                                                              .ConfigureAwait(false)
                                                        : new UnidadeOrganizacionalDto();
                        var leito = atendimento.LeitoId.HasValue
                                        ? await leitoAppService.Object.Obter(atendimento.LeitoId.Value).ConfigureAwait(false)
                                        : new LeitoDto();
                        filtro.NomeHospital = empresa.NomeFantasia;
                        filtro.NomeUsuario = string.Concat(
                            loginInformations.User.Name,
                            " ",
                            loginInformations.User.Surname);
                        filtro.DataHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                        filtro.Paciente = string.Concat(paciente.CodigoPaciente, " - ", paciente.NomeCompleto);
                        if (paciente.Nascimento.HasValue)
                        {
                            var _idade = DateDifference.GetExtendedDifference(paciente.Nascimento.Value);
                            filtro.Nascimento = string.Format(
                                "{0} ({1}a {2}m {3}d)",
                                paciente.Nascimento.Value.ToString("dd/MM/yyyy"),
                                _idade.Ano,
                                _idade.Mes,
                                _idade.Dia);
                        }
                        else
                        {
                            filtro.Nascimento = string.Empty;
                        }

                        filtro.Convenio = convenio.NomeFantasia.IsNullOrWhiteSpace() ? " " : convenio.NomeFantasia;
                        filtro.Atendimento = atendimento.Codigo;
                        filtro.UnidadeOrganizacional = unidadeOrganizacional.Descricao.IsNullOrWhiteSpace()
                                                           ? " "
                                                           : unidadeOrganizacional.Descricao;
                        filtro.Leito = leito.Descricao.IsNullOrWhiteSpace() ? " " : leito.Descricao;

                        filtro.PrescricaoId = filtro.Respostas[0].PrescricaoId;

                        // }
                        if (prescricao != null || prescricao.Id != 0)
                        {
                            filtro.Prescricao = prescricao.Codigo;
                        }
                        else
                        {
                            filtro.Prescricao = string.Empty;
                        }

                        filtro.Internacao = atendimento.DataRegistro.ToString("dd/MM/yyyy");
                    }

                    // Obtido do ASPX
                    var reportViewer = new ReportViewer();
                    var scriptManager = new ScriptManager();
                    reportViewer.LocalReport.ReportPath = string.Concat(
                        this.Server.MapPath("~"),
                        @"\Relatorios\Assistenciais\PrescricoesMedicas\PrescricaoMedica.rdlc");
                    var dados = filtro;


                    var alergiasDescricao = string.Empty;

                    if (atendimento.Paciente.PacienteAlergias != null)
                    {
                        foreach (var item in atendimento.Paciente.PacienteAlergias)
                        {
                            alergiasDescricao += string.Concat(item.Alergia, ", ");
                        }

                        if (alergiasDescricao.Length > 0)
                        {
                            alergiasDescricao = alergiasDescricao.TrimEnd((", ").ToArray());
                        }
                    }

                    if (dados != null)
                    {
                        var a = dados.Atendimento;

                        // parâmetros para o relatório
                        var nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        var titulo = new ReportParameter("Titulo", dados.Titulo);
                        var nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                        var dataHora = new ReportParameter("DataHora", dados.DataHora);
                        var paciente = new ReportParameter("Paciente", dados.Paciente);
                        var atendimentoParam = new ReportParameter("Atendimento", dados.Atendimento);
                        var convenio = new ReportParameter("Convenio", dados.Convenio);
                        var internacao = new ReportParameter("Internacao", dados.Internacao);
                        var leito = new ReportParameter("Leito", dados.Leito);
                        var nascimento = new ReportParameter("Nascimento", dados.Nascimento);
                        var prescricao = new ReportParameter("Prescricao", dados.Prescricao);
                        var medico = new ReportParameter("Medico", dados.Medico);
                        var crm = new ReportParameter("CRM", dados.CRM);
                        ReportParameter alergias = new ReportParameter("alergias", alergiasDescricao);
                        var unidadeOrganizacional = new ReportParameter(
                            "unidadeOrganizacional",
                            dados.UnidadeOrganizacional);

                        reportViewer.LocalReport.SetParameters(
                            new[]
                                {
                                        nomeHospital, titulo, nomeUsuario, dataHora, paciente, atendimentoParam,
                                        convenio, internacao, leito, nascimento, prescricao, medico, crm,
                                        unidadeOrganizacional, alergias
                                });

                        // fonte de dados para o relatório - datasource
                        var relDS = new Assistencial();
                        var tabela = this.ConvertToDataTable(dados.Respostas, relDS.Tables["PrescricaoMedica"]);

                        // Logotipo
                        tabela.Rows[tabela.Rows.Count - 1]["Logotipo"] = atendimento.Empresa.Logotipo;

                        // fim - logotipo
                        var dataSource = new ReportDataSource("PrescricaoMedica", tabela);

                        reportViewer.LocalReport.DataSources.Add(dataSource);

                        scriptManager.RegisterPostBackControl(reportViewer);

                        // Gerando PDF
                        var mimeType = "application/pdf"; // string.Empty;
                        var encoding = string.Empty;
                        var extension = "pdf";

                        string[] streamIds;
                        Warning[] warnings;
                        var pdfBytes = reportViewer.LocalReport.Render(
                            "PDF",
                            null,
                            out mimeType,
                            out encoding,
                            out extension,
                            out streamIds,
                            out warnings);

                        var registroArquivo = new RegistroArquivo
                        {
                            Arquivo = pdfBytes,
                            RegistroTabelaId = (long)EnumArquivoTabela.PrescricaoMedica,
                            RegistroId = (long)filtro.PrescricaoId,
                            AtendimentoId = atendimento.Id
                        };

                        var id = await registroArquivoRepository.Object.InsertAndGetIdAsync(registroArquivo)
                                     .ConfigureAwait(false);
                    }

                    //reportViewer.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("erro ao gerar pdf", ex);
            }

            return null;
        }

        public class ImprimirAcrescimosESuspensosInputViewModel
        {
            public long PrescricaoId { get; set; }

            public DateTime[] DatasAgrupamento { get; set; }
        }
        public async Task<FileResult> ImprimirAcrescimosESuspensos(ImprimirAcrescimosESuspensosInputViewModel input)
        {
            using (var prescricaoMedicaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoMedicaAppService>())
            {
                this.Response.Headers.Add("Content-Disposition", string.Format("inline; filename=prescricao-{0}-{1}.pdf", "Prescrição", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));
                var pdfFiles = new List<byte[]>();
                if (!input.DatasAgrupamento.IsNullOrEmpty())
                {
                    foreach (var dataAgrupamento in input.DatasAgrupamento)
                    {
                        pdfFiles.Add(prescricaoMedicaAppService.Object.RetornaArquivoPrescricaoMedica(input.PrescricaoId, true, dataAgrupamento));
                    }
                }

                if (pdfFiles.IsNullOrEmpty())
                {
                    return null;
                }

                return this.File(FileHelper.ConcatAndAddContent(pdfFiles), "application/pdf");
            }
        }

        public async Task<FileResult> ImprimirTudo(long prescricaoId)
        {
            using (var prescricaoMedicaAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoMedicaAppService>())
            {
                return this.File(prescricaoMedicaAppService.Object.RetornaArquivoPrescricaoMedica(prescricaoId, false), "application/pdf", "prescricao.pdf");
            }
        }

        public async Task<FileResult> ImprimirSolicitacaoAntimicrobiano(List<long> ids)
        {
            using (var solicitacaoAntimicrobianoAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoAntimicrobianoAppService>())
            {
                return this.File(solicitacaoAntimicrobianoAppService.Object.RetornaArquivoSolicitacaoAntimicrobiano(ids), "application/pdf", "SolicitacaoAntimicrobiano.pdf");
            }
        }

        [HttpGet]
        public ActionResult ImprimirSolicitacaoExames(long solicitacaoExameId)
        {
            using (var solicitacaoExameAppService = IocManager.Instance.ResolveAsDisposable<ISolicitacaoExameAppService>())
            {
                return this.File(solicitacaoExameAppService.Object.RetornaArquivoSolicitacaoExame(solicitacaoExameId), "application/pdf", $"solicitacao-exame-{solicitacaoExameId}.pdf");
            }
        }

        [HttpGet]
        public ActionResult MapaDiaSintatico(long? unidadeOrganizacionalId, long? statusId)
        {
            using (var atentimentoRelatoriosAppService = IocManager.Instance.ResolveAsDisposable<IAtentimentoRelatoriosAppService>())
            {
                return this.File(atentimentoRelatoriosAppService.Object.RetornaArquivoMapaDiaSintatico(unidadeOrganizacionalId ?? 0, statusId ?? 0), "application/pdf", $"mapa-diario-sintatico.pdf");;
            }
        }

        public class MapaDiaInputViewModel
        {
            public long? UnidadeOrganizacionalId { get; set; }
            public long? StatusId { get; set; }
        }
    }
}