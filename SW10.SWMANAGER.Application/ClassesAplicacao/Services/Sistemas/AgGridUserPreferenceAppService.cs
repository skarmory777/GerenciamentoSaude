using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Sistemas;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas
{
    public class AgGridUserPreferenceAppService : SWMANAGERAppServiceBase, IAgGridUserPreferenceAppService
    {
        public async Task<AgGridUserPreferenceDto> GetPreferences(string gridIdentifier)
        {
            using(var agGridUserPreferenceRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgGridUserPreference,long>>())
            {
                if (!AbpSession.UserId.HasValue)
                {
                    return null;
                }
                return AgGridUserPreferenceDto.Map(await agGridUserPreferenceRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.GridIdentifier == gridIdentifier && x.CreatorUserId == AbpSession.UserId));
            }
        }

        public async Task SavePreferences(AgGridUserPreferenceDto dto)
        {
            using (var agGridUserPreferenceRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgGridUserPreference, long>>())
            {
                var entity = await GetPreferences(dto.GridIdentifier);
                if (entity != null)
                {
                    entity.FilterModel = dto.FilterModel;
                    entity.SortModel = dto.SortModel;
                    entity.ColumnGroupState = dto.ColumnGroupState;
                    entity.ColumnState = dto.ColumnState;
                }
                else
                {
                    entity = dto;
                }

                if(entity != null)
                {
                    await agGridUserPreferenceRepository.Object.InsertOrUpdateAndGetIdAsync(AgGridUserPreferenceDto.Map(entity));
                }
            }
        }
    }

}
