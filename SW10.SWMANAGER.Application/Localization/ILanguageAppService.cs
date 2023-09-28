using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.Localization.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.Localization
{
    public interface ILanguageAppService : IApplicationService
    {
        Task<GetLanguagesOutput> GetLanguages();

        Task<GetLanguageForEditOutput> GetLanguageForEdit(NullableIdDto input);

        Task CreateOrUpdateLanguage(CreateOrUpdateLanguageInput input);

        Task DeleteLanguage(EntityDto input);

        Task SetDefaultLanguage(SetDefaultLanguageInput input);

        Task<PagedResultDto<LanguageTextListDto>> GetLanguageTexts(GetLanguageTextsInput input);

        Task UpdateLanguageText(UpdateLanguageTextInput input);
    }
}
