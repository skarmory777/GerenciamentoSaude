using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.EntregaContas;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas.Inputs;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Enumeradores;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas
{
    public class FaturamentoEntregaContaRecebidaAppService : SWMANAGERAppServiceBase, IFaturamentoEntregaContaRecebidaAppService
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FaturamentoEntregaContaRecebidaAppService(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }


        public async Task<PagedResultDto<VisualizacaoContaRecebidaDto>> ListarContasRecebidasPorQuitacao(ContasRecebidasPorQuitacaoInput input)
        {
            try
            {
                var resultDto = new List<VisualizacaoContaRecebidaDto>();
                var contasRecebidasRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoEntregaContaRecebida, long>>();

                var query = contasRecebidasRepository.Object.GetAll().AsNoTracking().
                    Where(x => x.QuitacaoId == input.QuitacaoId)
                    .Include(x => x.FaturamentoEntregaConta.ContaMedica.Paciente.SisPessoa);

                var qtdContasRecebidas = await query.CountAsync();
                var contasRecebidas = await query.AsNoTracking()
                    .OrderBy(x => x.FaturamentoEntregaConta.ContaMedica.Paciente.NomeCompleto).PageBy(input).ToListAsync();

                resultDto = contasRecebidas.Select(x => new VisualizacaoContaRecebidaDto()
                {
                    Id = x.Id,
                    Paciente = x.FaturamentoEntregaConta.ContaMedica.Paciente.NomeCompleto,
                    ValorRecebido = x.ValorRecebido,
                    ValorGlosaRecuperavel = x.ValorGlosaRecuperavel,
                    ValorGlosaIrrecuperavel = x.ValorGlosaIrrecuperavel,
                    EntregaContaId = x.FaturamentoEntregaContaId ?? 0,
                    QuitacaoId = x.QuitacaoId ?? 0
                }).ToList();


                return new PagedResultDto<VisualizacaoContaRecebidaDto>(qtdContasRecebidas, resultDto);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task ConciliarContasRecebidas(ConcilicarContasRecebidasInput input)
        {
            try
            {
                var quitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Quitacao, long>>();
                using var unitOfWork = _unitOfWorkManager.Begin();

                var quitacao = await quitacaoRepository.Object.GetAll()
                    .Include(x => x.LancamentoQuitacoes)
                    .Include(x => x.LancamentoQuitacoes.Select(x => x.Lancamento))
                    .SingleOrDefaultAsync(x => x.Id.Equals(input.QuitacaoId));

                var lancamentoId = quitacao.LancamentoQuitacoes.FirstOrDefault().LancamentoId;

                await CriarContasRecebidas(input.EntregaContas, quitacao.Id);
                await RemoverVinculosQuitacao(quitacao);
                await VincularContasAQuitacao(input);

                quitacao.DataConsolidado = input.DataConsolidado;
                quitacao.ValorImpostos = (decimal?)input.ValorImposto;
                quitacao.TipoQuitacaoId = (long)EnumTipoQuitacao.QuitacaoLancamento;
                await quitacaoRepository.Object.UpdateAsync(quitacao);

                await unitOfWork.CompleteAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        private async Task CriarContasRecebidas(List<EntregaContaInput> entregaContas, long quitacaoId)
        {
            try
            {
                var faturamentoEntregaContaRecebidaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoEntregaContaRecebida, long>>();

                foreach (var item in entregaContas)
                {
                    var entregaContaRecebida = new FaturamentoEntregaContaRecebida()
                    {
                        FaturamentoEntregaContaId = item.EntregaContaId,
                        QuitacaoId = quitacaoId,
                        ValorRecebido = item.ValorRecebido,
                        ValorGlosaRecuperavel = item.ValorGlosaRecuperavel,
                        ValorGlosaIrrecuperavel = item.ValorGlosaIrrecuperavel
                    };

                    await faturamentoEntregaContaRecebidaRepository.Object.InsertAsync(entregaContaRecebida);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        private async Task RemoverVinculosQuitacao(Quitacao quitacao)
        {
            var lancamentosQuitacoesExcluir = new List<long>();
            var documentosExcluir = new List<long>();
            var lancamentoQuitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LancamentoQuitacao, long>>();
            var documentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>();

            try
            {
                lancamentosQuitacoesExcluir = quitacao.LancamentoQuitacoes.Select(x => x.Id).ToList();
                documentosExcluir = (quitacao.LancamentoQuitacoes.Select(x => x.Lancamento.DocumentoId.Value).Distinct()).ToList();
                foreach (var lancamentoQuitacaoId in lancamentosQuitacoesExcluir)
                {
                    await lancamentoQuitacaoRepository.Object.DeleteAsync(lancamentoQuitacaoId);
                }

                foreach (var documentoId in documentosExcluir)
                {
                    await documentoRepository.Object.DeleteAsync(documentoId);
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        private async Task VincularContasAQuitacao(ConcilicarContasRecebidasInput input)
        {
            try
            {
                var documentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Documento, long>>();
                var lancamentoQuitacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LancamentoQuitacao, long>>();
                var lancamentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Lancamento, long>>();
                var lotesInformados = input.EntregaContas.Select(x => x.EntregaLoteId).Distinct();

                foreach (var loteId in lotesInformados)
                {
                    var documento = await documentoRepository.Object.GetAll()
                        .Include(x => x.LancamentoDocumentos)
                        .FirstOrDefaultAsync(x => x.FatEntregaLoteId == loteId);

                    var primeiroLancamento = documento.LancamentoDocumentos.FirstOrDefault();

                    var novoLancamentoQuitacao = new LancamentoQuitacao()
                    {
                        LancamentoId = primeiroLancamento.Id,
                        QuitacaoId = input.QuitacaoId,
                        IsDeleted = false,
                        ValorEfetivo = (decimal)input.EntregaContas.Where(x => x.EntregaLoteId.Equals(loteId)).Sum(y => y.ValorRecebido),
                        ValorQuitacao = (decimal)input.EntregaContas.Where(x => x.EntregaLoteId.Equals(loteId)).Sum(y => y.ValorRecebido),
                        IsSistema = false,
                    };

                    await lancamentoQuitacaoRepository.Object.InsertAsync(novoLancamentoQuitacao);

                    primeiroLancamento.SituacaoLancamentoId = (long)EnumSituacaoLancamento.Quitado;
                    await lancamentoRepository.Object.UpdateAsync(primeiroLancamento);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }        
    }
}
