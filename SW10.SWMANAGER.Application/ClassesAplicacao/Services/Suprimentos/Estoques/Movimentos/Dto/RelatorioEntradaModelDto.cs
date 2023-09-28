using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class RelatorioEntradaModelDto
    {
        public long PreMovimentoId { get; set; }
        public string Titulo { get; set; }
        public string NomeHospital { get; set; }
        public string NomeUsuario { get; set; }
        public string DataHora { get; set; }
        public string Fornecedor { get; set; }
        public string CNPJFornecedor { get; set; }
        public string Estoque { get; set; }
        public string Documento { get; set; }
        public string DataEntrada { get; set; }
        public string TipoEntrada { get; set; }
        public string UsuarioEntrada { get; set; }
        public string TotalItens { get; set; }
        public string ValorFrete { get; set; }
        public string ValorDesconto { get; set; }
        public string ValorTotal { get; set; }
        public string Paciente { get; set; }
        public string Setor { get; set; }

        public bool IsEntrada { get; set; }


        public List<RelatorioEntradaItemModalDto> Itens { get; set; }
    }

    public class RelatorioEntradaItemModalDto
    {
        public string CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; }
        public string Sigla { get; set; }
        public string Lote { get; set; }
        public string Validade { get; set; }
        public string ValorUnitario { get; set; }
        public string Quantidade { get; set; }
        public string ValorTotal { get; set; }
        public string ValorIPI { get; set; }

    }
}
