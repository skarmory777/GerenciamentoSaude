using SW10.SWMANAGER.ClassesAplicaca.Services.Faturamentos.VersoesTISS.V3_03_03;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Interfaces;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Servicos.V3_03_03.GuiasLotes;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Sessions;

using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TISS.Servicos.V3_03_03
{
    public class OperacoesV3_03_03AppService : SWMANAGERAppServiceBase, IOperacoesV3_03_03AppService
    {
        private readonly ISessionAppService _sessionService;

        public OperacoesV3_03_03AppService(ISessionAppService sessionService)
        {
            _sessionService = sessionService;
        }

        public DefaultReturn<string> GerarLoteXML(long loteId)
        {
            var _retornoPadrao = new DefaultReturn<string>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            var _faturamentoEntregaLoteRepository = new SWRepository<FaturamentoEntregaLote>(AbpSession, _sessionService);

            var entregaLote = _faturamentoEntregaLoteRepository.GetAll()
                                                                .Where(w => w.Id == loteId)
                                                                .Include(i => i.Convenio)
                                                                .Include(i => i.Convenio.SisPessoa)
                                                                .FirstOrDefault();

            var entregaLoteDto = FaturamentoEntregaLoteDto.Mapear(entregaLote);

            var teste = this.TenantManager.Tenants.Where(w => w.Id == AbpSession.TenantId);

            var _identificacaoPrestadorNaOperadoraRepository = new SWRepository<IdentificacaoPrestadorNaOperadora>(AbpSession, _sessionService);

            var identificacao = _identificacaoPrestadorNaOperadoraRepository.GetAll()
                                                                     .Where(w => w.ConvenioId == entregaLote.ConvenioId
                                                                             && w.EmpresaId == entregaLote.EmpresaId)
                                                                     .FirstOrDefault();

            entregaLoteDto.IdentificacaoPrestadorNaOperadora = identificacao?.Descricao;

            //if (identificacao != null && !string.IsNullOrEmpty(identificacao.Descricao))
            //{
            //    entregaLoteDto.IdentificacaoPrestadorNaOperadora = identificacao.Descricao;
            //}
            //else
            //{
            //    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "LOT0001", Parametros = new List<object> { entregaLote.Convenio.NomeFantasia } });
            //}

            string xml;

            ValidaDadosXML03_03_03Service validaDados = new ValidaDadosXML03_03_03Service();

            var retornoValidacaoCabecalho = validaDados.ValidarCabecalho(entregaLoteDto);

            _retornoPadrao.Errors.AddRange(retornoValidacaoCabecalho.Errors);

            mensagemTISS mensagemTISS = new mensagemTISS();



            CabecalhoLotesV3_03_03Service cabecalhoLotesService = new CabecalhoLotesV3_03_03Service();

            //var retornoCabecalho = cabecalhoLotesService.CriarCabecalho(entregaLoteDto);
            //mensagemTISS.cabecalho = retornoCabecalho.ReturnObject;

            mensagemTISS.cabecalho = cabecalhoLotesService.CriarCabecalho(entregaLoteDto);

            // _retornoPadrao.Errors.AddRange(retornoCabecalho.Errors);

            GuiasLotesV3_03_03Service guiasLotesService = new GuiasLotesV3_03_03Service(AbpSession, _sessionService);

            prestadorOperadora prestadorOperadora = new prestadorOperadora();


            prestadorOperadora.ItemElementName = ItemChoiceType8.loteGuias;
            //prestadorOperadora.Item = guiasLotesService.GerarGuiasLote(loteId, identificacao?.Descricao);

            var retornoGuia = guiasLotesService.GerarGuiasLote(loteId, identificacao?.Descricao);
            prestadorOperadora.Item = retornoGuia.ReturnObject;

            _retornoPadrao.Errors.AddRange(retornoGuia.Errors);

            mensagemTISS.Item = prestadorOperadora;

            mensagemTISS.epilogo = GerarEpilogo(mensagemTISS);


            var xmlserializer = new XmlSerializer(typeof(mensagemTISS));
            var stringWriter = new StringWriter();

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("ans", "http://www.ans.gov.br/padroes/tiss/schemas");
            ns.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            var encoding = Encoding.GetEncoding("ISO-8859-1");

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = encoding,
            };

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    xmlserializer.Serialize(xmlWriter, mensagemTISS, ns);
                }

                xml = encoding.GetString(stream.ToArray());
            }

            _retornoPadrao.ReturnObject = xml;


            if (_retornoPadrao.Errors.Count > 0)
            {
                _retornoPadrao.Warnings.Add(new ErroDto { CodigoErro = "WLT0001", Parametros = new List<object> { entregaLoteDto.CodEntregaLote } });
            }

            return _retornoPadrao;
        }

        private epilogo GerarEpilogo(mensagemTISS mensagemTISS)
        {
            epilogo epilogo = new epilogo();

            string xml = null;

            var xmlserializer = new XmlSerializer(typeof(mensagemTISS));
            var stringWriter = new StringWriter();
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            var encoding = Encoding.GetEncoding("ISO-8859-1");
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = encoding
            };

            using (var stream = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(stream, xmlWriterSettings))
                {
                    xmlserializer.Serialize(xmlWriter, mensagemTISS, ns);
                }
                xml = encoding.GetString(stream.ToArray());
            }



            XElement root = XElement.Parse(xml);

            //  root.Element("prestadorOperadora").RemoveAttributes();

            var strValue = FuncoesGlobais.RemoveAccents(root.Value);


            epilogo.hash = FuncoesGlobais.CalculateMD5Hash(strValue);

            return epilogo;
        }

    }
}
