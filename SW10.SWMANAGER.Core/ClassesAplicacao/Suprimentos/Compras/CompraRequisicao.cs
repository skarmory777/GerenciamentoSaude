using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras
{
    /// <summary>
    /// Representa uma Requisição de Compra. 
    /// Uma Requisicao de Compra correspopnde a um pedido de aquisição de Produtos ou Serviços a ser utilizado pelo Hospital
    /// PS: Aponta para a entidade CmpRequisicao no BD
    ///     Herda de CamposPadraoCRUD 
    /// </summary>
    [Table("CmpRequisicao")]
    public class CompraRequisicao : CamposPadraoCRUD
    {
        #region ↓ Propriedades
        [ForeignKey("FinFormaPagamento"), Column("FinFormaPagamentoId")]
        public long? FinFormaPagamentoId { get; set; }
        public FormaPagamento FinFormaPagamento { get; set; }

        public bool IsUrgente { get; set; }

        public bool IsAlteraAposGravacao { get; set; }

        public bool IsRequisicaoAprovada { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }

        public bool IsOrdemCompraFinalizada { get; set; }
        [Index("Cmp_Idx_DataRequisicao")]
        [DataType(DataType.DateTime)]
        public DateTime DataRequisicao { get; set; }

        [Index("Cmp_Idx_DataLimiteEntrega")]
        [DataType(DataType.DateTime)]
        public DateTime DataLimiteEntrega { get; set; }
        [Index("Cmp_Idx_DataInicioCotacao")]
        [DataType(DataType.DateTime)]
        public DateTime? DataInicioCotacao { get; set; }

        [Index("Cmp_Idx_DataFinalCotacao")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFinalCotacao { get; set; }

        [Index("Cmp_Idx_DataHoraVencimento")]
        [DataType(DataType.DateTime)]
        public DateTime? DataHoraVencimento { get; set; }

        #region → Chaves Estrangeiras
        [ForeignKey("Empresa"), Column("CmpEmpresaId")]
        public long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [ForeignKey("MotivoPedido"), Column("CmpMotivoPedidoId")]
        public long MotivoPedidoId { get; set; }
        public CompraMotivoPedido MotivoPedido { get; set; }

        [ForeignKey("TipoRequisicao"), Column("CmpTipoRequisicaoId")]
        public long TipoRequisicaoId { get; set; }
        public CompraRequisicaoTipo TipoRequisicao { get; set; }

        [ForeignKey("Estoque"), Column("EstEstoqueId")]
        public long? EstoqueId { get; set; }
        public Estoque Estoque { get; set; }

        [ForeignKey("RequisicaoModo"), Column("CmpModoId")]
        public long ModoRequisicaoId { get; set; }
        public CompraRequisicaoModo RequisicaoModo { get; set; }

        [ForeignKey("AprovacaoStatus"), Column("CmpAprovacaoStatusId")]
        public long AprovacaoStatusId { get; set; } = 1;
        public CompraAprovacaoStatus AprovacaoStatus { get; set; }
        #endregion Chaves Estrangeiras

        #endregion
    }
}
