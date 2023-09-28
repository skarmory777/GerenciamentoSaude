using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Origens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto
{
    [AutoMap(typeof(Origem))]
    public class OrigemDto : CamposPadraoCRUDDto
    {
        public string Descricao { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public virtual UnidadeOrganizacionalDto UnidadeOrganizacional { get; set; }

        public bool IsAtivo { get; set; }

        //public virtual ICollection<PacienteDto> Pacientes { get; set; }

        public static OrigemDto Mapear(Origem input)
        {
            if (input == null)
            {
                return null;
            }

            var result = MapearBase<OrigemDto>(input);

            result.Codigo = input.Codigo;
            result.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsAtivo = input.IsAtivo;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            if (input.UnidadeOrganizacional != null)
            {
                result.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(input.UnidadeOrganizacional);
            }

            return result;
        }

        public static Origem Mapear(OrigemDto input)
        {
            var result = new Origem();
            result.Codigo = input.Codigo;
            result.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
            result.CreationTime = input.CreationTime;
            result.CreatorUserId = input.CreatorUserId;
            result.Descricao = input.Descricao;
            result.Id = input.Id;
            result.IsAtivo = input.IsAtivo;
            result.IsSistema = input.IsSistema;
            result.LastModificationTime = input.LastModificationTime;
            result.LastModifierUserId = input.LastModifierUserId;

            if (input.UnidadeOrganizacional != null)
            {
                result.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(input.UnidadeOrganizacional);
            }

            return result;
        }

        public static IEnumerable<OrigemDto> Mapear(List<Origem> input)
        {
            foreach (var item in input)
            {
                var result = new OrigemDto();
                result.Codigo = item.Codigo;
                result.UnidadeOrganizacionalId = item.UnidadeOrganizacionalId;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsAtivo = item.IsAtivo;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                if (item.UnidadeOrganizacional != null)
                {
                    result.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(item.UnidadeOrganizacional);
                }

                yield return result;
            }
        }

        public static IEnumerable<Origem> Mapear(List<OrigemDto> input)
        {
            foreach (var item in input)
            {
                var result = new Origem();
                result.Codigo = item.Codigo;
                result.UnidadeOrganizacionalId = item.UnidadeOrganizacionalId;
                result.CreationTime = item.CreationTime;
                result.CreatorUserId = item.CreatorUserId;
                result.Descricao = item.Descricao;
                result.Id = item.Id;
                result.IsAtivo = item.IsAtivo;
                result.IsSistema = item.IsSistema;
                result.LastModificationTime = item.LastModificationTime;
                result.LastModifierUserId = item.LastModifierUserId;

                if (item.UnidadeOrganizacional != null)
                {
                    result.UnidadeOrganizacional = UnidadeOrganizacionalDto.Mapear(item.UnidadeOrganizacional);
                }

                yield return result;
            }
        }

    }
}
