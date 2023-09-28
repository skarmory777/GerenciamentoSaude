using System.Collections;
using System.Collections.Generic;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Autorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes.Dto;
using System.Threading.Tasks;
namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Autorizacoes
{
    public interface IFaturamentoAutorizacaoAppService : IApplicationService
    {
        Task<PagedResultDto<FaturamentoAutorizacaoDto>> Listar(ListarAutorizacoesInput input);

        Task<PagedResultDto<FaturamentoAutorizacaoDetalheDto>> ListarDetalhes(ListarAutorizacoesInput input);

        Task CriarOuEditar(FaturamentoAutorizacaoDto input);

        Task SalvarDetalhe(FaturamentoAutorizacaoDetalhe input);

        Task Excluir(FaturamentoAutorizacaoDto input);

        Task<FaturamentoAutorizacaoDto> Obter(long id);

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        IEnumerable<FaturamentoAutorizacaoSolicitacaoItemDto> RetornaItensParaAutorizacao(FaturamentoAutorizacaoAppService.RetornaItensParaAutorizacaoFilterDto input);
    }
}
