using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Kits.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto
{
    [AutoMap(typeof(KitExame))]
    public class KitExameDto : CamposPadraoCRUDDto
    {
        public long? KitId { get; set; }
        public long? ExameId { get; set; }

        public bool IsLiberaKit { get; set; }

        public KitDto Kit { get; set; }

        //public ICollection<KitExameItem> KitExameItens { get; set; }

        public ExameDto Exame { get; set; }

        #region Mapeamento
        public static KitExameDto Mapear(KitExame input)
        {
            var result = new KitExameDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            result.ExameId = input.ExameId;
            result.IsLiberaKit = input.IsLiberaKit;
            result.KitId = input.KitId;

            if (input.Exame != null)
            {
                result.Exame = input.Exame.MapTo<ExameDto>();
            }

            if (input.Kit != null)
            {
                result.Kit = input.Kit.MapTo<KitDto>();
            }

            return result;
        }

        public static KitExame Mapear(KitExameDto input)
        {
            var result = new KitExame();
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

        public static IEnumerable<KitExameDto> Mapear(List<KitExame> input)
        {
            foreach (var item in input)
            {
                var result = Mapear(item);

                yield return result;
            }
        }

        public static IEnumerable<KitExame> Mapear(List<KitExameDto> input)
        {
            foreach (var item in input)
            {
                var result = Mapear(item);

                yield return result;
            }
        }
        #endregion

    }
}
