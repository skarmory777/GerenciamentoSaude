using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Vistantes.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Visitantes;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Visitantes
{
    using Abp.Auditing;
    using Abp.Dependency;

    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Visitantes.Dto;

    public class VisitanteAppService : SWMANAGERAppServiceBase, IVisitanteAppService
    {
        #region Dependencias

        private readonly IIocManager _iocManager;

        public VisitanteAppService(IIocManager iocManager)
        {
            this._iocManager = iocManager;
        }

        #endregion dependencias.

        [UnitOfWork]
        public async Task CriarOuEditar(VisitanteDto input)
        {
            try
            {
                using (var visitanteRepository = this._iocManager.ResolveAsDisposable<IRepository<Visitante, long>>())
                using (var atendimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var unitOfWorkManager = this._iocManager.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    //var visitante = input.MapTo<Visitante>();
                    var visitante = VisitanteDto.MapearParaCore(input);

                    visitante.AtendimentoId = input.AteId;

                    if (visitante.AtendimentoId.HasValue && input.AteId != 0)
                    {
                        visitante.AtendimentoId = input.AteId;
                    }
                    else
                    {
                        visitante.AtendimentoId = input.AtendimentoId;
                    }

                    if (visitante.AtendimentoId.HasValue && visitante.AtendimentoId != 0)
                    {
                        var atendimentodto = new AtendimentoDto();
                        var atendimento = await atendimentoRepository.Object.GetAsync((long)visitante.AtendimentoId)
                                              .ConfigureAwait(false);
                        atendimentodto = AtendimentoDto.MapearFromCore(atendimento);
                        visitante.UnidadeOrganizacionalId = atendimento.UnidadeOrganizacionalId;
                        visitante.LeitoId = atendimento.LeitoId;
                    }

                    if (visitante.AtendimentoId == 0)
                    {
                        visitante.AtendimentoId = null;
                    }

                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        if (input.Id.Equals(0))
                        {
                            await visitanteRepository.Object.InsertAsync(visitante).ConfigureAwait(false);
                        }
                        else
                        {
                            visitante.Atendimento = null;
                            visitante.Leito = null;
                            visitante.UnidadeOrganizacional = null;

                            await visitanteRepository.Object.UpdateAsync(visitante).ConfigureAwait(false);
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
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
                using (var visitanteRepository = this._iocManager.ResolveAsDisposable<IRepository<Visitante, long>>())
                using (var unitOfWorkManager = this._iocManager.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        await visitanteRepository.Object.DeleteAsync(id).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<VisitanteDto>> ListarFiltro(ListarVisitantesInput input)
        {
            try
            {
                using (var visitanteRepository = this._iocManager.ResolveAsDisposable<IRepository<Visitante, long>>())
                {
                    var data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    //data = moment.data
                    if (input.Nome == string.Empty)
                    {
                        input.Nome = null;
                    }

                    if (input.Paciente == string.Empty)
                    {
                        input.Paciente = null;
                    }

                    if (input.Documento == string.Empty)
                    {
                        input.Documento = null;
                    }

                    if (input.StartDate == null)
                    {
                        input.StartDate = data;
                    }

                    data = data.AddDays(1);
                    if (input.EndDate == null)
                    {
                        input.EndDate = data;
                    }

                    var query = visitanteRepository.Object.GetAll().AsNoTracking().Include(m => m.Atendimento)
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.Leito).Include(m => m.Atendimento.Leito)
                        .Include(m => m.Atendimento.Paciente).Include(m => m.Atendimento.Paciente.SisPessoa)
                        .Where(m => m.DataEntrada >= input.StartDate && m.DataEntrada <= input.EndDate)
                        .WhereIf(!input.Nome.IsNullOrWhiteSpace(), m => m.Nome.Contains(input.Nome))
                        .WhereIf(
                            !input.Paciente.IsNullOrWhiteSpace(),
                            m => m.Atendimento.Paciente.NomeCompleto.Contains(input.Paciente))
                        .WhereIf(input.Fornecedor > 0, m => m.FornecedorId == input.Fornecedor).WhereIf(
                            !input.Documento.IsNullOrWhiteSpace(),
                            m => m.Documento.Contains(input.Documento));

                    var contarVisitantes = await query.CountAsync().ConfigureAwait(false);

                    var visitantesDtos = await query.OrderBy(input.Sorting).PageBy(input)
                                             .Select(v => VisitanteDto.MapearFormCore(v)).ToListAsync()
                                             .ConfigureAwait(false);

                    return new PagedResultDto<VisitanteDto>(contarVisitantes, visitantesDtos);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<VisitanteIndexOut>> IndexVisitanteFiltro(ListarVisitantesInput input)
        {
            try
            {
                using (var visitanteRepository = this._iocManager.ResolveAsDisposable<IRepository<Visitante, long>>())
                {
                    var data = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);

                    //data = moment.data
                    if (input.Nome == string.Empty)
                    {
                        input.Nome = null;
                    }

                    if (input.Paciente == string.Empty)
                    {
                        input.Paciente = null;
                    }

                    if (input.Documento == string.Empty)
                    {
                        input.Documento = null;
                    }

                    if (input.StartDate == null)
                    {
                        input.StartDate = data;
                    }

                    data = data.AddDays(1);
                    if (input.EndDate == null)
                    {
                        input.EndDate = data;
                    }

                    var query = visitanteRepository.Object.GetAll().AsNoTracking().Include(m => m.Atendimento)
                        .Include(m => m.UnidadeOrganizacional).Include(m => m.Leito).Include(m => m.Atendimento.Leito)
                        .Include(m => m.Atendimento.Paciente).Include(m => m.Atendimento.Paciente.SisPessoa)
                        .Where(m => m.DataEntrada >= input.StartDate && m.DataEntrada <= input.EndDate)
                        .WhereIf(!input.Nome.IsNullOrWhiteSpace(), m => m.Nome.Contains(input.Nome))
                        .WhereIf(
                            input.PacienteId.HasValue && input.PacienteId.Value > 0,
                            m => m.Atendimento.PacienteId == input.PacienteId)
                        .WhereIf(
                            !input.Paciente.IsNullOrWhiteSpace(),
                            m => m.Atendimento.Paciente.NomeCompleto.Contains(input.Paciente))
                        .WhereIf(input.Fornecedor > 0, m => m.FornecedorId == input.Fornecedor)
                        .WhereIf(!input.Documento.IsNullOrWhiteSpace(), m => m.Documento.Contains(input.Documento))
                        .Select(
                            v => new VisitanteIndexOut
                            {
                                Id = v.Id,
                                LeitoId = v.LeitoId,
                                UnidadeOrganizacionalId = v.UnidadeOrganizacionalId,
                                AtendimentoId = v.AtendimentoId ?? 0,
                                FornecedorId = v.FornecedorId,
                                Codigo = v.Codigo,
                                Descricao = v.Descricao,
                                Nome = v.Nome,
                                NomePaciente =
                                             v.Atendimento != null && v.Atendimento.Paciente != null
                                                 ? v.Atendimento.Paciente.NomeCompleto
                                                 : null,
                                AtendimentoDataAlta = v.Atendimento != null ? v.Atendimento.DataAlta : null,
                                AtendimentoDataRegistro =
                                             v.Atendimento != null ? (DateTime?)v.Atendimento.DataRegistro : null,
                                LeitoDescricao = v.Leito != null ? v.Leito.Descricao : null,
                                UnidadeOrganizacionalDescricao =
                                             v.UnidadeOrganizacional != null ? v.UnidadeOrganizacional.Descricao : null,
                                Documento = v.Documento,
                                DataEntrada = v.DataEntrada,
                                DataSaida = v.DataSaida,
                                IsAcompanhante = v.IsAcompanhante,
                                IsEmergencia = v.IsEmergencia,
                                IsInternado = v.IsInternado,
                                IsMedico = v.IsMedico,
                                IsSetor = v.IsSetor,
                                IsFornecedor = v.IsFornecedor,
                                IsVisitante = v.IsVisitante,
                                IsSistema = v.IsSistema,
                                CreationTime = v.CreationTime,
                                CreatorUserId = v.CreatorUserId,
                                LastModificationTime = v.LastModificationTime,
                                LastModifierUserId = v.LastModifierUserId,
                                DeletionTime = v.DeletionTime,
                                IsDeleted = v.IsDeleted,
                                DeleterUserId = v.DeleterUserId,
                            });

                    var contarVisitantes = await query.CountAsync().ConfigureAwait(false);

                    var visitantes =
                        await query.OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    return new PagedResultDto<VisitanteIndexOut>(contarVisitantes, visitantes);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<VisitanteDto> Obter(long id)
        {
            try
            {
                using (var visitanteRepository = this._iocManager.ResolveAsDisposable<IRepository<Visitante, long>>())
                {

                    var result = await visitanteRepository.Object.GetAll().AsNoTracking().Include(m => m.Atendimento)
                                     .Include(m => m.Fornecedor).Include(m => m.UnidadeOrganizacional)
                                     .Include(m => m.Atendimento.UnidadeOrganizacional)
                                     .Include(m => m.Atendimento.Leito).Include(m => m.Atendimento.Paciente)
                                     .Include(m => m.Atendimento.Paciente.SisPessoa)
                                     //.Include(m => m.AtendimentoId)
                                     .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);


                    // TROCAR PARA MAPEAMENTO MANUAL. ESTA MUITO LENTO

                    var visitante = new VisitanteDto();

                    if (result != null)
                    {
                        visitante = VisitanteDto.MapearFormCore(result);
                    }

                    return visitante;
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public VisitanteDto ObterVisitantePorAtendimentoId(long atendimentoId)
        {
            try
            {
                using (var _visitanteRepository = this._iocManager.ResolveAsDisposable<IRepository<Visitante, long>>())
                {
                    var result = _visitanteRepository.Object.GetAll().AsNoTracking().Include(v => v.Leito).FirstOrDefault(v => v.AtendimentoId == 161312);

                    // TROCAR PARA MAPEAMENTO MANUAL. ESTA MUITO LENTO
                    var visitante = new VisitanteDto();

                    if (result != null)
                    {
                        visitante = VisitanteDto.MapearFormCore(result);
                    }

                    return visitante;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdownModalVisitantePaciente(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            // List<AtendimentoDto> atendimentosDto = new List<AtendimentoDto>();
            try
            {
                using (var atendimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    //get com filtro
                    var query = atendimentoRepository.Object.GetAll().AsNoTracking().Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa).Where(m => m.DataAlta == null)
                        .Where(m => m.IsInternacao == true)
                        .WhereIf(
                            !dropdownInput.search.IsNullOrEmpty(),
                            m => m.Id.ToString().Contains(dropdownInput.search)
                                 || m.Paciente.NomeCompleto.Contains(dropdownInput.search)).OrderBy(p => p.Codigo)
                        .Select(
                            p => new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Id, " - ", p.Paciente.NomeCompleto)
                            });
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdownModalVisitantePaciente2(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            // List<AtendimentoDto> atendimentosDto = new List<AtendimentoDto>();
            try
            {
                using (var atendimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    //get com filtro
                    var query = atendimentoRepository.Object.GetAll().AsNoTracking().Include(m => m.Paciente)
                        .Include(m => m.Paciente.SisPessoa).Where(m => m.DataAlta == null)
                        .Where(m => m.IsAmbulatorioEmergencia == true).WhereIf(
                            !dropdownInput.search.IsNullOrEmpty(),
                            m => m.Id.ToString().Contains(dropdownInput.search)
                                 || m.Paciente.NomeCompleto.Contains(dropdownInput.search)).OrderBy(p => p.Codigo)
                        .Select(
                            p => new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Paciente.Id, " - ", p.Paciente.NomeCompleto)
                            });
                    //paginação 
                    var queryResultPage = query.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                    var total = await query.CountAsync().ConfigureAwait(false);

                    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
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
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);


            long atendimentoId;

            long.TryParse(dropdownInput.filtro, out atendimentoId);
            using (var visitanteRepository = this._iocManager.ResolveAsDisposable<IRepository<Visitante, long>>())
            using (var atendimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {

                //return await ListarDropdownLambda(dropdownInput
                //                                 , _visitanteRepository
                //                                , w => w.AtendimentoId == atendimentoId
                //                                , p => new DropdownItems { id = p.Id, text = p.Nome }
                //                                , o => o.Nome);

                var pacienteId = atendimentoRepository.Object.GetAll().AsNoTracking().Where(w => w.Id == atendimentoId)
                    .Select(s => s.PacienteId).FirstOrDefault();

                var atendimentos = atendimentoRepository.Object.GetAll().AsNoTracking()
                    .Where(w => w.PacienteId == pacienteId);


                var visitantasRepetidas = visitanteRepository.Object.GetAll().AsNoTracking().Where(
                            w => atendimentos.Any(a => a.Id == w.AtendimentoId)

                                 //w.AtendimentoId == atendimentoId 


                                 && (dropdownInput.search == string.Empty || dropdownInput.search == null
                                                                          || w.Documento.Contains(dropdownInput.search)
                                                                          || w.Nome.Contains(dropdownInput.search)))
                        .GroupBy(g => g.Documento)
                        .Select(s => s.Max(m => m.Id)) //; new { s.First().Nome, s.First().Documento }; } )
                                                       //.ToList()
                    ;


                var visitas = visitanteRepository.Object.GetAll().AsNoTracking()
                    .Where(w => visitantasRepetidas.Any(a => a == w.Id)).OrderBy(o => o.Nome).Select(
                        p => new DropdownItems { id = p.Id, text = string.Concat(p.Documento, " - ", p.Nome) });

                var queryResultPage = visitas.Skip(numberOfObjectsPerPage * pageInt).Take(numberOfObjectsPerPage);

                return new ResultDropdownList()
                {
                    Items = await visitas.ToListAsync().ConfigureAwait(false),
                    TotalCount = await visitas.CountAsync().ConfigureAwait(false)
                };
            }
        }
    }
}