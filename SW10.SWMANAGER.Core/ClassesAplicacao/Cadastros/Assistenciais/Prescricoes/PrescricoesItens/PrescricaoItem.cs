using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    [Table("AssPrescricaoItem")]
    public class PrescricaoItem : CamposPadraoCRUD, IDescricao
    {
        [ForeignKey("Divisao"), Column("AssDivisaoId")]
        public long? DivisaoId { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsAlertaDuplicidade { get; set; }

        public bool IsExigeJustificativa { get; set; }

        public string Justificativa { get; set; }

        [ForeignKey("TipoPrescricao"), Column("AssTipoPrescricaoId")]
        public long? TipoPrescricaoId { get; set; }

        [ForeignKey("TipoControle"), Column("AssTipoControleId")]
        public long? TipoControleId { get; set; }

        public bool IsAlteraQuantidade { get; set; }

        public long TotalDias { get; set; }

        public decimal? Quantidade { get; set; }

        [ForeignKey("Unidade"), Column("EstUnidadeId")]
        public long? UnidadeId { get; set; }

        [ForeignKey("FormaAplicacao"), Column("AssFormaAplicacaoId")]
        public long? FormaAplicacaoId { get; set; }

        [ForeignKey("Frequencia"), Column("AssFrequenciaId")]
        public long? FrequenciaId { get; set; }

        [ForeignKey("VelocidadeInfusao"), Column("AssVelocidadeInfusaoId")]
        public long? VelocidadeInfusaoId { get; set; }

        [ForeignKey("UnidadeRequisicao"), Column("EstUnidadeRequisicaoId")]
        public long? UnidadeRequisicaoId { get; set; }

        [ForeignKey("Produto"), Column("EstProdutoId")]
        public long? ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [ForeignKey("FaturamentoItem"), Column("FatItemId")]
        public long? FaturamentoItemId { get; set; }
        public FaturamentoItem FaturamentoItem { get; set; }

        public Divisao Divisao { get; set; }

        public TipoPrescricao TipoPrescricao { get; set; }

        public TipoControle TipoControle { get; set; }

        public Unidade Unidade { get; set; }

        public FormaAplicacao FormaAplicacao { get; set; }

        public Frequencia Frequencia { get; set; }

        public VelocidadeInfusao VelocidadeInfusao { get; set; }

        public Unidade UnidadeRequisicao { get; set; }

        [ForeignKey("Estoque"), Column("EstEstoqueId")]
        public long? EstoqueId { get; set; }
        public Estoque Estoque { get; set; }

        public bool IsDiluente { get; set; }


        public bool IsNegrito { get; set; }

        public bool IsItalico { get; set; }
        
        [ForeignKey("PrescricaoItemParent")]
        public long? PrescricaoItemId { get; set; }
        
        public PrescricaoItem PrescricaoItemParent { get; set; }

        public bool IsControleDosagem { get; set; }
        public decimal? MinimoAceitavel { get; set; }
        public decimal? MaximoAceitavel { get; set; }
        public decimal? MinimoBloqueio { get; set; }
        public decimal? MaximoBloqueio { get; set; }




        public IList<ConfiguracaoPrescricaoItem> ConfiguracaoPrescricaoItems { get; set; }

    }
}
