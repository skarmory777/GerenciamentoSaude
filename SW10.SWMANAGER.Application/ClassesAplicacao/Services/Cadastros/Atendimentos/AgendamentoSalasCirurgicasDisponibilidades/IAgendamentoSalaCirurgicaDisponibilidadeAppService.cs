using Abp.Application.Services;

using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoSalasCirurgicasDisponibilidades.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoSalasCirurgicasDisponibilidades
{
    public interface IAgendamentoSalaCirurgicaDisponibilidadeAppService : IApplicationService
    {
        Task<List<AgendamentoSalaCirurgicaDisponibilidadeDto>> ListarAtivos(long? salaCirurgicaId, long? tipoCirurgiaId, long? empresaId);
        Task<IResultDropdownList<long>> ListarTiposCirurugiasDisponiveisDropdown(DropdownInput dropdownInput);
        Task<ICollection<AgendamentoSalaCirurgicaDisponibilidadeDto>> ListarPorData(DateTime date);
        Task<ICollection<AgendamentoSalaCirurgicaDisponibilidade>> ListarPorSalaTipoCirurgia(DateTime date, long salaId, long tipoCirurgiaId);
        Task<GenericoIdNome> ObterSomenteUmTipoCirurgia(long salaId);
        Task<AgendamentoSalaCirurgicaDisponibilidade> ObterProximoAgendamento(DateTime date, long salaId, long tipoCirurgiaId, DateTime hora);
        Task<AgendamentoSalaCirurgicaDisponibilidadeDto> Obter(long id);
    }
}
