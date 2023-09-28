#region Usings
using Abp.Dependency;
using Abp.Domain.Repositories;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Reporting.WebForms;

using PdfSharp.Pdf.IO;

using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Relatorios.Models;
using SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos.GuiasClasses;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Relatorios.Guias;
using SW10.SWMANAGER.Web.Controllers;
using SW10.SWMANAGER.Web.Relatorios.Faturamento.Guias;
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
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturamentoGuiasController : SWMANAGERControllerBase
    {
        // Dados Guia Spsadt
        List<FaturamentoContaItemDto> _itensGuiaPrincipal = new List<FaturamentoContaItemDto>();
        List<FaturamentoContaItemDto> _itensGuiaPrincipalPrimeiraPagina = new List<FaturamentoContaItemDto>();
        List<FaturamentoContaItemDto> _itensGuiaOutrasDespesas = new List<FaturamentoContaItemDto>();
        List<FaturamentoContaItemDto> _listasGuiaComplementar = new List<FaturamentoContaItemDto>();
        List<ProfissionalEspecialidade> _medicosEspecialidades = new List<ProfissionalEspecialidade>();


        public ActionResult Index()
        {
            var model = new FaturamentoGuiasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Guias/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var model = new FaturamentoGuiaDto();

            if (id.HasValue)
            {
                using (var guiaAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoGuiaAppService>())
                {
                    model = await guiaAppService.Object.Obter((long)id);
                }
            }
            else
            {
                model.Id = 0;
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Guias/_CriarOuEditarModal.cshtml", model);
        }

        public class VisualizarGuiaInput
        {
            public long? ContaId { get; set; }
            public long? AtendimentoId { get; set; }
            public long? GuiaId { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult> VisualizarGuia(VisualizarGuiaInput input)
        {
            using (var guiaAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoGuiaAppService>())
            {
                var guia = await guiaAppService.Object.Obter((long)input.GuiaId);
                try
                {

                    switch (guia.Codigo)
                    {
                        case "1":
                            // return VisualizarGuiaConsulta();
                            break;

                        case "2":
                            var spsadtInput = new GuiaSpsadtInput
                            {
                                ContaId = input.ContaId,
                                AtendimentoId = input.AtendimentoId
                            };

                            // var res = await VisualizarSpsadt(spsadtInput);
                            var res = await VisualizarSpsadt((long)input.AtendimentoId);
                            //var res = await GuiaResumoInternacaoPdf((long)input.AtendimentoId);

                            return res;
                            break;
                        case "3":
                            res = await GuiaResumoInternacaoPdf((long)input.AtendimentoId);
                            return res;
                        default:
                            return null;
                            break;
                    }
                }
                catch (Exception ex)
                {

                }
                return null;
            }
        }

        // Guia Resumo Internacao

        FaturamentoContaDto contaDto = null;


        private async Task<ActionResult> GuiaResumoInternacaoPdf(long atendimentoId, long? contaId = null)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var contaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
                {
                    var atendimento = await atendimentoAppService.Object.Obter((long)atendimentoId);

                    FaturamentoConta conta = null;
                    if (contaId == null)
                    {
                        conta = contaRepository.Object.GetAll()
                                               .Include(i => i.ContaItens)
                                               .Include(i => i.ContaItens.Select(s => s.FaturamentoItem))
                                               .Include(i => i.ContaItens.Select(s => s.FaturamentoItem.Grupo))
                                               //.Include(i => i.ContaItens.Select(s => s.Medico))
                                               .Include(i => i.ContaItens.Select(s => s.MedicoEspecialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Auxiliar1))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar1Especialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Auxiliar2))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar2Especialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Auxiliar3))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar3Especialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Instrumentador))
                                               .Include(i => i.ContaItens.Select(s => s.InstrumentadorEspecialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Anestesista))
                                               .Include(i => i.ContaItens.Select(s => s.AnestesistaEspecialidade))
                                               .Include(i => i.ContaItens.Select(s => s.FaturamentoConfigConvenio))
                                               .Where(w => w.AtendimentoId == atendimentoId)
                                               .OrderBy(o => o.DataInicio)
                                               .FirstOrDefault();
                    }
                    else
                    {
                        conta = contaRepository.Object.GetAll()
                                           .Include(i => i.ContaItens)
                                           .Include(i => i.ContaItens.Select(s => s.FaturamentoItem))
                                           .Include(i => i.ContaItens.Select(s => s.FaturamentoItem.Grupo))
                                               //.Include(i => i.ContaItens.Select(s => s.Medico))
                                               .Include(i => i.ContaItens.Select(s => s.MedicoEspecialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Auxiliar1))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar1Especialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Auxiliar2))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar2Especialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Auxiliar3))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar3Especialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Instrumentador))
                                               .Include(i => i.ContaItens.Select(s => s.InstrumentadorEspecialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Anestesista))
                                               .Include(i => i.ContaItens.Select(s => s.AnestesistaEspecialidade))
                                               .Include(i => i.ContaItens.Select(s => s.FaturamentoConfigConvenio))
                                           .Where(w => w.Id == contaId)
                                           .FirstOrDefault();
                    }

                    contaDto = FaturamentoContaDto.Mapear(conta);

                    contaDto.Itens = new List<FaturamentoContaItemDto>();

                    foreach (var item in conta.ContaItens)
                    {
                        contaDto.Itens.Add(FaturamentoContaItemDto.MapearFromCore(item));
                    }


                    var dados = GuiaResumoInternacaoModel.MapearFromAtendimento(atendimento, contaDto);

                    // Guia principal
                    Web.Relatorios.Faturamento.Guias.InternacaoResumo.resumo_internacao_dataset resumo_internacao_dataset = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.resumo_internacao_dataset();
                    DataTable tabela = this.ConvertToDataTable(dados.Lista, resumo_internacao_dataset.Tables["resumo_internacao_table"]);
                    DataRow row = tabela.NewRow();
                    row["Logotipo"] = atendimento.Convenio.Logotipo; //atendimento.Empresa.Logotipo;
                                                                     //   tabela.Rows[tabela.Rows.Count - 1].Delete();
                    tabela.Rows.Add(row);

                    ReportDataSource dataSource = new ReportDataSource("resumo_internacao_dataset", tabela);
                    ReportViewer reportViewer = new ReportViewer();
                    reportViewer.LocalReport.DataSources.Add(dataSource);

                    ScriptManager scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(reportViewer);

                    reportViewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"\Relatorios\Faturamento\Guias\InternacaoResumo\guia_internacao_resumo.rdlc");

                    SetDadosProcedimentoExamesRealizados(reportViewer, contaDto.Itens);
                    SetEquipe(reportViewer, contaDto.Itens);

                    SetParametrosResumoInternacao(reportViewer, dados);

                    // var pdfOutrasDespesas = SetDadosOutrasDespesas(contaDto.Itens, dados, atendimento);

                    // APARENTEMENTE FALTANDO SUB-RELATORIOS

                    reportViewer.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;


                    //Web.Relatorios.Faturamento.Guias.InternacaoResumo.Internacao_outrasdespesas internacao_outrasdespesas = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.Internacao_outrasdespesas();
                    //DataTable outrasDespesas_tabela = internacao_outrasdespesas.Tables["OutrasDespesas"];

                    //ReportDataSource procedimentosRealizadodataSource = new ReportDataSource("ProcedimentosRealizados_dataSet", outrasDespesas_tabela);
                    //reportViewer.LocalReport.DataSources.Add(procedimentosRealizadodataSource);




                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = "pdf";

                    string[] streamIds;
                    Warning[] warnings;
                    byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


                    var listaPDFs = new List<byte[]> { pdfBytes };


                    var arquivosPdfOutrasDespesas = GerarOutrasDespesas(contaDto.Itens, dados, atendimento);

                    listaPDFs.AddRange(arquivosPdfOutrasDespesas);



                    var novoPdf = concatAndAddContent(listaPDFs);

                    var nomeArquivo = string.Concat(Guid.NewGuid().ToString(), ".pdf");

                    var path = string.Concat(Server.MapPath("/"), @"temp\", nomeArquivo);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    FileStream file = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                    file.Write(novoPdf, 0, novoPdf.Length);
                    file.Close();

                    reportViewer.LocalReport.Refresh();


                    Response.Headers.Add("Content-Disposition", string.Concat("inline; filename=", nomeArquivo));
                    //return File(pdfBytes, "application/pdf");
                    //return File(novoPdf, "application/pdf");

                    var pathRetorno = string.Concat(@"/temp/", nomeArquivo);

                    return Content(pathRetorno);
                }
            }
            catch (Exception e)
            {
                e.ToString();
                return null;
            }
        }

        public static byte[] concatAndAddContent(List<byte[]> pdfByteContent)
        {
            byte[] allBytes;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(iTextSharp.text.PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                iTextSharp.text.pdf.PdfReader reader;
                foreach (byte[] p in pdfByteContent)
                {
                    reader = new iTextSharp.text.pdf.PdfReader(p);
                    int pages = reader.NumberOfPages;

                    // loop over document pages
                    for (int i = 1; i <= pages; i++)
                    {
                        doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                        doc.NewPage();
                        page = writer.GetImportedPage(reader, i);

                        cb.AddTemplate(page, 0, 0);
                    }
                }

                doc.Close();
                allBytes = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();
            }

            return allBytes;
        }





        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            using (var profissionalSaudeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
            using (var especialidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Especialidade, long>>())
            {
                var faturamentoContaItens = contaDto.Itens;

                Web.Relatorios.Faturamento.Guias.InternacaoResumo.ProcedimentosRealizados_dataSet procedimentosRealizados_dataSet = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.ProcedimentosRealizados_dataSet();
                DataTable procedimentosRealizados_tabela = procedimentosRealizados_dataSet.Tables["ProcedimentoRealizado"];

                if (faturamentoContaItens != null)
                {
                    var fatItens = faturamentoContaItens.Where(w => string.IsNullOrEmpty(w.FaturamentoItem.Grupo.CodTipoOutraDespesa)).ToList();
                    int cont = 0;

                    for (int i = 10; i < fatItens.Count; i++)
                    {
                        var item = fatItens[i];

                        DataRow ProcedimentoRealizadorow = procedimentosRealizados_tabela.NewRow();
                        ProcedimentoRealizadorow["data"] = string.Format("{0:dd/MM/yyyy}", item.Data);
                        ProcedimentoRealizadorow["horaInicio"] = string.Format("{0:HH:mm}", item.HoraIncio);
                        ProcedimentoRealizadorow["horaFim"] = string.Format("{0:HH:mm}", item.HoraFim);
                        ProcedimentoRealizadorow["tabela"] = item.TabelaUtilizada;
                        ProcedimentoRealizadorow["codTUSS"] = item.FaturamentoItem.CodTuss;
                        ProcedimentoRealizadorow["descricaoTUSS"] = item.FaturamentoItem.DescricaoTuss;
                        ProcedimentoRealizadorow["qtde"] = item.Qtde;
                        ProcedimentoRealizadorow["viaAcesso"] = "01";
                        ProcedimentoRealizadorow["tecnicaUtilizada"] = "01";
                        ProcedimentoRealizadorow["reducaoAcrescimo"] = "0,00";
                        ProcedimentoRealizadorow["valorUnitario"] = string.Format("{0:#,##0.00}", item.ValorItem);
                        ProcedimentoRealizadorow["valorTotal"] = string.Format("{0:#,##0.00}", (item.Qtde * item.ValorItem));

                        procedimentosRealizados_tabela.Rows.Add(ProcedimentoRealizadorow);
                    }

                    ReportDataSource procedimentosRealizadodataSource = new ReportDataSource("ProcedimentosRealizados_dataSet", procedimentosRealizados_tabela);
                    e.DataSources.Add(procedimentosRealizadodataSource);

                }

                Web.Relatorios.Faturamento.Guias.InternacaoResumo.Equipe_dataSet equipe_dataSet = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.Equipe_dataSet();
                DataTable equipe_tabela = equipe_dataSet.Tables["Equipe"];

                if (medicosEspecialidades != null && medicosEspecialidades.Count() > 7)
                {

                    var dist = medicosEspecialidades.Where(w => w.ProfissionalId != null).ToList();




                    //Web.Relatorios.Faturamento.Guias.InternacaoResumo.Equipe_dataSet equipe_dataSet = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.Equipe_dataSet();
                    //DataTable equipe_tabela = equipe_dataSet.Tables["Equipe"];

                    // foreach (var item in dist)
                    for (int i = 8; i < dist.Count(); i++)
                    {
                        var item = dist[i];

                        if (item.ProfissionalId != null)
                        {
                            // qtd++;
                            var profissional = profissionalSaudeRepository.Object.GetAll()
                                                                          .Include(j => j.SisPessoa)
                                                                           .Include(j => j.SisPessoa.Enderecos)
                                                                           .Include(j => j.SisPessoa.Enderecos.Select(s => s.Estado))
                                                                          .Include(j => j.Estado)
                                                                          .Include(j => j.Conselho)
                                                                          .Where(w => w.Id == item.ProfissionalId)
                                                                          .FirstOrDefault();

                            if (profissional != null)
                            {
                                DataRow equiperow = equipe_tabela.NewRow();

                                equiperow["Seq"] = (i++).ToString().PadLeft(2, '0');
                                //equiperow["GrauParticipacao"] = item.FaturamentoConta.Medico
                                equiperow["CodigoNaOperadora_CPF"] = profissional.Cpf;
                                equiperow["NomeProfissional"] = profissional.NomeCompleto;
                                equiperow["ConselhoProfissional"] = profissional.Conselho?.Codigo;
                                equiperow["NumeroConselho"] = profissional.NumeroConselho;
                                equiperow["UF"] = profissional.Estado?.Codigo;

                                if (item.EspecialidadeId != null)
                                {
                                    var especialidade = especialidadeRepository.Object.GetAll()
                                                                               .Include(j => j.SisCbo)
                                                                               .Where(w => w.Id == item.EspecialidadeId)
                                                                               .FirstOrDefault();

                                    if (especialidade != null)
                                    {
                                        equiperow["CodigoCBO"] = especialidade.SisCbo?.Codigo;
                                    }
                                }

                                equipe_tabela.Rows.Add(equiperow);
                            }

                            //if (qtd == 7)
                            //{
                            //    break;
                            //}
                        }
                    }

                    //ReportDataSource equipedataSource = new ReportDataSource("Equipe_dataSet", equipe_tabela);
                    //e.DataSources.Add(equipedataSource);
                }


                ReportDataSource equipedataSource = new ReportDataSource("Equipe_dataSet", equipe_tabela);
                e.DataSources.Add(equipedataSource);

                // SetDadosOutrasDespesas(e, faturamentoContaItens);
            }
        }

        private void SetParametrosResumoInternacao(ReportViewer rv, GuiaResumoInternacaoModel dados)
        {
            ReportParameter NomePaciente = new ReportParameter("NomePaciente", dados.NomePaciente);
            ReportParameter Matricula = new ReportParameter("Matricula", !string.IsNullOrEmpty(dados.Matricula) ? dados.Matricula : "|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|");
            ReportParameter RegistroANS = new ReportParameter("RegistroANS", dados.RegistroANS);
            ReportParameter ValidadeCarteira = new ReportParameter("ValidadeCarteira", !string.IsNullOrEmpty(dados.ValidadeCarteira) ? dados.ValidadeCarteira : "|_|_|/|_|_|/|_|_|_|_|");
            ReportParameter Senha = new ReportParameter("Senha", dados.Senha);
            ReportParameter CodCNES = new ReportParameter("CodCNES", dados.CodCNES);
            ReportParameter DataAutorizacao = new ReportParameter("DataAutorizacao", !string.IsNullOrEmpty(dados.DataAutorizacao) ? dados.DataAutorizacao : "|_|_|/|_|_|/|_|_|_|_|");
            ReportParameter NomeContratado = new ReportParameter("NomeContratado", dados.NomeContratado);
            ReportParameter ValidadeSenha = new ReportParameter("ValidadeSenha", !string.IsNullOrEmpty(dados.ValidadeSenha) ? dados.ValidadeSenha : "|_|_|/|_|_|/|_|_|_|_|");// dados.ValidadeSenha);
            ReportParameter NumeroGuia = new ReportParameter("NumeroGuia", dados.NumeroGuia);
            ReportParameter Cid1 = new ReportParameter("Cid1", dados.Cid1);
            ReportParameter Cid2 = new ReportParameter("Cid2", dados.Cid2);
            ReportParameter Cid3 = new ReportParameter("Cid3", dados.Cid3);
            ReportParameter Cid4 = new ReportParameter("Cid4", dados.Cid4);
            ReportParameter CodOperadora = new ReportParameter("CodOperadora", !string.IsNullOrEmpty(dados.CodOperadora) ? dados.CodOperadora : "|_|_|_|_|_|_|_|_|_|_|_|_|_|_|");
            ReportParameter CaraterAtendimento = new ReportParameter("CaraterAtendimento", dados.CaraterAtendimento);
            ReportParameter TipoFaturamento = new ReportParameter("TipoFaturamento", dados.TipoFaturamento);
            ReportParameter DataIniFaturamento = new ReportParameter("DataIniFaturamento", dados.DataIniFaturamento);
            ReportParameter DataFimFaturamento = new ReportParameter("DataFimFaturamento", dados.DataFimFaturamento);
            ReportParameter HoraIniFaturamento = new ReportParameter("HoraIniFaturamento", dados.HoraIniFaturamento);
            ReportParameter HoraFimFaturamento = new ReportParameter("HoraFimFaturamento", dados.HoraFimFaturamento);
            ReportParameter TipoInternacao = new ReportParameter("TipoInternacao", dados.TipoInternacao);
            ReportParameter RegimeInternacao = new ReportParameter("RegimeInternacao", dados.RegimeInternacao);
            //  ReportParameter TotalProcedimentos = new ReportParameter("TotalProcedimentos", dados.TotalProcedimentos);
            ReportParameter TotalDiaria = new ReportParameter("TotalDiaria", dados.TotalDiaria);
            ReportParameter TotalTaxasAlugueis = new ReportParameter("TotalTaxasAlugueis", dados.TotalTaxasAlugueis);
            ReportParameter TotalMateriais = new ReportParameter("TotalMateriais", dados.TotalMateriais);
            ReportParameter TotalOpme = new ReportParameter("TotalOpme", dados.TotalOpme);
            ReportParameter TotalMedicamentos = new ReportParameter("TotalMedicamentos", dados.TotalMedicamentos);
            ReportParameter TotalGasesMedicinais = new ReportParameter("TotalGasesMedicinais", dados.TotalGasesMedicinais);
            ReportParameter TotalGeral = new ReportParameter("TotalGeral", dados.TotalGeral);
            ReportParameter RN = new ReportParameter("RN", dados.RN ? "S" : "N");
            ReportParameter CNS = new ReportParameter("CNS", !string.IsNullOrEmpty(dados.CNS) ? dados.CNS : "|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|");
            ReportParameter IndicadorAcidente = new ReportParameter("IndicadorAcidente", dados.IndicadorAcidente);
            ReportParameter MotivoEncerramento = new ReportParameter("MotivoEncerramento", dados.MotivoEncerramento);
            ReportParameter CidObito = new ReportParameter("CidObito", dados.CidObito);

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
                //TotalProcedimentos      ,
                //TotalDiaria             ,
                //TotalTaxasAlugueis      ,
                //TotalMateriais          ,
                //TotalOpme               ,
                //TotalMedicamentos       ,
                //TotalGasesMedicinais    ,
                //TotalGeral              ,
                RN,
                CNS,
                IndicadorAcidente,
                MotivoEncerramento,
                CidObito
            });
        }

        private void SetDadosProcedimentoExamesRealizados(ReportViewer reportViewer, List<FaturamentoContaItemDto> faturamentoContaItens)
        {
            Web.Relatorios.Faturamento.Guias.InternacaoResumo.ProcedimentosRealizados_dataSet procedimentosRealizados_dataSet = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.ProcedimentosRealizados_dataSet();

            DataTable procedimentosRealizados_tabela = procedimentosRealizados_dataSet.Tables["ProcedimentoRealizado"];
            if (faturamentoContaItens != null)
            {



                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);
                //faturamentoContaItens.AddRange(faturamentoContaItens);


                var fatItens = faturamentoContaItens.Where(w => string.IsNullOrEmpty(w.FaturamentoItem.Grupo.CodTipoOutraDespesa));
                // var qtdReal = fatItens.Count();

                int cont = 0;



                foreach (var item in fatItens)
                {
                    DataRow ProcedimentoRealizadorow = procedimentosRealizados_tabela.NewRow();
                    ProcedimentoRealizadorow["data"] = string.Format("{0:dd/MM/yyyy}", item.Data);
                    ProcedimentoRealizadorow["horaInicio"] = string.Format("{0:HH:mm}", item.HoraIncio);
                    ProcedimentoRealizadorow["horaFim"] = string.Format("{0:HH:mm}", item.HoraFim);
                    ProcedimentoRealizadorow["tabela"] = item.TabelaUtilizada;
                    ProcedimentoRealizadorow["codTUSS"] = item.FaturamentoItem.CodTuss;
                    ProcedimentoRealizadorow["descricaoTUSS"] = item.FaturamentoItem.DescricaoTuss;
                    ProcedimentoRealizadorow["qtde"] = item.Qtde;
                    ProcedimentoRealizadorow["viaAcesso"] = "01";
                    ProcedimentoRealizadorow["tecnicaUtilizada"] = "01";
                    ProcedimentoRealizadorow["reducaoAcrescimo"] = "0,00";
                    ProcedimentoRealizadorow["valorUnitario"] = string.Format("{0:#,##0.00}", item.ValorItem);
                    ProcedimentoRealizadorow["valorTotal"] = string.Format("{0:#,##0.00}", (item.Qtde * item.ValorItem));

                    procedimentosRealizados_tabela.Rows.Add(ProcedimentoRealizadorow);

                    cont++;

                    if (cont == 10)
                    {
                        break;
                    }
                }

                for (int i = cont; i < 10; i++)
                {

                    DataRow ProcedimentoRealizadorow = procedimentosRealizados_tabela.NewRow();
                    ProcedimentoRealizadorow["data"] = "___________________";
                    ProcedimentoRealizadorow["horaInicio"] = "_____";
                    ProcedimentoRealizadorow["horaFim"] = "______";
                    ProcedimentoRealizadorow["tabela"] = "____";
                    ProcedimentoRealizadorow["codTUSS"] = "____________";
                    ProcedimentoRealizadorow["descricaoTUSS"] = "______________________________________";
                    ProcedimentoRealizadorow["qtde"] = "____________________";
                    ProcedimentoRealizadorow["viaAcesso"] = "_____";
                    ProcedimentoRealizadorow["tecnicaUtilizada"] = "_____";
                    ProcedimentoRealizadorow["reducaoAcrescimo"] = "________________";
                    ProcedimentoRealizadorow["valorUnitario"] = "________________";
                    ProcedimentoRealizadorow["valorTotal"] = "________________";



                    procedimentosRealizados_tabela.Rows.Add(ProcedimentoRealizadorow);
                }

                var valorTotal = fatItens.Sum(s => s.ValorItem * s.Qtde);
                ReportParameter rpTotalProcedimentos = new ReportParameter("TotalProcedimentos", string.Format("{0:#,##0.00}", valorTotal));

                var totalDiarias = faturamentoContaItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "05" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Diarias)).Sum(s => s.ValorItem * s.Qtde);
                ReportParameter rpTotalDiaria = new ReportParameter("TotalDiaria", string.Format("{0:#,##0.00}", totalDiarias));

                var totalTaxasAlugueis = faturamentoContaItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "07" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.TaxasAluguéis)).Sum(s => s.ValorItem * s.Qtde);
                ReportParameter rpTotalTaxasAlugueis = new ReportParameter("TotalTaxasAlugueis", string.Format("{0:#,##0.00}", totalTaxasAlugueis));

                var totalMateriais = faturamentoContaItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "03" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Materiais)).Sum(s => s.ValorItem * s.Qtde);
                ReportParameter rpTotalMateriais = new ReportParameter("TotalMateriais", string.Format("{0:#,##0.00}", totalMateriais));

                var totalOPME = faturamentoContaItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "08" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.OPME)).Sum(s => s.ValorItem * s.Qtde);
                ReportParameter rpTotalOPME = new ReportParameter("TotalOpme", string.Format("{0:#,##0.00}", totalOPME));

                var totalMedicamentos = faturamentoContaItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "02" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Medicamentos)).Sum(s => s.ValorItem * s.Qtde);
                ReportParameter rpTotalMedicamentos = new ReportParameter("TotalMedicamentos", string.Format("{0:#,##0.00}", totalMedicamentos));

                var totalGasesMedicinais = faturamentoContaItens.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "01" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.GasesMedicinais)).Sum(s => s.ValorItem * s.Qtde);
                ReportParameter rpTotalGasesMedicinais = new ReportParameter("TotalGasesMedicinais", string.Format("{0:#,##0.00}", totalGasesMedicinais));

                var totalGeral = faturamentoContaItens.Sum(s => s.ValorItem * s.Qtde);
                ReportParameter rpTotalGeral = new ReportParameter("TotalGeral", string.Format("{0:#,##0.00}", totalGeral));







                ReportParameter rpExibirSegundaPagina = new ReportParameter("ExibirSegundaPagina", faturamentoContaItens.Count > 10 ? "S" : "N");



                reportViewer.LocalReport.SetParameters(new ReportParameter[] { rpTotalProcedimentos
                                                                             , rpTotalDiaria
                                                                             , rpTotalTaxasAlugueis
                                                                             , rpTotalMateriais
                                                                             , rpTotalOPME
                                                                             , rpTotalMedicamentos
                                                                             , rpTotalGasesMedicinais
                                                                             , rpTotalGeral
                                                                             , rpExibirSegundaPagina});


            }
            ReportDataSource procedimentosRealizadodataSource = new ReportDataSource("ProcedimentosRealizados_dataSet", procedimentosRealizados_tabela);

            reportViewer.LocalReport.DataSources.Add(procedimentosRealizadodataSource);



        }


        private List<byte[]> GerarOutrasDespesas(List<FaturamentoContaItemDto> faturamentoContaItensTodos, GuiaResumoInternacaoModel dados, AtendimentoDto atendimento)
        {
            List<byte[]> arquivos = new List<byte[]>();

            List<FaturamentoContaItemDto> contaItensPorPagina = new List<FaturamentoContaItemDto>();

            int i = 0;


            //faturamentoContaItens.AddRange(faturamentoContaItens);
            //faturamentoContaItens.AddRange(faturamentoContaItens);
            //faturamentoContaItens.AddRange(faturamentoContaItens);
            //faturamentoContaItens.AddRange(faturamentoContaItens);
            //faturamentoContaItens.AddRange(faturamentoContaItens);
            //faturamentoContaItens.AddRange(faturamentoContaItens);

            var faturamentoContaItens = faturamentoContaItensTodos.Where(w => !string.IsNullOrEmpty(w.FaturamentoItem.Grupo.CodTipoOutraDespesa) || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId != null);


            foreach (var item in faturamentoContaItens)
            {
                contaItensPorPagina.Add(item);
                i++;

                if (i % 7 == 0)
                {
                    arquivos.Add(SetDadosOutrasDespesas(contaItensPorPagina, dados, atendimento));
                    i = 0;
                    contaItensPorPagina = new List<FaturamentoContaItemDto>();
                }
            }

            if (contaItensPorPagina.Count > 0)
            {

                for (int j = contaItensPorPagina.Count; j < 7; j++)
                {
                    contaItensPorPagina.Add(new FaturamentoContaItemDto());
                }

                arquivos.Add(SetDadosOutrasDespesas(contaItensPorPagina, dados, atendimento));
            }

            return arquivos;
        }



        private byte[] SetDadosOutrasDespesas(List<FaturamentoContaItemDto> faturamentoContaItens, GuiaResumoInternacaoModel dados, AtendimentoDto atendimento)
        {

            ReportViewer reportViewer = new ReportViewer();
            ScriptManager scriptManager = new ScriptManager();

            scriptManager.RegisterPostBackControl(reportViewer);
            reportViewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"\Relatorios\Faturamento\Guias\InternacaoResumo\guia_internacao_outrasdespesas.rdlc");

            ReportParameter RegistroANS = new ReportParameter("RegistroANS", dados.RegistroANS);
            ReportParameter NomeContratado = new ReportParameter("NomeContratado", dados.NomeContratado);
            ReportParameter CodCNES = new ReportParameter("CNES", dados.CodCNES);
            ReportParameter NumeroGuia = new ReportParameter("NumeroGuiaReferencia", dados.NumeroGuia);
            ReportParameter CodOperadora = new ReportParameter("CodigoNaOperadora", !string.IsNullOrEmpty(dados.CodOperadora) ? dados.CodOperadora : "|_|_|_|_|_|_|_|_|_|_|_|_|_|_|");





            Web.Relatorios.Faturamento.Guias.InternacaoResumo.resumo_internacao_dataset resumo_internacao_dataset = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.resumo_internacao_dataset();
            DataTable tabela = this.ConvertToDataTable(dados.Lista, resumo_internacao_dataset.Tables["resumo_internacao_table"]);
            DataRow row = tabela.NewRow();
            row["Logotipo"] = atendimento.Convenio.Logotipo; //atendimento.Empresa.Logotipo;
                                                             //   tabela.Rows[tabela.Rows.Count - 1].Delete();
            tabela.Rows.Add(row);

            ReportDataSource dataSource = new ReportDataSource("resumo_internacao_dataset", tabela);

            reportViewer.LocalReport.DataSources.Add(dataSource);


            var contaItensComItemFaturamento = faturamentoContaItens.Where(w => w.FaturamentoItem != null);

            var valorTotal = faturamentoContaItens.Sum(s => s.ValorItem * s.Qtde);
            ReportParameter rpTotalProcedimentos = new ReportParameter("TotalProcedimentos", string.Format("{0:#,##0.00}", valorTotal));

            var totalDiarias = contaItensComItemFaturamento.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "05" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Diarias)).Sum(s => s.ValorItem * s.Qtde);
            ReportParameter rpTotalDiaria = new ReportParameter("TotalDiaria", string.Format("{0:#,##0.00}", totalDiarias));

            var totalTaxasAlugueis = contaItensComItemFaturamento.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "07" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.TaxasAluguéis)).Sum(s => s.ValorItem * s.Qtde);
            ReportParameter rpTotalTaxasAlugueis = new ReportParameter("TotalTaxasAlugueis", string.Format("{0:#,##0.00}", totalTaxasAlugueis));

            var totalMateriais = contaItensComItemFaturamento.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "03" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Materiais)).Sum(s => s.ValorItem * s.Qtde);
            ReportParameter rpTotalMateriais = new ReportParameter("TotalMateriais", string.Format("{0:#,##0.00}", totalMateriais));

            var totalOPME = contaItensComItemFaturamento.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "08" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.OPME)).Sum(s => s.ValorItem * s.Qtde);
            ReportParameter rpTotalOPME = new ReportParameter("TotalOpme", string.Format("{0:#,##0.00}", totalOPME));

            var totalMedicamentos = contaItensComItemFaturamento.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "02" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.Medicamentos)).Sum(s => s.ValorItem * s.Qtde);
            ReportParameter rpTotalMedicamentos = new ReportParameter("TotalMedicamentos", string.Format("{0:#,##0.00}", totalMedicamentos));

            var totalGasesMedicinais = contaItensComItemFaturamento.Where(w => (w.FaturamentoItem.Grupo.CodTipoOutraDespesa == "01" || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == (long)EnumCodigoDespesa.GasesMedicinais)).Sum(s => s.ValorItem * s.Qtde);
            ReportParameter rpTotalGasesMedicinais = new ReportParameter("TotalGasesMedicinais", string.Format("{0:#,##0.00}", totalGasesMedicinais));


            reportViewer.LocalReport.SetParameters(new ReportParameter[] {
                RegistroANS
               ,  NomeContratado
               ,  CodCNES
               ,  NumeroGuia
               ,  CodOperadora
               ,  rpTotalProcedimentos
                , rpTotalDiaria
                , rpTotalTaxasAlugueis
                , rpTotalMateriais
                , rpTotalOPME
                , rpTotalMedicamentos
                , rpTotalGasesMedicinais

            });

            // 7 é o número maximo de registros por página
            for (int j = 0; j < 7; j++)
            {
                FaturamentoContaItemDto contaItem = null;
                if (faturamentoContaItens.Count >= j + 1)
                {
                    contaItem = faturamentoContaItens[j];
                }
                else
                {
                    contaItem = new FaturamentoContaItemDto();
                }

                var i = j + 1;

                ReportParameter CD = new ReportParameter(string.Concat("CD", i), contaItem.FaturamentoItem != null ? contaItem.FaturamentoItem.Grupo.CodTipoOutraDespesa.LastOrDefault().ToString() : "|_|");
                ReportParameter Data = new ReportParameter(string.Concat("Data", i), contaItem.Data != null ? string.Format("{0:dd/MM/yyyy}", contaItem.Data) : "|_|_|/|_|_|/|_|_|_|_|");
                ReportParameter HoraInicial = new ReportParameter(string.Concat("HoraInicial", i), contaItem.HoraIncio != null ? string.Format("{0:HH:mm}", contaItem.HoraIncio) : "|_|_|:|_|_|");
                ReportParameter HoraFinal = new ReportParameter(string.Concat("HoraFinal", i), contaItem.HoraFim != null ? string.Format("{0:HH:mm}", contaItem.HoraFim) : "|_|_|:|_|_|");
                ReportParameter Tabela = new ReportParameter(string.Concat("Tabela", i), (contaItem.FaturamentoConfigConvenioDto != null && !string.IsNullOrEmpty(contaItem.FaturamentoConfigConvenioDto.Codigo)) ? contaItem.FaturamentoConfigConvenioDto.Codigo : "|_|_|");
                ReportParameter CodigoItem = new ReportParameter(string.Concat("CodItem", i), (contaItem.FaturamentoItem != null && !string.IsNullOrEmpty(contaItem.FaturamentoItem.CodTuss)) ? contaItem.FaturamentoItem.CodTuss : "|_|_|_|_|_|_|_|_|_|_|");
                ReportParameter Descricao = new ReportParameter(string.Concat("Descricao", i), (contaItem.FaturamentoItem != null && !string.IsNullOrEmpty(contaItem.FaturamentoItem.DescricaoTuss)) ? contaItem.FaturamentoItem.DescricaoTuss : "");
                ReportParameter Qtd = new ReportParameter(string.Concat("Qtd", i), contaItem.Qtde != 0 ? string.Format("{0:#,##0.00}", contaItem.Qtde) : "|_|_|_|_|,|_|_|");
                ReportParameter UnidadeMedida = new ReportParameter(string.Concat("UnidadeMedida", i), "|_|_|_|");
                ReportParameter RedAcres = new ReportParameter(string.Concat("RedAcresc", i), "|_|_|");
                ReportParameter ValorUnitario = new ReportParameter(string.Concat("ValorUnitario", i), contaItem.ValorItem != 0 ? string.Format("{0:#,##0.00}", contaItem.ValorItem) : "|_|_|_|_|_|_|,|_|_|");
                ReportParameter ValorTotal = new ReportParameter(string.Concat("ValorTotal", i), (contaItem.ValorItem != 0 && contaItem.Qtde != 0) ? string.Format("{0:#,##0.00}", (contaItem.Qtde * contaItem.ValorItem)) : "|_|_|_|_|_|_|,|_|_|");
                ReportParameter RegistroAnvisa = new ReportParameter(string.Concat("RegistroAnvisa", i), "|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|");
                ReportParameter ReferenciaMaterial = new ReportParameter(string.Concat("ReferenciaMaterial", i), "|_______________________________________________________________|");
                ReportParameter AutorizacaoFuncionamento = new ReportParameter(string.Concat("AutorizacaoFuncionamento", i), "|_|_|_|_|_|_|_|_|_|_|_|_|_|_|_|");

                reportViewer.LocalReport.SetParameters(new ReportParameter[] {
                CD,
                Data,
                HoraInicial,
                HoraFinal,
                HoraFinal,
                Tabela,
                CodigoItem,
                Descricao,
                Qtd,
                UnidadeMedida,
                RedAcres,
                ValorUnitario,
                ValorTotal,
                RegistroAnvisa,
                ReferenciaMaterial,
                AutorizacaoFuncionamento

                }
                );
            }



            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = "pdf";

            string[] streamIds;
            Warning[] warnings;
            byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            return pdfBytes;


            //Web.Relatorios.Faturamento.Guias.InternacaoResumo.Internacao_outrasdespesas_dataSet internacao_outrasdespesas = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.Internacao_outrasdespesas_dataSet();

            //DataTable outrasDespesas_tabela = internacao_outrasdespesas.Tables["OutraDespesa"];

            //if (faturamentoContaItens != null)
            //{
            //    var outrasDespesas = faturamentoContaItens.ToList();//.Where(w => !string.IsNullOrEmpty(w.FaturamentoItem.Grupo.CodTipoOutraDespesa) || w.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId !=null);


            //    outrasDespesas.AddRange(outrasDespesas);
            //    //outrasDespesas.AddRange(outrasDespesas);
            //    //outrasDespesas.AddRange(outrasDespesas);
            //    //outrasDespesas.AddRange(outrasDespesas);
            //    //outrasDespesas.AddRange(outrasDespesas);
            //    int j = 0;

            //    for (int i = 0; i < 2; i++)
            //    {


            //        foreach (var item in outrasDespesas)
            //        {
            //            DataRow outraDespesarow = outrasDespesas_tabela.NewRow();
            //            outraDespesarow["Data"] = string.Format("{0:dd/MM/yyyy}", item.Data);
            //            outraDespesarow["HoraInicial"] = string.Format("{0:HH:mm}", item.HoraIncio);
            //            outraDespesarow["HoraFinal"] = string.Format("{0:HH:mm}", item.HoraFim);
            //            outraDespesarow["Tabela"] = item.TabelaUtilizada;
            //            outraDespesarow["CodigoItem"] = item.FaturamentoItem.CodTuss;
            //            outraDespesarow["Descricao"] = item.FaturamentoItem.DescricaoTuss;
            //            outraDespesarow["Qtde"] = item.Qtde;
            //            //outraDespesarow["viaAcesso"] = "01";
            //            //outraDespesarow["tecnicaUtilizada"] = "01";
            //            outraDespesarow["RedAcres"] = "0,00";
            //            outraDespesarow["ValorUnitario"] = string.Format("{0:#,##0.00}", item.ValorItem);
            //            outraDespesarow["ValorTotal"] = string.Format("{0:#,##0.00}", (item.Qtde * item.ValorItem));
            //            outraDespesarow["ValorTotalDecimal"] = item.Qtde * item.ValorItem;
            //            outraDespesarow["CodigoDespesa"] = item.FaturamentoItem.Grupo.CodTipoOutraDespesa;
            //            outraDespesarow["Seq"] = j++;

            //            outrasDespesas_tabela.Rows.Add(outraDespesarow);
            //        }

            //        ReportDataSource procedimentosRealizadodataSource = new ReportDataSource("Internacao_outrasdespesas_dataSet", outrasDespesas_tabela);

            //        // reportViewer.LocalReport.DataSources.Add(procedimentosRealizadodataSource);

            //        e.DataSources.Add(procedimentosRealizadodataSource);
            //    }

            // }
        }



        private List<byte[]> GerarSpsadtContinuacao(List<FaturamentoContaItemDto> faturamentoContaItensTodos, GuiaResumoInternacaoModel dados, AtendimentoDto atendimento)
        {
            List<byte[]> arquivos = new List<byte[]>();

            List<FaturamentoContaItemDto> contaItensPorPagina = new List<FaturamentoContaItemDto>();

            int i = 0;

            foreach (var item in faturamentoContaItensTodos)
            {
                contaItensPorPagina.Add(item);
                i++;

                if (i % 10 == 0)
                {
                    arquivos.Add(SetDadosSpsadtContinuacao(contaItensPorPagina, dados, atendimento));
                    i = 0;
                    contaItensPorPagina = new List<FaturamentoContaItemDto>();
                }
            }

            if (contaItensPorPagina.Count > 0)
            {

                for (int j = contaItensPorPagina.Count; j < 10; j++)
                {
                    contaItensPorPagina.Add(new FaturamentoContaItemDto());
                }

                arquivos.Add(SetDadosSpsadtContinuacao(contaItensPorPagina, dados, atendimento));
            }



            return arquivos;


        }

        private byte[] SetDadosSpsadtContinuacao(List<FaturamentoContaItemDto> faturamentoContaItens, GuiaResumoInternacaoModel dados, AtendimentoDto atendimento)
        {


            using (var profissionalSaudeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
            using (var especialidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Especialidade, long>>())
            {
                ReportViewer reportViewer = new ReportViewer();
                ScriptManager scriptManager = new ScriptManager();

                scriptManager.RegisterPostBackControl(reportViewer);
                reportViewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"\Relatorios\Faturamento\Guias\Spsadt\ItensSolicitadosContinuacao.rdlc");

                Web.Relatorios.Faturamento.Guias.Guias guias = new Web.Relatorios.Faturamento.Guias.Guias();
                DataTable tabelaItensSolicitacaoes = this.ConvertToDataTable(dados.Lista, guias.Tables["SpsadtItensSolic"]);

                foreach (var item in faturamentoContaItens)
                {
                    DataRow rowItensSolicitacaoes = tabelaItensSolicitacaoes.NewRow();

                    rowItensSolicitacaoes["Tabela"] = item.FaturamentoConfigConvenioDto?.Codigo ?? "______";
                    rowItensSolicitacaoes["CodigoProcedimento"] = item.FaturamentoItem?.CodTuss ?? "______________";

                    var tamanho = !string.IsNullOrEmpty(item.FaturamentoItem?.DescricaoTuss) ? (item.FaturamentoItem.DescricaoTuss.Length < 30 ? item.FaturamentoItem.DescricaoTuss.Length : 30) : 0;
                    rowItensSolicitacaoes["Descricao"] = tamanho > 0 ? item.FaturamentoItem.DescricaoTuss.Substring(0, tamanho) : "______________________________________________________________";

                    //rowItensSolicitacaoes["Descricao"] = item.FaturamentoItem?.DescricaoTuss?? "______________________________________________________________";
                    rowItensSolicitacaoes["QtSolic"] = item.Qtde != 0 ? item.Qtde.ToString() : "________";
                    rowItensSolicitacaoes["QtAutoriz"] = item.Qtde != 0 ? item.Qtde.ToString() : "________";

                    tabelaItensSolicitacaoes.Rows.Add(rowItensSolicitacaoes);
                }

                ReportDataSource dataSource = new ReportDataSource("GuiaSpsadtItensSolicContinuacao", tabelaItensSolicitacaoes);
                reportViewer.LocalReport.DataSources.Add(dataSource);




                DataTable tabelaExames = this.ConvertToDataTable(dados.Lista, guias.Tables["SpsadtExames"]);


                foreach (var item in faturamentoContaItens)
                {
                    DataRow rowExames = tabelaExames.NewRow();

                    rowExames["Data"] = item.Data != null ? string.Format("{0:dd/MM/yyyy}", item.Data) : "___________";
                    rowExames["HoraInicial"] = item.HoraIncio != null ? string.Format("{0:HH:mm}", item.HoraIncio) : "_______";
                    rowExames["HoraFinal"] = item.HoraFim != null ? string.Format("{0:HH:mm}", item.HoraFim) : "_______";
                    rowExames["Tabela"] = item.FaturamentoConfigConvenioDto?.Codigo ?? "_____";
                    rowExames["CodigoProcedimento"] = item.FaturamentoItem?.CodTuss ?? "____________";

                    var tamanho = !string.IsNullOrEmpty(item.FaturamentoItem?.DescricaoTuss) ? (item.FaturamentoItem.DescricaoTuss.Length < 30 ? item.FaturamentoItem.DescricaoTuss.Length : 30) : 0;
                    rowExames["Descricao"] = tamanho > 0 ? item.FaturamentoItem.DescricaoTuss.Substring(0, tamanho) : "___________________________________";

                    // rowExames["Descricao"] = item.FaturamentoItem?.DescricaoTuss ?? "____________________________";
                    rowExames["Qtde"] = item.Qtde != 0 ? item.Qtde.ToString() : "_____";
                    rowExames["Via"] = item.ViaAcesso ?? "____";
                    rowExames["Tec"] = item.Tecnica ?? "____";
                    rowExames["RedAcresc"] = "1";
                    rowExames["ValorUnitario"] = string.Format("{0:#,##0.00}", item.ValorItem);
                    rowExames["ValorTotal"] = string.Format("{0:#,##0.00}", (item.Qtde * item.ValorItem));

                    tabelaExames.Rows.Add(rowExames);
                }

                ReportDataSource dataSourceExame = new ReportDataSource("SpsadtExamesContinuacao", tabelaExames);
                reportViewer.LocalReport.DataSources.Add(dataSourceExame);
                DataTable tabelaEquipe = this.ConvertToDataTable(dados.Lista, guias.Tables["SpsadtEquipe"]);

                int qtd = 0;
                foreach (var item in medicosEspecialidades)
                {
                    if (qtd < 4)
                    {
                        qtd++;
                        continue;
                    }


                    DataRow rowExames = tabelaEquipe.NewRow();

                    if (item.ProfissionalId != null)
                    {

                        var profissional = profissionalSaudeRepository.Object.GetAll()
                                                                      .Include(j => j.SisPessoa)
                                                                       .Include(j => j.SisPessoa.Enderecos)
                                                                       .Include(j => j.SisPessoa.Enderecos.Select(s => s.Estado))
                                                                      .Include(j => j.Estado)
                                                                      .Include(j => j.Conselho)
                                                                      .Where(w => w.Id == item.ProfissionalId)
                                                                      .FirstOrDefault();

                        if (profissional != null)
                        {


                            //rowExames["CodigoCbo"] = profissional.Cpf ??"____________";
                            rowExames["CodigoOperadoraCpf"] = profissional.Cpf ?? "___________________";
                            rowExames["ConselhoProfissional"] = profissional.Conselho?.Codigo ?? "____________";
                            rowExames["GrauPart"] = "_____";
                            rowExames["NomeProfissional"] = profissional.NomeCompleto ?? "____________________________________________________________";
                            rowExames["NumeroConselho"] = profissional.NumeroConselho.ToString() ?? "____________";
                            rowExames["SeqRef"] = "____";// (i++).ToString().PadLeft(2, '0');
                            rowExames["Uf"] = profissional.Estado?.Codigo ?? "___";
                        }

                        if (item.EspecialidadeId != null)
                        {
                            var especialidade = especialidadeRepository.Object.GetAll()
                                                                       .Include(j => j.SisCbo)
                                                                       .Where(w => w.Id == item.EspecialidadeId)
                                                                       .FirstOrDefault();

                            if (especialidade != null)
                            {
                                rowExames["CodigoCbo"] = especialidade.SisCbo?.Codigo ?? "____________";
                            }
                            else
                            {
                                rowExames["CodigoCbo"] = "____________";
                            }
                        }
                        else
                        {
                            rowExames["CodigoCbo"] = "____________";
                        }

                    }

                    tabelaEquipe.Rows.Add(rowExames);
                }


                ReportDataSource dataSourceEquipe = new ReportDataSource("SpsadtEquipeContinuacao", tabelaEquipe);
                reportViewer.LocalReport.DataSources.Add(dataSourceEquipe);

                string mimeType = string.Empty;
                string encoding = string.Empty;
                string extension = "pdf";

                string[] streamIds;
                Warning[] warnings;
                byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                return pdfBytes;
            }

        }



        class ProfissionalEspecialidade
        {
            public long? ProfissionalId { get; set; }
            public long? EspecialidadeId { get; set; }
        }


        List<ProfissionalEspecialidade> medicosEspecialidades;

        private void SetEquipe(ReportViewer reportViewer, List<FaturamentoContaItemDto> faturamentoContaItens)
        {
            using (var profissionalSaudeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
            using (var especialidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Especialidade, long>>())
            {
                Web.Relatorios.Faturamento.Guias.InternacaoResumo.Equipe_dataSet equipe_dataSet = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.Equipe_dataSet();

                DataTable equipe_tabela = equipe_dataSet.Tables["Equipe"];
                if (faturamentoContaItens != null)
                {
                    var fatContaItens = faturamentoContaItens.Where(w => string.IsNullOrEmpty(w.FaturamentoItem.Grupo.CodTipoOutraDespesa));

                    medicosEspecialidades = new List<ProfissionalEspecialidade>();

                    // var qtdReal = fatItens.Count();

                    int i = 1;

                    foreach (var item in fatContaItens)
                    {
                        medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.MedicoId, EspecialidadeId = item.MedicoEspecialidade?.EspecialidadeId });
                        medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.Auxiliar1Id, EspecialidadeId = item.Auxiliar1Especialidade?.EspecialidadeId });
                        medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.Auxiliar2Id, EspecialidadeId = item.Auxiliar2Especialidade?.EspecialidadeId });
                        medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.Auxiliar3Id, EspecialidadeId = item.Auxiliar3Especialidade?.EspecialidadeId });
                        medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.InstrumentadorId, EspecialidadeId = item.InstrumentadorEspecialidade?.EspecialidadeId });
                        medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.AnestesistaId, EspecialidadeId = item.EspecialidadeAnestesista?.EspecialidadeId });
                    }

                    var dist = medicosEspecialidades.ToList();

                    _medicosEspecialidades = dist;

                    var qtdReal = dist.Count();

                    int qtd = 0;

                    foreach (var item in dist)
                    {
                        if (item.ProfissionalId != null)
                        {
                            qtd++;
                            var profissional = profissionalSaudeRepository.Object.GetAll()
                                                                          .Include(j => j.SisPessoa)
                                                                           .Include(j => j.SisPessoa.Enderecos)
                                                                           .Include(j => j.SisPessoa.Enderecos.Select(s => s.Estado))
                                                                          .Include(j => j.Estado)
                                                                          .Include(j => j.Conselho)
                                                                          .Where(w => w.Id == item.ProfissionalId)
                                                                          .FirstOrDefault();

                            if (profissional != null)
                            {
                                DataRow equiperow = equipe_tabela.NewRow();

                                equiperow["Seq"] = (i++).ToString().PadLeft(2, '0');
                                //equiperow["GrauParticipacao"] = item.FaturamentoConta.Medico
                                equiperow["CodigoNaOperadora_CPF"] = profissional.Cpf;
                                equiperow["NomeProfissional"] = profissional.NomeCompleto;
                                equiperow["ConselhoProfissional"] = profissional.Conselho?.Codigo;
                                equiperow["NumeroConselho"] = profissional.NumeroConselho;
                                equiperow["UF"] = profissional.Estado?.Codigo;

                                if (item.EspecialidadeId != null)
                                {
                                    var especialidade = especialidadeRepository.Object.GetAll()
                                                                               .Include(j => j.SisCbo)
                                                                               .Where(w => w.Id == item.EspecialidadeId)
                                                                               .FirstOrDefault();

                                    if (especialidade != null)
                                    {
                                        equiperow["CodigoCBO"] = especialidade.SisCbo?.Codigo;
                                    }
                                }

                                equipe_tabela.Rows.Add(equiperow);
                            }

                            if (qtd == 7)
                            {
                                break;
                            }
                        }

                    }

                    for (int j = qtd; j < 7; j++)
                    {
                        DataRow equiperow = equipe_tabela.NewRow();

                        equiperow["Seq"] = "_____";
                        equiperow["GrauParticipacao"] = "_____";
                        equiperow["CodigoNaOperadora_CPF"] = "_____________";
                        equiperow["NomeProfissional"] = "_________________________________________________________________________";
                        equiperow["ConselhoProfissional"] = "__________";
                        equiperow["NumeroConselho"] = "____________";
                        equiperow["UF"] = "____";
                        equiperow["CodigoCBO"] = "__________";


                        equipe_tabela.Rows.Add(equiperow);

                    }



                }


                ReportDataSource equipeSource = new ReportDataSource("Equipe_dataSet", equipe_tabela);

                reportViewer.LocalReport.DataSources.Add(equipeSource);
            }
        }



        // Fim - guia resumo internacao
        //public async Task<ActionResult> VisualizarSpsadt(GuiaSpsadtInput input)
        public async Task<ActionResult> VisualizarSpsadt(long atendimentoId, long? contaId = null)
        {
            try
            {
                var dados = new GuiaSpsadtModel();
                //var conta = await _contaMedicaAppService.ObterReportModel((long)input.ContaId);
                //var atendimento = await _atendimentoAppService.Obter((long)input.AtendimentoId);

                using (var _contaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
                using (var contaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoConta, long>>())
                using (var _atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    var atendimento = await _atendimentoAppService.Object.Obter((long)atendimentoId);

                    FaturamentoConta conta = null;
                    if (contaId == null)
                    {
                        conta = contaRepository.Object.GetAll()
                                               .Include(i => i.ContaItens)
                                               .Include(i => i.ContaItens.Select(s => s.FaturamentoItem))
                                               .Include(i => i.ContaItens.Select(s => s.FaturamentoItem.Grupo))
                                               //.Include(i => i.ContaItens.Select(s => s.Medico))
                                               .Include(i => i.ContaItens.Select(s => s.MedicoEspecialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Auxiliar1))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar1Especialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Auxiliar2))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar2Especialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Auxiliar3))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar3Especialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Instrumentador))
                                               .Include(i => i.ContaItens.Select(s => s.InstrumentadorEspecialidade))
                                               //.Include(i => i.ContaItens.Select(s => s.Anestesista))
                                               .Include(i => i.ContaItens.Select(s => s.AnestesistaEspecialidade))
                                               .Include(i => i.ContaItens.Select(s => s.FaturamentoConfigConvenio))
                                               .Where(w => w.AtendimentoId == atendimentoId)
                                               .OrderBy(o => o.DataInicio)
                                               .FirstOrDefault();
                    }
                    else
                    {
                        conta = contaRepository.Object.GetAll()
                                           .Include(i => i.ContaItens)
                                           .Include(i => i.ContaItens.Select(s => s.FaturamentoItem))
                                           .Include(i => i.ContaItens.Select(s => s.FaturamentoItem.Grupo))
                                               //.Include(i => i.ContaItens.Select(s => s.Medico))
                                               .Include(i => i.ContaItens.Select(s => s.MedicoEspecialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Auxiliar1))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar1Especialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Auxiliar2))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar2Especialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Auxiliar3))
                                               .Include(i => i.ContaItens.Select(s => s.Auxiliar3Especialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Instrumentador))
                                               .Include(i => i.ContaItens.Select(s => s.InstrumentadorEspecialidade))
                                               //   .Include(i => i.ContaItens.Select(s => s.Anestesista))
                                               .Include(i => i.ContaItens.Select(s => s.AnestesistaEspecialidade))
                                               .Include(i => i.ContaItens.Select(s => s.FaturamentoConfigConvenio))
                                           .Where(w => w.Id == contaId)
                                           .FirstOrDefault();
                    }

                    contaDto = FaturamentoContaDto.Mapear(conta);

                    contaDto.Itens = new List<FaturamentoContaItemDto>();

                    foreach (var item in conta.ContaItens)
                    {
                        contaDto.Itens.Add(FaturamentoContaItemDto.MapearFromCore(item));
                    }

                    dados.LerAtendimento(atendimento, contaDto.Itens);

                    // Itens da conta
                    ListarFaturamentoContaItensInput listarItensInput = new ListarFaturamentoContaItensInput();
                    listarItensInput.Filtro = conta.Id.ToString();



                    var contaItensPaged = await _contaItemAppService.Object.ListarPorConta(listarItensInput);
                    var contaItens = contaItensPaged.Items as List<FaturamentoContaItemDto>;

                    // Separando itens de acordo com grupo.IsOutraDespesa (guia principal ou 'outras despesas')
                    // Spsadt
                    List<FaturamentoContaItemDto> itensGuiaPrincipal = new List<FaturamentoContaItemDto>();
                    List<FaturamentoContaItemDto> itensGuiaPrincipalPrimeiraPagina = new List<FaturamentoContaItemDto>();
                    List<List<FaturamentoContaItemDto>> listasGuiaComplementar = new List<List<FaturamentoContaItemDto>>();
                    // Outras Despesas
                    List<FaturamentoContaItemDto> itensGuiaOutrasDespesas = new List<FaturamentoContaItemDto>();
                    List<List<FaturamentoContaItemDto>> listasGuiaOutrasDespesas = new List<List<FaturamentoContaItemDto>>();

                    itensGuiaPrincipal = contaItens.Where(x => (x.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId == null && string.IsNullOrEmpty(x.FaturamentoItem.Grupo.CodTipoOutraDespesa))).ToList();
                    itensGuiaOutrasDespesas = contaItens.Where(x => (x.FaturamentoItem.Grupo.FaturamentoCodigoDespesaId != null || !string.IsNullOrEmpty(x.FaturamentoItem.Grupo.CodTipoOutraDespesa))).ToList();

                    _itensGuiaPrincipal = itensGuiaPrincipal;


                    //foreach (var item in contaItens)
                    //{
                    //    if (!item.IsValorItemManual)
                    //    {
                    //        item.ValorItem = await _contaItemAppService.CalcularValorUnitarioItem(conta.EmpresaId ?? 0, conta.ConvenioId ?? 0, conta.PlanoId ?? 0, item);
                    //    }
                    //}




                    int totalItensGuiaPrincipal = itensGuiaPrincipal.Count;
                    bool gerarGuiaComplementar = totalItensGuiaPrincipal > 5;

                    // Verificando necessidade de guias complementares, gerando subListas de itens se necessario
                    if (gerarGuiaComplementar)
                    {
                        itensGuiaPrincipalPrimeiraPagina = itensGuiaPrincipal.GetRange(0, 5);
                        var lista = itensGuiaPrincipal.GetRange(5, totalItensGuiaPrincipal - 5);
                        // Cada guia complementar suporta ate 7 itens
                        var tamanho = 10;

                        // Gerando subListas de ate 7 itens
                        for (int i = 0; i < lista.Count; i += tamanho)
                        {
                            listasGuiaComplementar.Add(lista.GetRange(i, Math.Min(tamanho, lista.Count - i)));
                        }

                        _listasGuiaComplementar = itensGuiaPrincipal.GetRange(5, totalItensGuiaPrincipal - 5);
                    }
                    else
                    {
                        itensGuiaPrincipalPrimeiraPagina = itensGuiaPrincipal.GetRange(0, totalItensGuiaPrincipal);

                        // Complementar com itens vazios para ocupar espaco certo no relatorio
                        int count = itensGuiaPrincipalPrimeiraPagina.Count;

                        if (count < 5)
                        {
                            int diferenca = 5 - count;

                            for (int i = diferenca + 1; i < 5; i++)
                            {
                                var novoItem = new FaturamentoContaItemDto();
                                novoItem.FaturamentoItem = new FaturamentoItemDto();



                                itensGuiaPrincipalPrimeiraPagina.Add(novoItem);
                            }
                        }
                    }






                    // Guia principal
                    //Web.Relatorios.Faturamento.Guias.InternacaoResumo.resumo_internacao_dataset resumo_internacao_dataset = new Web.Relatorios.Faturamento.Guias.InternacaoResumo.resumo_internacao_dataset();
                    //DataTable tabela = this.ConvertToDataTable(dados.Lista, resumo_internacao_dataset.Tables["resumo_internacao_table"]);
                    //DataRow row = tabela.NewRow();
                    //row["Logotipo"] = atendimento.Convenio.Logotipo; //atendimento.Empresa.Logotipo;
                    ////   tabela.Rows[tabela.Rows.Count - 1].Delete();
                    //tabela.Rows.Add(row);

                    //ReportDataSource dataSource = new ReportDataSource("resumo_internacao_dataset", tabela);
                    //ReportViewer reportViewer = new ReportViewer();
                    //reportViewer.LocalReport.DataSources.Add(dataSource);






                    // Guia principal Spsadt
                    Guias relDS = new Guias();
                    DataTable tabela = this.ConvertToDataTable(dados.Contas, relDS.Tables["Spsadt"]);//precisa?

                    DataRow row = tabela.NewRow();
                    row["Logotipo"] = atendimento.Convenio.Logotipo; //atendimento.Empresa.Logotipo;
                    tabela.Rows.Add(row);
                    ReportDataSource dataSource = new ReportDataSource("Spsadt", tabela);
                    ReportViewer GuiaSpsadt = new ReportViewer();
                    GuiaSpsadt.LocalReport.DataSources.Add(dataSource);
                    ScriptManager scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(GuiaSpsadt);
                    GuiaSpsadt.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\Spsadt\guia_spsadt.rdlc");
                    SetParametrosSpsadt(GuiaSpsadt, dados);

                    // Sub Relatorios, passando parametros
                    _itensGuiaPrincipal = itensGuiaPrincipal;
                    _itensGuiaPrincipalPrimeiraPagina = itensGuiaPrincipalPrimeiraPagina;

                    // "Link" para carregamweento dos dados dos subRelatorios no relatorio principal
                    GuiaSpsadt.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingSpsadt);
                    GuiaSpsadt.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingSpsadtExames);
                    GuiaSpsadt.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingSpsadtEquipe);
                    //  GuiaSpsadt.LocalReport.SubreportProcessing += LocalReport_SubreportProcessingSpsadtExamesContinuacao;

                    // Pdf (byte array)
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = "pdf";
                    byte[] pdfBytes = GuiaSpsadt.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    GuiaSpsadt.LocalReport.Refresh();
                    // Fim - Guia Principal

                    // Spsadt: paginas complementares
                    List<byte[]> pdfBytesComplementares = new List<byte[]>();


                    #region outras despesas antigos - Sudano

                    ////

                    //// Um relatorio extra para cada subLista de itens
                    //foreach (var lista in listasGuiaComplementar)
                    //{
                    //    Guias DS = new Guias();
                    //    List<GuiaSpsadtDespesaItem> despesaItens = new List<GuiaSpsadtDespesaItem>();

                    //    foreach (var item in lista)
                    //    {
                    //        var despesaItem = new GuiaSpsadtDespesaItem();
                    //        despesaItem.Descricao = item.Descricao + item.FaturamentoItem.Descricao;
                    //        despesaItem.CodigoItem = item.FaturamentoItem.CodTuss;
                    //        despesaItem.Qtde = item.Qtde.ToString();
                    //        despesaItem.Tabela = item.FaturamentoConfigConvenioDto?.Codigo;
                    //        despesaItens.Add(despesaItem);
                    //    }

                    //    DataTable tabelaComplemetar = this.ConvertToDataTable(despesaItens, DS.Tables["SpsadtDespesasItens"]);
                    //    ReportDataSource dataSourceComplementar = new ReportDataSource("SpsadtDespesasItens", tabelaComplemetar);
                    //    ReportViewer GuiaComplementar = new ReportViewer();
                    //    GuiaComplementar.LocalReport.DataSources.Add(dataSourceComplementar);
                    //    ScriptManager scriptManagerComplementar = new ScriptManager();
                    //    scriptManagerComplementar.RegisterPostBackControl(GuiaComplementar);
                    //    GuiaComplementar.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\Spsadt\guia_spsadt_despesas.rdlc");

                    //    // PDFs de paginas complementares
                    //    Warning[] warningsComplementar;
                    //    string[] streamIdsComplementar;
                    //    string mimeTypeComplementar = string.Empty;
                    //    string encodingComplementar = string.Empty;
                    //    string extensionComplementar = "pdf";
                    //    byte[] pdfBytesComplementar = GuiaComplementar.LocalReport.Render("PDF", null, out mimeTypeComplementar, out encodingComplementar, out extensionComplementar, out streamIdsComplementar, out warningsComplementar);
                    //    pdfBytesComplementares.Add(pdfBytesComplementar);
                    //}
                    //// FIM - Guias complementares

                    //// Guias 'Outras Despesas'
                    //// Gerando sublistas (uma para cada pagina)
                    //var tamanhoOutrasDespesas = 7;

                    //for (int i = 0; i < itensGuiaOutrasDespesas.Count; i += tamanhoOutrasDespesas)
                    //{
                    //    listasGuiaOutrasDespesas.Add(itensGuiaOutrasDespesas.GetRange(i, Math.Min(tamanhoOutrasDespesas, itensGuiaOutrasDespesas.Count - i)));
                    //}

                    //List<byte[]> pdfBytesOutrasDespesas = new List<byte[]>();

                    //// Um relatorio extra para cada subLista de itens
                    //foreach (var lista in listasGuiaOutrasDespesas)
                    //{
                    //    Guias DS = new Guias();
                    //    List<GuiaSpsadtDespesaItem> despesaItens = new List<GuiaSpsadtDespesaItem>();

                    //    foreach (var item in lista)
                    //    {
                    //        var despesaItem = new GuiaSpsadtDespesaItem();
                    //        despesaItem.Descricao = item.Descricao + item.FaturamentoItem.Descricao;

                    //        despesaItem.CodigoItem = item.FaturamentoItem.CodTuss;
                    //        despesaItem.Qtde = item.Qtde.ToString();
                    //        despesaItem.Data = string.Format("{0:dd/MM/yyyy}", item.Data);
                    //        despesaItem.HoraInicial = string.Format("{0:HH:mm}", item.HoraIncio);
                    //        despesaItem.HoraFinal = string.Format("{0:HH:mm}", item.HoraFim);
                    //        despesaItem.ValorUnitario = string.Format("{0:#,##0.00}", item.ValorItem);
                    //        despesaItem.ValorTotal = string.Format("{0:#,##0.00}", (item.ValorItem * item.Qtde));
                    //        despesaItem.Tabela = item.FaturamentoConfigConvenioDto?.Codigo;

                    //        despesaItens.Add(despesaItem);
                    //    }

                    //    DataTable tabelaComplemetar = this.ConvertToDataTable(despesaItens, DS.Tables["SpsadtDespesasItens"]);
                    //    ReportDataSource dataSourceComplementar = new ReportDataSource("SpsadtDespesasItens", tabelaComplemetar);
                    //    ReportViewer GuiaComplementar = new ReportViewer();
                    //    GuiaComplementar.LocalReport.DataSources.Add(dataSourceComplementar);
                    //    ScriptManager scriptManagerComplementar = new ScriptManager();
                    //    scriptManagerComplementar.RegisterPostBackControl(GuiaComplementar);
                    //    GuiaComplementar.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\Spsadt\guia_spsadt_despesas.rdlc");

                    //    //    // PDF 'Outras Despesas'
                    //    Warning[] warningsComplementar;
                    //    string[] streamIdsComplementar;
                    //    string mimeTypeComplementar = string.Empty;
                    //    string encodingComplementar = string.Empty;
                    //    string extensionComplementar = "pdf";
                    //    byte[] pdfBytesComplementar = GuiaComplementar.LocalReport.Render("PDF", null, out mimeTypeComplementar, out encodingComplementar, out extensionComplementar, out streamIdsComplementar, out warningsComplementar);
                    //    pdfBytesComplementares.Add(pdfBytesComplementar);
                    //}

                    #endregion

                    // FIM - Guias 'Outras Despesas'



                    var dados2 = GuiaResumoInternacaoModel.MapearFromAtendimento(atendimento, contaDto);


                    var arquivosPdfItensContinuacao = GerarSpsadtContinuacao(contaDto.Itens, dados2, atendimento);
                    pdfBytesComplementares.AddRange(arquivosPdfItensContinuacao);

                    var arquivosPdfOutrasDespesas = GerarOutrasDespesas(contaDto.Itens, dados2, atendimento);

                    pdfBytesComplementares.AddRange(arquivosPdfOutrasDespesas);


                    // Anexando todos os relatorios
                    GuiaSpsadt.LocalReport.Refresh();
                    MemoryStream guiaPrincipalStream = new MemoryStream(pdfBytes); // guia principal
                    PdfSharp.Pdf.PdfDocument guiaPrincipalPdf = PdfSharp.Pdf.IO.PdfReader.Open(guiaPrincipalStream, PdfDocumentOpenMode.Import);
                    PdfSharp.Pdf.PdfDocument pdfDefinitivo = new PdfSharp.Pdf.PdfDocument();

                    // Gerando paginas da guia principal
                    int principalPageCount = guiaPrincipalPdf.PageCount;
                    for (int i = 0; i < principalPageCount; i++)
                    {
                        PdfSharp.Pdf.PdfPage page = guiaPrincipalPdf.Pages[i];
                        page = pdfDefinitivo.AddPage(page);
                    }

                    // Guias complementares
                    foreach (var pdfBs in pdfBytesComplementares)
                    {
                        MemoryStream guiaComplementarStream = new MemoryStream(pdfBs); // guia outras despesas
                        PdfSharp.Pdf.PdfDocument guiaComplementarPdf = PdfSharp.Pdf.IO.PdfReader.Open(guiaComplementarStream, PdfDocumentOpenMode.Import);
                        int compPageCount = guiaComplementarPdf.PageCount;
                        for (int i = 0; i < compPageCount; i++)
                        {
                            PdfSharp.Pdf.PdfPage page = guiaComplementarPdf.Pages[i];
                            page = pdfDefinitivo.AddPage(page);
                        }
                    }

                    // Gerando pdf byte array, relatorio definitivo
                    byte[] definitivaBytes = null;

                    using (MemoryStream stream = new MemoryStream())
                    {
                        pdfDefinitivo.Save(stream, true);
                        definitivaBytes = stream.ToArray();
                    }

                    // Response.Headers.Add("Content-Disposition", "inline; filename=guia_spsadt.pdf");

                    //return File(definitivaBytes, "application/pdf");


                    var nomeArquivo = string.Concat(Guid.NewGuid().ToString(), ".pdf");

                    var path = string.Concat(Server.MapPath("/"), @"temp\", nomeArquivo);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    FileStream file = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                    //  file.Write(definitivaBytes, 0, definitivaBytes.Length);
                    file.Write(definitivaBytes, 0, definitivaBytes.Length);

                    file.Close();

                    // reportViewer.LocalReport.Refresh();


                    Response.Headers.Add("Content-Disposition", string.Concat("inline; filename=", nomeArquivo));
                    //return File(pdfBytes, "application/pdf");
                    //return File(novoPdf, "application/pdf");

                    var pathRetorno = string.Concat(@"/temp/", nomeArquivo);

                    return Content(pathRetorno);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return null;
        }

        private void LocalReport_SubreportProcessingSpsadtExamesContinuacao(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                var dados = new GuiaSpsadtModel();
                dados.Contas = new List<ContaMedicaReportModel>();
                var x = new ContaMedicaReportModel();
                x.AtendimentoCodigo = "1234";
                dados.Contas.Add(x);
                Guias relDS = new Guias();

                // Exames
                List<GuiaSpsadtExame> exames = new List<GuiaSpsadtExame>();

                // Itens solicitados
                List<GuiaSpsadtItemSolic> itensSolics = new List<GuiaSpsadtItemSolic>();

                foreach (var item in _listasGuiaComplementar)
                {
                    var valorTotal = (item.ValorItem * item.Qtde);

                    var ex1 = new GuiaSpsadtExame();
                    ex1.CodigoProcedimento = item.FaturamentoItem.CodTuss;
                    ex1.Descricao = item.Descricao + " " + item.FaturamentoItem.Descricao;
                    ex1.Tabela = item.TabelaUtilizada;
                    ex1.Data = item.Data?.ToString("dd/MM/yy");
                    ex1.HoraInicial = string.Format("{0:HH:mm}", item.HoraIncio);// item.HoraIncio?.ToString("mm:ss");
                    ex1.HoraFinal = string.Format("{0:HH:mm}", item.HoraFim);// item.HoraFim?.ToString("mm:ss tt");
                    ex1.Qtde = item.Qtde > 0 ? item.Qtde.ToString() : "";
                    ex1.ValorUnitario = item.ValorItem > 0 ? string.Format("{0:#,##0.00}", item.ValorItem) : "";
                    ex1.ValorTotal = valorTotal > 0 ? string.Format("{0:#,##0.00}", valorTotal) : "";
                    ex1.Tabela = item.FaturamentoConfigConvenioDto?.Codigo;
                    exames.Add(ex1);
                }

                DataTable tabelaExames = this.ConvertToDataTable(exames, relDS.Tables["SpsadtExames"]);
                ReportDataSource dataSourceExames = new ReportDataSource("procedimentos_continuacao", tabelaExames);
                e.DataSources.Add(dataSourceExames);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void SetParametrosSpsadt(ReportViewer rv, GuiaSpsadtModel dados)
        {
            ReportParameter NumeroGuiaPrestador = new ReportParameter("NumeroGuiaPrestador", string.IsNullOrEmpty(dados.NumeroGuiaPrestador) ? " " : dados.NumeroGuiaPrestador);
            ReportParameter RegistroAns = new ReportParameter("RegistroAns", string.IsNullOrEmpty(dados.RegistroAns) ? " " : dados.RegistroAns);
            ReportParameter NumeroGuiaPrincipal = new ReportParameter("NumeroGuiaPrincipal", string.IsNullOrEmpty(dados.NumeroGuiaPrincipal) ? " " : dados.NumeroGuiaPrincipal);
            ReportParameter DataAutorizacao = new ReportParameter("DataAutorizacao", string.IsNullOrEmpty(dados.DataAutorizacao) ? " " : dados.DataAutorizacao);
            ReportParameter Senha = new ReportParameter("Senha", string.IsNullOrEmpty(dados.Senha) ? " " : dados.Senha);
            ReportParameter DataValidadeSenha = new ReportParameter("DataValidadeSenha", string.IsNullOrEmpty(dados.DataValidadeSenha) ? " " : dados.DataValidadeSenha);
            ReportParameter NumeroGuiaOperadora = new ReportParameter("NumeroGuiaOperadora", string.IsNullOrEmpty(dados.NumeroGuiaOperadora) ? " " : dados.NumeroGuiaOperadora);
            ReportParameter NumeroCarteira = new ReportParameter("NumeroCarteira", string.IsNullOrEmpty(dados.NumeroCarteira) ? " " : dados.NumeroCarteira);
            ReportParameter ValidadeCarteira = new ReportParameter("ValidadeCarteira", string.IsNullOrEmpty(dados.ValidadeCarteira) ? " " : dados.ValidadeCarteira);
            ReportParameter NomePaciente = new ReportParameter("NomePaciente", string.IsNullOrEmpty(dados.NomePaciente) ? " " : dados.NomePaciente);
            ReportParameter CartaoNacionalSaude = new ReportParameter("CartaoNacionalSaude", string.IsNullOrEmpty(dados.NumeroCns) ? " " : dados.NumeroCns);
            ReportParameter AtendimentoRn = new ReportParameter("AtendimentoRn", string.IsNullOrEmpty(dados.AtendimentoRn) ? " " : dados.AtendimentoRn);
            ReportParameter CodigoOperadora = new ReportParameter("CodigoOperadora", string.IsNullOrEmpty(dados.CodigoOperadora) ? " " : dados.CodigoOperadora);
            ReportParameter NomeContratado = new ReportParameter("NomeContratado", string.IsNullOrEmpty(dados.NomeContratado) ? " " : dados.NomeContratado);
            ReportParameter NomeProfissionalSolicitante = new ReportParameter("NomeProfissionalSolicitante", string.IsNullOrEmpty(dados.NomeProfissionalSolicitante) ? " " : dados.NomeProfissionalSolicitante);
            ReportParameter ConselhoProfissional = new ReportParameter("ConselhoProfissional", string.IsNullOrEmpty(dados.ConselhoProfissional) ? " " : dados.ConselhoProfissional);
            ReportParameter NumeroConselho = new ReportParameter("NumeroConselho", string.IsNullOrEmpty(dados.NumeroConselho) ? " " : dados.NumeroConselho);
            ReportParameter UF = new ReportParameter("UF", string.IsNullOrEmpty(dados.UF) ? " " : dados.UF);
            ReportParameter CodigoCbo = new ReportParameter("CodigoCbo", string.IsNullOrEmpty(dados.CodigoCbo) ? " " : dados.CodigoCbo);
            ReportParameter AssinaturaProfissionalSolicitante = new ReportParameter("AssinaturaProfissionalSolicitante", string.IsNullOrEmpty(dados.AssinaturaProfissionalSolicitante) ? " " : dados.AssinaturaProfissionalSolicitante);
            ReportParameter CaraterAtendimento = new ReportParameter("CaraterAtendimento", string.IsNullOrEmpty(dados.CaraterAtendimento) ? " " : dados.CaraterAtendimento);
            ReportParameter DataSolicitacao = new ReportParameter("DataSolicitacao", string.IsNullOrEmpty(dados.DataSolicitacao) ? " " : dados.DataSolicitacao);
            ReportParameter IndicacaoClinica = new ReportParameter("IndicacaoClinica", string.IsNullOrEmpty(dados.IndicacaoClinica) ? " " : dados.IndicacaoClinica);
            ReportParameter CodigoCne = new ReportParameter("CodigoCne", string.IsNullOrEmpty(dados.CodigoCne) ? " " : dados.CodigoCne);
            ReportParameter TipoAtendimento = new ReportParameter("TipoAtendimento", string.IsNullOrEmpty(dados.TipoAtendimento) ? " " : dados.TipoAtendimento);
            ReportParameter IndicacaoAcidente = new ReportParameter("IndicacaoAcidente", string.IsNullOrEmpty(dados.IndicacaoAcidente) ? " " : dados.IndicacaoAcidente);
            ReportParameter TipoConsulta = new ReportParameter("TipoConsulta", string.IsNullOrEmpty(dados.TipoConsulta) ? " " : dados.TipoConsulta);
            ReportParameter MotivoEncerramentoAtendimento = new ReportParameter("MotivoEncerramentoAtendimento", string.IsNullOrEmpty(dados.MotivoEncerramentoAtendimento) ? " " : dados.MotivoEncerramentoAtendimento);
            // Identificacao Equipe
            ReportParameter SequenciaRef1 = new ReportParameter("SequenciaRef1", string.IsNullOrEmpty(dados.SequenciaRef1) ? " " : dados.SequenciaRef1);
            ReportParameter SequenciaRef2 = new ReportParameter("SequenciaRef2", string.IsNullOrEmpty(dados.SequenciaRef2) ? " " : dados.SequenciaRef2);
            ReportParameter SequenciaRef3 = new ReportParameter("SequenciaRef3", string.IsNullOrEmpty(dados.SequenciaRef3) ? " " : dados.SequenciaRef3);
            ReportParameter SequenciaRef4 = new ReportParameter("SequenciaRef4", string.IsNullOrEmpty(dados.SequenciaRef4) ? " " : dados.SequenciaRef4);
            ReportParameter GrauPart1 = new ReportParameter("GrauPart1", string.IsNullOrEmpty(dados.GrauPart1) ? " " : dados.GrauPart1);
            ReportParameter GrauPart2 = new ReportParameter("GrauPart2", string.IsNullOrEmpty(dados.GrauPart2) ? " " : dados.GrauPart2);
            ReportParameter GrauPart3 = new ReportParameter("GrauPart3", string.IsNullOrEmpty(dados.GrauPart3) ? " " : dados.GrauPart3);
            ReportParameter GrauPart4 = new ReportParameter("GrauPart4", string.IsNullOrEmpty(dados.GrauPart4) ? " " : dados.GrauPart4);
            ReportParameter CodigoOperadoraCpf1 = new ReportParameter("CodigoOperadoraCpf1", string.IsNullOrEmpty(dados.CodigoOperadoraCpf1) ? " " : dados.CodigoOperadoraCpf1);
            ReportParameter CodigoOperadoraCpf2 = new ReportParameter("CodigoOperadoraCpf2", string.IsNullOrEmpty(dados.CodigoOperadoraCpf2) ? " " : dados.CodigoOperadoraCpf2);
            ReportParameter CodigoOperadoraCpf3 = new ReportParameter("CodigoOperadoraCpf3", string.IsNullOrEmpty(dados.CodigoOperadoraCpf3) ? " " : dados.CodigoOperadoraCpf3);
            ReportParameter CodigoOperadoraCpf4 = new ReportParameter("CodigoOperadoraCpf4", string.IsNullOrEmpty(dados.CodigoOperadoraCpf4) ? " " : dados.CodigoOperadoraCpf4);
            ReportParameter NomeProfissional1 = new ReportParameter("NomeProfissional1", string.IsNullOrEmpty(dados.NomeProfissional1) ? " " : dados.NomeProfissional1);
            ReportParameter NomeProfissional2 = new ReportParameter("NomeProfissional2", string.IsNullOrEmpty(dados.NomeProfissional2) ? " " : dados.NomeProfissional2);
            ReportParameter NomeProfissional3 = new ReportParameter("NomeProfissional3", string.IsNullOrEmpty(dados.NomeProfissional3) ? " " : dados.NomeProfissional3);
            ReportParameter NomeProfissional4 = new ReportParameter("NomeProfissional4", string.IsNullOrEmpty(dados.NomeProfissional4) ? " " : dados.NomeProfissional4);
            ReportParameter ConselhoProfissional1 = new ReportParameter("ConselhoProfissional1", string.IsNullOrEmpty(dados.ConselhoProfissional1) ? " " : dados.ConselhoProfissional1);
            ReportParameter ConselhoProfissional2 = new ReportParameter("ConselhoProfissional2", string.IsNullOrEmpty(dados.ConselhoProfissional2) ? " " : dados.ConselhoProfissional2);
            ReportParameter ConselhoProfissional3 = new ReportParameter("ConselhoProfissional3", string.IsNullOrEmpty(dados.ConselhoProfissional3) ? " " : dados.ConselhoProfissional3);
            ReportParameter ConselhoProfissional4 = new ReportParameter("ConselhoProfissional4", string.IsNullOrEmpty(dados.ConselhoProfissional4) ? " " : dados.ConselhoProfissional4);
            ReportParameter NumeroConselho1 = new ReportParameter("NumeroConselho1", string.IsNullOrEmpty(dados.NumeroConselho1) ? " " : dados.NumeroConselho1);
            ReportParameter NumeroConselho2 = new ReportParameter("NumeroConselho2", string.IsNullOrEmpty(dados.NumeroConselho2) ? " " : dados.NumeroConselho2);
            ReportParameter NumeroConselho3 = new ReportParameter("NumeroConselho3", string.IsNullOrEmpty(dados.NumeroConselho3) ? " " : dados.NumeroConselho3);
            ReportParameter NumeroConselho4 = new ReportParameter("NumeroConselho4", string.IsNullOrEmpty(dados.NumeroConselho4) ? " " : dados.NumeroConselho4);
            ReportParameter Uf1 = new ReportParameter("Uf1", string.IsNullOrEmpty(dados.Uf1) ? " " : dados.Uf1);
            ReportParameter Uf2 = new ReportParameter("Uf2", string.IsNullOrEmpty(dados.Uf2) ? " " : dados.Uf2);
            ReportParameter Uf3 = new ReportParameter("Uf3", string.IsNullOrEmpty(dados.Uf3) ? " " : dados.Uf3);
            ReportParameter Uf4 = new ReportParameter("Uf4", string.IsNullOrEmpty(dados.Uf4) ? " " : dados.Uf4);
            ReportParameter CodigoCbo1 = new ReportParameter("CodigoCbo1", string.IsNullOrEmpty(dados.CodigoCbo1) ? " " : dados.CodigoCbo1);
            ReportParameter CodigoCbo2 = new ReportParameter("CodigoCbo2", string.IsNullOrEmpty(dados.CodigoCbo2) ? " " : dados.CodigoCbo2);
            ReportParameter CodigoCbo3 = new ReportParameter("CodigoCbo3", string.IsNullOrEmpty(dados.CodigoCbo3) ? " " : dados.CodigoCbo3);
            ReportParameter CodigoCbo4 = new ReportParameter("CodigoCbo4", string.IsNullOrEmpty(dados.CodigoCbo4) ? " " : dados.CodigoCbo4);
            // Datas e Assinaturas (procedimentos em serie)
            ReportParameter DataRealizacaoProcedimentoSerie1 = new ReportParameter("DataRealizacaoProcedimentoSerie1", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie1) ? " " : dados.DataRealizacaoProcedimentoSerie1);
            ReportParameter DataRealizacaoProcedimentoSerie2 = new ReportParameter("DataRealizacaoProcedimentoSerie2", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie2) ? " " : dados.DataRealizacaoProcedimentoSerie2);
            ReportParameter DataRealizacaoProcedimentoSerie3 = new ReportParameter("DataRealizacaoProcedimentoSerie3", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie3) ? " " : dados.DataRealizacaoProcedimentoSerie3);
            ReportParameter DataRealizacaoProcedimentoSerie4 = new ReportParameter("DataRealizacaoProcedimentoSerie4", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie4) ? " " : dados.DataRealizacaoProcedimentoSerie4);
            ReportParameter DataRealizacaoProcedimentoSerie5 = new ReportParameter("DataRealizacaoProcedimentoSerie5", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie5) ? " " : dados.DataRealizacaoProcedimentoSerie5);
            ReportParameter DataRealizacaoProcedimentoSerie6 = new ReportParameter("DataRealizacaoProcedimentoSerie6", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie6) ? " " : dados.DataRealizacaoProcedimentoSerie6);
            ReportParameter DataRealizacaoProcedimentoSerie7 = new ReportParameter("DataRealizacaoProcedimentoSerie7", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie7) ? " " : dados.DataRealizacaoProcedimentoSerie7);
            ReportParameter DataRealizacaoProcedimentoSerie8 = new ReportParameter("DataRealizacaoProcedimentoSerie8", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie8) ? " " : dados.DataRealizacaoProcedimentoSerie8);
            ReportParameter DataRealizacaoProcedimentoSerie9 = new ReportParameter("DataRealizacaoProcedimentoSerie9", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie9) ? " " : dados.DataRealizacaoProcedimentoSerie9);
            ReportParameter DataRealizacaoProcedimentoSerie10 = new ReportParameter("DataRealizacaoProcedimentoSerie10", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie10) ? " " : dados.DataRealizacaoProcedimentoSerie10);
            ReportParameter ObservacaoJustificativa = new ReportParameter("ObservacaoJustificativa", string.IsNullOrEmpty(dados.ObservacaoJustificativa) ? " " : dados.ObservacaoJustificativa);
            ReportParameter TotalProcedimentos = new ReportParameter("TotalProcedimentos", string.IsNullOrEmpty(dados.TotalProcedimentos) ? " " : dados.TotalProcedimentos);
            ReportParameter TotalTaxasAlugueis = new ReportParameter("TotalTaxasAlugueis", string.IsNullOrEmpty(dados.TotalTaxasAlugueis) ? " " : dados.TotalTaxasAlugueis);
            ReportParameter TotalMateriais = new ReportParameter("TotalMateriais", string.IsNullOrEmpty(dados.TotalMateriais) ? " " : dados.TotalMateriais);
            ReportParameter TotalOpme = new ReportParameter("TotalOpme", string.IsNullOrEmpty(dados.TotalOpme) ? " " : dados.TotalOpme);
            ReportParameter TotalMedicamentos = new ReportParameter("TotalMedicamentos", string.IsNullOrEmpty(dados.TotalMedicamentos) ? " " : dados.TotalMedicamentos);
            ReportParameter TotalGeral = new ReportParameter("TotalGeral", string.IsNullOrEmpty(dados.TotalGeral) ? " " : dados.TotalGeral);
            ReportParameter TotalGasesMedicinais = new ReportParameter("TotalGasesMedicinais", string.IsNullOrEmpty(dados.TotalGasesMedicinais) ? " " : dados.TotalGasesMedicinais);
            ReportParameter RN = new ReportParameter("RN", dados.RN ? "S" : "N");
            ReportParameter CodigoCnes = new ReportParameter("CodigoCnes", dados.CNES);

            rv.LocalReport.SetParameters(new ReportParameter[] {
                          //   Titulo ,
                             NumeroGuiaPrestador ,
                             // Guia
                             RegistroAns ,
                             NumeroGuiaPrincipal ,
                             DataAutorizacao ,
                             Senha ,
                             DataValidadeSenha,
                             NumeroGuiaOperadora ,
                             NumeroCarteira ,
                             ValidadeCarteira ,
                             NomePaciente ,
                             CartaoNacionalSaude ,
                           //  AtendimentoRn ,
                             CodigoOperadora ,
                             NomeContratado ,
                             NomeProfissionalSolicitante ,
                             ConselhoProfissional ,
                             NumeroConselho ,
                             UF ,
                             CodigoCbo ,
                            // AtendimentoRn,
                         //    AssinaturaProfissionalSolicitante ,
                             CaraterAtendimento ,
                             DataSolicitacao ,
                             IndicacaoClinica ,
                        //     CodigoCne ,
                             TipoAtendimento ,
                             IndicacaoAcidente ,
                             TipoConsulta ,
                             MotivoEncerramentoAtendimento ,
                             // Identificacao Equipe
                             //SequenciaRef1 ,
                             //SequenciaRef2 ,
                             //SequenciaRef3 ,
                             //SequenciaRef4 ,
                             //GrauPart1 ,
                             //GrauPart2 ,
                             //GrauPart3 ,
                             //GrauPart4 ,
                             //CodigoOperadoraCpf1 ,
                             //CodigoOperadoraCpf2 ,
                             //CodigoOperadoraCpf3 ,
                             //CodigoOperadoraCpf4 ,
                             //NomeProfissional1 ,
                             //NomeProfissional2 ,
                             //NomeProfissional3 ,
                             //NomeProfissional4 ,
                             //ConselhoProfissional1 ,
                             //ConselhoProfissional2 ,
                             //ConselhoProfissional3 ,
                             //ConselhoProfissional4 ,
                             //NumeroConselho1 ,
                             //NumeroConselho2 ,
                             //NumeroConselho3 ,
                             //NumeroConselho4 ,
                             //Uf1 ,
                             //Uf2 ,
                             //Uf3 ,
                             //Uf4 ,
                             //CodigoCbo1 ,
                             //CodigoCbo2 ,
                             //CodigoCbo3 ,
                             //CodigoCbo4 ,
                             //// Datas e Assinaturas (procedimentos em serie)
                             //DataRealizacaoProcedimentoSerie1 ,
                             //DataRealizacaoProcedimentoSerie2 ,
                             //DataRealizacaoProcedimentoSerie3 ,
                             //DataRealizacaoProcedimentoSerie4 ,
                             //DataRealizacaoProcedimentoSerie5 ,
                             //DataRealizacaoProcedimentoSerie6 ,
                             //DataRealizacaoProcedimentoSerie7 ,
                             //DataRealizacaoProcedimentoSerie8 ,
                             //DataRealizacaoProcedimentoSerie9 ,
                             //DataRealizacaoProcedimentoSerie10 ,
                             //ObservacaoJustificativa ,
                             TotalProcedimentos ,
                             TotalTaxasAlugueis ,
                             TotalMateriais ,
                             TotalOpme ,
                             TotalMedicamentos ,
                             TotalGeral,
                             TotalGasesMedicinais,
                             RN,
                             CodigoCnes
                        });
        }

        public void LocalReport_SubreportProcessingSpsadt(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                var dados = new GuiaSpsadtModel();
                dados.Contas = new List<ContaMedicaReportModel>();
                var x = new ContaMedicaReportModel();
                x.AtendimentoCodigo = "12313212";
                dados.Contas.Add(x);
                Guias relDS = new Guias();

                // Itens solicitados
                List<GuiaSpsadtItemSolic> itensSolics = new List<GuiaSpsadtItemSolic>();

                foreach (var item in _itensGuiaPrincipalPrimeiraPagina)
                {
                    var ex1 = new GuiaSpsadtItemSolic();
                    ex1.CodigoProcedimento = item.FaturamentoItem.CodTuss ?? "_________________";
                    ex1.Descricao = item.FaturamentoItem.DescricaoTuss ?? "___________________________________________________________________________________________________________";
                    ex1.QtAutoriz = item.Qtde > 0 ? item.Qtde.ToString() : "____";
                    ex1.QtSolic = item.Qtde > 0 ? item.Qtde.ToString() : "____";
                    ex1.Tabela = item.FaturamentoConfigConvenioDto?.Codigo ?? "_________";

                    itensSolics.Add(ex1);
                }

                DataTable tabelaItensSolic = this.ConvertToDataTable(itensSolics, relDS.Tables["SpsadtItensSolic"]);
                ReportDataSource dataSourceItensSolic = new ReportDataSource("GuiaSpsadtItensSolic", tabelaItensSolic);
                e.DataSources.Add(dataSourceItensSolic);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void LocalReport_SubreportProcessingSpsadtExames(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                var dados = new GuiaSpsadtModel();
                dados.Contas = new List<ContaMedicaReportModel>();
                var x = new ContaMedicaReportModel();
                x.AtendimentoCodigo = "1234";
                dados.Contas.Add(x);
                Guias relDS = new Guias();

                // Exames
                List<GuiaSpsadtExame> exames = new List<GuiaSpsadtExame>();

                // Itens solicitados
                List<GuiaSpsadtItemSolic> itensSolics = new List<GuiaSpsadtItemSolic>();

                foreach (var item in _itensGuiaPrincipalPrimeiraPagina)
                {
                    var valorTotal = (item.ValorItem * item.Qtde);

                    var ex1 = new GuiaSpsadtExame();
                    ex1.CodigoProcedimento = item.FaturamentoItem.CodTuss ?? "__________________";

                    var tamanho = !string.IsNullOrEmpty(item.FaturamentoItem.DescricaoTuss) ? (item.FaturamentoItem.DescricaoTuss.Length < 30 ? item.FaturamentoItem.DescricaoTuss.Length : 30) : 0;

                    ex1.Descricao = tamanho > 0 ? item.FaturamentoItem.DescricaoTuss.Substring(0, tamanho) : "_______________________________________";
                    ex1.Data = item.Data?.ToString("dd/MM/yy");
                    ex1.HoraInicial = item.HoraIncio != null ? string.Format("{0:HH:mm}", item.HoraIncio) : "_________";
                    ex1.HoraFinal = item.HoraFim != null ? string.Format("{0:HH:mm}", item.HoraFim) : "_________";
                    ex1.Qtde = item.Qtde > 0 ? item.Qtde.ToString() : "_______";
                    ex1.ValorUnitario = item.ValorItem > 0 ? string.Format("{0:#,##0.00}", item.ValorItem) : "______________";
                    ex1.ValorTotal = valorTotal > 0 ? string.Format("{0:#,##0.00}", valorTotal) : "_______________";
                    ex1.Tabela = item.FaturamentoConfigConvenioDto?.Codigo ?? "________";

                    ex1.Via = "____";
                    ex1.Tec = "____";
                    ex1.RedAcresc = "1";
                    exames.Add(ex1);
                }

                DataTable tabelaExames = this.ConvertToDataTable(exames, relDS.Tables["SpsadtExames"]);
                ReportDataSource dataSourceExames = new ReportDataSource("GuiaSpsadtExames", tabelaExames);
                e.DataSources.Add(dataSourceExames);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void LocalReport_SubreportProcessingSpsadtEquipe(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                using (var profissionalSaudeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Medico, long>>())
                using (var especialidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Especialidade, long>>())
                {
                    var dados = new GuiaSpsadtModel
                    {
                        Contas = new List<ContaMedicaReportModel>()
                    };
                    var x = new ContaMedicaReportModel
                    {
                        AtendimentoCodigo = "123"
                    };
                    dados.Contas.Add(x);
                    Guias relDS = new Guias();

                    // Equipe




                    if (_itensGuiaPrincipal != null)
                    {
                        var fatContaItens = _itensGuiaPrincipal;

                        medicosEspecialidades = new List<ProfissionalEspecialidade>();

                        // var qtdReal = fatItens.Count();

                        int i = 1;

                        foreach (var item in fatContaItens)
                        {
                            medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.MedicoId, EspecialidadeId = item.MedicoEspecialidade?.EspecialidadeId });
                            medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.Auxiliar1Id, EspecialidadeId = item.Auxiliar1Especialidade?.EspecialidadeId });
                            medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.Auxiliar2Id, EspecialidadeId = item.Auxiliar2Especialidade?.EspecialidadeId });
                            medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.Auxiliar3Id, EspecialidadeId = item.Auxiliar3Especialidade?.EspecialidadeId });
                            medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.InstrumentadorId, EspecialidadeId = item.InstrumentadorEspecialidade?.EspecialidadeId });
                            medicosEspecialidades.Add(new ProfissionalEspecialidade { ProfissionalId = item.AnestesistaId, EspecialidadeId = item.EspecialidadeAnestesista?.EspecialidadeId });
                        }

                        var dist = medicosEspecialidades.ToList();

                        var qtdReal = dist.Count();

                        int qtd = 0;

                        List<GuiaSpsadtEquipe> equipe = new List<GuiaSpsadtEquipe>();
                        foreach (var item in dist)
                        {
                            if (item.ProfissionalId != null)
                            {
                                qtd++;
                                var profissional = profissionalSaudeRepository.Object.GetAll()
                                                                              .Include(j => j.SisPessoa)
                                                                               .Include(j => j.SisPessoa.Enderecos)
                                                                               .Include(j => j.SisPessoa.Enderecos.Select(s => s.Estado))
                                                                              .Include(j => j.Estado)
                                                                              .Include(j => j.Conselho)
                                                                              .Where(w => w.Id == item.ProfissionalId)
                                                                              .FirstOrDefault();

                                if (profissional != null)
                                {
                                    var ex1 = new GuiaSpsadtEquipe();

                                    ex1.CodigoOperadoraCpf = profissional.Cpf ?? "___________________";
                                    ex1.ConselhoProfissional = profissional.Conselho?.Codigo ?? "______________";
                                    ex1.GrauPart = "_______";
                                    ex1.NomeProfissional = profissional.NomeCompleto ?? "____________________________________________________________________";
                                    ex1.NumeroConselho = profissional.NumeroConselho != 0 ? profissional.NumeroConselho.ToString() : "_____________";
                                    ex1.SeqRef = (i++).ToString().PadLeft(2, '0');
                                    ex1.Uf = profissional.Estado?.Codigo ?? "_________";

                                    if (item.EspecialidadeId != null)
                                    {
                                        var especialidade = especialidadeRepository.Object.GetAll()
                                                                                   .Include(j => j.SisCbo)
                                                                                   .Where(w => w.Id == item.EspecialidadeId)
                                                                                   .FirstOrDefault();

                                        if (especialidade != null)
                                        {
                                            ex1.CodigoCbo = especialidade.SisCbo?.Codigo ?? "______________";
                                        }
                                        else
                                        {
                                            ex1.CodigoCbo = "______________";
                                        }
                                    }
                                    else
                                    {
                                        ex1.CodigoCbo = "______________";
                                    }

                                    equipe.Add(ex1);

                                }
                            }

                            if (qtd == 4)
                            {
                                break;
                            }

                        }

                        // List<GuiaSpsadtEquipe> equipe = new List<GuiaSpsadtEquipe>();
                        for (int j = qtd; j < 4; j++)
                        {
                            var ex1 = new GuiaSpsadtEquipe();
                            ex1.CodigoCbo = "______________";
                            ex1.CodigoOperadoraCpf = "___________________";
                            ex1.ConselhoProfissional = "______________";
                            ex1.GrauPart = "_______";
                            ex1.NomeProfissional = "____________________________________________________________________";
                            ex1.NumeroConselho = "_____________";
                            ex1.SeqRef = (i++).ToString().PadLeft(2, '0');
                            ex1.Uf = "_________";

                            equipe.Add(ex1);
                        }

                        DataTable tabelaEquipe = this.ConvertToDataTable(equipe, relDS.Tables["SpsadtEquipe"]);
                        ReportDataSource dataSourceEquipe = new ReportDataSource("GuiaSpsadtEquipe", tabelaEquipe);
                        e.DataSources.Add(dataSourceEquipe);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void LocalReport_SubreportProcessingSpsadtDespesas(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                var dados = new GuiaSpsadtModel();
                dados.Contas = new List<ContaMedicaReportModel>();
                var x = new ContaMedicaReportModel();
                x.AtendimentoCodigo = "12313212";
                dados.Contas.Add(x);
                Guias relDS = new Guias();

                // Despesa Itens

                List<GuiaSpsadtDespesaItem> equipe = new List<GuiaSpsadtDespesaItem>();
                for (int i = 0; i < 7; i++)
                {
                    var ex1 = new GuiaSpsadtDespesaItem();
                    ex1.Descricao = "" + i.ToString();
                    ex1.Cd = "" + i.ToString();
                    equipe.Add(ex1);
                }

                DataTable tabelaEquipe = this.ConvertToDataTable(equipe, relDS.Tables["SpsadtDespesasItens"]);
                ReportDataSource dataSourceEquipe = new ReportDataSource("SpsadtDespesasItens", tabelaEquipe);
                e.DataSources.Add(dataSourceEquipe);

                // Despesa loop

                //List<teste> loop = new List<teste>();
                //for (int i = 0; i < 4; i++)
                //{
                //    var ex1 = new teste();
                //    ex1.Despesas = "despesas" + i.ToString();

                //    loop.Add(ex1);
                //}

                //DataTable tabelaEquipeloop = this.ConvertToDataTable(loop, relDS.Tables["SpsadtDespesas"]);
                //ReportDataSource dataSourceEquipeloop = new ReportDataSource("SpsadtDespesasLoopDataSet", tabelaEquipeloop);
                //e.DataSources.Add(dataSourceEquipeloop);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public DataTable ConvertToDataTable<T>(IList<T> data, DataTable table)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));

            if (data != null)
            {
                foreach (T item in data)
                {
                    try
                    {
                        DataRow row = table.NewRow();
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

        //public static IEnumerable<List<T>> repartirLista<T> (List<T> lista, int tamanho)
        //{

        //        for (int i = 0; i < lista.Count; i+= tamanho)
        //        {
        //            yield return lista.GetRange(i, Math.Min(tamanho, lista.Count - i));
        //        }

        //}
    }

    public class GuiaSpsadtItemSolic
    {
        public string Tabela { get; set; }
        public string CodigoProcedimento { get; set; }
        public string Descricao { get; set; }
        public string QtSolic { get; set; }
        public string QtAutoriz { get; set; }
    }

    public class GuiaSpsadtExame
    {
        public string Data { get; set; }
        public string HoraInicial { get; set; }
        public string HoraFinal { get; set; }
        public string Tabela { get; set; }
        public string CodigoProcedimento { get; set; }
        public string Descricao { get; set; }
        public string Qtde { get; set; }
        public string Via { get; set; }
        public string Tec { get; set; }
        public string RedAcresc { get; set; }
        public string ValorUnitario { get; set; }
        public string ValorTotal { get; set; }
    }

    public class GuiaSpsadtEquipe
    {
        public string SeqRef { get; set; }
        public string GrauPart { get; set; }
        public string CodigoOperadoraCpf { get; set; }
        public string NomeProfissional { get; set; }
        public string ConselhoProfissional { get; set; }
        public string NumeroConselho { get; set; }
        public string Uf { get; set; }
        public string CodigoCbo { get; set; }
    }

    //public class GuiaSpsadtDespesaItem
    //{
    //    public string Cd { get; set; }
    //    public string Data { get; set; }
    //    public string HoraInicial { get; set; }
    //    public string HoraFinal { get; set; }
    //    public string Tabela { get; set; }
    //    public string CodigoItem { get; set; }
    //    public string Qtde { get; set; }
    //    public string UnidadeMedida { get; set; }
    //    public string RedAcres { get; set; }
    //    public string ValorUnitario { get; set; }
    //    public string ValorTotal { get; set; }
    //    public string RegistroAnvisa { get; set; }
    //    public string RefMaterialFabricante { get; set; }
    //    public string NumAutorizacaoFuncionamento { get; set; }
    //    public string Descricao { get; set; }

    //    public void LerContaItem(FaturamentoContaItem item)
    //    {
    //        Cd = "";
    //        Data = item.Data?.ToString("dd/MM/yyyy");
    //        HoraInicial = item.HoraIncio.ToString();
    //        HoraFinal = item.HoraFim.ToString();
    //        Tabela = "";
    //        CodigoItem = item.Codigo;
    //        Qtde = item.Qtde.ToString();
    //        UnidadeMedida = "";
    //        RedAcres = "";
    //        ValorUnitario = "";
    //        ValorTotal = "";
    //        RegistroAnvisa = "";
    //        RefMaterialFabricante = "";
    //        NumAutorizacaoFuncionamento = "";
    //        Descricao = item.Descricao;
    //    }
    //}

}