using Abp.Application.Services.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class 
        SolicitacaoAntimicrobianosIndicacaoDto: FullAuditedEntityDto<long>
    {
        public long TipoSolicitacaoAntimicrobianosIndicacaoId { get; set; }

        public TipoSolicitacaoAntimicrobianosIndicacaoDto TipoIndicacao { get; set; }

        public long SolicitacaoAntimicrobianoId { get; set; }

        public SolicitacaoAntimicrobianoDto SolicitacaoAntimicrobiano { get; set; }
    }
}
