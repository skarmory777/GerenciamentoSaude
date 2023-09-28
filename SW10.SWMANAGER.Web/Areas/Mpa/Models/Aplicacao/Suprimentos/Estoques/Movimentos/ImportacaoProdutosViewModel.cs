using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;

using System.Collections.Generic;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos
{
    public class ImportacaoProdutosViewModel
    {
        public List<EstoqueImportacaoProdutoDto> ImportacaoProdutos { get; set; }
        public long FornecedorId { get; set; }
        public string CNPJNota { get; set; }
    }
}