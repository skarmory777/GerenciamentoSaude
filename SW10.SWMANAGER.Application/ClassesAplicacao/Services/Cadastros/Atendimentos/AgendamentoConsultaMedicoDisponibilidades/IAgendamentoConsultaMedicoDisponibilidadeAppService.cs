using Abp.Application.Services;
using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades
{
    public interface IAgendamentoConsultaMedicoDisponibilidadeAppService : IApplicationService
    {
        Task<PagedResultDto<AgendamentoConsultaMedicoDisponibilidadeDto>> Listar(ListarAgendamentoConsultaMedicoDisponibilidadesInput input);

        Task CriarOuEditar(CriarOuEditarAgendamentoConsultaMedicoDisponibilidade input);

        Task Excluir(CriarOuEditarAgendamentoConsultaMedicoDisponibilidade input);

        Task<CriarOuEditarAgendamentoConsultaMedicoDisponibilidade> Obter(long id);

        Task<AgendamentoConsultaMedicoDisponibilidadeDto> Obter(long medicoId, long medicoEspecialidadeId, DateTime horaAgendamento);

        Task<FileDto> ListarParaExcel(ListarAgendamentoConsultaMedicoDisponibilidadesInput input);

        Task<ICollection<AgendamentoConsultaMedicoDisponibilidade>> ListarPorData(DateTime date);

        Task<ListResultDto<AgendamentoConsultaMedicoDisponibilidadeDto>> ListarTodos();

        Task<List<AgendamentoConsultaMedicoDisponibilidade>> ListarAtivos(long? medicoId, long? especialidadeId);

        Task<IResultDropdownList<long>> ListarMedicosDisponiveisDropdown(DropdownInput dropdownInput);

        Task<IResultDropdownList<long>> ListarEspecialidadesMedicoDropdown(DropdownInput dropdownInput);

        Task<ICollection<AgendamentoConsultaMedicoDisponibilidade>> ListarPorDataMedicoEspecialidade(DateTime date, long medicoId, long especialidadeId);

        Task<GenericoIdNome> ObterSomenteUmaEspecialidade(long medicoId);

    }
}
