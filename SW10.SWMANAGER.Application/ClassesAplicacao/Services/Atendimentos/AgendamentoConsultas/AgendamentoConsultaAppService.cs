using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas
{
    using Abp.Auditing;
    using RestSharp;
    using SW10.SWMANAGER.Helpers;

    public class AgendamentoConsultaAppService : SWMANAGERAppServiceBase, IAgendamentoConsultaAppService
    {
        //private readonly IRepository<AgendamentoConsulta, long> _agendamentoConsultaRepository;
        //private readonly IListarAgendamentoConsultasExcelExporter _listarAgendamentoConsultasExcelExporter;
        //private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly IRepository<AgendamentoCirurgico, long> _agendamentoCirurgicoRepository;
        //private readonly IContaAppService _contaAppService;
        //private readonly IFaturamentoContaItemAppService _faturamentoContaItemAppService;
        //private readonly IAtendimentoAppService _atendimentoAppService;

        //public AgendamentoConsultaAppService(IRepository<AgendamentoConsulta, long> agendamentoConsultaRepository
        //    , IListarAgendamentoConsultasExcelExporter listarAgendamentoConsultasExcelExporter,
        //    IUnitOfWorkManager unitOfWorkManager,
        //    IRepository<AgendamentoCirurgico, long> agendamentoCirurgicoRepository,
        //    IContaAppService contaAppService,
        //    IFaturamentoContaItemAppService faturamentoContaItemAppService,
        //    IAtendimentoAppService atendimentoAppService)
        //{
        //    _agendamentoConsultaRepository = agendamentoConsultaRepository;
        //    _listarAgendamentoConsultasExcelExporter = listarAgendamentoConsultasExcelExporter;
        //    _unitOfWorkManager = unitOfWorkManager;
        //    _agendamentoCirurgicoRepository = agendamentoCirurgicoRepository;
        //    _contaAppService = contaAppService;
        //    _faturamentoContaItemAppService = faturamentoContaItemAppService;
        //    _atendimentoAppService = atendimentoAppService;
        //}




        public async Task CriarOuEditar(CriarOuEditarAgendamentoConsulta input)
        {
            try
            {
                using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
                {
                    var agendamentoConsulta = input.MapTo<AgendamentoConsulta>();
                    if (input.Id.Equals(0))
                    {
                        await agendamentoConsultaRepository.Object.InsertAsync(agendamentoConsulta).ConfigureAwait(false);
                    }
                    else
                    {
                        await agendamentoConsultaRepository.Object.UpdateAsync(agendamentoConsulta).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(long id)
        {
            try
            {
                using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
                {
                    await agendamentoConsultaRepository.Object.DeleteAsync(id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<AgendamentoConsultaDto>> ListarTodos()
        {
            try
            {
                using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
                {
                    var query = await agendamentoConsultaRepository.Object
                                    .GetAll()
                                    .Include(m => m.AgendamentoConsultaMedicoDisponibilidade.Intervalo)
                                    .Include(m => m.Convenio)
                                    .Include(i => i.Convenio.SisPessoa)
                                    .Include(m => m.Medico)
                                    .Include(m => m.Medico.SisPessoa)
                                    .Include(m => m.MedicoEspecialidade)
                                    .Include(m => m.Paciente)
                                    .Include(m => m.Paciente.SisPessoa)
                                    .Include(m => m.Plano)
                                    .AsNoTracking()
                                    .ToListAsync().ConfigureAwait(false);

                    var agendamentoConsultasDto = query.Select(AgendamentoConsultaDto.Mapear).ToList();

                    return new ListResultDto<AgendamentoConsultaDto> { Items = agendamentoConsultasDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<AgendamentoConsultaDto>> Listar(ListarAgendamentoConsultasInput input)
        {
            var contarAgendamentoConsultas = 0;
            List<AgendamentoConsulta> agendamentoConsultas;
            var agendamentoConsultasDtos = new List<AgendamentoConsultaDto>();
            try
            {
                using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
                {
                    var query = agendamentoConsultaRepository.Object
                        .GetAll()
                        .Include(m => m.AgendamentoConsultaMedicoDisponibilidade.Intervalo)
                        .Include(m => m.Convenio)
                        .Include(i => i.Convenio.SisPessoa)
                        .Include(m => m.Medico)
                        .Include(m => m.Medico.SisPessoa)
                        .Include(m => m.MedicoEspecialidade)
                        .Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa)
                        .Include(m => m.Plano).AsNoTracking()
                        .WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.Medico.NomeCompleto.Contains(input.Filtro) ||
                            //m.Especialidade.Nome.ToUpper().Contains(input.Filtro.ToUpper()) ||
                            m.Paciente.NomeCompleto.Contains(input.Filtro));

                    contarAgendamentoConsultas = await query
                                                     .CountAsync().ConfigureAwait(false);

                    agendamentoConsultas = await query
                                               .OrderBy(input.Sorting)
                                               .PageBy(input)
                                               .ToListAsync().ConfigureAwait(false);

                    agendamentoConsultasDtos = agendamentoConsultas
                        .MapTo<List<AgendamentoConsultaDto>>();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

            return new PagedResultDto<AgendamentoConsultaDto>(contarAgendamentoConsultas, agendamentoConsultasDtos);
        }

        public async Task<FileDto> ListarParaExcel(ListarAgendamentoConsultasInput input)
        {
            try
            {
                var result = await this.Listar(input).ConfigureAwait(false);
                var agendamentoConsultas = result.Items;
                return new FileDto();//  _listarAgendamentoConsultasExcelExporter.ExportToFile(agendamentoConsultas.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ICollection<AgendamentoConsulta>> ListarPorMedico(long? medicoId, long? medicoEspecialidadeId, DateTime start, DateTime end, EnumTipoAgendamento tipoAgendamento = EnumTipoAgendamento.Consulta)
        {
            try
            {
                using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
                {
                    //var query = await _agendamentoConsultaRepository
                    return await agendamentoConsultaRepository.Object
                               .GetAll()
                               .AsNoTracking()
                               .Include(m => m.AgendamentoConsultaMedicoDisponibilidade)
                               .Include(m => m.AgendamentoConsultaMedicoDisponibilidade.Intervalo)
                               .Include(m => m.Convenio)
                               .Include(i => i.Convenio.SisPessoa)
                               .Include(m => m.Medico)
                               .Include(m => m.Medico.SisPessoa)
                               .Include(m => m.MedicoEspecialidade.Especialidade)
                               .Include(m => m.Paciente)
                               .Include(m => m.Paciente.SisPessoa)
                               .Include(m => m.Plano)
                               .WhereIf(medicoId.HasValue && medicoId >= 0, m => m.MedicoId == medicoId.Value)
                               .WhereIf(medicoEspecialidadeId.HasValue && medicoEspecialidadeId >= 0, m => m.MedicoEspecialidade.EspecialidadeId == medicoEspecialidadeId.Value)
                               .Where(m => m.DataAgendamento >= start
                                           && m.DataAgendamento <= end
                                           && (m.IsConsulta && tipoAgendamento == EnumTipoAgendamento.Consulta)
                               // && (m.IsCirurgia && tipoAgendamento == EnumTipoAgendamento.Cirurgia)
                               ) // start <= m.DataAgendamento && end >= m.DataAgendamento)
                               .ToListAsync().ConfigureAwait(false);
                    //return query.MapTo<List<AgendamentoConsultaDto>>();
                    //var agendamentoConsultas = await query
                    ////.AsNoTracking()
                    //.ToListAsync();
                    //var agendamentoConsultasDtos = agendamentoConsultas.MapTo<List<AgendamentoConsultaDto>>();
                    //return agendamentoConsultasDtos;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ICollection<AgendamentoConsulta>> ListarPorData(DateTime start, DateTime end)
        {
            using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
            {
                return await agendamentoConsultaRepository.Object
                           .GetAll()
                           .AsNoTracking()
                           .Include(m => m.AgendamentoConsultaMedicoDisponibilidade.Intervalo)
                           .Include(m => m.Convenio)
                           .Include(i => i.Convenio.SisPessoa)
                           .Include(m => m.Medico)
                           .Include(m => m.Medico.SisPessoa)
                           .Include(m => m.MedicoEspecialidade.Especialidade)
                           .Include(m => m.Paciente)
                           .Include(m => m.Paciente.SisPessoa)
                           .Include(m => m.Plano)
                           .Where(m => m.DataAgendamento >= start && m.DataAgendamento <= end)
                           .ToListAsync().ConfigureAwait(false);
            }
        }

        public async Task<ICollection<AgendamentoConsulta>> ListarPorDataMedicoEspecialidade(DateTime start, DateTime end, long medicoId, long medicoEspecialidadeId)
        {
            using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
            {
                return await agendamentoConsultaRepository.Object
                           .GetAll()
                           .AsNoTracking()
                           .Include(m => m.AgendamentoConsultaMedicoDisponibilidade.Intervalo)
                           .Include(m => m.Convenio)
                           .Include(i => i.Convenio.SisPessoa)
                           .Include(m => m.Medico)
                           .Include(m => m.Medico.SisPessoa)
                           .Include(m => m.MedicoEspecialidade.Especialidade)
                           .Include(m => m.Paciente)
                           .Include(m => m.Paciente.SisPessoa)
                           .Include(m => m.Plano)
                           .Where(m => m.DataAgendamento >= start
                                       && m.DataAgendamento <= end
                                       && m.MedicoId == medicoId
                                       && m.MedicoEspecialidadeId == medicoEspecialidadeId)
                           .ToListAsync().ConfigureAwait(false);
            }
        }

        public async Task<CriarOuEditarAgendamentoConsulta> Obter(long id)
        {
            try
            {
                using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
                {
                    var result = await agendamentoConsultaRepository.Object
                                     .GetAll()
                                     .AsNoTracking()
                                     .Include(m => m.AgendamentoConsultaMedicoDisponibilidade.Intervalo)
                                     .Include(m => m.Convenio)
                                     .Include(i => i.Convenio.SisPessoa)
                                     .Include(m => m.Medico)
                                     .Include(m => m.Medico.SisPessoa)
                                     .Include(m => m.MedicoEspecialidade.Especialidade)
                                     .Include(m => m.Paciente)
                                     .Include(m => m.Paciente.SisPessoa)
                                     .Include(m => m.Plano)
                                     .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

                    var agendamentoConsulta = result
                        .MapTo<CriarOuEditarAgendamentoConsulta>();

                    return agendamentoConsulta;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<bool> ChecarDisponibilidade(long medicoDisponibilidadeId, DateTime hora, long id = 0)
        {
            using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
            {
                var query = await agendamentoConsultaRepository.Object
                                .GetAll()
                                .AsNoTracking()
                                //.Include(m => m.AgendamentoConsultaMedicoDisponibilidade.Intervalo)
                                //.Include(m => m.Convenio)
                                //.Include(i => i.Convenio.SisPessoa)
                                //.Include(m => m.Medico)
                                //.Include(m => m.Medico.SisPessoa)
                                //.Include(m => m.MedicoEspecialidade.Especialidade)
                                //.Include(m => m.Paciente)
                                //.Include(m => m.Paciente.SisPessoa)
                                //.Include(m => m.Plano)
                                .Where(m => m.AgendamentoConsultaMedicoDisponibilidadeId == medicoDisponibilidadeId && m.HoraAgendamento == hora)
                                .ToListAsync().ConfigureAwait(false);
                if (query.Any())
                {
                    //no caso de edição, se o horário for o próprio do registro, ele deve ser incluído.
                    if (id > 0)
                    {
                        var agendamento = query.FirstOrDefault().Id == id;
                        if (agendamento)
                        {
                            return true;
                        }
                    }
                    return false; //Não está disponível
                }
                else
                {
                    return true; //Está disponível
                }
            }
        }

        public async Task<DefaultReturn<string>> AlterarAgendamento(long id, int dias, int horas, int minutos)
        {
            using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var agendamento = await agendamentoConsultaRepository.Object
                                                            .GetAll()
                                                            .FirstOrDefaultAsync(w => w.Id == id).ConfigureAwait(false);

                if (agendamento != null)
                {
                    agendamento.HoraAgendamento = agendamento.HoraAgendamento.AddDays(dias).AddHours(horas).AddMinutes(minutos);

                    agendamento.DataAgendamento = agendamento.HoraAgendamento.Date;
                }

                unitOfWork.Complete();
                unitOfWorkManager.Object.Current.SaveChanges();
                unitOfWork.Dispose();
            }

            return null;
        }


        public async Task<DefaultReturn<string>> ConfirmarAtendimento(long id)
        {
            DefaultReturn<string> retorno = null;

            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var agendamento = agendamentoCirurgicoRepository.Object.GetAll()
                                                                 .Include(i => i.AgendamentoConsulta)
                                                                 .FirstOrDefault(w => w.AgendamentoConsultaId == id);
                if (agendamento != null)
                {
                    var atendimentoId = await atendimentoAppService.Object.ObterAtendindimentoAbertoPaciente((long)agendamento.AgendamentoConsulta.PacienteId).ConfigureAwait(false);

                    if (atendimentoId == null)
                    {
                        //Construir tela para inserir dados do atendimento

                        //Confirmar com o Márcio
                    }
                    else
                    {
                        var retornoAtendimento = await this.InserirItensFaturamentoAtendimentoExistente(agendamento.AgendamentoConsulta.Id, (long)atendimentoId).ConfigureAwait(false);

                        retorno = new DefaultReturn<string>
                        {
                            ReturnObject = retornoAtendimento.ReturnObject.Id
                                              .ToString()
                        };
                    }
                }

                return retorno;
            }
        }

        public async Task<DefaultReturn<FaturamentoContaItemInsertDto>> InserirItensFaturamentoAtendimentoExistente(long agendamentoId, long atendimentoId)
        {
            var retorno = new DefaultReturn<FaturamentoContaItemInsertDto>();
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var agendamentoConsultaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoConsulta, long>>())
                using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
                using (var contaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
                using (var faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var agendamento = agendamentoConsultaRepository.Object
                                                                .GetAll()
                                                                .FirstOrDefault(w => w.Id == agendamentoId);

                    if (agendamento != null && agendamento.IsCirurgia)
                    {

                        var agendamentoCirurgico = agendamentoCirurgicoRepository.Object
                                                                                  .GetAll()
                                                                                  .Include(i => i.AgendamentoConsulta)
                                                                                  .Include(i => i.AgendamentoSalaCirurgicaDisponibilidade)
                                                                                  .Include(i => i.Cirurgias)
                                                                                  .FirstOrDefault(w => w.AgendamentoConsultaId == agendamentoId);



                        var contaId = await contaAppService.Object.ObterUltimaContaAtendimentoId(atendimentoId).ConfigureAwait(false);

                        if (contaId != 0)
                        {
                            var itensFaturamento = new List<FaturamentoContaItemDto>();

                            foreach (var item in agendamentoCirurgico.Cirurgias)
                            {
                                var itemfaturamentoDto =
                                    new FaturamentoContaItemDto
                                    {
                                        FaturamentoContaId = contaId,
                                        Id = item.FaturamentoItemId ?? 0,
                                        Qtde = 1
                                    };


                                itensFaturamento.Add(itemfaturamentoDto);
                            }


                            var faturamentoContaItemInsertDto =
                                new FaturamentoContaItemInsertDto
                                {
                                    AtendimentoId = atendimentoId,
                                    ContaId = contaId,
                                    Data = agendamento.DataAgendamento,
                                    ItensFaturamento = itensFaturamento
                                };





                            retorno = await faturamentoContaItemAppService.Object.InserirItensContaFaturamento(faturamentoContaItemInsertDto).ConfigureAwait(false);
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

            }
            catch (Exception)
            {

            }

            return retorno;
        }

        public async Task<ICollection<AgendamentoConsulta>> ListarPorDataSalaTipoCirurgia(DateTime start, DateTime end, long salaId, long tipoCirurgiaId)
        {
            using (var agendamentoCirurgicoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AgendamentoCirurgico, long>>())
            {
                return await agendamentoCirurgicoRepository.Object
                           .GetAll()
                           .AsNoTracking()
                           // .Include(m => m.Intervalo)
                           //.Include(m => m.Convenio)
                           //.Include(i => i.Convenio.SisPessoa)
                           //.Include(m => m.Medico)
                           //.Include(m => m.Medico.SisPessoa)
                           //.Include(m => m.MedicoEspecialidade.Especialidade)
                           //.Include(m => m.Paciente)
                           //.Include(m => m.Paciente.SisPessoa)
                           //.Include(m => m.Plano)
                           .Where(m => m.AgendamentoConsulta.DataAgendamento >= start
                                       && m.AgendamentoConsulta.DataAgendamento <= end
                                       && m.AgendamentoSalaCirurgicaDisponibilidade.SalaCirurgicaId == salaId
                                       && m.AgendamentoSalaCirurgicaDisponibilidade.TipoCirurgiaId == tipoCirurgiaId)
                           .Select(s => s.AgendamentoConsulta)
                           .ToListAsync().ConfigureAwait(false);
            }
        }


        public byte[] RetornaArquivoAgendamentoCirurgico(DateTime dataInicial, DateTime dataFinal)
        {
            return this.CreateJasperReport("AgendamentoCirurgico")
                .SetMethod(Method.POST)
                .AddParameter("DATAINICIAL", dataInicial.ToString("yyyy-MM-dd"))
                .AddParameter("DATAFINAL", dataFinal.ToString("yyyy-MM-dd"))
                .AddParameter("UsuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName())
                .GenerateReport();
        }
    }
}
