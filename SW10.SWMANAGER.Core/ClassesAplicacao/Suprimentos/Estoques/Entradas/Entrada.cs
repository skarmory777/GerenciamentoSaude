using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cfops;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Entradas
{
    [Table("Entrada")]
    public class Entrada : CamposPadraoCRUD
    {
        public long EmpresaId { get; set; }
        public long FornecedorId { get; set; }
        public long TipoDocumentoId { get; set; }
        public long CentroCustoId { get; set; }
        public long CfopId { get; set; }
        public long EstoqueId { get; set; }
        public long NumeroDocumento { get; set; }
        [Index("Idx_Data")]
        public DateTime Data { get; set; }
        public decimal AcrescimoDesconto { get; set; }
        public decimal Frete { get; set; }
        public decimal ValorDocumento { get; set; }


        [ForeignKey("CentroCustoId")]
        public CentroCusto CentroCusto { get; set; }
        [ForeignKey("CfopId")]
        public Cfop Cfop { get; set; }
        [ForeignKey("EstoqueId")]
        public ProdutoEstoque Estoque { get; set; }

        public ICollection<EntradaItem> EntradaItem { get; set; }
    }
}

