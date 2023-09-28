using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Controladorias.NotasFiscais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Controladorias.NotasFiscais.Dto
{
    [AutoMap(typeof(NotaFiscal))]
    public class CriarOuEditarNotaFiscal : CamposPadraoCRUDDto
    {
        //CAMPOS COMPLETOS VINDOS DO SEFAZ

        //public string ChaveAcesso { get; set; }

        //public string Versao { get; set; }

        //public NotaFiscalIdentificacaoDto Ide { get; set; }

        //public NotaFiscalEmitenteDto Emitente { get; set; }

        //public NotaFiscalAvulsaDto Avulsa { get; set; }

        //public NotaFiscalDestinatarioDto Destinatario { get; set; }

        //public NotaFiscalRetiradaDto Retirada { get; set; }

        //public NotaFiscalEntregaDto Entrega { get; set; }

        //public List<NotaFiscalautXMLDto> AutXml { get; set; }

        //public List<NotaFiscalDetalheDto> DetalhesNota { get; set; }

        //public NotaFiscalTotalDto TotalNota { get; set; }

        //public NotaFiscalCobrancaDto Cobranca { get; set; }

        //public List<NotaFiscalPagamentoDto> Pagamento { get; set; }

        //public NotaFiscalInformacaoAdicionalDto InformacaoAdicional { get; set; }

        //public NotaFiscalexportaDto Exporta { get; set; }

        //public NotaFiscalcompraDto Compra { get; set; }

        //public NotaFiscalcanaDto Cana { get; set; }

        //public short Nsu { get; set; }
        //public string XmlNota { get; set; }
        //public long Numero { get; set; }
        //public long Modelo { get; set; }
        //public long Serie { get; set; }
        //public DateTime DataEmissao { get; set; }
        //public DateTime DataRecebimento { get; set; }


        ////CAMPOS DO SEFAZ

        public string ChaveAcesso { get; set; }

        public long Cnpj { get; set; }

        public long Cpf { get; set; }

        public byte Situacao { get; set; }

        public DateTime DataEmissao { get; set; }

        public DateTime DataRecebimento { get; set; }

        public string DigitoValidacao { get; set; }

        public long InscricaoEstadual { get; set; }

        public long NumeroProtocolo { get; set; }

        public string ProxyDataEmissao { get; set; }

        public byte TipoNota { get; set; }

        public decimal VersaoNota { get; set; }

        public decimal ValorNota { get; set; }

        public string Nome { get; set; }

        public byte CStat { get; set; }

        public byte Ambiente { get; set; }

        public string VersaoAplicacao { get; set; }

        public decimal VersaoServico { get; set; }

        public string Motivo { get; set; }

        public short Nsu { get; set; }

        public string Schema { get; set; }

        public string XmlNota { get; set; }

        public long Numero { get; set; }

        public long Modelo { get; set; }

        public long Serie { get; set; }

        public long EmpresaId { get; set; }

        public virtual EmpresaDto Empresa { get; set; }

        ////CAMPOS VINDOS DO WEBAPI DA TECNOSPEED

        //public long Handle { get; set; }

        //public string ChaveAcesso { get; set; }

        //public string Situacao { get; set; }

        //public long? CodigoNotaFiscal { get; set; }

        //public long? NumeroRecibo { get; set; }

        //public long? NumeroProtocoloEnvio { get; set; }

        //public long? NumeroProtocoloCancelamento { get; set; }

        //public long? NumeroProtocoloInutilizacao { get; set; }

        //public long? NumeroRegistroDpec { get; set; }

        //public string ModoEntrada { get; set; }

        //public string ModoSaida { get; set; }

        //public string Cnpj { get; set; }

        //public string Motivo { get; set; }

        //public DateTime? DataAutorizacao { get; set; }

        //public DateTime? DataCadastro { get; set; }

        //public DateTime? DataCancelamento { get; set; }

        //public DateTime? DataEmissao { get; set; }

        //public string Impresso { get; set; }

        //public string EnviaEmail { get; set; }

        //public string Email { get; set; }

        //public string DocumentoDestinatario { get; set; }

        //public string NomeDestinatario { get; set; }

        //public long? GrupoId { get; set; }

        //public long? IntegracaoId { get; set; }

        //public long? NumeroLote { get; set; }

        //public long Numero { get; set; }

        //public string DhDpec { get; set; }

        //public string NomeGrupo { get; set; }

        //public int Eventos { get; set; }

        //public int Ambiente { get; set; }

        //public string Impressora { get; set; }

        //public int Origem { get; set; }

        //public int SincronizadoPm { get; set; }

        //public string CStat { get; set; }

        //public int Importado { get; set; }

        //public int Destinada { get; set; }

        //public string XmlDestinatario { get; set; }

        //CAMPOS DEFINIDOS NA REUNIÃO COM O MÁRCIO
        //public string NaturezaOperacaoId { get; set; }

        //public string NaturezaOperacao { get; set; }

        //public long ChaveAcesso { get; set; }

        //public long NumeroNota { get; set; }

        //public string VersaoXml { get; set; }

        //public long ProtocoloAutorizacao { get; set; }

        //public DateTime DataProtocoloAutorizacao { get; set; }

        //public Emitente Emitente { get; set; }

        //public Destinatario Destinatario { get; set; }

        //public virtual ICollection<ItensNotaFiscal> ProdutosServicos { get; set; }
    }
}
