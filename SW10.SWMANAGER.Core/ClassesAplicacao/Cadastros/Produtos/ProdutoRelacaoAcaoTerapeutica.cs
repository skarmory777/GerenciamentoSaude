﻿using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosAcoesTerapeutica;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos
{
    [Table("ProdutoRelacaoAcaoTerapeutica")]
    public class ProdutoRelacaoAcaoTerapeutica : CamposPadraoCRUD
    {
        public long ProdutoId { get; set; }
        public long ProdutoAcaoTerapeuticaId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("ProdutoAcaoTerapeuticaId")]
        public ProdutoAcaoTerapeutica ProdutoAcaoTerapeutica { get; set; }
    }
}
