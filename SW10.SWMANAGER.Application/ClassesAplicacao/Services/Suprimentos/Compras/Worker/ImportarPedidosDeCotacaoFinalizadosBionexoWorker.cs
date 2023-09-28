using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Abp.UI;
using RestSharp;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;
using System;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using System.Xml.Serialization;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Compras.Worker
{
    public class ImportarPedidosDeCotacaoFinalizadosBionexoWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        #region → Services
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUltimoIdAppService _ultimoIdAppService;
        #endregion Servicos 

        #region → Repositorios
        private readonly IRepository<CmpOrdemCompra, long> _ordemCompraRepository;
        private readonly IRepository<OrdemCompraItem, long> _ordemCompraItemRepository;
        private readonly IRepository<CompraCotacao, long> _compraCotacaoRepository;
        private readonly IRepository<OrdemCompraStatus, long> _ordemCompraStatusRepository;
        private readonly IRepository<CompraCotacaoStatus, long> _compraCotacaoStatusRepository;
        #endregion  Repositorios

        #region Constructor
        public ImportarPedidosDeCotacaoFinalizadosBionexoWorker(AbpTimer timer,
                                                                IRepository<CmpOrdemCompra, long> ordemCompraRepository,
                                                                IRepository<CompraCotacao, long> compraCotacaoRepository,
                                                                IRepository<OrdemCompraItem, long> ordemCompraItemRepository,
                                                                IUltimoIdAppService ultimoIdAppService,
                                                                IRepository<OrdemCompraStatus, long> ordemCompraStatusRepository,
                                                                IRepository<CompraCotacaoStatus, long> compraCotacaoStatusRepository) : base(timer)
        {
            Timer.Period = 600000; // 10 minutos;

            _ordemCompraRepository = ordemCompraRepository;
            _compraCotacaoRepository = compraCotacaoRepository;
            _ordemCompraItemRepository = ordemCompraItemRepository;
            _ultimoIdAppService = ultimoIdAppService;
            _ordemCompraStatusRepository = ordemCompraStatusRepository;
            _compraCotacaoStatusRepository = compraCotacaoStatusRepository;
        }
        #endregion

        #region Public Methods
        [UnitOfWork(false)]
        protected override void DoWork()
        {
            // Cotações enviadas ao Bionexo, mas que ainda não houve retorno do Pedido de Cotação (Status Finalizado) 
            var comprasCotacao = _compraCotacaoRepository.GetAll().Include(x => x.Requisicao).Where(x => x.DataEnvioBionexo != null &&
                                                                                                    x.IsDeleted == false &&
                                                                                                    x.IdBionexo != null &&
                                                                                                    x.DataRetornoBionexo == null).ToList();

            foreach (var compraCotacao in comprasCotacao)
            {
                try
                {
                    //ProcessaImportacaoPedidoCotacaoWebserviceBionexo(compraCotacao);
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion

        #region Private Methods
        private void ProcessaImportacaoPedidoCotacaoWebserviceBionexo(CompraCotacao compraCotacao)
        {
            const string ERRO_RETORNO_BIONEXO = "-1";
            const string OPERATION_WEG = "WEG";
            const int RETORNO_BIONEXO_STATUS = 5;

            var client = new RestClient(ConfigurationManager.AppSettings.Get("BionexoWsBaseUrl"));
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/xml");
            var body = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:web=""http://webservice.bionexo.com/"">
                " + "\n" +
                            @"   <soapenv:Header/>
                " + "\n" +
                            @"   <soapenv:Body>
                " + "\n" +
                            @"   <web:post>
                " + "\n" +
                            @"    <login>" + ConfigurationManager.AppSettings.Get("BionexoWsLogin") + @"</login>
                " + "\n" +
                            @"    <password>" + ConfigurationManager.AppSettings.Get("BionexoWsPassword") + @"</password>
                " + "\n" +
                            @"    <operation>" + OPERATION_WEG + @"</operation>
                " + "\n" +
                            @"    <parameters>ID=" + compraCotacao.IdBionexo + @"</parameters>        
                " + "\n" +
                            @"   </web:post>
                " + "\n" +
                            @"   </soapenv:Body>
                " + "\n" +
                            @"</soapenv:Envelope> ";

            request.AddParameter("application/xml", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            using (TextReader reader = new StringReader(response.Content))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(reader);

                XmlNamespaceManager ns = new XmlNamespaceManager(xDoc.NameTable);
                ns.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                ns.AddNamespace("ns1", "http://webservice.bionexo.com/");

                XmlCDataSection cDataNode = (XmlCDataSection)(xDoc.SelectSingleNode("//soap:Envelope/soap:Body/ns1:postResponse/return", ns).ChildNodes[0]);
                var respostaBionexo = cDataNode.Data.Split(';'); 
                var statusRetorno = respostaBionexo[0].ToString();           
                var dataRetornoBionexo = DateTime.Parse(respostaBionexo[1].ToString());
                var mensagemRetorno = respostaBionexo[2].ToString();

                if (statusRetorno != ERRO_RETORNO_BIONEXO) {
                    XmlSerializer serializer = new XmlSerializer(typeof(PedidoCotacaoRetornoBionexo.Respostas));
                    using (StringReader readerRetorno = new StringReader(mensagemRetorno))
                    {
                        using (var unitOfWork = _unitOfWorkManager.Begin())
                        {
                            // Atualiza os dados da Compra Cotação
                            compraCotacao.DataRetornoBionexo = DateTime.Now;
                            compraCotacao.CompraCotacaoStatusId = RETORNO_BIONEXO_STATUS;
                            _compraCotacaoRepository.Update(compraCotacao);

                            // Adiciona a nova Ordem de Compra
                            var ordemCompra = new CmpOrdemCompra();
                            ordemCompra.Codigo = _ultimoIdAppService.ObterProximoCodigo("CmpOrdemCompra").Result;
                            ordemCompra.DataFinalEntrega = DateTime.Now.AddDays(1);
                            ordemCompra.DataPrevistaEntrega = DateTime.Now.AddDays(1);
                            ordemCompra.EmpresaId = compraCotacao.Requisicao.EmpresaId;
                            ordemCompra.UnidadeOrganizacionalId = compraCotacao.Requisicao.UnidadeOrganizacionalId;
                            ordemCompra.OrdemCompraStatusId = 1; // Aberto
                            ordemCompra.PrazoPagamento = 0;
                            ordemCompra.DataOrdemCompra = DateTime.Now;
                            ordemCompra.ValorDesconto = 0;
                            ordemCompra.ValorFrete = 0;
                            ordemCompra.Id = _ordemCompraRepository.InsertAndGetId(ordemCompra);

                            var pedidoCotacaoRetornoBionexo = (PedidoCotacaoRetornoBionexo.Respostas)serializer.Deserialize(reader);

                            if (pedidoCotacaoRetornoBionexo.Itens != null) {
                                foreach (var itemCotacaoResposta in pedidoCotacaoRetornoBionexo.Itens.Item)
                                {
                                    // Adiciona o item da Ordem de Compra
                                    var ordemCompraItem = new OrdemCompraItem();
                                    ordemCompraItem.OrdemCompraId = ordemCompra.Id;
                                    //ordemCompraItem.ValorUnitario = 
                                    //ordemCompraItem.
                                }
                            }

                            unitOfWork.Complete();
                            _unitOfWorkManager.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                }
            }
        }

        #endregion
    }
}
