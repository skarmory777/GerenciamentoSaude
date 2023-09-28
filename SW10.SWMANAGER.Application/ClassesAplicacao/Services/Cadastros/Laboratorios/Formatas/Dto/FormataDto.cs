using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Formatas.Dto
{
    [AutoMap(typeof(Formata))]
    public class FormataDto : CamposPadraoCRUDDto
    {
        public string Formatacao { get; set; }
        public string FormataItens { get; set; }

        public List<FormataItem> Itens { get; set; }

        #region Mapeamento
        public static FormataDto Mapear(Formata input)
        {
            var result = new FormataDto();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Formatacao = input.Formatacao;

            return result;
        }

        public static Formata Mapear(FormataDto input)
        {
            var result = new Formata();
            result.Codigo = input.Codigo;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;
            result.Formatacao = input.Formatacao;

            return result;
        }

        public static IEnumerable<FormataDto> Mapear(List<Formata> input)
        {
            foreach (var item in input)
            {
                var result = new FormataDto();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.Formatacao = item.Formatacao;

                yield return result;
            }
        }

        public static IEnumerable<Formata> Mapear(List<FormataDto> input)
        {
            foreach (var item in input)
            {
                var result = new Formata();
                result.Codigo = item.Codigo;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;
                result.Formatacao = item.Formatacao;

                yield return result;
            }
        }
        #endregion

    }
}
