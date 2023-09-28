using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Modelos.Dto;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    public interface IModeloPrescricaoAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);
        Task<PagedResultDto<ModelePrescricaoIndex>> Listar(ModeloPrescricaoListarInput input);
        Task<ModeloPrescricaoDto> Obter(long id);
    }
}
