using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.CentralAtendimentos
{
    public interface IAutorizacaoProcedimentoAppService : IApplicationService
    {
        Task<PagedResultDto<AutorizacaoProcedimentoIndexDto>> ListarAutorizacao(ListarAutorizacaoProcedimentoInput input);
        PagedResultDto<AutorizacaoProcedimentoItemDto> ListarItensJson(string json);
        DefaultReturn<AutorizacaoProcedimentoDto> CriarOuEditarAutorizacaoProcedimento(AutorizacaoProcedimentoDto input);
        Task<AutorizacaoProcedimentoDto> Obter(long id);
        Task<List<AutorizacaoProcedimentoItemDto>> ObterItens(long id);
        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);
        Task<PagedResultDto<AutorizacaoProcedimentoIndexDto>> ListarProrrogacaoInternacao(ListarAutorizacaoProcedimentoInput input);
        DefaultReturn<AutorizacaoProcedimentoDto> CriarOuEditarProrrogacaoInternacao(AutorizacaoProcedimentoDto input);
        Task<AutorizacaoProcedimentoDto> ObterProrrogacaoPorAtendimento(long atendimentoId);
    }
}
