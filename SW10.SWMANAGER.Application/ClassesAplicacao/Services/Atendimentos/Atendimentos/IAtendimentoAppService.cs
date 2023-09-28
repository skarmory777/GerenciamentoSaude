using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos
{
    using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
    using System.Linq;

    public interface IAtendimentoAppService : IApplicationService
    {
        Task<DefaultReturn<AtendimentoDto>> CriarOuEditar(AtendimentoDto input);

        Task SetTomadaDecisao(SetAltaInput input);

        Task SetAlta(SetAltaInput input);

        Task SetAltaMedica(SetAltaInput input);

        Task Excluir(long id, long? motivoCancelamentoId = null);

        List<Atendimento> ListarUltimos2Atendimentos(long? id);

        Task<AtendimentoDto> Obter(long id);

        Task<AtendimentoDto> ObterIQ(IQueryable<Atendimento> iQueryableAtendimento);

        Task<AtendimentoDto> ObterAssistencial(long id);

        //Task<AtendimentoDto> ObterPorLeito(long id);

        AtendimentoDto ObterPorLeito(long id);

        Task<long> CriarNovoAtendimento();

        Task<FileDto> ListarParaExcel(ListarAtendimentosInput input);

        Task<PagedResultDto<AtendimentoDto>> ListarTodos();

        Task<PagedResultDto<AtendimentoDto>> ListarParaInternacao();

        Task<PagedResultDto<AtendimentoIndexDto>> ListarAtendimentos(ListarAtendimentosInput input);

        Task<PagedResultDto<AtendimentoDto>> ListarFiltroPreAtendimento(ListarAtendimentosInput input);

        Task<PagedResultDto<AtendimentoDto>> ListarPorPaciente(ListarInput input);

        Task<PagedResultDto<AtendimentoIndexDto>> ListarFiltro(ListarAtendimentosInput input);

        Task<PagedResultDto<AtendimentoIndexDto>> ListarFiltroInternacao(ListarAtendimentosInput input);

        Task<PagedResultDto<SolicitacaoExameIndex>> GetDetalhamentoExamesSolicitacao(DetalhamentoExameInput input);

        Task<PagedResultDto<SolicitacaoExameItemList>> GetDetalhamentoExameItemSolicitacao(DetalhamentoExameInput input);

        Task<PagedResultDto<SolicitacaoExameIndex>> GetDetalhamentoExamesResultado(DetalhamentoExameInput input);

        Task<PagedResultDto<SolicitacaoExameItemList>> GetDetalhamentoExameItemResultado(DetalhamentoExameInput input);

        Task<PagedResultDto<GenericoIdNome>> ListarAtendimentoPaciente();

        Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarAtendimentosEmAbertoDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarAtendimentosComSaidaDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarAtendimentosAmbulatorioInternacao(DropdownInput dropdownInput);

        Task SetDataPrevistaAlta(SetAltaInput input);

        Task<IResultDropdownList<long>> ListarAtendimentosSemAlta(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarPacientesSemAlta(DropdownInput dropdownInput);

        Task Reativar(long id, long? leitoId);

        Task<ListResultDto<VWRptAtendimentoDetalhadoDto>> ListarAtendimentoDetalhadoReport(DateTime startDate, DateTime endDate, long empresaId, long pacienteId, long convenioId, long medicoId, long especialidadeId, long unidadeOrganizacionalId, int tipoAtendimento, int tipoRel = 2, int tipoPeriodo = 1);

        Task<ListResultDto<VWRptAtendimentoResumidoDto>> ListarAtendimentoResumidoReport(DateTime startDate, DateTime endDate, long empresaId, long pacienteId, long convenioId, long medicoId, long especialidadeId, long unidadeOrganizacionalId, int tipoAtendimento, int tipoRel = 2, int tipoPeriodo = 1);

        Task<long?> ObterAtendindimentoAbertoPaciente(long pacienteId);

        Task AtualizarAssistencial(long id, long? protocoloAtendimentoId, long? classificacaoRiscoId);

        Task AtualizarStatusAssistencial(long id, long? stausId, bool isPedenteExame, bool isPedenteMedicacao, bool isPedenteProcedimento, int? statusAguardando, int? statusAtendido);
        Task CancelarAlta(long atedimentoId);

        Task<AtendimentoDto> ObterAtendimentoLeitoPaciente(long id);
        Task AlterarMedicoAtendimento(long atendimentoId);

        Task<AtendimentoDto> ObterComPacienteEndereco(long id);

        Task<AtendimentoDto> ObterParaProrrogacao(long id);

        Task<PacienteMedicoDto> ObterPacienteMedico(long id);

        Task<long> TotalExamesResultados(long id,string tipo);
        Task<long> TotalExamesSolicitados(long id, string tipo);

    }
}
