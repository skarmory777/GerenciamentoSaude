using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Sistemas;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas.Dto
{
    public class AgGridUserPreferenceDto : FullAuditedEntityDto<long>
    {
        public string GridIdentifier { get; set; }
        public string ColumnState { get; set; }

        public string ColumnGroupState { get; set; }

        public string SortModel { get; set; }

        public string FilterModel { get; set; }

        public static AgGridUserPreference Map(AgGridUserPreferenceDto dto)
        {
            if( dto == null)
            {
                return null;
            }

            return new AgGridUserPreference
            {
                Id = dto.Id,
                GridIdentifier = dto.GridIdentifier,
                ColumnGroupState = dto.ColumnGroupState,
                ColumnState = dto.ColumnState,
                SortModel = dto.SortModel,
                FilterModel = dto.FilterModel,
                CreationTime = dto.CreationTime,
                CreatorUserId = dto.CreatorUserId,
                DeleterUserId = dto.DeleterUserId,
                DeletionTime = dto.DeletionTime,
                IsDeleted = dto.IsDeleted,
                LastModificationTime = dto.LastModificationTime,
                LastModifierUserId = dto.LastModifierUserId,
            };
        }

        public static AgGridUserPreferenceDto Map(AgGridUserPreference entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new AgGridUserPreferenceDto
            {
                Id = entity.Id,
                GridIdentifier = entity.GridIdentifier,
                ColumnGroupState = entity.ColumnGroupState,
                ColumnState = entity.ColumnState,
                SortModel = entity.SortModel,
                FilterModel = entity.FilterModel,
                CreationTime = entity.CreationTime,
                CreatorUserId = entity.CreatorUserId,
                DeleterUserId = entity.DeleterUserId,
                DeletionTime = entity.DeletionTime,
                IsDeleted = entity.IsDeleted,
                LastModificationTime = entity.LastModificationTime,
                LastModifierUserId = entity.LastModifierUserId,
            };
        }
    }
}
