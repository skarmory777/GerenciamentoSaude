using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos
{
    [Table("SisMovAutomatico")]
    public class MovimentoAutomatico : CamposPadraoCRUD
    {
        public long EmpresaId { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public long? CentroCustoId { get; set; }
        public long? TerceirizadoId { get; set; }
        public long? TurnoId { get; set; }
        public long? TipoAcomodacaoId { get; set; }
        public float Quantidade { get; set; }
        [Index("Sis_Idx_IsAmbulatorio")]
        public bool IsAmbulatorio { get; set; }
        [Index("Sis_Idx_IsInternacao")]
        public bool IsInternacao { get; set; }
        [Index("Sis_Idx_IsNovoAtendimento")]
        public bool IsNovoAtendimento { get; set; }
        [Index("Sis_Idx_IsDiaria")]
        public bool IsDiaria { get; set; }
        [Index("Sis_Idx_IsCobraPernoite")]
        public bool IsCobraPernoite { get; set; }
        [Index("Sis_Idx_IsCobraRefeicao")]
        public bool IsCobraRefeicao { get; set; }

        [Index("Sis_Idx_IsCobraFralda")]
        public bool IsCobraFralda { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        [ForeignKey("UnidadeOrganizacionalId")]
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [ForeignKey("CentroCustoId")]
        public CentroCusto CentroCusto { get; set; }

        [ForeignKey("TerceirizadoId")]
        public Terceirizado Terceirizado { get; set; }

        [ForeignKey("TurnoId")]
        public Turno Turno { get; set; }

        [ForeignKey("TipoAcomodacaoId")]
        public TipoAcomodacao TipoAcomodacao { get; set; }

        public IList<MovimentoAutomaticoConvenioPlano> MovimentosAutomaticosConveiosPlanos { get; set; }
        public IList<MovimentoAutomaticoEspecialidade> MovimentosAutomaticosEspecialidades { get; set; }
        public IList<MovimentoAutomaticoFaturamentoItem> MovimentosAutomaticosFaturamentosItens { get; set; }
        public IList<MovimentoAutomaticoTipoGuia> MovimentosAutomaticosTiposGuias { get; set; }

    }
}
