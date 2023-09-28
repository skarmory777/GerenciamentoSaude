using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Dto
{
    [AutoMap(typeof(TipoResultado))]
    public class TipoResultadoDto : CamposPadraoCRUDDto
    {

        #region Mapeamento
        public static TipoResultadoDto Mapear(TipoResultado input)
        {
            var result = new TipoResultadoDto();
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

        public static TipoResultado Mapear(TipoResultadoDto input)
        {
            var result = new TipoResultado();
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

        public static IEnumerable<TipoResultadoDto> Mapear(List<TipoResultado> input)
        {
            foreach (var item in input)
            {
                var result = new TipoResultadoDto();
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

        public static IEnumerable<TipoResultado> Mapear(List<TipoResultadoDto> input)
        {
            foreach (var item in input)
            {
                var result = new TipoResultado();
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
