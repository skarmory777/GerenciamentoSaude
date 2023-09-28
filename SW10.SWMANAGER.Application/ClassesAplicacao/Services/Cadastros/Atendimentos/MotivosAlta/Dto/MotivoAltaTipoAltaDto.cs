using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosAlta.Dto
{
    [AutoMap(typeof(MotivoAltaTipoAlta))]
    public class MotivoAltaTipoAltaDto : CamposPadraoCRUDDto
    {

        public long? TabelaItemTissId { get; set; }

        [ForeignKey("TabelaItemTissId")]
        public virtual TabelaDominio TabelaDominio { get; set; }

        internal static MotivoAltaTipoAltaDto Mapear(MotivoAltaTipoAlta input)
        {
            if (input == null)
            {
                return null;
            }

            var dto = MotivoAltaTipoAltaDto.MapearBase<MotivoAltaTipoAltaDto>(input);

            return dto;
        }

        public static List<MotivoAltaTipoAltaDto> Mapear(List<MotivoAltaTipoAlta> entityList)
        {
            var dtoList = new List<MotivoAltaTipoAltaDto>();

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
