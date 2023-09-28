using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto
{
    [AutoMap(typeof(StatusSolicitacaoProcedimento))]
    public class StatusSolicitacaoProcedimentoDto : CamposPadraoCRUDDto
    {
        #region Mapeamento
        public static StatusSolicitacaoProcedimentoDto Mapear(StatusSolicitacaoProcedimento input)
        {
            var result = new StatusSolicitacaoProcedimentoDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            return result;
        }

        public static StatusSolicitacaoProcedimento Mapear(StatusSolicitacaoProcedimentoDto input)
        {
            var result = new StatusSolicitacaoProcedimento();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            return result;
        }

        public static IEnumerable<StatusSolicitacaoProcedimentoDto> Mapear(List<StatusSolicitacaoProcedimento> input)
        {
            foreach (var item in input)
            {
                var result = new StatusSolicitacaoProcedimentoDto();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                yield return result;
            }
        }

        public static IEnumerable<StatusSolicitacaoProcedimento> Mapear(List<StatusSolicitacaoProcedimentoDto> input)
        {
            foreach (var item in input)
            {
                var result = new StatusSolicitacaoProcedimento();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                yield return result;
            }
        }
        #endregion
    }
}
