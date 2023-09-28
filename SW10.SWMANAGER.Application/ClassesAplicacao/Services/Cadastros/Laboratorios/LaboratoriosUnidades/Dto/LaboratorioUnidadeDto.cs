using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto
{
    [AutoMap(typeof(LaboratorioUnidade))]
    public class LaboratorioUnidadeDto : CamposPadraoCRUDDto
    {

        #region Mapeamento
        public static LaboratorioUnidadeDto Mapear(LaboratorioUnidade input)
        {
            var result = new LaboratorioUnidadeDto();
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

        public static LaboratorioUnidade Mapear(LaboratorioUnidadeDto input)
        {
            var result = new LaboratorioUnidade();
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

        public static IEnumerable<LaboratorioUnidadeDto> Mapear(List<LaboratorioUnidade> input)
        {
            foreach (var item in input)
            {
                var result = new LaboratorioUnidadeDto();
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

        public static IEnumerable<LaboratorioUnidade> Mapear(List<LaboratorioUnidadeDto> input)
        {
            foreach (var item in input)
            {
                var result = new LaboratorioUnidade();
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
