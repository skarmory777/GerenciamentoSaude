using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;

namespace SW10.SWMANAGER.Organizations.Dto
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    [AutoMapTo(typeof(OrganizationUnit))]
    public class OrganizationUnitDto : AuditedEntityDto<long>
    {
        public long? ParentId { get; set; }

        public string Code { get; set; }

        public string DisplayName { get; set; }

        public int MemberCount { get; set; }


        #region Mapeamento

        public static OrganizationUnit Mapear(OrganizationUnitDto organizationUnitDto)
        {
            if (organizationUnitDto == null)
            {
                return null;
            }

            var organizationUnit = new OrganizationUnit
            {
                Code = organizationUnitDto.Code,
                DisplayName = organizationUnitDto.DisplayName,
                Id = organizationUnitDto.Id,
                ParentId = organizationUnitDto.ParentId
            };


            return organizationUnit;
        }


        public static OrganizationUnitDto Mapear(OrganizationUnit organizationUnit)
        {
            if (organizationUnit == null)
            {
                return null;
            }

            var organizationUnitDto = new OrganizationUnitDto
            {
                Code = organizationUnit.Code,
                DisplayName = organizationUnit.DisplayName,
                Id = organizationUnit.Id,
                ParentId = organizationUnit.ParentId
            };


            return organizationUnitDto;
        }

        #endregion

    }
}