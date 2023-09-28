using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Extensions;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using DFe.Utils;
using NFe.Classes.Servicos.DistribuicaoDFe;
using NFe.Classes.Servicos.DistribuicaoDFe.Schemas;
using NFe.Classes.Servicos.Tipos;
using NFe.Servicos;
using NFe.Servicos.Retorno;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Controladorias.NotasFiscais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.Notifications;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Controladorias.NotasFiscais;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Controladorias
{
    public class NotasFiscaisController : SWMANAGERControllerBase
    {
        //private readonly INotaFiscalAppService _notaFiscalAppService;
        ////private readonly INotaFiscalManifestacaoDestinatarioAppService _notaFiscalManifestacaoDestinatarioAppService;
        //private readonly IUserAppService _userAppService;
        private string arquivoConfiguracao = @"\config_02746015000129.xml"; //criar rotina para buscar pelo cnpj da empresa
        //private const string TituloErro = "Erro";
        //private ConfiguracaoApp _configuracoes = new ConfiguracaoApp();
        //private NFe.Classes.NFe _nfe = new NFe.Classes.NFe();
        //private readonly string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        //private readonly IAppNotifier _appNotifier;
        private int notasProcessadas = 0;
        private List<string> listNotasProcessadas = new List<string>();
        private string checkNota = string.Empty;
        ////TESTE
        //private readonly IEmpresaAppService _empresaAppService;
        NotaFiscal notaFiscal;

        //public NotasFiscaisController(
        //    INotaFiscalAppService notaFiscalAppService,
        //    IAppNotifier appNotifier,
        //    IUserAppService userAppService,
        //    IEmpresaAppService empresaAppService)
        //{
        //    _notaFiscalAppService = notaFiscalAppService;
        //    _appNotifier = appNotifier;
        //    _userAppService = userAppService;
        //    _empresaAppService = empresaAppService;
        //}

        // GET: Mpa/Agendamentos
        public async Task<ActionResult> Index()
        {
            using (var userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            using (var empresaAppService = IocManager.Instance.ResolveAsDisposable<IEmpresaAppService>())
            {
                var userId = AbpSession.UserId.Value;
                var userEmpresas = await userAppService.Object.GetUserEmpresas(userId).ConfigureAwait(false);
                //var empresa = TempData.Peek("Empresa") as CriarOuEditarEmpresa;

                //TESTE
                ListResultDto<EmpresaDto> empresas = await userAppService.Object.GetUserEmpresas(AbpSession.UserId.Value).ConfigureAwait(false);


                if (empresas == null || empresas.Items.Count == 0)
                {
                    empresas = await empresaAppService.Object.ListarTodos().ConfigureAwait(false);
                }
                //FIM TESTE

                var viewModel = new NotasFiscaisViewModel
                {
                    UserName = "SW",
                    Password = "1234",
                    //TESTE
                    Empresas = new SelectList(empresas.Items.Select(m => new { m.Id, m.NomeFantasia }), "Id", "NomeFantasia")
                };

                //if (empresa == null || (empresa != null && empresa.Id == 0))
                //{
                //    viewModel.Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia");
                //}
                //else
                //{
                //    viewModel.Empresas = new SelectList(userEmpresas.Items, "Id", "NomeFantasia", empresa.Id);
                //}
                return View("~/Areas/Mpa/Views/Aplicacao/Controladorias/NotasFiscais/Index.cshtml", viewModel);
            }
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            //if (id.HasValue)
            //{
            //    var output = await _notaFiscalAppService.Obter((long)id);
            //    var _notaStr = FuncoesXml.ObterNodeDeStringXml("NFe", output.XmlNota);
            //    var _output = FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(_notaStr);
            //    var nota = _output.infNFe;
            //    TempData["NotaFiscalDetalhe"] = nota.det;
            //    TempData["NotaFiscalTotal"] = nota.total;
            //    var viewModel = new CriarOuEditarModalViewModel()
            //    {
            //        Ambiente = output.Ambiente,
            //        AutXml = output.AutXml,
            //        Avulsa = output.Avulsa,
            //        Cana = output.Cana,
            //        ChaveAcesso = output.ChaveAcesso,
            //        Cnpj = output.Cnpj,
            //        Cobranca = output.Cobranca,
            //        Compra = output.Compra,
            //        Cpf = output.Cpf,
            //        CreationTime = output.CreationTime,
            //        CreatorUserId = output.CreatorUserId,
            //        CStat = output.CStat,
            //        DataEmissao = output.DataEmissao,
            //        DataRecebimento = output.DataRecebimento,
            //        DeleterUserId = output.DeleterUserId,
            //        DeletionTime = output.DeletionTime,
            //        Destinatario = output.Destinatario,
            //        DetalhesNota = output.DetalhesNota,
            //        DigitoValidacao = output.DigitoValidacao,
            //        Emitente = output.Emitente,
            //        Entrega = output.Entrega,
            //        Exporta = output.Exporta,
            //        Id = output.Id,
            //        Ide = output.Ide,
            //        InformacaoAdicional = output.InformacaoAdicional,
            //        InscricaoEstadual = output.InscricaoEstadual,
            //        IsDeleted = output.IsDeleted,
            //        IsSistema = output.IsSistema,
            //        LastModificationTime = output.LastModificationTime,
            //        LastModifierUserId = output.LastModifierUserId,
            //        Modelo = output.Modelo,
            //        Motivo = output.Motivo,
            //        Nome = output.Nome,
            //        Nsu = output.Nsu,
            //        Numero = output.Numero,
            //        NumeroProtocolo = output.NumeroProtocolo,
            //        Pagamento = output.Pagamento,
            //        ProxyDataEmissao = output.ProxyDataEmissao,
            //        Retirada = output.Retirada,
            //        Schema = output.Schema,
            //        Serie = output.Serie,
            //        Situacao = output.Situacao,
            //        TipoNota = output.TipoNota,
            //        TotalNota = output.TotalNota,
            //        ValorNota = output.ValorNota,
            //        VersaoAplicacao = output.VersaoAplicacao,
            //        VersaoNota = output.VersaoNota,
            //        VersaoServico = output.VersaoServico,
            //        XmlNota = output.XmlNota
            //    };

            //    TempData["NotaFiscalViewModel"] = viewModel;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Controladorias/NotasFiscais/_CriarOuEditarModal.cshtml");
            //}
            //else
            //{
            //    throw new UserFriendlyException(L("NotFound"));
            //}
        }

        public PartialViewResult NotaFiscalDetalhes(long id)
        {
            //var detalhe = _notaFiscalAppService.ObterDetalhe(id);
            //TempData["NotaFiscalDetalhe"] = detalhe;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Controladorias/NotasFiscais/_NotaFiscalDetalhes.cshtml");
        }

        public PartialViewResult NotaFiscalTotais(long id)
        {
            //var total = _notaFiscalAppService.ObterTotal(id);
            //TempData["NotaFiscalTotal"] = total;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Controladorias/NotasFiscais/_NotaFiscalTotais.cshtml");
        }
        //public JsonResult Sincronizar(string user, string pass)
        //{
        //    var requestData = "encode=true&cnpj=02746015000129&grupo=AMERICAN&arquivo=" + HttpUtility.UrlEncode("documento=dfe\nnsu=0\ntiponsu=0");

        //    string Result;
        //    //Cria a rota para o servidor da requisição
        //    WebRequest request = WebRequest.Create("https://managersaas.tecnospeed.com.br:8081/ManagerAPIWeb/nfe/envia");
        //    //Define o formato da requisição
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    //Monta a hash de autorização
        //    byte[] credentials = new UTF8Encoding().GetBytes(user + ":" + pass);
        //    //Converte a hash de autorização para o header da requisição
        //    request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentials);
        //    //Define o método da requisição
        //    request.Method = "POST";
        //    //Encoda o conteúdo da requisição em um array de bytes
        //    byte[] byteArray = Encoding.UTF8.GetBytes(requestData);
        //    //Define o tamanho da requisição
        //    request.ContentLength = byteArray.Length;
        //    Stream dataStream = request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
        //    //Captura a resposta da requisição
        //    WebResponse response = request.GetResponse();
        //    dataStream = response.GetResponseStream();
        //    //Implementa um TextReader que lê caracteres de um fluxo de bytes em uma codificação específica.
        //    StreamReader reader = new StreamReader(dataStream);
        //    Result = reader.ReadToEnd();
        //    reader.Close();
        //    dataStream.Close();
        //    response.Close();

        //    return Json(Result, JsonRequestBehavior.AllowGet);

        //    //var client = new RestClient("https://managersaas.tecnospeed.com.br:8081/ManagerAPIWeb/nfe/envia"); //?cnpj=02746015000129&grupo=AMERICAN&arquivo=documento%253Ddfe%250Ansu%253D0%250Atiponsu%253D0");
        //    //var request = new RestRequest(Method.POST);
        //    //Parameter p = new Parameter();
        //    //p.Name = "cnpj";
        //    //p.Value = "02746015000129";
        //    //request.AddParameter(p);
        //    //p = new Parameter();
        //    //p.Name = "grupo";
        //    //p.Value = "AMERICAN";
        //    //request.AddParameter(p);
        //    //p = new Parameter();
        //    //p.Name = "arquivo";
        //    //p.Value = "documento%253Ddfe%250Ansu%253D0%250Atiponsu%253D0";
        //    //request.AddParameter(p);
        //    //request.AddHeader("cache-control", "no-cache");
        //    //var auth = FuncoesGlobais.encode(user + ":" + pass);
        //    //request.AddHeader("authorization", "Basic " + auth);            
        //    //IRestResponse response = client.Execute(request);
        //    //return Json(response.Content, "application/json; charset=utf-8", JsonRequestBehavior.AllowGet);
        //}

        public async Task<ContentResult> NfeDistribuicaoDfe()
        {
            using (var appNotifier = IocManager.Instance.ResolveAsDisposable<IAppNotifier>())
            {
                try
                {
                    var empresa = TempData.Peek("Empresa") as EmpresaDto;
                    if (empresa == null || (empresa != null && empresa.Id == 0))
                    {
                        throw new UserFriendlyException("EmpresaNaoSelecionada");
                    }
                    var arquivoConfiguracao = $@"\config_{empresa.Cnpj}.xml";

                    var configuracoes = this.CarregarConfiguracao();
                    retDistDFeInt retConsulta = null;
                    //CarregaDadosCertificado();
                    using (var service = new ServicosNFe(configuracoes.CfgServico))
                    {
                        var max = "";
                        var result = service.NfeDistDFeInteresse(configuracoes.EnderecoEmitente.UF.ToString(), configuracoes.Emitente.CNPJ, max.ToString());
                        retConsulta = result.Retorno;
                    }

                    var ultNsu = retConsulta.ultNSU;
                    var maxNsu = retConsulta.maxNSU;
                    //var notasProcessadas = 0; //retConsulta.cStat;
                    //do
                    //{
                    var mensagemSuccess = "Success".ToPascalCase(CultureInfo.InvariantCulture).ToEnum<NotificationSeverity>();
                    var sincronizacaoConcluida = L("SincronizacaoConcluida") + " " + notasProcessadas + " " + L("NotasProcessadas");
                    if (retConsulta.loteDistDFeInt != null && retConsulta.loteDistDFeInt.Length > 0)
                    {
                        var listResNFe = new List<resNFe>();
                        var listProcEventoNFe = new List<procEventoNFe>();
                        for (int i = 0; i < retConsulta.loteDistDFeInt.Length; i++)
                        {
                            string conteudo = Compressao.Unzip(retConsulta.loteDistDFeInt[i].XmlNfe);
                            conteudo = conteudo.Replace("ISENTO", "0");
                            //if ((conteudo.StartsWith("<retNFe") || conteudo.StartsWith("<resNFe")) && !conteudo.Contains("<NFe"))
                            if (conteudo.Contains("<chNFe") && !conteudo.Contains("<NFe"))
                            {
                                //var retConteudo =
                                //    FuncoesXml.XmlStringParaClasse<NFe.Classes.Servicos.DistribuicaoDFe.Schemas.resNFe>(conteudo);
                                //chNFe = retConteudo.chNFe;
                                //listResNFe.Add(FuncoesXml.XmlStringParaClasse<resNFe>(conteudo));
                                //var nota = FuncoesXml.XmlStringParaClasse<resNFe>(conteudo);
                                //var chNFe = nota.chNFe;
                                var nota = FuncoesXml.ObterNodeDeStringXml("chNFe", conteudo);
                                var xml = new XmlDocument();
                                xml.LoadXml(nota);
                                var chNFeXml = xml.ChildNodes[0];
                                var chNFe = chNFeXml.InnerText;
                                var nfe = ObterDetalhesNota(chNFe);
                                if (!chNFe.IsIn(listNotasProcessadas.ToArray()))
                                {
                                    //await ProcessarNota(nfe, chNFe, conteudo, retConsulta, i);
                                }
                                //notasProcessadas++;
                            }
                            else if (conteudo.Contains("<NFe"))
                            {
                                var _notaStr = FuncoesXml.ObterNodeDeStringXml("NFe", conteudo);
                                if (!_notaStr.IsNullOrWhiteSpace())
                                {
                                    var _notaXml = FuncoesXml.XmlStringParaClasse<NFe.Classes.NFe>(_notaStr);

                                    string chNFe = string.Empty;
                                    var nfe = new NFe.Classes.nfeProc();
                                    chNFe = _notaXml.infNFe.Id.Substring(3);  // nota.chNFe;
                                    if (_notaXml.infNFe.det.Count() == 0)
                                    {
                                        nfe = ObterDetalhesNota(chNFe);
                                    }
                                    else
                                    {
                                        nfe.NFe = _notaXml; //FuncoesXml.XmlStringParaClasse<NFe.Classes.nfeProc>(conteudo);
                                    }

                                    checkNota = listNotasProcessadas.Where(m => m.Contains(chNFe)).FirstOrDefault();
                                    if (checkNota == null)
                                    //if (!chNFe.IsIn(listNotasProcessadas.ToArray()))
                                    {
                                        //await ProcessarNota(nfe, chNFe, conteudo, retConsulta, i);
                                    }

                                    //notasProcessadas++;
                                    //}
                                    //else if (conteudo.StartsWith("<procEventoNFe"))
                                    //{
                                    //    //var procEventoNFeConteudo =
                                    //    //    FuncoesXml.XmlStringParaClasse<NFe.Classes.Servicos.DistribuicaoDFe.Schemas.procEventoNFe>(conteudo);
                                    //    //chNFe = procEventoNFeConteudo.retEvento.infEvento.chNFe;
                                    //    listProcEventoNFe.Add(FuncoesXml.XmlStringParaClasse<procEventoNFe>(conteudo));
                                    //    notasProcessadas++;
                                    //}
                                    //else
                                    //{
                                    //    var _result = FuncoesXml.XmlStringParaClasse<nfeProc>(conteudo);
                                    //    var test = _result.NFe;
                                    //}
                                }
                            }
                        }
                        if (listProcEventoNFe.Count > 0)
                        {
                            foreach (var proc in listProcEventoNFe)
                            {

                            }

                        }
                    }
                    else
                    {
                        await appNotifier.Object.SendMessageAsync(AbpSession.ToUserIdentifier(), sincronizacaoConcluida, mensagemSuccess).ConfigureAwait(false);
                        return this.Content(L("SincronizacaoConcluida"));
                    }
                    using (var service = new ServicosNFe(configuracoes.CfgServico))
                    {
                        var result = service.NfeDistDFeInteresse(configuracoes.EnderecoEmitente.UF.ToString(), configuracoes.Emitente.CNPJ, ultNsu.ToString());

                        retConsulta = result.Retorno;
                    }
                    if (retConsulta.ultNSU >= ultNsu)
                    {
                        await appNotifier.Object.SendMessageAsync(AbpSession.ToUserIdentifier(), sincronizacaoConcluida, mensagemSuccess).ConfigureAwait(false);
                        return this.Content(L("SincronizacaoParcialConcluida"));
                    }
                    //notasProcessadas += retConsulta.cStat;
                    ultNsu = retConsulta.ultNSU;
                    maxNsu = retConsulta.maxNSU;

                    //} while (notasProcessadas < 100); //(ultNsu <= maxNsu);                
                    await appNotifier.Object.SendMessageAsync(AbpSession.ToUserIdentifier(), sincronizacaoConcluida, mensagemSuccess).ConfigureAwait(false);
                    return this.Content(L("SincronizacaoConcluida"));
                }
                catch (Exception ex)
                {
                    var mensagemFalha = L("FalhaSincronizacao") + " " + ex.Message.ToString();
                    var mensagemErro = "Error".ToPascalCase(CultureInfo.InvariantCulture).ToEnum<NotificationSeverity>();
                    await appNotifier.Object.SendMessageAsync(AbpSession.ToUserIdentifier(), mensagemFalha, mensagemErro).ConfigureAwait(false);
                    return this.Content(L("FalhaSincronizacao"));
                }
            }

        }

        //private async Task ProcessarNota(NFe.Classes.nfeProc nfe, string chNFe, string conteudo, retDistDFeInt retConsulta, int i)
        //{
        //    try
        //    {
        //        long numeroNota = 0;
        //        long modeloNota = 0;
        //        long serieNota = 0;
        //        modeloNota = Convert.ToInt64(chNFe.Substring(20, 2));
        //        serieNota = Convert.ToInt64(chNFe.Substring(22, 3));
        //        numeroNota = Convert.ToInt64(chNFe.Substring(25, 9));
        //        notaFiscal = new NotaFiscal();
        //        notaFiscal = await _notaFiscalAppService.Obter(chNFe);
        //        if (notaFiscal == null)
        //        {
        //            notaFiscal = new NotaFiscal();
        //        }

        //        notaFiscal.XmlNota = conteudo;
        //        if (nfe.NFe != null && nfe.NFe.infNFe != null)
        //        {
        //            var infNfe = nfe.NFe.infNFe;
        //            if (infNfe.autXML != null && infNfe.autXML.Count > 0)
        //            {
        //                var autXmls = new List<NotaFiscalautXML>();
        //                foreach (var xml in infNfe.autXML)
        //                {
        //                    autXmls.Add(
        //                        new NotaFiscalautXML
        //                        {
        //                            CNPJ = xml.CNPJ,
        //                            CPF = xml.CPF
        //                        }
        //                    );
        //                    notaFiscal.AutXml = autXmls;
        //                }
        //            }
        //            if (infNfe.avulsa != null)
        //            {
        //                notaFiscal.Avulsa = new NotaFiscalAvulsa
        //                {
        //                    CNPJ = infNfe.avulsa.CNPJ,
        //                    dEmi = infNfe.avulsa.dEmi,
        //                    dPag = infNfe.avulsa.dPag,
        //                    fone = infNfe.avulsa.fone,
        //                    matr = infNfe.avulsa.matr,
        //                    nDAR = infNfe.avulsa.nDAR,
        //                    repEmi = infNfe.avulsa.repEmi,
        //                    UF = infNfe.avulsa.UF,
        //                    vDAR = infNfe.avulsa.vDAR,
        //                    xAgente = infNfe.avulsa.xAgente,
        //                    xOrgao = infNfe.avulsa.xOrgao
        //                };
        //            }
        //            if (infNfe.cana != null)
        //            {
        //                var cana = new NotaFiscalcana();
        //                if (infNfe.cana.deduc != null && infNfe.cana.deduc.Count > 0)
        //                {
        //                    var deducs = new List<NotaFiscalcanadeduc>();
        //                    foreach (var item in infNfe.cana.deduc)
        //                    {
        //                        deducs.Add(new NotaFiscalcanadeduc
        //                        {
        //                            vDed = item.vDed,
        //                            vFor = item.vFor,
        //                            vLiqFor = item.vLiqFor,
        //                            vTotDed = item.vTotDed,
        //                            xDed = item.xDed
        //                        });
        //                    }
        //                    cana.deduc = deducs;
        //                }
        //                if (infNfe.cana.forDia != null && infNfe.cana.forDia.Count > 0)
        //                {
        //                    var forDias = new List<NotaFiscalcanaforDia>();
        //                    foreach (var item in infNfe.cana.forDia)
        //                    {
        //                        forDias.Add(new NotaFiscalcanaforDia
        //                        {
        //                            dia = item.dia,
        //                            qtde = item.qtde,
        //                            qTotAnt = item.qTotAnt,
        //                            qTotGer = item.qTotGer,
        //                            qTotMes = item.qTotMes
        //                        });
        //                    }
        //                    cana.forDia = forDias;
        //                }
        //                cana.@ref = infNfe.cana.@ref;
        //                cana.safra = infNfe.cana.safra;
        //                notaFiscal.Cana = cana;
        //            }
        //            if (infNfe.cobr != null)
        //            {
        //                var cobr = new NotaFiscalCobranca();
        //                if (infNfe.cobr.dup != null && infNfe.cobr.dup.Count > 0)
        //                {
        //                    cobr.dup = new List<NotaFiscalCobrancaDuplicata>();
        //                    foreach (var item in infNfe.cobr.dup)
        //                    {
        //                        cobr.dup.Add(
        //                            new NotaFiscalCobrancaDuplicata
        //                            {
        //                                dVenc = item.dVenc,
        //                                nDup = item.nDup,
        //                                vDup = item.vDup
        //                            });
        //                    }
        //                }
        //                //cobr.dup = infNfe.cobr.dup;
        //                if (infNfe.cobr.fat != null)
        //                {
        //                    cobr.fat = new NotaFiscalCobrancaFatura
        //                    {
        //                        nFat = infNfe.cobr.fat.nFat,
        //                        vDesc = infNfe.cobr.fat.vDesc,
        //                        vLiq = infNfe.cobr.fat.vLiq,
        //                        vOrig = infNfe.cobr.fat.vOrig
        //                    };
        //                }
        //                notaFiscal.Cobranca = cobr;
        //            }
        //            if (infNfe.compra != null)
        //            {
        //                notaFiscal.Compra = new NotaFiscalcompra
        //                {
        //                    xCont = infNfe.compra.xCont,
        //                    xNEmp = infNfe.compra.xNEmp,
        //                    xPed = infNfe.compra.xPed
        //                };
        //            }
        //            if (infNfe.dest != null)
        //            {
        //                VersaoServico versao;
        //                switch (infNfe.versao)
        //                {
        //                    case "1.0":
        //                    case "100":
        //                        versao = VersaoServico.ve100;
        //                        break;
        //                    case "2.0":
        //                    case "200":
        //                        versao = VersaoServico.ve200;
        //                        break;
        //                    default:
        //                        versao = VersaoServico.ve310;
        //                        break;
        //                }
        //                notaFiscal.Destinatario = new NotaFiscalDestinatario(versao)
        //                {
        //                    CNPJ = infNfe.dest.CNPJ,
        //                    CPF = infNfe.dest.CPF,
        //                    email = infNfe.dest.email,
        //                    idEstrangeiro = infNfe.dest.idEstrangeiro,
        //                    IE = infNfe.dest.IE,
        //                    IM = infNfe.dest.IM,
        //                    indIEDest = infNfe.dest.indIEDest,
        //                    ISUF = infNfe.dest.ISUF,
        //                    xNome = infNfe.dest.xNome
        //                };
        //                if (infNfe.dest.enderDest != null)
        //                {
        //                    var ender = infNfe.dest.enderDest;
        //                    notaFiscal.Destinatario.enderDest = new NotaFiscalEnderecoDestinatario
        //                    {
        //                        CEP = ender.CEP,
        //                        cMun = ender.cMun,
        //                        cPais = ender.cPais,
        //                        fone = ender.fone,
        //                        nro = ender.nro,
        //                        UF = ender.UF,
        //                        xBairro = ender.xBairro,
        //                        xCpl = ender.xCpl,
        //                        xLgr = ender.xLgr,
        //                        xMun = ender.xMun,
        //                        xPais = ender.xPais
        //                    };
        //                }
        //            }
        //            if (infNfe.det != null && infNfe.det.Count > 0)
        //            {
        //                var detalhes = new List<NotaFiscalDetalhe>();
        //                foreach (var item in infNfe.det)
        //                {
        //                    var detalhe = new NotaFiscalDetalhe();
        //                    if (item.imposto != null)
        //                    {
        //                        var imposto = new NotaFiscalImposto();
        //                        if (item.imposto.COFINS != null)
        //                        {
        //                            var cofins = new NotaFiscalCOFINS();
        //                            foreach (var propriedade in item.imposto.COFINS.TipoCOFINS.LerPropriedades())
        //                            {
        //                                if (propriedade.Key.Equals("COFINSAliq"))
        //                                {
        //                                    var tipoCOFINS = propriedade.Value.As<COFINSAliq>();
        //                                    cofins.TipoCOFINS = new NotaFiscalCOFINSAliq
        //                                    {
        //                                        CST = tipoCOFINS.CST,
        //                                        pCOFINS = tipoCOFINS.pCOFINS,
        //                                        vBC = tipoCOFINS.vBC,
        //                                        vCOFINS = tipoCOFINS.vCOFINS
        //                                    };
        //                                    break;
        //                                }
        //                                if (propriedade.Key.Equals("COFINSQtde"))
        //                                {
        //                                    var tipoCOFINS = propriedade.Value.As<COFINSQtde>();
        //                                    cofins.TipoCOFINS = new NotaFiscalCOFINSQtde
        //                                    {

        //                                        CST = tipoCOFINS.CST,
        //                                        qBCProd = tipoCOFINS.qBCProd,
        //                                        vAliqProd = tipoCOFINS.vAliqProd,
        //                                        vCOFINS = tipoCOFINS.vCOFINS
        //                                    };
        //                                    break;
        //                                }
        //                                if (propriedade.Key.Equals("COFINSNT"))
        //                                {
        //                                    var tipoCOFINS = propriedade.Value.As<COFINSNT>();
        //                                    cofins.TipoCOFINS = new NotaFiscalCOFINSNT
        //                                    {

        //                                        CST = tipoCOFINS.CST
        //                                    };
        //                                    break;
        //                                }
        //                                if (propriedade.Key.Equals("COFINSOutr"))
        //                                {
        //                                    var tipoCOFINS = propriedade.Value.As<COFINSOutr>();
        //                                    cofins.TipoCOFINS = new NotaFiscalCOFINSOutr
        //                                    {
        //                                        CST = tipoCOFINS.CST,
        //                                        pCOFINS = tipoCOFINS.pCOFINS,
        //                                        qBCProd = tipoCOFINS.qBCProd,
        //                                        vAliqProd = tipoCOFINS.vAliqProd,
        //                                        vBC = tipoCOFINS.vBC,
        //                                        vCOFINS = tipoCOFINS.vCOFINS
        //                                    };
        //                                    break;
        //                                }
        //                            }
        //                            imposto.COFINS = cofins;
        //                        }
        //                        detalhe.imposto = imposto;
        //                    }
        //                    if (item.impostoDevol != null)
        //                    {
        //                        var impostoDevol = new NotaFiscalImpostoDevolvido();
        //                        if (item.impostoDevol.IPI != null)
        //                        {
        //                            impostoDevol.IPI = new NotaFiscalIPIDevolvido
        //                            {
        //                                vIPIDevol = item.impostoDevol.IPI.vIPIDevol
        //                            };
        //                        }
        //                        impostoDevol.pDevol = item.impostoDevol.pDevol;

        //                        detalhe.impostoDevol = impostoDevol;
        //                    }
        //                    if (item.prod != null)
        //                    {
        //                        var prod = new NotaFiscalProduto();
        //                        if (item.prod.detExport != null && item.prod.detExport.Count > 0)
        //                        {
        //                            var detsExpor = new List<NotaFiscaldetExport>();
        //                            foreach (var det in item.prod.detExport)
        //                            {
        //                                var detExpor = new NotaFiscaldetExport();
        //                                var exporInd = new NotaFiscalexportInd()
        //                                {
        //                                    chNFe = det.exportInd.chNFe,
        //                                    nRE = det.exportInd.nRE,
        //                                    qExport = det.exportInd.qExport
        //                                };
        //                                detExpor.exportInd = exporInd;
        //                                detExpor.nDraw = det.nDraw;
        //                            }
        //                            prod.detExport = detsExpor;
        //                        }
        //                        if (item.prod.DI != null && item.prod.DI.Count > 0)
        //                        {
        //                            var dis = new List<NotaFiscalDI>();
        //                            foreach (var di in item.prod.DI)
        //                            {
        //                                var _di = new NotaFiscalDI();
        //                                if (di.adi != null && di.adi.Count > 0)
        //                                {
        //                                    var adis = new List<NotaFiscaladi>();
        //                                    foreach (var adi in di.adi)
        //                                    {
        //                                        var _adi = new NotaFiscaladi();
        //                                        _adi.cFabricante = adi.cFabricante;
        //                                        _adi.nAdicao = adi.nAdicao;
        //                                        _adi.nDraw = adi.nDraw;
        //                                        _adi.nSeqAdic = _adi.nSeqAdic;
        //                                        _adi.vDescDI = _adi.vDescDI;
        //                                        adis.Add(_adi);
        //                                    }
        //                                    _di.adi = adis;
        //                                }

        //                                _di.cExportador = di.cExportador;
        //                                _di.CNPJ = di.CNPJ;
        //                                _di.nDI = di.nDI;
        //                                if (di.dDesemb != null)
        //                                {
        //                                    _di.dDesemb = di.dDesemb;
        //                                }
        //                                if (di.dDI != null)
        //                                {
        //                                    _di.dDI = di.dDI;
        //                                }
        //                                if (di.ProxydDesemb != null)
        //                                {
        //                                    _di.ProxydDesemb = di.ProxydDesemb;
        //                                }
        //                                if (di.ProxydDI != null)
        //                                {
        //                                    _di.ProxydDI = di.ProxydDI;
        //                                }
        //                                _di.tpIntermedio = di.tpIntermedio;
        //                                _di.tpViaTransp = di.tpViaTransp;
        //                                _di.UFDesemb = di.UFDesemb;
        //                                _di.UFTerceiro = di.UFTerceiro;
        //                                _di.vAFRMM = di.vAFRMM;
        //                                _di.xLocDesemb = di.xLocDesemb;

        //                                dis.Add(_di);

        //                            }
        //                            prod.DI = dis;
        //                        }
        //                        if (item.prod.ProdutoEspecifico != null)
        //                        {
        //                            foreach (var propriedade in item.prod.ProdutoEspecifico.LerPropriedades())
        //                            {
        //                                if (propriedade.Key.Equals("veicProd"))
        //                                {
        //                                    var esp = propriedade.Value.As<veicProd>();
        //                                    prod.ProdutoEspecifico = new NotaFiscalveicProd
        //                                    {
        //                                        anoFab = esp.anoFab,
        //                                        anoMod = esp.anoMod,
        //                                        cCor = esp.cCor,
        //                                        cCorDENATRAN = esp.cCorDENATRAN,
        //                                        chassi = esp.chassi,
        //                                        cilin = esp.cilin,
        //                                        cMod = esp.cMod,
        //                                        CMT = esp.CMT,
        //                                        condVeic = esp.condVeic,
        //                                        dist = esp.dist,
        //                                        espVeic = esp.espVeic,
        //                                        lota = esp.lota,
        //                                        nMotor = esp.nMotor,
        //                                        nSerie = esp.nSerie,
        //                                        pesoB = esp.pesoB,
        //                                        pesoL = esp.pesoL,
        //                                        pot = esp.pot,
        //                                        tpComb = esp.tpComb,
        //                                        tpOp = esp.tpOp,
        //                                        tpPint = esp.tpPint,
        //                                        tpRest = esp.tpRest,
        //                                        tpVeic = esp.tpVeic,
        //                                        VIN = esp.VIN,
        //                                        xCor = esp.xCor
        //                                    };
        //                                    break;
        //                                }
        //                                if (propriedade.Key.Equals("med"))
        //                                {
        //                                    var esp = propriedade.Value.As<med>();
        //                                    var med = new NotaFiscalmed
        //                                    {
        //                                        nLote = esp.nLote,
        //                                        qLote = esp.qLote,
        //                                        vPMC = esp.vPMC
        //                                    };
        //                                    if (esp.dFab != null)
        //                                    {
        //                                        med.dFab = esp.dFab;
        //                                    }
        //                                    if (esp.dVal != null)
        //                                    {
        //                                        med.dVal = esp.dVal;
        //                                    }
        //                                    if (esp.ProxydFab != null)
        //                                    {
        //                                        med.ProxydFab = esp.ProxydFab;
        //                                    }
        //                                    if (med.ProxydVal != null)
        //                                    {
        //                                        med.ProxydVal = esp.ProxydVal;
        //                                    }

        //                                    prod.ProdutoEspecifico = med;

        //                                    break;
        //                                }
        //                                if (propriedade.Key.Equals("arma"))
        //                                {
        //                                    var esp = propriedade.Value.As<arma>();
        //                                    prod.ProdutoEspecifico = new NotaFiscalarma
        //                                    {
        //                                        descr = esp.descr,
        //                                        nCano = esp.nCano,
        //                                        nSerie = esp.nSerie,
        //                                        tpArma = esp.tpArma
        //                                    };
        //                                    break;
        //                                }
        //                                if (propriedade.Key.Equals("comb"))
        //                                {
        //                                    var esp = propriedade.Value.As<comb>();
        //                                    var comb = new NotaFiscalcomb
        //                                    {
        //                                        CODIF = esp.CODIF,
        //                                        cProdANP = esp.cProdANP,
        //                                        pMixGN = esp.pMixGN,
        //                                        qTemp = esp.qTemp,
        //                                        UFCons = esp.UFCons
        //                                    };

        //                                    if (esp.CIDE != null)
        //                                    {
        //                                        var cide = new NotaFiscalCIDE
        //                                        {
        //                                            qBCProd = esp.CIDE.qBCProd,
        //                                            vAliqProd = esp.CIDE.vAliqProd,
        //                                            vCIDE = esp.CIDE.vCIDE
        //                                        };
        //                                        comb.CIDE = cide;
        //                                    }
        //                                    if (esp.encerrante != null)
        //                                    {
        //                                        var enc = new NotaFiscalEncerrante
        //                                        {
        //                                            nBico = esp.encerrante.nBico,
        //                                            nBomba = esp.encerrante.nBomba,
        //                                            nTanque = esp.encerrante.nTanque,
        //                                            vEncFin = esp.encerrante.vEncFin,
        //                                            vEncIni = esp.encerrante.vEncIni
        //                                        };
        //                                        comb.encerrante = enc;
        //                                    }

        //                                    prod.ProdutoEspecifico = comb;
        //                                    break;
        //                                }
        //                            }

        //                        }
        //                        prod.NVE = item.prod.NVE;
        //                        prod.cEAN = item.prod.cEAN;
        //                        prod.cEANTrib = item.prod.cEANTrib;
        //                        prod.CEST = item.prod.CEST;
        //                        prod.CFOP = item.prod.CFOP;
        //                        prod.cProd = item.prod.cProd;
        //                        prod.EXTIPI = item.prod.EXTIPI;
        //                        prod.indTot = item.prod.indTot;
        //                        prod.NCM = item.prod.NCM;
        //                        prod.nFCI = item.prod.nFCI;
        //                        prod.nItemPed = item.prod.nItemPed;
        //                        prod.nRECOPI = item.prod.nRECOPI;
        //                        prod.qCom = item.prod.qCom;
        //                        prod.qTrib = item.prod.qTrib;
        //                        prod.uCom = item.prod.uCom;
        //                        prod.uTrib = item.prod.uTrib;
        //                        prod.vDesc = item.prod.vDesc;
        //                        prod.vFrete = item.prod.vFrete;
        //                        prod.vOutro = item.prod.vOutro;
        //                        prod.vProd = item.prod.vProd;
        //                        prod.vSeg = item.prod.vSeg;
        //                        prod.vUnCom = item.prod.vUnCom;
        //                        prod.vUnTrib = item.prod.vUnTrib;
        //                        prod.xPed = item.prod.xPed;
        //                        prod.xProd = item.prod.xProd;
        //                        detalhe.prod = prod;
        //                    }
        //                    detalhe.infAdProd = item.infAdProd;
        //                    detalhe.nItem = item.nItem;
        //                    detalhes.Add(detalhe);
        //                }
        //                notaFiscal.DetalhesNota = detalhes;
        //            }
        //            if (infNfe.emit != null)
        //            {
        //                var emit = infNfe.emit;
        //                var emitente = new NotaFiscalEmitente
        //                {
        //                    CNAE = emit.CNAE,
        //                    CNPJ = emit.CNPJ,
        //                    CPF = emit.CPF,
        //                    CRT = emit.CRT,
        //                    IE = emit.IE,
        //                    IEST = emit.IEST,
        //                    IM = emit.IM,
        //                    xFant = emit.xFant,
        //                    xNome = emit.xNome
        //                };
        //                if (emit.enderEmit != null)
        //                {
        //                    var end = emit.enderEmit;
        //                    emitente.enderEmit = new NotaFiscalEnderecoEmitente
        //                    {
        //                        CEP = end.CEP,
        //                        cMun = end.cMun,
        //                        cPais = end.cPais,
        //                        fone = end.fone,
        //                        nro = end.nro,
        //                        UF = end.UF,
        //                        xBairro = end.xBairro,
        //                        xCpl = end.xCpl,
        //                        xLgr = end.xLgr,
        //                        xMun = end.xMun,
        //                        xPais = end.xPais
        //                    };
        //                }
        //                notaFiscal.Emitente = emitente;
        //            }
        //            if (infNfe.entrega != null)
        //            {
        //                var ent = infNfe.entrega;
        //                var entrega = new NotaFiscalEntrega
        //                {
        //                    cMun = ent.cMun,
        //                    CNPJ = ent.CNPJ,
        //                    CPF = ent.CPF,
        //                    nro = ent.nro,
        //                    UF = ent.UF,
        //                    xBairro = ent.xBairro,
        //                    xCpl = ent.xCpl,
        //                    xLgr = ent.xLgr,
        //                    xMun = ent.xMun
        //                };
        //                notaFiscal.Entrega = entrega;
        //            }
        //            if (infNfe.exporta != null)
        //            {
        //                var exp = infNfe.exporta;
        //                var exporta = new NotaFiscalexporta
        //                {
        //                    UFSaidaPais = exp.UFSaidaPais,
        //                    xLocDespacho = exp.xLocDespacho,
        //                    xLocExporta = exp.xLocExporta
        //                };
        //                notaFiscal.Exporta = exporta;
        //            }
        //            if (infNfe.ide != null)
        //            {
        //                var d = infNfe.ide;
        //                var ide = new NotaFiscalIdentificacao
        //                {
        //                    cDV = d.cDV,
        //                    cMunFG = d.cMunFG,
        //                    cNF = d.cNF,
        //                    cUF = d.cUF,
        //                    finNFe = d.finNFe,
        //                    idDest = d.idDest,
        //                    indFinal = d.indFinal,
        //                    indPag = d.indPag,
        //                    indPres = d.indPres,
        //                    mod = d.mod,
        //                    natOp = d.natOp,
        //                    nNF = d.nNF,
        //                    procEmi = d.procEmi,
        //                    serie = d.serie,
        //                    tpAmb = d.tpAmb,
        //                    tpEmis = d.tpEmis,
        //                    tpImp = d.tpImp,
        //                    tpNF = d.tpNF,
        //                    verProc = d.verProc,
        //                    xJust = d.xJust
        //                };
        //                if (!d.ProxydEmi.IsNullOrEmpty())
        //                {
        //                    ide.ProxydEmi = d.ProxydEmi;
        //                }
        //                if (!d.ProxyDhEmi.IsNullOrEmpty())
        //                {
        //                    ide.ProxyDhEmi = d.ProxyDhEmi;
        //                }
        //                if (!d.ProxydhCont.IsNullOrEmpty())
        //                {
        //                    ide.ProxydhCont = d.ProxydhCont;
        //                }
        //                if (!d.ProxydhSaiEnt.IsNullOrEmpty())
        //                {
        //                    ide.ProxydhSaiEnt = d.ProxydhSaiEnt;
        //                }
        //                if (!d.ProxydSaiEnt.IsNullOrEmpty())
        //                {
        //                    ide.ProxydSaiEnt = d.ProxydSaiEnt;
        //                }
        //                if (d.dEmi != null)
        //                {
        //                    ide.dEmi = d.dEmi;
        //                }
        //                if (d.dhCont != null)
        //                {
        //                    ide.dhCont = d.dhCont;
        //                }
        //                if (d.dhEmi != null)
        //                {
        //                    ide.dhEmi = d.dhEmi;
        //                }
        //                if (d.dhSaiEnt != null)
        //                {
        //                    ide.dhSaiEnt = d.dhSaiEnt;
        //                }
        //                if (d.dSaiEnt != null)
        //                {
        //                    ide.dSaiEnt = d.dSaiEnt;
        //                }



        //                if (d.NFref != null && d.NFref.Count > 0)
        //                {
        //                    var nfrefs = new List<NotaFiscalNFref>();
        //                    foreach (var r in d.NFref)
        //                    {
        //                        var nref = new NotaFiscalNFref();

        //                        if (r.refECF != null)
        //                        {
        //                            var ecf = new NotaFiscalrefECF
        //                            {
        //                                mod = r.refECF.mod,
        //                                nCOO = r.refECF.nCOO,
        //                                nECF = r.refECF.nECF

        //                            };
        //                            nref.refECF = ecf;
        //                        }
        //                        if (r.refNF != null)
        //                        {
        //                            var rnf = new NotaFiscalrefNF
        //                            {
        //                                AAMM = r.refNF.AAMM,
        //                                CNPJ = r.refNF.CNPJ,
        //                                cUF = r.refNF.cUF,
        //                                mod = r.refNF.mod,
        //                                nNF = r.refNF.nNF,
        //                                serie = r.refNF.serie
        //                            };
        //                            nref.refNF = rnf;
        //                        }
        //                        if (r.refNFe != null)
        //                        {
        //                            var rnf = new NotaFiscalrefNF();
        //                        }
        //                        nref.refNFe = r.refNFe;
        //                        if (r.refNFP != null)
        //                        {
        //                            var rnf = new NotaFiscalrefNFP
        //                            {
        //                                AAMM = r.refNFP.AAMM,
        //                                CNPJ = r.refNFP.CNPJ,
        //                                CPF = r.refNFP.CPF,
        //                                cUF = r.refNFP.cUF,
        //                                IE = r.refNFP.IE,
        //                                mod = r.refNFP.mod,
        //                                nNF = r.refNFP.nNF,
        //                                serie = r.refNFP.serie
        //                            };
        //                        }
        //                        nfrefs.Add(nref);
        //                    }
        //                    ide.NFref = nfrefs;
        //                }
        //                notaFiscal.Ide = ide;
        //            }
        //            if (infNfe.infAdic != null)
        //            {
        //                var inf = infNfe.infAdic;
        //                var adi = new NotaFiscalInformacaoAdicional
        //                {
        //                    infAdFisco = inf.infAdFisco,
        //                    infCpl = inf.infCpl,
        //                };
        //                if (inf.obsCont != null && inf.obsCont.Count > 0)
        //                {
        //                    var ob = inf.obsCont;
        //                    var ocs = new List<NotaFiscalInformacaoAdicionalobsCont>();
        //                    foreach (var obs in ob)
        //                    {
        //                        ocs.Add(new NotaFiscalInformacaoAdicionalobsCont
        //                        {
        //                            xCampo = obs.xCampo,
        //                            xTexto = obs.xTexto
        //                        });
        //                    }
        //                    adi.obsCont = ocs;
        //                }
        //                if (inf.obsFisco != null && inf.obsFisco.Count > 0)
        //                {
        //                    var of = inf.obsFisco;
        //                    var ofs = new List<NotaFiscalInformacaoAdicionalobsFisco>();
        //                    foreach (var obs in of)
        //                    {
        //                        ofs.Add(new NotaFiscalInformacaoAdicionalobsFisco
        //                        {
        //                            xCampo = obs.xCampo,
        //                            xTexto = obs.xTexto
        //                        });
        //                    }
        //                    adi.obsFisco = ofs;
        //                }
        //                if (inf.procRef != null && inf.procRef.Count > 0)
        //                {
        //                    var pr = inf.procRef;
        //                    var prs = new List<NotaFiscalInformacaoAdicionalprocRef>();
        //                    foreach (var proc in pr)
        //                    {
        //                        prs.Add(new NotaFiscalInformacaoAdicionalprocRef
        //                        {
        //                            indProc = proc.indProc,
        //                            nProc = proc.nProc
        //                        });
        //                    }
        //                    adi.procRef = prs;
        //                }
        //                notaFiscal.InformacaoAdicional = adi;
        //            }
        //            if (infNfe.pag != null && infNfe.pag.Count > 0)
        //            {
        //                var pgtos = new List<NotaFiscalPagamento>();
        //                foreach (var pag in infNfe.pag)
        //                {
        //                    var pgto = new NotaFiscalPagamento
        //                    {
        //                        tPag = pag.tPag,
        //                        vPag = pag.vPag
        //                    };
        //                    if (pag.card != null)
        //                    {
        //                        var card = new NotaFiscalPagamentoCartao
        //                        {
        //                            cAut = pag.card.cAut,
        //                            CNPJ = pag.card.CNPJ,
        //                            tBand = pag.card.tBand,
        //                            tpIntegra = pag.card.tpIntegra
        //                        };
        //                        pgto.card = card;
        //                    }
        //                    pgtos.Add(pgto);
        //                }
        //                notaFiscal.Pagamento = pgtos;
        //            }
        //            if (infNfe.retirada != null)
        //            {
        //                var ret = infNfe.retirada;
        //                notaFiscal.Retirada = new NotaFiscalRetirada
        //                {
        //                    cMun = ret.cMun,
        //                    CNPJ = ret.CNPJ,
        //                    CPF = ret.CPF,
        //                    nro = ret.nro,
        //                    UF = ret.UF,
        //                    xBairro = ret.xBairro,
        //                    xCpl = ret.xCpl,
        //                    xLgr = ret.xLgr,
        //                    xMun = ret.xMun
        //                };
        //            }
        //            if (infNfe.total != null)
        //            {
        //                var tot = infNfe.total;
        //                var tn = new NotaFiscalTotal();
        //                if (tot.ICMSTot != null)
        //                {
        //                    var ic = tot.ICMSTot;
        //                    tn.ICMSTot = new NotaFiscalICMSTot
        //                    {
        //                        vBC = ic.vBC,
        //                        vBCST = ic.vBCST,
        //                        vCOFINS = ic.vCOFINS,
        //                        vDesc = ic.vDesc,
        //                        vFCPUFDest = ic.vFCPUFDest,
        //                        vFrete = ic.vFrete,
        //                        vICMS = ic.vICMS,
        //                        vICMSDeson = ic.vICMSDeson,
        //                        vICMSUFDest = ic.vICMSUFDest,
        //                        vICMSUFRemet = ic.vICMSUFRemet,
        //                        vII = ic.vII,
        //                        vIPI = ic.vIPI,
        //                        vNF = ic.vNF,
        //                        vOutro = ic.vOutro,
        //                        vPIS = ic.vPIS,
        //                        vProd = ic.vProd,
        //                        vSeg = ic.vSeg,
        //                        vST = ic.vST,
        //                        vTotTrib = ic.vTotTrib
        //                    };
        //                }
        //                if (tot.ISSQNtot != null)
        //                {
        //                    var it = tot.ISSQNtot;
        //                    tn.ISSQNtot = new NotaFiscalISSQNtot
        //                    {
        //                        cRegTrib = it.cRegTrib,
        //                        dCompet = it.dCompet,
        //                        vBC = it.vBC,
        //                        vCOFINS = it.vCOFINS,
        //                        vDeducao = it.vDeducao,
        //                        vDescCond = it.vDescCond,
        //                        vDescIncond = it.vDescIncond,
        //                        vISS = it.vISS,
        //                        vISSRet = it.vISSRet,
        //                        vOutro = it.vOutro,
        //                        vPIS = it.vPIS,
        //                        vServ = it.vServ
        //                    };
        //                }
        //                if (tot.retTrib != null)
        //                {
        //                    var ret = tot.retTrib;
        //                    tn.retTrib = new NotaFiscalretTrib
        //                    {
        //                        vBCIRRF = ret.vBCIRRF,
        //                        vBCRetPrev = ret.vBCRetPrev,
        //                        vIRRF = ret.vIRRF,
        //                        vRetCOFINS = ret.vRetCOFINS,
        //                        vRetCSLL = ret.vRetCSLL,
        //                        vRetPIS = ret.vRetPIS,
        //                        vRetPrev = ret.vRetPrev
        //                    };
        //                }
        //                notaFiscal.TotalNota = tn;
        //            }
        //            if (conteudo.Contains("infProt"))
        //            {
        //                var protStr = FuncoesXml.ObterNodeDeStringXml("infProt", conteudo);  //nfe.protNFe.infProt;
        //                XmlDocument myXml = new XmlDocument();
        //                myXml.LoadXml(protStr);
        //                //var prot = FuncoesXml.XmlStringParaClasse<NFe.Classes.Protocolo.infProt>(protXml);
        //                notaFiscal.Ambiente = Convert.ToByte(myXml.GetElementsByTagName("tpAmb")[0].InnerXml); //(byte)prot.tpAmb; //retConsulta.tpAmb;
        //                notaFiscal.CStat = Convert.ToByte(myXml.GetElementsByTagName("cStat")[0].InnerXml); //(byte)prot.cStat; //retConsulta.cStat;
        //                notaFiscal.DataRecebimento = Convert.ToDateTime(myXml.GetElementsByTagName("dhRecbto")[0].InnerXml); //prot.dhRecbto; //notaFiscal.Ide.dhEmi; // nota.dhRecbto;
        //                notaFiscal.DigitoValidacao = Convert.ToString(myXml.GetElementsByTagName("digVal")[0].InnerXml); //prot.digVal; // string.Empty; //nota.digVal;
        //                notaFiscal.NumeroProtocolo = Convert.ToInt64(myXml.GetElementsByTagName("nProt")[0].InnerXml); //Convert.ToInt64(prot.nProt); //0; //_notaXml.infNFe.transp. //(long)nota.nProt;
        //                notaFiscal.Motivo = myXml.GetElementsByTagName("xMotivo")[0].InnerXml;
        //            }
        //            else
        //            {
        //                //                    if (!chNFe.StartsWith("31"))
        //                //                    {
        //                ConsultarSituacaoNotaFiscal(chNFe);
        //                //                    }
        //                //                    else
        //                //{
        //                //    notaFiscal.Ambiente = retConsulta.tpAmb;
        //                //    notaFiscal.CStat = retConsulta.cStat;
        //                //    notaFiscal.DataRecebimento = notaFiscal.Ide.dhEmi; // nota.dhRecbto;
        //                //    notaFiscal.DigitoValidacao = string.Empty; //nota.digVal;
        //                //    notaFiscal.NumeroProtocolo = 0; //_notaXml.infNFe.transp. //(long)nota.nProt;
        //                //    notaFiscal.Motivo = retConsulta.xMotivo;
        //                //}
        //            }
        //            var empresa = TempData.Peek("Empresa") as CriarOuEditarEmpresa;
        //            long ie = 0;
        //            notaFiscal.EmpresaId = empresa.Id;
        //            notaFiscal.XmlNota = FuncoesXml.ClasseParaXmlString<NFe.Classes.nfeProc>(nfe); //conteudo;
        //            notaFiscal.Modelo = modeloNota;
        //            notaFiscal.Serie = serieNota;
        //            notaFiscal.Numero = numeroNota;
        //            notaFiscal.Nsu = (short)retConsulta.loteDistDFeInt[i].NSU;
        //            notaFiscal.Schema = retConsulta.loteDistDFeInt[i].schema;
        //            notaFiscal.VersaoAplicacao = retConsulta.verAplic;
        //            notaFiscal.VersaoServico = retConsulta.versao;
        //            notaFiscal.ChaveAcesso = chNFe; //nota.chNFe;
        //            notaFiscal.Cnpj = notaFiscal.Emitente.CNPJ.IsNullOrWhiteSpace() ? 0 : Convert.ToInt64(notaFiscal.Emitente.CNPJ); // (long)nota.CNPJ;
        //            notaFiscal.Cpf = notaFiscal.Emitente.CPF.IsNullOrWhiteSpace() ? 0 : Convert.ToInt64(notaFiscal.Emitente.CPF); //(long)nota.CPF;
        //            notaFiscal.DataEmissao = notaFiscal.Ide.dhEmi; //nota.dhEmi;
        //            var ieBool = long.TryParse(notaFiscal.Emitente.IE, out ie);
        //            notaFiscal.InscricaoEstadual = ieBool ? ie : 0;// (long)nota.IE;
        //            notaFiscal.Nome = notaFiscal.Emitente.xNome; //nota.xNome;
        //            notaFiscal.ProxyDataEmissao = notaFiscal.Ide.ProxyDhEmi;  // nota.ProxyDhEmi;
        //            notaFiscal.Situacao = 0; // nota.cSitNFe;
        //            notaFiscal.TipoNota = (byte)notaFiscal.Ide.tpNF; //nota.tpNF;
        //            notaFiscal.ValorNota = notaFiscal.TotalNota.ICMSTot.vNF; //nota.vNF;
        //            notaFiscal.VersaoNota = retConsulta.versao; //Convert.ToInt32(notaFiscal.Ide.verProc); //nota.versao;

        //            await _notaFiscalAppService.CriarOuEditar(notaFiscal);
        //            listNotasProcessadas.Add(notaFiscal.ChaveAcesso);
        //            notasProcessadas++;
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public async Task<JsonResult> ConfirmarNota(string chaveAcesso)
        {
            var retorno = string.Empty;
            try
            {
                //var empresa = TempData.Peek("Empresa") as CriarOuEditarEmpresa;
                //if (empresa == null || (empresa != null && empresa.Id == 0))
                //{
                //    throw new UserFriendlyException("EmpresaNaoSelecionada");
                //}
                //arquivoConfiguracao = string.Format(@"\{0}_{1}.xml", "config", empresa.Cnpj);
                //_configuracoes = CarregarConfiguracao();
                ////CarregaDadosCertificado();
                //var service = new ServicosNFe(_configuracoes.CfgServico);
                //var manifest = service.RecepcaoEventoManifestacaoDestinatario(1, 1, chaveAcesso, TipoEventoManifestacaoDestinatario.TeMdConfirmacaoDaOperacao, _configuracoes.Emitente.CNPJ);
                //var resultManifest = manifest.Retorno;
                //var breakPoint = resultManifest.retEvento;
                //service.Dispose();
                //if (!(resultManifest.retEvento.FirstOrDefault().infEvento.cStat.Equals(135) || resultManifest.retEvento.FirstOrDefault().infEvento.cStat.Equals(573)))
                //{
                //    throw new UserFriendlyException(L("ErroManifestacaoDestinatario", resultManifest.xMotivo));
                //}
                //var notaFiscal = await _notaFiscalAppService.Obter(chaveAcesso);
                //var input = new NotaFiscalManifestacaoDestinatarioDto();
                //input.ChaveAcesso = chaveAcesso;
                //input.DataOperacao = resultManifest.retEvento.FirstOrDefault().infEvento.dhRegEvento;
                //input.DescricaoRetorno = resultManifest.retEvento.FirstOrDefault().infEvento.xMotivo;
                //input.DescricaoTipoEvento = resultManifest.retEvento.FirstOrDefault().infEvento.xEvento;
                //input.NotaFiscalId = notaFiscal.Id;
                //input.NumeroProtocolo = resultManifest.retEvento.FirstOrDefault().infEvento.nProt;
                //input.TipoEvento = Convert.ToInt32(resultManifest.retEvento.FirstOrDefault().infEvento.tpEvento);
                //await _notaFiscalManifestacaoDestinatarioAppService.CriarOuEditar(input);
                //notaFiscal.IsManifestacaoDestinatario = true;
                //await _notaFiscalAppService.CriarOuEditar(notaFiscal);
                //retorno = resultManifest.xMotivo;
            }
            catch (Exception ex)
            {
                retorno = "ERRO: " + ex.Message.ToString();
                //throw new UserFriendlyException("ERRO: " + ex.Message.ToString());
            }
            return Json(retorno, "application/json;charset=utf-8", JsonRequestBehavior.AllowGet);
        }

        public void ConsultarSituacaoNotaFiscal(string chaveAcesso)
        {
            var result = string.Empty;
            try
            {
                //var notaFiscal = await _notaFiscalAppService.Obter(chaveAcesso);

                if (notaFiscal == null)
                {
                    throw new UserFriendlyException("NotaFiscalNaoEncontrada");
                }
                var empresa = TempData.Peek("Empresa") as EmpresaDto;
                if (empresa == null || (empresa != null && empresa.Id == 0))
                {
                    throw new UserFriendlyException("EmpresaNaoSelecionada");
                }
                var arquivoConfiguracao = string.Format(@"\{0}_{1}.xml", "config", empresa.Cnpj);
                var configuracoes = this.CarregarConfiguracao();
                //var uf = (DFe.Classes.Entidades.EstadoNfe)Enum.Parse(typeof(DFe.Classes.Entidades.EstadoNfe), notaFiscal.Emitente.enderEmit.UF);
                //_configuracoes.CfgServico.cUF = uf;
                //switch (notaFiscal.Emitente.enderEmit.UF)
                //{
                //    case "AM":
                //    case "BA":
                //    case "CE":
                //    case "GO":
                //    case "MG":
                //    case "MT":
                //    case "PE":
                //    case "PR":
                //    _configuracoes.CfgServico.VersaoNfeConsultaProtocolo = VersaoServico.ve200;
                //        break;
                //    default:
                //        break;

                RetornoNfeConsultaProtocolo manifest = null;
                using (var service = new ServicosNFe(configuracoes.CfgServico))
                {
                    manifest = service.NfeConsultaProtocolo(chaveAcesso);
                }

                if (manifest != null && manifest.Retorno != null)
                {
                    var resultManifest = manifest.Retorno;
                    if (resultManifest.protNFe != null)
                    {
                        var protNFe = resultManifest.protNFe;
                        if (protNFe.infProt != null)
                        {
                            var _nfe = protNFe.infProt;
                            switch (_nfe.cStat)
                            {
                                case 100:
                                case 101:
                                case 102:
                                    break;
                                default:
                                    throw new Exception(_nfe.xMotivo);
                                    //break;
                            }
                            notaFiscal.Motivo = _nfe.xMotivo;
                            notaFiscal.CStat = Convert.ToByte(_nfe.cStat);
                            notaFiscal.DigitoValidacao = _nfe.digVal;
                            notaFiscal.DataRecebimento = _nfe.dhRecbto.DateTime;
                            notaFiscal.NumeroProtocolo = Convert.ToInt64(_nfe.nProt);
                            notaFiscal.Ambiente = (byte)_nfe.tpAmb;
                            //await _notaFiscalAppService.CriarOuEditar(notaFiscal);
                            //result = _nfe.xMotivo;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERRO: " + ex.Message.ToString());
                //return Json(result, "application/json;charset=utf-8", JsonRequestBehavior.AllowGet);
            }
            //return Json(result, "application/json;charset=utf-8", JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> AtualizarSituacaoNotaFiscal(string chaveAcesso)
        {
            var result = string.Empty;
            //try
            //{
            //    var notaFiscal = await _notaFiscalAppService.Obter(chaveAcesso);

            //    if (notaFiscal == null)
            //    {
            //        throw new UserFriendlyException("NotaFiscalNaoEncontrada");
            //    }
            //    var empresa = TempData.Peek("Empresa") as CriarOuEditarEmpresa;
            //    if (empresa == null || (empresa != null && empresa.Id == 0))
            //    {
            //        throw new UserFriendlyException("EmpresaNaoSelecionada");
            //    }
            //    arquivoConfiguracao = string.Format(@"\{0}_{1}.xml", "config", empresa.Cnpj);
            //    _configuracoes = CarregarConfiguracao();
            //    var uf = (DFe.Classes.Entidades.EstadoNfe)Enum.Parse(typeof(DFe.Classes.Entidades.EstadoNfe), notaFiscal.Emitente.enderEmit.UF);
            //    switch (notaFiscal.Emitente.enderEmit.UF)
            //    {
            //        case "AM":
            //            //    case "BA":
            //            //    case "CE":
            //            //    case "GO":
            //            //    case "MG":
            //            //    case "MT":
            //            //    case "PE":
            //            //    case "PR":
            //            _configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls11;
            //            _configuracoes.CfgServico.cUF = uf;
            //            //_configuracoes.CfgServico.VersaoNfeConsultaProtocolo = VersaoServico.ve200;
            //            break;
            //        default:
            //            _configuracoes.CfgServico.cUF = uf;
            //            _configuracoes.CfgServico.ProtocoloDeSeguranca = SecurityProtocolType.Tls;
            //            break;
            //    }
            //    var service = new ServicosNFe(_configuracoes.CfgServico);
            //    var manifest = service.NfeConsultaProtocolo(chaveAcesso);
            //    service.Dispose();
            //    if (manifest != null && manifest.Retorno != null)
            //    {
            //        var resultManifest = manifest.Retorno;
            //        if (resultManifest.protNFe != null)
            //        {
            //            var protNFe = resultManifest.protNFe;
            //            if (protNFe.infProt != null)
            //            {
            //                var _nfe = protNFe.infProt;
            //                switch (_nfe.cStat)
            //                {
            //                    case 100:
            //                    case 101:
            //                    case 102:
            //                        break;
            //                    default:
            //                        throw new Exception(_nfe.xMotivo);
            //                        //break;
            //                }
            //                notaFiscal.Motivo = _nfe.xMotivo;
            //                notaFiscal.CStat = Convert.ToByte(_nfe.cStat);
            //                notaFiscal.DigitoValidacao = _nfe.digVal;
            //                notaFiscal.DataRecebimento = _nfe.dhRecbto;
            //                notaFiscal.NumeroProtocolo = Convert.ToInt64(_nfe.nProt);
            //                notaFiscal.Ambiente = (byte)_nfe.tpAmb;
            //                await _notaFiscalAppService.CriarOuEditar(notaFiscal);
            //                result = _nfe.xMotivo;
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    result = "ERRO: " + ex.Message.ToString();
            //    return Json(result, "application/json;charset=utf-8", JsonRequestBehavior.AllowGet);
            //}
            return Json(result, "application/json;charset=utf-8", JsonRequestBehavior.AllowGet);
        }

        public NFe.Classes.nfeProc ObterDetalhesNota(string chave)
        {
            var configuracoes = this.CarregarConfiguracao();
            //CarregaDadosCertificado();
            RetornoRecepcaoEvento manifest = null;
            using (var service = new ServicosNFe(configuracoes.CfgServico))
            {
                manifest = service.RecepcaoEventoManifestacaoDestinatario(1, 1, chave, NFeTipoEvento.TeMdCienciaDaOperacao, configuracoes.Emitente.CNPJ);
                
            }

            var resultManifest = manifest.Retorno;
            var breakPoint = resultManifest.retEvento;
            RetornoNfeDownload resultDownload = null;
            using (var service = new ServicosNFe(configuracoes.CfgServico))
            {
                resultDownload = service.NfeDownloadNf(configuracoes.Emitente.CNPJ, new List<string> { chave });
                //var result = service.NfeConsultaProtocolo(chave);
            }

            var retConsulta = resultDownload.Retorno.retNFe;
            if (retConsulta.Count() == 0)
            {
                throw new Exception(resultDownload.Retorno.cStat + " - " + resultDownload.Retorno.xMotivo);
            }
            else
            {
                NFe.Classes.nfeProc nfeProc = new NFe.Classes.nfeProc();
                var result = retConsulta[0].XmlNfe;
                if (result != null)
                {
                    foreach (var propriedade in result.LerPropriedades())
                    {
                        if (propriedade.Key.Equals("nfeProc"))
                        {
                            var a = propriedade.Value.As<NFe.Classes.nfeProc>();
                            nfeProc = a;
                            break;
                        }
                    }
                }
                return nfeProc;
            }
        }

        //Rotinas ZeusNFe
        private ConfiguracaoApp CarregarConfiguracao()
        {
            var fullPath = @"c:\nfe" + arquivoConfiguracao; // /configuracao.xml");
            try
            {
                ConfiguracaoApp file = new ConfiguracaoApp();
                if (System.IO.File.Exists(fullPath))
                {
                    file = FuncoesXml.ArquivoXmlParaClasse<ConfiguracaoApp>(fullPath);
                    //_configuracoes = file;
                    if (file.CfgServico.TimeOut == 0)
                    {
                        file.CfgServico.TimeOut = 999999; //mínimo
                    }
                    file.CfgServico.DiretorioSchemas = @"c:\nfe\Schemas";
                    file.CfgServico.DiretorioSalvarXml = @"c:\nfe\Xmls";
                    //throw new Exception("acessou pasta " + file.CfgServico.Certificado.Arquivo);
                    return file;
                }
                else
                {
                    throw new Exception("sem acesso ao caminho " + fullPath);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message.ToString());
            }
        }
    }
}