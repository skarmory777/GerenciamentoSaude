using System;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Ocorrencias;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ocorrencias.Dto
{
    public class OcorrenciaDto : CamposPadraoCRUDDto
    {
        public DateTime Data { get; set; }
        public long TipoOcorrenciaId { get; set; }
        
        public TipoOcorrenciaDto TipoOcorrencia { get; set; }
        
        public string SourceModel { get; set; }
        
        public long? SourceId { get; set; }
        
        public string RelationModel { get; set; }
        
        public long? RelationId { get; set; }
        
        public string Texto { get; set; }

        public static OcorrenciaDto Mapear(Ocorrencia entity)
        {
            if (entity == null)
            {
                return null;
            }
            var dto = MapearBase<OcorrenciaDto>(entity);
            dto.Data = entity.Data;
            dto.Texto = entity.Texto;
            dto.SourceId = entity.SourceId;
            dto.RelationModel = entity.RelationModel;
            dto.RelationId = entity.RelationId;
            dto.SourceModel = entity.SourceModel;
            dto.TipoOcorrencia = TipoOcorrenciaDto.Mapear(entity.TipoOcorrencia);
            dto.TipoOcorrenciaId = entity.TipoOcorrenciaId;
            return dto;
        }
        
        public static Ocorrencia Mapear(OcorrenciaDto dto)
        {
            if (dto == null)
            {
                return null;
            }
            var entity = MapearBase<Ocorrencia>(dto);
            entity.Data = dto.Data;
            entity.Texto = dto.Texto;
            entity.SourceId = dto.SourceId;
            entity.SourceModel = dto.SourceModel;
            entity.TipoOcorrencia = TipoOcorrenciaDto.Mapear(dto.TipoOcorrencia);
            entity.TipoOcorrenciaId = dto.TipoOcorrenciaId;
            return entity;
        }
    }

    public class TipoOcorrenciaDto : CamposPadraoCRUDDto
    {
        public static TipoOcorrenciaDto Mapear(TipoOcorrencia entity)
        {
            return entity == null ? null : MapearBase<TipoOcorrenciaDto>(entity);
        }
        
        public static TipoOcorrencia Mapear(TipoOcorrenciaDto dto)
        {
            return dto == null ? null : MapearBase<TipoOcorrencia>(dto);
        }
    }
}