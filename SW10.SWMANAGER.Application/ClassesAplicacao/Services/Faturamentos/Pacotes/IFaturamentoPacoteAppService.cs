using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes.Dtos;

using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes
{
    public interface IFaturamentoPacoteAppService : IApplicationService
    {
        FaturamentoPacoteDto Obter(long id);
        Task<PagedResultDto<PacoteDto>> ListarPacotesPorConta(ListarFaturamentoPacoteInput input);

        Task<PagedResultDto<FaturamentoContaMedicaPacoteDto>> ListarPacotesPorContaMedica(FaturamentoContaPacoteFilterDto input);

        Task<IResultDropdownList<long>> ListarDropdownPacoteContaMedica(DropdownInput dropdownInput);
        
        Task<IResultDropdownList<long>> ListarDropdownPacoteContaMedicaPorPacote(DropdownInput dropdownInput);
        
        DefaultReturn<FaturamentoPacoteDto> InserirPacote(FaturamentoPacoteDto pacoteDto);
        Task<IResultDropdownList<long>> ListarDropdownPacoteConta(DropdownInput dropdownInput);
        Task ExcluirPacote(long id);

        Task<DefaultReturn<DefaultReturnBool>> ExcluirItemsPacote(long contaMedicaId, List<long> itemIds, bool excluirDiversosPacotes = false);
    }
}
