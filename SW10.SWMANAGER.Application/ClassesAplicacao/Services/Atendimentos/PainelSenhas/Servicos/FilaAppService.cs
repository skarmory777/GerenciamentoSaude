using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Servicos
{
    using Abp.Auditing;
    using Abp.Dependency;
    using SW10.SWMANAGER.Background;
    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha.HangfireJob;

    public class FilaAppService : SWMANAGERAppServiceBase, IFilaAppService
    {
        private readonly IRepository<Fila, long> _filaRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FilaAppService(IRepository<Fila, long> filaRepository
            , IUnitOfWorkManager unitOfWorkManager)
        {
            _filaRepository = filaRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<FilaIndex>> Listar(ListaFilaInput input)
        {
            try
            {
                var filasDto = await _filaRepository.GetAll().AsNoTracking()
                                          .Include(i => i.TipoLocalChamadaInicial)
                                                     .Where(w => (input.Filtro == "" || input.Filtro == null)
                                                                || w.Descricao.Contains(input.Filtro))
                                                      .Select(s => new FilaIndex
                                                      {
                                                          Id = s.Id,
                                                          Codigo = s.Codigo,
                                                          Descricao = s.Descricao,
                                                          TipoLocalChamadaInicial = s.TipoLocalChamadaInicial.Descricao
                                                      }).ToListAsync().ConfigureAwait(false);

                return new PagedResultDto<FilaIndex>(
                  filasDto.Count,
                  filasDto
                  );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FilaDto> Obter(long id)
        {
            try
            {
                var query = await _filaRepository
                    .GetAll()
                    .AsNoTracking()
                    .Where(m => m.Id == id)
                    .Include(i => i.TipoLocalChamadaInicial)
                    .Include(i => i.Empresa)
                    .FirstOrDefaultAsync().ConfigureAwait(false);

                var filaDto = FilaDto.Mapear(query);

                return filaDto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task Excluir(long id)
        {
            try
            {
                await _filaRepository.DeleteAsync(id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }


        public async Task<DefaultReturn<FilaDto>> CriarOuEditar(FilaDto input)
        {
            var _retornoPadrao = new DefaultReturn<FilaDto>
            {
                Warnings = new List<ErroDto>(),
                Errors = new List<ErroDto>()
            };

            try
            {
                using (var recurringJobManager = IocManager.Instance.ResolveAsDisposable<IRecurringJobManager>())
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    // ContaAdministrativaValidacaoService contaAdministrativaValidacaoService = new ContaAdministrativaValidacaoService();

                    // _retornoPadrao = contaAdministrativaValidacaoService.Validar(input);

                    if (_retornoPadrao.Errors.Count == 0)
                    {
                        if (input.Id == 0)
                        {
                            var fila = FilaDto.Mapear(input);

                            fila.UltimaZera = DateTime.Now;
                            _filaRepository.Insert(fila);
                            _retornoPadrao.ReturnObject = FilaDto.Mapear(fila);
                        }
                        else
                        {
                            var fila = _filaRepository.GetAll().FirstOrDefault(w => w.Id == input.Id);

                            if (fila != null)
                            {
                                fila.Codigo = input.Codigo;
                                fila.Descricao = input.Descricao;
                                fila.NumeroInicial = input.NumeroInicial;
                                fila.NumeroFinal = input.NumeroFinal;
                                fila.TipoLocalChamadaInicialId = input.TipoLocalChamadaInicialId;
                                fila.IsZera = input.IsZera;
                                if (input.HoraZera != null)
                                {
                                    fila.HoraZera = new DateTime(1900, 1, 1, ((DateTime)input.HoraZera).Hour, ((DateTime)input.HoraZera).Minute, 0);
                                }
                                else
                                {
                                    fila.HoraZera = null;
                                }
                                fila.Cor = input.Cor;
                                fila.IsNaoImprimeSenha = input.IsNaoImprimeSenha;

                                fila.IsAtivo = input.IsAtivo;
                                fila.IsDomingo = input.IsDomingo;
                                fila.IsSegunda = input.IsSegunda;
                                fila.IsTerca = input.IsTerca;
                                fila.IsQuarta = input.IsQuarta;
                                fila.IsQuinta = input.IsQuinta;
                                fila.IsSexta = input.IsSexta;
                                fila.IsSabado = input.IsSabado;

                                fila.EmpresaId = input.EmpresaId;
                                fila.QtdImpressaoSenha = input.QtdImpressaoSenha;

                                _filaRepository.Update(fila);
                                _retornoPadrao.ReturnObject = FilaDto.Mapear(fila);

                                if (!fila.IsZera)
                                {
                                    recurringJobManager.Object.RemoveIfExists(ZerarFilaJob.GeraZeraFilaJobId(fila.Id, GetCurrentTenant()?.Id));
                                }
                                else
                                {
                                    await recurringJobManager.Object.AddOrUpdateAsync<ZerarFilaJob, ZerarFilaJobArgs>(
                                        ZerarFilaJob.GeraZeraFilaJobId(fila.Id, GetCurrentTenant()?.Id),
                                        ZerarFilaJobArgs.GerarZerarFilaJobArgs(fila.Id, GetCurrentTenant()?.Id), 
                                        $"{fila.HoraZera.Value.Minute} {fila.HoraZera.Value.Hour} * * *");
                                }
                            }
                        }
                    }
                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    _retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }
            return _retornoPadrao;
        }
    }
}
