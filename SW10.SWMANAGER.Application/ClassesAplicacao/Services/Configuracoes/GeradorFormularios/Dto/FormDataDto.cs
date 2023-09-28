using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.GeradorFormularios.Dto
{
    [AutoMap(typeof(FormData))]
    public class FormDataDto : CamposPadraoCRUDDto
    {
        public long ColConfigId { get; set; }

        public string Valor { get; set; }
        public ColConfigDto Coluna { get; set; }

        public static FormDataDto Mapear(FormData entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<FormDataDto>(entity);

            dto.Valor = entity.Valor;
            dto.ColConfigId = entity.ColConfigId;
            dto.Coluna = ColConfigDto.Mapear(entity.Coluna);

            return dto;
        }

        public static FormDataDto MapearValores(FormData entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dto = MapearBase<FormDataDto>(entity);

            dto.Valor = entity.Valor;
            dto.ColConfigId = entity.ColConfigId;

            return dto;
        }

        public static FormData MapearEntidade(FormDataDto dto)
        {
            if (dto == null)
            {
                return null;
            }

            var entity = new FormData();

            entity.Id = dto.Id;
            entity.Codigo = dto.Codigo;
            entity.Descricao = dto.Descricao;
            entity.IsDeleted = dto.IsDeleted;
            entity.DeleterUserId = dto.DeleterUserId;
            entity.DeletionTime = dto.DeletionTime;
            entity.CreatorUserId = dto.CreatorUserId;
            entity.CreationTime = dto.CreationTime;
            entity.IsSistema = dto.IsSistema;
            entity.LastModificationTime = dto.LastModificationTime;
            entity.LastModifierUserId = dto.LastModifierUserId;

            entity.Valor = dto.Valor;
            entity.ColConfigId = dto.ColConfigId;
            entity.Coluna = ColConfigDto.MapearEntidade(dto.Coluna);

            return entity;
        }
        //public FormRespostaDto Resposta { get; set; }
    }
}