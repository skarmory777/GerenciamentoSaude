﻿/********************************************************************************/
/* Projeto: Biblioteca ZeusNFe                                                  */
/* Biblioteca C# para emissão de Nota Fiscal Eletrônica - NFe e Nota Fiscal de  */
/* Consumidor Eletrônica - NFC-e (http://www.nfe.fazenda.gov.br)                */
/*                                                                              */
/* Direitos Autorais Reservados (c) 2014 Adenilton Batista da Silva             */
/*                                       Zeusdev Tecnologia LTDA ME             */
/*                                                                              */
/*  Você pode obter a última versão desse arquivo no GitHub                     */
/* localizado em https://github.com/adeniltonbs/Zeus.Net.NFe.NFCe               */
/*                                                                              */
/*                                                                              */
/*  Esta biblioteca é software livre; você pode redistribuí-la e/ou modificá-la */
/* sob os termos da Licença Pública Geral Menor do GNU conforme publicada pela  */
/* Free Software Foundation; tanto a versão 2.1 da Licença, ou (a seu critério) */
/* qualquer versão posterior.                                                   */
/*                                                                              */
/*  Esta biblioteca é distribuída na expectativa de que seja útil, porém, SEM   */
/* NENHUMA GARANTIA; nem mesmo a garantia implícita de COMERCIABILIDADE OU      */
/* ADEQUAÇÃO A UMA FINALIDADE ESPECÍFICA. Consulte a Licença Pública Geral Menor*/
/* do GNU para mais detalhes. (Arquivo LICENÇA.TXT ou LICENSE.TXT)              */
/*                                                                              */
/*  Você deve ter recebido uma cópia da Licença Pública Geral Menor do GNU junto*/
/* com esta biblioteca; se não, escreva para a Free Software Foundation, Inc.,  */
/* no endereço 59 Temple Street, Suite 330, Boston, MA 02111-1307 USA.          */
/* Você também pode obter uma copia da licença em:                              */
/* http://www.opensource.org/licenses/lgpl-license.php                          */
/*                                                                              */
/* Zeusdev Tecnologia LTDA ME - adenilton@zeusautomacao.com.br                  */
/* http://www.zeusautomacao.com.br/                                             */
/* Rua Comendador Francisco josé da Cunha, 111 - Itabaiana - SE - 49500-000     */
/********************************************************************************/

using DFe.Utils;
using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NFe.Classes.Servicos.DistribuicaoDFe.Schemas
{
    [Serializable()]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "http://www.portalfiscal.inf.br/nfe")]
    [XmlRoot(Namespace = "http://www.portalfiscal.inf.br/nfe", IsNullable = false)]
    public class resEvento
    {
        /// <summary>
        /// D02 - Versão do leiaute
        /// </summary>
        [XmlAttribute()]
        public decimal versao { get; set; }

        /// <summary>
        /// D03 - Código do órgão de recepção do Evento. 
        /// Utilizar 91 para identificar o Ambiente Nacional.
        /// </summary>
        public string cOrgao { get; set; }

        /// <summary>
        /// D04 - CNPJ do Emitente
        /// </summary>
        public ulong CNPJ { get; set; }

        /// <summary>
        /// D05 - CPF do Emitente
        /// </summary>
        public ulong CPF { get; set; }

        /// <summary>
        /// D06 - Chave de acesso da NF-e
        /// </summary>
        [XmlElement(DataType = "integer")]
        public string chNFe { get; set; }

        /// <summary>
        /// D07 - Data e hora do evento
        /// </summary>
        [XmlIgnore]
        public DateTime dhEvento { get; set; }

        /// <summary>
        /// Proxy para dhEvento no formato AAAA-MM-DDThh:mm:ssTZD (UTC - Universal Coordinated Time)
        /// </summary>
        [XmlElement(ElementName = "dhEvento")]
        public string ProxydhEvento
        {
            get { return dhEvento.ParaDataHoraStringUtc(); }
            set { dhEvento = DateTime.Parse(value); }
        }

        /// <summary>
        /// D08 - Código do evento
        /// </summary>
        public string tpEvento { get; set; }

        /// <summary>
        /// D09 - Número sequencial do evento
        /// </summary>
        public string nSeqEvento { get; set; }

        /// <summary>
        /// D10 - Descrição do evento
        /// </summary>
        public string xEvento { get; set; }

        /// <summary>
        /// D11 - Data de autorização do evento
        /// </summary>
        public DateTime dhRecbto { get; set; }

        /// <summary>
        /// D12 - Número de protocolo do evento
        /// </summary>
        public ulong nProt { get; set; }
    }
}
