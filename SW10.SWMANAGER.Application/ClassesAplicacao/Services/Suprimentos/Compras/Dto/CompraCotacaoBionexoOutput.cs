using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    public class CompraCotacaoBionexoOutput
    {
		[XmlRoot(ElementName = "postResponse", Namespace = "http://webservice.bionexo.com/")]
		public class PostResponse
		{
			[XmlElement(ElementName = "return")]
			[XmlText]
			public string Return { get; set; }
			[XmlAttribute(AttributeName = "ns1", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Ns1 { get; set; }
		}

		[XmlRoot(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		public class Body
		{
			[XmlElement(ElementName = "postResponse", Namespace = "http://webservice.bionexo.com/")]
			public PostResponse PostResponse { get; set; }
		}

		[XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
		public class Envelope
		{
			[XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
			public Body Body { get; set; }
			[XmlAttribute(AttributeName = "soap", Namespace = "http://www.w3.org/2000/xmlns/")]
			public string Soap { get; set; }
		}
	}
}
