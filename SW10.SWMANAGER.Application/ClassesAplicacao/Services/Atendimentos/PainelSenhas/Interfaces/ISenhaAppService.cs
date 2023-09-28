using Abp.Application.Services;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces
{
    public interface ISenhaAppService : IApplicationService
    {
        Task<IResultDropdownList<long>> ListarSenhasPorlocalChamadaDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarSenhasNaoChamadas(DropdownInput dropdownInput);
        MonitorChamadaIndex CarregarPainelSenha(long painelId);
        Task<SenhaDto> Obter(long id);
        Task<SenhaMovimentacaoDto> ObterMovimento(long id);
        Task CriarMovimento(long atendimentoId, long tipoLocalChamadaId);
        Task<IResultDropdownList<long>> ListarSenhasPorlocalChamadaAtendimentoDropdown(DropdownInput dropdownInput);
    }
}
