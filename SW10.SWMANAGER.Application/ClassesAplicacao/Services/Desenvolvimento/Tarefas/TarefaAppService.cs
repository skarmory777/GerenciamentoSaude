using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Tarefas.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Desenvolvimento
{
    public class TarefaAppService : SWMANAGERAppServiceBase, ITarefaAppService
    {
        #region Cabecalho
        private readonly IRepository<Tarefa, long> _tarefaRepository;
        private readonly IRepository<TarefaIntervalo, long> _intervaloRepository;
        private readonly IProjetoAppService _projetoAppService;
        private readonly IRepository<DocRotulo, long> _docRotuloRepository;
        private readonly IUserAppService _userAppService;
        private readonly IComentarioAppService _comentarioAppService;

        public TarefaAppService(
            IRepository<Tarefa, long> tarefaRepository,
            IRepository<TarefaIntervalo, long> intervaloRepository,
            IProjetoAppService projetoAppService,
            IRepository<DocRotulo, long> docRotuloRepository,
            IUserAppService userAppService,
            IComentarioAppService comentarioAppService
            )
        {
            _tarefaRepository = tarefaRepository;
            _intervaloRepository = intervaloRepository;
            _projetoAppService = projetoAppService;
            _docRotuloRepository = docRotuloRepository;
            _userAppService = userAppService;
            _comentarioAppService = comentarioAppService;
        }
        #endregion cabecalho.

        public async Task CriarOuEditar(TarefaDto input)
        {
            try
            {
                var tarefa = input.MapTo<Tarefa>();

                if (input.Id.Equals(0))
                {
                    await _tarefaRepository.InsertAsync(tarefa);
                }
                else
                {
                    await _tarefaRepository.UpdateAsync(tarefa);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(TarefaDto input)
        {
            try
            {
                await _tarefaRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TarefaDto>> ListarFiltrando(ListarTarefasInput input)
        {
            var contarTarefas = 0;
            List<Tarefa> tarefa;
            List<TarefaDto> tarefasDtos;
            // List<TarefaDto> tarefasDtos = new List<TarefaDto>();

            try
            {
                if (input.StatusId == "--Selecione um valor--")
                {
                    input.StatusId = null;
                }

                if (input.PrioridadeId == "--Selecione um valor--")
                {
                    input.PrioridadeId = null;
                }

                if (input.ModuloId == "--Selecione um valor--")
                {
                    input.ModuloId = null;
                }

                var usersQ = from p in UserManager.Users
                             orderby p.Name
                             ascending
                             select new { id = p.Id, text = p.Name + " " + p.Surname };
                ;

                var users = usersQ.ToList();


                //var users = ListarUsuarios();


                var query = _tarefaRepository
                    .GetAll()
                    .Include(r => r.Modulo)
                    .Include(r => r.Prioridade)
                    .Include(r => r.Projeto)
                    .Include(r => r.Status)
                    .WhereIf(string.IsNullOrEmpty(input.StatusId), r => !r.Status.IsMostrarGlobal)
                    //.Where(r => !r.Status.IsMostrarGlobal)
                    .WhereIf(!string.IsNullOrEmpty(input.ResponsavelId), r => r.ResponsavelId.ToString() == input.ResponsavelId)
                    .WhereIf(!string.IsNullOrEmpty(input.StatusId), r => r.StatusId.ToString() == input.StatusId)
                    .WhereIf(!string.IsNullOrEmpty(input.PrioridadeId), r => r.PrioridadeId.ToString() == input.PrioridadeId)
                    .WhereIf(!string.IsNullOrEmpty(input.ModuloId), r => r.ModuloId.ToString() == input.ModuloId)
                     .WhereIf(!string.IsNullOrEmpty(input.TarefaId), r => r.Id.ToString() == input.TarefaId)
                     //.WhereIf(input.FiltrarData, r => r.DataRegistro >= input.StartDate && r.DataRegistro <= input.EndDate)
                     .WhereIf(input.TipoData == "prevIni", r => r.DataPrevistaInicio >= input.StartDate && r.DataPrevistaInicio <= input.EndDate)
                     .WhereIf(input.TipoData == "prevTerm", r => r.DataPrevistaTermino >= input.StartDate && r.DataPrevistaTermino <= input.EndDate)
                     .WhereIf(input.TipoData == "inicio", r => r.DataInicio >= input.StartDate && r.DataInicio <= input.EndDate)
                     .WhereIf(input.TipoData == "fim", r => r.DataTermino >= input.StartDate && r.DataTermino <= input.EndDate)
                    .WhereIf(!string.IsNullOrEmpty(input.Filtro),
                          r => r.Descricao.ToUpper().Contains(input.Filtro.ToUpper()) ||
                          r.Projeto.Descricao.ToUpper().Contains(input.Filtro.ToUpper()) ||
                          r.Modulo.Descricao.ToUpper().Contains(input.Filtro.ToUpper()) ||
                          r.Prioridade.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                          );

                contarTarefas = await query
                .CountAsync();

                tarefa = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();


                tarefasDtos = new List<TarefaDto>();
                tarefasDtos = tarefa
                .MapTo<List<TarefaDto>>();

                foreach (var t in tarefasDtos)
                {
                    if (t.ResponsavelId != null)
                        t.ResponsavelNome = users.FirstOrDefault(q => q.id == (long)t.ResponsavelId).text;

                    if (t.ProjetoId != null)
                    {
                        var x = await _projetoAppService.Obter((long)t.ProjetoId);
                        t.ProjetoNome = x.Descricao;
                    }

                    if (t.StatusId != null)
                        t.StatusDescricao = _docRotuloRepository.Get((long)t.StatusId).Descricao;

                    if (t.PrioridadeId != null)
                        t.PrioridadeDescricao = _docRotuloRepository.Get((long)t.PrioridadeId).Descricao;

                    if (t.CreatorUserId != null)
                        t.User = _projetoAppService.UsuarioLogado((long)t.CreatorUserId);

                }

                return new PagedResultDto<TarefaDto>(
                contarTarefas,
                tarefasDtos
                );


                // docRotulosDtos = docRotulos
                //   .MapTo<List<DocRotuloDto>>();

                // return new PagedResultDto<TarefaDto>(
                //contarRotulos,
                //docRotulosDtos
                //);


                // return new PagedResultDto<TarefaDto> { Items = tarefasDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public UserManager ListarUsuarios()
        //{
        //    var usersQ = from p in  UserManager.Users
        //                 orderby p.Name
        //                 ascending
        //                 select new { id = p.Id, text = p.Name + " " + p.Surname };
        //    ;

        //    var users = usersQ.ToList();

        //    return users;
        //}

        public List<IEnumerable> ListarUsuarios()
        {
            List<IEnumerable> listaUsusarios = new List<IEnumerable>();

            var usersQ = from p in UserManager.Users
                         orderby p.Name
                         ascending
                         select new { id = p.Id, text = p.Name + " " + p.Surname };
            ;

            var users = usersQ.ToList();

            foreach (var x in users)
            {
                listaUsusarios.Add(x.ToString());
            }

            return listaUsusarios;

        }

        public async Task<PagedResultDto<TarefaDto>> ListarTodos()
        {
            try
            {
                var usersQ = from p in UserManager.Users
                             orderby p.UserName
                             ascending
                             select new { id = p.Id, text = p.UserName };
                ;

                var users = usersQ.ToList();

                var tarefas = await _tarefaRepository
                    .GetAll()

                    .AsNoTracking()
                    .ToListAsync();

                var tarefasDtos = tarefas
                    .MapTo<List<TarefaDto>>();

                foreach (var t in tarefasDtos)
                {
                    if (t.ResponsavelId != null)
                        t.ResponsavelNome = users.FirstOrDefault(q => q.id == (long)t.ResponsavelId).text;

                    if (t.ProjetoId != null)
                    {
                        var x = await _projetoAppService.Obter((long)t.ProjetoId);
                        t.ProjetoNome = x.Descricao;
                    }

                    if (t.StatusId != null)
                        t.StatusDescricao = _docRotuloRepository.Get((long)t.StatusId).Descricao;

                    if (t.PrioridadeId != null)
                        t.PrioridadeDescricao = _docRotuloRepository.Get((long)t.PrioridadeId).Descricao;

                }

                return new PagedResultDto<TarefaDto> { Items = tarefasDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<TarefaDto>> ListarTarefasExecutando()
        {
            try
            {
                var usersQ = from p in UserManager.Users
                             orderby p.UserName
                             ascending
                             select new { id = p.Id, text = p.UserName };
                ;

                var users = usersQ.ToList();

                var tarefas = await _tarefaRepository
                    .GetAll()
                    .Where(t => t.IntervaloInicio != null)
                    .AsNoTracking()
                    .ToListAsync();

                //var tarefasDtos = tarefas
                //    .MapTo<List<TarefaDto>>();

                List<TarefaDto> tarefasDtos = new List<TarefaDto>();
                foreach (var tarefa in tarefas)
                {
                    // var intervalos = _intervaloRepository.GetAll()
                    //     .Where(i => i.TarefaId.Equals(tarefa.Id))
                    ////     .Where(j => j.Inicio >= input.StartDateNotNull && j.Fim <= input.EndDateNotNull);
                    //;

                    TimeSpan ts = new TimeSpan();

                    // if (intervalos.Count() > 0)
                    // {
                    var tarDto = tarefa.MapTo<TarefaDto>();

                    //     foreach (var interval in intervalos)
                    //     {
                    //         var intDto = interval.MapTo<TarefaIntervaloDto>();
                    ts += (DateTime.Now - (DateTime)tarDto.IntervaloInicio);
                    //     }

                    tarDto.TempoDecorrido = string.Format("{0} horas, {1} minutos", ts.Hours, ts.Minutes);

                    if (tarDto.ResponsavelId != null)
                        tarDto.ResponsavelNome = users.FirstOrDefault(q => q.id == (long)tarDto.ResponsavelId).text;

                    //tarDto.CalcularTempoDecorrido();
                    tarefasDtos.Add(tarDto);
                    //}
                }


                return new PagedResultDto<TarefaDto> { Items = tarefasDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<TarefaDto>> ListarTarefasHorasRealizadas(ListarInput input)
        {
            try
            {
                var usersQ = from p in UserManager.Users
                             orderby p.UserName
                             ascending
                             select new { id = p.Id, text = p.UserName };
                ;

                var users = usersQ.ToList();

                List<Tarefa> tarefas = new List<Tarefa>();

                if (string.IsNullOrEmpty(input.Filtro))
                {
                    tarefas = await _tarefaRepository
                    .GetAll()
                    .AsNoTracking()
                    .ToListAsync();
                }
                else
                {
                    tarefas = await _tarefaRepository
                    .GetAll()
                    .Where(t => t.ResponsavelId.ToString().Equals(input.Filtro))
                    .AsNoTracking()
                    .ToListAsync();
                }

                // Verificando se a tarefa foi trabalhada dentro do periodo selecionado no filtro range picker
                List<TarefaDto> tarefasDtos = new List<TarefaDto>();
                foreach (var tarefa in tarefas)
                {
                    var intervalos = _intervaloRepository.GetAll()
                        .Where(i => i.TarefaId.Equals(tarefa.Id))
                        .Where(j => j.Inicio >= input.StartDateNotNull && j.Fim <= input.EndDateNotNull);

                    TimeSpan ts = new TimeSpan();

                    if (intervalos.Count() > 0)
                    {
                        var tarDto = tarefa.MapTo<TarefaDto>();

                        foreach (var interval in intervalos)
                        {
                            var intDto = interval.MapTo<TarefaIntervaloDto>();
                            ts += ((DateTime)intDto.Fim - (DateTime)intDto.Inicio);
                        }

                        tarDto.TempoDecorrido = string.Format("{0} horas, {1} minutos", ts.Hours, ts.Minutes);
                        if (tarDto.ResponsavelId != null)
                            tarDto.ResponsavelNome = users.FirstOrDefault(q => q.id == (long)tarDto.ResponsavelId).text;

                        //tarDto.CalcularTempoDecorrido();
                        tarefasDtos.Add(tarDto);
                    }
                }

                //var tarefasDtos = tarefas
                //    .MapTo<List<TarefaDto>>();

                return new PagedResultDto<TarefaDto> { Items = tarefasDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<TarefaIntervaloDto>> ListarIntervalosExecutando()
        {
            try
            {
                var intervalos = await _intervaloRepository
                    .GetAll()
                    .Where(t => t.Fim == null)
                    .Include(t => t.Tarefa)
                    .AsNoTracking()
                    .ToListAsync();

                var intervalosDtos = intervalos
                    .MapTo<List<TarefaIntervaloDto>>();

                foreach (var i in intervalosDtos)
                {
                    i.CalcularTempoDecorrido();
                }

                return new PagedResultDto<TarefaIntervaloDto> { Items = intervalosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<TarefaIntervaloDto>> ListarIntervalosHorasRealizadas(ListarInput input)
        {
            try
            {
                List<TarefaIntervalo> intervalos = new List<TarefaIntervalo>();

                if (string.IsNullOrEmpty(input.Filtro))
                {
                    intervalos = await _intervaloRepository
                    .GetAll()
                    .Include(f => f.Tarefa)
                    .Where(j => j.Inicio >= input.StartDateNotNull && j.Fim <= input.EndDateNotNull)
                    .AsNoTracking()
                    .ToListAsync();
                }
                else
                {
                    intervalos = await _intervaloRepository
                    .GetAll()
                    .Include(f => f.Tarefa)
                    .Where(t => t.ResponsavelId.ToString().Equals(input.Filtro))
                    .Where(j => j.Inicio >= input.StartDateNotNull && j.Fim <= input.EndDateNotNull)
                    .AsNoTracking()
                    .ToListAsync();
                }

                var intervalosDtos = intervalos
                    .MapTo<List<TarefaIntervaloDto>>();

                foreach (var interv in intervalosDtos)
                {
                    interv.Descricao = interv.Tarefa.Descricao;
                    interv.CalcularTempoDecorrido();
                }

                return new PagedResultDto<TarefaIntervaloDto> { Items = intervalosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<PagedResultDto<TarefaIntervaloDto>> ListarIntervalosPorUsuarioPeriodo (ListarInput input)
        //{
        //    try
        //    {
        //        var intervalos = await _intervaloRepository
        //            .GetAllListAsync()
        //        //.GetAll();
        //        //.GetAllAsync();
        //        //  .ToList();
        //        //.GetAllIncluding(
        //        //    r => r.Tarefa//.ResponsavelId
        //        //)
        //        //.Where(i => i.Tarefa.ResponsavelId.ToString().Equals(input.Filtro))
        //        //.Where(j => j.Inicio >= input.StartDateNotNull && j.Fim <= input.EndDateNotNull)
        //        //.ToList()
        //        ;

        //        List<TarefaIntervaloDto> intervalosDtos = new List<TarefaIntervaloDto>();

        //        foreach (var inter in intervalos)
        //        {
        //            var novoIntervalo = inter.MapTo<TarefaIntervaloDto>();

        //            intervalosDtos.Add(novoIntervalo);
        //        }

        //        //var intervalosDtos = intervalos
        //        //    .MapTo<List<TarefaIntervaloDto>>();

        //        return new PagedResultDto<TarefaIntervaloDto> { Items = intervalosDtos };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }
        //}

        public async Task<TarefaDto> Obter(long id)
        {
            try
            {
                var query = await _tarefaRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var tarefa = query.MapTo<TarefaDto>();

                return tarefa;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        // 
        public async Task<bool> IniciarContagemIntervalo(long tarefaId)
        {
            try
            {
                var tarefa = _tarefaRepository.Get(tarefaId);

                if (tarefa.IntervaloInicio != null)
                {
                    return false;
                }

                var tarefas = _tarefaRepository.GetAll();

                var tarefaPreviaExecutando = tarefas.FirstOrDefault(t => t.ResponsavelId == tarefa.ResponsavelId && t.IntervaloInicio != null);

                if (tarefaPreviaExecutando != null)
                {
                    TarefaIntervalo novoIntervalo = new TarefaIntervalo();
                    novoIntervalo.Inicio = (DateTime)tarefaPreviaExecutando.IntervaloInicio;
                    novoIntervalo.Fim = DateTime.Now;
                    novoIntervalo.TarefaId = tarefaPreviaExecutando.Id;
                    novoIntervalo.ResponsavelId = await _comentarioAppService.GetUsuarioLogadoAsync();
                    _intervaloRepository.Insert(novoIntervalo);
                    tarefaPreviaExecutando.IntervaloInicio = null;
                    await _tarefaRepository.UpdateAsync(tarefaPreviaExecutando);
                }

                tarefa.IntervaloInicio = DateTime.Now;

                await _tarefaRepository.UpdateAsync(tarefa);

                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<bool> PararContagemIntervalo(long tarefaId)
        {
            try
            {
                var tarefa = _tarefaRepository.Get(tarefaId);

                if (tarefa.IntervaloInicio == null)
                    return false;

                TarefaIntervalo novoIntervalo = new TarefaIntervalo();
                novoIntervalo.Inicio = (DateTime)tarefa.IntervaloInicio;
                novoIntervalo.Fim = DateTime.Now;
                novoIntervalo.TarefaId = tarefa.Id;
                novoIntervalo.ResponsavelId = await _comentarioAppService.GetUsuarioLogadoAsync();
                _intervaloRepository.Insert(novoIntervalo);

                tarefa.IntervaloInicio = null;

                await _tarefaRepository.UpdateAsync(tarefa);

                return true;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<string> CalcularTempoDecorrido(long tarefaId)
        {
            try
            {
                var intervalos = _intervaloRepository
                    .GetAll()
                    .Where(i => i.TarefaId.Equals(tarefaId));

                TimeSpan tempoDecorrido = new TimeSpan();

                foreach (var intervalo in intervalos)
                {
                    tempoDecorrido += ((DateTime)intervalo.Fim - (DateTime)intervalo.Inicio);
                }

                return string.Format("{0} horas, {1} minutos", tempoDecorrido.Hours, tempoDecorrido.Minutes);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<ProducaoResponsavel> CalcularProducaoPorReponsavel()
        {
            try
            {
                var intervalosQ = await _intervaloRepository
                   .GetAllListAsync();
                var intervalos = intervalosQ.ToList();

                var usersQ = from p in UserManager.Users
                             orderby p.UserName
                             ascending
                             select new { id = p.Id, text = p.UserName };
                ;

                var users = usersQ.ToList();
                var porResponsaveis = intervalos.GroupBy(g => g.ResponsavelId);

                ProducaoResponsavel producaoResponsavel = new ProducaoResponsavel();
                var tarefasExecutando = AsyncHelper.RunSync(() => ListarTarefasExecutando());
                foreach (var resp in porResponsaveis)
                {
                    var respId = resp.FirstOrDefault().ResponsavelId;

                    if (respId == null)
                        continue;

                    var user = users.FirstOrDefault(q => q.id == (long)respId);
                    var nome = user.text;

                    TimeSpan horasTrabalhadas = new TimeSpan();
                    foreach (var intervalo in resp)
                    {
                        horasTrabalhadas += ((DateTime)intervalo.Fim - (DateTime)intervalo.Inicio);
                    }

                    // Se o responsavel estiver executando tarefa neste momento, somar tambem tempo de trabalho em execucao
                    var tarefaExecutandoResp = tarefasExecutando.Items.FirstOrDefault(t => t.ResponsavelId == resp.Key);
                    if (tarefaExecutandoResp != null)
                    {
                        horasTrabalhadas += DateTime.Now - (DateTime)tarefaExecutandoResp.IntervaloInicio;
                    }

                    producaoResponsavel.NovoDado(nome, horasTrabalhadas.TotalHours);
                }

                return producaoResponsavel;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroCalcular"), ex);
            }
        }
    }

    public class ProducaoResponsavel
    {
        public List<KeyValuePair<string, double>> Dados { get; set; }

        public ProducaoResponsavel()
        {
            Dados = new List<KeyValuePair<string, double>>();
        }

        public void NovoDado(string responsavel, double valor)
        {
            Dados.Add(new KeyValuePair<string, double>(responsavel, valor));
        }
    }
}
