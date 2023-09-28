using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto
{
    [AutoMap(typeof(AutorizacaoProcedimentoItem))]
    public class AutorizacaoProcedimentoItemDto : CamposPadraoCRUDDto
    {
        public long? FaturamentoItemId { get; set; }
        public string Senha { get; set; }
        public DateTime? DataAutorizacao { get; set; }
        public bool IsOrtese { get; set; }
        public string AutorizadoPor { get; set; }
        public int? QuantidadeSolicitada { get; set; }
        public int? QuantidadeAutorizada { get; set; }
        public long? StatusId { get; set; }
        public string Observacao { get; set; }
        public long AutorizacaoProcedimentoId { get; set; }
        public AutorizacaoProcedimentoDto AutorizacaoProcedimento { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        public int IdGrid { get; set; }
        public string FaturamentoItemStr { get; set; }
        public bool ItemSelecionado { get; set; }
        public string StatusDescricao { get; set; }
        public string FaturamentoItemDescricao { get; set; }
        public StatusSolicitacaoProcedimentoDto StatusSolicitacaoProcedimento { get; set; }

        #region Mapeamento
        public static AutorizacaoProcedimentoItemDto Mapear(AutorizacaoProcedimentoItem input)
        {
            var result = new AutorizacaoProcedimentoItemDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            result.AutorizacaoProcedimentoId = input.AutorizacaoProcedimentoId;
            result.AutorizadoPor = input.AutorizadoPor;
            result.DataAutorizacao = input.DataAutorizacao;
            result.FaturamentoItemId = input.FaturamentoItemId;
            result.IsOrtese = input.IsOrtese;
            result.Observacao = input.Observacao;
            result.QuantidadeAutorizada = input.QuantidadeAutorizada;
            result.QuantidadeSolicitada = input.QuantidadeSolicitada;
            result.Senha = input.Senha;
            result.StatusId = input.StatusId;

            if (input.AutorizacaoProcedimento != null)
            {
                result.AutorizacaoProcedimento = AutorizacaoProcedimentoDto.Mapear(input.AutorizacaoProcedimento);
            }

            if (input.FaturamentoItem != null)
            {
                result.FaturamentoItem = FaturamentoItemDto.Mapear(input.FaturamentoItem);
            }

            if (input.StatusSolicitacaoProcedimento != null)
            {
                result.StatusSolicitacaoProcedimento = StatusSolicitacaoProcedimentoDto.Mapear(input.StatusSolicitacaoProcedimento);
            }

            return result;
        }

        public static AutorizacaoProcedimentoItem Mapear(AutorizacaoProcedimentoItemDto input)
        {
            var result = new AutorizacaoProcedimentoItem();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            result.AutorizacaoProcedimentoId = input.AutorizacaoProcedimentoId;
            result.AutorizadoPor = input.AutorizadoPor;
            result.DataAutorizacao = input.DataAutorizacao;
            result.FaturamentoItemId = input.FaturamentoItemId;
            result.IsOrtese = input.IsOrtese;
            result.Observacao = input.Observacao;
            result.QuantidadeAutorizada = input.QuantidadeAutorizada;
            result.QuantidadeSolicitada = input.QuantidadeSolicitada;
            result.Senha = input.Senha;
            result.StatusId = input.StatusId;

            return result;
        }

        public static IEnumerable<AutorizacaoProcedimentoItemDto> Mapear(List<AutorizacaoProcedimentoItem> input)
        {
            foreach (var item in input)
            {
                var result = new AutorizacaoProcedimentoItemDto();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                result.AutorizacaoProcedimentoId = item.AutorizacaoProcedimentoId;
                result.AutorizadoPor = item.AutorizadoPor;
                result.DataAutorizacao = item.DataAutorizacao;
                result.FaturamentoItemId = item.FaturamentoItemId;
                result.IsOrtese = item.IsOrtese;
                result.Observacao = item.Observacao;
                result.QuantidadeAutorizada = item.QuantidadeAutorizada;
                result.QuantidadeSolicitada = item.QuantidadeSolicitada;
                result.Senha = item.Senha;
                result.StatusId = item.StatusId;

                if (item.AutorizacaoProcedimento != null)
                {
                    result.AutorizacaoProcedimento = AutorizacaoProcedimentoDto.Mapear(item.AutorizacaoProcedimento);
                }

                if (item.FaturamentoItem != null)
                {
                    result.FaturamentoItem = FaturamentoItemDto.Mapear(item.FaturamentoItem);
                }

                if (item.StatusSolicitacaoProcedimento != null)
                {
                    result.StatusSolicitacaoProcedimento = StatusSolicitacaoProcedimentoDto.Mapear(item.StatusSolicitacaoProcedimento);
                }

                yield return result;
            }
        }

        public static IEnumerable<AutorizacaoProcedimentoItem> Mapear(List<AutorizacaoProcedimentoItemDto> input)
        {
            foreach (var item in input)
            {
                var result = new AutorizacaoProcedimentoItem();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                result.AutorizacaoProcedimentoId = item.AutorizacaoProcedimentoId;
                result.AutorizadoPor = item.AutorizadoPor;
                result.DataAutorizacao = item.DataAutorizacao;
                result.FaturamentoItemId = item.FaturamentoItemId;
                result.IsOrtese = item.IsOrtese;
                result.Observacao = item.Observacao;
                result.QuantidadeAutorizada = item.QuantidadeAutorizada;
                result.QuantidadeSolicitada = item.QuantidadeSolicitada;
                result.Senha = item.Senha;
                result.StatusId = item.StatusId;

                yield return result;
            }
        }
        #endregion
    }
}
