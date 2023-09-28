using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Sistemas
{
    public interface IAgGridUserPreferenceAppService: IApplicationService
    {
        Task<AgGridUserPreferenceDto> GetPreferences(string gridIdentifier);

        Task SavePreferences(AgGridUserPreferenceDto dto);
    }

}
