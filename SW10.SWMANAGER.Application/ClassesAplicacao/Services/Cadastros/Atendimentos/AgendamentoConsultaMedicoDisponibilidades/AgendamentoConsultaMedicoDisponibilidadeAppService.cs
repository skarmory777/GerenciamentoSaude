using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades.Exporting;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.AgendamentoConsultaMedicoDisponibilidades
{

    public class AgendamentoConsultaMedicoDisponibilidadeAppService : SWMANAGERAppServiceBase, IAgendamentoConsultaMedicoDisponibilidadeAppService
    {
        public async Task CriarOuEditar(CriarOuEditarAgendamentoConsultaMedicoDisponibilidade input)
        {
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                {
                    var agendamentoConsultaMedicoDisponibilidade = CriarOuEditarAgendamentoConsultaMedicoDisponibilidade.Mapear(input);

                    if (input.Id.Equals(0))
                    {
                        await _agendamentoConsultaMedicoDisponibilidadeRepository.Object.InsertAsync(agendamentoConsultaMedicoDisponibilidade);
                    }
                    else
                    {
                        await _agendamentoConsultaMedicoDisponibilidadeRepository.Object.UpdateAsync(agendamentoConsultaMedicoDisponibilidade);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(CriarOuEditarAgendamentoConsultaMedicoDisponibilidade input)
        {
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                    await _agendamentoConsultaMedicoDisponibilidadeRepository.Object.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<AgendamentoConsultaMedicoDisponibilidadeDto>> ListarTodos()
        {
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                {
                    var query = await _agendamentoConsultaMedicoDisponibilidadeRepository.Object
                    .GetAll()
                    .Include(m => m.Intervalo)
                    .Include(m => m.Medico)
                    .Include(m => m.Medico.SisPessoa)
                    .Include(m => m.MedicoEspecialidade.Especialidade)
                    .ToListAsync();

                    var agendamentoConsultaMedicoDisponibilidadesDto = AgendamentoConsultaMedicoDisponibilidadeDto.Mapear(query);

                    return new ListResultDto<AgendamentoConsultaMedicoDisponibilidadeDto> { Items = agendamentoConsultaMedicoDisponibilidadesDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<List<AgendamentoConsultaMedicoDisponibilidade>> ListarAtivos(long? medicoId, long? especialidadeId)
        {
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                {
                    return await _agendamentoConsultaMedicoDisponibilidadeRepository.Object
                    .GetAll()
                    .Include(m => m.Intervalo)
                    .Include(m => m.Medico)
                    .Include(m => m.Medico.SisPessoa)
                    .Include(m => m.MedicoEspecialidade.Especialidade)
                    .WhereIf(medicoId.HasValue && medicoId > 0, m => m.MedicoId == medicoId)
                    .WhereIf(especialidadeId.HasValue && especialidadeId > 0, m => m.MedicoEspecialidade.EspecialidadeId == especialidadeId)
                    //.GetAllListAsync(m => m.DataFim >= DateTime.Now)
                    .ToListAsync().ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<AgendamentoConsultaMedicoDisponibilidadeDto>> Listar(ListarAgendamentoConsultaMedicoDisponibilidadesInput input)
        {
            var contarAgendamentoConsultaMedicoDisponibilidades = 0;
            List<AgendamentoConsultaMedicoDisponibilidade> agendamentoConsultaMedicoDisponibilidades;
            List<AgendamentoConsultaMedicoDisponibilidadeDto> agendamentoConsultaMedicoDisponibilidadesDtos = new List<AgendamentoConsultaMedicoDisponibilidadeDto>();
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                {
                    var query = _agendamentoConsultaMedicoDisponibilidadeRepository.Object
                    .GetAll()
                    .Include(m => m.Intervalo)
                    .Include(m => m.Medico)
                    .Include(m => m.Medico.SisPessoa)
                    .Include(m => m.MedicoEspecialidade.Especialidade);
                    //.WhereIf(!input..IsNullOrEmpty(), m =>
                    //    m.Medico.NomeCompleto.ToUpper().Contains(input.Filtro.ToUpper()) ||
                    //    m.MedicoEspecialidade.Especialidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
                    //    m.DataInicio.ToString("dd/MM/yyyy").ToUpper().Contains(input.Filtro.ToUpper()) ||
                    //    m.DataFim.ToString("dd/MM/yyyy").ToUpper().Contains(input.Filtro.ToUpper()) ||
                    //    m.HoraFim.ToString("hh:mm:ss").ToUpper().Contains(input.Filtro.ToUpper()) ||
                    //    m.HoraInicio.ToString("hh:mm:ss").ToUpper().Contains(input.Filtro.ToUpper())
                    //);

                    contarAgendamentoConsultaMedicoDisponibilidades = await query
                        .CountAsync();

                    agendamentoConsultaMedicoDisponibilidades = await query
                        .AsNoTracking()
                        .OrderBy(input.Sorting)
                        .PageBy(input)
                        .ToListAsync();

                    agendamentoConsultaMedicoDisponibilidadesDtos = AgendamentoConsultaMedicoDisponibilidadeDto.Mapear(agendamentoConsultaMedicoDisponibilidades);
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<AgendamentoConsultaMedicoDisponibilidadeDto>(
                contarAgendamentoConsultaMedicoDisponibilidades,
                agendamentoConsultaMedicoDisponibilidadesDtos
                );
        }

        public async Task<FileDto> ListarParaExcel(ListarAgendamentoConsultaMedicoDisponibilidadesInput input)
        {
            try
            {
                using (var _listarAgendamentoConsultaMedicoDisponibilidadesExcelExporter = IocManager.Instance.ResolveAsDisposable<IListarAgendamentoConsultaMedicoDisponibilidadesExcelExporter>())
                {
                    var result = await Listar(input);
                    var agendamentoConsultaMedicoDisponibilidades = result.Items;
                    return _listarAgendamentoConsultaMedicoDisponibilidadesExcelExporter.Object.ExportToFile(agendamentoConsultaMedicoDisponibilidades.ToList());
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ICollection<AgendamentoConsultaMedicoDisponibilidade>> ListarPorData(DateTime date)
        {
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                {
                    var dayOfWeek = (int)date.DayOfWeek;

                    return await _agendamentoConsultaMedicoDisponibilidadeRepository.Object
                        .GetAll()
                        .Include(m => m.Intervalo)
                        .Include(m => m.Medico)
                        .Include(m => m.Medico.SisPessoa)
                        .Include(m => m.MedicoEspecialidade.Especialidade)
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
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<CriarOuEditarAgendamentoConsultaMedicoDisponibilidade> Obter(long id)
        {
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                {
                    var result = await _agendamentoConsultaMedicoDisponibilidadeRepository.Object
                    .GetAll()
                    .Include(m => m.Intervalo)
                    .Include(m => m.Medico)
                    .Include(m => m.Medico.SisPessoa)
                    .Include(m => m.MedicoEspecialidade)
                    .Include(m => m.MedicoEspecialidade.Especialidade)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                    var agendamentoConsultaMedicoDisponibilidade = CriarOuEditarAgendamentoConsultaMedicoDisponibilidade.Mapear(result);

                    return agendamentoConsultaMedicoDisponibilidade;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<AgendamentoConsultaMedicoDisponibilidadeDto> Obter(long medicoId, long especialidadeId, DateTime horaAgendamento)
        {
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                {
                    var query = await _agendamentoConsultaMedicoDisponibilidadeRepository.Object
                    .GetAll()
                    .Include(m => m.Intervalo)
                    .Include(m => m.Medico)
                    .Include(m => m.Medico.SisPessoa)
                    .Include(m => m.MedicoEspecialidade.Especialidade)
                    .Where(m =>
                        m.MedicoId == medicoId
                        && m.MedicoEspecialidade.EspecialidadeId == especialidadeId
                    )
                    .ToListAsync();

                    var results = new List<AgendamentoConsultaMedicoDisponibilidadeDto>();
                    foreach (var item in query)
                    {

                        var dataIni = new DateTime(01, 01, 01, item.HoraInicio.Hour, item.HoraInicio.Minute, item.HoraInicio.Second);
                        var dataFim = new DateTime(01, 01, 01, item.HoraFim.Hour, item.HoraFim.Minute, item.HoraFim.Second);
                        var agendamento = new DateTime(01, 01, 01, horaAgendamento.Hour, horaAgendamento.Minute, horaAgendamento.Second);
                        if (dataIni <= agendamento && dataFim >= agendamento)
                        {
                            results.Add(new AgendamentoConsultaMedicoDisponibilidadeDto
                            {
                                CreationTime = item.CreationTime,
                                CreatorUserId = item.CreatorUserId,
                                DataFim = item.DataFim,
                                DataInicio = item.DataInicio,
                                DeleterUserId = item.DeleterUserId,
                                DeletionTime = item.DeletionTime,
                                Domingo = item.Domingo,
                                HoraFim = item.HoraFim,
                                HoraInicio = item.HoraInicio,
                                Id = item.Id,
                                IntervaloId = item.IntervaloId,
                                IsDeleted = item.IsDeleted,
                                IsSistema = item.IsSistema,
                                LastModificationTime = item.LastModificationTime,
                                LastModifierUserId = item.LastModifierUserId,
                                MedicoEspecialidadeId = item.MedicoEspecialidadeId,
                                MedicoId = item.MedicoId,
                                Quarta = item.Quarta,
                                Quinta = item.Quinta,
                                Sexta = item.Sexta,
                                Sabado = item.Sabado,
                                Segunda = item.Segunda,
                                Terca = item.Terca
                            });
                        }
                    }

                    return results.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarMedicosDisponiveisDropdown(DropdownInput dropdownInput)
        {
            using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                return await this.CreateSelect2(_agendamentoConsultaMedicoDisponibilidadeRepository.Object)
                       .EnableDistinct()
                    .AddIdField("SisMedico.Id")
                    .AddTextField(" CONCAT(SisMedico.Codigo,' - ', SisMedicoPessoa.NomeCompleto)")
                    .AddFromClause("AteAgendamentoConsultaMedicoDisponibilidade INNER JOIN SisMedico ON AteAgendamentoConsultaMedicoDisponibilidade.SisMedicoId = SisMedico.id INNER JOIN SisPessoa AS SisMedicoPessoa ON SisMedicoPessoa.id = SisMedico.SisPessoaId ")
                    .AddWhereMethod(
                        (input, dapperParameters) =>
                            {
                                var whereBuilder = new StringBuilder();

                                dapperParameters.Add("deleted", false);

                                whereBuilder.Append(
                                    " AND AteAgendamentoConsultaMedicoDisponibilidade.IsDeleted = @deleted AND SisMedico.IsDeleted = @deleted AND SisMedicoPessoa.IsDeleted = @deleted ");

                                whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND (SisMedicoPessoa.NomeCompleto LIKE '%' + @search + '%' OR SisMedico.Codigo LIKE '%' + @search + '%')");

                                return whereBuilder.ToString();
                            })
                    .AddOrderByClause("CONCAT(SisMedico.Codigo,' - ', SisMedicoPessoa.NomeCompleto)")
                    .AddDefaultErrorMessage(L("ErroPesquisar"))
                    .ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarEspecialidadesMedicoDropdown(DropdownInput dropdownInput)
        {
            using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                return await this.CreateSelect2(_agendamentoConsultaMedicoDisponibilidadeRepository.Object)
                       .EnableDistinct()
                       .AddIdField("SisEspecialidade.id")
                       .AddTextField(" CONCAT(SisEspecialidade.Codigo,' - ', SisEspecialidade.Descricao)")
                       .AddFromClause(@"AteAgendamentoConsultaMedicoDisponibilidade 
                            INNER JOIN SisMedicoEspecialidade ON AteAgendamentoConsultaMedicoDisponibilidade.SisMedicoEspecialidadeId = SisMedicoEspecialidade.id 
                            INNER JOIN SisEspecialidade ON SisEspecialidade.id = SisMedicoEspecialidade.SisEspecialidadeId ")
                       .AddWhereMethod(
                           (input, dapperParameters) =>
                               {
                                   var whereBuilder = new StringBuilder();

                                   dapperParameters.Add("deleted", false);

                                   whereBuilder.Append(
                                       " AND AteAgendamentoConsultaMedicoDisponibilidade.IsDeleted = @deleted AND SisMedicoEspecialidade.IsDeleted = @deleted AND SisEspecialidade.IsDeleted = @deleted ");

                                   long medicoId = 0;
                                   var isMedico = long.TryParse(input.filtro, out medicoId);
                                   if (medicoId == 0)
                                   {
                                       throw new Exception(L("InformarMedico"));
                                   }

                                   whereBuilder.Append("AND AteAgendamentoConsultaMedicoDisponibilidade.SisMedicoId = @medicoId");

                                   dapperParameters.Add("medicoId", medicoId);

                                   whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND (SisMedicoEspecialidade.Descricao LIKE '%' + @search + '%' OR SisMedicoEspecialidade.Codigo LIKE '%' + @search + '%')");

                                   return whereBuilder.ToString();
                               })
                       .AddOrderByClause("CONCAT(SisEspecialidade.Codigo,' - ', SisEspecialidade.Descricao)")
                       .AddDefaultErrorMessage(L("ErroPesquisar"))
                       .ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        public async Task<ICollection<AgendamentoConsultaMedicoDisponibilidade>> ListarPorDataMedicoEspecialidade(DateTime date, long medicoId, long especialidadeId)
        {
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                {
                    var dayOfWeek = (int)date.DayOfWeek;

                    return await _agendamentoConsultaMedicoDisponibilidadeRepository.Object
                        .GetAll()
                        .Include(m => m.Intervalo)
                        .Include(m => m.Medico)
                        .Include(m => m.Medico.SisPessoa)
                        .Include(m => m.MedicoEspecialidade.Especialidade)
                        .Where(m =>
                        m.DataInicio <= date
                        && m.DataFim >= date

                        && m.MedicoId == medicoId
                        && m.MedicoEspecialidadeId == especialidadeId

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



        public async Task<GenericoIdNome> ObterSomenteUmaEspecialidade(long medicoId)
        {
            try
            {
                using (var _agendamentoConsultaMedicoDisponibilidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsultaMedicoDisponibilidade, long>>())
                {

                    var especialidades = await _agendamentoConsultaMedicoDisponibilidadeRepository.Object.GetAll()
                                                                   .Include(i => i.MedicoEspecialidade)
                                                                   .Include(i => i.MedicoEspecialidade.Especialidade)
                                                                   .Where(w => w.MedicoId == medicoId)
                                                                   .ToListAsync();


                    if (especialidades != null && especialidades.Count == 1)
                    {
                        return new GenericoIdNome { Id = especialidades[0].MedicoEspecialidade.Id, Nome = especialidades[0].MedicoEspecialidade?.Especialidade?.Descricao };
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
