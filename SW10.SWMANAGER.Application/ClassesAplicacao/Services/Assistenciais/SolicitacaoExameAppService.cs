using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using MoreLinq;
using Org.BouncyCastle.Crypto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Exames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Laboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Auditing;
    using Abp.Dependency;
    using Cadastros.Pessoas.Dto;
    using Dapper;
    using RestSharp;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
    using SW10.SWMANAGER.Dto;
    using SW10.SWMANAGER.Helpers;
    using System.Configuration;
    using System.Data.SqlClient;

    public class SolicitacaoExameAppService : SWMANAGERAppServiceBase, ISolicitacaoExameAppService
    {
        //private readonly IRepository<SolicitacaoExame, long> _solicitacaoExameRepository;
        //private readonly IRepository<SolicitacaoExameItem, long> _solicitacaoExameItemRepository;
        //private readonly IAtendimentoAppService _atendimentoAppService;
        //private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly IUltimoIdAppService _ultimoIdAppService;

        //public SolicitacaoExameAppService(
        //    IRepository<SolicitacaoExame, long> solicitacaoExameRepository,
        //    IRepository<SolicitacaoExameItem, long> solicitacaoExameItemRepository,
        //    IAtendimentoAppService atendimentoAppService,
        //    IUnitOfWorkManager unitOfWorkManager,
        //    IUltimoIdAppService ultimoIdAppService
        //    )
        //{
        //    _solicitacaoExameRepository = solicitacaoExameRepository;
        //    _solicitacaoExameItemRepository = solicitacaoExameItemRepository;
        //    _atendimentoAppService = atendimentoAppService;
        //    _unitOfWorkManager = unitOfWorkManager;
        //    _ultimoIdAppService = ultimoIdAppService;
        //}

        [UnitOfWork(IsDisabled = false)]
        public async Task<DefaultReturn<SolicitacaoExameDto>> CriarOuEditar(SolicitacaoExameDto input)
        {
            try
            {
                var defaultReturn = new DefaultReturn<SolicitacaoExameDto>
                {
                    Errors = new List<ErroDto>()
                };


                var itens = new List<SolicitacaoExameItem>();
                if (!input.Itens.IsNullOrWhiteSpace())
                {
                    itens = JsonConvert.DeserializeObject<List<SolicitacaoExameItem>>(input.Itens, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });

                    if (itens.Count() == 0)
                    {
                        defaultReturn.Errors.Add(new ErroDto { CodigoErro = "SOEX0002" });
                    }
                    else
                    {

                        var group = itens.GroupBy(g => g.FaturamentoItemId).Select(s => s.Count());

                        if (group.Max() > 1)
                        {
                            defaultReturn.Errors.Add(new ErroDto { CodigoErro = "SOEX0001" });
                        }
                    }


                }

                if (defaultReturn.Errors.Count() == 0)
                {
                    foreach (var item in itens)
                    {
                        item.FaturamentoItem = null;
                        item.Material = null;
                    }

                    var solicitacaoExame = new SolicitacaoExame
                    {
                        AtendimentoId = input.AtendimentoId,
                        Codigo = input.Codigo,
                        CreationTime = input.CreationTime,
                        CreatorUserId = input.CreatorUserId,
                        DataSolicitacao = input.DataSolicitacao,
                        DeleterUserId = input.DeleterUserId,
                        DeletionTime = input.DeletionTime,
                        Descricao = input.Descricao,
                        Id = input.Id,
                        IsDeleted = input.IsDeleted,
                        IsSistema = input.IsSistema,
                        LastModificationTime = input.LastModificationTime,
                        LastModifierUserId = input.LastModifierUserId,
                        LeitoId = input.LeitoId,
                        MedicoSolicitanteId = input.MedicoSolicitanteId,
                        Observacao = input.Observacao,
                        OrigemId = input.OrigemId,
                        PrescricaoId = input.PrescricaoId,
                        Prioridade = input.Prioridade,
                        UnidadeOrganizacionalId = input.UnidadeOrganizacionalId,
                        Justificativa = input.Justificativa
                    };

                    using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                    using (var solicitacaoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExame, long>>())
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    {
                        //input.MapTo<SolicitacaoExame>();
                        if (input.Id.Equals(0))
                        {

                            var ultimosId = await ultimoIdAppService.Object.ListarTodos().ConfigureAwait(false);
                            var ultimoId = ultimosId.Items.FirstOrDefault(m => m.NomeTabela == "SolicitacaoExame");
                            solicitacaoExame.Codigo = ultimoId.Codigo;
                            input.Codigo = solicitacaoExame.Codigo;
                            var codigo = Convert.ToInt64(ultimoId.Codigo);
                            codigo++;
                            ultimoId.Codigo = codigo.ToString();
                            await ultimoIdAppService.Object.CriarOuEditar(ultimoId).ConfigureAwait(false);


                            solicitacaoExame.SolicitacaoExameItens = itens;
                            using (var unitOfWork = unitOfWorkManager.Object.Begin())
                            {
                                input.Id = await solicitacaoExameRepository.Object.InsertAndGetIdAsync(solicitacaoExame).ConfigureAwait(false);
                                unitOfWork.Complete();
                                unitOfWorkManager.Object.Current.SaveChanges();
                                unitOfWork.Dispose();
                            }
                        }
                        else
                        {
                            using (var unitOfWork = unitOfWorkManager.Object.Begin())
                            {
                                var ori = await solicitacaoExameRepository.Object
                                              .GetAll()
                                              .Include(i => i.SolicitacaoExameItens)
                                              .Where(w => w.Id == input.Id)
                                              .FirstOrDefaultAsync().ConfigureAwait(false);

                                ori.Codigo = input.Codigo;
                                ori.DataSolicitacao = input.DataSolicitacao;
                                ori.Descricao = input.Descricao;
                                ori.LeitoId = input.LeitoId;
                                ori.MedicoSolicitanteId = input.MedicoSolicitanteId;
                                ori.Observacao = input.Observacao;
                                ori.OrigemId = input.OrigemId;
                                ori.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                                ori.Justificativa = input.Justificativa;

                                //Delete
                                ori.SolicitacaoExameItens.RemoveAll(r => itens.All(a => a.Id != r.Id));

                                //Update
                                foreach (var item in ori.SolicitacaoExameItens)
                                {
                                    var novo = itens.First(w => w.Id == item.Id);
                                    item.Codigo = novo.Codigo;
                                    item.DataValidade = novo.DataValidade;
                                    item.Descricao = novo.Descricao;
                                    item.FaturamentoItemId = novo.FaturamentoItemId;
                                    item.GuiaNumero = novo.GuiaNumero;
                                    item.Justificativa = novo.Justificativa;
                                    item.KitExameId = novo.KitExameId;
                                    item.MaterialId = novo.MaterialId;
                                    item.SenhaNumero = novo.SenhaNumero;
                                    item.StatusSolicitacaoExameItemId = novo.StatusSolicitacaoExameItemId;
                                }

                                //Insert
                                foreach (var item in itens.Where(w => w.Id == 0))
                                {
                                    ori.SolicitacaoExameItens.Add(new SolicitacaoExameItem
                                    {
                                        SolicitacaoExameId = item.SolicitacaoExameId,
                                        Codigo = item.Codigo,
                                        DataValidade = item.DataValidade,
                                        Descricao = item.Descricao,
                                        FaturamentoItemId = item.FaturamentoItemId,
                                        GuiaNumero = item.GuiaNumero,
                                        Justificativa = item.Justificativa,
                                        KitExameId = item.KitExameId,
                                        MaterialId = item.MaterialId,
                                        SenhaNumero = item.SenhaNumero,
                                        StatusSolicitacaoExameItemId = item.StatusSolicitacaoExameItemId
                                    });
                                }

                                await solicitacaoExameRepository.Object.UpdateAsync(ori).ConfigureAwait(false);
                                unitOfWork.Complete();
                                unitOfWorkManager.Object.Current.SaveChanges();
                                unitOfWork.Dispose();
                                //input = _solicitacaoExame.MapTo<SolicitacaoExameDto>();
                            }
                        }
                    }

                    if (ConfigurationManager.AppSettings["ExecutarProcedureSolicitacaoExame"] == "true")
                    {
                        try
                        {
                            using (var connection = new SqlConnection(this.GetConnection()))
                            {
                                var retorno = await connection.QueryAsync("sp_ImportaASAExame");
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    using (var parametrizacoesDomainService = IocManager.Instance.ResolveAsDisposable<IParametrizacoesDomainService>())
                    {
                        if (parametrizacoesDomainService.Object.AssistencialHabilitaColetaAutomatica())
                        {
                            await GerarColeta(input.Id).ConfigureAwait(false);
                        }
                    }
                    
                    defaultReturn.ReturnObject = input;
                }

                return defaultReturn;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var solicitacaoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExame, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await solicitacaoExameRepository.Object.DeleteAsync(id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<SolicitacaoExameDto>> ListarTodos()
        {
            try
            {
                using (var solicitacaoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExame, long>>())
                {
                    var query = solicitacaoExameRepository.Object
                    .GetAll()
                    .AsNoTracking()
                    .Include(m => m.Atendimento.Paciente)
                    .Include(m => m.Atendimento.Paciente.SisPessoa)
                    .Include(m => m.Atendimento.Medico)
                    .Include(m => m.Atendimento.Medico.SisPessoa)
                    .Include(m => m.Atendimento.Empresa)
                    .Include(m => m.UnidadeOrganizacional);


                    var admissoesMedicasDto = await query.ToListAsync().ConfigureAwait(false);

                    return new ListResultDto<SolicitacaoExameDto> { Items = SolicitacaoExameDto.Mapear(admissoesMedicasDto).ToList() };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<SolicitacaoExameIndex>> Listar(ListarSolicitacoesExamesInput input)
        {
            var contar = 0;
            var listDto = new List<SolicitacaoExameIndex>();

            input.StartDate = input.StartDate.Date;
            input.EndDate = input.EndDate.Date.AddDays(1).AddMilliseconds(-1);
            try
            {
                using (var solicitacaoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExame, long>>())
                {
                    var query = solicitacaoExameRepository.Object
                    .GetAll()
                    .AsNoTracking()
                    //.Include(m => m.Atendimento.Paciente)
                    //.Include(m => m.Atendimento.Medico)
                    //.Include(m => m.Atendimento.Empresa)
                    .Include(m => m.Atendimento.UnidadeOrganizacional)
                    .Include(m => m.MedicoSolicitante)
                    //.Include(m => m.Leito)
                    .Where(m => m.CreationTime >= input.StartDate && m.CreationTime <= input.EndDate)
                    .Where(m => m.Atendimento.PacienteId == input.PacienteId)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        //m.CreationTime.IsBetween(input.StartDate, input.EndDate) || //.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                        m.Atendimento.Medico.NomeCompleto.Contains(input.Filtro) ||
                        m.Atendimento.Medico.Cpf.Contains(input.Filtro) ||
                        m.Atendimento.Medico.Nascimento.Value.ToShortTimeString().Contains(input.Filtro) ||
                        m.Atendimento.Medico.Rg.Contains(input.Filtro) ||
                        m.Atendimento.Paciente.NomeCompleto.Contains(input.Filtro) ||
                        m.Atendimento.Paciente.Cpf.Contains(input.Filtro) ||
                        ((DateTime)m.Atendimento.Paciente.Nascimento).ToShortTimeString().Contains(input.Filtro) || m
                            .Atendimento.Paciente.Rg.Contains(input.Filtro)).Select(
                        m => new SolicitacaoExameIndex
                        {
                            Id = m.Id,
                            Codigo = m.Codigo,
                            DataSolicitacao = m.DataSolicitacao,
                            MedicoSolicitante = m.MedicoSolicitante.NomeCompleto,
                            UnidadeOrganizacional = m.UnidadeOrganizacional.Descricao,
                            IsDeleted = m.IsDeleted
                        });

                    contar = await query.CountAsync().ConfigureAwait(false);

                    var list = await query
                                   .OrderBy(input.Sorting)
                                   .PageBy(input)
                                   .ToListAsync().ConfigureAwait(false);

                    listDto = list;

                    return new PagedResultDto<SolicitacaoExameIndex>(contar, listDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<SolicitacaoExameDto> Obter(long id)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var solicitacaoExameItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExameItem, long>>())
                using (var solicitacaoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExame, long>>())
                {
                    var query = await solicitacaoExameRepository.Object
                                .GetAll()
                                .AsNoTracking()
                                .Where(m => m.Id == id)
                                //.Include(m => m.Atendimento.Medico)
                                //.Include(m => m.Atendimento.Empresa)
                                //.Include(m => m.UnidadeOrganizacional)
                                //.Include(m => m.MedicoSolicitante)
                                //.Include(m => m.Leito)
                                //.Include(m => m.SolicitacaoExameItens)
                                .Select(m => new SolicitacaoExameDto
                                {
                                    AtendimentoId = m.AtendimentoId,
                                    Codigo = m.Codigo,
                                    CreationTime = m.CreationTime,
                                    CreatorUserId = m.CreatorUserId,
                                    DataSolicitacao = m.DataSolicitacao,
                                    DeleterUserId = m.DeleterUserId,
                                    DeletionTime = m.DeletionTime,
                                    Descricao = m.Descricao,
                                    Id = m.Id,
                                    IsDeleted = m.IsDeleted,
                                    IsSistema = m.IsSistema,
                                    LastModificationTime = m.LastModificationTime,
                                    LastModifierUserId = m.LastModifierUserId,
                                    LeitoId = m.LeitoId,
                                    MedicoSolicitanteId = m.MedicoSolicitanteId,
                                    Observacao = m.Observacao,
                                    OrigemId = m.OrigemId,
                                    PrescricaoId = m.PrescricaoId,
                                    Prioridade = m.Prioridade,
                                    UnidadeOrganizacionalId = m.UnidadeOrganizacionalId,
                                })
                                .FirstOrDefaultAsync().ConfigureAwait(false);
                    if (query.AtendimentoId.HasValue)
                    {
                        query.Atendimento = await atendimentoAppService.Object.Obter(query.AtendimentoId.Value);
                    }

                    //itens da solicitação
                    var itens = await solicitacaoExameItemRepository.Object
                                    .GetAll()
                                    .AsNoTracking()
                                    .Include(m => m.FaturamentoItem)
                                    .Include(m => m.KitExame)
                                    .Include(m => m.Material)
                                    .Include(m => m.StatusSolicitacaoExameItem)
                                    .Where(w => w.SolicitacaoExameId == id)
                                    .ToListAsync().ConfigureAwait(false);
                    var idGrid = 1;
                    var itensDto = SolicitacaoExameItemDto.Mapear(itens);
                    itensDto.ForEach(c => c.IdGrid = idGrid++);
                    var json = JsonConvert.SerializeObject(itensDto);
                    query.Itens = json;
                    query.SolicitacaoItens = itensDto;
                    var result = query;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<SolicitacaoExameDto>> ListarFiltro(string filtro)
        {
            try
            {
                using (var solicitacaoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExame, long>>())
                {
                    var query = solicitacaoExameRepository.Object
                    .GetAll()
                    .AsNoTracking()
                    .Include(m => m.Atendimento.Paciente)
                    .Include(m => m.Atendimento.Paciente.SisPessoa)
                    .Include(m => m.Atendimento.Medico)
                    .Include(m => m.Atendimento.Medico.SisPessoa)
                    .Include(m => m.Atendimento.Empresa)
                    .Include(m => m.UnidadeOrganizacional)
                    .WhereIf(
                        !filtro.IsNullOrWhiteSpace(),
                        m => m.CreationTime.ToShortTimeString().Contains(filtro) || m.Atendimento.Medico.NomeCompleto.Contains(filtro) ||
                         m.Atendimento.Medico.Cpf.Contains(filtro) ||
                         m.Atendimento.Medico.Nascimento.Value.ToShortTimeString().Contains(filtro) ||
                         m.Atendimento.Medico.Rg.Contains(filtro) ||
                         m.Atendimento.Paciente.NomeCompleto.Contains(filtro) ||
                         m.Atendimento.Paciente.Cpf.Contains(filtro) ||
                         ((DateTime)m.Atendimento.Paciente.Nascimento).ToShortTimeString().Contains(filtro) ||
                         m.Atendimento.Paciente.Rg.Contains(filtro));

                    var admissoesMedicas = await query.ToListAsync().ConfigureAwait(false);
                    var admissoesMedicasDto = SolicitacaoExameDto.Mapear(admissoesMedicas).ToList();

                    return new ListResultDto<SolicitacaoExameDto> { Items = admissoesMedicasDto };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var solicitacaoExameDtos = new List<SolicitacaoExameDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }
                using (var solicitacaoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExame, long>>())
                {
                    var query = solicitacaoExameRepository.Object.GetAll().AsNoTracking()
                    .WhereIf(
                        !dropdownInput.search.IsNullOrEmpty(),
                        m => m.Descricao.Contains(dropdownInput.search) || m.Codigo.Contains(dropdownInput.search))
                    .OrderBy(p => p.Descricao)
                    .Select(p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) });

                    var queryResultPage = query
                      .Skip(numberOfObjectsPerPage * pageInt)
                      .Take(numberOfObjectsPerPage);

                    var result = await queryResultPage.ToListAsync().ConfigureAwait(false);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = result, TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task RequisitarSolicitacao(SolicitacaoExameInputDto input)
        {
            try
            {
                var solicitacao = (await this.CriarOuEditar(input.Solicitacao).ConfigureAwait(false)).ReturnObject;

                foreach (var item in input.SolicitacaoExameItens)
                {
                    var solicitacaoItem = new SolicitacaoExameItem
                    {
                        Codigo = item.Codigo,
                        CreationTime = DateTime.Now,
                        CreatorUserId = AbpSession.UserId,
                        DataValidade = item.DataValidade,
                        Descricao = item.Descricao,
                        FaturamentoItemId = item.FaturamentoItemId,
                        GuiaNumero = item.GuiaNumero,
                        Id = item.Id,
                        IsDeleted = false,
                        IsSistema = item.IsSistema,
                        Justificativa = item.Justificativa,
                        KitExameId = item.KitExameId,
                        MaterialId = item.MaterialId,
                        SenhaNumero = item.SenhaNumero,
                        SolicitacaoExameId = solicitacao.Id,
                        PrescricaoItemRespostaId = item.PrescricaoItemRespostaId
                    };

                    //item.SolicitacaoExameId = solicitacao.Id;
                    using (var solicitacaoExameItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExameItem, long>>())
                    {
                        await solicitacaoExameItemRepository.Object.InsertAsync(solicitacaoItem).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroSalvar", ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<SolicitacaoExameDto> ObterParaImpressao(long id)
        {
            try
            {
                using (var solicitacaoExameItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExameItem, long>>())
                using (var solicitacaoExameRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExame, long>>())
                {
                    var query = await solicitacaoExameRepository.Object
                                .GetAll()
                                .AsNoTracking()
                                .Where(m => m.Id == id)
                                 .Include(m => m.Origem)
                                .Include(m => m.Atendimento)
                                .Include(m => m.Atendimento.Paciente)
                                .Include(m => m.Atendimento.Paciente.SisPessoa)
                                .Include(m => m.Atendimento.Paciente.SisPessoa.Sexo)
                                 .Include(m => m.Atendimento.Convenio)
                                .Include(m => m.Atendimento.Convenio.SisPessoa)
                                 .Include(m => m.Atendimento.Plano)
                                .Include(m => m.Atendimento.Empresa)
                                .Include(m => m.Atendimento.ProtocoloAtendimento)
                                //.Include(m => m.UnidadeOrganizacional)
                                .Include(m => m.MedicoSolicitante.SisPessoa)
                                //.Include(m => m.Leito)
                                //.Include(m => m.SolicitacaoExameItens)
                                .Select(m => new SolicitacaoExameDto
                                {
                                    AtendimentoId = m.AtendimentoId,
                                    Atendimento = new AtendimentoDto
                                    {
                                        Codigo = m.Atendimento.Codigo,
                                        ConvenioId = m.Atendimento.ConvenioId,
                                        Convenio = new Cadastros.Convenios.Dto.ConvenioDto { Codigo = m.Atendimento.Convenio.Codigo, SisPessoa = new SisPessoaDto { NomeFantasia = m.Atendimento.Convenio.SisPessoa.NomeFantasia } },
                                        EmpresaId = m.Atendimento.EmpresaId,
                                        PacienteId = m.Atendimento.PacienteId,
                                        Paciente = new Cadastros.Pacientes.Dto.PacienteDto
                                        {
                                            CodigoPaciente = m.Atendimento.Paciente.CodigoPaciente,
                                            NomeCompleto = m.Atendimento.Paciente.SisPessoa.NomeCompleto,
                                            Nascimento = m.Atendimento.Paciente.SisPessoa.Nascimento
                                        },
                                        ProtocoloAtendimento = new ProtocoloAtendimentoDto { Descricao = m.Atendimento.ProtocoloAtendimento.Descricao }
                                    },
                                    Codigo = m.Codigo,
                                    Descricao = m.Descricao,
                                    MedicoSolicitanteId = m.MedicoSolicitanteId,
                                    MedicoSolicitante = new Cadastros.Medicos.Dto.MedicoDto { NumeroConselho = m.MedicoSolicitante.NumeroConselho, NomeCompleto = m.MedicoSolicitante.SisPessoa.NomeCompleto },
                                    Observacao = m.Observacao,
                                    Sexo = m.Atendimento.Paciente.SisPessoa.Sexo.Descricao,
                                    Origem = m.Origem.Descricao,
                                    Plano = m.Atendimento.Plano.Descricao,
                                    Justificativa = m.Justificativa
                                })
                                .FirstOrDefaultAsync().ConfigureAwait(false);
                    //itens da solicitação
                    var itens = await solicitacaoExameItemRepository.Object
                                    .GetAll()
                                    .AsNoTracking()
                                    .Include(m => m.FaturamentoItem)
                                    .Include(m => m.FaturamentoItem.Grupo)
                                    .Include(m => m.KitExame)
                                    .Include(m => m.Material)
                                    .Include(m => m.StatusSolicitacaoExameItem)
                                    .Where(w => w.SolicitacaoExameId == id)
                                    .ToListAsync().ConfigureAwait(false);
                    var idGrid = 1;
                    var itensDto = SolicitacaoExameItemDto.Mapear(itens);
                    itensDto.ForEach(c => c.IdGrid = idGrid++);
                    var json = JsonConvert.SerializeObject(itensDto);
                    var itensStr = json;
                    query.Itens = json;
                    query.SolicitacaoItens = itensDto;
                    var result = query;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        
        public bool ValidarSolicitacaoExame(long atendimentoId)
        {
            using(var parametrizacoesAppService = IocManager.Instance.ResolveAsDisposable<IParametrizacoesAppService>())
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {
                var parametrizacao = parametrizacoesAppService.Object.GetParametrizacoesSync();
                var atendimentoIsInternacao = atendimentoRepository.Object
                    .GetAll()
                    .AsNoTracking()
                    .Where(x => x.Id == atendimentoId)
                    .Select(x => x.IsInternacao).FirstOrDefault();
                if (parametrizacao?.SolicitacaoExameHoraOutroDia != null)
                {
                    return DateTime.Now.TimeOfDay >= parametrizacao.SolicitacaoExameHoraOutroDia && atendimentoIsInternacao;
                }

                return false;
            }
        }


        public byte[] RetornaArquivoSolicitacaoExame(long solicitacaoExameId)
        {
            return this.CreateJasperReport("SolicitacaoExame")
                .SetMethod(Method.POST)
                .AddParameter("SolicitacaoExameId", solicitacaoExameId.ToString())
                .AddParameter("UsuarioImpressao", this.GetCurrentUser().FullName)
                .AddParameter("Dominio", this.GetConnectionStringName())
                .GenerateReport();
        }
        
        public async Task GerarColeta(long id)
        {

            using (var SolicitacaoRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExame, long>>())
            using (var SolicitacaoExameItemRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<SolicitacaoExameItem, long>>())
            using (var labResultadoAppService = IocManager.Instance.ResolveAsDisposable<IResultadoAppService>())
            using (var labResultadoExameAppService =
                IocManager.Instance.ResolveAsDisposable<IResultadoExameAppService>())
            using (var labResultadoExameRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            {
                try
                {
                    var solicitacao = SolicitacaoExameDto.Mapear(await SolicitacaoRepository.Object.GetAll()
                        .AsNoTracking()
                        .Include(x => x.Atendimento)
                        .Include(x => x.Leito)
                        .Include(x => x.UnidadeOrganizacional)
                        .Include(x => x.MedicoSolicitante)
                        .FirstOrDefaultAsync(x => x.Id == id));

                    solicitacao.SolicitacaoItens = SolicitacaoExameItemDto.Mapear(await SolicitacaoExameItemRepository
                        .Object.GetAll().AsNoTracking().Where(x => x.SolicitacaoExameId == solicitacao.Id)
                        .ToListAsync());

                    var solicitacaoItemIds = solicitacao.SolicitacaoItens.Select(x => x.Id);
                    var labResultadoSolicitacao =
                        await labResultadoAppService.Object.ObterPorSolicitacaoExameId(solicitacao.Id);

                    if (labResultadoSolicitacao != null)
                    {
                        var resultadoExamesList = (await labResultadoExameAppService.Object
                            .ListarPorResultado(labResultadoSolicitacao.Id)
                            .ConfigureAwait(false)).Items.ToList();
                        var labResultadoSolicitacaoExames = await labResultadoExameRepository.Object.GetAll()
                            .AsNoTracking().Where(x => x.ResultadoId == labResultadoSolicitacao.Id).ToListAsync();
                        var labResultadoSolicitacaoExamesIds =
                            labResultadoSolicitacaoExames.Select(x => x.SolicitacaoExameId);
                        if (solicitacaoItemIds.Any(x => !labResultadoSolicitacaoExamesIds.Contains(x)))
                        {
                            resultadoExamesList.AddRange(solicitacao.SolicitacaoItens
                                .Where(x =>
                                    solicitacaoItemIds.Where(x => !labResultadoSolicitacaoExamesIds.Contains(x))
                                        .Contains(x.Id))
                                .Select(x => ResultadoExameIndexCrudDto.Mapear(x, solicitacao.AtendimentoId)));
                        }

                        labResultadoSolicitacao.ResultadosExamesList = JsonConvert.SerializeObject(resultadoExamesList);
                    }
                    else
                    {
                        var resultadoExamesList = (solicitacao.SolicitacaoItens.Select(x =>
                            ResultadoExameIndexCrudDto.Mapear(x, solicitacao.AtendimentoId)));
                        resultadoExamesList.ForEach(x=> x.ExameStatusId = ExameStatusDto.Inicial);
                        labResultadoSolicitacao = ResultadoDto.MapearSolicitacaoParaResultado(solicitacao);
                        labResultadoSolicitacao.ResultadoStatusId = ResultadoStatusDto.Inicial;
                        labResultadoSolicitacao.ResultadosExamesList = JsonConvert.SerializeObject(resultadoExamesList);
                    }

                    await labResultadoAppService.Object.CriarOuEditar(labResultadoSolicitacao);

                }
                catch (Exception e)
                {
                    
                }

            }
        }


        public async Task<LaboratorioPainelDetalhamentoCounters> RetornaContadores(long? labResultadoId)
        {
            using (var labResultadoExameRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<ResultadoExame, long>>())
            {
                var solicitaoItems = labResultadoExameRepository.Object.GetAll().AsNoTracking()
                    .Where(x => x.ResultadoId == labResultadoId);
                var result = new LaboratorioPainelDetalhamentoCounters
                {
                    InicialValor = solicitaoItems.Where(x => x.ExameStatusId ==ExameStatusDto.Inicial).DistinctBy(x=> x.Id).Count(),
                    EmColetaValor = solicitaoItems.Where(x => x.ExameStatusId ==ExameStatusDto.EmColeta).DistinctBy(x=> x.Id).Count(),
                    ColetadoValor = solicitaoItems.Where(x => x.ExameStatusId ==ExameStatusDto.Coletado).DistinctBy(x=> x.Id).Count(),
                    DigitadoValor = solicitaoItems.Where(x => x.ExameStatusId ==ExameStatusDto.Digitado).DistinctBy(x=> x.Id).Count(),
                    InterfaceadoValor = solicitaoItems.Where(x => x.ExameStatusId ==ExameStatusDto.Interfaceado).DistinctBy(x=> x.Id).Count(),
                    EnviadoEquipamentoValor = solicitaoItems.Where(x => x.ExameStatusId ==ExameStatusDto.EnviadoEquipamento).DistinctBy(x=> x.Id).Count(),
                    PendenteValor = solicitaoItems.Where(x => x.IsPendencia).DistinctBy(x=> x.Id).Count(),
                    ConferidoValor = solicitaoItems.Where(x=> x.ExameStatusId == ExameStatusDto.Conferido).DistinctBy(x=> x.Id).Count(),
                };

                return result;
            }
        }

        public async Task<PagedResultDto<ExamesDetalhamentoViewModel>> RetornaExamesPorSolicitacaoId(ExamesDetalhamentoInput dto)
        {
            const string defaultField = "SolicitacaoExameItem.Id";
            const string selectStatement = @"
                DISTINCT
                SolicitacaoExameItem.Id,
                FatItem.Codigo AS Codigo,
                FatItem.Descricao AS Exame,
                FatItem.DescricaoTuss AS ExameDescricao,
                FatItem.Mneumonico AS ExameMneumonico,
                SolicitacaoExame.DataSolicitacao AS DataSolicitacao,
                StatusSolicitacaoExameItem.Descricao AS Status,
                StatusSolicitacaoExameItem.CorStatus AS StatusCor,
                PessoaMedicoSolicitante.NomeCompleto AS MedicoSolicitante,
                MedicoSolicitante.NumeroConselho,
                Conselho.Codigo AS CodigoConselho,
                Material.Descricao AS DescricaoMaterial,
                Material.Codigo AS CodigoMaterial,
                CASE WHEN Exists (
	                SELECT 1 
	                FROM LabResultadoExame AS ResultadoExame 
	                WHERE ResultadoExame.SolicitacaoExameItemId = SolicitacaoExameItem.Id 
	                AND ResultadoExame.IsDeleted = @IsDeleted
                ) THEN 1 ELSE 0 END ExisteResultadoExame,
                SolicitacaoExameItem.MotivoPendencia,
                SolicitacaoExameItem.IsPendencia,
                CONVERT(varchar(10),SolicitacaoExameItem.PendenciaDateTime, 103) AS PendenciaDateTime,
                CONCAT(PendenciaUser.Name,' ', PendenciaUser.Surname) AS UsuarioPendencia";

            const string fromStatement = @"AssSolicitacaoExame SolicitacaoExame
                INNER JOIN AssSolicitacaoExameItem SolicitacaoExameItem ON SolicitacaoExame.Id = SolicitacaoExameItem.AssSolicitacaoExameId
                INNER JOIN FatItem ON FatItem.Id = SolicitacaoExameItem.FatItemId
                LEFT JOIN AssStatusSolicitacaoExameItem AS StatusSolicitacaoExameItem ON StatusSolicitacaoExameItem.Id = SolicitacaoExameItem.StatusSolicitacaoExameItemId
                LEFT JOIN SisMedico AS MedicoSolicitante ON   SolicitacaoExame.SisMedicoSolicitanteId = MedicoSolicitante.Id
                LEFT JOIN SisPessoa AS PessoaMedicoSolicitante ON MedicoSolicitante.SisPessoaId = PessoaMedicoSolicitante.Id
                LEFT JOIN SisConselho AS Conselho ON MedicoSolicitante.SisConselhoId = Conselho.Id
                LEFT JOIN LabMaterial AS Material ON Material.Id = SolicitacaoExameItem.LabMaterialId
                LEFT JOIN AbpUsers AS PendenciaUser ON PendenciaUser.Id = SolicitacaoExameItem.PendenciaUserId";

            const string whereStatement = "SolicitacaoExame.IsDeleted = @IsDeleted AND SolicitacaoExameItem.IsDeleted = @IsDeleted";
            
            return await this.CreateDataTable<ExamesDetalhamentoViewModel, ExamesDetalhamentoInput>()
                .AddDefaultField(defaultField)
                .AddSelectClause(selectStatement)
                .AddFromClause(fromStatement)
                .AddWhereClause(whereStatement)
                .AddWhereMethod((input, dapperParameters) =>
                {
                    dapperParameters.Add("IsDeleted", 0);
                    return "SolicitacaoExame.Id = @id";
                })
                .ExecuteAsync(dto);
        }
    }
}
