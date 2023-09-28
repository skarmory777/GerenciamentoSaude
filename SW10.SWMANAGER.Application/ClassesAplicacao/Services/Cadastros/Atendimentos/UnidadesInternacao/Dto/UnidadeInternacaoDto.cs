using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.UnidadesInternacao.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao.Dto
{
    [AutoMap(typeof(UnidadeInternacao))]
    public class UnidadeInternacaoDto : CamposPadraoCRUDDto
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string Localizacao { get; set; }

        public bool IsHospitalDia { get; set; }

        public bool IsAtivo { get; set; }

        public long? UnidadeInternacaoTipoId { get; set; }

        public virtual UnidadeInternacaoTipoDto UnidadeInternacaoTipo { get; set; }

        public static UnidadeInternacaoDto Mapear(UnidadeInternacao entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<UnidadeInternacaoDto>(entity);
            dto.Localizacao = entity.Localizacao;
            dto.IsHospitalDia = entity.IsHospitalDia;
            dto.IsAtivo = entity.IsAtivo;
            dto.UnidadeInternacaoTipoId = entity.UnidadeInternacaoTipoId;
            dto.UnidadeInternacaoTipo = UnidadeInternacaoTipoDto.Mapear(entity.UnidadeInternacaoTipo);

            return dto;
        }

        public static List<UnidadeInternacaoDto> Mapear(List<UnidadeInternacao> entityList)
        {
            var dtoList = new List<UnidadeInternacaoDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItemDto = Mapear(item);
                dtoList.Add(newItemDto);
            }

            return dtoList;
        }
    }
}
