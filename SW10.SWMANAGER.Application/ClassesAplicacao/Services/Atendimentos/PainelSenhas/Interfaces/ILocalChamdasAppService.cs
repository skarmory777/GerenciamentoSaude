using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces
{
    public interface ILocalChamadasAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarLocalChamadaDropdown(DropdownInput dropdownInput);
        Task<IResultDropdownList<long>> ListarLocalChamadaPorTipoDropdown(DropdownInput dropdownInput);
        Task<PagedResultDto<SenhaIndex>> ListarSenhasNaoChamadasIndex(ListarPainelSenhaInput input);
        Task AlterarTipoLocalChamadaSenha(long senhaMovAtualId, long tipoLocalChamadaNovoId);
        Task<LocalChamadaDto> Obter(long id);
    }
}
