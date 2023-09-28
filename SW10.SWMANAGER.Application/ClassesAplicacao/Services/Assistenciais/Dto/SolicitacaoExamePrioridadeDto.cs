using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    [AutoMap(typeof(SolicitacaoExamePrioridade))]
    public class SolicitacaoExamePrioridadeDto : CamposPadraoCRUDDto
    {
        public const long Rotina = 1;
        public const long Urgencia = 2; 
        public static SolicitacaoExamePrioridadeDto Mapear(SolicitacaoExamePrioridade input)
        {
            var result = new SolicitacaoExamePrioridadeDto
            {
                Codigo = input.Codigo,
                CreationTime = input.CreationTime,
                CreatorUserId = input.CreatorUserId,
                Descricao = input.Descricao,
                Id = input.Id,
                IsSistema = input.IsSistema,
                LastModificationTime = input.LastModificationTime,
                LastModifierUserId = input.LastModifierUserId
            };

            return result;
        }

        public static SolicitacaoExamePrioridade Mapear(SolicitacaoExamePrioridadeDto input)
        {
            var result = new SolicitacaoExamePrioridade
            {
                Codigo = input.Codigo,
                CreationTime = input.CreationTime,
                CreatorUserId = input.CreatorUserId,
                Descricao = input.Descricao,
                Id = input.Id,
                IsSistema = input.IsSistema,
                LastModificationTime = input.LastModificationTime,
                LastModifierUserId = input.LastModifierUserId
            };

            return result;
        }

        public static IEnumerable<SolicitacaoExamePrioridadeDto> Mapear(List<SolicitacaoExamePrioridade> input)
        {
            foreach (var item in input)
            {
                var result = new SolicitacaoExamePrioridadeDto
                {
                    Codigo = item.Codigo,
                    CreationTime = item.CreationTime,
                    CreatorUserId = item.CreatorUserId,
                    Descricao = item.Descricao,
                    Id = item.Id,
                    IsSistema = item.IsSistema,
                    LastModificationTime = item.LastModificationTime,
                    LastModifierUserId = item.LastModifierUserId
                };

                yield return result;
            }
        }

        public static IEnumerable<SolicitacaoExamePrioridade> Mapear(List<SolicitacaoExamePrioridadeDto> input)
        {
            foreach (var item in input)
            {
                var result = new SolicitacaoExamePrioridade
                {
                    Codigo = item.Codigo,
                    CreationTime = item.CreationTime,
                    CreatorUserId = item.CreatorUserId,
                    Descricao = item.Descricao,
                    Id = item.Id,
                    IsSistema = item.IsSistema,
                    LastModificationTime = item.LastModificationTime,
                    LastModifierUserId = item.LastModifierUserId
                };

                yield return result;
            }
        }
    }
}
