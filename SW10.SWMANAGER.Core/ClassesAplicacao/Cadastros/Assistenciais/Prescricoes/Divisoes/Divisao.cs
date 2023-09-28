using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposPrescricoes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes
{
    [Table("AssDivisao")]
    public class Divisao : CamposPadraoCRUD, IDescricao
    {
        [StringLength(60)]
        public override string Descricao { get; set; }

        [ForeignKey("TipoPrescricao"), Column("AssTipoPrescricaoId")]
        public long? TipoPrescricaoId { get; set; }

        public int Ordem { get; set; }

        public bool IsDivisaoPrincipal { get; set; }

        [ForeignKey("DivisaoPrincipal"), Column("AssDivisaoId")]
        public long? DivisaoPrincipalId { get; set; }

        public Divisao DivisaoPrincipal { get; set; }

        public TipoPrescricao TipoPrescricao { get; set; }

        //public ICollection<DivisaoTipoResposta> TiposRespostas { get; set; }

        public ICollection<Divisao> Subdivisoes { get; set; }

        public bool IsQuantidade { get; set; }
        public bool IsUnidadeMedida { get; set; }
        public bool IsVelocidadeInfusao { get; set; }
        public bool IsDuracao { get; set; }
        public bool IsFormaAplicacao { get; set; }
        public bool IsFrequencia { get; set; }
        public bool IsUniddeOrganizacional { get; set; }
        public bool IsMedico { get; set; }
        public bool IsDataInicio { get; set; }
        public bool IsDiasAplicacao { get; set; }
        public bool IsObservacao { get; set; }
        public bool IsCopiarPrescricao { get; set; }
        public bool IsTipoMedicacao { get; set; }
        public bool IsExameImagem { get; set; }
        public bool IsExameLaboratorial { get; set; }
        public bool IsSetorExame { get; set; }
        public bool IsProdutoEstoque { get; set; }
        public bool IsControlaVolume { get; set; }
        public bool IsSangueDerivado { get; set; }
        public bool IsSeNecessario { get; set; }
        public bool IsUrgente { get; set; }
        public bool IsAgora { get; set; }
        public bool IsAcm { get; set; }
        public bool IsEstoque { get; set; }
        public bool IsFaturamento { get; set; }
        public bool IsMedicamento { get; set; }

        public bool IsDoseUnica { get; set; }

        public ICollection<ConfiguracaoPrescricaoItem> ConfiguracaoPrescricaoItems { get; set; }
    }
}
