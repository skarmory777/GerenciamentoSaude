using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.InternacoesTev;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(TevMovimento))]
    public class TevMovimentoDto : CamposPadraoCRUDDto
    {
        public DateTime Data { get; set; }

        public long? RiscoId { get; set; }
        public TevRiscoDto Risco { get; set; }

        public string Observacao { get; set; }

        public long? AtendimentoId { get; set; }
        public AtendimentoDto Atendimento { get; set; }

        public static TevMovimentoDto Mapear(TevMovimento input)
        {
            var result = new TevMovimentoDto();
            result.AtendimentoId = input.AtendimentoId;
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Data = input.Data;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Observacao = input.Observacao;
            result.RiscoId = input.RiscoId;

            if (input.Atendimento != null)
            {
                result.Atendimento = AtendimentoDto.Mapear(input.Atendimento);
            }

            if (input.Risco != null)
            {
                result.Risco = TevRiscoDto.Mapear(input.Risco);
            }

            return result;
        }

        public static TevMovimento Mapear(TevMovimentoDto input)
        {
            var result = new TevMovimento();
            result.AtendimentoId = input.AtendimentoId;
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Data = input.Data;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Observacao = input.Observacao;
            result.RiscoId = input.RiscoId;

            if (input.Atendimento != null)
            {
                result.Atendimento = AtendimentoDto.Mapear(input.Atendimento);
            }

            if (input.Risco != null)
            {
                result.Risco = TevRiscoDto.Mapear(input.Risco);
            }

            return result;
        }

        public static IEnumerable<TevMovimentoDto> Mapear(List<TevMovimento> input)
        {
            foreach (var item in input)
            {
                var result = new TevMovimentoDto();
                result.AtendimentoId = item.AtendimentoId;
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Data = item.Data;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.Observacao = item.Observacao;
                result.RiscoId = item.RiscoId;

                if (item.Atendimento != null)
                {
                    result.Atendimento = AtendimentoDto.Mapear(item.Atendimento);
                }

                if (item.Risco != null)
                {
                    result.Risco = TevRiscoDto.Mapear(item.Risco);
                }

                yield return result;
            }
        }

        public static IEnumerable<TevMovimento> Mapear(List<TevMovimentoDto> input)
        {
            foreach (var item in input)
            {
                var result = new TevMovimento();
                result.AtendimentoId = item.AtendimentoId;
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Data = item.Data;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.Observacao = item.Observacao;
                result.RiscoId = item.RiscoId;

                if (item.Atendimento != null)
                {
                    result.Atendimento = AtendimentoDto.Mapear(item.Atendimento);
                }

                if (item.Risco != null)
                {
                    result.Risco = TevRiscoDto.Mapear(item.Risco);
                }

                yield return result;
            }
        }
    }
}
