using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto
{
    [AutoMap(typeof(TipoRespostaTipoRespostaConfiguracao))]
    public class TipoRespostaTipoRespostaConfiguracaoDto : CamposPadraoCRUDDto
    {
        public long TipoRespostaId { get; set; }
        public virtual TipoRespostaDto TipoResposta { get; set; }
        public long TipoRespostaConfiguracaoId { get; set; }
        public virtual TipoRespostaConfiguracaoDto TipoRespostaConfiguracao { get; set; }

        public static TipoRespostaTipoRespostaConfiguracao Mapear(TipoRespostaTipoRespostaConfiguracaoDto dto)
        {
            if (dto == null) return null;

            var entity = MapearBase<TipoRespostaTipoRespostaConfiguracao>(dto);
            entity.TipoRespostaId = dto.TipoRespostaId;
            entity.TipoRespostaConfiguracaoId = dto.TipoRespostaConfiguracaoId;
            entity.TipoResposta = TipoRespostaDto.Mapear(dto.TipoResposta);
            entity.TipoRespostaConfiguracao = TipoRespostaConfiguracaoDto.Mapear(dto.TipoRespostaConfiguracao);

            return entity;
        }

        public static TipoRespostaTipoRespostaConfiguracaoDto Mapear(TipoRespostaTipoRespostaConfiguracao entity)
        {
            if (entity == null) return null;

            var dto = MapearBase<TipoRespostaTipoRespostaConfiguracaoDto>(entity);
            dto.TipoRespostaId = entity.TipoRespostaId;
            dto.TipoRespostaConfiguracaoId = entity.TipoRespostaConfiguracaoId;
            dto.TipoResposta = TipoRespostaDto.Mapear(entity.TipoResposta);
            dto.TipoRespostaConfiguracao = TipoRespostaConfiguracaoDto.Mapear(entity.TipoRespostaConfiguracao);

            return dto;
        }

        public static List<TipoRespostaTipoRespostaConfiguracao> Mapear(List<TipoRespostaTipoRespostaConfiguracaoDto> dtoList)
        {
            var entityList = new List<TipoRespostaTipoRespostaConfiguracao>();

            if (dtoList == null) return null;

            foreach (var item in dtoList)
            {
                var newItem = Mapear(item);
                entityList.Add(newItem);
            }

            return entityList;
        }

        public static List<TipoRespostaTipoRespostaConfiguracaoDto> Mapear(List<TipoRespostaTipoRespostaConfiguracao> entityList)
        {
            var dtoList = new List<TipoRespostaTipoRespostaConfiguracaoDto>();

            if (entityList == null) return null;

            foreach (var item in entityList)
            {
                var newItem = Mapear(item);
                dtoList.Add(newItem);
            }

            return dtoList;
        }
    }
}
