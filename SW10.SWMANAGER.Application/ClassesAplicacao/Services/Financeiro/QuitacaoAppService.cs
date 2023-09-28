using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Threading;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.EntregaContas;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Inputs;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.ServicoValidacao;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class QuitacaoAppService : SWMANAGERAppServiceBase, IQuitacaoAppService
    {
        private readonly IRepository<Quitacao, long> _quitacaoRepository;
        private readonly IRepository<LancamentoQuitacao, long> _lancamentoQuitacaoRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Lancamento, long> _lancamentoRepository;
        private readonly IRepository<Cheque, long> _chequeRepository;
        private readonly IRepository<MeioPagamento, long> _meioPagamentoRepository;

        public QuitacaoAppService(IRepository<Quitacao, long> quitacaoRepository
                                          , IRepository<LancamentoQuitacao, long> lancamentoQuitacaoRepository
            , IUnitOfWorkManager unitOfWorkManager
            , IRepository<Lancamento, long> lancamentoRepository
            , IRepository<Cheque, long> chequeRepository
            , IRepository<MeioPagamento, long> meioPagamentoRepository)
        {
            _quitacaoRepository = quitacaoRepository;
            _lancamentoQuitacaoRepository = lancamentoQuitacaoRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _lancamentoRepository = lancamentoRepository;
            _chequeRepository = chequeRepository;
            _meioPagamentoRepository = meioPagamentoRepository;
        }


        Task<QuitacaoDto> IQuitacaoAppService.ObterPorLancamento(List<long> ids)
        {
            throw new NotImplementedException();
        }

        public virtual DefaultReturn<QuitacaoDto> CriarOuEditar(QuitacaoDto input)
        {
            var _retornoPadrao = new DefaultReturn<QuitacaoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();
            try
            {
                QuitacaoValidacaoService quitacaoValidacaoService = new QuitacaoValidacaoService(_lancamentoQuitacaoRepository
                                                                                                , _meioPagamentoRepository
                                                                                                , _chequeRepository);

                _retornoPadrao = quitacaoValidacaoService.Validar(input);

                if (_retornoPadrao.Errors.Count() == 0)
                {
                    var lancamentosQuitacoesDto = JsonConvert.DeserializeObject<List<QuitacaoIndex>>(input.LancamentosJson, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        if (input.Id == 0)
                        {
                            var quitacao = input.MapTo<Quitacao>();
                            quitacao.TipoQuitacaoId = (long)EnumTipoQuitacao.QuitacaoLancamento;
                            var meioPagamento = _meioPagamentoRepository.GetAll()
                                                        .Where(w => w.Id == quitacao.MeioPagamentoId)
                                                        .FirstOrDefault();

                            if (meioPagamento.TipoMeioPagamentoId == (long)EnumTipoMeioPagamento.Cheque)
                            {
                                quitacao.ChequeId = input.ChequeId;

                                if (input.ChequeId != null)
                                {
                                    var cheque = _chequeRepository.GetAll().Where(w => w.Id == input.ChequeId).FirstOrDefault();

                                    if (cheque != null)
                                    {
                                        quitacao.Numero = cheque.Numero.ToString();
                                    }
                                }
                            }

                            AtualizaListaLancamentoQuitacao(quitacao, lancamentosQuitacoesDto);

                            AsyncHelper.RunSync(() => _quitacaoRepository.InsertAsync(quitacao));

                            unitOfWork.Complete();
                            _unitOfWorkManager.Current.SaveChanges();

                            AtualizarStatusLancamentos(quitacao);
                            AtualizarUtilizacaoCheque(quitacao);

                            _retornoPadrao.ReturnObject = quitacao.MapTo<QuitacaoDto>();
                        }

                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
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

        void AtualizaListaLancamentoQuitacao(Quitacao quitacao, List<QuitacaoIndex> lancamentosQuitacoesDto)
        {
            if (quitacao.LancamentoQuitacoes == null)
            {
                quitacao.LancamentoQuitacoes = new List<LancamentoQuitacao>();
            }
            else
            {

                quitacao.LancamentoQuitacoes.RemoveAll(r => !lancamentosQuitacoesDto.Any(a => a.Id == r.Id));

                foreach (var lancamentoQuitacao in quitacao.LancamentoQuitacoes)
                {
                    var lancamentoQuitacaoDto = lancamentosQuitacoesDto.Where(w => w.Id == lancamentoQuitacao.Id)
                                                   .First();

                    lancamentoQuitacao.ValorEfetivo = lancamentoQuitacaoDto.ValorEfetivo ?? 0;
                    lancamentoQuitacao.ValorQuitacao = lancamentoQuitacaoDto.ValorQuitacao ?? 0;
                    lancamentoQuitacao.Acrescimo = lancamentoQuitacaoDto.Acrescimo ?? 0;
                    lancamentoQuitacao.Desconto = lancamentoQuitacaoDto.Desconto ?? 0;
                    lancamentoQuitacao.MoraMulta = lancamentoQuitacaoDto.MoraMulta ?? 0;
                    lancamentoQuitacao.Juros = lancamentoQuitacaoDto.Juros ?? 0;
                    lancamentoQuitacao.LancamentoId = lancamentoQuitacaoDto.LancamentoId;

                }
            }

            foreach (var lancamentoQuitacaoDto in lancamentosQuitacoesDto.Where(w => (w.Id == 0)))
            {
                var lancamentoQuitacao = new LancamentoQuitacao();

                lancamentoQuitacao.ValorEfetivo = lancamentoQuitacaoDto.ValorEfetivo ?? 0;
                lancamentoQuitacao.ValorQuitacao = lancamentoQuitacaoDto.ValorQuitacao ?? 0;
                lancamentoQuitacao.Acrescimo = lancamentoQuitacaoDto.Acrescimo ?? 0;
                lancamentoQuitacao.Desconto = lancamentoQuitacaoDto.Desconto ?? 0;
                lancamentoQuitacao.MoraMulta = lancamentoQuitacaoDto.MoraMulta ?? 0;
                lancamentoQuitacao.Juros = lancamentoQuitacaoDto.Juros ?? 0;
                lancamentoQuitacao.LancamentoId = lancamentoQuitacaoDto.LancamentoId;

                quitacao.LancamentoQuitacoes.Add(lancamentoQuitacao);
            }


        }

        void AtualizarStatusLancamentos(Quitacao quitacao)
        {
            var lancamentosIds = quitacao.LancamentoQuitacoes.Select(s => s.LancamentoId).ToList();

            var lancamentos = _lancamentoRepository.GetAll()
                                             .Where(w => lancamentosIds.Any(a => a == w.Id))
                                             .ToList();

            foreach (var item in lancamentos)
            {
                var valorQuitacoes = _lancamentoQuitacaoRepository.GetAll()
                                                             .Where(w => w.LancamentoId == item.Id)
                                                             .Sum(s => s.ValorEfetivo);

                if (item.ValorLancamento <= valorQuitacoes)
                {
                    item.SituacaoLancamentoId = (long)EnumSituacaoLancamento.Quitado;
                }
                else
                {
                    item.SituacaoLancamentoId = (long)EnumSituacaoLancamento.ParcialmenteQuitado;
                }

                AsyncHelper.RunSync(() => _lancamentoRepository.UpdateAsync(item));

            }
        }

        void AtualizarUtilizacaoCheque(Quitacao quitacao)
        {
            var meioPagamento = _meioPagamentoRepository.GetAll()
                                                        .Where(w => w.Id == quitacao.MeioPagamentoId)
                                                        .FirstOrDefault();
            if (meioPagamento != null && meioPagamento.TipoMeioPagamentoId == (long)EnumTipoMeioPagamento.Cheque)
            {
                var cheque = _chequeRepository.GetAll()
                                              .Where(w => w.TalaoCheque.ContaCorrenteId == quitacao.ContaCorrenteId
                                                      && w.Numero.ToString() == quitacao.Numero)
                                              .FirstOrDefault();

                if (cheque != null)
                {
                    cheque.Data = DateTime.Now;
                    AsyncHelper.RunSync(() => _chequeRepository.UpdateAsync(cheque));

                }

            }
        }

        public async Task<DefaultReturn<QuitacaoDto>> Excluir(long id)
        {
            var retornoPadrao = new DefaultReturn<QuitacaoDto>();

            try
            {
                var controleBancarioService = IocManager.Instance.ResolveAsDisposable<IControleBancarioAppService>();
                using (var quitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Quitacao, long>>())
                using (var lancamentoQuitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LancamentoQuitacao, long>>())
                using (var lancamentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Lancamento, long>>())
                {
                    var lancamentosIdsAtualizar = new List<long>();
                    var quitacao = await quitacaoRepository.Object.GetAll().AsNoTracking()
                                    .Include(i => i.LancamentoQuitacoes)
                                    .Include(x => x.LancamentoQuitacoes.Select(y => y.Lancamento))
                                    .SingleOrDefaultAsync(x => x.Id == id);

                    if (quitacao.TransferenciaIdentificador.HasValue)
                    {
                        return await controleBancarioService.Object.ExcluirTransferencias(quitacao.TransferenciaIdentificador.Value);
                    }

                    foreach (var lancamentoQuitacao in quitacao.LancamentoQuitacoes)
                    {                        
                        lancamentosIdsAtualizar.Add(lancamentoQuitacao.Lancamento.Id);                        
                        await lancamentoQuitacaoRepository.Object.DeleteAsync(lancamentoQuitacao.Id);
                    }

                    foreach (var lancamentoId in lancamentosIdsAtualizar)
                    {
                        var lancamento = await lancamentoRepository.Object.GetAll().SingleOrDefaultAsync(x => x.Id.Equals(lancamentoId));
                        lancamento.SituacaoLancamentoId = (long)EnumSituacaoLancamento.Aberto;
                        await lancamentoRepository.Object.UpdateAsync(lancamento);
                    }

                    await quitacaoRepository.Object.DeleteAsync(quitacao.Id);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return retornoPadrao;
        }

        public async Task<ListResultDto<QuitacaoIndex>> ListarQuitacoesPorLancamento(ListarQuitacaoLancamentoInput input)
        {
            var entregaContaRecebidaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoEntregaContaRecebida, long>>();
            List<QuitacaoIndex> quitacoes = new List<QuitacaoIndex>();

            long lancamentoId;

            long.TryParse(input.Filtro, out lancamentoId);


            var query = await _lancamentoQuitacaoRepository.GetAll()
                                           .Where(w => w.LancamentoId == lancamentoId)
                                           .Include(i => i.Quitacao)
                                           .Include(i => i.Quitacao.MeioPagamento)
                                           .Include(i => i.Quitacao.ContaCorrente)
                                           .Include(i => i.Lancamento)
                                           .ToListAsync();


            foreach (var item in query)
            {
                QuitacaoIndex quitacaoIndex = new QuitacaoIndex();

                quitacaoIndex.Id = item.QuitacaoId;
                quitacaoIndex.DataMovimento = item.Quitacao.DataMovimento;
                quitacaoIndex.Acrescimo = item.Acrescimo;
                quitacaoIndex.MoraMulta = item.MoraMulta;
                quitacaoIndex.ValorQuitacao = item.ValorQuitacao;
                quitacaoIndex.MeioPagamento = item.Quitacao.MeioPagamento.Descricao;
                quitacaoIndex.Numero = item.Quitacao.Numero;
                quitacaoIndex.Juros = item.Juros;
                quitacaoIndex.ContaCorrente = item.Quitacao.ContaCorrente.Descricao;
                quitacaoIndex.PossuiEntregaContaRecebida = await entregaContaRecebidaRepository.Object.GetAll().AnyAsync(x => x.QuitacaoId == item.QuitacaoId);
                quitacoes.Add(quitacaoIndex);
            }

            return new PagedResultDto<QuitacaoIndex>(
                 quitacoes.Count,
                 quitacoes
                 );

        }

        virtual public async Task<ListResultDto<ListarQuitacao>> ListarQuitacoes(ListarQuitacoesInput input)
        {
            try
            {
                if (input.MovimentoDe.HasValue)
                {
                    input.MovimentoDe = input.MovimentoDe.Value.ToLocalTime();
                }

                if (input.MovimentoAte.HasValue)
                {
                    input.MovimentoAte = input.MovimentoAte.Value.ToLocalTime();
                }

                if (input.ConciliacaoDe.HasValue)
                {
                    input.ConciliacaoDe = input.ConciliacaoDe.Value.ToLocalTime();
                }

                if (input.ConciliacaoAte.HasValue)
                {
                    input.ConciliacaoAte = input.ConciliacaoAte.Value.ToLocalTime();
                }

                using (var quitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Quitacao, long>>())
                using (var lancamentoQuitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LancamentoQuitacao, long>>())
                {
                    var query = quitacaoRepository.Object.GetAll().AsNoTracking().
                                    Where(x =>
                                            (input.Filtro == "" || input.Filtro == null)
                                            && (input.MovimentoDe == null || input.IgnorarDataMovimento || (input.MovimentoDe <= x.DataMovimento && input.MovimentoAte >= x.DataMovimento))
                                            && (input.ConciliacaoDe == null || input.IgnorarDataConciliacao || (input.ConciliacaoDe <= x.DataConsolidado && input.ConciliacaoAte >= x.DataConsolidado))
                                            && (input.EmpresaId == null || input.EmpresaId == x.LancamentoQuitacoes.FirstOrDefault().Lancamento.Documento.EmpresaId)
                                            && (input.PessoaId == null || input.PessoaId == x.LancamentoQuitacoes.FirstOrDefault().Lancamento.Documento.PessoaId)
                                            && (input.ContaCorrenteId == null || input.ContaCorrenteId == x.ContaCorrenteId)
                                            && (input.MeioPagamentoId == null || input.MeioPagamentoId == x.MeioPagamentoId)
                                        )
                                    .Include(x => x.ContaCorrente)
                                    .Include(x => x.MeioPagamento)
                                    .Include(x => x.LancamentoQuitacoes.Select(l => l.Lancamento.Documento.Pessoa));


                    query = query.OrderByDescending(x => x.DataMovimento);

                    if (!string.IsNullOrEmpty(input.Sorting))
                    {
                        var splitSort = input.Sorting.Split(' ');
                        var campo = splitSort[0];
                        if (campo == "DataMovimento")
                        {
                            if (splitSort[1] == "DESC")
                                query = query.OrderByDescending(x => x.DataMovimento);
                            else
                                query = query.OrderBy(x => x.DataMovimento);
                        }
                        if (campo == "Quitacao")
                        {
                            if (splitSort[1] == "DESC")                            
                                query = query.OrderByDescending(x => x.Id);
                            else
                                query = query.OrderBy(x => x.Id);
                        }
                        if (campo == "MeioPagamento")
                        {
                            if (splitSort[1] == "DESC")
                                query = query.OrderByDescending(x => x.MeioPagamento.Descricao);
                            else
                                query = query.OrderBy(x => x.MeioPagamento.Descricao);
                        }
                        if (campo == "ContaCorrente")
                        {
                            if (splitSort[1] == "DESC")
                                query = query.OrderByDescending(x => x.ContaCorrente.Descricao);
                            else
                                query = query.OrderBy(x => x.ContaCorrente.Descricao);
                        }
                        if (campo == "DataCompensado")
                        {
                            if (splitSort[1] == "DESC")
                                query = query.OrderByDescending(x => x.DataCompensado);
                            else
                                query = query.OrderBy(x => x.DataCompensado);
                        }
                        if (campo == "DataConsolidado")
                        {
                            if (splitSort[1] == "DESC")
                                query = query.OrderByDescending(x => x.DataConsolidado);
                            else
                                query = query.OrderBy(x => x.DataConsolidado);
                        }
                        if (campo == "Observacao")
                        {
                            if (splitSort[1] == "DESC")
                                query = query.OrderByDescending(x => x.Observacao);
                            else
                                query = query.OrderBy(x => x.Observacao);
                        }
                    }

                    var qtdLancamentos = await query.CountAsync();
                    query = query.Skip(input.SkipCount * input.MaxResultCount).Take(input.MaxResultCount);

                    var result = query.Select(x => new ListarQuitacao()
                    {
                        Id = x.Id,
                        DataMovimento = x.DataMovimento,
                        ContaCorrenteDescricao = x.ContaCorrente.Descricao,
                        DataCompensado = x.DataCompensado,
                        DataConsolidado = x.DataConsolidado,
                        Observacao = x.Observacao,
                        PessoaNome = x.LancamentoQuitacoes.Count > 0 ? x.LancamentoQuitacoes.FirstOrDefault().Lancamento.Documento.Pessoa.Descricao : "",
                        MeioPagamentoDescricao = x.MeioPagamento.Descricao,
                        ValorTotal = x.LancamentoQuitacoes.Count > 0 ? 
                            (x.LancamentoQuitacoes.Sum(lq => lq.ValorQuitacao) - (x.ValorImpostos ?? 0)): 0,
                        IsCredito =  x.LancamentoQuitacoes.Count > 0 ? x.LancamentoQuitacoes.FirstOrDefault().Lancamento.IsCredito : false
                    }).ToList();

                    return new PagedResultDto<ListarQuitacao>(qtdLancamentos, result);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<ListarQuitacao>> ListarQuitacoesNaoConsolidadas(ListarQuitacoesNaoConsolidadasInput input)
        {
            try
            {
                var quitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Quitacao, long>>();

                var query = quitacaoRepository.Object.GetAll().AsNoTracking()
                    .Include(x => x.ContaCorrente)
                    .Include(x => x.MeioPagamento)
                    .Include(x => x.LancamentoQuitacoes.Select(l => l.Lancamento.Documento.Pessoa))
                    .Where(x => x.DataConsolidado == null)
                    .Where(x => x.DataMovimento == input.DataMovimento)
                    .Where(x => x.ContaCorrenteId.Equals(input.ContaCorrenteId));


                var qtdEntregaContas = await query.CountAsync();
                var quitacoes = await query.AsNoTracking().OrderBy(x => x.Id).PageBy(input).ToListAsync();

                var resultDto = quitacoes.Select(x => new ListarQuitacao()
                {
                    Id = x.Id,
                    DataMovimento = x.DataMovimento,
                    ContaCorrenteDescricao = x.ContaCorrente.Descricao,
                    DataCompensado = x.DataCompensado,
                    DataConsolidado = x.DataConsolidado,
                    Observacao = x.Observacao,
                    PessoaNome = x.LancamentoQuitacoes.Count > 0 ? x.LancamentoQuitacoes.FirstOrDefault().Lancamento.Documento.Pessoa?.Descricao : "",
                    MeioPagamentoDescricao = x.MeioPagamento.Descricao,
                    ValorTotal = x.LancamentoQuitacoes.Sum(lq => lq.ValorQuitacao),
                    IsCredito = x.LancamentoQuitacoes.FirstOrDefault().Lancamento.IsCredito
                }).ToList();

                return new PagedResultDto<ListarQuitacao>(qtdEntregaContas, resultDto);
            }
            catch(Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
