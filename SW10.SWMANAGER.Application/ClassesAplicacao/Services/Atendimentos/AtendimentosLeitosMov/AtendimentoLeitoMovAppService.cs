using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Castle.Core.Internal;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.AtendimentosLeitosMov;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentosLeitosMov.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.AtendimentosLeitosMov.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.Leitos.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentosLeitosMov
{
    public class AtendimentoLeitoMovAppService : SWMANAGERAppServiceBase, IAtendimentoLeitoMovAppService
    {
        [UnitOfWork]
        public async Task Criar(AtendimentoLeitoMovDto input)
        {
            try
            {
                var atendimentoLeitoMov = AtendimentoLeitoMovDto.Mapear(input);// .MapTo<AtendimentoLeitoMov>();
                using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                {
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        await atendimentoLeitoMovRepository.Object.InsertAsync(atendimentoLeitoMov).ConfigureAwait(false);

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }

                    // ALTERANDO ATENDOMENTO
                    if (input.AtendimentoId.HasValue && input.AtendimentoId.Value > 0)
                    {
                        var atendimento = new AtendimentoDto
                        {
                            Id = Convert.ToInt64(input.AtendimentoId),
                            LeitoId = Convert.ToInt64(input.LeitoId),
                            UnidadeOrganizacionalId = Convert.ToInt64(input.UnidadeOrganizacionalId)
                        };

                        await this.ObterEditarAtendimento(atendimento).ConfigureAwait(false);
                    }
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(this.L("ErroSalvar"));
            }
        }

        [UnitOfWork]

        // public async Task<AtendimentoLeitoMovDto> Editar(AtendimentoLeitoMovDto atendimentoLeitoMovDto)
        public async Task Editar(AtendimentoLeitoMovDto atendimentoLeitoMovDto)
        {
            try
            {
                using (var atendimentoLeitoMovRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                {
                    var result = await atendimentoLeitoMovRepository.Object.GetAll().Include(m => m.Atendimento)
                                     .Include(m => m.Leito).Include(m => m.User)
                                     .WhereIf(
                                         atendimentoLeitoMovDto.LeitoId != null,
                                         m => m.LeitoId == atendimentoLeitoMovDto.LeitoId)
                                     .Where(m => m.AtendimentoId == atendimentoLeitoMovDto.AtendimentoId)
                                     .FirstOrDefaultAsync().ConfigureAwait(false);

                    // UNITY OF WORK ESTAVA CAINDO EM EXCEPTION
                    // using (var unitOfWork = _unitOfWorkManager.Begin())
                    // {
                    if (result != null)
                    {
                        result.DataFinal = atendimentoLeitoMovDto.DataFinal;
                        await atendimentoLeitoMovRepository.Object.UpdateAsync(result).ConfigureAwait(false);
                    }

                    var atendimentoLeitoMov = AtendimentoLeitoMovDto.Mapear(result); // .MapTo<AtendimentoLeitoMovDto>();
                }

                // return atendimentoLeitoMov;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroEditar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await atendimentoLeitoMovRepository.Object.DeleteAsync(id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"));
            }
        }

        [UnitOfWork(false)]
        public AtendimentoLeitoMovDto Obter(long? id, long? atendimentoId = null)
        {
            try
            {
                using (var atendimentoLeitoMovRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                {
                    var result = atendimentoLeitoMovRepository.Object.GetAll().AsNoTracking()
                        .Include(m => m.Atendimento).Include(m => m.Leito).Include(m => m.Atendimento.Paciente)

                        // .Include(m => m.User)
                        // .Where(m => m.Atendimento.DataAlta == null)
                        .Include(m => m.Atendimento.Paciente.SisPessoa)

                        // .Include(m => m.User)
                        // .Where(m => m.Atendimento.DataAlta == null)
                        .Where(m => m.LeitoId == id).FirstOrDefault(m => m.AtendimentoId == atendimentoId);

                    var atendimentoLeitoMovDto = new AtendimentoLeitoMovDto();

                    if (result != null)
                    {
                        atendimentoLeitoMovDto.Id = result.Id;
                        atendimentoLeitoMovDto.DataInicial = result.DataInicial;
                        atendimentoLeitoMovDto.DataFinal = result.DataFinal;
                        atendimentoLeitoMovDto.DataInclusao = result.DataInclusao;
                        atendimentoLeitoMovDto.Atendimento = AtendimentoDto.MapearFromCore(result.Atendimento);
                        atendimentoLeitoMovDto.Leito = LeitoDto.MapearFromCore(result.Leito);
                    }

                    return atendimentoLeitoMovDto;
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"));
            }
        }

        [UnitOfWork(false)]
        public AtendimentoLeitoMovDto Obter(long atendimentoId)
        {
            try
            {
                using (var _atendimentoLeitoMovRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                {
                    var result = _atendimentoLeitoMovRepository.Object.GetAll().AsNoTracking()
                        .Include(m => m.Atendimento).Include(m => m.Leito).Include(m => m.Atendimento.Paciente)

                        // .Include(m => m.User)
                        // .Where(m => m.Atendimento.DataAlta == null)
                        .Include(m => m.Atendimento.Paciente.SisPessoa)
                        .FirstOrDefault(m => m.AtendimentoId == atendimentoId);

                    var atendimentoLeitoMovDto = new AtendimentoLeitoMovDto();

                    if (result == null)
                    {
                        return atendimentoLeitoMovDto;
                    }

                    atendimentoLeitoMovDto.Id = result.Id;
                    atendimentoLeitoMovDto.DataInicial = result.DataInicial;
                    atendimentoLeitoMovDto.DataFinal = result.DataFinal;
                    atendimentoLeitoMovDto.DataInclusao = result.DataInclusao;
                    atendimentoLeitoMovDto.Atendimento = AtendimentoDto.MapearFromCore(result.Atendimento);
                    atendimentoLeitoMovDto.Leito = LeitoDto.MapearFromCore(result.Leito);

                    return atendimentoLeitoMovDto;
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"));
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<AtendimentoLeitoMovIndexDto>> ListarFiltro(ListarAtendimentosLeitosMovInput input)
        {
            // List<AtendimentoLeitoMovDto> atendimentoLeitoMovDtos = new List<AtendimentoLeitoMovDto>();
            long atendimentoId = 0;
            var IdAtendimento = long.TryParse(input.Id, out atendimentoId);
            if (atendimentoId == 0)
            {
                throw new UserFriendlyException(this.L("SelecioneAtendimento"));
            }

            try
            {
                using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                {
                    var query = atendimentoLeitoMovRepository.Object.GetAll().AsNoTracking().Include(m => m.Atendimento)
                        .Include(m => m.Leito.TipoAcomodacao)

                        // .Include(m => m.Leito)
                        .Include(m => m.User).Include(m => m.Atendimento.Paciente)
                        .Include(m => m.Atendimento.Paciente.SisPessoa).Include(m => m.Leito)
                        .Where(m => m.AtendimentoId == atendimentoId)

                        // .WhereIf(input.Paciente != null, m => m.Atendimento.Paciente.NomeCompleto == input.Paciente)
                        // .WhereIf(input.Fornecedor > 0, m => m.FornecedorId == input.Fornecedor)
                        // .WhereIf(input.Documento != null, m => m.Documento == input.Documento)
                        .WhereIf(
                            !input.Filtro.IsNullOrEmpty(),
                            m => m.Atendimento.Paciente.NomeCompleto.Contains(input.Filtro)).Select(
                            s => new AtendimentoLeitoMovIndexDto
                            {
                                CodigoAtendimento = s.Atendimento.Codigo,
                                Leito = s.Leito.Descricao,
                                Id = s.Id,
                                TipoLeito = s.Leito.TipoAcomodacao.Descricao,
                                Paciente = s.Atendimento.Paciente.NomeCompleto,
                                DataInicial = s.DataInicial,
                                DataFinal = s.DataFinal,
                                DataInclusao = s.DataInclusao,
                                DataAlta = s.Atendimento.DataAlta
                            });

                    var contarVisitantes = await query.CountAsync().ConfigureAwait(false);

                    var atendimentoLeitoMovDto =
                        await query.OrderBy("Id").PageBy(input).ToListAsync().ConfigureAwait(false);


                    var ultimoMovimento = atendimentoLeitoMovDto.LastOrDefault();

                    if (ultimoMovimento != null)
                    {
                        ultimoMovimento.IsUltimoHistorico = true;
                    }

                    return new PagedResultDto<AtendimentoLeitoMovIndexDto>(contarVisitantes, atendimentoLeitoMovDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork]
        public async Task ObterEditarAtendimento(AtendimentoDto atendimentoDto)
        {
            try
            {
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                {
                    var result = await atendimentoRepository.Object.GetAll().Where(m => m.Id == atendimentoDto.Id)
                                     .FirstOrDefaultAsync().ConfigureAwait(false);

                    if (result != null)
                    {
                        using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            result.LeitoId = atendimentoDto.LeitoId;
                            result.UnidadeOrganizacionalId = atendimentoDto.UnidadeOrganizacionalId;

                            await atendimentoRepository.Object.UpdateAsync(result)
                                .ConfigureAwait(false); // .MapTo<EstoquePreMovimentoDto>();

                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }

                    var atendimento = AtendimentoDto.Mapear(result);
                }

                // .MapTo<AtendimentoDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }


        public async Task<DefaultReturn<AtendimentoLeitoMovDto>> TransferirLeito(long leitoOrigemId, long leitoDestinoId, long atendimentoId, DateTime dataHoraTransferencia, long? atendimentoDestinoId)
        {

            var _retornoPadrao = new DefaultReturn<AtendimentoLeitoMovDto>
            {
                Warnings = new List<ErroDto>(),
                Errors = new List<ErroDto>()
            };

            using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
            using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            {
                AtendimentoLeitoMov ultimoMovimento = null;
                var ultimoMovimentoLista = await atendimentoLeitoMovRepository.Object.GetAll()
                    .Where(w => w.LeitoId == leitoOrigemId && w.AtendimentoId == atendimentoId && w.DataFinal == null)
                    .OrderBy(o => o.Id)
                    .ToListAsync().ConfigureAwait(false);

                if (ultimoMovimentoLista != null && ultimoMovimentoLista.Count > 0)
                {
                    ultimoMovimento = ultimoMovimentoLista.LastOrDefault();
                }

                if (ultimoMovimento != null && (dataHoraTransferencia <= ultimoMovimento.DataInicial))
                {
                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "MOVL001" });
                }
                else
                {
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        var leitoDestino = await leitoRepository.Object.GetAll().FirstOrDefaultAsync(w => w.Id == leitoDestinoId)
                                               .ConfigureAwait(false);

                        var leitoOcupado = leitoDestino != null && leitoDestino.LeitoStatusId == 2; // status 2 = ocupado


                        var atendimento = await atendimentoRepository.Object.GetAll()
                                              .FirstOrDefaultAsync(w => w.Id == atendimentoId).ConfigureAwait(false);

                        if (atendimento != null)
                        {
                            atendimento.LeitoId = leitoDestinoId;
                            await atendimentoRepository.Object.UpdateAsync(atendimento).ConfigureAwait(false);
                        }


                        if (ultimoMovimento != null)
                        {
                            ultimoMovimento.DataFinal = dataHoraTransferencia;
                            await atendimentoLeitoMovRepository.Object.UpdateAsync(ultimoMovimento).ConfigureAwait(false);
                        }

                        var atendimentoLeitoMov = new AtendimentoLeitoMov
                        {
                            AtendimentoId = atendimentoId,
                            DataInicial = dataHoraTransferencia,
                            DataInclusao = DateTime.Now,
                            LeitoId = leitoDestinoId,
                            UserId = this.AbpSession.UserId
                        };


                        await atendimentoLeitoMovRepository.Object.InsertAsync(atendimentoLeitoMov).ConfigureAwait(false);


                        var leitoOrigem = await leitoRepository.Object.GetAll().Where(w => w.Id == leitoOrigemId)
                                              .FirstOrDefaultAsync().ConfigureAwait(false);

                        if (leitoOrigem != null && !leitoOcupado)
                        {

                            leitoOrigem.LeitoStatusId = 1;

                            await leitoRepository.Object.UpdateAsync(leitoOrigem).ConfigureAwait(false);
                        }


                        // var leitoDestino = _leitoRepository.GetAll()
                        // .Where(w => w.Id == leitoDestinoId)
                        // .FirstOrDefault();
                        if (leitoDestino != null && !leitoOcupado)
                        {

                            leitoDestino.LeitoStatusId = 2;

                            await leitoRepository.Object.UpdateAsync(leitoDestino).ConfigureAwait(false);
                        }


                        if (leitoOcupado)
                        {
                            AtendimentoLeitoMov ultimoMovimentoDestino = null;

                            var ultimoMovimentoDestinoLista = await atendimentoLeitoMovRepository.Object.GetAll()
                                                                  .Where(
                                                                      w => w.LeitoId == leitoDestinoId
                                                                           && w.AtendimentoId == atendimentoDestinoId
                                                                           && w.DataFinal == null).OrderBy(o => o.Id)
                                                                  .ToListAsync().ConfigureAwait(false);

                            if (ultimoMovimentoDestinoLista != null && ultimoMovimentoDestinoLista.Count > 0)
                            {
                                ultimoMovimentoDestino = ultimoMovimentoDestinoLista.LastOrDefault();
                            }

                            if (ultimoMovimentoDestino != null)
                            {
                                ultimoMovimentoDestino.DataFinal = dataHoraTransferencia;
                            }

                            var atendimentoLeitoMovDestino = new AtendimentoLeitoMov
                            {
                                AtendimentoId = atendimentoDestinoId,
                                DataInicial = dataHoraTransferencia,
                                DataInclusao = DateTime.Now,
                                LeitoId = leitoOrigemId,
                                UserId = this.AbpSession.UserId
                            };

                            var atendimentoDestino = await atendimentoRepository.Object.GetAll()
                                                         .Where(w => w.Id == atendimentoDestinoId).FirstOrDefaultAsync()
                                                         .ConfigureAwait(false);

                            if (atendimentoDestino != null)
                            {
                                atendimentoDestino.LeitoId = leitoOrigemId;
                                await atendimentoRepository.Object.UpdateAsync(atendimentoDestino).ConfigureAwait(false);
                            }


                            await atendimentoLeitoMovRepository.Object.InsertAsync(atendimentoLeitoMovDestino)
                                .ConfigureAwait(false);
                        }

                        unitOfWork.Complete();
                        await unitOfWorkManager.Object.Current.SaveChangesAsync().ConfigureAwait(false);
                        unitOfWork.Dispose();
                    }
                }
            }

            return _retornoPadrao;
        }


        [UnitOfWork]
        public async Task<DefaultReturn<AtendimentoLeitoMovDto>> ExcluirMovimentoLeito(long id)
        {
            var _retornoPadrao = new DefaultReturn<AtendimentoLeitoMovDto>
            {
                Warnings = new List<ErroDto>(),
                Errors = new List<ErroDto>()
            };


            AtendimentoLeitoMov penultimoMovimento = null;
            Leito leito = null;

            try
            {
                using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
                using (var leitoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Leito, long>>())
                using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var movimentoLeito = atendimentoLeitoMovRepository.Object.GetAll().FirstOrDefault(w => w.Id == id);

                    if (movimentoLeito != null)
                    {
                        var historioAtendimentoMovimentoLeito = atendimentoLeitoMovRepository.Object.GetAll()
                                                                                              .Where(w => w.AtendimentoId == movimentoLeito.AtendimentoId)
                                                                                              .OrderBy(o => o.Id)
                                                                                              .ToList();

                        if (historioAtendimentoMovimentoLeito.Count <= 1)
                        {
                            _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "MOVL002" });
                        }
                        else
                        {
                            penultimoMovimento = historioAtendimentoMovimentoLeito[historioAtendimentoMovimentoLeito.Count - 2];

                            // penultimoMovimento.DataFinal = null;

                            leito = leitoRepository.Object.GetAll().FirstOrDefault(w => w.Id == penultimoMovimento.LeitoId);

                            if (leito != null)
                            {
                                if (leito.LeitoStatusId != 1)
                                {
                                    _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "MOVL004", Parametros = new List<object> { leito.Descricao } });
                                }
                            }
                        }

                        if (_retornoPadrao.Errors.Count == 0)
                        {
                            if (historioAtendimentoMovimentoLeito.Count > 1)
                            {
                                // penultimoMovimento = historioAtendimentoMovimentoLeito[historioAtendimentoMovimentoLeito.Count - 2];
                                penultimoMovimento.DataFinal = null;

                                // var leito = _leitoRepository.GetAll()
                                // .Where(w => w.Id == penultimoMovimento.LeitoId)
                                // .FirstOrDefault();
                                if (leito != null)
                                {
                                    // ocupado
                                    leito.LeitoStatusId = 2;

                                    var atendimento = atendimentoRepository.Object.GetAll().FirstOrDefault(w => w.Id == movimentoLeito.AtendimentoId);

                                    if (atendimento != null)
                                    {
                                        atendimento.LeitoId = leito.Id;
                                    }
                                }

                                var ultimoMovimento = historioAtendimentoMovimentoLeito[historioAtendimentoMovimentoLeito.Count - 1];


                                var ultimoLeito = leitoRepository.Object.GetAll().FirstOrDefault(w => w.Id == ultimoMovimento.LeitoId);

                                if (ultimoLeito != null)
                                {
                                    // vago
                                    ultimoLeito.LeitoStatusId = 1;
                                }

                                await atendimentoLeitoMovRepository.Object.DeleteAsync(ultimoMovimento.Id).ConfigureAwait(false);
                            }
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"));
            }

            return _retornoPadrao;
        }

        public async Task<DefaultReturn<AtendimentoLeitoMovDto>> AltarDataMovimentoLeito(long id, DateTime novaData)
        {
            var _retornoPadrao = new DefaultReturn<AtendimentoLeitoMovDto>
            {
                Warnings = new List<ErroDto>(),
                Errors = new List<ErroDto>()
            };


            AtendimentoLeitoMov penultimoMovimento = null;

            using (var atendimentoLeitoMovRepository = IocManager.Instance.ResolveAsDisposable<IRepository<AtendimentoLeitoMov, long>>())
            {
                var movimentoLeito = await atendimentoLeitoMovRepository.Object.GetAll().Where(w => w.Id == id)
                                         .FirstOrDefaultAsync().ConfigureAwait(false);



                if (movimentoLeito != null)
                {
                    var historioAtendimentoMovimentoLeito = await atendimentoLeitoMovRepository.Object.GetAll()
                                                                .Where(
                                                                    w => w.AtendimentoId
                                                                         == movimentoLeito.AtendimentoId).ToListAsync()
                                                                .ConfigureAwait(false);
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        if (historioAtendimentoMovimentoLeito.Count > 1)
                        {
                            penultimoMovimento =
                                historioAtendimentoMovimentoLeito[historioAtendimentoMovimentoLeito.Count - 2];
                        }

                        if (penultimoMovimento != null && penultimoMovimento.DataInicial > novaData)
                        {
                            _retornoPadrao.Errors.Add(new ErroDto { CodigoErro = "MOVL003" });
                        }

                        if (!_retornoPadrao.Errors.Any())
                        {
                            if (penultimoMovimento != null)
                            {
                                penultimoMovimento.DataFinal = novaData;
                            }

                            movimentoLeito.DataInicial = novaData;
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }

            return _retornoPadrao;
        }
    }
}
