using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.InternacoesTev;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(TevRisco))]
    public class TevRiscoDto : CamposPadraoCRUDDto
    {
        public static TevRiscoDto Mapear(TevRisco input)
        {
            var result = new TevRiscoDto();
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

        public static TevRisco Mapear(TevRiscoDto input)
        {
            var result = new TevRisco();
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

        public static IEnumerable<TevRiscoDto> Mapear(List<TevRisco> input)
        {
            foreach (var item in input)
            {
                var result = new TevRiscoDto();
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

        public static IEnumerable<TevRisco> Mapear(List<TevRiscoDto> input)
        {
            foreach (var item in input)
            {
                var result = new TevRisco();
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
    }
}
