using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto
{
    [AutoMap(typeof(LeitoServico))]
    public class LeitoServicoDto : CamposPadraoCRUDDto
    {
        [StringLength(10)]
        public string Codigo { get; set; }

        [StringLength(255)]
        public string Descricao { get; set; }

        [StringLength(10)]
        public string Ramal { get; set; }

        public static LeitoServicoDto Mapear(LeitoServico entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<LeitoServicoDto>(entity);
            dto.Ramal = entity.Ramal;
            
            return dto;
        }

        public static List<LeitoServicoDto> Mapear(List<LeitoServico> entityList)
        {
            var dtoList = new List<LeitoServicoDto>();

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