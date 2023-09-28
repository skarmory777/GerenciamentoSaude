using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProfissionaisSaude;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Operacoes.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.ProntuariosEletronicos.Dto
{
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
    using System.Linq;

    [AutoMap(typeof(Prontuario))]
    public class ProntuarioEletronicoDto : CamposPadraoCRUDDto
    {
        public DateTime DataAdmissao { get; set; }

        public long UnidadeOrganizacionalId { get; set; }

        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        public long AtendimentoId { get; set; }

        public AtendimentoDto Atendimento { get; set; }

        public long? ProfissionalSaudeId { get; set; }

        public ProfissionalSaudeDto ProfissionalSaude { get; set; }

        public string Observacao { get; set; }

        public long? FormRespostaId { get; set; }

        public long? FormConfigId { get; set; }

        public FormResposta FormResposta { get; set; }

        public long? OperacaoId { get; set; }

        public OperacaoDto Operacao { get; set; }

        public long? ProntuarioPrincipalId { get; set; }

        public ProntuarioEletronicoDto ProntuarioPrincipal { get; set; }

        public ICollection<ProntuarioEletronicoDto> SubProntuarios { get; set; }

        public ICollection<ProntuarioEletronicoLogDto> ProntuarioLogs { get; set; }

        public bool EstaInativo { get; set; }

        public long? InativacaoUserId { get; set; }

        public DateTime? InativacaoData { get; set; }

        public string InativacaoJustificativa { get; set; }

        public long? AtivacaoUserId { get; set; }

        public DateTime? AtivacaoData { get; set; }

        public string AtivacaoJustificativa { get; set; }
        public string FormConfigNome { get; set; }

        public long? LeitoId { get; set; }
        public LeitoDto Leito { get; set; }

        public static ProntuarioEletronicoDto Mapear(Prontuario entity)
        {
            if (entity == null) return null;
            var dto = MapearBase<ProntuarioEletronicoDto>(entity);

            dto.DataAdmissao = entity.DataAdmissao;
            dto.Observacao = entity.Observacao;
            dto.Atendimento = AtendimentoDto.Mapear(entity.Atendimento);
            dto.AtendimentoId = entity.AtendimentoId;
            dto.FormRespostaId = entity.FormRespostaId;
            dto.FormResposta = entity.FormResposta;
            dto.FormConfigId = entity.FormResposta?.FormConfigId;
            dto.OperacaoId = entity.OperacaoId;
            dto.Operacao = OperacaoDto.Mapear(entity.Operacao);
            dto.ProfissionalSaudeId = entity.ProfissionalSaudeId;
            dto.ProfissionalSaude = ProfissionalSaudeDto.Mapear(entity.ProfissionalSaude);
            dto.SubProntuarios = entity.SubProntuarios?.ToList().Select(x => Mapear(x)).ToList();
            dto.EstaInativo = entity.EstaInativo;
            dto.InativacaoUserId = entity.InativacaoUserId;
            dto.InativacaoData = entity.InativacaoData;
            dto.InativacaoJustificativa = entity.InativacaoJustificativa;
            dto.AtivacaoData = entity.AtivacaoData;
            dto.AtivacaoUserId = entity.AtivacaoUserId;
            dto.AtivacaoJustificativa = entity.AtivacaoJustificativa;
            dto.LeitoId = entity.LeitoId;
            dto.Leito = LeitoDto.Mapear(entity.Leito);

            return dto;
        }

        public static Prontuario Mapear(ProntuarioEletronicoDto dto)
        {
            if (dto == null) return null;
            var entity = MapearBase<Prontuario>(dto);

            entity.DataAdmissao = dto.DataAdmissao;
            entity.Observacao = dto.Observacao;            
            entity.AtendimentoId = dto.AtendimentoId;            
            entity.FormRespostaId = dto.FormRespostaId;
            entity.FormResposta = dto.FormResposta;            
            entity.OperacaoId = dto.OperacaoId;
            entity.ProfissionalSaudeId = dto.ProfissionalSaudeId;
            entity.EstaInativo = dto.EstaInativo;
            entity.InativacaoUserId = dto.InativacaoUserId;
            entity.InativacaoData = dto.InativacaoData;
            entity.InativacaoJustificativa = dto.InativacaoJustificativa;
            entity.ProntuarioPrincipalId = dto.ProntuarioPrincipalId;
            entity.UnidadeOrganizacionalId = dto.UnidadeOrganizacionalId;
            entity.AtivacaoData = dto.AtivacaoData;
            entity.AtivacaoUserId = dto.AtivacaoUserId;
            entity.AtivacaoJustificativa = dto.AtivacaoJustificativa;
            entity.LeitoId = dto.LeitoId;

            return entity;
        }
    }
}
