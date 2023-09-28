using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Medicos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto
{
    [AutoMap(typeof(AutorizacaoProcedimento))]
    public class AutorizacaoProcedimentoDto : CamposPadraoCRUDDto
    {
        public long? SolicitanteId { get; set; }
        public long? PacienteId { get; set; }
        public long ConvenioId { get; set; }
        public long FaturamentoItemId { get; set; }
        public string Autorizacao { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string AutorizadoPor { get; set; }
        public string Observacao { get; set; }
        public string NumeroGuia { get; set; }
        public bool IsOstese { get; set; }

        public MedicoDto Solicitante { get; set; }
        public PacienteDto Paciente { get; set; }
        public ConvenioDto Convenio { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public string Itens { get; set; }

        public bool IsProrrogacao { get; set; }

        public List<ComentarioAutorizacaoProcedimentoDto> Comentarios { get; set; }
        public string FormaAutorizacao { get; set; }
        public string DadosContato { get; set; }

        public long? AtendimentoId { get; set; }
        public AtendimentoDto Atendimento { get; set; }


        #region Mapeamento
        public static AutorizacaoProcedimentoDto Mapear(AutorizacaoProcedimento input)
        {
            var result = new AutorizacaoProcedimentoDto();
            result.Codigo = input.Codigo;
            result.ConvenioId = input.ConvenioId;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataSolicitacao = input.DataSolicitacao;
            result.Id = input.Id;
            result.Descricao = input.Descricao;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Observacao = input.Observacao;
            result.PacienteId = input.PacienteId;
            result.SolicitanteId = input.SolicitanteId;
            result.AtendimentoId = input.AtendimentoId;

            if (input.Convenio != null)
            {
                result.Convenio = ConvenioDto.Mapear(input.Convenio);
            }

            if (input.Paciente != null)
            {
                result.Paciente = PacienteDto.Mapear(input.Paciente);
            }

            if (input.Solicitante != null)
            {
                result.Solicitante = MedicoDto.Mapear(input.Solicitante);
            }

            if (input.Atendimento != null)
            {
                result.Atendimento = AtendimentoDto.Mapear(input.Atendimento);
            }

            return result;
        }

        public static AutorizacaoProcedimento Mapear(AutorizacaoProcedimentoDto input)
        {
            var result = new AutorizacaoProcedimento();
            result.Codigo = input.Codigo;
            result.ConvenioId = input.ConvenioId;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.DataSolicitacao = input.DataSolicitacao;
            result.Id = input.Id;
            result.Descricao = input.Descricao;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Observacao = input.Observacao;
            result.PacienteId = input.PacienteId;
            result.SolicitanteId = input.SolicitanteId;
            result.AtendimentoId = input.AtendimentoId;

            return result;
        }

        public static IEnumerable<AutorizacaoProcedimentoDto> Mapear(List<AutorizacaoProcedimento> list)
        {
            foreach (var input in list)
            {
                var result = new AutorizacaoProcedimentoDto();
                result.Codigo = input.Codigo;
                result.ConvenioId = input.ConvenioId;
                result.CreationTime = input.CreationTime;
                result.CreatorUserId = input.CreatorUserId;
                result.DataSolicitacao = input.DataSolicitacao;
                result.Id = input.Id;
                result.Descricao = input.Descricao;
                result.IsSistema = input.IsSistema;
                result.LastModificationTime = input.LastModificationTime;
                result.LastModifierUserId = input.LastModifierUserId;
                result.Observacao = input.Observacao;
                result.PacienteId = input.PacienteId;
                result.SolicitanteId = input.SolicitanteId;
                result.AtendimentoId = input.AtendimentoId;

                if (input.Convenio != null)
                {
                    result.Convenio = ConvenioDto.Mapear(input.Convenio);
                }

                if (input.Paciente != null)
                {
                    result.Paciente = PacienteDto.Mapear(input.Paciente);
                }

                if (input.Solicitante != null)
                {
                    result.Solicitante = MedicoDto.Mapear(input.Solicitante);
                }

                if (input.Atendimento != null)
                {
                    result.Atendimento = AtendimentoDto.Mapear(input.Atendimento);
                }

                yield return result;
            }
        }

        public static IEnumerable<AutorizacaoProcedimento> Mapear(List<AutorizacaoProcedimentoDto> list)
        {
            foreach (var input in list)
            {
                var result = new AutorizacaoProcedimento();
                result.Codigo = input.Codigo;
                result.ConvenioId = input.ConvenioId;
                result.CreationTime = input.CreationTime;
                result.CreatorUserId = input.CreatorUserId;
                result.DataSolicitacao = input.DataSolicitacao;
                result.Id = input.Id;
                result.Descricao = input.Descricao;
                result.IsSistema = input.IsSistema;
                result.LastModificationTime = input.LastModificationTime;
                result.LastModifierUserId = input.LastModifierUserId;
                result.Observacao = input.Observacao;
                result.PacienteId = input.PacienteId;
                result.SolicitanteId = input.SolicitanteId;

                yield return result;
            }
        }
        #endregion

    }
}
