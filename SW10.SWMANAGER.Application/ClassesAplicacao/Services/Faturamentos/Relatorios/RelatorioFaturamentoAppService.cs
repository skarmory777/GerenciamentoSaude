using Abp.Domain.Repositories;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Relatorios
{
    public class RelatorioFaturamentoAppService : SWMANAGERAppServiceBase, IRelatorioFaturamentoAppService
    {
        //    private readonly IRepository<Faturamento, long> _atendimentoRepository;
        internal IRepository<Empresa, long> _empresaRepositorio { get; private set; }

        private readonly IRepository<UserEmpresa, long> _userEmpresas;

        private readonly IContaAppService _contaMedicaAppService;
        private readonly IFaturamentoContaItemAppService _contaItemAppService;
        private readonly IAtendimentoAppService _atendimentoAppService;
        private readonly IFaturamentoConfigConvenioAppService _configConvenioAppService;
        private readonly IFaturamentoGuiaAppService _guiaAppService;

        public RelatorioFaturamentoAppService(
            IRepository<Empresa, long> empresaRepositorio,
            IRepository<UserEmpresa, long> userEmpresas,
            IContaAppService contaMedicaAppService,
            IFaturamentoContaItemAppService contaItemAppService,
            IAtendimentoAppService atendimentoAppService,
            IFaturamentoConfigConvenioAppService configConvenioAppService,
            IFaturamentoGuiaAppService guiaAppService
            )
        {
            _empresaRepositorio = empresaRepositorio;
            _userEmpresas = userEmpresas;
            _contaMedicaAppService = contaMedicaAppService;
            _contaItemAppService = contaItemAppService;
            _atendimentoAppService = atendimentoAppService;
            _configConvenioAppService = configConvenioAppService;
            _guiaAppService = guiaAppService;
        }


        public IList<GenericoIdNome> ListarEmpresaUsuario(long id)
        {
            var result = _userEmpresas
                .GetAll()
                .Where(m => m.UserId == id)
                .Where(m => m.IsDeleted == false);

            //var result = await query
            //    .Select(m => m.Empresa)
            //    .ToListAsync();

            //var result = await _userEmpresas
            //    .Query(q => q.Where(g => g.IsDeleted == false))
            //    .ToListAsync();

            if (result != null)
            {
                return result
                 .Select(s => new GenericoIdNome
                 {
                     Id = s.EmpresaId,
                     Nome = s.Empresa.NomeFantasia
                 })
                 .ToList();
            }

            return new List<GenericoIdNome>();
        }


        //public async Task<byte[]> GuiaResumoInternacaoPdf(long atendimentoId, string reportPath, dynamic resumo_internacao_dataset)
        //{
        //    try
        //    {
        //        var atendimento = await _atendimentoAppService.Obter((long)atendimentoId);
        //        var dados = GuiaResumoInternacaoModel.MapearFromAtendimento(atendimento);

        //        // Guia principal
        //        DataTable tabela = this.ConvertToDataTable(dados.Lista, resumo_internacao_dataset.Tables["resumo_internacao_table"]);
        //        DataRow row = tabela.NewRow();
        //        row["Logotipo"] = atendimento.Empresa.Logotipo;
        //        //   tabela.Rows[tabela.Rows.Count - 1].Delete();
        //        tabela.Rows.Add(row);

        //        ReportDataSource dataSource = new ReportDataSource("resumo_internacao_dataset", tabela);
        //        ReportViewer reportViewer = new ReportViewer();
        //        reportViewer.LocalReport.DataSources.Add(dataSource);
        //        ScriptManager scriptManager = new ScriptManager();
        //        scriptManager.RegisterPostBackControl(reportViewer);
        //        reportViewer.LocalReport.ReportPath = reportPath;
        //        SetParametrosResumoInternacao(reportViewer, dados);

        //        // APARENTEMENTE FALTANDO SUB-RELATORIOS
        //        string mimeType = string.Empty;
        //        string encoding = string.Empty;
        //        string extension = "pdf";

        //        string[] streamIds;
        //        Warning[] warnings;
        //        byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
        //        reportViewer.LocalReport.Refresh();

        //        //    Response.Headers.Add("Content-Disposition", "inline; filename=PulseiraInternacao.pdf");
        //        return pdfBytes;
        //    }
        //    catch (Exception e)
        //    {
        //        e.ToString();
        //        return null;
        //    }
        //}


        //private void SetParametrosResumoInternacao(ReportViewer rv, GuiaResumoInternacaoModel dados)
        //{
        //    ReportParameter NomePaciente = new ReportParameter("NomePaciente", dados.NomePaciente);
        //    ReportParameter Matricula = new ReportParameter("Matricula", dados.Matricula);
        //    ReportParameter RegistroANS = new ReportParameter("RegistroANS", dados.RegistroANS);
        //    ReportParameter ValidadeCarteira = new ReportParameter("ValidadeCarteira", dados.ValidadeCarteira);
        //    ReportParameter Senha = new ReportParameter("Senha", dados.Senha);
        //    ReportParameter CodCNES = new ReportParameter("CodCNES", dados.CodCNES);
        //    ReportParameter DataAutorizacao = new ReportParameter("DataAutorizacao", dados.DataAutorizacao);
        //    ReportParameter NomeContratado = new ReportParameter("NomeContratado", dados.NomeContratado);
        //    ReportParameter ValidadeSenha = new ReportParameter("ValidadeSenha", dados.ValidadeSenha);
        //    ReportParameter NumeroGuia = new ReportParameter("NumeroGuia", dados.NumeroGuia);
        //    ReportParameter Cid1 = new ReportParameter("Cid1", dados.Cid1);
        //    ReportParameter Cid2 = new ReportParameter("Cid2", dados.Cid2);
        //    ReportParameter Cid3 = new ReportParameter("Cid3", dados.Cid3);
        //    ReportParameter Cid4 = new ReportParameter("Cid4", dados.Cid4);
        //    ReportParameter CodOperadora = new ReportParameter("CodOperadora", dados.CodOperadora);
        //    ReportParameter CaraterAtendimento = new ReportParameter("CaraterAtendimento", dados.CaraterAtendimento);
        //    ReportParameter TipoFaturamento = new ReportParameter("TipoFaturamento", dados.TipoFaturamento);
        //    ReportParameter DataIniFaturamento = new ReportParameter("DataIniFaturamento", dados.DataIniFaturamento);
        //    ReportParameter DataFimFaturamento = new ReportParameter("DataFimFaturamento", dados.DataFimFaturamento);
        //    ReportParameter HoraIniFaturamento = new ReportParameter("HoraIniFaturamento", dados.HoraIniFaturamento);
        //    ReportParameter HoraFimFaturamento = new ReportParameter("HoraFimFaturamento", dados.HoraFimFaturamento);
        //    ReportParameter TipoInternacao = new ReportParameter("TipoInternacao", dados.TipoInternacao);
        //    ReportParameter RegimeInternacao = new ReportParameter("RegimeInternacao", dados.RegimeInternacao);
        //    ReportParameter TotalProcedimentos = new ReportParameter("TotalProcedimentos", dados.TotalProcedimentos);
        //    ReportParameter TotalDiaria = new ReportParameter("TotalDiaria", dados.TotalDiaria);
        //    ReportParameter TotalTaxasAlugueis = new ReportParameter("TotalTaxasAlugueis", dados.TotalTaxasAlugueis);
        //    ReportParameter TotalMateriais = new ReportParameter("TotalMateriais", dados.TotalMateriais);
        //    ReportParameter TotalOpme = new ReportParameter("TotalOpme", dados.TotalOpme);
        //    ReportParameter TotalMedicamentos = new ReportParameter("TotalMedicamentos", dados.TotalMedicamentos);
        //    ReportParameter TotalGasesMedicinais = new ReportParameter("TotalGasesMedicinais", dados.TotalGasesMedicinais);
        //    ReportParameter TotalGeral = new ReportParameter("TotalGeral", dados.TotalGeral);
        //    ReportParameter RN = new ReportParameter("RN", dados.RN ? "S" : "N");




        //    rv.LocalReport.SetParameters(new ReportParameter[] {
        //        NomePaciente            ,
        //        Matricula               ,
        //        RegistroANS             ,
        //        ValidadeCarteira        ,
        //        Senha                   ,
        //        CodCNES                 ,
        //        DataAutorizacao         ,
        //        NomeContratado          ,
        //        ValidadeSenha           ,
        //        NumeroGuia              ,
        //        Cid1                    ,
        //        Cid2                    ,
        //        Cid3                    ,
        //        Cid4                    ,
        //        CodOperadora            ,
        //        CaraterAtendimento      ,
        //        TipoFaturamento         ,
        //        DataIniFaturamento      ,
        //        DataFimFaturamento      ,
        //        HoraIniFaturamento      ,
        //        HoraFimFaturamento      ,
        //        TipoInternacao          ,
        //        RegimeInternacao        ,
        //        TotalProcedimentos      ,
        //        TotalDiaria             ,
        //        TotalTaxasAlugueis      ,
        //        TotalMateriais          ,
        //        TotalOpme               ,
        //        TotalMedicamentos       ,
        //        TotalGasesMedicinais    ,
        //        TotalGeral              ,
        //        RN
        //    });
        //}


        //private DataTable ConvertToDataTable<T>(IList<T> data, DataTable table)
        //{
        //    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

        //    if (data != null)
        //    {
        //        foreach (T item in data)
        //        {
        //            try
        //            {
        //                DataRow row = table.NewRow();
        //                foreach (PropertyDescriptor prop in properties)
        //                    try
        //                    {
        //                        if (prop.GetValue(item) != null)
        //                        {
        //                            if (prop.PropertyType == typeof(string))
        //                            {
        //                                if ((string)prop.GetValue(item) != string.Empty)
        //                                {
        //                                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
        //                        }


        //                    }
        //                    catch (Exception exs)
        //                    {

        //                    }
        //                table.Rows.Add(row);
        //            }
        //            catch (Exception ex)
        //            {
        //                ex.ToString();
        //            }
        //        }
        //    }

        //    return table;
        //}

    }
}
