using System.Collections.Generic;
using System.Xml.Serialization;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras
{
    public static class PedidoCotacaoRetornoBionexo
    {
		[XmlRoot(ElementName = "Cabecalho", Namespace = "http://www.bionexo.com.br")]
		public class Cabecalho
		{
			[XmlElement(ElementName = "PDC", Namespace = "http://www.bionexo.com.br")]
			public string PDC { get; set; }
			[XmlElement(ElementName = "Requisicao", Namespace = "http://www.bionexo.com.br")]
			public string Requisicao { get; set; }
			[XmlElement(ElementName = "Data_Vencimento", Namespace = "http://www.bionexo.com.br")]
			public string Data_Vencimento { get; set; }
			[XmlElement(ElementName = "Hora_Vencimento", Namespace = "http://www.bionexo.com.br")]
			public string Hora_Vencimento { get; set; }
		}

		[XmlRoot(ElementName = "Fornecedor", Namespace = "http://www.bionexo.com.br")]
		public class Fornecedor
		{
			[XmlElement(ElementName = "Razao_Social", Namespace = "http://www.bionexo.com.br")]
			public string Razao_Social { get; set; }
			[XmlElement(ElementName = "CNPJ", Namespace = "http://www.bionexo.com.br")]
			public string CNPJ { get; set; }
			[XmlElement(ElementName = "Faturamento_Minimo", Namespace = "http://www.bionexo.com.br")]
			public string Faturamento_Minimo { get; set; }
			[XmlElement(ElementName = "Prazo_Entrega", Namespace = "http://www.bionexo.com.br")]
			public string Prazo_Entrega { get; set; }
			[XmlElement(ElementName = "Validade_Proposta", Namespace = "http://www.bionexo.com.br")]
			public string Validade_Proposta { get; set; }
			[XmlElement(ElementName = "Id_Forma_Pagamento", Namespace = "http://www.bionexo.com.br")]
			public string Id_Forma_Pagamento { get; set; }
			[XmlElement(ElementName = "Frete", Namespace = "http://www.bionexo.com.br")]
			public string Frete { get; set; }
			[XmlElement(ElementName = "Data_Confirmacao", Namespace = "http://www.bionexo.com.br")]
			public string Data_Confirmacao { get; set; }
			[XmlElement(ElementName = "Observacao_Confirmacao", Namespace = "http://www.bionexo.com.br")]
			public string Observacao_Confirmacao { get; set; }
		}

		[XmlRoot(ElementName = "Fornecedores", Namespace = "http://www.bionexo.com.br")]
		public class Fornecedores
		{
			[XmlElement(ElementName = "Fornecedor", Namespace = "http://www.bionexo.com.br")]
			public Fornecedor Fornecedor { get; set; }
		}

		[XmlRoot(ElementName = "Campo_Extra", Namespace = "http://www.bionexo.com.br")]
		public class Campo_Extra
		{
			[XmlElement(ElementName = "Nome", Namespace = "http://www.bionexo.com.br")]
			public string Nome { get; set; }
			[XmlElement(ElementName = "Valor", Namespace = "http://www.bionexo.com.br")]
			public string Valor { get; set; }
		}

		[XmlRoot(ElementName = "Resposta", Namespace = "http://www.bionexo.com.br")]
		public class Resposta
		{
			[XmlElement(ElementName = "CNPJ", Namespace = "http://www.bionexo.com.br")]
			public string CNPJ { get; set; }
			[XmlElement(ElementName = "Fabricante", Namespace = "http://www.bionexo.com.br")]
			public string Fabricante { get; set; }
			[XmlElement(ElementName = "Embalagem", Namespace = "http://www.bionexo.com.br")]
			public string Embalagem { get; set; }
			[XmlElement(ElementName = "Preco_Unitario", Namespace = "http://www.bionexo.com.br")]
			public string Preco_Unitario { get; set; }
			[XmlElement(ElementName = "Preco_Total", Namespace = "http://www.bionexo.com.br")]
			public string Preco_Total { get; set; }
			[XmlElement(ElementName = "Comentario", Namespace = "http://www.bionexo.com.br")]
			public string Comentario { get; set; }
		}

		[XmlRoot(ElementName = "Item", Namespace = "http://www.bionexo.com.br")]
		public class Item
		{
			[XmlElement(ElementName = "Id_Artigo", Namespace = "http://www.bionexo.com.br")]
			public string Id_Artigo { get; set; }
			[XmlElement(ElementName = "Cod_Produto", Namespace = "http://www.bionexo.com.br")]
			public string Cod_Produto { get; set; }
			[XmlElement(ElementName = "Quantidade", Namespace = "http://www.bionexo.com.br")]
			public string Quantidade { get; set; }
			[XmlElement(ElementName = "Campo_Extra", Namespace = "http://www.bionexo.com.br")]
			public List<Campo_Extra> Campo_Extra { get; set; }
			[XmlElement(ElementName = "Resposta", Namespace = "http://www.bionexo.com.br")]
			public Resposta Resposta { get; set; }
		}

		[XmlRoot(ElementName = "Itens", Namespace = "http://www.bionexo.com.br")]
		public class Itens
		{
			[XmlElement(ElementName = "Item", Namespace = "http://www.bionexo.com.br")]
			public List<Item> Item { get; set; }
		}

		[XmlRoot(ElementName = "Respostas", Namespace = "http://www.bionexo.com.br")]
		public class Respostas
		{
			[XmlElement(ElementName = "Cabecalho", Namespace = "http://www.bionexo.com.br")]
			public Cabecalho Cabecalho { get; set; }
			[XmlElement(ElementName = "Fornecedores", Namespace = "http://www.bionexo.com.br")]
			public Fornecedores Fornecedores { get; set; }
			[XmlElement(ElementName = "Itens", Namespace = "http://www.bionexo.com.br")]
			public Itens Itens { get; set; }
			[XmlAttribute(AttributeName = "xmlns")]
			public string Xmlns { get; set; }
		}
	}
}