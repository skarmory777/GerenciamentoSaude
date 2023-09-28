using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class SolicitacaoAntimicrobianosResultadoDto : FullAuditedEntityDto<long>
    {
        
        public long TipoSolicitacaoAntimicrobianosResultadoId { get; set; }

        public TipoSolicitacaoAntimicrobianosResultadoDto TipoResultado { get; set; }

        public bool Valor { get; set; }

        public long CulturaId { get; set; }
        public SolicitacaoAntimicrobianosCulturaDto Cultura { get; set; }
    }

    public class SolicitacaoAntimicrobianosCulturaDto : CamposPadraoCRUDDto
    {
        public long SolicitacaoAntimicrobianoId { get; set; }

        public SolicitacaoAntimicrobianoDto SolicitacaoAntimicrobiano { get; set; }

        public TipoSolicitacaoAntimicrobianosCulturaDto Tipo { get; set; }

        public long TipoId { get; set; }

        public DateTime DataCultura { get; set; }

        public string OutrosResultados { get; set; }

        public bool? StatusResultado { get; set; }

        public ICollection<SolicitacaoAntimicrobianosResultadoDto> SolicitacaoAntimicrobianosResultados { get; set; }

    }

    public class TipoSolicitacaoAntimicrobianosCulturaDto : CamposPadraoCRUDDto
    {

    }
}
