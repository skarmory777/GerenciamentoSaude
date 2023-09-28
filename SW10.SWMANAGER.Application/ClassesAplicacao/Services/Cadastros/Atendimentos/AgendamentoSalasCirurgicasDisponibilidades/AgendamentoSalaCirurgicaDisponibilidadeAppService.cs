using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoSalasCirurgicasDisponibilidades.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoSalasCirurgicasDisponibilidades
{
    public class AgendamentoSalaCirurgicaDisponibilidadeAppService : SWMANAGERAppServiceBase, IAgendamentoSalaCirurgicaDisponibilidadeAppService
    {
        public async Task<List<AgendamentoSalaCirurgicaDisponibilidadeDto>> ListarAtivos(long? salaCirurgicaId, long? tipoCirurgiaId, long? empresaId)
        {
            using (var _agendamentoSalaCirurgicaDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoSalaCirurgicaDisponibilidade, long>>())
            using (var _userAppService = IocManager.Instance.ResolveAsDisposable<IUserAppService>())
            {

                var userId = AbpSession.UserId;
                var userEmpresas = await _userAppService.Object.GetUserEmpresas(userId.Value);

                var empresaIds = (userEmpresas != null && userEmpresas.Items != null) ? userEmpresas.Items.Select(s => s.Id) : new List<long>();

                var disponibilidade = await _agendamentoSalaCirurgicaDisponibilidadeRepository.Object.GetAll()
                                                            .Include(i => i.Intervalo)
                                                            .Include(i => i.SalaCirurgica)
                                                            .Include(i => i.TipoCirurgia)
                                                            .Where(w => (salaCirurgicaId == null || w.SalaCirurgicaId == salaCirurgicaId)
                                                                                            && (tipoCirurgiaId == null || w.TipoCirurgiaId == tipoCirurgiaId)
                                                                                            && (empresaId == null || w.EmpresaId == empresaId)
                                                                                            && (empresaIds.Any(a => a == w.EmpresaId))
                                                            ).ToListAsync();

                var disponibilidadeDto = AgendamentoSalaCirurgicaDisponibilidadeDto.Mapear(disponibilidade);

                return disponibilidadeDto;
            }
        }

        public async Task<IResultDropdownList<long>> ListarTiposCirurugiasDisponiveisDropdown(DropdownInput dropdownInput)
        {
            using (var _agendamentoSalaCirurgicaDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoSalaCirurgicaDisponibilidade, long>>())
            {
                long salaId;
                long.TryParse(dropdownInput.filtro, out salaId);


                return await base.ListarDropdownLambda(dropdownInput
                                       , _agendamentoSalaCirurgicaDisponibilidadeRepository.Object
                                       , m => ((AgendamentoSalaCirurgicaDisponibilidade)m).SalaCirurgicaId == salaId
                                         && (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                         || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                        , p => new DropdownItems { id = p.TipoCirurgia.Id, text = string.Concat(p.TipoCirurgia.Codigo.ToString(), " - ", p.TipoCirurgia.Descricao) }
                                                         , o => o.TipoCirurgia.Descricao
                                                         );
            }
        }

        public async Task<ICollection<AgendamentoSalaCirurgicaDisponibilidadeDto>> ListarPorData(DateTime date)
        {
            try
            {
                using (var _agendamentoSalaCirurgicaDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoSalaCirurgicaDisponibilidade, long>>())
                {

                    var dayOfWeek = (int)date.DayOfWeek;

                    var query = await _agendamentoSalaCirurgicaDisponibilidadeRepository.Object
                        .GetAll()
                        .Include(m => m.Intervalo)
                        .Include(m => m.SalaCirurgica)
                        .Include(m => m.TipoCirurgia)
                        .Where(m =>
                        m.DataInicio <= date
                        && m.DataFim >= date
                        && (dayOfWeek == 0
                        ? m.Domingo == true
                        : dayOfWeek == 1
                        ? m.Segunda == true
                        : dayOfWeek == 2
                        ? m.Terca == true
                        : dayOfWeek == 3
                        ? m.Quarta == true
                        : dayOfWeek == 4
                        ? m.Quinta == true
                        : dayOfWeek == 5
                        ? m.Sexta == true
                        : m.Sabado == true)
                        )
                        .ToListAsync();

                    return AgendamentoSalaCirurgicaDisponibilidadeDto.Mapear(query);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ICollection<AgendamentoSalaCirurgicaDisponibilidade>> ListarPorSalaTipoCirurgia(DateTime date, long salaId, long tipoCirurgiaId)
        {
            try
            {
                using (var _agendamentoSalaCirurgicaDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoSalaCirurgicaDisponibilidade, long>>())
                {
                    var dayOfWeek = (int)date.DayOfWeek;

                    return await _agendamentoSalaCirurgicaDisponibilidadeRepository.Object
                        .GetAll()
                        .Include(m => m.Intervalo)
                        .Include(m => m.SalaCirurgica)
                        .Include(m => m.TipoCirurgia)
                        .Where(m =>
                        m.DataInicio <= date
                        && m.DataFim >= date

                        && m.SalaCirurgicaId == salaId
                        && m.TipoCirurgiaId == tipoCirurgiaId

                        && (dayOfWeek == 0
                        ? m.Domingo == true
                        : dayOfWeek == 1
                        ? m.Segunda == true
                        : dayOfWeek == 2
                        ? m.Terca == true
                        : dayOfWeek == 3
                        ? m.Quarta == true
                        : dayOfWeek == 4
                        ? m.Quinta == true
                        : dayOfWeek == 5
                        ? m.Sexta == true
                        : m.Sabado == true)
                        )
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<GenericoIdNome> ObterSomenteUmTipoCirurgia(long salaId)
        {
            try
            {
                using (var _agendamentoSalaCirurgicaDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoSalaCirurgicaDisponibilidade, long>>())
                {

                    var tiposCirurugias = await _agendamentoSalaCirurgicaDisponibilidadeRepository.Object.GetAll()
                                                                   .Include(i => i.TipoCirurgia)
                                                                   .Where(w => w.SalaCirurgicaId == salaId)
                                                                   .ToListAsync();


                    if (tiposCirurugias != null && tiposCirurugias.Count == 1 && tiposCirurugias[0].TipoCirurgia != null)
                    {
                        return new GenericoIdNome { Id = tiposCirurugias[0].TipoCirurgia.Id, Nome = tiposCirurugias[0].TipoCirurgia?.Descricao };
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<AgendamentoSalaCirurgicaDisponibilidade> ObterProximoAgendamento(DateTime date, long salaId, long tipoCirurgiaId, DateTime hora)
        {
            try
            {
                using (var _agendamentoSalaCirurgicaDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoSalaCirurgicaDisponibilidade, long>>())
                {
                    var dayOfWeek = (int)date.DayOfWeek;

                    return await _agendamentoSalaCirurgicaDisponibilidadeRepository.Object
                        .GetAll()
                        .Include(m => m.Intervalo)
                        .Include(m => m.SalaCirurgica)
                        .Include(m => m.TipoCirurgia)
                        .Where(m =>
                        m.DataInicio <= date
                        && m.DataFim >= date

                        && m.SalaCirurgicaId == salaId
                        && m.TipoCirurgiaId == tipoCirurgiaId

                        && m.HoraInicio > hora

                        && (dayOfWeek == 0
                        ? m.Domingo == true
                        : dayOfWeek == 1
                        ? m.Segunda == true
                        : dayOfWeek == 2
                        ? m.Terca == true
                        : dayOfWeek == 3
                        ? m.Quarta == true
                        : dayOfWeek == 4
                        ? m.Quinta == true
                        : dayOfWeek == 5
                        ? m.Sexta == true
                        : m.Sabado == true)
                        )
                        .OrderBy(o => o.HoraInicio)
                        .FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<AgendamentoSalaCirurgicaDisponibilidadeDto> Obter(long id)
        {
            using (var _agendamentoSalaCirurgicaDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoSalaCirurgicaDisponibilidade, long>>())
            {
                var query = await _agendamentoSalaCirurgicaDisponibilidadeRepository.Object.GetAll()
                                                                          .Include(i => i.Empresa)
                                                                          .Where(w => w.Id == id)
                                                                          .FirstOrDefaultAsync();

                return AgendamentoSalaCirurgicaDisponibilidadeDto.Mapear(query);
            }
        }
    }
}
