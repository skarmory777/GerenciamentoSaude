#region Usings

using BarcodeFree;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Visitantes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.ModeloTextos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.MailingTemplates;
using SW10.SWMANAGER.DataExporting.HtmlParaImagem;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AmbulatorioEmergencias;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos.Fichas
{
    public class FichaAmbulatorioController : SWMANAGERControllerBase
    {
        #region Dependencias
        private readonly IContaAppService _contaMedicaAppService;
        private readonly IPacienteAppService _pacienteAppService;
        private readonly IFaturamentoContaItemAppService _contaItemAppService;
        private readonly IAtendimentoAppService _atendimentoAppService;
        private readonly IModeloTextoGuiaAppService _modeloTextoGuiaAppService;
        private readonly IUserAppService _userAppService;
        private readonly IFaturamentoConfigConvenioAppService _configConvenioAppService;
        private readonly IFaturamentoGuiaAppService _guiaAppService;
        private readonly IMailingTemplateAppService _mailingTemplateAppService;
        private readonly IVisitanteAppService _visitanteAppService;
        private ISessionAppService _sessionAppService;
        private readonly IHtmlToPDF _htmlToPDFAppService;

        // Dados Guia Spsadt
        List<FaturamentoContaItemDto> _itensGuiaPrincipal = new List<FaturamentoContaItemDto>();

        public FichaAmbulatorioController(
            IContaAppService contaMedicaAppService,
            IPacienteAppService pacienteAppService,
            IFaturamentoContaItemAppService contaItemAppService,
            IAtendimentoAppService atendimentoAppService,
            IModeloTextoGuiaAppService modeloTextoGuiaAppService,
            ISessionAppService sessionAppService,
            IUserAppService userAppService,
            IFaturamentoConfigConvenioAppService configConvenioAppService,
            IMailingTemplateAppService mailingTemplateAppService,
             IVisitanteAppService visitanteAppService,
            IFaturamentoGuiaAppService guiaAppService,
            IHtmlToPDF htmlToPDFAppService
            )
        {
            _contaMedicaAppService = contaMedicaAppService;
            _pacienteAppService = pacienteAppService;
            _contaItemAppService = contaItemAppService;
            _atendimentoAppService = atendimentoAppService;
            _modeloTextoGuiaAppService = modeloTextoGuiaAppService;
            _sessionAppService = sessionAppService;
            _userAppService = userAppService;
            _configConvenioAppService = configConvenioAppService;
            _mailingTemplateAppService = mailingTemplateAppService;
            _visitanteAppService = visitanteAppService;
            _guiaAppService = guiaAppService;
            _htmlToPDFAppService = htmlToPDFAppService;
        }
        #endregion dependencias.

        public async Task<ActionResult> ModalFichaAmbulatorio(long atendimentoId)
        {
            ModalFichaAmbulatorioViewModel viewModel = new ModalFichaAmbulatorioViewModel
            {
                AtendimentoId = atendimentoId.ToString()
                // ,
                // FichaPdf = await GerarFichaInternacao(atendimentoId) as FileContentResult
            };

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AmbulatorioEmergencias/Home/Ficha/_ModalFichaAmbulatorio.cshtml", viewModel);
        }

        private static int CalculateIdade(DateTime DataNascimento)
        {

            int anos = DateTime.Now.Year - DataNascimento.Year;

            if (DateTime.Now.Month < DataNascimento.Month || (DateTime.Now.Month == DataNascimento.Month && DateTime.Now.Day < DataNascimento.Day))

                anos--;

            return anos;

        }

        public async Task<string> GerarFichaAmbulatorio(long atendimentoId)
        {
            var atendimento = await _atendimentoAppService.Obter(atendimentoId);
            var modeloTexto = _modeloTextoGuiaAppService.Obter(atendimento.FatGuiaId, atendimento.EmpresaId);
            var loginInformations = await _sessionAppService.GetCurrentLoginInformations();
            var visitante = _visitanteAppService.ObterVisitantePorAtendimentoId(atendimentoId);
            var dados = FichaAmbulatorioInternacaoModel.MapearFromAtendimento(atendimento, visitante, modeloTexto.Texto, string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname));

            try
            {

                #region [ Gerando código de Barra ]
                string barCode = dados.CodigoAtendimento.PadLeft(8, '0');
                var UUID = "CodigoBarra-" + System.Guid.NewGuid().ToString() + ".png";
                //https://www.colome.org/free-dll-generate-image-barcodes/
                Code128 text = new Code128();
                barCode = text.Encode(barCode);
                ImageBarcode barcode; //barcode2, barcode3, barcode4;
                barcode = new ImageBarcode(barCode.Length * 40, 80, barCode);
                //barcode2 = new ImageBarcode(barCode.Length * 40, 80, barCode, false, Color.Red, Color.Yellow);
                //barcode3 = new ImageBarcode(barCode.Length * 40, 80, barCode, true);
                //barcode4 = new ImageBarcode(barCode.Length * 40, 80, barCode, false, Color.Turquoise, Color.LightGray);      
                using (Bitmap fileImg = barcode.GenerateImage())
                {
                    var pathImg = string.Concat(Server.MapPath("/"), @"Temp\" + UUID);
                    fileImg.Save(pathImg, System.Drawing.Imaging.ImageFormat.Png);
                }
                #endregion                             

                #region[Parametros]

                var htmlParameterDynamic = FichaAmbulatorioInternacaoModel.ParameterParse(dados, modeloTexto, _atendimentoAppService, atendimento, UUID);

                #endregion

                #region [Gerando PDF]
                StringReader sr = new StringReader(htmlParameterDynamic);
                var UUID_PDF = "FichaAmbulatorio-" + System.Guid.NewGuid().ToString() + ".pdf";
                var path = string.Concat(Server.MapPath("/"), @"Temp\" + UUID_PDF);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                FileStream file = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                var _pdfBytes = _htmlToPDFAppService.ConvertHtmlToPDF(sr);
                file.Write(_pdfBytes, 0, _pdfBytes.Length);
                file.Close();
                #endregion

                return UUID_PDF;

                #region [Modo antigo de exibição]
                //// Renderizar Png
                //string mime, enc, ext;
                //string[] streams;
                //Warning[] ws;
                //string deviceInfo = "<DeviceInfo>" +
                //                    "<OutputFormat>png</OutputFormat>"   +
                //                    "<PageWidth>7.2in</PageWidth>"       +
                //                    "<PageHeight>4.5in</PageHeight>"     +
                //                    "<MarginTop>0.1in</MarginTop>"       +
                //                    "<MarginLeft>0.1in</MarginLeft>"     +
                //                    "<MarginRight>0.1in</MarginRight>"   +
                //                    "<MarginBottom>0.1in</MarginBottom>" +
                //                    "</DeviceInfo>";

                //byte[] bytes = reportViewer.LocalReport.Render("Image", deviceInfo, out mime, out enc, out ext, out streams, out ws);
                //byte[] pdfOut;
                //// iText - Gerar pdf e anexar report_png

                //// Margens
                //float ml = 1f;
                //float mr = 1f;
                //float mt = 1f;
                //float mb = 1f;

                //using (Document documento = new Document(PageSize.A4, ml, mr, mt, mb))
                //using (MemoryStream msOutput = new MemoryStream())
                //using (PdfWriter writer = PdfWriter.GetInstance(documento, msOutput))
                //{
                //    writer.CloseStream = false;
                //    documento.Open();

                //    // Report_png
                //    Image img = Image.GetInstance(bytes);
                //    img.ScalePercent(90f);
                //    img.ScaleToFit(documento.PageSize.Width, documento.PageSize.Height / 2);
                //    img.Alignment = Element.ALIGN_CENTER;
                //    documento.Add(img);

                //    // Linha horizontal
                //    Font lineFont = FontFactory.GetFont("Verdana", 14, Font.BOLD, BaseColor.BLACK);
                //    Paragraph line = new Paragraph("__________________________________________________________\n\n", lineFont);
                //    line.Alignment = Element.ALIGN_CENTER;
                //    documento.Add(line);

                //    // CONFIG PRONTUARIO
                //    //var email = await _mailingTemplateAppService.Obter(2); // id 1 temp para teste

                //    //// XML Worker
                //    //var w = XMLWorkerHelper.GetInstance();

                //    //using (var stream = GenerateStreamFromString(email.ContentTemplate))
                //    //{
                //    //    w.ParseXHtml(writer, documento, stream, System.Text.Encoding.UTF8);
                //    //}
                //    // Fim - XML Worker
                //    // Fim - mailing template

                //    documento.Close();
                //    pdfOut = msOutput.ToArray();
                //}
                // Fim - iText

                //Response.Headers.Add("Content-Disposition", "inline; filename=ficha_amb.pdf");
                //return File(pdfOut, "application/pdf");                
                #endregion

                #region [Antigo modo de geração PDF]
                //https://www.aspsnippets.com/Articles/ASPNet-MVC-Dynamically-generate-and-display-Barcode-Image.aspx
                //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
                //using (Bitmap bitMap = new Bitmap(barCode.Length * 40, 80))
                //{
                //    using (Graphics graphics = Graphics.FromImage(bitMap))
                //    {
                //        System.Drawing.Font oFont = new System.Drawing.Font("IDAutomationHC39M", 16);
                //        PointF point = new PointF(2f, 2f);
                //        SolidBrush blackBrush = new SolidBrush(Color.Black);
                //        SolidBrush whiteBrush = new SolidBrush(Color.White);
                //        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                //        graphics.DrawString(barCode, oFont, blackBrush, point);
                //    }

                //    byte[] byteImage = null;
                //    using (MemoryStream ms = new MemoryStream())
                //    {
                //        bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //        byteImage = ms.ToArray();

                //        //Convert.ToBase64String(byteImage);
                //        imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                //    }

                //    var path = string.Concat(Server.MapPath("/"), @"Common\Images\Barcode\" + UUID);
                //    using (System.Drawing.Image image = System.Drawing.Image.FromStream(new MemoryStream(byteImage)))
                //    {
                //        image.Save(path, System.Drawing.Imaging.ImageFormat.Png);  // Or Png
                //    }
                //}
                #endregion

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //// Metodo auxiliar String to Stream

        //public static Stream GenerateStreamFromString(string s)
        //{
        //    var stream = new MemoryStream();
        //    var writer = new StreamWriter(stream);
        //    writer.Write(s);
        //    writer.Flush();
        //    stream.Position = 0;
        //    return stream;
        //}

        ////  Fim - metodo auxiliar String to Stream

        //public void SetParametrosFicha(ReportViewer rv, FichaAmbulatorioModel dados)
        //{
        //    ReportParameter Paciente = new ReportParameter("Paciente", dados.Paciente);
        //    ReportParameter Usuario = new ReportParameter("Usuario", dados.Usuario);
        //    ReportParameter Empresa = new ReportParameter("Empresa", dados.Empresa);
        //    ReportParameter EmpresaRazaoSocial = new ReportParameter("EmpresaRazaoSocial", dados.EmpresaRazaoSocial);
        //    ReportParameter DataHora = new ReportParameter("DataHora", dados.DataHora);
        //    ReportParameter CodigoPaciente = new ReportParameter("CodigoPaciente", dados.CodigoPaciente);
        //    ReportParameter DataAtendimento = new ReportParameter("DataAtendimento", dados.DataAtendimento);
        //    ReportParameter Sexo = new ReportParameter("Sexo", dados.Sexo);
        //    ReportParameter Nascimento = new ReportParameter("Nascimento", dados.Nascimento);
        //    ReportParameter Identidade = new ReportParameter("Identidade", dados.Identidade);
        //    ReportParameter Cpf = new ReportParameter("Cpf", dados.Cpf);
        //    ReportParameter EstadoCivil = new ReportParameter("EstadoCivil", dados.EstadoCivil);
        //    ReportParameter Complemento = new ReportParameter("Complemento", dados.Complemento);
        //    ReportParameter Cidade = new ReportParameter("Cidade", dados.Cidade);
        //    ReportParameter Telefone = new ReportParameter("Telefone", dados.Telefone);
        //    ReportParameter Profissao = new ReportParameter("Profissao", dados.Profissao);
        //    ReportParameter Numero = new ReportParameter("Numero", dados.Numero);
        //    ReportParameter Pais = new ReportParameter("Pais", dados.Pais);
        //    ReportParameter Cep = new ReportParameter("Cep", dados.Cep);
        //    ReportParameter CodigoAtendimento = new ReportParameter("CodigoAtendimento", dados.CodigoAtendimento);
        //    ReportParameter DataAlta = new ReportParameter("DataAlta", dados.DataAlta);
        //    ReportParameter Alta = new ReportParameter("Alta", dados.Alta);
        //    ReportParameter Matricula = new ReportParameter("Matricula", dados.Matricula);
        //    ReportParameter Validade = new ReportParameter("DataValidade", dados.Validade);
        //    ReportParameter DataPagto = new ReportParameter("DataPagto", dados.DataPagto);
        //    ReportParameter IdAcompanahante = new ReportParameter("IdAcompanhante", dados.IdAcompanahante);
        //    ReportParameter CodDep = new ReportParameter("CodDep", dados.CodDep);
        //    ReportParameter Endereco = new ReportParameter("Endereco", dados.Endereco);
        //    ReportParameter Bairro = new ReportParameter("Bairro", dados.Bairro);
        //    ReportParameter Estado = new ReportParameter("Estado", dados.Estado);
        //    ReportParameter Nacionalidade = new ReportParameter("Nacionalidade", dados.Nacionalidade);
        //    ReportParameter Filiacao = new ReportParameter("Filiacao", dados.Filiacao);
        //    ReportParameter Medico = new ReportParameter("Medico", dados.Medico);
        //    ReportParameter Especialidade = new ReportParameter("Especialidade", dados.Especialidade);
        //    ReportParameter IndicadoPor = new ReportParameter("IndicadoPor", dados.IndicadoPor);
        //    ReportParameter Origem = new ReportParameter("Origem", dados.Origem);
        //    ReportParameter Tratamento = new ReportParameter("Tratamento", dados.Tratamento);
        //    ReportParameter Convenio = new ReportParameter("Convenio", dados.Convenio);
        //    ReportParameter Plano = new ReportParameter("Plano", dados.Plano);
        //    ReportParameter Guia = new ReportParameter("Guia", dados.Guia);
        //    ReportParameter NumeroGuia = new ReportParameter("NumeroGuia", dados.NumeroGuia);
        //    ReportParameter Titular = new ReportParameter("Titular", dados.Titular);
        //    //ReportParameter ModeloTexto        = new ReportParameter("ModeloTexto",        dados.ModeloTexto);


        //    rv.LocalReport.SetParameters(new ReportParameter[] {

        //                Paciente          ,
        //                Usuario           ,
        //                Empresa           ,
        //                EmpresaRazaoSocial,
        //                DataHora          ,
        //                CodigoPaciente    ,
        //                DataAtendimento   ,
        //                Sexo              ,
        //                Nascimento        ,
        //                Identidade        ,
        //                Cpf               ,
        //                EstadoCivil       ,
        //                Complemento       ,
        //                Cidade            ,
        //                Telefone          ,
        //                Profissao         ,
        //                Numero            ,
        //                Pais              ,
        //                Cep               ,
        //                CodigoAtendimento ,
        //                DataAlta          ,
        //                Alta              ,
        //                Matricula         ,
        //                Validade          ,
        //                DataPagto         ,
        //                IdAcompanahante   ,
        //                CodDep            ,
        //                Endereco          ,
        //                Bairro            ,
        //                Estado            ,
        //                Nacionalidade     ,
        //                Filiacao          ,
        //                Medico            ,
        //                Especialidade     ,
        //                IndicadoPor       ,
        //                Origem            ,
        //                Tratamento        ,
        //                Convenio          ,
        //                Plano             ,
        //                Guia              ,
        //                NumeroGuia        ,
        //                Titular           
        //                //ModeloTexto
        //    });
        //}

        //public DataTable ConvertToDataTable<T>(IList<T> data, DataTable table)
        //{
        //    PropertyDescriptorCollection properties =
        //       TypeDescriptor.GetProperties(typeof(T));

        //    if (data != null)
        //    {
        //        foreach (T item in data)
        //        {
        //            try
        //            {
        //                DataRow row = table.NewRow();
        //                foreach (PropertyDescriptor prop in properties)
        //                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
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

    public class FichaAmbulatorioInput
    {
        public long AtendimentoId { get; set; }
    }

    public class FichaAmbulatorioModel
    {
        public string Paciente { get; set; }
        public string Usuario { get; set; }
        public string Empresa { get; set; }
        public string EmpresaRazaoSocial { get; set; }
        public string DataHora { get; set; }
        public string CodigoPaciente { get; set; }
        public string DataAtendimento { get; set; }
        public string Sexo { get; set; }
        public string Nascimento { get; set; }
        public string Identidade { get; set; }
        public string Cpf { get; set; }
        public string EstadoCivil { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Telefone { get; set; }
        public string Profissao { get; set; }
        public string Numero { get; set; }
        public string Pais { get; set; }
        public string Cep { get; set; }
        public string CodigoAtendimento { get; set; }
        public string DataAlta { get; set; }
        public string Alta { get; set; }
        public string Matricula { get; set; }
        public string Validade { get; set; }
        public string DataPagto { get; set; }
        public string IdAcompanahante { get; set; }
        public string CodDep { get; set; }
        public string Endereco { get; set; }
        public string Bairro { get; set; }
        public string Estado { get; set; }
        public string Nacionalidade { get; set; }
        public string Filiacao { get; set; }
        public string Medico { get; set; }
        public string Especialidade { get; set; }
        public string IndicadoPor { get; set; }
        public string Origem { get; set; }
        public string Tratamento { get; set; }
        public string Convenio { get; set; }
        public string Plano { get; set; }
        public string Guia { get; set; }
        public string NumeroGuia { get; set; }
        public string Titular { get; set; }
        public string ModeloTexto { get; set; }

        public static FichaAmbulatorioModel MapearFromAtendimento(AtendimentoDto atendimento, string modeloTexto = "")
        {
            var ficha = new FichaAmbulatorioModel();

            ficha.Paciente = atendimento.Paciente?.NomeCompleto;
            ficha.Empresa = atendimento.Empresa?.NomeFantasia;
            ficha.EmpresaRazaoSocial = atendimento.Empresa.RazaoSocial;
            ficha.DataHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm");//atendimento.DataRegistro.ToString("dd/MM/yyyy hh:mm");
            ficha.CodigoPaciente = atendimento.Paciente.CodigoPaciente.ToString();
            ficha.DataAtendimento = atendimento.DataRegistro.ToString("dd/MM/yyyy hh:mm");
            ficha.Sexo = atendimento.Paciente.Sexo != null ? atendimento.Paciente.Sexo.Descricao : atendimento.Paciente.SisPessoa.Sexo?.Descricao;
            ficha.Nascimento = !atendimento.Paciente.Nascimento.HasValue ? "" : ((DateTime)atendimento.Paciente.Nascimento).ToString("dd/MM/yyyy");
            ficha.Identidade = atendimento.Paciente.Rg;
            ficha.Cpf = atendimento.Paciente.Cpf;
            ficha.EstadoCivil = atendimento.Paciente.EstadoCivil != null ? atendimento.Paciente.EstadoCivil?.Descricao : atendimento.Paciente.SisPessoa.EstadoCivil?.Descricao;
            ficha.Complemento = atendimento.Paciente.Complemento != null ? atendimento.Paciente.Complemento : atendimento.Paciente.SisPessoa.Enderecos?[0].Complemento;
            ficha.Pais = atendimento.Paciente.Pais != null ? atendimento.Paciente.Pais?.Nome : ((atendimento.Paciente.SisPessoa?.Enderecos?.Count > 0) ? atendimento.Paciente.SisPessoa.Enderecos?[0].Pais?.Nome : "");
            ficha.Estado = atendimento.Paciente.Estado != null ? atendimento.Paciente.Estado?.Uf : ((atendimento.Paciente.SisPessoa?.Enderecos?.Count > 0) ? atendimento.Paciente.SisPessoa.Enderecos?[0].Estado?.Nome : "");
            ficha.Cidade = atendimento.Paciente.Cidade != null ? atendimento.Paciente.Cidade?.Nome : ((atendimento.Paciente.SisPessoa?.Enderecos?.Count > 0) ? atendimento.Paciente.SisPessoa.Enderecos?[0].Cidade?.Nome : "");
            ficha.Endereco = atendimento.Paciente.Logradouro != null ? atendimento.Paciente.Logradouro : ((atendimento.Paciente.SisPessoa?.Enderecos?.Count > 0) ? atendimento.Paciente.SisPessoa.Enderecos?[0].Logradouro : "");
            ficha.Bairro = atendimento.Paciente.Bairro != null ? atendimento.Paciente.Bairro : ((atendimento.Paciente.SisPessoa?.Enderecos?.Count > 0) ? atendimento.Paciente.SisPessoa.Enderecos?[0].Bairro : "");
            ficha.Nacionalidade = atendimento.Paciente.Nacionalidade != null ? atendimento.Paciente.Nacionalidade?.Descricao : atendimento.Paciente.SisPessoa.Nacionalidade?.Descricao;
            ficha.Cep = atendimento.Paciente.Cep != null ? atendimento.Paciente.Cep : atendimento.Paciente.SisPessoa.Enderecos?[0].Cep;
            ficha.Telefone = atendimento.Paciente.Telefone1;
            ficha.Profissao = atendimento.Paciente.Profissao != null ? atendimento.Paciente.Profissao.Descricao : atendimento.Paciente.SisPessoa.Profissao?.Descricao;
            ficha.Numero = atendimento.Paciente.Numero;
            ficha.CodigoAtendimento = atendimento.Codigo;
            ficha.DataAlta = atendimento.DataAlta.ToString();
            ficha.Alta = "";
            ficha.Matricula = atendimento.Matricula;
            ficha.Validade = ((DateTime)atendimento.ValidadeCarteira).ToString("dd/MM/yyyy");
            ficha.DataPagto = atendimento.DataUltimoPagamento?.ToString("dd/MM/yyyy");
            ficha.IdAcompanahante = atendimento.RgResponsavel;
            ficha.CodDep = atendimento.CodDependente;
            ficha.Filiacao = atendimento.Paciente.NomeMae;
            ficha.Medico = atendimento.Medico.NomeCompleto;
            ficha.Especialidade = atendimento.Especialidade?.Descricao;
            ficha.IndicadoPor = "";
            ficha.Origem = atendimento.Origem?.Descricao;
            ficha.Tratamento = "";
            ficha.Convenio = atendimento.Convenio?.NomeFantasia;
            ficha.Plano = atendimento.Plano?.Descricao;
            ficha.Guia = atendimento.FatGuia?.Descricao;
            ficha.NumeroGuia = atendimento.GuiaNumero;
            ficha.Titular = atendimento.Titular;
            ficha.ModeloTexto = modeloTexto;

            return ficha;
        }

        public List<string> Lista { get; set; }

        public FichaAmbulatorioModel()
        {
            Lista = new List<string>();
        }

    }
}