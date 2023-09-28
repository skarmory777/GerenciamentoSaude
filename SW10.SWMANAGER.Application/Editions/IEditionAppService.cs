using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Editions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Editions
{
    public interface IEditionAppService : IApplicationService
    {
        Task<ListResultDto<EditionListDto>> GetEditions();

        Task<GetEditionForEditOutput> GetEditionForEdit(NullableIdDto input);

        Task CreateOrUpdateEdition(CreateOrUpdateEditionDto input);

        Task DeleteEdition(EntityDto input);

        Task<List<ComboboxItemDto>> GetEditionComboboxItems(int? selectedEditionId = null);
    }
}