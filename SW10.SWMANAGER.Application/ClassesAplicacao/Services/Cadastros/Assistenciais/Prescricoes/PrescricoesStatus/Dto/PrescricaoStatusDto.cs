using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus.Dto
{
    [AutoMap(typeof(PrescricaoStatus))]
    public class PrescricaoStatusDto : CamposPadraoCRUDDto, IDescricao
    {
        public string Cor { get; set; }

        public static PrescricaoStatusDto Mapear(PrescricaoStatus input)
        {
            var result = new PrescricaoStatusDto();
            result.Codigo = input.Codigo;
            result.Cor = input.Cor;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            return result;
        }

        public static PrescricaoStatus Mapear(PrescricaoStatusDto input)
        {
            var result = new PrescricaoStatus();
            result.Codigo = input.Codigo;
            result.Cor = input.Cor;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            return result;
        }

        public static IEnumerable<PrescricaoStatusDto> Mapear(List<PrescricaoStatus> input)
        {
            foreach (var item in input)
            {
                var result = new PrescricaoStatusDto();
                result.Codigo = item.Codigo;
                result.Cor = item.Cor;
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

        public static IEnumerable<PrescricaoStatus> Mapear(List<PrescricaoStatusDto> input)
        {
            foreach (var item in input)
            {
                var result = new PrescricaoStatus();
                result.Codigo = item.Codigo;
                result.Cor = item.Cor;
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
    }
}
