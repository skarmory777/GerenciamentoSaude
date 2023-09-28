using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProfissionaisSaude;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Conselhos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposPrestadores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProfissionaisSaude
{
    [AutoMap(typeof(ProfissionalSaude))]
    public class ProfissionalSaudeDto : CamposPadraoCRUDDto
    {
        public long? SisPessoaId { get; set; }

        public SisPessoa SisPessoa { get; set; }

        public long? TipoVinculoEmpregaticioId { get; set; }

        public TipoVinculoEmpregaticioDto TipoVinculoEmpregaticio { get; set; }

        public long? TipoParticipacaoId { get; set; }

        public TipoParticipacaoDto TipoParticipacao { get; set; }

        public bool IsCorpoClinico { get; set; }

        public DateTime DataNascimento { get; set; }

        public string CNS { get; set; }

        public long? TipoPrestadorId { get; set; }

        public TipoPrestadorDto TipoPrestador { get; set; }

        public long? ConselhoId { get; set; }

        public ConselhoDto Conselho { get; set; }

        public int NumeroConselho { get; set; }

        public string Faculdade { get; set; }

        public bool IsAtivo { get; set; }

        public long? UserId { get; set; }

        //public UserDto User { get; set; } //****Preciso ver esse relacionamento: Marcus
        public static ProfissionalSaudeDto Mapear(ProfissionalSaude entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<ProfissionalSaudeDto>(entity);

            dto.CNS = entity.CNS;

            dto.SisPessoaId = entity.SisPessoaId;
            dto.SisPessoa = entity.SisPessoa;

            dto.TipoVinculoEmpregaticioId = entity.TipoVinculoEmpregaticioId;
            dto.TipoVinculoEmpregaticio = TipoVinculoEmpregaticioDto.Mapear(entity.TipoVinculoEmpregaticio);

            dto.TipoParticipacaoId = entity.TipoParticipacaoId;
            dto.TipoParticipacao = TipoParticipacaoDto.Mapear(entity.TipoParticipacao);

            dto.TipoPrestadorId = entity.TipoPrestadorId;
            dto.TipoPrestador = TipoPrestadorDto.Mapear(entity.TipoPrestador);

            dto.ConselhoId = entity.ConselhoId;
            dto.Conselho = ConselhoDto.Mapear(entity.Conselho);
            dto.NumeroConselho = entity.NumeroConselho;
            dto.Faculdade = entity.Faculdade;
            dto.IsAtivo = entity.IsAtivo;
            dto.UserId = entity.UserId;

            return dto;
        }
    }
}
