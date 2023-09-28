using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_Estoque")]
    public class Estoque : CamposPadraoCRUD, IDescricao
    {

        /// <summary>
        /// UnidadeOrganizacional
        /// </summary>
        //[ForeignKey("SetorId")]
        //public UnidadeOrganizacional UnidadeOrganiczacional { get; set; }
        //public long? SetorId { get; set; }

        /// <summary>
        /// 1 - Distribuicao / 2 - SubEstoque
        /// </summary>
        public long TipoEstoque { get; set; }

        /// <summary>
        /// 1 - Custo Medio, 2 - UltimoPreco
        /// </summary>
        public long TipoCusto { get; set; }

        /// <summary>
        /// Impressoras
        /// </summary>
        public bool IsImpressaoAutomatica { get; set; }

        /// <summary>
        /// Impressoras
        /// 1 - Setor, 2 - Estoque, 3 - Ambos
        /// </summary>
        public long DevolucaoProdutos { get; set; }

        /// <summary>
        /// Impressoras
        /// 
        /// </summary>
        public string CaminhoImpressoraSolicitacaoProduto { get; set; }

        /// <summary>
        /// Impressoras
        /// 
        /// </summary>
        public string CaminhoImpressoraCodigoBarra { get; set; }

        /// <summary>
        /// Impressoras
        /// 
        /// </summary>
        public string CaminhoImpressoraEtiquetaPaciente { get; set; }

        /// <summary>
        /// Tipo Movimentacao
        /// 
        /// </summary>
        public bool IsSaidaPaciente { get; set; }

        /// <summary>
        /// Tipo Movimentacao
        /// 
        /// </summary>
        public bool IsSaidaSetor { get; set; }

        /// <summary>
        /// Tipo Movimentacao
        /// 
        /// </summary>
        public bool IsDevolucaoPaciente { get; set; }

        /// <summary>
        /// Tipo Movimentacao
        /// 
        /// </summary>
        public bool IsDevolucaoSetor { get; set; }

        /// <summary>
        /// Tipo Movimentacao
        /// 
        /// </summary>
        public bool IsTransferenciaEstoques { get; set; }

    }
}
