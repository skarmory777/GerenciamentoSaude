using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Table("AssSolicitacaoAntimicrobianosResultados")]
    public class SolicitacaoAntimicrobianosResultados : FullAuditedEntity<long>
    {
        
        public long TipoSolicitacaoAntimicrobianosResultadoId { get; set; }

        public TipoSolicitacaoAntimicrobianosResultado TipoResultado { get; set; }

        public bool Valor { get; set; }

        public long CulturaId { get; set; }

        public SolicitacaoAntimicrobianosCulturas Cultura { get; set; }
    }

    [Table("AssSolicitacaoAntimicrobianosCulturas")]
    public class SolicitacaoAntimicrobianosCulturas : CamposPadraoCRUD
    {
        public long SolicitacaoAntimicrobianoId { get; set; }

        public SolicitacaoAntimicrobiano SolicitacaoAntimicrobiano { get; set; }

        public TipoSolicitacaoAntimicrobianosCultura Tipo { get; set; }

        public long TipoId { get; set; }

        [Index("Ass_Idx_DataCultura")]
        public DateTime DataCultura { get; set; }

        public string OutrosResultados { get; set; }

        public bool? StatusResultado { get; set; }

        public ICollection<SolicitacaoAntimicrobianosResultados> SolicitacaoAntimicrobianosResultados { get; set; }

    }

    [Table("AssTipoSolicitacaoAntimicrobianosCultura")]
    public class TipoSolicitacaoAntimicrobianosCultura : CamposPadraoCRUD
    {

    }
}
