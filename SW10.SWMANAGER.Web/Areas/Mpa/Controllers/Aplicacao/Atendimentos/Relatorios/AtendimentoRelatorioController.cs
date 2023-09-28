namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos.Relatorios
{
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.UI;
    using Abp.Web.Mvc.Authorization;
    using Abp.Web.Security.AntiForgery;
    using Microsoft.Reporting.WebForms;
    using SW10.SWMANAGER.Authorization;
    using SW10.SWMANAGER.Authorization.Users;
    using SW10.SWMANAGER.ClassesAplicacao;
    using SW10.SWMANAGER.ClassesAplicacao.ModeloTexto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Relatorios;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Relatorios.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Visitantes;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Impressora;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Visitantes;
    using SW10.SWMANAGER.Sessions;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Internacao;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Relatorios;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos;
    using SW10.SWMANAGER.Web.Controllers;
    using SW10.SWMANAGER.Web.Relatorios.Atendimento;
    using SW10.SWMANAGER.Web.Relatorios.Faturamento.Guias.InternacaoSolicitacao;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.UI;
    using Atendimento = SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimento;
    using Abp.Authorization.Users;
    using Organizations;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas;

    [DisableAbpAntiForgeryTokenValidation]
    public class AtendimentoRelatorioController : SWMANAGERControllerBase
    {
        #region Dependencias

        #endregion dependencias.

        string obterIdade(DateTime? nascimento)
        {
            var ret = string.Empty;

            if (nascimento != null)
            {
                const string Retorno = "{0} {1}";
                var idade = DateDifference.GetExtendedDifference((DateTime)nascimento);

                if (idade.Ano > 0)
                {
                    ret = string.Format(Retorno, idade.Ano, "Anos");
                }
                else if (idade.Mes > 0)
                {
                    ret = string.Format(Retorno, idade.Mes, "Meses");
                }
                else
                {
                    ret = string.Format(Retorno, idade.Dia, "Dias");
                }
            }

            if (ret == "0 Dias")
            {
                ret = string.Empty;
            }

            return ret;
        }

        public async Task<ActionResult> ReltorioLeitosPdf(long? empresaId = null)
        {
            try
            {
                using (var relatorioAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioAtendimentoAppService>())
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                {
                    var reportViewer = new ReportViewer();
                    var scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(reportViewer);
                    reportViewer.LocalReport.ReportPath = string.Concat(this.Server.MapPath("~"), @"\Relatorios\Atendimento\RelatorioInternado.rdlc");

                    //localização do relatório
                    var dados = await this.CarregarIndex(empresaId).ConfigureAwait(false);
                    var relatorioInternacao = await relatorioAtendimentoAppService.Object.ListarRelatorio(dados.Empresa, dados.MedicoId, dados.EspecilidadeId, dados.ConvenioId).ConfigureAwait(false);

                    //  var relatorioInternacao = _leitoAppService.ListarPorUnidadePaginado(new ListarLeitosInput());

                    var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);

                    if (relatorioInternacao.Items.Count != 0)
                    {
                        dados.Titulo = string.Concat("Mapa Diário - ", Convert.ToString(DateTime.Now));

                        if (empresaId != 0)
                        {
                            dados.NomeHospital = relatorioInternacao.Items[0].Empresa.NomeFantasia.ToString();
                        }
                        else
                        {
                            dados.NomeHospital = "";
                        }

                        dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                        dados.DataHora = Convert.ToString(DateTime.Now);


                        dados.Lista = relatorioInternacao.Items.OrderBy(o => o.LeitoId).Select(m => new TesteObjeto
                        {
                            CodAtendimento = m.Codigo,
                            CodPaciente = (m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente)) == "0" ? "" : m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente),
                            Convenio = m.Convenio == null ? string.Empty : m.Convenio.NomeFantasia,//.Substring(0,),
                            DataInternacao = (m.DataRegistro == null || m.DataRegistro == DateTime.MinValue) ? string.Empty : string.Format("{0:dd/MM/yyyy}", m.DataRegistro),
                            Empresa = m.Empresa == null ? string.Empty : m.Empresa.NomeFantasia,//.Substring(0, 15),
                            Leito = m.Leito == null ? string.Empty : m.Leito.Descricao,
                            Medico = m.Medico == null ? string.Empty : m.Medico.NomeCompleto,
                            Origem = m.Origem == null ? string.Empty : m.Origem.Descricao,

                            Paciente = m.Paciente == null ? string.Empty : m.Paciente.NomeCompleto,

                            //    UnidOrganizacional = m.UnidadeOrganizacional == null ? string.Empty : m.UnidadeOrganizacional.Descricao,
                            Idade = (m.Paciente == null || m.Paciente.Nascimento == null || m.Paciente.Nascimento == DateTime.MinValue) ? string.Empty : this.obterIdade(m.Paciente.Nascimento),
                            DiasInternado = (m.DataRegistro == null || m.DataRegistro == DateTime.MinValue) ? string.Empty : Convert.ToString(Math.Round(DateTime.Now.Subtract(m.DataRegistro).TotalDays, 0))
                        }).OrderBy(x => x.Leito).ToList();
                    }

                    if (dados != null)
                    {
                        var nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        var titulo = new ReportParameter("Titulo", dados.Titulo);
                        var usuario = new ReportParameter("Usuario", dados.NomeUsuario);
                        var dataHora = new ReportParameter("DataHora", dados.DataHora);
                        reportViewer.LocalReport.SetParameters(new ReportParameter[] { nomeHospital, titulo, usuario, dataHora });

                        var relDS = new Web.Relatorios.Atendimento.Atendimento();
                        var tabela = this.ConvertToDataTable(dados.Lista, relDS.Tables["AtendimentoDS"]);
                        var dataSource = new ReportDataSource
                        {
                            Value = tabela,
                            Name = "RelatorioInternado"
                        };
                        reportViewer.LocalReport.DataSources.Clear();
                        reportViewer.LocalReport.DataSources.Add(dataSource);
                        reportViewer.LocalReport.Refresh();

                        var mimeType = string.Empty;
                        var encoding = string.Empty;
                        var extension = "pdf";

                        string[] streamIds;
                        Warning[] warnings;
                        var pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                        reportViewer.LocalReport.Refresh();

                        this.Response.Headers.Add("Content-Disposition", "inline; filename=relatorio_leitos.pdf");
                        return this.File(pdfBytes, "application/pdf");
                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }

            return null;
        }

        [HttpPost]
        public async Task<ActionResult> ReltorioLeitosPdfTeste(long? empresaId = 0, List<long?> conveniosId = null, long? unidadeOrganizacionalId = 0, long? statusLeito = 0)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var leitoAppService = IocManager.Instance.ResolveAsDisposable<ILeitoAppService>())
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                using (var userUnidade = IocManager.Instance.ResolveAsDisposable<IRepository<UserOrganizationUnit, long>>())
                using (var userEmpresas = IocManager.Instance.ResolveAsDisposable<IRepository<UserEmpresa, long>>())
                {
                    var reportViewer = new ReportViewer();
                    var scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(reportViewer);
                    reportViewer.LocalReport.ReportPath = string.Concat(this.Server.MapPath("~"), @"\Relatorios\Atendimento\RelatorioInternado.rdlc");

                    //localização do relatório
                    var dados = await this.CarregarIndex(empresaId).ConfigureAwait(false);

                    var leitos = await leitoAppService.Object.ListarParaRelatorioMapaLeitos(dados.Empresa, statusLeito).ConfigureAwait(false);

                    var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);

                    dados.Titulo = string.Concat("Mapa Diário - ", string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));

                    //if (empresaId != 0)
                    //{
                    //    dados.NomeHospital = relatorioInternacao.Items[0].Empresa.NomeFantasia.ToString();
                    //}
                    //else
                    //{
                    //    dados.NomeHospital = "";
                    //}

                    dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                    dados.DataHora = string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);

                    dados.Lista = new List<TesteObjeto>();

                    leitos = leitos.OrderBy(o => o.UnidadeOrganizacional.Descricao).ToList();

                    var leitosid = leitos.Select(s => s.Id);



                    var empresasId = userEmpresas.Object
                       .GetAll()
                       .Where(m => m.UserId == this.AbpSession.UserId).Select(s => s.EmpresaId);

                    //************** Get UnidadeOrg. do Usuário

                    var userunidadeId = userUnidade.Object
                       .GetAll()
                       .Where(m => m.UserId == this.AbpSession.UserId).Select(s => s.OrganizationUnitId);

                    //var _Unidade = iocResolver.Resolve<IRepository</** UNIDADE ORGRANIZACIONAL **/, long>>();

                    //var unidadeId = _Unidade
                    //   .GetAll()
                    //   .Where(a => a.IsInternacao == true)
                    //   .Where(a => userunidadeId.Any(s => s == a.OrganizationUnitId));
                    //************** 


                    if (conveniosId != null && conveniosId.Count == 1 && conveniosId[0] == null)
                    {
                        conveniosId = new List<long?>();
                    }

                    var atendimentosLeitos = atendimentoRepository.Object
                            .GetAll()
                            .Include(x => x.Paciente)
                            .Include(x => x.Paciente.SisPessoa)
                            .Include(x => x.Medico)
                            .Include(x => x.Medico.SisPessoa)
                            .Include(x => x.AtendimentoTipo)
                            .Include(x => x.Convenio)
                            .Include(x => x.Convenio.SisPessoa)
                            .Include(x => x.Empresa)
                            .Include(x => x.Especialidade)
                            .Include(x => x.Guia)
                            .Include(x => x.Leito)
                            .Include(x => x.Leito.UnidadeOrganizacional)
                            .Include(x => x.Leito.LeitoStatus)
                            .Include(x => x.Leito.TipoAcomodacao)
                            .Include(x => x.MotivoAlta)
                            .Include(x => x.Nacionalidade)
                            .Include(x => x.Origem)
                            .Include(x => x.Plano)
                            .Include(x => x.ServicoMedicoPrestado)
                            .Include(x => x.UnidadeOrganizacional)
                            .Where(a => a.IsInternacao == true)
                            .Where(a => a.DataAlta == null)
                            .Where(a => (empresaId == 0 || a.EmpresaId == empresaId) && a.DataAlta == null)
                            .Where(a => empresasId.Any(s => s == a.EmpresaId))
                            .Where(a => (unidadeOrganizacionalId == 0 || a.UnidadeOrganizacionalId == unidadeOrganizacionalId))
                            // .Where(a => unidadeId.Any(s => s == a.UnidadeOrganizacionalId))
                            .Where(a => !conveniosId.Any() || conveniosId.Any(s => s == a.ConvenioId))
                            .Where(a => leitosid.Any(s => s == a.LeitoId))
                            .AsNoTracking()
                            .ToList();

                    var qtdAtendimentos = (double)atendimentosLeitos.Count();
                    var att = atendimentosLeitos.GroupBy(g => g.Leito.TipoAcomodacaoId);
                    var resumoConvenios = atendimentosLeitos.GroupBy(g => g.ConvenioId).Select(s => new ResumoConvenio
                    {
                        qtd = s.Count(),
                        Convenio = s.First().Convenio.SisPessoa.NomeFantasia,
                        Percent = string.Format("{0:#.##}%", (s.Count() / qtdAtendimentos) * 100),
                        FPercent = (s.Count() / qtdAtendimentos) * 100
                    }).ToList();

                    var resumoTiposLeito = atendimentosLeitos.GroupBy(g => g.Leito.TipoAcomodacaoId).Select(s => new ResumoTipoLeito
                    {
                        Qtd = s.Count(),
                        TpLeito = s.FirstOrDefault()?.Leito?.TipoAcomodacao?.Descricao,
                        Extra = s.Count(x => x.Leito.Extra),
                        Livre = s.Count(x => x.Leito.LeitoStatusId == 1),
                        ExtraLivre = s.Count(x => x.Leito.Extra && x.Leito.LeitoStatusId == 1),
                        Bloqueado = s.Count(x => x.Leito.LeitoStatusId != 1 && x.Leito.LeitoStatusId != 2),
                        Ocupado = s.Count(x => x.Leito.LeitoStatusId == 2),
                        OcupacaoTpLeito = string.Format("{0:#.##}%", (s.Count(x => x.Leito.LeitoStatusId == 2) / s.Count()) * 100),
                        OcupacaoTotal = string.Format("{0:#.##}%", (s.Count(x => x.Leito.LeitoStatusId == 2) / leitos.Count()) * 100),
                        Percent = string.Format("{0:#.##}%", (s.Count() / qtdAtendimentos) * 100),
                        FPercent = (s.Count() / qtdAtendimentos) * 100
                    }).ToList();

                    foreach (var leito in leitos)
                    {
                        TesteObjeto obj;

                        var m = atendimentosLeitos.Where(w => leito.Id == w.Leito.Id).FirstOrDefault();

                        if (m != null)
                        {
                            obj = new TesteObjeto
                            {
                                CodAtendimento = m.Codigo,
                                CodPaciente = (m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente)) == "0" ? "" : m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente),
                                Convenio = m.Convenio == null ? string.Empty : m.Convenio.NomeFantasia,//.Substring(0,),
                                Plano = m.Plano == null ? string.Empty : m.Plano.Descricao,
                                DataInternacao = (m.DataRegistro == null || m.DataRegistro == DateTime.MinValue) ? string.Empty : string.Format("{0:dd/MM/yyyy HH:mm}", m.DataRegistro),
                                Empresa = m.Empresa == null ? string.Empty : m.Empresa.NomeFantasia,//.Substring(0, 15),
                                Leito = m.Leito == null ? string.Empty : m.Leito.Descricao,
                                Medico = m.Medico == null ? string.Empty : m.Medico.NomeCompleto,
                                Origem = m.Origem == null ? string.Empty : m.Origem.Descricao,
                                Paciente = m.Paciente == null ? string.Empty : m.Paciente.NomeCompleto,
                                Idade = (m.Paciente == null || m.Paciente.Nascimento == null || m.Paciente.Nascimento == DateTime.MinValue) ? string.Empty : this.obterIdade(m.Paciente.Nascimento),
                                DiasInternado = (m.DataRegistro == null || m.DataRegistro == DateTime.MinValue) ? string.Empty : Convert.ToString(Math.Round(DateTime.Now.Subtract(m.DataRegistro).TotalDays, 0)),
                                UnidOrganizacional = m.Leito == null ? string.Empty : m.Leito.UnidadeOrganizacional.Descricao
                            };

                            dados.Lista.Add(obj);
                        }
                        else
                        {
                            obj = new TesteObjeto
                            {
                                CodAtendimento = "",
                                CodPaciente = "",
                                Convenio = "",
                                DataInternacao = "",
                                Empresa = "",
                                Leito = leito.Descricao,
                                Medico = "",
                                Origem = "",
                                Paciente = leito.LeitoStatus?.Descricao,
                                Idade = "",
                                DiasInternado = "",
                                UnidOrganizacional = leito == null ? string.Empty : leito.UnidadeOrganizacional.Descricao
                            };

                            dados.Lista.Add(obj);
                        }

                    }

                    if (dados != null)
                    {
                        var nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        var titulo = new ReportParameter("Titulo", dados.Titulo);
                        var usuario = new ReportParameter("Usuario", dados.NomeUsuario);
                        var dataHora = new ReportParameter("DataHora", dados.DataHora);
                        reportViewer.LocalReport.SetParameters(new ReportParameter[] { nomeHospital, titulo, usuario, dataHora });

                        var relDS = new Web.Relatorios.Atendimento.Atendimento();
                        var tabela = this.ConvertToDataTable(dados.Lista, relDS.Tables["AtendimentoDS"]);

                        var resumoConveniosTabela = this.ConvertToDataTable(resumoConvenios, relDS.Tables["ResumoConveniosDS"]);
                        var resumoTiposLeitoTabela = this.ConvertToDataTable(resumoTiposLeito, relDS.Tables["ResumoTipoLeitos"]);

                        var dataSource = new ReportDataSource();
                        dataSource.Value = tabela;
                        dataSource.Name = "RelatorioInternado";

                        var ResumoConveniosDS = new ReportDataSource();
                        ResumoConveniosDS.Value = resumoConveniosTabela;
                        ResumoConveniosDS.Name = "ResumoConvenios";

                        var ResumoTipoLeitos = new ReportDataSource();
                        ResumoTipoLeitos.Value = resumoTiposLeitoTabela;
                        ResumoTipoLeitos.Name = "ResumoTipoLeitos";


                        reportViewer.LocalReport.DataSources.Clear();
                        reportViewer.LocalReport.DataSources.Add(dataSource);
                        reportViewer.LocalReport.DataSources.Add(ResumoConveniosDS);
                        reportViewer.LocalReport.DataSources.Add(ResumoTipoLeitos);


                        reportViewer.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;



                        //reportViewer.LocalReport.DataSources.Add(dataSource3);
                        reportViewer.LocalReport.Refresh();

                        var mimeType = string.Empty;
                        var encoding = string.Empty;
                        var extension = "pdf";

                        string[] streamIds;
                        Warning[] warnings;
                        var pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                        var absPath = string.Concat(this.Server.MapPath("/"), @"temp\");
                        var path = string.Empty;
                        var file = string.Empty;
                        var pathReturn = string.Empty;

                        file = string.Concat("RelatorioInternado-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                        path = string.Concat(absPath, file);
                        pathReturn = this.Url.Content("~/temp/" + file);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                        fs.Write(pdfBytes, 0, pdfBytes.Length);
                        fs.Close();

                        reportViewer.LocalReport.Refresh();

                        this.Response.Headers.Add("Content-Disposition", string.Format("inline; filename=relatorio_leitos-{0}.pdf", Guid.NewGuid().ToString()));

                        return this.Content(pathReturn);
                    }
                }
            }
            catch (Exception e)
            {
                using (var erroRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Erro, long>>())
                {
                    await erroRepository.Object.InsertAsync(new Erro(e)).ConfigureAwait(false);
                }
            }

            return null;
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            var _sender = (LocalReport)sender;
            var relDS = new Web.Relatorios.Atendimento.Atendimento();

            if (e.ReportPath.Contains("subrelatoriointernadoresumoconvenio"))
            {
                e.DataSources.Add(new ReportDataSource("ResumoConveniosDS", _sender.DataSources["ResumoConvenios"].Value));
            }
            else
            {
                e.DataSources.Add(new ReportDataSource("ResumoTipoLeitos", _sender.DataSources["ResumoTipoLeitos"].Value));
            }
        }

        [HttpPost]
        public async Task<ActionResult> ReltorioLeitosPdfSintetico(long? empresaId = 0, List<long?> conveniosId = null, long? unidadeOrganizacionalId = 0, long? statusLeito = 0)
        {
            var novaLista = new List<TesteObjeto>();
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var leitoAppService = IocManager.Instance.ResolveAsDisposable<ILeitoAppService>())
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                using (var userEmpresas = IocManager.Instance.ResolveAsDisposable<IRepository<UserEmpresa, long>>())
                {
                    var reportViewer = new ReportViewer();
                    var scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(reportViewer);
                    reportViewer.LocalReport.ReportPath = string.Concat(this.Server.MapPath("~"), @"\Relatorios\Atendimento\RelatorioInternadosSintetico.rdlc");

                    //localização do relatório
                    var dados = await this.CarregarIndex(empresaId).ConfigureAwait(false);

                    var leitos = await leitoAppService.Object.ListarParaRelatorioMapaLeitos(dados.Empresa, statusLeito).ConfigureAwait(false);

                    var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);

                    if (leitos.Count != 0)
                    {
                        dados.Titulo = string.Concat("Mapa Diário - ", string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now));

                        //if (empresaId != 0)
                        //{
                        //    dados.NomeHospital = relatorioInternacao.Items[0].Empresa.NomeFantasia.ToString();
                        //}
                        //else
                        //{
                        //    dados.NomeHospital = "";
                        //}

                        dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                        dados.DataHora = string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);

                        dados.Lista = new List<TesteObjeto>();

                        leitos = leitos.OrderBy(o => o.UnidadeOrganizacional.Descricao).ToList();

                        var leitosid = leitos.Select(s => s.Id);

                        var empresasId = userEmpresas.Object
                            .GetAll()
                            .Where(m => m.UserId == this.AbpSession.UserId).Select(s => s.EmpresaId);

                        if (conveniosId != null && conveniosId.Count == 1 && conveniosId[0] == null)
                        {
                            conveniosId = new List<long?>();
                        }


                        var atendimentosLeitos = atendimentoRepository.Object
                                .GetAll()
                                .Include(x => x.Paciente)
                                .Include(x => x.Paciente.SisPessoa)
                                .Include(x => x.Medico)
                                .Include(x => x.Medico.SisPessoa)
                                .Include(x => x.AtendimentoTipo)
                                .Include(x => x.Convenio)
                                .Include(x => x.Convenio.SisPessoa)
                                .Include(x => x.Empresa)
                                .Include(x => x.Especialidade)
                                .Include(x => x.Guia)
                                .Include(x => x.Leito)
                                .Include(x => x.Leito.UnidadeOrganizacional)
                                .Include(x => x.Leito.LeitoStatus)
                                .Include(x => x.MotivoAlta)
                                .Include(x => x.Nacionalidade)
                                .Include(x => x.Origem)
                                .Include(x => x.Plano)
                                .Include(x => x.ServicoMedicoPrestado)
                                .Include(x => x.UnidadeOrganizacional)
                                .Where(a => a.IsInternacao == true)
                                .Where(a => a.DataAlta == null)
                                .Where(a => (empresaId == 0 || a.EmpresaId == empresaId) && a.DataAlta == null)
                                .Where(a => empresasId.Any(s => s == a.EmpresaId))
                                .Where(a => (unidadeOrganizacionalId == 0 || a.UnidadeOrganizacionalId == unidadeOrganizacionalId))
                                .Where(a => !conveniosId.Any() || conveniosId.Any(s => s == a.ConvenioId))
                                .Where(a => leitosid.Any(s => s == a.LeitoId))
                                .ToList();


                        foreach (var leito in leitos)
                        {
                            TesteObjeto obj;

                            var m = atendimentosLeitos.Where(w => leito.Id == w.Leito.Id).FirstOrDefault();

                            if (m != null)
                            {
                                var med = m.Medico == null && m.Medico.SisPessoa != null ? string.Empty : m.Medico.SisPessoa.NomeCompleto;

                                if (string.IsNullOrEmpty(med))
                                {
                                    med = m.Medico == null && m.Medico.SisPessoa != null ? string.Empty : m.Medico.SisPessoa.NomeFantasia;

                                }

                                try
                                {
                                    if (med.Length > 31)
                                        med = med.Substring(0, 31);
                                }
                                catch { }

                                obj = new TesteObjeto
                                {
                                    CodAtendimento = m.Codigo,
                                    CodPaciente = (m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente)) == "0" ? "" : m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente),
                                    Convenio = m.Convenio == null ? string.Empty : m.Convenio.NomeFantasia,//.Substring(0,),
                                    Plano = m.Plano == null ? string.Empty : m.Plano.Descricao,//.Substring(0,),
                                    DataInternacao = (m.DataRegistro == null || m.DataRegistro == DateTime.MinValue) ? string.Empty : string.Format("{0:dd/MM/yyyy HH:mm}", m.DataRegistro),
                                    Empresa = m.Empresa == null ? string.Empty : m.Empresa.NomeFantasia,//.Substring(0, 15),
                                    Leito = m.Leito == null ? string.Empty : m.Leito.Descricao,
                                    Medico = med,
                                    Origem = m.Plano == null ? string.Empty : m.Plano.Descricao,//NO SINTETICO QUE EXIBE PLANO SOBRESCREVE O CAMPO ORIGEM
                                    Paciente = m.Paciente == null ? string.Empty : m.Paciente.NomeCompleto,
                                    Idade = (m.Paciente == null || m.Paciente.Nascimento == null || m.Paciente.Nascimento == DateTime.MinValue) ? string.Empty : this.obterIdade(m.Paciente.Nascimento),
                                    DiasInternado = (m.DataRegistro == null || m.DataRegistro == DateTime.MinValue) ? string.Empty : Convert.ToString(Math.Round(DateTime.Now.Subtract(m.DataRegistro).TotalDays, 0)),
                                    UnidOrganizacional = m.Leito == null ? string.Empty : m.Leito.UnidadeOrganizacional.Descricao
                                };

                                dados.Lista.Add(obj);

                            }
                            else
                            {
                                obj = new TesteObjeto
                                {
                                    CodAtendimento = "",
                                    CodPaciente = "",
                                    Convenio = "",
                                    DataInternacao = "",
                                    Empresa = "",
                                    Leito = leito.Descricao,
                                    Medico = "",
                                    Origem = "",
                                    Paciente = leito.LeitoStatus.Descricao,
                                    Idade = "",
                                    DiasInternado = "",
                                    UnidOrganizacional = leito == null ? string.Empty : leito.UnidadeOrganizacional.Descricao
                                };

                                dados.Lista.Add(obj);
                            }
                        }

                        if (dados != null)
                        {
                            var nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                            var titulo = new ReportParameter("Titulo", dados.Titulo);
                            var usuario = new ReportParameter("Usuario", dados.NomeUsuario);
                            var dataHora = new ReportParameter("DataHora", dados.DataHora);
                            reportViewer.LocalReport.SetParameters(new ReportParameter[] { nomeHospital, titulo, usuario, dataHora });

                            var relDS = new Web.Relatorios.Atendimento.Atendimento();

                            var tabela = this.ConvertToDataTable(dados.Lista, relDS.Tables["AtendimentoDS"]);

                            var dataSource = new ReportDataSource();

                            dataSource.Value = tabela;
                            dataSource.Name = "relatorio_leitos_sintetico_dataset";
                            reportViewer.LocalReport.DataSources.Clear();
                            reportViewer.LocalReport.DataSources.Add(dataSource);
                            reportViewer.LocalReport.Refresh();

                            var mimeType = string.Empty;
                            var encoding = string.Empty;
                            var extension = "pdf";

                            string[] streamIds;
                            Warning[] warnings;
                            var pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


                            var absPath = string.Concat(this.Server.MapPath("/"), @"temp\");
                            var path = string.Empty;
                            var file = string.Empty;
                            var pathReturn = string.Empty;

                            file = string.Concat("RelatorioInternado-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                            path = string.Concat(absPath, file);
                            pathReturn = this.Url.Content("~/temp/" + file);

                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                            fs.Write(pdfBytes, 0, pdfBytes.Length);
                            fs.Close();

                            reportViewer.LocalReport.Refresh();

                            this.Response.Headers.Add("Content-Disposition", string.Format("inline; filename=relatorio_leitos-{0}.pdf", Guid.NewGuid().ToString()));

                            return this.Content(pathReturn);

                        }
                    }
                }
            }
            catch (Exception e)
            {
                using (var erroRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Erro, long>>())
                {
                    await erroRepository.Object.InsertAsync(new Erro(e)).ConfigureAwait(false);
                }
            }

            return null;
        }


        /// <summary>
        /// Entrada para filtro de visualização do Report de produtos
        /// </summary>
        /// <returns></returns>
        //GET: Mpa/Relatorios/RelatorioInternado
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public async Task<ActionResult> Index()
        {
            var result = await this.CarregarIndex(0).ConfigureAwait(false);
            //result.EhMovimentacao = false;
            return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Relatorios/Index.cshtml", result);
        }


        /// <summary>
        /// Entrada para filtro de visualização do Report de Atendimentos - LM
        /// </summary>
        /// <returns></returns>
        //GET: Mpa/AtendimentoRelatorio/RelatorioAtendimento
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento)]
        public async Task<ActionResult> RelatorioAtendimento()
        {
            var result = new RptAtendimentoViewModel();
            // result.EhMovimentacao = false;
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Relatorios/IndexAtendimento.cshtml", result);
        }


        /// <summary>
        /// PartialView que renderiza o relatório com os filtros selecionados no formulário
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public async Task<ActionResult> Visualizar(FiltroModel filtro)
        {
            using (var relatorioAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioAtendimentoAppService>())
            using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            {
                var relatorioInternacao = await relatorioAtendimentoAppService.Object.ListarRelatorio(filtro.Empresa, filtro.ConvenioId, filtro.EspecilidadeId, filtro.MedicoId).ConfigureAwait(false);

                var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);
                //foreach (var item in relatorioInternacao.Items)
                //{
                //    //Math.Truncate(decimalNumber)
                //    string teste = Convert.ToString(Math.Round(DateTime.Now.Subtract(item.DataRegistro).TotalDays, 0));
                //   //decimal teste2 = Math.Truncate(Convert.ToDecimal(teste));
                //}
                var _filtro = new FiltroModel();
                if (relatorioInternacao.Items.Count != 0)
                {
                    _filtro.Titulo = string.Concat("Mapa Diário - ", Convert.ToString(DateTime.Now));
                    //_filtro.NomeHospital = "Lipp";
                    _filtro.NomeHospital = relatorioInternacao.Items[0].Empresa.NomeFantasia.ToString();
                    _filtro.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                    _filtro.DataHora = Convert.ToString(DateTime.Now);


                    _filtro.Lista = relatorioInternacao.Items.Select(m => new TesteObjeto
                    {
                        CodAtendimento = m.Codigo,
                        CodPaciente = m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente),
                        Convenio = m.Convenio == null ? string.Empty : m.Convenio.NomeFantasia,
                        DataInternacao = m.DataRegistro == null ? string.Empty : m.DataRegistro.ToString(),
                        Empresa = m.Empresa == null ? string.Empty : m.Empresa.NomeFantasia,
                        Leito = m.Leito == null ? string.Empty : m.Leito.Descricao,
                        Medico = m.Medico == null ? string.Empty : m.Medico.NomeCompleto,
                        Origem = m.Origem == null ? string.Empty : m.Origem.Descricao,
                        Paciente = m.Paciente == null ? string.Empty : m.Paciente.NomeCompleto,
                        UnidOrganizacional = m.UnidadeOrganizacional == null ? string.Empty : m.UnidadeOrganizacional.Descricao,
                        Idade = !(m.Paciente == null || m.Paciente.Nascimento == null && m.Paciente.Nascimento == DateTime.MinValue) ? string.Empty : obterIdade(m.Paciente.Nascimento.Value),
                        DiasInternado = m.DataRegistro == null ? string.Empty : Convert.ToString(Math.Round(DateTime.Now.Subtract(m.DataRegistro).TotalDays, 0))
                    }
                    ).ToList();
                }

                return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Relatorios/RelatorioInternado.aspx", _filtro);
            }
        }

        private async Task<FiltroModel> CarregarIndex(long? empresaId)
        {
            using (var relatorioAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioAtendimentoAppService>())
            using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            {
                var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);

                var userId = this.AbpSession.UserId;
                //var userEmpresas = _userAppService.GetUserEmpresas(userId.Value);
                // var userEmpresas = _relatorioAtendimentoAppService.ListarEmpresaUsuario(userId.Value);

                var result = new FiltroModel();

                result.Empresas = (relatorioAtendimentoAppService.Object.ListarEmpresaUsuario(userId.Value))
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.Nome
                    }).ToList();

                var padrao = new SelectListItem { Text = "Selecione", Value = "0" };
                result.Empresas.Insert(0, padrao);
                result.Empresa = empresaId ?? 0;
                return result;
            }
        }


        //Gustavo Rosa 15/05/2018
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento)]
        public async Task<ActionResult> IndexRelatorioAtendimento()
        {
            var result = await this.CarregarIndex().ConfigureAwait(false);
            //result.EhMovimentacao = false;
            return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Relatorios/IndexRelatorioAtendimento.cshtml", result);
        }

        /// <summary>
        /// PartialView que renderiza o relatório com os filtros selecionados no formulário
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento)]
        public async Task<ActionResult> Gerar(FiltroModel filtro)
        {
            using (var relatorioAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioAtendimentoAppService>())
            using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            {
                var relatorioAtendimento = await relatorioAtendimentoAppService.Object.ListarRelatorio(filtro.Empresa, filtro.ConvenioId, filtro.MedicoId, filtro.EspecilidadeId).ConfigureAwait(false);

                var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);
                //foreach (var item in relatorioInternacao.Items)
                //{
                //    //Math.Truncate(decimalNumber)
                //    string teste = Convert.ToString(Math.Round(DateTime.Now.Subtract(item.DataRegistro).TotalDays, 0));
                //   //decimal teste2 = Math.Truncate(Convert.ToDecimal(teste));
                //}
                var _filtro = new FiltroModel();
                if (relatorioAtendimento.Items.Count != 0)
                {
                    _filtro.Titulo = string.Concat("Relatório de Atendimento - ", Convert.ToString(DateTime.Now));
                    //_filtro.NomeHospital = "Lipp";
                    _filtro.NomeHospital = relatorioAtendimento.Items[0].Empresa.NomeFantasia.ToString();
                    _filtro.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                    _filtro.DataHora = Convert.ToString(DateTime.Now);


                    _filtro.Lista = relatorioAtendimento.Items.Select(m => new TesteObjeto
                    {
                        CodAtendimento = m.Codigo,
                        CodPaciente = m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente),
                        Convenio = m.Convenio == null ? string.Empty : m.Convenio.NomeFantasia,
                        DataInternacao = m.DataRegistro == null ? string.Empty : m.DataRegistro.ToString(),
                        Empresa = m.Empresa == null ? string.Empty : m.Empresa.NomeFantasia,
                        Leito = m.Leito == null ? string.Empty : m.Leito.Descricao,
                        Medico = m.Medico == null ? string.Empty : m.Medico.NomeCompleto,
                        Origem = m.Origem == null ? string.Empty : m.Origem.Descricao,
                        Paciente = m.Paciente == null ? string.Empty : m.Paciente.NomeCompleto,
                        UnidOrganizacional = m.UnidadeOrganizacional == null ? string.Empty : m.UnidadeOrganizacional.Descricao,
                        Idade = !(m.Paciente == null || m.Paciente.Nascimento == null && m.Paciente.Nascimento == DateTime.MinValue) ? string.Empty : obterIdade(m.Paciente.Nascimento.Value),
                        DiasInternado = m.DataRegistro == null ? string.Empty : Convert.ToString(Math.Round(DateTime.Now.Subtract(m.DataRegistro).TotalDays, 0))
                    }).ToList();
                }

                return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Relatorios/RelatorioAtendimento.aspx", _filtro);
            }
        }


        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioAtendimento)]
        public async Task<ActionResult> GerarPDF(FiltroModel filtro)
        {
            try
            {
                using (var relatorioAtendimentoAppService = IocManager.Instance.ResolveAsDisposable<IRelatorioAtendimentoAppService>())
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                {
                    var reportViewer = new ReportViewer();
                    var scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(reportViewer);
                    reportViewer.LocalReport.ReportPath = string.Concat(
                        this.Server.MapPath("~"),
                        @"\Relatorios\Atendimento\RelatorioAtendimento.rdlc");

                    //localização do relatório
                    var dados = await this.CarregarIndex().ConfigureAwait(false);
                    var relatorioAtendimento = await relatorioAtendimentoAppService.Object.ListarRelatorio(
                                                   dados.Empresa,
                                                   dados.ConvenioId,
                                                   dados.EspecilidadeId,
                                                   dados.MedicoId).ConfigureAwait(false);
                    var loginInformations =
                        await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);
                    var list = new List<RelatorioAtendimentoDto>();
                    foreach (var m in relatorioAtendimento.Items)
                    {
                        list.Add(
                            new RelatorioAtendimentoDto()
                            {
                                Convenio = m.Convenio == null ? string.Empty : m.Convenio.NomeFantasia,
                                DataAtendimento = m.DataRegistro.ToString("dd/MM/yyyy"),
                                Especialidade = m.Especialidade == null ? string.Empty : m.Especialidade.Descricao,
                                Paciente = m.Paciente == null ? string.Empty : m.Paciente.NomeCompleto,
                                Medico = m.Medico == null ? string.Empty : m.Medico.NomeCompleto,
                                TipoAtendimento = m.IsInternacao ? "I" : "A",
                                Empresa = m.Empresa == null ? string.Empty : m.Empresa.NomeFantasia,
                                UnidadeOrganizacional =
                                        m.UnidadeOrganizacional == null ? string.Empty : m.UnidadeOrganizacional.Descricao

                            });
                    }

                    if (relatorioAtendimento.Items.Count != 0)
                    {
                        dados.Titulo = string.Concat("Relatório de Atendimento - ", Convert.ToString(DateTime.Now));
                        dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                        dados.DataHora = Convert.ToString(DateTime.Now);

                    }

                    if (dados != null)
                    {
                        var nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        var titulo = new ReportParameter("Titulo", dados.Titulo);
                        var usuario = new ReportParameter("Usuario", dados.NomeUsuario);
                        var dataHora = new ReportParameter("DataHora", dados.DataHora);
                        var convenio = new ReportParameter("Convenio", dados.Convenio);
                        var dataAtendimento = new ReportParameter("DataAtendimento", dados.DataAtendimento.ToString());
                        var tipoAtendimento = new ReportParameter("TipoAtendimento", dados.TipoAtendimento);
                        var paciente = new ReportParameter("Paciente", dados.Paciente);
                        var medico = new ReportParameter("Medico", dados.Medico);
                        var especilidade = new ReportParameter("Especialidade", dados.Especilidade);
                        var unidadeOrganizacional = new ReportParameter(
                            "UnidadeOrganizacional",
                            dados.UnidadeOrganizacional);

                        reportViewer.LocalReport.SetParameters(
                            new ReportParameter[]
                                {
                                nomeHospital,
                                titulo,
                                usuario,
                                dataHora,
                                convenio,
                                dataAtendimento,
                                tipoAtendimento,
                                paciente,
                                medico,
                                especilidade,
                                unidadeOrganizacional
                                });

                        var relDS = new Web.Relatorios.Atendimento.RelatorioAtendimentoDS();
                        var tabela = this.ConvertToDataTable(list, relDS.Tables["Atendimento"]);
                        var dataSource = new ReportDataSource();
                        dataSource.Value = tabela;
                        dataSource.Name = "RelatorioAtendimento";
                        reportViewer.LocalReport.DataSources.Clear();
                        reportViewer.LocalReport.DataSources.Add(dataSource);
                        reportViewer.LocalReport.Refresh();

                        var mimeType = string.Empty;
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
                        reportViewer.LocalReport.Refresh();
                        //reportViewer.Dispose();
                        this.Response.Headers.Add("Content-Disposition", "inline; filename=relatorio-atendimento.pdf");
                        return this.File(pdfBytes, "application/pdf");

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
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
            {
                var loginInformations = await sessionAppService.Object.GetCurrentLoginInformations().ConfigureAwait(false);

                var userId = this.AbpSession.UserId;

                var result = new FiltroModel();
                var empresas = await userAppService.Object.GetUserEmpresas(userId.Value).ConfigureAwait(false);
                result.Empresas = empresas.Items
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = s.NomeFantasia
                    }).ToList();

                //var padrao = new SelectListItem { Text = "Selecione", Value = "0" };
                //result.Empresas.Insert(0, padrao);
                return result;
            }
        }

        // Relatórios de Atendimento
        public ContentResult VisualizarRptAtendimentoDetalhadoPDF(RptAtendimentoViewModel input)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
                using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                {
                    var empresa = new EmpresaDto();
                    var usuarioEmpresas = Task.Run(() => userAppService.Object.GetUserEmpresas(AbpSession.UserId.Value)).Result;
                    if (input.EmpresaId.HasValue && input.EmpresaId.Value > 0)
                    {
                        empresa = Task.Run(() => empresaAppService.Object.Obter(input.EmpresaId.Value)).Result;
                    }
                    else
                    {
                        empresa = usuarioEmpresas.Items.FirstOrDefault();
                    }

                    var usuario = Task.Run(() => userAppService.Object.GetUser()).Result;
                    var loginInformations = Task.Run(() => sessionAppService.Object.GetCurrentLoginInformations()).Result;
                    var dados = new FiltroModel();
                    dados.Titulo = "Relatório de atendimentos " + (input.TipoRel < 4 ? "resumido" : "detalhado");
                    dados.NomeHospital = empresa.NomeFantasia;
                    dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                    dados.DataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    dados.StartDate = input.StartDate.ToString("dd/MM/yyyy");
                    dados.EndDate = input.EndDate.ToString("dd/MM/yyyy");
                    dados.TipoPeriodo = (input.TipoPeriodo == 1 ? "Atendidos" : "Altas");
                    dados.Filtrado = (input.Filtrado);

                    var reportViewer = new ReportViewer();
                    // Obtido do ASPX
                    var scriptManager = new ScriptManager();

                    switch (input.TipoRel)
                    {
                        case 4:
                            if (input.TipoPeriodo == 1)
                            {
                                reportViewer.LocalReport.ReportPath = string.Concat(
                                    this.Server.MapPath("~"),
                                    @"Relatorios\Atendimento\DetalhadoData.rdlc");
                                dados.Titulo += " por data do atendimento";
                            }
                            else
                            {
                                reportViewer.LocalReport.ReportPath = string.Concat(
                                    this.Server.MapPath("~"),
                                    @"Relatorios\Atendimento\DetalhadoAlta.rdlc");
                                dados.Titulo += " por data de alta";
                            }

                            break;
                        case 5:
                            reportViewer.LocalReport.ReportPath = string.Concat(
                                this.Server.MapPath("~"),
                                @"Relatorios\Atendimento\DetalhadoConvenio.rdlc");
                            dados.Titulo += " por convênio";
                            break;
                        default:
                            reportViewer.LocalReport.ReportPath = string.Concat(
                                this.Server.MapPath("~"),
                                @"Relatorios\Atendimento\DetalhadoMedico.rdlc");
                            dados.Titulo += " por médico";
                            break;
                    }

                    if (dados != null)
                    {
                        //parâmetros para o relatório
                        var nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        var nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                        var titulo = new ReportParameter("Titulo", dados.Titulo);
                        var dataHora = new ReportParameter("DataHora", dados.DataHora);
                        var _startDate = new ReportParameter("StartDate", dados.StartDate);
                        var _endDate = new ReportParameter("EndDate", dados.EndDate);
                        var TipoPeriodo = new ReportParameter("TipoPeriodo", dados.TipoPeriodo);
                        var Filtrado = new ReportParameter("Filtrado", dados.Filtrado);


                        reportViewer.LocalReport.SetParameters(
                            new ReportParameter[]
                                {
                                nomeHospital, nomeUsuario, titulo, dataHora, _startDate, _endDate, TipoPeriodo,
                                Filtrado
                                });

                        //fonte de dados para o relatório - datasource
                        var list = Task.Run(
                            () => atendimentoAppService.Object.ListarAtendimentoDetalhadoReport(
                                startDate: input.StartDate,
                                endDate: input.EndDate,
                                medicoId: input.MedicoId.Value,
                                pacienteId: input.PacienteId.Value,
                                convenioId: input.ConvenioId.Value,
                                empresaId: input.EmpresaId.Value,
                                especialidadeId: input.EspecialidadeId.Value,
                                unidadeOrganizacionalId: input.UnidadeOrganizacionalId.Value,
                                tipoAtendimento: input.TipoAtendimento,
                                tipoRel: input.TipoRel,
                                tipoPeriodo: input.TipoPeriodo)).Result;
                        var listDto = list.Items;
                        var listDs = new List<AtendimentoDetalhadoDsDto>();
                        foreach (var item in listDto)
                        {
                            var _item = new AtendimentoDetalhadoDsDto();
                            _item.CodigoAtendimento = item.CodigoAtendimento;
                            _item.Atendimento = item.Atendimento;
                            _item.CodPaciente = item.CodPaciente;
                            _item.Paciente = item.Paciente;
                            _item.Unidade = item.Unidade;
                            _item.DataAtendimento = item.DataAtendimento.ToString("dd/MM/yyyy HH:mm");
                            _item.Convenio = item.Convenio;
                            _item.Medico = item.Medico;
                            _item.Empresa = item.Empresa;
                            _item.Origem = item.Origem;
                            _item.Especialidade = item.Especialidade;
                            _item.Plano = item.Plano;
                            _item.TipoAtendimento = item.TipoAtendimento;
                            _item.Guia = item.Guia;
                            _item.NumeroGuia = item.NumeroGuia;
                            _item.DataAlta = item.DataAlta.HasValue
                                                 ? item.DataAlta.Value.ToString("dd/MM/yyyy HH:mm")
                                                 : "";
                            _item.DataAltaMedica = item.DataAltaMedica.HasValue
                                                       ? item.DataAltaMedica.Value.ToString("dd/MM/yyyy HH:mm")
                                                       : "";
                            _item.Senha = item.Senha;
                            _item.Nascimento = item.Nascimento.HasValue
                                                   ? item.Nascimento.Value.ToString("dd/MM/yyyy")
                                                   : "";
                            _item.IdadeAno = item.IdadeAno;
                            listDs.Add(_item);
                        }

                        var relDS = new RelatorioAtendimentoDS();


                        var tabela = this.ConvertToDataTable(listDs, relDS.Tables["Detalhado"]);


                        // Logotipo
                        if (tabela.Rows.Count > 0)
                        {
                            tabela.Rows[0]["Logotipo"] = empresa.Logotipo;
                        }
                        // fim - logotipo

                        var dataSource = new ReportDataSource("Detalhado", tabela);

                        reportViewer.LocalReport.DataSources.Add(dataSource);

                        scriptManager.RegisterPostBackControl(reportViewer);

                        // Gerando PDF
                        var mimeType = "application/pdf"; //string.Empty;
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

                        //if (System.IO.File.Exists(@"C:\Temp\SaldoProduto.pdf"))
                        var absPath = string.Concat(this.Server.MapPath("/"), @"temp\");
                        var path = string.Empty;
                        var file = string.Empty;
                        var pathReturn = string.Empty;
                        switch (input.TipoRel)
                        {
                            case 4:
                                if (input.TipoPeriodo == 1)
                                {
                                    file = string.Concat(
                                        "AtendimentoDetalhadoData-",
                                        DateTime.Now.ToString("yyyyMMddHHmmss"),
                                        ".pdf");
                                    path = string.Concat(absPath, file);
                                    pathReturn = this.Url.Content("~/temp/" + file);

                                }
                                else
                                {
                                    file = string.Concat(
                                        "AtendimentoDetalhadoAlta-",
                                        DateTime.Now.ToString("yyyyMMddHHmmss"),
                                        ".pdf");
                                    path = string.Concat(absPath, file);
                                    pathReturn = this.Url.Content("~/temp/" + file);
                                }

                                break;
                            case 5:
                                file = string.Concat(
                                    "AtendimentoDetalhadoConvenio-",
                                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                                    ".pdf");
                                path = string.Concat(absPath, file);
                                pathReturn = this.Url.Content("~/temp/" + file);
                                break;
                            default:
                                file = string.Concat(
                                    "AtendimentoDetalhadoMedico-",
                                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                                    ".pdf");
                                path = string.Concat(absPath, file);
                                pathReturn = this.Url.Content("~/temp/" + file);
                                break;
                        }

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                        {
                            fs.Write(pdfBytes, 0, pdfBytes.Length);
                        }

                        //RegistroArquivo registroArquivo = new RegistroArquivo();
                        //var seq = _registroArquivoAppService.
                        //registroArquivo.Arquivo = pdfBytes;
                        //registroArquivo.RegistroTabelaId = (long)EnumArquivoTabela.SaldoProduto;
                        //registroArquivo.RegistroId = 1; //(long)filtro.PrescricaoId;
                        //var id = _registroArquivoRepository.InsertAndGetId(registroArquivo);

                        reportViewer.LocalReport.Refresh();
                        //reportViewer.Dispose();

                        this.Response.Headers.Add(
                            "Content-Disposition",
                            string.Format(
                                "inline; filename=AtendimentoDetalhado-{0}.pdf",
                                DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")));
                        return this.Content(pathReturn);
                        //return File(pdfBytes, "application/pdf");
                        //return File(pdfBytes, "application/pdf", @"c:\temp\SaldoProduto.pdf");
                        //return PartialView("~/areas/mpa/views/aplicacao/relatorios/_viewer.cshtml",)

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

        public ContentResult VisualizarRptAtendimentoResumidoPDF(RptAtendimentoViewModel input)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
                using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
                using (var sessionAppService = IocManager.Instance.ResolveAsDisposable<ISessionAppService>())
                {
                    var empresa = new EmpresaDto();
                    var usuarioEmpresas = Task.Run(() => userAppService.Object.GetUserEmpresas(AbpSession.UserId.Value)).Result;
                    if (input.EmpresaId.HasValue && input.EmpresaId.Value > 0)
                    {
                        empresa = Task.Run(() => empresaAppService.Object.Obter(input.EmpresaId.Value)).Result;
                    }
                    else
                    {
                        empresa = usuarioEmpresas.Items.FirstOrDefault();
                    }
                    var usuario = Task.Run(() => userAppService.Object.GetUser()).Result;
                    var loginInformations = Task.Run(() => sessionAppService.Object.GetCurrentLoginInformations()).Result;
                    var dados = new FiltroModel();
                    dados.Titulo = "Relatório de atendimentos " + (input.TipoRel < 4 ? "resumido" : "detalhado");
                    dados.NomeHospital = empresa.NomeFantasia;
                    dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                    dados.DataHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    dados.StartDate = input.StartDate.ToString("dd/MM/yyyy");
                    dados.EndDate = input.EndDate.ToString("dd/MM/yyyy");
                    dados.TipoPeriodo = (input.TipoPeriodo == 1 ? "Atendidos" : "Altas");
                    dados.Filtrado = (input.Filtrado);
                    // Obtido do ASPX
                    var reportViewer = new ReportViewer();
                    var scriptManager = new ScriptManager();

                    switch (input.TipoRel)
                    {
                        case 1:
                            reportViewer.LocalReport.ReportPath = string.Concat(
                                this.Server.MapPath("~"),
                                @"Relatorios\Atendimento\ResumidoConvenio.rdlc");
                            dados.Titulo += " por convênio";
                            break;
                        case 2:
                            reportViewer.LocalReport.ReportPath = string.Concat(
                                this.Server.MapPath("~"),
                                @"Relatorios\Atendimento\ResumidoMedico.rdlc");
                            dados.Titulo += " por médico";
                            break;
                        default:
                            reportViewer.LocalReport.ReportPath = string.Concat(
                                this.Server.MapPath("~"),
                                @"Relatorios\Atendimento\ResumidoEspecialidade.rdlc");
                            dados.Titulo += " por especialidade";
                            break;
                    }

                    if (dados != null)
                    {
                        //parâmetros para o relatório
                        var nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        var nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                        var titulo = new ReportParameter("Titulo", dados.Titulo);
                        var dataHora = new ReportParameter("DataHora", dados.DataHora);
                        var _startDate = new ReportParameter("StartDate", dados.StartDate);
                        var _endDate = new ReportParameter("EndDate", dados.EndDate);
                        var TipoPeriodo = new ReportParameter("TipoPeriodo", dados.TipoPeriodo);
                        var Filtrado = new ReportParameter("Filtrado", dados.Filtrado);

                        reportViewer.LocalReport.SetParameters(
                            new ReportParameter[]
                                {
                                nomeHospital, nomeUsuario, titulo, dataHora, _startDate, _endDate, TipoPeriodo,
                                Filtrado
                                });

                        //fonte de dados para o relatório - datasource
                        var list = Task.Run(
                            () => atendimentoAppService.Object.ListarAtendimentoResumidoReport(
                                startDate: input.StartDate,
                                endDate: input.EndDate,
                                medicoId: input.MedicoId.Value,
                                pacienteId: input.PacienteId.Value,
                                convenioId: input.ConvenioId.Value,
                                empresaId: input.EmpresaId.Value,
                                especialidadeId: input.EspecialidadeId.Value,
                                unidadeOrganizacionalId: input.UnidadeOrganizacionalId.Value,
                                tipoAtendimento: input.TipoAtendimento,
                                tipoRel: input.TipoRel,
                                tipoPeriodo: input.TipoPeriodo)).Result;
                        var listDto = list.Items;
                        var listDs = new List<AtendimentoResumidoDsDto>();
                        foreach (var item in listDto)
                        {
                            var _item = new AtendimentoResumidoDsDto
                            {
                                Convenio = item.Convenio,
                                Medico = item.Medico,
                                Empresa = item.Empresa,
                                Especialidade = item.Especialidade,
                                Plano = item.Plano,
                                AmbulatorioEmergencia = item.AmbulatorioEmergencia.ToString(),
                                Atendimentos = item.Atendimentos.ToString(),
                                ComAlta = item.ComAlta.ToString(),
                                HomeCare = item.HomeCare.ToString(),
                                Indefinidos = item.Indefinidos.ToString(),
                                Internacoes = item.Internacoes.ToString(),
                                InternacoesAtivas = item.InternacoesAtivas.ToString(),
                                PreAtendimentos = item.PreAtendimentos.ToString(),
                                SemAlta = item.SemAlta.ToString()
                            };

                            listDs.Add(_item);
                        }

                        var relDS = new RelatorioAtendimentoDS();
                        var tabela = this.ConvertToDataTable(listDs, relDS.Tables["Resumido"]);


                        // Logotipo
                        if (tabela.Rows.Count > 0)
                        {
                            tabela.Rows[0]["Logotipo"] = empresa.Logotipo;
                        }
                        // fim - logotipo

                        var dataSource = new ReportDataSource("Resumido", tabela);

                        reportViewer.LocalReport.DataSources.Add(dataSource);

                        scriptManager.RegisterPostBackControl(reportViewer);

                        // Gerando PDF
                        var mimeType = "application/pdf"; //string.Empty;
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

                        //if (System.IO.File.Exists(@"C:\Temp\SaldoProduto.pdf"))
                        var absPath = string.Concat(this.Server.MapPath("/"), @"temp\");
                        var path = string.Empty;
                        var file = string.Empty;
                        var pathReturn = string.Empty;
                        switch (input.TipoRel)
                        {
                            case 1:
                                file = string.Concat(
                                    "AtendimentoResumidoConvenio-",
                                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                                    ".pdf");
                                path = string.Concat(absPath, file);
                                pathReturn = this.Url.Content("~/temp/" + file);
                                break;
                            case 2:
                                file = string.Concat(
                                    "AtendimentoResumidoMedico-",
                                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                                    ".pdf");
                                path = string.Concat(absPath, file);
                                pathReturn = this.Url.Content("~/temp/" + file);
                                break;
                            default:
                                file = string.Concat(
                                    "AtendimentoResumidoEspecialidade-",
                                    DateTime.Now.ToString("yyyyMMddHHmmss"),
                                    ".pdf");
                                path = string.Concat(absPath, file);
                                pathReturn = this.Url.Content("~/temp/" + file);
                                break;
                        }

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        using (var fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write))
                        {
                            fs.Write(pdfBytes, 0, pdfBytes.Length);
                        }

                        //RegistroArquivo registroArquivo = new RegistroArquivo();
                        //var seq = _registroArquivoAppService.
                        //registroArquivo.Arquivo = pdfBytes;
                        //registroArquivo.RegistroTabelaId = (long)EnumArquivoTabela.SaldoProduto;
                        //registroArquivo.RegistroId = 1; //(long)filtro.PrescricaoId;
                        //var id = _registroArquivoRepository.InsertAndGetId(registroArquivo);

                        reportViewer.LocalReport.Refresh();
                        //reportViewer.Dispose();

                        this.Response.Headers.Add(
                            "Content-Disposition",
                            string.Format(
                                "inline; filename=AtendimentoResumido-{0}.pdf",
                                DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss")));

                        return this.Content(pathReturn);
                        //return File(pdfBytes, "application/pdf");
                        //return File(pdfBytes, "application/pdf", @"c:\temp\SaldoProduto.pdf");
                        //return PartialView("~/areas/mpa/views/aplicacao/relatorios/_viewer.cshtml",)

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


        public ActionResult IndexRelatorioAgendamentoCirurgico()
        {
            return this.View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Relatorios/IndexAgendamentoCirurgico.cshtml");
        }

        public FileResult ImprimiRelatorioAgendamentoCirurgico(DateTime dataInicial, DateTime dataFinal)
        {

            using(var agendamentoConsultaAppService = IocManager.Instance.ResolveAsDisposable<IAgendamentoConsultaAppService>() )
            {
                return this.File(agendamentoConsultaAppService.Object.RetornaArquivoAgendamentoCirurgico(dataInicial, dataFinal), "application/pdf", $"AgendamentoCirurgico_{dataInicial.ToString("dd_MM_yyyy")}_{dataFinal.ToString("dd_MM_yyyy")}.pdf");
            }
        }


        // FIIIIIMMMMM


        private void ProcessarDadosInternacao(FiltroModel filtro)
        {
            var db = new Core.DataSetReportsTableAdapters.RelatorioMovimentoAdapter();

            var grupo = filtro.GrupoProduto.GetValueOrDefault();
            var classe = filtro.Classe.GetValueOrDefault();
            var subClasse = filtro.SubClasse.GetValueOrDefault();
            var query = db.GetData().Where(w => w.GrupoId == grupo);

            if (classe != 0)
            {
                query = query.Where(w => w.GrupoClasseId == classe);
            }

            if (subClasse != 0)
            {
                query = query.Where(w => w.GrupoSubClasseId == subClasse);
            }

            filtro.DadosMovimentacao = query.ToList();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public async Task<ActionResult> IndexAtendimentoEtiqueta(int linhas = 1)
        {
            var atendimento = this.TempData.Peek("Atendimento") as AtendimentoDto;
            var model = new List<AtendimentoEtiqueta>();
            var idade = obterIdade(atendimento.Paciente.Nascimento.Value);
            for (var l = 0; l < linhas; l++)
            {
                for (var i = 0; i < 2; i++)
                {
                    model.Add(new AtendimentoEtiqueta
                    {
                        AtendimentoId = atendimento.Id.ToString(),
                        CodigoAtendimento = atendimento.Codigo,
                        Convenio = atendimento.Convenio == null ? string.Empty : FuncoesGlobais.TresPontos(atendimento.Convenio.NomeFantasia, 25),
                        DataAtendimento = atendimento.DataRegistro,
                        DataNascimento = (DateTime)atendimento.Paciente.Nascimento,
                        Idade = idade,
                        MatriculaConvenio = atendimento.Convenio == null ? string.Empty : atendimento.Convenio.Codigo,
                        Paciente = atendimento.Paciente == null ? string.Empty : FuncoesGlobais.TresPontos(atendimento.Paciente.NomeCompleto, 25)
                    });
                }
            }

            using (var viewer = new ReportViewer())
            {
                viewer.ProcessingMode = ProcessingMode.Local;
                viewer.LocalReport.ReportPath = this.Request.MapPath(this.Request.ApplicationPath)
                                                + @"Relatorios\Atendimento\AtendimentoEtiqueta.rdlc";
                viewer.LocalReport.DataSources.Add(new ReportDataSource("AtendimentoEtiquetaDT", model));


                Warning[] warnings;
                string[] streamIds;
                var mimeType = string.Empty;
                var encoding = string.Empty;
                var extension = "pdf";
                var pdfBytes = viewer.LocalReport.Render(
                    "PDF",
                    null,
                    out mimeType,
                    out encoding,
                    out extension,
                    out streamIds,
                    out warnings);
                viewer.LocalReport.Refresh();

                this.Response.Headers.Add("Content-Disposition", "inline; filename=AtendimentoEtiqueta.pdf");
                return this.File(pdfBytes, "application/pdf");
            }
        }

        public async Task<ContentResult> AtendimentoTempData(long id)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var atendimento = await atendimentoAppService.Object.Obter(id).ConfigureAwait(false);
                //this.TempData["Atendimento"] = atendimento;
                return this.Content(string.Empty);
            }
        }

        public static int TruncateIndexOfAtWord(string value, int length)
        {
            if (value == null || value.Length < length || value.IndexOf(" ", length, StringComparison.Ordinal) == -1)
                return value?.IndexOf(" ", length, StringComparison.Ordinal) ?? 0;

            return value.IndexOf(" ", length, StringComparison.Ordinal);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public async Task<string> EtiquetaPaciente(long atendimentoId, long? numOfCopies)
        {
            try
            {
                using (var modeloTextoAppService = IocManager.Instance.ResolveAsDisposable<IModeloTextoAppService>())
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var impressoraAppService = IocManager.Instance.ResolveAsDisposable<IImpressoraArquivosAppService>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var atendimentoIQueryable = atendimentoRepository.Object.GetAll().Include(a => a.Paciente)
                        .Include(a => a.Paciente.SisPessoa).Include(a => a.Convenio).Include(a => a.Convenio.SisPessoa)
                        .Where(a => a.Id == atendimentoId).AsNoTracking();

                    var atendimento = await atendimentoAppService.Object.ObterIQ(atendimentoIQueryable)
                                          .ConfigureAwait(false);

                    var idade = "";
                    if (atendimento.Paciente.Nascimento != null)
                    {
                        idade = this.obterIdade(atendimento.Paciente.Nascimento);
                    }

                    var pacienteNome = atendimento.Paciente == null ? string.Empty : atendimento.Paciente?.NomeCompleto;

                    if (pacienteNome.Length >= 25)
                    {
                        //var idx = TruncateIndexOfAtWord(pacienteNome, 25);
                        //if (idx != -1)
                        //{
                        //    pacienteNome = pacienteNome.Insert(idx, "<br/>");
                        //}
                        pacienteNome = FuncoesGlobais.TresPontos(
                            pacienteNome,
                            pacienteNome.Length > 57 ? 57 : pacienteNome.Length);
                    }
                    else
                    {
                        pacienteNome = FuncoesGlobais.TresPontos(pacienteNome, 32);
                    }

                    var dados = new AtendimentoEtiqueta
                    {
                        AtendimentoId = atendimento.Id.ToString(),
                        CodigoAtendimento = atendimento.Codigo,
                        Convenio =
                                            atendimento.Convenio == null
                                                ? string.Empty
                                                : FuncoesGlobais.TresPontos(atendimento.Convenio?.NomeFantasia, 32),
                        DataAtendimento = atendimento.DataRegistro,
                        DataNascimento = atendimento.Paciente?.Nascimento ?? new DateTime(),
                        Idade = idade,
                        MatriculaConvenio = atendimento.Matricula,
                        Paciente = pacienteNome
                    };

                    var modelo = await modeloTextoAppService.Object
                                     .ObterPorTipoAsync((long)EnumTipoModelo.EtiquetaPaciente).ConfigureAwait(false);

                    if (modelo == null)
                    {
                        throw new UserFriendlyException("Não há modelo de impressão. Entre em contato como suporte");
                    }

                    var campos = dados.GetType().GetProperties();
                    var texto = modelo.Texto;

                    foreach (var campo in campos)
                    {
                        var valor = string.Empty;
                        if (modelo.TipoModelo.Variaveis.Any(c => string.Equals(c.Descricao, campo.Name)))
                        {
                            if (campo.PropertyType == typeof(DateTime))
                            {
                                if (campo.Name == "DataAtendimento")
                                {
                                    valor = ((DateTime)campo.GetValue(dados)).ToString("dd/MM/yyyy HH:mm:ss");
                                }
                                else
                                {
                                    valor = ((DateTime)campo.GetValue(dados)).ToString("dd/MM/yyyy");
                                }
                            }
                            else
                            {
                                valor = (string)campo.GetValue(dados);
                            }

                            texto = texto.MergeTexto(campo.Name, valor);
                        }
                        else
                        {
                            texto = texto.MergeTexto(campo.Name, valor);
                        }
                    }


                    var uuidPdf = $"_EtiquetaPaciente-{Guid.NewGuid()}.pdf";

                    var printerCookie = ImpressoraHelper.CookiePorModelo(EnumTipoModelo.EtiquetaPaciente);

                    if (this.HttpContext.Request.Cookies.AllKeys.Any(x => x == printerCookie))
                    {
                        impressoraAppService.Object.EnviarParaImprimir(
                            this.HttpContext.Server.UrlDecode(
                                this.HttpContext.Request.Cookies.Get(printerCookie)?.Value),
                            modelo.GerarPdf(texto),
                            uuidPdf,
                            numOfCopies ?? 1);
                    }

                    return uuidPdf;
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Erro ao realizar a impressão da etiqueta de paciente. Entre em contato com o suporte", e);
                return null;
            }
        }

        /// <summary>
        /// The modal etiqueta paciente.
        /// </summary>
        /// <param name="atendimentoId">
        /// The atendimento id.
        /// </param>
        /// <param name="numOfCopies">
        /// The num of copies.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> ModalEtiquetaPaciente(long atendimentoId, long? numOfCopies)
        {
            var viewModel =
                new ModalEtiquetaPacienteViewModel { AtendimentoId = atendimentoId.ToString(), NumOfCopies = numOfCopies ?? 1 };
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Etiqueta/_ModalEtiquetaPaciente.cshtml", viewModel);
        }

        /// <summary>
        /// The visitante etiqueta.
        /// </summary>
        /// <param name="visitanteId">
        /// The visitante id.
        /// </param>
        /// <param name="numOfCopies">
        /// The num of copies.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public async Task<string> VisitanteEtiqueta(long visitanteId, long? numOfCopies)
        {
            try
            {
                using (var modeloTextoAppService = IocManager.Instance.ResolveAsDisposable<IModeloTextoAppService>())
                using (var visitanteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Visitante, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var impressoraAppService = IocManager.Instance.ResolveAsDisposable<IImpressoraArquivosAppService>())
                {
                    var visitante = await visitanteRepository.Object.GetAll().AsNoTracking().Include(m => m.Atendimento)
                                        .Include(m => m.Fornecedor).Include(m => m.UnidadeOrganizacional).AsNoTracking()
                                        //.Include(m => m.AtendimentoId)
                                        .FirstOrDefaultAsync(m => m.Id == visitanteId).ConfigureAwait(false);

                    var atendimentoIQueryable = atendimentoRepository.Object.GetAll().AsNoTracking().Include(a => a.Paciente)
                        .Include(a => a.Paciente.SisPessoa).Include(a => a.Convenio).Include(a => a.Convenio.SisPessoa)
                        .Include(a => a.Leito).Include(a => a.Leito.TipoAcomodacao)
                        .Where(a => a.Id == (long)visitante.AtendimentoId).AsNoTracking();

                    var atendimento = await atendimentoIQueryable.FirstOrDefaultAsync().ConfigureAwait(false);

                    var modelo = await modeloTextoAppService.Object
                                     .ObterPorTipoAsync((long)EnumTipoModelo.EtiquetaVisitante).ConfigureAwait(false);

                    if (modelo == null)
                    {
                        throw new UserFriendlyException("Não há modelo de impressão. Entre em contato como suporte");
                    }

                    var dados = EtiquetaVisitanteModel.MapearFromAtendimento(atendimento, visitante);

                    var campos = dados.GetType().GetProperties();
                    var texto = modelo.Texto;

                    foreach (var campo in campos)
                    {
                        var valor = string.Empty;
                        if (modelo.TipoModelo.Variaveis.Any(c => string.Equals(c.Descricao, campo.Name)))
                        {
                            if (campo.PropertyType == typeof(DateTime))
                            {
                                valor = ((DateTime)campo.GetValue(dados)).ToString("dd/MM/yyyy HH:mm:ss");
                            }
                            else
                            {
                                valor = (string)campo.GetValue(dados);
                            }

                            texto = texto.MergeTexto(campo.Name, valor);
                        }
                        else
                        {
                            texto = texto.MergeTexto(campo.Name, valor);
                        }
                    }


                    var uuidPdf = $"_EtiquetaVisitante-{Guid.NewGuid()}.pdf";


                    var printerCookie = ImpressoraHelper.CookiePorModelo(EnumTipoModelo.EtiquetaVisitante);

                    if (this.HttpContext.Request.Cookies.AllKeys.Any(x => x == printerCookie))
                    {
                        impressoraAppService.Object.EnviarParaImprimir(
                            this.HttpContext.Server.UrlDecode(
                                this.HttpContext.Request.Cookies.Get(printerCookie)?.Value),
                            modelo.GerarPdf(texto),
                            uuidPdf,
                            numOfCopies ?? 1);
                    }

                    return uuidPdf;
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Erro ao realizar a impressão da etiqueta de visitante. Entre em contato com o suporte", e);
                //return false;
            }
        }

        /// <summary>
        /// The modal etiqueta visitante.
        /// </summary>
        /// <param name="visitanteId">
        /// The visitante id.
        /// </param>
        /// <param name="numOfCopies">
        /// The num of copies.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<ActionResult> ModalEtiquetaVisitante(long visitanteId, long? numOfCopies)
        {
            var viewModel = new ModalEtiquetaVisitanteViewModel { VisitanteId = visitanteId.ToString(), NumOfCopies = numOfCopies ?? 1 };
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Etiqueta/_ModalEtiquetaVisitante.cshtml", viewModel);
        }

        /// <summary>
        /// The pulseira internacao.
        /// </summary>
        /// <param name="atendimentoId">
        /// The atendimento id.
        /// </param>
        /// <param name="numOfCopies">
        /// The num of copies.
        /// </param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public async Task<string> PulseiraInternacao(long atendimentoId, long? numOfCopies)
        {
            try
            {
                using (var modeloTextoAppService = IocManager.Instance.ResolveAsDisposable<IModeloTextoAppService>())
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var impressoraAppService = IocManager.Instance.ResolveAsDisposable<IImpressoraArquivosAppService>())
                {

                    var atendimentoIQueryable = atendimentoRepository.Object.GetAll().AsNoTracking().Include(a => a.Paciente)
                        .Include(a => a.Paciente.SisPessoa).Include(a => a.Convenio).Include(a => a.Convenio.SisPessoa)
                        .Include(a => a.Leito).Include(a => a.Leito.TipoAcomodacao).Where(a => a.Id == atendimentoId)
                        .AsNoTracking();

                    var atendimento = await atendimentoAppService.Object.ObterIQ(atendimentoIQueryable)
                                          .ConfigureAwait(false);

                    var modelo = await modeloTextoAppService.Object.ObterPorTipoAsync((long)EnumTipoModelo.Pulseira)
                                     .ConfigureAwait(false);

                    if (modelo == null)
                    {
                        throw new Exception("Não há modelo de impressão. Entre em contato como suporte");
                    }

                    var dados = PulseiraInternacaoModel.MapearFromAtendimento(atendimento);

                    var campos = dados.GetType().GetProperties();
                    var texto = modelo.Texto;

                    foreach (var campo in campos)
                    {
                        var valor = string.Empty;
                        if (modelo.TipoModelo.Variaveis.Any(c => string.Equals(c.Descricao, campo.Name)))
                        {
                            if (campo.PropertyType == typeof(DateTime))
                            {
                                valor = ((DateTime)campo.GetValue(dados)).ToString("dd/MM/yyyy HH:mm:ss");
                            }
                            else
                            {
                                valor = (string)campo.GetValue(dados);
                            }

                            texto = texto.MergeTexto(campo.Name, valor);
                        }
                        else
                        {
                            texto = texto.MergeTexto(campo.Name, valor);
                        }
                    }


                    var uuidPdf = $"_PulseiraInternacao-{Guid.NewGuid()}.pdf";


                    var printerCookie = ImpressoraHelper.CookiePorModelo(EnumTipoModelo.Pulseira);

                    if (this.HttpContext.Request.Cookies.AllKeys.Any(x => x == printerCookie))
                    {
                        impressoraAppService.Object.EnviarParaImprimir(
                            this.HttpContext.Server.UrlDecode(
                                this.HttpContext.Request.Cookies.Get(printerCookie)?.Value),
                            modelo.GerarPdf(texto),
                            uuidPdf,
                            numOfCopies ?? 1);
                    }

                    return uuidPdf;
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException("Erro ao realizar a impressão da pulseira de internação. Entre em contato com o suporte", e);
            }

        }

        public async Task<ActionResult> ModalPulseiraInternacao(long atendimentoId, long? numOfCopies)
        {
            var viewModel = new ModalPulseiraViewModel { AtendimentoId = atendimentoId.ToString(), NumOfCopies = numOfCopies ?? 1 };
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/Pulseira/_ModalPulseira.cshtml", viewModel);
        }

        public async Task<ActionResult> SolicInternacao(long atendimentoId)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    var atendimento = await atendimentoAppService.Object.Obter((long)atendimentoId).ConfigureAwait(false);
                    var dados = SolicInternacaoModel.MapearFromAtendimento(atendimento);

                    // Guia principal
                    var solic_internacao_dataset = new solic_internacao_dataset();
                    var tabela = this.ConvertToDataTable(dados.Lista, solic_internacao_dataset.Tables["solic_internacao_table"]);
                    var row = tabela.NewRow();
                    row["Logotipo"] = atendimento.Empresa.Logotipo;
                    tabela.Rows.Add(row);
                    var dataSource = new ReportDataSource("solic_internacao_dataset", tabela);
                    var reportViewer = new ReportViewer();
                    reportViewer.LocalReport.DataSources.Add(dataSource);
                    var scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(reportViewer);

                    reportViewer.LocalReport.ReportPath = string.Concat(this.Server.MapPath("~"), @"\Relatorios\Faturamento\Guias\InternacaoSolicitacao\guia_internacao_solic.rdlc");

                    this.SetParametrosSolicInternacao(reportViewer, dados);

                    var mimeType = string.Empty;
                    var encoding = string.Empty;
                    var extension = "pdf";

                    string[] streamIds;
                    Warning[] warnings;

                    var pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    reportViewer.LocalReport.Refresh();

                    this.Response.Headers.Add("Content-Disposition", "inline; filename=PulseiraInternacao.pdf");
                    return this.File(pdfBytes, "application/pdf");
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }

        public async Task<ActionResult> ModalSolicInternacao(long atendimentoId)
        {
            var viewModel = new ModalSolicInternacaoViewModel
            {
                AtendimentoId = atendimentoId.ToString()
            };
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/SolicInternacao/_ModalSolicInternacao.cshtml", viewModel);
        }

        public async Task<ActionResult> ResumoInternacao(long atendimentoId)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    var atendimento = await atendimentoAppService.Object.Obter((long)atendimentoId).ConfigureAwait(false);
                    var dados = ResumoInternacaoModel.MapearFromAtendimento(atendimento);

                    // Guia principal
                    var resumo_internacao_dataset = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.resumo_internacao_dataset();
                    var tabela = this.ConvertToDataTable(dados.Lista, resumo_internacao_dataset.Tables["resumo_internacao_table"]);
                    var row = tabela.NewRow();
                    row["Logotipo"] = atendimento.Empresa.Logotipo;
                    //   tabela.Rows[tabela.Rows.Count - 1].Delete();
                    tabela.Rows.Add(row);
                    var reportViewer = new ReportViewer();
                    var dataSource = new ReportDataSource("resumo_internacao_dataset", tabela);

                    reportViewer.LocalReport.DataSources.Add(dataSource);
                    var scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(reportViewer);

                    reportViewer.LocalReport.ReportPath = string.Concat(
                        this.Server.MapPath("~"),
                        @"\Relatorios\Faturamento\Guias\InternacaoResumo\guia_internacao_resumo.rdlc");

                    this.SetParametrosResumoInternacao(reportViewer, dados);

                    // APARENTEMENTE FALTANDO SUB-RELATORIOS

                    var mimeType = string.Empty;
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
                    reportViewer.LocalReport.Refresh();
                    //reportViewer.Dispose();

                    this.Response.Headers.Add("Content-Disposition", "inline; filename=PulseiraInternacao.pdf");
                    return this.File(pdfBytes, "application/pdf");
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }

        public async Task<ActionResult> ModalResumoInternacao(long atendimentoId)
        {
            var viewModel = new ModalResumoInternacaoViewModel();
            viewModel.AtendimentoId = atendimentoId.ToString();
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/ResumoINternacao/_ModalResumoInternacao.cshtml", viewModel);
        }

        //public async Task<ActionResult> GuiaAmbulatorio(long atendimentoId)
        //{
        //    try
        //    {
        //        var atendimento = await _atendimentoAppService.Obter((long)atendimentoId);
        //        var dados = GuiaAmbulatorioModel.MapearFromAtendimento(atendimento);

        //        // Guia principal
        //        Web.Relatorios.Faturamento.Guias.InternacaoResumo.resumo_internacao_dataset resumo_internacao_dataset = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.resumo_internacao_dataset();
        //        DataTable tabela = this.ConvertToDataTable(dados.Lista, resumo_internacao_dataset.Tables["resumo_internacao_table"]);
        //        DataRow row = tabela.NewRow();
        //        row["Logotipo"] = atendimento.Empresa.Logotipo;
        //        tabela.Rows.Add(row);
        //        ReportDataSource dataSource = new ReportDataSource("resumo_internacao_dataset", tabela);
        //        ReportViewer reportViewer = new ReportViewer();
        //        reportViewer.LocalReport.DataSources.Add(dataSource);
        //        ScriptManager scriptManager = new ScriptManager();
        //        scriptManager.RegisterPostBackControl(reportViewer);

        //        reportViewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"\Relatorios\Faturamento\Guias\InternacaoResumo\guia_internacao_resumo.rdlc");

        //        // SetParametrosGuiaAmbulatorio(reportViewer, dados);

        //        // APARENTEMENTE FALTANDO SUB-RELATORIOS

        //        string mimeType = string.Empty;
        //        string encoding = string.Empty;
        //        string extension = "pdf";

        //        string[] streamIds;
        //        Warning[] warnings;
        //        byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
        //        reportViewer.LocalReport.Refresh();

        //        Response.Headers.Add("Content-Disposition", "inline; filename=PulseiraInternacao.pdf");
        //        return File(pdfBytes, "application/pdf");
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        return null;
        //    }
        //}

        public async Task<ActionResult> ModalGuiaAmbulatorio(long atendimentoId)
        {
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var viewModel = new ModalGuiaAmbulatorioViewModel();
                viewModel.AtendimentoId = atendimentoId.ToString();

                var atd = await atendimentoAppService.Object.Obter(atendimentoId).ConfigureAwait(false);

                // Id fixada pelo seed (1 = consuta)
                switch (atd.FatGuiaId)
                {
                    case 1:
                        viewModel.TipoGuia = "consulta";
                        break;
                    default:
                        return null;
                }

                //                    Areas\Mpa\Views\Aplicacao\Atendimentos\AmbulatorioEmergencias\Home\Guia\_ModalGuiaAmbulatorio.cshtml
                return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Guia/_ModalGuiaAmbulatorio.cshtml", viewModel);
            }
        }

        public async Task<ActionResult> HonorarioIndividual(long atendimentoId)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    var atendimento = await atendimentoAppService.Object.Obter((long)atendimentoId).ConfigureAwait(false);
                    var dados = HonorarioIndividualModel.MapearFromAtendimento(atendimento);

                    // Guia principal
                    var honorario_individual_dataset = new Web.Relatorios.Faturamento.Guias.HonorarioIndividual.honorario_individual_dataset();
                    var tabela = this.ConvertToDataTable(dados.Lista, honorario_individual_dataset.Tables["honorario_individual_table"]);
                    var row = tabela.NewRow();
                    row["Logotipo"] = atendimento.Empresa.Logotipo;
                    //   tabela.Rows[tabela.Rows.Count - 1].Delete();
                    tabela.Rows.Add(row);

                    var reportViewer = new ReportViewer();
                    var dataSource = new ReportDataSource("honorario_individual_dataset", tabela);

                    reportViewer.LocalReport.DataSources.Add(dataSource);
                    var scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(reportViewer);

                    reportViewer.LocalReport.ReportPath = string.Concat(
                        this.Server.MapPath("~"),
                        @"\Relatorios\Faturamento\Guias\HonorarioIndividual\guia_honorario_individual.rdlc");

                    this.SetParametrosHonorarioIndividual(reportViewer, dados);

                    // APARENTEMENTE FALTANDO SUB-RELATORIOS

                    var mimeType = string.Empty;
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
                    reportViewer.LocalReport.Refresh();
                    //reportViewer.Dispose();

                    this.Response.Headers.Add("Content-Disposition", "inline; filename=PulseiraInternacao.pdf");
                    return this.File(pdfBytes, "application/pdf");
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }


        public async Task<ActionResult> ModalHorarioIndividual(long atendimentoId)
        {
            var viewModel = new ModalHonorarioIndividualViewModel
            {
                AtendimentoId = atendimentoId.ToString()
            };
            return this.PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Internacoes/Home/HonorarioIndividual/_ModalHonorarioIndividual.cshtml", viewModel);
        }

        private void SetParametrosSolicInternacao(ReportViewer rv, SolicInternacaoModel dados)
        {
            var RegistroAns = new ReportParameter("RegistroAns", dados.RegistroAns);
            var AtendimentoRn = new ReportParameter("AtendimentoRn", dados.AtendimentoRn);
            var NomePaciente = new ReportParameter("NomePaciente", dados.NomePaciente);
            var NomeContratado = new ReportParameter("NomeContratado", dados.NomeContratado);
            var RegimeInternacao = new ReportParameter("RegimeInternacao", dados.RegimeInternacao);
            var CodigoOperadoraCnpj = new ReportParameter("CodigoOperadoraCnpj", dados.CodigoOperadoraCnpj);
            var NomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
            var DataSugerInterna = new ReportParameter("DataSugerInterna", dados.DataSugerInterna);
            var QtdDiariasSolicitadas = new ReportParameter("QtdDiariasSolicitadas", dados.QtdDiariasSolicitadas);
            var PrevOPME = new ReportParameter("PrevOPME", dados.PrevOPME);
            var PrevQuimio = new ReportParameter("PrevQuimio", dados.PrevQuimio);
            var Cid1 = new ReportParameter("Cid1", dados.Cid1);
            var Cid2 = new ReportParameter("Cid2", dados.Cid2);
            var Cid3 = new ReportParameter("Cid3", dados.Cid3);
            var Cid4 = new ReportParameter("Cid4", dados.Cid4);
            var NumeroGuiaPrestador = new ReportParameter("NumeroGuiaPrestador", dados.NumeroGuiaPrestador);
            var NumeroCarteira = new ReportParameter("NumeroCarteira", dados.NumeroCarteira);
            var ValidadeCarteira = new ReportParameter("ValidadeCarteira", dados.ValidadeCarteira);
            var IndicacaoClinica = new ReportParameter("IndicacaoClinica", dados.IndicacaoClinica);
            var NomeProfissionalSolicitante = new ReportParameter("NomeProfissionalSolicitante", dados.NomeProfissionalSolicitante);
            var ConselhoProfissional = new ReportParameter("ConselhoProfissional", dados.ConselhoProfissional);
            var NumeroConselho = new ReportParameter("NumeroConselho", dados.NumeroConselho);
            var UF = new ReportParameter("UF", dados.UF);
            var CodigoCbo = new ReportParameter("CodigoCbo", dados.CodigoCbo);
            var CodigoCnes = new ReportParameter("CodigoCnes", dados.CodigoCnes);
            var RN = new ReportParameter("AtendimentoRn", dados.AtendimentoRn);

            rv.LocalReport.SetParameters(new ReportParameter[] {
                RegistroAns                         ,
                AtendimentoRn                       ,
                NomePaciente                        ,
                NomeContratado                      ,
                RegimeInternacao                    ,
                CodigoOperadoraCnpj                 ,
                NomeHospital                        ,
                DataSugerInterna                    ,
                QtdDiariasSolicitadas               ,
                PrevOPME                            ,
                PrevQuimio                          ,
                Cid1                                ,
                Cid2                                ,
                Cid3                                ,
                Cid4                                ,
                NumeroGuiaPrestador                 ,
                NumeroCarteira                      ,
                ValidadeCarteira                    ,
                IndicacaoClinica                    ,
                NomeProfissionalSolicitante         ,
                ConselhoProfissional                ,
                NumeroConselho                      ,
                UF                                  ,
                CodigoCbo                           ,
                CodigoCnes                          ,
                RN
            });
        }

        private void SetParametrosPulseira(ReportViewer rv, PulseiraInternacaoModel dados)
        {
            var NomePaciente = new ReportParameter("NomePaciente", dados.NomePaciente);
            var Nascimento = new ReportParameter("Nascimento", dados.Nascimento);
            var CodigoAtendimento = new ReportParameter("CodigoAtendimento", dados.CodigoAtendimento);
            var Atendimento = new ReportParameter("Atendimento", dados.Atendimento);
            var Matricula = new ReportParameter("Matricula", dados.Matricula);
            var Convenio = new ReportParameter("Convenio", dados.Convenio);

            rv.LocalReport.SetParameters(new ReportParameter[] {
                NomePaciente     ,
                Nascimento       ,
                CodigoAtendimento,
                Atendimento      ,
                Matricula        ,
                Convenio
            });
        }

        private void SetParametrosEtiquetaVisitante(ReportViewer rv, EtiquetaVisitanteModel dados, VisitanteDto visitante, AtendimentoDto atendimento)
        {
            var NomePaciente = new ReportParameter("NomePaciente", dados.NomePaciente);
            var Nascimento = new ReportParameter("Nascimento", dados.Nascimento);
            var CodigoAtendimento = new ReportParameter("CodigoAtendimento", dados.CodigoAtendimento);
            var Atendimento = new ReportParameter("Atendimento", dados.Atendimento);
            var Matricula = new ReportParameter("Matricula", dados.Matricula);
            var Convenio = new ReportParameter("Convenio", dados.Convenio);

            var NomeVisitante = new ReportParameter("NomeVisitante", visitante.Nome);
            var Documento = new ReportParameter("Documento", visitante.Documento);
            var DataEntrada = new ReportParameter("DataEntrada", ((DateTime)visitante.DataEntrada).ToString("dd/MM/yyyy"));
            var Fornecedor = new ReportParameter("Fornecedor", visitante.Fornecedor?.Descricao);
            var Local = new ReportParameter("Local", atendimento.UnidadeOrganizacional?.Descricao);

            rv.LocalReport.SetParameters(new ReportParameter[] {
                NomePaciente     ,
                Nascimento       ,
                CodigoAtendimento,
                Atendimento      ,
                Matricula        ,
                Convenio,

                NomeVisitante,
                Documento,
                DataEntrada,
                Fornecedor,
                Local
            });
        }

        private void SetParametrosResumoInternacao(ReportViewer rv, ResumoInternacaoModel dados)
        {
            var NomePaciente = new ReportParameter("NomePaciente", dados.NomePaciente);
            var Matricula = new ReportParameter("Matricula", dados.Matricula);
            var RegistroANS = new ReportParameter("RegistroANS", dados.RegistroANS);
            var ValidadeCarteira = new ReportParameter("ValidadeCarteira", dados.ValidadeCarteira);
            var Senha = new ReportParameter("Senha", dados.Senha);
            var CodCNES = new ReportParameter("CodCNES", dados.CodCNES);
            var DataAutorizacao = new ReportParameter("DataAutorizacao", dados.DataAutorizacao);
            var NomeContratado = new ReportParameter("NomeContratado", dados.NomeContratado);
            var ValidadeSenha = new ReportParameter("ValidadeSenha", dados.ValidadeSenha);
            var NumeroGuia = new ReportParameter("NumeroGuia", dados.NumeroGuia);
            var Cid1 = new ReportParameter("Cid1", dados.Cid1);
            var Cid2 = new ReportParameter("Cid2", dados.Cid2);
            var Cid3 = new ReportParameter("Cid3", dados.Cid3);
            var Cid4 = new ReportParameter("Cid4", dados.Cid4);
            var CodOperadora = new ReportParameter("CodOperadora", dados.CodOperadora);
            var CaraterAtendimento = new ReportParameter("CaraterAtendimento", dados.CaraterAtendimento);
            var TipoFaturamento = new ReportParameter("TipoFaturamento", dados.TipoFaturamento);
            var DataIniFaturamento = new ReportParameter("DataIniFaturamento", dados.DataIniFaturamento);
            var DataFimFaturamento = new ReportParameter("DataFimFaturamento", dados.DataFimFaturamento);
            var HoraIniFaturamento = new ReportParameter("HoraIniFaturamento", dados.HoraIniFaturamento);
            var HoraFimFaturamento = new ReportParameter("HoraFimFaturamento", dados.HoraFimFaturamento);
            var TipoInternacao = new ReportParameter("TipoInternacao", dados.TipoInternacao);
            var RegimeInternacao = new ReportParameter("RegimeInternacao", dados.RegimeInternacao);
            var TotalProcedimentos = new ReportParameter("TotalProcedimentos", dados.TotalProcedimentos);
            var TotalDiaria = new ReportParameter("TotalDiaria", dados.TotalDiaria);
            var TotalTaxasAlugueis = new ReportParameter("TotalTaxasAlugueis", dados.TotalTaxasAlugueis);
            var TotalMateriais = new ReportParameter("TotalMateriais", dados.TotalMateriais);
            var TotalOpme = new ReportParameter("TotalOpme", dados.TotalOpme);
            var TotalMedicamentos = new ReportParameter("TotalMedicamentos", dados.TotalMedicamentos);
            var TotalGasesMedicinais = new ReportParameter("TotalGasesMedicinais", dados.TotalGasesMedicinais);
            var TotalGeral = new ReportParameter("TotalGeral", dados.TotalGeral);
            var RN = new ReportParameter("RN", dados.RN ? "S" : "N");


            var CNS = new ReportParameter("CNS", "");
            var IndicadorAcidente = new ReportParameter("IndicadorAcidente", "");
            var MotivoEncerramento = new ReportParameter("MotivoEncerramento", "");
            var CidObito = new ReportParameter("CidObito", "");
            var ExibirSegundaPagina = new ReportParameter("ExibirSegundaPagina", "N");




            rv.LocalReport.SetParameters(new ReportParameter[] {
                NomePaciente            ,
                Matricula               ,
                RegistroANS             ,
                ValidadeCarteira        ,
                Senha                   ,
                CodCNES                 ,
                DataAutorizacao         ,
                NomeContratado          ,
                ValidadeSenha           ,
                NumeroGuia              ,
                Cid1                    ,
                Cid2                    ,
                Cid3                    ,
                Cid4                    ,
                CodOperadora            ,
                CaraterAtendimento      ,
                TipoFaturamento         ,
                DataIniFaturamento      ,
                DataFimFaturamento      ,
                HoraIniFaturamento      ,
                HoraFimFaturamento      ,
                TipoInternacao          ,
                RegimeInternacao        ,
                TotalProcedimentos      ,
                TotalDiaria             ,
                TotalTaxasAlugueis      ,
                TotalMateriais          ,
                TotalOpme               ,
                TotalMedicamentos       ,
                TotalGasesMedicinais    ,
                TotalGeral              ,
                RN,
                CNS,
                IndicadorAcidente,
                MotivoEncerramento,
                CidObito,
                ExibirSegundaPagina
            });
        }

        private void SetParametrosHonorarioIndividual(ReportViewer rv, HonorarioIndividualModel dados)
        {
            var RegistroANS = new ReportParameter("RegistroANS", dados.RegistroANS);
            var NumeroGuiaSolicitacao = new ReportParameter("NumeroGuiaSolicitacao", dados.NumeroGuiaSolicitacao);
            var Senha = new ReportParameter("Senha", dados.Senha);
            var NumeroGuiaOperadora = new ReportParameter("NumeroGuiaOperadora", dados.NumeroGuiaOperadora);
            var Matricula = new ReportParameter("Matricula", dados.Matricula);
            var NomePaciente = new ReportParameter("NomePaciente", dados.NomePaciente);
            var RN = new ReportParameter("RN", dados.RN ? "S" : "N");
            var CodOperadora = new ReportParameter("CodOperadora", dados.CodOperadora);
            var NomeHospitalLocal = new ReportParameter("NomeHospitalLocal", dados.NomeHospitalLocal);
            var CodCNES = new ReportParameter("CodCNES", dados.CodCNES);
            var NomeContratado = new ReportParameter("NomeContratado", dados.NomeContratado);
            var DataIniFaturamento = new ReportParameter("DataIniFaturamento", dados.DataIniFaturamento);
            var DataFimFaturamento = new ReportParameter("DataFimFaturamento", dados.DataFimFaturamento);
            var DataEmissao = new ReportParameter("DataEmissao", dados.DataEmissao);
            var TotaGeral = new ReportParameter("TotalGeral", dados.TotaGeral);




            rv.LocalReport.SetParameters(new ReportParameter[] {
                RegistroANS                ,
                NumeroGuiaSolicitacao      ,
                Senha                      ,
                NumeroGuiaOperadora        ,
                Matricula                  ,
                NomePaciente               ,
                RN                         ,
                CodOperadora               ,
                NomeHospitalLocal          ,
                CodCNES                    ,
                NomeContratado             ,
                DataIniFaturamento         ,
                DataFimFaturamento         ,
                DataEmissao                ,
                TotaGeral
            });
        }

        public DataTable ConvertToDataTable<T>(IList<T> data, DataTable table)
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
                            try
                            {
                                if (prop.GetValue(item) != null)
                                {
                                    if (prop.PropertyType == typeof(string))
                                    {
                                        if ((string)prop.GetValue(item) != string.Empty)
                                        {
                                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                                        }
                                    }
                                    else
                                    {
                                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                                    }
                                }
                                else
                                {
                                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                                }


                            }
                            catch (Exception exs)
                            {

                            }
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


        public class SolicInternacaoModel
        {
            public string RegistroAns { get; set; }
            public string AtendimentoRn { get; set; }
            public string NomePaciente { get; set; }
            public string NomeContratado { get; set; }
            public string RegimeInternacao { get; set; }
            public string CodigoOperadoraCnpj { get; set; }
            public string NomeHospital { get; set; }
            public string DataSugerInterna { get; set; }
            public string QtdDiariasSolicitadas { get; set; }
            public string PrevOPME { get; set; }
            public string PrevQuimio { get; set; }
            public string Cid1 { get; set; }
            public string Cid2 { get; set; }
            public string Cid3 { get; set; }
            public string Cid4 { get; set; }
            public string NumeroGuiaPrestador { get; set; }
            public string NumeroCarteira { get; set; }
            public string ValidadeCarteira { get; set; }
            public string IndicacaoClinica { get; set; }
            public string NomeProfissionalSolicitante { get; set; }
            public string ConselhoProfissional { get; set; }
            public string NumeroConselho { get; set; }
            public string UF { get; set; }
            public string CodigoCbo { get; set; }
            public string CodigoCnes { get; set; }


            public List<string> Lista { get; set; }

            public SolicInternacaoModel()
            {
                this.Lista = new List<string>();
            }

            public static SolicInternacaoModel MapearFromAtendimento(AtendimentoDto atendimento)
            {
                var solic = new SolicInternacaoModel();

                solic.RegistroAns = atendimento.Convenio?.RegistroANS;
                if (atendimento.Paciente.SisPessoa != null && atendimento.Paciente.SisPessoa.Nascimento != null)
                {
                    solic.AtendimentoRn = FuncoesGlobais.IsRN((DateTime)atendimento.Paciente.SisPessoa.Nascimento) ? "S" : "N";
                }
                solic.NomePaciente = atendimento.Paciente.NomeCompleto;
                solic.NomeContratado = atendimento.Empresa?.NomeFantasia;
                solic.RegimeInternacao = atendimento.AtendimentoTipo?.Codigo;
                solic.CodigoOperadoraCnpj = atendimento.Empresa?.Cnpj;
                solic.NomeHospital = atendimento.Empresa?.NomeFantasia;
                solic.DataSugerInterna = DateTime.Now.ToString();
                solic.QtdDiariasSolicitadas = "";
                solic.PrevOPME = "";
                solic.PrevQuimio = "";
                solic.Cid1 = "";
                solic.Cid2 = "";
                solic.Cid3 = "";
                solic.Cid4 = "";
                solic.NumeroGuiaPrestador = atendimento.GuiaNumero;
                solic.NumeroCarteira = atendimento.Matricula;

                if (atendimento.ValidadeCarteira != null)
                {
                    solic.ValidadeCarteira = ((DateTime)atendimento.ValidadeCarteira).ToString("dd/MM/yyyy");
                }

                solic.IndicacaoClinica = "";
                solic.NomeProfissionalSolicitante = atendimento.Medico?.NomeCompleto;
                solic.ConselhoProfissional = atendimento.Medico?.Conselho?.Codigo;
                solic.NumeroConselho = atendimento.Medico?.NumeroConselho.ToString();
                solic.UF = atendimento.Medico?.Conselho?.Uf;
                solic.CodigoCbo = atendimento.Especialidade?.SisCbo?.Codigo;
                solic.CodigoCnes = atendimento.Medico?.Cns;

                return solic;
            }
        }

        public class PulseiraInternacaoModel
        {
            public string NomePaciente { get; set; }
            public string Nascimento { get; set; }
            public string CodigoAtendimento { get; set; }
            public string Atendimento { get; set; }
            public string Matricula { get; set; }
            public string Convenio { get; set; }

            public List<string> Lista { get; set; }

            public PulseiraInternacaoModel()
            {
                this.Lista = new List<string>();
            }

            public static PulseiraInternacaoModel MapearFromAtendimento(AtendimentoDto atendimento)
            {
                var pulseira = new PulseiraInternacaoModel();

                pulseira.NomePaciente = atendimento.Paciente?.NomeCompleto;

                if (atendimento.Paciente.Nascimento != null)
                {
                    pulseira.Nascimento = ((DateTime)atendimento.Paciente.Nascimento).ToString("dd/MM/yy");
                }

                pulseira.CodigoAtendimento = atendimento.Codigo;
                pulseira.Atendimento = atendimento.DataRegistro.ToString();
                pulseira.Matricula = atendimento.Matricula;
                pulseira.Convenio = atendimento.Convenio?.NomeFantasia;

                return pulseira;
            }
        }

        public class EtiquetaVisitanteModel
        {
            public string NomePaciente { get; set; }
            public string Nascimento { get; set; }
            public string CodigoAtendimento { get; set; }
            public string Atendimento { get; set; }
            public string Matricula { get; set; }
            public string Convenio { get; set; }


            public string NomeVisitante { get; set; }
            public string Documento { get; set; }
            public string Fornecedor { get; set; }
            public string Entrada { get; set; }
            public string Local { get; set; }

            public string Tipo { get; set; }

            public List<string> Lista { get; set; }

            public EtiquetaVisitanteModel()
            {
                this.Lista = new List<string>();
            }

            public static EtiquetaVisitanteModel MapearFromAtendimento(AtendimentoDto atendimento, VisitanteDto visitante)
            {
                var etiqueta = new EtiquetaVisitanteModel
                {
                    NomePaciente = atendimento.Paciente?.NomeCompleto,
                    CodigoAtendimento = atendimento.Codigo,
                    Atendimento = atendimento.DataRegistro.ToString(),
                    Matricula = atendimento.Matricula,
                    Convenio = atendimento.Convenio?.NomeFantasia,
                    NomeVisitante = visitante.Nome,
                    Documento = visitante.Documento,
                    Fornecedor = visitante.Fornecedor?.Descricao,
                    Entrada = ((DateTime)visitante.DataEntrada).ToString("dd/MM/yyyy HH:mm:ss"),
                    Local = atendimento.Leito?.Descricao
                };


                if (visitante.IsAcompanhante)
                {
                    etiqueta.Tipo = "Acompanhante";
                }
                else if (visitante.IsMedico)
                {
                    etiqueta.Tipo = "Médico";
                }
                else if (visitante.IsFornecedor)
                {
                    etiqueta.Tipo = "Fornecedor";
                }
                else if (visitante.IsVisitante)
                {
                    etiqueta.Tipo = "Visitante";
                }

                if (atendimento.Paciente.Nascimento != null)
                {
                    etiqueta.Nascimento = ((DateTime)atendimento.Paciente?.Nascimento).ToString("dd/MM/yy");
                }

                return etiqueta;
            }

            public static EtiquetaVisitanteModel MapearFromAtendimento(Atendimento atendimento, Visitante visitante)
            {
                var etiqueta = new EtiquetaVisitanteModel
                {
                    NomePaciente = atendimento.Paciente?.NomeCompleto,
                    CodigoAtendimento = atendimento.Codigo,
                    Atendimento = atendimento.DataRegistro.ToString(),
                    Matricula = atendimento.Matricula,
                    Convenio = atendimento.Convenio?.NomeFantasia,
                    NomeVisitante = visitante.Nome,
                    Documento = visitante.Documento,
                    Fornecedor = visitante.Fornecedor?.Descricao,
                    Entrada = ((DateTime)visitante.DataEntrada).ToString("dd/MM/yyyy HH:mm:ss"),
                    Local = atendimento.Leito?.Descricao
                };


                if (visitante.IsAcompanhante)
                {
                    etiqueta.Tipo = "Acompanhante";
                }
                else if (visitante.IsMedico)
                {
                    etiqueta.Tipo = "Médico";
                }
                else if (visitante.IsFornecedor)
                {
                    etiqueta.Tipo = "Fornecedor";
                }
                else if (visitante.IsVisitante)
                {
                    etiqueta.Tipo = "Visitante";
                }

                if (atendimento.Paciente.Nascimento != null)
                {
                    etiqueta.Nascimento = ((DateTime)atendimento.Paciente?.Nascimento).ToString("dd/MM/yy");
                }

                return etiqueta;
            }

        }

        public class ResumoInternacaoModel
        {
            public string NomePaciente { get; set; }
            public string Matricula { get; set; }
            public string RegistroANS { get; set; }
            public string ValidadeCarteira { get; set; }
            public string Senha { get; set; }
            public string DataAutorizacao { get; set; }
            public string CodCNES { get; set; }
            public string NomeContratado { get; set; }
            public string ValidadeSenha { get; set; }
            public string NumeroGuia { get; set; }
            public string Cid1 { get; set; }
            public string Cid2 { get; set; }
            public string Cid3 { get; set; }
            public string Cid4 { get; set; }
            public string CodOperadora { get; set; }
            public string CaraterAtendimento { get; set; }
            public string TipoFaturamento { get; set; }
            public string DataIniFaturamento { get; set; }
            public string DataFimFaturamento { get; set; }
            public string HoraIniFaturamento { get; set; }
            public string HoraFimFaturamento { get; set; }
            public string TipoInternacao { get; set; }
            public string RegimeInternacao { get; set; }
            public string TotalProcedimentos { get; set; }
            public string TotalDiaria { get; set; }
            public string TotalTaxasAlugueis { get; set; }
            public string TotalMateriais { get; set; }
            public string TotalOpme { get; set; }
            public string TotalMedicamentos { get; set; }
            public string TotalGasesMedicinais { get; set; }
            public string TotalGeral { get; set; }
            public bool RN { get; set; }

            public List<string> Lista { get; set; }

            public ResumoInternacaoModel()
            {
                this.Lista = new List<string>();
            }

            public static ResumoInternacaoModel MapearFromAtendimento(AtendimentoDto atendimento)
            {
                var model = new ResumoInternacaoModel();

                model.NomePaciente = atendimento.Paciente?.NomeCompleto;
                model.RegistroANS = atendimento.Convenio?.RegistroANS;
                model.Matricula = atendimento.Matricula;
                model.Senha = atendimento.Senha;
                model.DataAutorizacao = atendimento.DataAutorizacao != null ? ((DateTime)atendimento.DataAutorizacao).ToString("dd/MM/yyyy") : "";
                model.CodCNES = atendimento.Empresa?.Cnes.ToString();
                model.NomeContratado = atendimento.Empresa?.NomeFantasia;
                model.ValidadeSenha = atendimento.ValidadeSenha != null ? ((DateTime)atendimento.ValidadeSenha).ToString("dd/MM/yyyy") : "";
                model.NumeroGuia = atendimento.GuiaNumero;
                model.Cid1 = "";
                model.Cid2 = "";
                model.Cid3 = "";
                model.Cid4 = "";
                model.CodOperadora = "";
                model.CaraterAtendimento = "";
                model.TipoFaturamento = "";
                model.DataIniFaturamento = "";
                model.DataFimFaturamento = "";
                model.HoraIniFaturamento = "";
                model.HoraFimFaturamento = "";
                model.TipoInternacao = "";
                model.RegimeInternacao = "";
                model.TotalProcedimentos = "";
                model.TotalDiaria = "";
                model.TotalTaxasAlugueis = "";
                model.TotalMateriais = "";
                model.TotalOpme = "";
                model.TotalMedicamentos = "";
                model.TotalGasesMedicinais = "";
                model.TotalGeral = "";



                if (atendimento.ValidadeCarteira != null)
                {
                    model.ValidadeCarteira = atendimento.ValidadeCarteira != null ? ((DateTime)atendimento.ValidadeCarteira).ToString("dd/MM/yyyy") : "";
                }

                if (atendimento.Paciente.Nascimento.HasValue)
                {
                    var idade = DateDifference.GetExtendedDifference((DateTime)atendimento.Paciente.Nascimento);
                    model.RN = (idade.Ano == 0 && idade.Mes == 0 && idade.Dia <= 30);
                }


                //model.Convenio = atendimento.Convenio?.NomeFantasia;
                //if (atendimento.Paciente.Nascimento != null)
                //{
                //    model.Nascimento = ((DateTime)atendimento.Paciente?.Nascimento).ToString("dd/MM/yy");
                //}

                //model.CodigoAtendimento = atendimento.Codigo;
                //model.Atendimento = atendimento.DataRegistro.ToString();


                return model;
            }
        }

        public class HonorarioIndividualModel
        {
            public string RegistroANS { get; set; }
            public string NumeroGuiaSolicitacao { get; set; }
            public string Senha { get; set; }
            public string NumeroGuiaOperadora { get; set; }
            public string Matricula { get; set; }
            public string NomePaciente { get; set; }
            public bool RN { get; set; }
            public string CodOperadora { get; set; }
            public string NomeHospitalLocal { get; set; }
            public string CodCNES { get; set; }
            public string NomeContratado { get; set; }
            public string DataIniFaturamento { get; set; }
            public string DataFimFaturamento { get; set; }
            public string DataEmissao { get; set; }
            public string TotaGeral { get; set; }

            public List<string> Lista { get; set; }

            public HonorarioIndividualModel()
            {
                this.Lista = new List<string>();
            }

            public static HonorarioIndividualModel MapearFromAtendimento(AtendimentoDto atendimento)
            {
                var model = new HonorarioIndividualModel();

                model.RegistroANS = atendimento.Convenio?.RegistroANS;
                model.NumeroGuiaSolicitacao = "";
                model.Senha = atendimento.Senha;
                model.NumeroGuiaOperadora = atendimento.GuiaNumero;
                model.Matricula = atendimento.Matricula;
                model.NomePaciente = atendimento.Paciente.NomeCompleto;
                model.CodOperadora = "";
                model.NomeHospitalLocal = atendimento.Empresa?.NomeFantasia;
                model.CodCNES = atendimento.Empresa?.Cnes.ToString();
                model.NomeContratado = atendimento.Medico?.NomeCompleto;
                model.DataIniFaturamento = "";
                model.DataFimFaturamento = "";
                model.DataEmissao = ((DateTime)atendimento.DataRegistro).ToString("dd/MM/yyyy");
                model.TotaGeral = "";


                if (atendimento.Paciente.Nascimento.HasValue)
                {
                    var idade = DateDifference.GetExtendedDifference((DateTime)atendimento.Paciente.Nascimento);
                    model.RN = (idade.Ano == 0 && idade.Mes == 0 && idade.Dia <= 30);
                }

                return model;
            }
        }
    }

    internal class ResumoConvenio
    {
        public string Convenio { get; set; }
        public string Percent { get; set; }
        public int qtd { get; set; }
        public double FPercent { get; set; }
    }

    internal class ResumoTipoLeito
    {
        public int Bloqueado { get; set; }
        public int Extra { get; set; }
        public int ExtraLivre { get; set; }
        public int Livre { get; set; }
        public string OcupacaoTotal { get; set; }
        public string OcupacaoTpLeito { get; set; }
        public int Ocupado { get; set; }
        public string Percent { get; set; }
        public int Qtd { get; set; }
        public string TpLeito { get; set; }
        public double FPercent { get; set; }
    }
}