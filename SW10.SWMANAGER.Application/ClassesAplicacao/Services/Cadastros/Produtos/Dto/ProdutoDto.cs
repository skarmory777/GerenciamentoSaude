using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Grupos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto
{
    /// <summary>
    /// Classe Produto.
    /// Representa um Dto de um produto. Mapeia a Classe Produto que define a referente entidade no BD
    /// </summary>
    [AutoMap(typeof(Produto))]
    public class ProdutoDto : CamposPadraoCRUDDto
    {

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
        public virtual EtiquetaDto EtiquetaDto { get; set; }

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

        #region → Propriedades construidas para o Dto

        /// <summary>
        /// Indica se o produto possui movimentacao de estoque. Este atributo só existe no Dto
        /// </summary>
        public bool possuiMovimentacaoDeEstoque { get; set; }

        /// <summary>
        /// Indica se o produto possui Requisição de Compra. Este atributo só existe no Dto
        /// </summary>
        public bool possuiRequisicaoDeCompraPendente { get; set; }

        public long? UnidadeReferencialId { get; set; }
        public virtual UnidadeDto UnidadeReferencial { get; set; }

        public long? UnidadeGerencialId { get; set; }
        public virtual UnidadeDto UnidadeGerencial { get; set; }

        #endregion 

        #region → Chaves Estrangeiras
        /// <summary>
        /// Produto principal (mestre) ao qual este produto se relaciona.
        /// </summary>
        public ProdutoDto ProdutoPrincipal { get; set; }
        public long? ProdutoPrincipalId { get; set; }

        /// <summary>
        /// Indicação do produto se é para uso específico de um determinado sexo
        /// </summary>

        public long GeneroId { get; set; }
        public virtual Genero Genero { get; set; }

        /// <summary>
        /// Grupo (espécie), Classe e Sub-Classe que pertence o produto
        /// </summary>

        public long GrupoId { get; set; }
        public virtual GrupoDto Grupo { get; set; }

        /// <summary>
        /// Classe e Sub-Classe que pertence o produto
        /// </summary>
        public long? GrupoClasseId { get; set; }
        public virtual GrupoClasseDto Classe { get; set; }

        /// <summary>
        /// Sub-Classe que pertence o produto
        /// </summary>
        public long? GrupoSubClasseId { get; set; }
        public virtual GrupoSubClasseDto SubClasse { get; set; }

        /// <summary>
        /// Referência DCB caso produmo for medicamento - Denominação Comum Brasileira
        /// </summary>
        public long? DCBId { get; set; }
        public virtual DcbDto DCB { get; set; }

        /// <summary>
        /// Localizacao no estoque responsável por recepcionar e guardar o produto
        /// </summary>

        public long EstoqueLocalizacaoId { get; set; }
        public virtual EstoqueLocalizacaoDto EstoqueLocalizacao { get; set; }

        public long? FaturamentoItemId { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }

        public long? ContaAdministrativaId { get; set; }
        public ContaAdministrativaDto ContaAdministrativa { get; set; }

        #endregion Chaves Estrangeiras

        #region → Coleções

        /// <summary>
        /// Lista de uniadades utilizada pelo produto;
        /// </summary>
        public virtual ICollection<ProdutoUnidadeDto> ProdutoUnidade { get; set; }

        #endregion

        #endregion Propriedades


        public static ProdutoDto Mapear(Produto produto)
        {
            if(produto == null)
            {
                return null;
            }

            ProdutoDto produtoDto = new ProdutoDto
            {
                Id = produto.Id,
                Codigo = produto.Codigo,
                Descricao = produto.Descricao,
                IsLote = produto.IsLote,
                IsValidade = produto.IsValidade,
                IsSerie = produto.IsSerie,
                IsKit = produto.IsKit,

                IsNegrito = produto.IsNegrito,
                IsItalico = produto.IsItalico
            };

            return produtoDto;
        }


        public static Produto Mapear(ProdutoDto produtoDto)
        {
            if (produtoDto == null)
            {
                return null;
            }

            Produto produto = new Produto
            {
                Id = produtoDto.Id,
                Codigo = produtoDto.Codigo,
                Descricao = produtoDto.Descricao,
                IsLote = produtoDto.IsLote,
                IsValidade = produtoDto.IsValidade,
                IsSerie = produtoDto.IsSerie,

                IsNegrito = produtoDto.IsNegrito,
                IsItalico = produtoDto.IsItalico
            };

            return produto;
        }

        public static List<ProdutoDto> Mapear(List<Produto> produtos)
        {
            var produtosDto = new List<ProdutoDto>();

            if (produtos != null)
            {
                foreach (var item in produtos)
                {
                    produtosDto.Add(Mapear(item));
                }
            }

            return produtosDto;
        }

    }
}
