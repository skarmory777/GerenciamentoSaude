using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Atestados;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto
{
    [AutoMap(typeof(ModeloAtestado))]
    public class ModeloAtestadoDto : CamposPadraoCRUDDto
    {
        public string Titulo { get; set; }
        public string Conteudo { get; set; }

        public static ModeloAtestadoDto Mapear(ModeloAtestado entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<ModeloAtestadoDto>(entity);
            dto.Titulo = entity.Titulo;
            dto.Conteudo = entity.Conteudo;

            return dto;
        }

        public static ModeloAtestado Mapear(ModeloAtestadoDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<ModeloAtestado>(dto);
            entity.Titulo = dto.Titulo;
            entity.Conteudo = dto.Conteudo;

            return entity;
        }

        public static List<ModeloAtestadoDto> Mapear(List<ModeloAtestado> entityList)
        {
            var dtoList = new List<ModeloAtestadoDto>();

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
