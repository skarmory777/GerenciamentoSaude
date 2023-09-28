using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Castle.Core.Internal;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.FormasAplicacao.Dto;
using System.Collections.Generic;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao.Dto
{
    [AutoMap(typeof(VelocidadeInfusao))]
    public class VelocidadeInfusaoDto : CamposPadraoCRUDDto, IDescricao
    {
        public ICollection<VelocidadeInfusaoFormaAplicacaoDto> FormaAplicacao { get; set; }

        public static VelocidadeInfusaoDto Mapear(VelocidadeInfusao input)
        {
            if(input == null)
            {
                return null;
            }

            var item =  MapearBase<VelocidadeInfusaoDto>(input);
            item.FormaAplicacao = VelocidadeInfusaoFormaAplicacaoDto.Mapear(input.FormaAplicacao);
            return item;
        }

        public static VelocidadeInfusao Mapear(VelocidadeInfusaoDto input)
        {
            if (input == null)
            {
                return null;
            }

            var item = MapearBase<VelocidadeInfusao>(input);
            item.FormaAplicacao = VelocidadeInfusaoFormaAplicacaoDto.Mapear(input.FormaAplicacao);
            return item;
        }

        public static IEnumerable<VelocidadeInfusaoDto> Mapear(List<VelocidadeInfusao> input)
        {
            if (!input.IsNullOrEmpty())
            {
                foreach (var item in input)
                {
                    yield return Mapear(item);
                }
            }
        }

        public static IEnumerable<VelocidadeInfusao> Mapear(List<VelocidadeInfusaoDto> input)
        {
            if (!input.IsNullOrEmpty())
            {
                foreach (var item in input)
                {
                    yield return Mapear(item);
                }
            }
        }

    }

    public class VelocidadeInfusaoFormaAplicacaoDto : FullAuditedEntityDto<long>
    {
        public long VelocidadeInfusaoId { get; set; }

        public VelocidadeInfusaoDto VelocidadeInfusao { get; set; }

        public long FormaApplicacaoId { get; set; }

        public FormaAplicacaoDto FormaApplicacao { get; set; }

        public static ICollection<VelocidadeInfusaoFormaAplicacaoDto> Mapear(ICollection<VelocidadeInfusaoFormaAplicacao> input)
        {
            if (input.IsNullOrEmpty())
            {
                return null;
            }
            else
            {
                return input.ToList().Select(x => Mapear(x)).ToList();
            }
        }

        public static ICollection<VelocidadeInfusaoFormaAplicacao> Mapear(ICollection<VelocidadeInfusaoFormaAplicacaoDto> input)
        {
            if (input.IsNullOrEmpty())
            {
                return null;
            }
            else
            {
                return input.ToList().Select(x => Mapear(x)).ToList();
            }
        }

        public static VelocidadeInfusaoFormaAplicacaoDto Mapear(VelocidadeInfusaoFormaAplicacao input)
        {
            if(input == null)
            {
                return null;
            }
            return new VelocidadeInfusaoFormaAplicacaoDto
            {
                Id = input.Id,
                CreationTime = input.CreationTime,
                CreatorUserId = input.CreatorUserId,
                DeleterUserId = input.DeleterUserId,
                DeletionTime = input.DeletionTime,
                IsDeleted = input.IsDeleted,
                LastModificationTime = input.LastModificationTime,
                LastModifierUserId = input.LastModifierUserId,
                FormaApplicacaoId = input.FormaApplicacaoId,
                VelocidadeInfusaoId = input.VelocidadeInfusaoId,
                //FormaApplicacao = FormaAplicacaoDto.Mapear(input.FormaApplicacao),
                //VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(input.VelocidadeInfusao)
            };
        }

        public static VelocidadeInfusaoFormaAplicacao Mapear(VelocidadeInfusaoFormaAplicacaoDto input)
        {
            if (input == null)
            {
                return null;
            }

            return new VelocidadeInfusaoFormaAplicacao
            {
                Id = input.Id,
                CreationTime = input.CreationTime,
                CreatorUserId = input.CreatorUserId,
                DeleterUserId = input.DeleterUserId,
                DeletionTime = input.DeletionTime,
                IsDeleted = input.IsDeleted,
                LastModificationTime = input.LastModificationTime,
                LastModifierUserId = input.LastModifierUserId,
                FormaApplicacaoId = input.FormaApplicacaoId,
                VelocidadeInfusaoId = input.VelocidadeInfusaoId,
                FormaApplicacao = FormaAplicacaoDto.Mapear(input.FormaApplicacao),
                VelocidadeInfusao = VelocidadeInfusaoDto.Mapear(input.VelocidadeInfusao)
            };
        }
    }

}
