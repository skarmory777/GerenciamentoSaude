using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto
{
    [AutoMap(typeof(LeitoCaracteristica))]
    public class LeitoCaracteristicaDto : CamposPadraoCRUDDto
    {
        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        [StringLength(10)]
        public string Ramal { get; set; }

        public static LeitoCaracteristicaDto Mapear(LeitoCaracteristica entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<LeitoCaracteristicaDto>(entity);
            dto.Ramal = entity.Ramal;

            return dto;
        }

        public static List<LeitoCaracteristicaDto> Mapear(List<LeitoCaracteristica> entityList)
        {
            var dtoList = new List<LeitoCaracteristicaDto>();

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