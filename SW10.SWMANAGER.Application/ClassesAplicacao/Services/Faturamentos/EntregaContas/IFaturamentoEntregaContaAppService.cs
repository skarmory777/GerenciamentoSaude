using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContas
{
    public interface IFaturamentoEntregaContaAppService : IApplicationService
    {
        Task RemoverDoLote(long id);

        Task CancelarEntregas(CrudEntregaContaInput input);

        Task<PagedResultDto<FaturamentoEntregaContaDto>> Listar(ListarEntregasInput input);

        Task<PagedResultDto<FaturamentoEntregaContaDto>> ListarConferidas(ListarEntregasInput input);

        Task<PagedResultDto<FaturamentoEntregaContaDto>> ListarEntregues(ListarEntregasInput input);

        Task<PagedResultDto<FaturamentoEntregaContaDto>> ListarParaLotesGerados(ListarEntregasInput input);

        Task CriarOuEditar(FaturamentoEntregaContaDto input);

        Task CriarVarias(CrudEntregaContaInput input);

        Task Excluir(FaturamentoEntregaContaDto input);

        Task<FaturamentoEntregaContaDto> Obter(long id);

        Task<FileDto> ListarParaExcel(ListarEntregasInput input);

        Task<ListResultDto<FaturamentoEntregaContaDto>> ListarTodos();

        Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput);

        Task<PagedResultDto<ContasQuitacaoPacienteDto>> ListarContasQuitacaoPaciente(ContasQuitacaoPacienteInput input);

        Task<bool> AlterarValorRecebidoAtual(long id, float newValue);

        Task<bool> AlterarValorGlosaRecuperavelTemp(long id, float newValue);

        Task<bool> AlterarValorGlosaIrrecuperavelTemp(long id, float newValue);
    }
}

