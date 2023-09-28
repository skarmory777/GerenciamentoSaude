using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro
{
    public class ControleBancarioAppService : SWMANAGERAppServiceBase, IControleBancarioAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public ControleBancarioAppService(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<DefaultReturn<QuitacaoDto>> CriarLancamento(QuitacaoDto input)
        {
            var _retornoPadrao = new DefaultReturn<QuitacaoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                using (var documentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>())
                using (var tipoDocumentoAppService = IocManager.Instance.ResolveAsDisposable<ITipoDocumentoAppService>())
                {
                    if (!input.TipoDocumentoId.HasValue)
                    {
                        var tipoDocumento = await tipoDocumentoAppService.Object.ObterPelaDescricao("Controle Bancário");
                        input.TipoDocumentoId = tipoDocumento.Id;
                    }
                    var documentoEntity = new Documento()
                    {
                        PessoaId = input.PessoaId,
                        EmpresaId = input.EmpresaId.Value,
                        Numero = input.Numero,
                        IsCredito = input.IsCredito,
                        ValorDocumento = input.ValorQuitacao,
                        TipoDocumentoId = input.TipoDocumentoId.Value,
                        Observacao = !string.IsNullOrEmpty(input.Observacao) ? input.Observacao : null,
                        LancamentoDocumentos = new List<Lancamento>(),
                        Rateios = new List<DocumentoRateio>()
                    };

                    var lancamentoEntity = new Lancamento()
                    {
                        ValorLancamento = input.ValorQuitacao,
                        IsCredito = input.IsCredito,
                        IsDeleted = false,
                        NossoNumero = input.Numero,
                        SituacaoLancamentoId = (long)EnumSituacaoLancamento.Quitado,
                        Parcela = 1,
                        IsSistema = false,
                        LancamentosQuitacoes = new List<LancamentoQuitacao>()
                    };
                    documentoEntity.LancamentoDocumentos.Add(lancamentoEntity);

                    var lancamentoQuitacaoEntity = new LancamentoQuitacao()
                    {
                        IsDeleted = false,
                        ValorEfetivo = input.ValorQuitacao,
                        ValorQuitacao = input.ValorQuitacao,
                        IsSistema = false,
                        Quitacao = new Quitacao()
                    };
                    lancamentoEntity.LancamentosQuitacoes.Add(lancamentoQuitacaoEntity);

                    var quitacaoEntity = new Quitacao()
                    {
                        ContaCorrenteId = (long)input.ContaCorrenteId,
                        MeioPagamentoId = (long)input.MeioPagamentoId,
                        Numero = input.Numero,
                        DataMovimento = input.DataMovimento,
                        DataCompensado = input.DataCompensado,
                        DataConsolidado = input.DataConsolidado,
                        Observacao = input.Observacao,
                        TransferenciaIdentificador = input.TransferenciaIdentificador,
                        TipoQuitacaoId = input.TipoQuitacaoId,
                        IsSistema = false,
                        IsDeleted = false
                    };
                    lancamentoQuitacaoEntity.Quitacao = quitacaoEntity;

                    if (!string.IsNullOrEmpty(input.RateioJson))
                    {
                        var rateiosDto = JsonConvert.DeserializeObject<List<DocumentoRateioDto>>(input.RateioJson);
                        foreach (var rateio in rateiosDto)
                        {
                            var rateioEntity = new DocumentoRateio()
                            {
                                CentroCustoId = rateio.CentroCustoId,
                                ContaAdministrativaId = rateio.ContaAdministrativaId,
                                EmpresaId = rateio.EmpresaId,
                                IsCredito = input.IsCredito,
                                IsSistema = false,
                                IsDeleted = false,
                                IsImposto = rateio.IsImposto,
                                Observacao = !string.IsNullOrEmpty(rateio.Observacao) ? rateio.Observacao : null,
                                Valor = rateio.Valor
                            };
                            documentoEntity.Rateios.Add(rateioEntity);
                        }
                    }

                    await documentoRepository.Object.InsertAsync(documentoEntity);
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

        public async Task<DefaultReturn<QuitacaoDto>> CriarTransferencia(TransferenciaDto input)
        {
            var _retornoPadrao = new DefaultReturn<QuitacaoDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {
                using (var tipoDocumentoAppService = IocManager.Instance.ResolveAsDisposable<ITipoDocumentoAppService>())
                {
                    var tipoDocumento = await tipoDocumentoAppService.Object.ObterPelaDescricao("Transferência Bancária");
                    var transferenciaIdentificador = Guid.NewGuid();
                    var origemLancamento = new QuitacaoDto()
                    {
                        EmpresaId = input.OrigemEmpresaId,
                        MeioPagamentoId = input.MeioPagamentoId,
                        ContaCorrenteId = input.OrigemContaCorrenteId,
                        DataMovimento = input.OrigemDataMovimento,
                        DataCompensado = input.OrigemDataMovimento,
                        DataConsolidado = input.OrigemDataMovimento,
                        Numero = input.OrigemNumero,
                        ValorQuitacao = input.Valor,
                        Observacao = input.OrigemObservacao,
                        IsCredito = false,
                        TipoDocumentoId = tipoDocumento.Id,
                        TransferenciaIdentificador = transferenciaIdentificador,
                        TipoQuitacaoId = (long)EnumTipoQuitacao.Transferencia
                    };
                    await CriarLancamento(origemLancamento);

                    var destinoLancamento = new QuitacaoDto()
                    {
                        EmpresaId = input.DestinoEmpresaId,
                        MeioPagamentoId = input.MeioPagamentoId,
                        ContaCorrenteId = input.DestinoContaCorrenteId,
                        DataMovimento = input.DestinoDataMovimento,
                        DataCompensado = input.DestinoDataMovimento,
                        DataConsolidado = input.DestinoDataMovimento,
                        Numero = input.DestinoNumero,
                        ValorQuitacao = input.Valor,
                        Observacao = input.DestinoObservacao,
                        IsCredito = true,
                        TipoDocumentoId = tipoDocumento.Id,
                        TransferenciaIdentificador = transferenciaIdentificador,
                        TipoQuitacaoId = (long)EnumTipoQuitacao.Transferencia
                    };
                    await CriarLancamento(destinoLancamento);
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

        public async Task<DefaultReturn<QuitacaoDto>> ExcluirTransferencias(Guid transferenciaIdentificador)
        {
            var documentoIdsToRemove = new List<long>();
            var lancamentoIdsToRemove = new List<long>();
            var lancamentoQuitacoesIdsToRemove = new List<long>();
            var quitacoesIdsToRemove = new List<long>();
            var _retornoPadrao = new DefaultReturn<QuitacaoDto>()
            {
                Warnings = new List<ErroDto>(),
                Errors = new List<ErroDto>()
            };

            try
            {
                var quitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Quitacao, long>>();
                var lancamentoQuitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LancamentoQuitacao, long>>();
                var lancamentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Lancamento, long>>();
                var documentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>();
                using var unitOfWork = _unitOfWorkManager.Begin();

                var quitacoes = await quitacaoRepository.Object.GetAll()
                                        .Include(i => i.LancamentoQuitacoes)
                                        .Include(x => x.LancamentoQuitacoes.Select(y => y.Lancamento.Documento))
                                        .Where(x => x.TransferenciaIdentificador == transferenciaIdentificador)
                                        .ToListAsync();

                foreach (var quitacao in quitacoes)
                {
                    foreach (var lancamentoQuitacao in quitacao.LancamentoQuitacoes)
                    {
                        documentoIdsToRemove.Add(lancamentoQuitacao.Lancamento.DocumentoId.Value);
                        lancamentoIdsToRemove.Add(lancamentoQuitacao.LancamentoId);
                        lancamentoQuitacoesIdsToRemove.Add(lancamentoQuitacao.Id);                                                
                    }
                    quitacoesIdsToRemove.Add(quitacao.Id);
                }

                foreach (var documentoId in documentoIdsToRemove)
                {
                    var documento = await documentoRepository.Object.GetAsync(documentoId);
                    await documentoRepository.Object.DeleteAsync(documento);
                }

                foreach (var lancamentoId in lancamentoIdsToRemove)
                {
                    var lancamento = await lancamentoRepository.Object.GetAsync(lancamentoId);
                    await lancamentoRepository.Object.DeleteAsync(lancamento);
                }

                foreach (var lancamentoQuitacaoId in lancamentoQuitacoesIdsToRemove)
                {
                    var lancamentoQuitacao = await lancamentoQuitacaoRepository.Object.GetAsync(lancamentoQuitacaoId);
                    await lancamentoQuitacaoRepository.Object.DeleteAsync(lancamentoQuitacao);
                }

                foreach (var quitacaoId in quitacoesIdsToRemove)
                {
                    var quitacao = await quitacaoRepository.Object.GetAsync(quitacaoId);
                    await quitacaoRepository.Object.DeleteAsync(quitacao);
                }

                await unitOfWork.CompleteAsync();
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
