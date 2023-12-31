﻿using MDFe.Classes.Retorno.MDFeRetRecepcao;
using MDFe.Classes.Servicos.Flags;
using System;
using System.Xml.Serialization;

namespace MDFe.Classes.Retorno
{
    [Serializable]
    [XmlRoot(ElementName = "mdfeProc", Namespace = "http://www.portalfiscal.inf.br/mdfe")]
    public class MDFeProcMDFe
    {
        public MDFeProcMDFe()
        {
            Versao = VersaoServico.Versao100;
        }

        [XmlAttribute(AttributeName = "versao")]
        public VersaoServico Versao { get; set; }

        [XmlElement(ElementName = "MDFe", Namespace = "http://www.portalfiscal.inf.br/mdfe")]
        public Informacoes.MDFe MDFe { get; set; }

        [XmlElement(ElementName = "protMDFe")]
        public MDFeProtMDFe ProtMDFe { get; set; }
    }
}