using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    /// <summary>
    /// Classe Produto.
    /// Representa um um Produto. Aponta para a entidade Est_Produto no BD
    /// Herda de CamposPadraoCRUD 
    /// </summary>
    [Table("Est_Produto")]
    public class Produto : CamposPadraoCRUD, IDescricao
    {
        #region ↓ Construtores

        public Produto()
        {

        }

        //public Produto()
        //{
        //    //Substitutos = new HashSet<Produto>();
        //}

        #endregion Construtores

        #region ↓ Propriedades

        /// <summary>
        /// Código de Barras fornecido pelo fabricante ou gerado internamente
        /// </summary>
        public string CodigoBarra { get; set; }

        /// <summary>
        /// Será utilizada na emissão das etiquetas de códigos de barras
        /// </summary>
        public string DescricaoResumida { get; set; }

        /// <summary>
        /// SIAF - Sistemas Integrados de Acompanhamento Financeiro.
        /// SIAFEM - Sistema de Administração Financeira para Estados e Municípios.
        /// SIAGEM - Sistema Integrado de Administração de Serviços para Estados e Municípios.
        /// </summary>
        public string CodigoSistemas { get; set; }

        /// <summary>
        /// Código para integração TISS - Troca de Informação em Saúde Suplementar
        /// </summary>
        public string CodigoTISS { get; set; }

        /// <summary>
        /// Especificações e as especificações detalhadas do produto
        /// </summary>
        public string Especificacao { get; set; }

        public long? EtiquetaId { get; set; }

        #region → Booleans
        /// <summary>
        /// Indica se o produto está ativo
        /// </summary>
        public bool IsAtivo { get; set; }

        /// <summary>
        /// Se produto é uma coleçao de outros produtos (kit)
        /// </summary>
        public bool IsKit { get; set; }

        /// <summary>
        /// Se é Órteses, Próteses e Materiais Especiais
        /// </summary>
        public bool IsOPME { get; set; }

        /// <summary>
        /// Se é Principal (mestre), ou seja, se ele é um nome genérico para referenciar outros produtos
        /// </summary>
        public bool IsPrincipal { get; set; }

        /// <summary>
        /// Se possui cadastro de parâmetros da "Curva ABC" para análise do consumo
        /// </summary>
        public bool IsCurvaABC { get; set; }

        /// <summary>
        /// Se terá controle de série
        /// Todo produto com controle de série a qtde de entreda sempre será 1
        /// </summary>
        public bool IsSerie { get; set; }

        /// <summary>
        /// Se terá controle de lote
        /// Todo produto com lote, possui controle de validade
        /// </summary>
        public bool IsLote { get; set; }

        /// <summary>
        /// Se terá controle de validade
        /// Um produto pode, em raros casos, ter controle de validade sem ter lote
        /// Ex. Kit pode ter controle de validade, mas sem um lote definido
        /// </summary>
        public bool IsValidade { get; set; }

        /// <summary>
        /// Se é medicamento
        /// </summary>
        public bool IsMedicamento { get; set; }

        /// <summary>
        /// Se é medicamento controlado
        /// </summary>
        public bool IsMedicamentoControlado { get; set; }

        /// <summary>
        /// Se o produto está liberado para consumo, transferência, ou seja, liberado para entrada/saída do estoque
        /// </summary>
        public bool IsLiberadoMovimentacao { get; set; }

        /// <summary>
        /// Se está bloqueado para compra
        /// </summary>
        public bool IsBloqueioCompra { get; set; }

        /// <summary>
        /// Se é consignado
        /// </summary>
        public bool IsConsignado { get; set; }

        /// <summary>
        /// Se é padronizado
        /// </summary>
        public bool IsPadronizado { get; set; }

        public bool IsFaturamentoItem { get; set; }

        public bool IsPrescricaoItem { get; set; }

        public bool IsNegrito { get; set; }

        public bool IsItalico { get; set; }

        #endregion Booleans

        #region → Chaves Estrangeiras

        /// <summary>
        /// Produto principal (mestre) ao qual este produto se relaciona.
        /// </summary>
        [ForeignKey("ProdutoPrincipalId")]
        public Produto ProdutoPrincipal { get; set; }
        public long? ProdutoPrincipalId { get; set; }

        /// <summary>
        /// Indicação do produto se é para uso específico de um determinado sexo
        /// </summary>
        public long GeneroId { get; set; }
        [ForeignKey("GeneroId")]
        public Genero Genero { get; set; }

        /// <summary>
        /// Grupo (espécie), Classe e Sub-Classe que pertence o produto
        /// </summary>
        public long GrupoId { get; set; }
        [ForeignKey("GrupoId")]
        public Grupo Grupo { get; set; }

        /// <summary>
        /// Classe e Sub-Classe que pertence o produto
        /// </summary>
        public Nullable<long> GrupoClasseId { get; set; }
        [ForeignKey("GrupoClasseId")]
        public GrupoClasse Classe { get; set; }

        /// <summary>
        /// Sub-Classe que pertence o produto
        /// </summary>
        public Nullable<long> GrupoSubClasseId { get; set; }
        [ForeignKey("GrupoSubClasseId")]
        public GrupoSubClasse SubClasse { get; set; }

        /// <summary>
        /// Referência DCB caso produmo for medicamento - Denominação Comum Brasileira
        /// </summary>
        public Nullable<long> DCBId { get; set; }
        [ForeignKey("DCBId")]
        public DCB DCB { get; set; }

        /// <summary>
        /// Localizacao no estoque responsável por recepcionar e guardar o produto
        /// </summary>
        public long EstoqueLocalizacaoId { get; set; }
        [ForeignKey("EstoqueLocalizacaoId")]
        public EstoqueLocalizacao EstoqueLocalizacao { get; set; }

        [ForeignKey("FaturamentoItem"), Column("FatItemId")]
        public long? FaturamentoItemId { get; set; }

        public FaturamentoItem FaturamentoItem { get; set; }

        [ForeignKey("ContaAdministrativa"), Column("FinContaAdministrativaId")]
        public long? ContaAdministrativaId { get; set; }

        public ContaAdministrativa ContaAdministrativa { get; set; }

        public long? LaboratorioId { get; set; }

        [ForeignKey("LaboratorioId")]
        public EstoqueLaboratorio EstoqueLaboratorio { get; set; }

        #endregion

        #region → Coleções
        /// <summary>
        /// Lista de uniadades utilizada pelo produto;
        /// </summary>
        public ICollection<ProdutoUnidade> ProdutoUnidades { get; set; }

        public List<ProdutoSaldo> ProdutoSaldos { get; set; }

        #endregion

        public decimal? UltimoValorCompra { get; set; }
        public decimal? ValorCompraMedia { get; set; }

        #endregion 
    }
}
