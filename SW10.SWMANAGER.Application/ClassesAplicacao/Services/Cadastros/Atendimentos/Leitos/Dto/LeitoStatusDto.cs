using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto
{
    [AutoMap(typeof(LeitoStatus))]
    public class LeitoStatusDto : CamposPadraoCRUDDto
    {
        [StringLength(7)]
        public string Cor { get; set; }

        public bool IsBloqueioAtendimento { get; set; }

        #region Mapeamento
        public static LeitoStatusDto Mapear(LeitoStatus input)
        {
            var result = new LeitoStatusDto();
            result.Codigo = input.Codigo;
            result.Cor = input.Cor;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsBloqueioAtendimento = input.IsBloqueioAtendimento;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            return result;
        }

        public static LeitoStatus Mapear(LeitoStatusDto input)
        {
            var result = new LeitoStatus();
            result.Codigo = input.Codigo;
            result.Cor = input.Cor;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsBloqueioAtendimento = input.IsBloqueioAtendimento;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            return result;
        }

        public static IEnumerable<LeitoStatusDto> Mapear(List<LeitoStatus> list)
        {
            foreach (var input in list)
            {
                var result = new LeitoStatusDto();
                result.Codigo = input.Codigo;
                result.Cor = input.Cor;
                result.CreationTime = input.CreationTime;
                result.CreatorUserId = input.CreatorUserId;
                result.Descricao = input.Descricao;
                result.Id = input.Id;
                result.IsBloqueioAtendimento = input.IsBloqueioAtendimento;
                result.IsSistema = input.IsSistema;
                result.LastModificationTime = input.LastModificationTime;
                result.LastModifierUserId = input.LastModifierUserId;

                yield return result;
            }
        }

        public static IEnumerable<LeitoStatus> Mapear(List<LeitoStatusDto> list)
        {
            foreach (var input in list)
            {
                var result = new LeitoStatus();
                result.Codigo = input.Codigo;
                result.Cor = input.Cor;
                result.CreationTime = input.CreationTime;
                result.CreatorUserId = input.CreatorUserId;
                result.Descricao = input.Descricao;
                result.Id = input.Id;
                result.IsBloqueioAtendimento = input.IsBloqueioAtendimento;
                result.IsSistema = input.IsSistema;
                result.LastModificationTime = input.LastModificationTime;
                result.LastModifierUserId = input.LastModifierUserId;

                yield return result;
            }
        }

        #endregion
    }
}
