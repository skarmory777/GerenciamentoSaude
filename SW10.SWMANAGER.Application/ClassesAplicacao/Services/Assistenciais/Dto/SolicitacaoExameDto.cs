using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(SolicitacaoExame))]
    public class SolicitacaoExameDto : CamposPadraoCRUDDto
    {
        public long? AtendimentoId { get; set; }
        public AtendimentoDto Atendimento { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public long? OrigemId { get; set; }
        public long? LeitoId { get; set; }
        public LeitoDto Leito { get; set; }
        public int Prioridade { get; set; }
        public long? UnidadeOrganizacionalId { get; set; }
        public UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }
        public long? MedicoSolicitanteId { get; set; }
        public MedicoDto MedicoSolicitante { get; set; }
        public string Observacao { get; set; }
        public long? PrescricaoId { get; set; }
        public PrescricaoMedicaDto Prescricao { get; set; }
        public string Itens { get; set; }
        public List<SolicitacaoExameItemDto> SolicitacaoItens { get; set; }
        public string Sexo { get; set; }
        public string Plano { get; set; }
        public string Origem { get; set; }
        public string Justificativa { get; set; }

        
        public long? PendenciaUserId { get; set; }
        public DateTime? PendenciaDateTime { get; set; }
        
        public string MotivoPendencia { get; set; }
       

        #region Mapeamento

        public static SolicitacaoExameDto Mapear(SolicitacaoExame solicitacaoExame)
        {
            var solicitacaoExameDto = new SolicitacaoExameDto
            {
                Id = solicitacaoExame.Id,
                Codigo = solicitacaoExame.Codigo,
                Descricao = solicitacaoExame.Descricao,
                AtendimentoId = solicitacaoExame.AtendimentoId,
                DataSolicitacao = solicitacaoExame.DataSolicitacao,
                OrigemId = solicitacaoExame.OrigemId,
                PendenciaUserId = solicitacaoExame.PendenciaUserId,
                MotivoPendencia = solicitacaoExame.MotivoPendencia,
                PendenciaDateTime = solicitacaoExame.PendenciaDateTime,
                UnidadeOrganizacionalId = solicitacaoExame.UnidadeOrganizacionalId,
                MedicoSolicitanteId = solicitacaoExame.MedicoSolicitanteId,
                Observacao = solicitacaoExame.Observacao,
                PrescricaoId = solicitacaoExame.PrescricaoId,
                Justificativa = solicitacaoExame.Justificativa,
                LeitoId = solicitacaoExame.LeitoId,
                Prioridade = solicitacaoExame.Prioridade
            };


            if (solicitacaoExame.Atendimento != null)
            {
                solicitacaoExameDto.Atendimento = AtendimentoDto.Mapear(solicitacaoExame.Atendimento);
            }

            if (solicitacaoExame.Leito != null)
            {
                solicitacaoExameDto.Leito = LeitoDto.Mapear(solicitacaoExame.Leito);
            }

            if (solicitacaoExame.UnidadeOrganizacional != null)
            {
                solicitacaoExameDto.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(solicitacaoExame.UnidadeOrganizacional);
            }

            if (solicitacaoExame.MedicoSolicitante != null)
            {
                solicitacaoExameDto.MedicoSolicitante = MedicoDto.Mapear(solicitacaoExame.MedicoSolicitante);
            }

            return solicitacaoExameDto;
        }

        public static SolicitacaoExame Mapear(SolicitacaoExameDto solicitacaoExameDto)
        {
            var solicitacaoExame = new SolicitacaoExame
            {
                Id = solicitacaoExameDto.Id,
                Codigo = solicitacaoExameDto.Codigo,
                Descricao = solicitacaoExameDto.Descricao,
                AtendimentoId = solicitacaoExameDto.AtendimentoId,
                DataSolicitacao = solicitacaoExameDto.DataSolicitacao,
                OrigemId = solicitacaoExameDto.OrigemId,
                LeitoId = solicitacaoExameDto.LeitoId,
                Prioridade = solicitacaoExameDto.Prioridade,
                UnidadeOrganizacionalId = solicitacaoExameDto.UnidadeOrganizacionalId,
                MedicoSolicitanteId = solicitacaoExameDto.MedicoSolicitanteId,
                Observacao = solicitacaoExameDto.Observacao,
                PrescricaoId = solicitacaoExameDto.PrescricaoId,
                Justificativa = solicitacaoExameDto.Justificativa,
                PendenciaUserId = solicitacaoExameDto.PendenciaUserId,
                MotivoPendencia = solicitacaoExameDto.MotivoPendencia,
                PendenciaDateTime = solicitacaoExameDto.PendenciaDateTime
            };

            if (solicitacaoExameDto.Atendimento != null)
            {
                solicitacaoExame.Atendimento = AtendimentoDto.Mapear(solicitacaoExameDto.Atendimento);
            }

            if (solicitacaoExameDto.Leito != null)
            {
                solicitacaoExame.Leito = LeitoDto.Mapear(solicitacaoExameDto.Leito);
            }

            if (solicitacaoExameDto.UnidadeOrganizacional != null)
            {
                solicitacaoExame.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(solicitacaoExameDto.UnidadeOrganizacional);
            }

            if (solicitacaoExameDto.MedicoSolicitante != null)
            {
                solicitacaoExame.MedicoSolicitante = MedicoDto.Mapear(solicitacaoExameDto.MedicoSolicitante);
            }

            return solicitacaoExame;
        }

        public static IEnumerable<SolicitacaoExameDto> Mapear(List<SolicitacaoExame> solicitacaoExame)
        {
            foreach (var item in solicitacaoExame)
            {
                var solicitacaoExameDto = new SolicitacaoExameDto
                {
                    Id = item.Id,
                    Codigo = item.Codigo,
                    Descricao = item.Descricao,
                    AtendimentoId = item.AtendimentoId,
                    DataSolicitacao = item.DataSolicitacao,
                    OrigemId = item.OrigemId,
                    LeitoId = item.LeitoId,
                    Prioridade = item.Prioridade,
                    UnidadeOrganizacionalId = item.UnidadeOrganizacionalId,
                    MedicoSolicitanteId = item.MedicoSolicitanteId,
                    Observacao = item.Observacao,
                    PrescricaoId = item.PrescricaoId,
                    PendenciaUserId = item.PendenciaUserId,
                    MotivoPendencia = item.MotivoPendencia,
                    PendenciaDateTime = item.PendenciaDateTime
                };

                if (item.Atendimento != null)
                {
                    solicitacaoExameDto.Atendimento = AtendimentoDto.Mapear(item.Atendimento);
                }

                if (item.Leito != null)
                {
                    solicitacaoExameDto.Leito = LeitoDto.Mapear(item.Leito);
                }

                if (item.UnidadeOrganizacional != null)
                {
                    solicitacaoExameDto.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(item.UnidadeOrganizacional);
                }

                if (item.MedicoSolicitante != null)
                {
                    solicitacaoExameDto.MedicoSolicitante = MedicoDto.Mapear(item.MedicoSolicitante);
                }

                yield return solicitacaoExameDto;
            }
        }

        public static IEnumerable<SolicitacaoExame> Mapear(List<SolicitacaoExameDto> solicitacaoExameDto)
        {
            foreach (var item in solicitacaoExameDto)
            {
                var solicitacaoExame = new SolicitacaoExame
                {
                    Id = item.Id,
                    Codigo = item.Codigo,
                    Descricao = item.Descricao,
                    AtendimentoId = item.AtendimentoId,
                    DataSolicitacao = item.DataSolicitacao,
                    OrigemId = item.OrigemId,
                    LeitoId = item.LeitoId,
                    Prioridade = item.Prioridade,
                    UnidadeOrganizacionalId = item.UnidadeOrganizacionalId,
                    MedicoSolicitanteId = item.MedicoSolicitanteId,
                    Observacao = item.Observacao,
                    PrescricaoId = item.PrescricaoId,
                    PendenciaUserId = item.PendenciaUserId,
                    MotivoPendencia = item.MotivoPendencia,
                    PendenciaDateTime = item.PendenciaDateTime
                };

                if (item.Atendimento != null)
                {
                    solicitacaoExame.Atendimento = AtendimentoDto.Mapear(item.Atendimento);
                }

                if (item.Leito != null)
                {
                    solicitacaoExame.Leito = LeitoDto.Mapear(item.Leito);
                }

                if (item.UnidadeOrganizacional != null)
                {
                    solicitacaoExame.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(item.UnidadeOrganizacional);
                }

                if (item.MedicoSolicitante != null)
                {
                    solicitacaoExame.MedicoSolicitante = MedicoDto.Mapear(item.MedicoSolicitante);
                }

                yield return solicitacaoExame;
            }
        }

        #endregion
    }
}
