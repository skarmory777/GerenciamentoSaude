using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Autorizacoes
{
    [Table("FatAutorizacaoDetalhe")]
    public class FaturamentoAutorizacaoDetalhe : CamposPadraoCRUD
    {
        [ForeignKey("Autorizacao"), Column("FatAutorizacaoId")]
        public long? AutorizacaoId { get; set; }
        public FaturamentoAutorizacao Autorizacao { get; set; }

        [ForeignKey("Convenio"), Column("SisConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("Plano"), Column("SisPlanoId")]
        public long? PlanoId { get; set; }
        public Plano Plano { get; set; }

        [ForeignKey("Grupo"), Column("FatGrupoId")]
        public long? GrupoId { get; set; }
        public FaturamentoGrupo Grupo { get; set; }

        [ForeignKey("SubGrupo"), Column("FatSubGrupoId")]
        public long? SubGrupoId { get; set; }
        public FaturamentoSubGrupo SubGrupo { get; set; }

        [ForeignKey("Item"), Column("FatItemId")]
        public long? ItemId { get; set; }
        public FaturamentoItem Item { get; set; }

        [ForeignKey("Unidade"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeId { get; set; }
        public UnidadeOrganizacional Unidade { get; set; }

        public bool IsLimiteQtd { get; set; }
        public int QtdMinimo { get; set; }
        public int QtdMaximo { get; set; }
    }
}


