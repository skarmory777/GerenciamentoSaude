using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Globais.HorasDia;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Auditing;
    using Abp.Dependency;
    using Abp.Runtime.Session;
    using Abp.Threading;
    using Castle.Core.Internal;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
    using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.Frequencias.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Globais.HorasDias.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
    using SW10.SWMANAGER.Helpers;

    public class PrescricaoItemRespostaAppService : SWMANAGERAppServiceBase, IPrescricaoItemRespostaAppService
    {
        [UnitOfWork(IsDisabled = true)]
        public async Task<PrescricaoItemRespostaDto> CriarOuEditar(PrescricaoItemRespostaDto input,  bool atualizaOuCriaArquivo = true)
        {
            try
            {
                using (var prescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var prescricaoItemHoraRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemHora, long>>())
                using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                using (var frequenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                using (var horaDiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<HoraDia, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    if (!input.DivisaoId.HasValue || (input.DivisaoId.HasValue && input.DivisaoId.Value == 0))
                    {
                        var prescricaoItem = await prescricaoItemAppService.Object.Obter(input.PrescricaoItemId.Value).ConfigureAwait(false);
                        input.DivisaoId = prescricaoItem.DivisaoId;
                    }

                    var tipoResposta = new PrescricaoItemResposta
                    {
                        Codigo = input.Codigo,
                        CreationTime = input.CreationTime,
                        CreatorUserId = input.CreatorUserId,
                        DeleterUserId = input.DeleterUserId,
                        DeletionTime = input.DeletionTime,
                        Descricao = input.Descricao,
                        DivisaoId = input.DivisaoId,
                        FormaAplicacaoId = input.FormaAplicacaoId,
                        FrequenciaId = input.FrequenciaId,
                        Id = input.Id,
                        IsDeleted = input.IsDeleted,
                        IsSeNecessario = input.IsSeNecessario,
                        IsSistema = input.IsSistema,
                        IsUrgente = input.IsUrgente,
                        LastModificationTime = input.LastModificationTime,
                        LastModifierUserId = input.LastModifierUserId,
                        MedicoId = input.MedicoId,
                        Observacao = input.Observacao,
                        PrescricaoItemId = input.PrescricaoItemId,
                        PrescricaoItemStatusId = input.PrescricaoItemStatusId,
                        PrescricaoMedicaId = input.PrescricaoMedicaId,
                        Quantidade = input.Quantidade,
                        TotalDias = input.TotalDias,
                        UnidadeId = input.UnidadeId,
                        UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                        VelocidadeInfusaoId = input.VelocidadeInfusaoId,
                        DiluenteId = input.DiluenteId,
                        VolumeDiluente = input.VolumeDiluente,
                        DoseUnica = input.DoseUnica,
                        DataAgrupamento =  input.DataAgrupamento,
                        ObsFrequencia =  input.ObsFrequencia
                    };

                    if (!string.IsNullOrEmpty(input.JustificativaBloqueioDosagemAceitavel))
                    {
                        tipoResposta.JustificativaBloqueioDosagemAceitavel = input.JustificativaBloqueioDosagemAceitavel;
                        tipoResposta.JustificativaBloqueioId = this.GetCurrentUser().Id;
                    }

                    if (input.DataInicial.HasValue)
                    {
                        tipoResposta.DataInicial = input.DataInicial.Value;
                    }

                    if (input.Id.Equals(0))
                    {
                        var prescricaoMedica = await prescricaoMedicaRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == input.PrescricaoMedicaId).ConfigureAwait(false);

                        if (input.IsAcrescimo && prescricaoMedica?.PrescricaoStatusId != PrescricaoStatus.Inicial)
                        {
                            tipoResposta.IsAcrescimo = input.IsAcrescimo;
                            tipoResposta.DataAcrescimo = DateTime.Now;
                            tipoResposta.AcrescimoUserId = AbpSession.UserId;
                        }

                        input.Id = await prescricaoItemRespostaRepository.Object.InsertAndGetIdAsync(tipoResposta).ConfigureAwait(false);
                    }
                    else
                    {
                        var updated = await prescricaoItemRespostaRepository.Object.GetAsync(input.Id).ConfigureAwait(false);
                        updated.Codigo = input.Codigo;
                        updated.CreationTime = input.CreationTime;
                        updated.CreatorUserId = input.CreatorUserId;
                        updated.DataInicial = input.DataInicial.HasValue ? input.DataInicial.Value : new DateTime?();
                        updated.Descricao = input.Descricao;
                        updated.DivisaoId = input.DivisaoId;
                        updated.FormaAplicacaoId = input.FormaAplicacaoId;
                        updated.FrequenciaId = input.FrequenciaId;
                        updated.IsSeNecessario = input.IsSeNecessario;
                        updated.IsSistema = input.IsSistema;
                        updated.IsUrgente = input.IsUrgente;
                        updated.MedicoId = input.MedicoId;
                        updated.Observacao = input.Observacao;
                        updated.PrescricaoItemId = input.PrescricaoItemId;
                        updated.PrescricaoMedicaId = input.PrescricaoMedicaId;
                        updated.Quantidade = input.Quantidade;
                        updated.TotalDias = input.TotalDias;
                        updated.UnidadeId = input.UnidadeId;
                        updated.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                        updated.VelocidadeInfusaoId = input.VelocidadeInfusaoId;
                        updated.DiluenteId = input.DiluenteId;
                        updated.VolumeDiluente = input.VolumeDiluente;
                        updated.DoseUnica = input.DoseUnica;
                        updated.ObsFrequencia = input.ObsFrequencia;

                        if (!string.IsNullOrEmpty(input.JustificativaBloqueioDosagemAceitavel))
                        {
                            updated.JustificativaBloqueioDosagemAceitavel = input.JustificativaBloqueioDosagemAceitavel;
                            updated.JustificativaBloqueioId = this.GetCurrentUser().Id;
                        }

                        await prescricaoItemRespostaRepository.Object.UpdateAsync(updated).ConfigureAwait(false);
                    }

                    var prescricaoItemRespostaDepsResolver = new PrescricaoItemRespostaDepsResolver(
                        prescricaoItemRepository,
                        prescricaoMedicaRepository,
                        prescricaoItemAppService,                                                                                                                                                                                                                                                                   
                        prescricaoItemRespostaRepository,
                        prescricaoItemHoraRepository,
                        unidadeOrganizacionalAppService,
                        frequenciaRepository,
                        horaDiaRepository,
                        unitOfWorkManager
                    );
                    
                    input.HorariosPrescricaoItens = await SalvaHorariosPrescricaoItem(input, prescricaoItemRespostaDepsResolver);

                    if (input.DiluenteId != null && input.DiluenteId != 0)
                    {
                        var diluente = await prescricaoItemRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(w => w.Id == input.DiluenteId);

                        if (diluente != null)
                        {
                            input.Diluente = new PrescricaoItemDto { Id = diluente.Id, Descricao = diluente.Descricao };
                        }
                    }
                    
                    var prescricao = await prescricaoMedicaRepository.Object.FirstOrDefaultAsync(input.PrescricaoMedicaId.Value).ConfigureAwait(false);
                    if ( await prescricaoItemRespostaRepository.Object.GetAll().AsNoTracking().AnyAsync(x => x.IsAcrescimo && x.PrescricaoMedicaId == input.PrescricaoMedicaId).ConfigureAwait(false))
                    {
                        if (prescricao.PrescricaoStatusId == PrescricaoStatus.Liberada || prescricao.PrescricaoStatusId == PrescricaoStatus.Aprovada)
                        {
                            if (prescricao.PrescricaoStatusId == PrescricaoStatus.Liberada)
                            {
                                prescricao.PrescricaoStatusId = PrescricaoStatus.LiberadaComAcrescimo;
                            }

                            if (prescricao.PrescricaoStatusId == PrescricaoStatus.Aprovada)
                            {
                                prescricao.PrescricaoStatusId = PrescricaoStatus.AprovadaComAcrescimo;
                            }

                            await prescricaoMedicaRepository.Object.UpdateAsync(prescricao);
                        }
                    }
                    else
                    {
                        if (prescricao.PrescricaoStatusId == PrescricaoStatus.LiberadaComAcrescimo || prescricao.PrescricaoStatusId == PrescricaoStatus.AprovadaComAcrescimo)
                        {
                            if (prescricao.PrescricaoStatusId == PrescricaoStatus.LiberadaComAcrescimo)
                            {
                                prescricao.PrescricaoStatusId = PrescricaoStatus.Liberada;
                            }

                            if (prescricao.PrescricaoStatusId == PrescricaoStatus.AprovadaComAcrescimo)
                            {
                                prescricao.PrescricaoStatusId = PrescricaoStatus.Aprovada;
                            }

                            await prescricaoMedicaRepository.Object.UpdateAsync(prescricao);
                        }
                    }
                    
                    unitOfWork.Complete();
                    //unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
                
                if (atualizaOuCriaArquivo)
                {
                    using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                    using (var prescricaoMedicaAppService =
                        IocManager.Instance.ResolveAsDisposable<IPrescricaoMedicaAppService>())
                    {
                        await prescricaoMedicaAppService.Object
                            .AtualizaArquivoPrescricaoMedica(input.PrescricaoMedicaId.Value).ConfigureAwait(false);
                    }
                }

                return input;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message + " - " + ex.InnerException?.Message, ex);
            }
        }


        public class PrescricaoItemRespostaDepsResolver
        {
            public IDisposableDependencyObjectWrapper<IRepository<PrescricaoItem, long>> PrescricaoItemRepository { get; set; }
            public IDisposableDependencyObjectWrapper<IRepository<PrescricaoMedica, long>> PrescricaoMedicaRepository { get; set; }
            public IDisposableDependencyObjectWrapper<IPrescricaoItemAppService> PrescricaoItemAppService { get; set; }
            public IDisposableDependencyObjectWrapper<IRepository<PrescricaoItemResposta, long>> PrescricaoItemRespostaRepository { get; set; }
            public IDisposableDependencyObjectWrapper<IRepository<PrescricaoItemHora, long>> PrescricaoItemHoraRepository { get; set; }
            public IDisposableDependencyObjectWrapper<IUnidadeOrganizacionalAppService> UnidadeOrganizacionalAppService { get; set; }
            public IDisposableDependencyObjectWrapper<IRepository<Frequencia, long>> FrequenciaRepository { get; set; }
            public IDisposableDependencyObjectWrapper<IRepository<HoraDia, long>> HoraDiaRepository { get; set; }
            public IDisposableDependencyObjectWrapper<IUnitOfWorkManager> UnitOfWorkManager { get; set; }

            public IEnumerable<FrequenciaDto> Frequencias { get; set; }

            public IEnumerable<UnidadeOrganizacionalDto> UnidadeOrganizacionais { get; set; }

            public IEnumerable<HoraDiaDto> Horas { get; set; }

            public PrescricaoItemRespostaDepsResolver(
                IDisposableDependencyObjectWrapper<IRepository<PrescricaoItem, long>> prescricaoItemRepository,
                IDisposableDependencyObjectWrapper<IRepository<PrescricaoMedica, long>> prescricaoMedicaRepository,
                IDisposableDependencyObjectWrapper<IPrescricaoItemAppService> prescricaoItemAppService,
                IDisposableDependencyObjectWrapper<IRepository<PrescricaoItemResposta, long>> prescricaoItemRespostaRepository,
                IDisposableDependencyObjectWrapper<IRepository<PrescricaoItemHora, long>> prescricaoItemHoraRepository,
                IDisposableDependencyObjectWrapper<IUnidadeOrganizacionalAppService> unidadeOrganizacionalAppService,
                IDisposableDependencyObjectWrapper<IRepository<Frequencia, long>> frequenciaRepository,
                IDisposableDependencyObjectWrapper<IRepository<HoraDia, long>> horaDiaRepository,
                IDisposableDependencyObjectWrapper<IUnitOfWorkManager> unitOfWorkManager)
            {
                PrescricaoItemRepository = prescricaoItemRepository;
                PrescricaoMedicaRepository = prescricaoMedicaRepository;
                PrescricaoItemAppService = prescricaoItemAppService;
                PrescricaoItemRespostaRepository = prescricaoItemRespostaRepository;
                PrescricaoItemHoraRepository = prescricaoItemHoraRepository;
                UnidadeOrganizacionalAppService = unidadeOrganizacionalAppService;
                FrequenciaRepository = frequenciaRepository;
                HoraDiaRepository = horaDiaRepository;
                UnitOfWorkManager = unitOfWorkManager;

                Frequencias = FrequenciaDto.Mapear(frequenciaRepository.Object.GetAll().AsNoTracking().ToList());
                Horas = HoraDiaDto.Mapear(HoraDiaRepository.Object.GetAll().AsNoTracking().ToList());
                UnidadeOrganizacionais = AsyncHelper.RunSync(() => unidadeOrganizacionalAppService.Object.ListarTodos()).Items;
            }
        }

        [UnitOfWork(IsDisabled = false)]
        public static async Task<PrescricaoItemRespostaDto> CriarOuEditarAsync(PrescricaoItemRespostaDto input, PrescricaoMedicaDto prescricaoMedica, PrescricaoItemRespostaDepsResolver resolver, IAbpSession abpSession)
        {
            try
            {

                if (!input.DivisaoId.HasValue || (input.DivisaoId.HasValue && input.DivisaoId.Value == 0))
                {
                    var prescricaoItem = await resolver.PrescricaoItemRepository.Object.GetAll()
                        .Select(x => new { Id = x.Id, DivisaoId = x.DivisaoId })
                        .FirstOrDefaultAsync(x => x.Id == input.PrescricaoItemId.Value).ConfigureAwait(false);
                    input.DivisaoId = prescricaoItem.DivisaoId;
                }

                var tipoResposta = new PrescricaoItemResposta
                {
                    Codigo = input.Codigo,
                    CreationTime = input.CreationTime,
                    CreatorUserId = input.CreatorUserId,
                    DeleterUserId = input.DeleterUserId,
                    DeletionTime = input.DeletionTime,
                    Descricao = input.Descricao,
                    DivisaoId = input.DivisaoId,
                    FormaAplicacaoId = input.FormaAplicacaoId,
                    FrequenciaId = input.FrequenciaId,
                    Id = input.Id,
                    IsDeleted = input.IsDeleted,
                    IsSeNecessario = input.IsSeNecessario,
                    IsSistema = input.IsSistema,
                    IsUrgente = input.IsUrgente,
                    LastModificationTime = input.LastModificationTime,
                    LastModifierUserId = input.LastModifierUserId,
                    MedicoId = input.MedicoId,
                    Observacao = input.Observacao,
                    PrescricaoItemId = input.PrescricaoItemId,
                    PrescricaoItemStatusId = input.PrescricaoItemStatusId,
                    PrescricaoMedicaId = input.PrescricaoMedicaId,
                    Quantidade = input.Quantidade,
                    TotalDias = input.TotalDias,
                    UnidadeId = input.UnidadeId,
                    UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                    VelocidadeInfusaoId = input.VelocidadeInfusaoId,
                    DiluenteId = input.DiluenteId,
                    VolumeDiluente = input.VolumeDiluente,
                    DoseUnica = input.DoseUnica,
                    DataAgrupamento = input.DataAgrupamento,
                    ObsFrequencia = input.ObsFrequencia
                };

                if (input.DataInicial.HasValue)
                {
                    tipoResposta.DataInicial = input.DataInicial.Value;
                }

                if (input.Id.Equals(0))
                {

                    if (input.IsAcrescimo && prescricaoMedica?.PrescricaoStatusId != PrescricaoStatus.Inicial)
                    {
                        tipoResposta.IsAcrescimo = input.IsAcrescimo;
                        tipoResposta.DataAcrescimo = DateTime.Now;
                        tipoResposta.AcrescimoUserId = abpSession.UserId;
                    }

                    input.Id = await resolver.PrescricaoItemRespostaRepository.Object.InsertAndGetIdAsync(tipoResposta).ConfigureAwait(false);
                }
                else
                {
                    var updated = await resolver.PrescricaoItemRespostaRepository.Object.GetAsync(input.Id).ConfigureAwait(false);
                    updated.Codigo = input.Codigo;
                    updated.CreationTime = input.CreationTime;
                    updated.CreatorUserId = input.CreatorUserId;
                    updated.DataInicial = input.DataInicial.HasValue ? input.DataInicial.Value : new DateTime?();
                    updated.Descricao = input.Descricao;
                    updated.DivisaoId = input.DivisaoId;
                    updated.FormaAplicacaoId = input.FormaAplicacaoId;
                    updated.FrequenciaId = input.FrequenciaId;
                    updated.IsSeNecessario = input.IsSeNecessario;
                    updated.IsSistema = input.IsSistema;
                    updated.IsUrgente = input.IsUrgente;
                    updated.MedicoId = input.MedicoId;
                    updated.Observacao = input.Observacao;
                    updated.PrescricaoItemId = input.PrescricaoItemId;
                    updated.PrescricaoMedicaId = input.PrescricaoMedicaId;
                    updated.Quantidade = input.Quantidade;
                    updated.TotalDias = input.TotalDias;
                    updated.UnidadeId = input.UnidadeId;
                    updated.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                    updated.VelocidadeInfusaoId = input.VelocidadeInfusaoId;
                    updated.DiluenteId = input.DiluenteId;
                    updated.VolumeDiluente = input.VolumeDiluente;
                    updated.DoseUnica = input.DoseUnica;
                    updated.ObsFrequencia = input.ObsFrequencia;
                    
                    tipoResposta = await resolver.PrescricaoItemRespostaRepository.Object.UpdateAsync(updated).ConfigureAwait(false);
                }

                input.HorariosPrescricaoItens = await SalvaHorariosPrescricaoItem(input, resolver, false).ConfigureAwait(false);

                if (input.DiluenteId != null && input.DiluenteId != 0)
                {
                    var diluente = await resolver.PrescricaoItemRepository.Object.GetAll().AsNoTracking().Select(x => new { x.Id, x.Descricao }).FirstOrDefaultAsync(w => w.Id == input.DiluenteId).ConfigureAwait(false);

                    if (diluente != null)
                    {
                        input.Diluente = new PrescricaoItemDto { Id = diluente.Id, Descricao = diluente.Descricao };
                    }
                }

                return input;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message + " - " + ex.InnerException?.Message);
            }
        }

        [UnitOfWork(IsDisabled = false)]
        public static async Task<List<PrescricaoItemHoraDto>> SalvaHorariosPrescricaoItem(PrescricaoItemRespostaDto input, PrescricaoItemRespostaDepsResolver resolver, bool deleteAll = true)
        {
            var result = new List<PrescricaoItemHoraDto>();
            //salvando os horários de medicação
            if (!input.FrequenciaId.HasValue) return result;

            var horasDias = new List<string>();

            //removendo horários anteriores deste item de prescrição
            if (deleteAll)
            {
                await resolver.PrescricaoItemHoraRepository.Object.DeleteAsync(m => m.PrescricaoItemRespostaId == input.Id).ConfigureAwait(false);
            }
            var unidadeOrganizacional = !input.UnidadeOrganizacionalId.HasValue ? new UnidadeOrganizacionalDto() : resolver.UnidadeOrganizacionais.FirstOrDefault(x => x.Id == input.UnidadeOrganizacionalId.Value);
            var frequencia = resolver.Frequencias.FirstOrDefault(x => x.Id == input.FrequenciaId.Value);
            if (frequencia == null || unidadeOrganizacional == null || frequencia.Intervalo == 0)
            {
                return result;
            }
            var dia = 0;
            //se for o primeiro dia, ajustar os horários de acordo com o início da prescrição
            var aHoraIni = !string.IsNullOrEmpty(frequencia.HoraInicialMedicacao) ? frequencia.HoraInicialMedicacao : unidadeOrganizacional.HoraInicioPrescricao;
            var horaIni = TimeSpan.FromHours(0);
            if (!aHoraIni.IsNullOrEmpty())
            {
                horaIni = TimeSpan.Parse(aHoraIni);
            }

            if (input.Horarios.IsNullOrEmpty())
            {
                var intervalo = 24 / frequencia.Intervalo;
                var tempHoras = "";
                for (var index = 1; index <= intervalo; index++)
                {
                    if (index == 1)
                    {
                        tempHoras += horaIni.ToString(@"hh\:mm") + " ";
                    }
                    else
                    {
                        tempHoras += horaIni.Add(TimeSpan.FromHours(frequencia.Intervalo * (index - 1))).ToString(@"hh\:mm") + " ";
                    }
                }

                tempHoras = tempHoras.Substring(0, tempHoras.Length - 1);

                input.Horarios = tempHoras;
            }

            var horas = input.Horarios?.Split(' ');
            if (horas != null)
            {
                horasDias.AddRange(horas);
            }

            var count = 0;
            var dataMedicamento = input.DataInicial ?? DateTime.Today;
            TimeSpan horaAtendimento;
            TimeSpan.TryParse(unidadeOrganizacional.HoraInicioPrescricao, out horaAtendimento);

            foreach (var _hora in horasDias)
            {
                var horaLoop = TimeSpan.Parse(_hora);

                horaIni = TimeSpan.FromTicks(horaLoop.Ticks);

                var horaDia = resolver.Horas.FirstOrDefault(m => m.Descricao.Equals(horaLoop.ToString("hh")));

                if (horaDia == null)
                {
                    continue;
                }

                var hora = new PrescricaoItemHora
                {
                    Descricao = horaIni.ToString("hh"),
                    DataMedicamento = new DateTime(dataMedicamento.Year, dataMedicamento.Month, dataMedicamento.Day, horaIni.Hours, 0, 0),
                    DiaMedicamento = dia,
                    Hora = horaIni.ToString(),
                    IsDeleted = false,
                    IsSistema = false,
                    PrescricaoItemRespostaId = input.Id,
                    HoraDiaId = horaDia.Id
                };
                //await _prescricaoItemHoraAppService.CriarOuEditar(hora);
                hora.Id = await resolver.PrescricaoItemHoraRepository.Object.InsertAndGetIdAsync(hora).ConfigureAwait(false);
                horaIni += horaIni.Add(TimeSpan.FromHours(frequencia.Intervalo));
                if (horaIni.Hours >= 24)
                {
                    dataMedicamento = dataMedicamento.AddDays(1);
                }

                result.Add(PrescricaoItemHoraDto.Mapear(hora));
            }

            return result;
        }

        [UnitOfWork]
        public async Task Excluir(PrescricaoItemRespostaDto input)
        {
            try
            {
                using (var prescricaoItemRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await prescricaoItemRespostaRepositorio.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public void ExcluirSync(PrescricaoItemRespostaDto input)
        {
            try
            {
                using (var prescricaoItemRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    prescricaoItemRespostaRepositorio.Object.Delete(input.Id);
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<PrescricaoItemRespostaDto> Obter(long id)
        {
            try
            {
                using (var prescricaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItem, long>>())
                using (var prescricaoItemAppService = IocManager.Instance.ResolveAsDisposable<IPrescricaoItemAppService>())
                using (var prescricaoItemRespostaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var prescricaoMedicaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoMedica, long>>())
                using (var prescricaoItemHoraRepository = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemHora, long>>())
                using (var unidadeOrganizacionalAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeOrganizacionalAppService>())
                using (var frequenciaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Frequencia, long>>())
                using (var horaDiaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<HoraDia, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {

                    var prescricaoItemRespostaDepsResolver = new PrescricaoItemRespostaDepsResolver(
                       prescricaoItemRepository,
                       prescricaoMedicaRepository,
                       prescricaoItemAppService,
                       prescricaoItemRespostaRepository,
                       prescricaoItemHoraRepository,
                       unidadeOrganizacionalAppService,
                       frequenciaRepository,
                       horaDiaRepository,
                       unitOfWorkManager
                   );

                    var result = await prescricaoItemRespostaRepository.Object
                                 .GetAll()
                                 .AsNoTracking()
                                 .Include(m => m.Divisao)
                                 .Include(m => m.Diluente)
                                 .Include(m => m.FormaAplicacao)
                                 .Include(m => m.Frequencia)
                                 .Include(m => m.Medico)
                                 .Include(m => m.Medico.SisPessoa)
                                 .Include(m => m.PrescricaoItem)
                                 .Include(m => m.PrescricaoItem.ConfiguracaoPrescricaoItems)
                                 .Include(m => m.Unidade)
                                 .Include(m => m.UnidadeOrganizacional)
                                 .Include(m => m.VelocidadeInfusao)
                                 .Include(m => m.PrescricaoMedica)
                                 .OrderBy(m => m.Codigo)
                                 .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

                    var prescricaoItemRespostaDto = PrescricaoItemRespostaDto.Mapear(result);

                    //buscando os horários atuais
                    var prescricaoItemHoras = await prescricaoItemHoraRepository.Object.GetAllListAsync(m => m.PrescricaoItemRespostaId == result.Id).ConfigureAwait(false);
                    if (prescricaoItemHoras.IsNullOrEmpty())
                    {
                        prescricaoItemRespostaDto.HorariosPrescricaoItens = await SalvaHorariosPrescricaoItem(prescricaoItemRespostaDto, prescricaoItemRespostaDepsResolver).ConfigureAwait(false);
                    }
                    else
                    {
                        prescricaoItemRespostaDto.HorariosPrescricaoItens = PrescricaoItemHoraDto.Mapear(prescricaoItemHoras).ToList();
                    }
                    if (prescricaoItemRespostaDto.Horarios.IsNullOrEmpty() && prescricaoItemRespostaDto.Frequencia?.Intervalo != 0)
                    {
                        var take = 24 / (prescricaoItemRespostaDto.Frequencia?.Intervalo ?? 24);
                        foreach (var item in prescricaoItemRespostaDto.HorariosPrescricaoItens.OrderBy(o => o.DataMedicamento).ThenBy(o => o.DiaMedicamento).Take((int)take))
                        {
                            prescricaoItemRespostaDto.Horarios += (string.Format("{0:00}:00 ", item.Hora));
                        }
                        if (!string.IsNullOrWhiteSpace(prescricaoItemRespostaDto.Horarios))
                        {
                            prescricaoItemRespostaDto.Horarios = prescricaoItemRespostaDto.Horarios.Substring(0, prescricaoItemRespostaDto.Horarios.Length - 1);
                        }
                    }
                    return prescricaoItemRespostaDto;
                }

            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PrescricaoItemRespostaDto> ObterJson(List<PrescricaoItemRespostaDto> list, long idGrid, long idDivisao)
        {
            try
            {
                var prescricaoItemResposta = list.FirstOrDefault(m => m.IdGridPrescricaoItemResposta == idGrid && m.DivisaoId == idDivisao);

                return prescricaoItemResposta;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<PrescricaoItemRespostaDto>> Listar(ListarInput input)
        {
            var prescricaoItensRespostaDto = new List<PrescricaoItemRespostaDto>();
            try
            {
                using (var prescricaoItemRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var prescricaoItemHoraRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemHora, long>>())
                {
                    var query = prescricaoItemRespostaRepositorio.Object
                    .GetAll()
                    .AsNoTracking()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro));

                    var contarTipoResposta = await query
                                                 .CountAsync().ConfigureAwait(false);

                    var prescricaoItemResposta = await query
                                                                              .OrderBy(input.Sorting)
                                                                              .PageBy(input)
                                                                              .ToListAsync().ConfigureAwait(false);

                    prescricaoItensRespostaDto = PrescricaoItemRespostaDto.Mapear(prescricaoItemResposta).ToList();

                    var idGrid = 1;
                    //prescricaoItensRespostaDto.ForEach(m => m.IdGridPrescricaoItemResposta = idGrid++);
                    foreach (var prescricaoItemRespostaDto in prescricaoItensRespostaDto)
                    {
                        prescricaoItemRespostaDto.IdGridPrescricaoItemResposta = idGrid++;
                        //buscando os horários atuais
                        var prescricaoItemHoras = await prescricaoItemHoraRepositorio.Object.GetAll().AsNoTracking().Where(m => m.PrescricaoItemRespostaId == prescricaoItemRespostaDto.Id).ToListAsync().ConfigureAwait(false);
                        foreach (var item in prescricaoItemHoras.OrderBy(o => o.DataMedicamento))
                        {
                            prescricaoItemRespostaDto.Horarios += (string.Format("{0:D2}:00 ", item.Hora));
                        }

                        prescricaoItemRespostaDto.Horarios = prescricaoItemRespostaDto.Horarios.Substring(0, prescricaoItemRespostaDto.Horarios.Length - 1);
                    }

                    return new PagedResultDto<PrescricaoItemRespostaDto>(contarTipoResposta, prescricaoItensRespostaDto);
                }
            }
            catch (System.Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoItemRespostaDto>> ListarTodos()
        {
            try
            {
                using (var prescricaoItemRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                {
                    var query = prescricaoItemRespostaRepositorio.Object
                    .GetAll()
                    .AsNoTracking()
                    .OrderBy(m => m.Codigo);

                    var tipoResposta = await query
                                           .ToListAsync().ConfigureAwait(false);

                    var tiposRespostasDto = PrescricaoItemRespostaDto.Mapear(tipoResposta).ToList();

                    return new ListResultDto<PrescricaoItemRespostaDto>
                    {
                        Items = tiposRespostasDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<PrescricaoItemRespostaDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var prescricaoItemRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var prescricaoItemHoraRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemHora, long>>())
                {
                    var query = prescricaoItemRespostaRepositorio.Object
                    .GetAll()
                    .AsNoTracking()
                    .WhereIf(!filtro.IsNullOrEmpty(), m => m.Codigo.Contains(filtro) || m.Descricao.Contains(filtro));

                    var prescricaoItemResposta = await query
                                                     .ToListAsync().ConfigureAwait(false);

                    var prescricaoItensRespostaDto = PrescricaoItemRespostaDto.Mapear(prescricaoItemResposta).ToList();

                    var idGrid = 1;
                    //prescricaoItensRespostaDto.ForEach(m => m.IdGridPrescricaoItemResposta = idGrid++);
                    foreach (var prescricaoItemRespostaDto in prescricaoItensRespostaDto)
                    {
                        prescricaoItemRespostaDto.IdGridPrescricaoItemResposta = idGrid++;
                        //buscando os horários atuais
                        var prescricaoItemHoras = await prescricaoItemHoraRepositorio.Object.GetAll().AsNoTracking().Where(m => m.PrescricaoItemRespostaId == prescricaoItemRespostaDto.Id).ToListAsync().ConfigureAwait(false);
                        foreach (var item in prescricaoItemHoras.OrderBy(o => o.DataMedicamento))
                        {
                            prescricaoItemRespostaDto.Horarios += (string.Format("{0:D2}:00 ", item.Hora));
                        }

                        prescricaoItemRespostaDto.Horarios = prescricaoItemRespostaDto.Horarios.Substring(0, prescricaoItemRespostaDto.Horarios.Length - 1);
                    }

                    return new ListResultDto<PrescricaoItemRespostaDto>
                    {
                        Items = prescricaoItensRespostaDto
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var prescricaoItemRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
            {
                return await this.CreateSelect2(prescricaoItemRespostaRepositorio.Object).ExecuteAsync(dropdownInput).ConfigureAwait(false);
            }
            //return await this.ListarCodigoDescricaoDropdown(dropdownInput, this._prescricaoItemRespostaRepositorio).ConfigureAwait(false);
        }

        [UnitOfWork(false)]
        public Task<FileDto> ListarParaExcel(ListarInput input)
        {
            throw new NotImplementedException();
        }


        [UnitOfWork]
        public async Task Suspender(long id)
        {
            try
            {
                using (var prescricaoItemRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var item = await prescricaoItemRespostaRepositorio.Object.GetAsync(id).ConfigureAwait(false);

                    item.PrescricaoItemStatusId = PrescricaoStatus.Suspensa;
                    item.SuspensoUserId = AbpSession.UserId;
                    item.DataSuspenso = DateTime.Now;

                    await prescricaoItemRespostaRepositorio.Object.UpdateAsync(item).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    unitOfWorkManager.Object.Current.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSuspender"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var prescricaoItemRespostaRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<PrescricaoItemResposta, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await prescricaoItemRespostaRepositorio.Object.DeleteAsync(id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                    unitOfWorkManager.Object.Current.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSuspender"), ex);
            }
        }
    }
}
