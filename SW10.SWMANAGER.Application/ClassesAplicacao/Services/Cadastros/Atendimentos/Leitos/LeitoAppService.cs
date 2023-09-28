using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Organizations;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.AtendimentosLeitosMov;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Pacientes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentosLeitosMov;
using SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos
{
    public class LeitoAppService : SWMANAGERAppServiceBase, ILeitoAppService
    {
        public async Task CriarOuEditar(CriarOuEditarLeito input)
        {
            try
            {
                var leito = CriarOuEditarLeito.Mapear(input);

                leito.DataAtualizacao = DateTime.Now;

                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                {
                    if (input.Id.Equals(0))
                    {
                        await leitoRepository.Object.InsertAsync(leito).ConfigureAwait(false);
                    }
                    else
                    {
                        var leitoEntity = leitoRepository.Object.GetAll().FirstOrDefault(w => w.Id == input.Id);

                        if (leitoEntity != null)
                        {
                            leitoEntity.Ativo = input.Ativo;
                            leitoEntity.Codigo = input.Codigo;
                            leitoEntity.DataAtualizacao = input.DataAtualizacao;
                            leitoEntity.Descricao = input.Descricao;
                            leitoEntity.Extra = input.Extra;
                            leitoEntity.HospitalDia = input.HospitalDia;
                            leitoEntity.LeitoAih = input.LeitoAih;
                            leitoEntity.LeitoStatusId = input.LeitoStatusId;
                            leitoEntity.Ramal = input.Ramal;
                            leitoEntity.Sexo = input.Sexo;
                            leitoEntity.TabelaItemSusId = input.TabelaItemSusId;
                            leitoEntity.TabelaItemTissId = input.TabelaItemTissId;
                            leitoEntity.TipoAcomodacaoId = input.TipoAcomodacaoId;
                            leitoEntity.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;


                            await leitoRepository.Object.UpdateAsync(leitoEntity).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(CriarOuEditarLeito input)
        {
            try
            {
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                {
                    await leitoRepository.Object.DeleteAsync(input.Id).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<PagedResultDto<LeitoDto>> Listar(ListarLeitosInput input)
        {
            var contarLeitos = 0;
            List<Leito> leitos;
            List<LeitoDto> leitosDtos = new List<LeitoDto>();
            long TipoAcoId = Convert.ToInt64(input.TipoAcomodacao);
            try
            {
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var userOrganizationUnitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UserOrganizationUnit, long>>())
                using (var unidadeOrganizacionalRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                using (var organizationUnitRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<OrganizationUnit, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimento, long>>())
                {
                    var unidades = from userOrg in userOrganizationUnitRepository.Object.GetAll().AsNoTracking()
                                   join org in unidadeOrganizacionalRepositorio.Object.GetAll().AsNoTracking() on userOrg.OrganizationUnitId
                                       equals org.Id
                                   join orgUnit in organizationUnitRepositorio.Object.GetAll().AsNoTracking() on org.OrganizationUnitId
                                       equals orgUnit.Id
                                   where userOrg.UserId == AbpSession.UserId
                                   select orgUnit;


                    var query = leitoRepository.Object.GetAll().AsNoTracking().Include(m => m.LeitoStatus).Include(m => m.TipoAcomodacao)
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.UnidadeOrganizacional.OrganizationUnit)
                        .Include(m => m.TabelaDominio)
                        .Where(
                            m => (!input.SomenteInternados && m.LeitoStatusId == 1)
                                 || (input.SomenteInternados && m.LeitoStatusId == 2)).Where(
                            w => unidades.Any(a => w.UnidadeOrganizacional.OrganizationUnit.Code.StartsWith(a.Code))
                                 && (input.UnidadeId == null || w.UnidadeOrganizacionalId == input.UnidadeId)).WhereIf(
                            input.TipoAcomodacao != null,
                            m => m.TipoAcomodacaoId == TipoAcoId);

                    contarLeitos = await query.CountAsync().ConfigureAwait(false);

                    leitos = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync()
                                 .ConfigureAwait(false);

                    leitosDtos = LeitoDto.Mapear(leitos);

                    if (input.SomenteInternados)
                    {
                        var leitosIds = leitosDtos.Select(s => s.Id).ToList();

                        var atendimentos = atendimentoRepository.Object.GetAll().AsNoTracking().Include(i => i.Paciente.SisPessoa).Where(w => w.DataAlta == null && leitosIds.Any(a => a == w.LeitoId)).ToList();
                        foreach (var item in leitosDtos)
                        {
                            var atendimentoLeito = atendimentos.Where(w => w.LeitoId == item.Id)
                                .Select(s => new { s.Paciente.SisPessoa.NomeCompleto, s.Id }).FirstOrDefault();

                            item.Paciente = atendimentoLeito.NomeCompleto;
                            item.AtendimentoId = atendimentoLeito.Id;
                        }
                    }

                    return new PagedResultDto<LeitoDto>(contarLeitos, leitosDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<List<Leito>> ListarParaRelatorioMapaLeitos(long empresaId, long? statusLeito = 0)
        {
            List<Leito> leitos;
            // List<LeitoDto> leitosDtos = new List<LeitoDto>();

            try
            {
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var userUnidadeOrganizacional = IocManager.Instance.ResolveAsDisposable<IRepository<UserOrganizationUnit, long>>())
                {
                    var unidadeOrganizacionalId = userUnidadeOrganizacional.Object.GetAll().AsNoTracking()
                        .Where(m => m.UserId == this.AbpSession.UserId).Select(s => s.OrganizationUnitId);

                    var query = leitoRepository.Object.GetAll().AsNoTracking().Include(m => m.LeitoStatus).Include(m => m.TipoAcomodacao)
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.UnidadeOrganizacional.OrganizationUnit)
                        .Where(m => unidadeOrganizacionalId.Any(a => a == m.UnidadeOrganizacional.OrganizationUnitId))
                        .WhereIf(statusLeito == 1, m => m.LeitoStatusId == 1).WhereIf(
                            statusLeito == 2,
                            m => m.LeitoStatusId == 2);

                    leitos = await query.OrderBy("Descricao")
                                 //.PageBy(input)
                                 .ToListAsync().ConfigureAwait(false);

                    return leitos;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<ListResultDto<LeitoComAtendimentoDto>> ListarTodos()
        {
            try
            {
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var atendimentoService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    var leitosComAtendimento = new List<LeitoComAtendimentoDto>();
                    var leitos = await leitoRepository.Object.GetAll().Include(m => m.LeitoStatus)
                                     .Include(m => m.TipoAcomodacao).Include(m => m.UnidadeOrganizacional)
                                     .Include(m => m.TabelaDominio).AsNoTracking().ToListAsync().ConfigureAwait(false);

                    var leitosDtos = LeitoDto.Mapear(leitos);


                    foreach (var leito in leitos)
                    {
                        var leitoComAtendimento = new LeitoComAtendimentoDto { Leito = LeitoDto.Mapear(leito), Id = leito.Id };

                        var atendimento = atendimentoService.Object.ObterPorLeito(leito.Id);

                        leitoComAtendimento.AtendimentoId = atendimento != null ? atendimento.Id : 0;
                        leitoComAtendimento.AtendimentoAtual = atendimento;
                        leitosComAtendimento.Add(leitoComAtendimento);
                    }

                    return new ListResultDto<LeitoComAtendimentoDto> { Items = leitosComAtendimento };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<ListResultDto<LeitoComAtendimentoDto>> ListarPorUnidade(ListarLeitosInput input)
        {
            try
            {
                using (var _leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var atendimentoService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var atendimentoLeitoMovService = IocManager.Instance.ResolveAsDisposable<IAtendimentoLeitoMovAppService>())
                {
                    long uoId = Convert.ToInt64(input.UO);
                    var leitosComAtendimento = new List<LeitoComAtendimentoDto>();
                    var leitos = await _leitoRepository.Object.GetAll().AsNoTracking().Include(m => m.LeitoStatus)
                                     .Include(m => m.TipoAcomodacao).Include(m => m.UnidadeOrganizacional)
                                     .Include(m => m.UnidadeOrganizacional.OrganizationUnit)
                                     .Include(m => m.TabelaDominio)
                                     .WhereIf(
                                         input.TipoAtendimento == "inter",
                                         m => m.UnidadeOrganizacional.IsInternacao == true)
                                     .WhereIf(
                                         input.TipoAtendimento == "ambu",
                                         m => m.UnidadeOrganizacional.IsAmbulatorioEmergencia == true).WhereIf(
                                         input.UO != null,
                                         m => m.UnidadeOrganizacional.OrganizationUnit.Id == uoId
                                              || m.UnidadeOrganizacional.Id == uoId).AsNoTracking()
                                     .OrderBy(input.Sorting).ToListAsync().ConfigureAwait(false);

                    var leitosDtos = new List<LeitoDto>();

                    foreach (var item in leitos)
                    {
                        var leito = LeitoDto.MapearFromCore(item);

                        var leitoComAtendimento = new LeitoComAtendimentoDto { Id = leito.Id, Leito = leito };

                        leitosComAtendimento.Add(leitoComAtendimento);
                    }

                    return new ListResultDto<LeitoComAtendimentoDto> { Items = leitosComAtendimento };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<PagedResultDto<LeitoComAtendimentoDto>> ListarPorUnidadePaginado(ListarLeitosInput input)
        {
            try
            {
                using (var userOrganizationUnitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UserOrganizationUnit, long>>())
                using (var organizationUnitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<OrganizationUnit, long>>())
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimento, long>>())
                using (var leitoStatusRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoStatus, long>>())
                using (var unidadeOrganizacioalRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                using (var pacienteRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Paciente, long>>())
                using (var sisPessoaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SisPessoa, long>>())
                using (var atdLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                {
                    var leitosComAtendimento = new List<LeitoComAtendimentoDto>();

                    //  var listaUnidades = _userOrganizationUnitRepository.GetAll().Where(w => w.UserId == AbpSession.UserId);

                    var listaUnidades =
                        from userUnit in userOrganizationUnitRepository.Object.GetAll().AsNoTracking()
                            .Where(w => w.UserId == AbpSession.UserId)
                        join organization in organizationUnitRepository.Object.GetAll().AsNoTracking() on userUnit.OrganizationUnitId equals
                            organization.Id
                        select organization;

                    var query = from l in leitoRepository.Object.GetAll().AsNoTracking()
                                join a in atendimentoRepository.Object.GetAll().AsNoTracking()
                                        .WhereIf(input.EmpresaId.HasValue, e => e.EmpresaId == input.EmpresaId.Value)
                                        .Where(w => !w.DataAlta.HasValue && !w.IsPreatendimento) on l.Id equals a
                                        .LeitoId
                                    into leitoAtendimento
                                from leitosAtendimento in leitoAtendimento.DefaultIfEmpty()
                                join status in leitoStatusRepository.Object.GetAll().AsNoTracking() on l.LeitoStatusId equals status.Id

                                //join UO in _UnidadeOrganizacioalRepository.GetAll().Where(w => listaUnidades.Any(a => a.OrganizationUnitId == w.Id))
                                join UO in
                                    unidadeOrganizacioalRepository.Object.GetAll().AsNoTracking().Where(
                                        w => listaUnidades.Any(a => w.OrganizationUnit.Code.StartsWith(a.Code))) on
                                    l.UnidadeOrganizacionalId equals UO.Id
                                join paciente in pacienteRepository.Object.GetAll().AsNoTracking() on leitosAtendimento.PacienteId equals
                                    paciente.Id into pacienteAtendimento
                                from pacientesAtendimentos in pacienteAtendimento.DefaultIfEmpty()
                                join sisPessoa in sisPessoaRepository.Object.GetAll().AsNoTracking() on pacientesAtendimentos.SisPessoaId
                                    equals sisPessoa.Id
                                where l.UnidadeOrganizacional.IsControlaLeito
                                      && (string.IsNullOrEmpty(input.UO)
                                          || l.UnidadeOrganizacional.Id.ToString() == input.UO)
                                      && (string.IsNullOrEmpty(input.Filtro) || l.Codigo.Contains(input.Filtro)
                                                                             || l.Descricao.Contains(input.Filtro)
                                                                             || l.UnidadeOrganizacional.Localizacao
                                                                                 .Contains(input.Filtro)
                                                                             || l.LeitoStatus.Descricao.Contains(
                                                                                 input.Filtro)
                                                                             || pacientesAtendimentos.NomeCompleto
                                                                                 .Contains(input.Filtro))
                                select new
                                {
                                    leito = l,
                                    status = status,
                                    unidade = UO,
                                    atendimento = leitosAtendimento,
                                    pacienteAtendimento = pacientesAtendimentos,
                                    pessoa = sisPessoa,
                                    id = l.Id
                                };



                    //.Include(m => m.LeitoStatus)
                    //.Include(m => m.TipoAcomodacao)
                    //.Include(m => m.UnidadeOrganizacional)
                    //.Include(m => m.UnidadeOrganizacional.OrganizationUnit)
                    //.Include(m => m.TabelaDominio)
                    //   .WhereIf(input.TipoAtendimento == "inter", m => m.UnidadeOrganizacional.IsInternacao == true)
                    //   .WhereIf(input.TipoAtendimento == "ambu", m => m.UnidadeOrganizacional.IsAmbulatorioEmergencia == true)
                    //   .WhereIf(!string.IsNullOrEmpty(input.UO), m => m.UnidadeOrganizacional.OrganizationUnit.Id.ToString() == input.UO || m.UnidadeOrganizacional.Id.ToString() == input.UO);


                    //var q = from l in query
                    //        join a in _atendimentoRepository.GetAll()
                    //        on l.Id equals a.LeitoId
                    //         into leitoAtendimento
                    //        from leitosAtendimento in leitoAtendimento.DefaultIfEmpty()


                    //        ;




                    var atend = atendimentoRepository.Object.GetAll().AsNoTracking().WhereIf(
                        input.EmpresaId.HasValue,
                        m => m.EmpresaId == input.EmpresaId.Value);

                    var queryLeitos = from l in leitoRepository.Object.GetAll().AsNoTracking()
                                      join status in leitoStatusRepository.Object.GetAll().AsNoTracking() on l.LeitoStatusId equals status.Id

                                      //join UO in _UnidadeOrganizacioalRepository.GetAll().Where(w => listaUnidades.Any(a => a.OrganizationUnitId == w.Id))
                                      join UO in
                                          unidadeOrganizacioalRepository.Object.GetAll().AsNoTracking().Where(
                                              w => listaUnidades.Any(a => w.OrganizationUnit.Code.StartsWith(a.Code)))
                                          on l.UnidadeOrganizacionalId equals UO.Id
                                      where !(from a in atend
                                              where a.IsInternacao && a.DataAlta == null && a.LeitoId == l.Id
                                              select a.Id).Any() && UO.IsControlaLeito
                                                                       && (string.IsNullOrEmpty(input.UO)
                                                                           || l.UnidadeOrganizacionalId.ToString()
                                                                           == input.UO)
                                                                       && (string.IsNullOrEmpty(input.Filtro)
                                                                           || l.Codigo.Contains(input.Filtro)
                                                                           || l.Descricao.Contains(input.Filtro)
                                                                           || l.UnidadeOrganizacional.Localizacao
                                                                               .Contains(input.Filtro)
                                                                           || l.LeitoStatus.Descricao.Contains(
                                                                               input.Filtro))
                                      select new { leito = l, status = status, unidade = UO, };

                    var leitosNaoOcupados = queryLeitos.AsNoTracking().OrderBy("Leito.Descricao");
                    // .PageBy(input)
                    //.ToList();




                    var leitos = await query.AsNoTracking().OrderBy("Leito.Id")
                                     // .PageBy(input)
                                     .ToListAsync().ConfigureAwait(false);



                    var leitosDtos = new List<LeitoDto>();

                    if (input.Todos || input.SomenteInternados)
                    {
                        foreach (var leitoMov in leitos.Where(w => w.status.Id == 2))
                        {
                            var leitoComAtendimento = new LeitoComAtendimentoDto();
                            leitoComAtendimento.AtendimentoLeitoMov = new AtendimentoLeitoMovDto();


                            var atdLeitosMovs = atdLeitoMovRepository.Object.GetAll().AsNoTracking().Where(x => x.LeitoId == leitoMov.id);

                            if (atdLeitosMovs != null && atdLeitosMovs.Any())
                            {
                                var atdLeitoMovMaisRecente = atdLeitosMovs.OrderBy(d => d.DataInicial).FirstOrDefault();
                            }

                            leitoComAtendimento.Leito = LeitoDto.MapearFromCore(leitoMov.leito);
                            leitoComAtendimento.Leito.LeitoStatus = new LeitoStatusDto
                            {
                                Id = leitoMov.status.Id,
                                Descricao = leitoMov.status.Descricao,
                                Cor = leitoMov.status.Cor
                            };
                            leitoComAtendimento.Leito.UnidadeOrganizacional =
                                UnidadeOrganizacionalDto.MapearFromCore(leitoMov.unidade);


                            leitoComAtendimento.AtendimentoLeitoMov.Leito = LeitoDto.MapearFromCore(leitoMov.leito);
                            leitoComAtendimento.AtendimentoLeitoMov.Leito.LeitoStatus =
                                new LeitoStatusDto { Id = leitoMov.status.Id };
                            leitoComAtendimento.AtendimentoLeitoMov.LeitoId = leitoMov?.leito?.Id;


                            if (leitoMov.atendimento != null && leitoMov.atendimento.Id > 0)
                            {
                                leitoComAtendimento.AtendimentoId = leitoMov?.atendimento?.Id;
                                leitoComAtendimento.AtendimentoAtual =
                                    SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
                                        .AtendimentoDto.MapearFromCore(leitoMov?.atendimento);
                                leitoComAtendimento.AtendimentoAtual.Paciente =
                                    new Pacientes.Dto.PacienteDto
                                    {
                                        SisPessoa = new Pessoas.Dto.SisPessoaDto
                                        {
                                            NomeCompleto = leitoMov?.pessoa.NomeCompleto
                                        }
                                    };

                                leitoComAtendimento.AtendimentoLeitoMov.Atendimento =
                                    SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
                                        .AtendimentoDto.MapearFromCore(leitoMov?.atendimento);
                                leitoComAtendimento.AtendimentoLeitoMov.AtendimentoId = leitoMov?.atendimento?.Id;

                                if (leitoMov.atendimento != null && leitoMov.atendimento.DataRegistro != null)
                                {
                                    leitoComAtendimento.AtendimentoAtual.DataRegistro =
                                        (DateTime)leitoMov.atendimento.DataRegistro;
                                }
                            }

                            leitosComAtendimento.Add(leitoComAtendimento);
                        }
                    }

                    if ((!input.EmpresaId.HasValue && (!input.SomenteInternados || input.Todos)))
                    {
                        foreach (var leito in leitosNaoOcupados)
                        {
                            var leitoComAtendimento = new LeitoComAtendimentoDto
                            {
                                AtendimentoLeitoMov = new AtendimentoLeitoMovDto(),
                                Leito = LeitoDto.MapearFromCore(leito.leito)
                            };
                            leitoComAtendimento.Leito.LeitoStatus = new LeitoStatusDto
                            {
                                Id = leito.status.Id,
                                Descricao = leito.status.Descricao,
                                Cor = leito.status.Cor
                            };
                            leitoComAtendimento.Leito.UnidadeOrganizacional =
                                UnidadeOrganizacionalDto.MapearFromCore(leito.unidade);

                            leitoComAtendimento.AtendimentoLeitoMov.Leito = LeitoDto.MapearFromCore(leito.leito);
                            leitoComAtendimento.AtendimentoLeitoMov.Leito.LeitoStatus =
                                new LeitoStatusDto { Id = leito.status.Id };
                            leitoComAtendimento.AtendimentoLeitoMov.LeitoId = leito?.leito?.Id;

                            leitosComAtendimento.Add(leitoComAtendimento);
                        }
                    }

                    return new PagedResultDto<LeitoComAtendimentoDto>(
                        query.Count() + (input.EmpresaId.HasValue || input.SomenteInternados ? 0 : queryLeitos.Count()),
                        leitosComAtendimento.OrderBy(o => o.Leito.Descricao).ToList());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<ICollection<LeitoDto>> ListarPorUnidadeParaDrop(long? id)
        {

            try
            {
                using (var unidadeOrganizacioalRepository = IocManager.Instance.ResolveAsDisposable<IRepository<UnidadeOrganizacional, long>>())
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                {
                    IQueryable<Leito> query;

                    var unidadeOrganizacional = unidadeOrganizacioalRepository.Object.GetAll().AsNoTracking()
                        .Include(i => i.OrganizationUnit).FirstOrDefault(w => w.Id == id);

                    if (unidadeOrganizacional != null)
                    {
                        query = from m in leitoRepository.Object.GetAll().AsNoTracking()
                                where m.UnidadeOrganizacional.OrganizationUnit.Code.StartsWith(
                                          unidadeOrganizacional.OrganizationUnit.Code) && m.LeitoStatusId == 1
                                select m;
                    }
                    else
                    {
                        query = from m in leitoRepository.Object.GetAll().AsNoTracking() where m.LeitoStatusId == 1 select m;
                    }

                    var leitos = await query.Include(m => m.LeitoStatus).Include(m => m.TipoAcomodacao)
                                     .Include(m => m.UnidadeOrganizacional).Include(m => m.TabelaDominio)
                                     .Where(m => m.LeitoStatusId == 1).AsNoTracking().ToListAsync()
                                     .ConfigureAwait(false);

                    var leitosDtos = LeitoDto.Mapear(leitos);


                    return leitosDtos;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<ListResultDto<LeitoDto>> ListarPorUnidadeParaDrop2(long id)
        {
            try
            {
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                {
                    var query = from m in leitoRepository.Object.GetAll().AsNoTracking() where m.UnidadeOrganizacionalId == id select m;

                    var leitos = await query.Include(m => m.LeitoStatus).Include(m => m.TipoAcomodacao)
                                     .Include(m => m.UnidadeOrganizacional).Include(m => m.TabelaDominio)
                                     .Where(m => m.LeitoStatusId == 1).AsNoTracking().ToListAsync()
                                     .ConfigureAwait(false);


                    var leitosDtos = LeitoDto.Mapear(leitos);

                    return new ListResultDto<LeitoDto> { Items = leitosDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<List<Leito>> ListarAutoComplete(string term)
        {
            try
            {
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                {
                    return await leitoRepository.Object.GetAll().Include(m => m.LeitoStatus).Include(m => m.TipoAcomodacao)
                               .Include(m => m.UnidadeOrganizacional).Include(m => m.TabelaDominio).WhereIf(
                                   !term.IsNullOrEmpty(),
                                   m => m.Descricao.Contains(term))
                               //.AsNoTracking()
                               .ToListAsync().ConfigureAwait(false);
                }


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<LeitoDto> Obter(long id)
        {
            try
            {
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                {
                    var query = await leitoRepository.Object.GetAll().Include(m => m.LeitoStatus)
                                    .Include(m => m.TipoAcomodacao).Include(m => m.UnidadeOrganizacional)
                                    .Include(m => m.TabelaDominio).Where(m => m.Id == id).FirstOrDefaultAsync()
                                    .ConfigureAwait(false);

                    var leito = LeitoDto.Mapear(query);


                    return leito;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public void OcuparLeito(long? leitoId)
        {
            try
            {
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var leitoStatusRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoStatus, long>>())
                {
                    var leito = leitoRepository.Object.Get(leitoId.Value);
                    leito.DataAtualizacao = DateTime.Now;
                    leito.LeitoStatus = leitoStatusRepository.Object.FirstOrDefault(l => l.Descricao == "Ocupado");
                    leitoRepository.Object.UpdateAsync(leito);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public void DesocuparLeito(long? leitoId)
        {
            try
            {
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var leitoStatusRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LeitoStatus, long>>())
                {
                    var leito = leitoRepository.Object.Get(leitoId.Value);
                    leito.DataAtualizacao = DateTime.Now;
                    leito.LeitoStatus = leitoStatusRepository.Object.FirstOrDefault(l => l.Descricao == "Vago");
                    leitoRepository.Object.UpdateAsync(leito);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro

                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                {

                    var query = from p in leitoRepository.Object.GetAll().AsNoTracking().WhereIf(
                                    !dropdownInput.search.IsNullOrEmpty(),
                                    m => m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                                         || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                         || m.DescricaoResumida.ToLower().Contains(dropdownInput.search.ToLower()))
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };
                    //paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    return new ResultDropdownList()
                    {
                        Items = await queryResultPage.ToListAsync().ConfigureAwait(false),
                        TotalCount = await query.CountAsync().ConfigureAwait(false)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        [DisableAuditing]
        public async Task<ResultDropdownList> ListarLeitoVagoDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                {
                    var query = from p in leitoRepository.Object.GetAll().AsNoTracking().WhereIf(
                                        !dropdownInput.search.IsNullOrEmpty(),
                                        m => m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                                             || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                             || m.DescricaoResumida.ToLower().Contains(dropdownInput.search.ToLower()))
                                    .Where(
                                        w => w.LeitoStatusId == 1 && w.UnidadeOrganizacional.IsInternacao) //leito Vago
                                orderby p.Descricao ascending
                                select new DropdownItems
                                {
                                    id = p.Id,
                                    text = string.Concat(p.Codigo, " - ", p.Descricao)
                                };
                    //paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    return new ResultDropdownList()
                    {
                        Items = await queryResultPage.ToListAsync().ConfigureAwait(false),
                        TotalCount = await query.CountAsync().ConfigureAwait(false)
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        //public async Task<ResultDropdownList> ListarLeitoVagoDropdown(DropdownInput dropdownInput)
        //{
        //    return base.ListarDescricaoDropdown(dropdownInput, _leitoRepository)

        //}

        public async Task AlterarStausLeito(long leitoId, long statusId)
        {
            using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
            {
                var leito = leitoRepository.Object.GetAll().FirstOrDefault(w => w.Id == leitoId);

                if (leito != null)
                {
                    leito.LeitoStatusId = statusId;
                }
            }
        }
    }
}
