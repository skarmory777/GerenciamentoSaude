using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta.Dto
{
    [AutoMap(typeof(MotivoAlta))]
    public class MotivoAltaDto : CamposPadraoCRUDDto
    {
        public long MotivoAltaTipoAltaId { get; set; }

        public MotivoAltaTipoAltaDto MotivoAltaTipoAlta { get; set; }


        public static MotivoAltaDto Mapear(MotivoAlta input)
        {
            if (input == null)
            {
                return null;
            }

            var dto = MapearBase<MotivoAltaDto>(input);

            dto.MotivoAltaTipoAltaId = input.MotivoAltaTipoAltaId;
            dto.MotivoAltaTipoAlta = MotivoAltaTipoAltaDto.Mapear(input.MotivoAltaTipoAlta);

            return dto;
        }

        public static List<MotivoAltaDto> Mapear(List<MotivoAlta> entityList)
        {
            var dtoList = new List<MotivoAltaDto>();

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
