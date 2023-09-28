using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Taxas;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto
{
    [AutoMap(typeof(FaturamentoTaxa))]
    public class TaxaJTable : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }

        public long? Nivel { get; set; } = 1;
        public double Percentual { get; set; }
        public bool IsAmbulatorio { get; set; }
        public bool IsInternacao { get; set; }
        public bool IsIncideFilme { get; set; }

        public bool IsIncidePorte { get; set; }
        public bool IsIncidePrecoItem { get; set; }

        public bool IsIncideManual { get; set; }
        public bool IsImplicita { get; set; }
        public bool IsTodosLocal { get; set; }
        public bool IsTodosTurno { get; set; }
        public bool IsTodosTipoLeito { get; set; }
        public bool IsTodosGrupo { get; set; }
        public bool IsTodosItem { get; set; }
        public bool IsTodosConvenio { get; set; }
        public bool IsTodosPlano { get; set; }
        public string LocalImpressao { get; set; }
        public string EmpresaNome { get; set; }
        public string UnidadeOrganizacaionalNome { get; set; }
        public string TurnoDescricao { get; set; }
        public string TipoLeitoDescricao { get; set; }
        public string GrupoDescricao { get; set; }
        public long? EmpresaId { get; set; }
        public long? UnidadeOrganizacaionalId { get; set; }
        public long? TurnoId { get; set; }
        public long? TipoLeitoId { get; set; }
        public long? GrupoId { get; set; }
        public string EmpresasJson { get; set; }
        public string LocaisJson { get; set; }
        public string GruposJson { get; set; }
        public string TurnosJson { get; set; }
        public string TiposLeitosJson { get; set; }

        public string ItemsJson { get; set; }
    }
}
