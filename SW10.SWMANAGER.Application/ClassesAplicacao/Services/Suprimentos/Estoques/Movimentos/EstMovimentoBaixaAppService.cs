using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class EstMovimentoBaixaAppService : SWMANAGERAppServiceBase, IEstMovimentoBaixaAppService
    {
        public bool PossuiVales(long preMovimentoId)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estMovimentoBaixaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixa, long>>())
            {
                var query = from movimento in estoqueMovimentoRepository.Object.GetAll()
                            join baixa in estMovimentoBaixaRepository.Object.GetAll()
                            on movimento.Id equals baixa.MovimentoBaixaPrincipalId
                            where movimento.EstoquePreMovimentoId == preMovimentoId
                            select movimento.Id;

                return query.Count() > 0;
            }
        }

        public bool PossuiNota(long preMovimentoId)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estMovimentoBaixaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixa, long>>())
            {
                var query = from movimento in estoqueMovimentoRepository.Object.GetAll()
                            join baixa in estMovimentoBaixaRepository.Object.GetAll()
                            on movimento.Id equals baixa.MovimentoBaixaId
                            where movimento.EstoquePreMovimentoId == preMovimentoId
                            select movimento.Id;

                return query.Count() > 0;
            }
        }

        public async Task<decimal> QuantidadeBaixaItemPendente(long movimentoItemId)
        {
            using (var estMovimentoBaixaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixaItem, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            {
                var query = estMovimentoBaixaItemRepository.Object.GetAll()
                .Where(w => w.EstoqueMovimentoItemId == movimentoItemId);

                var movimentoItem = estoqueMovimentoItemRepository.Object.Get(movimentoItemId);

                var quantidadeBaixada = query.ToList().Sum(s => s.Quantidade);

                return movimentoItem.Quantidade - quantidadeBaixada;
            }
        }

        public async Task<EstMovimentoBaixaItemDto> ObterItemBaixa(long baixaItemid)
        {
            using (var estMovimentoBaixaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixaItem, long>>())
            {
                return EstMovimentoBaixaItemDto.Mapear(await estMovimentoBaixaItemRepository.Object.GetAsync(baixaItemid));//.MapTo<EstMovimentoBaixaItemDto>();
            }
        }

        public bool PossuiItemConsignados(long preMovimentoId)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estMovimentoBaixaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixaItem, long>>())
            {
                var query = from movimento in estoqueMovimentoRepository.Object.GetAll()
                            join baixaItem in estMovimentoBaixaItemRepository.Object.GetAll()
                            on movimento.Id equals baixaItem.EstoqueMovimentoBaixaId
                            where movimento.EstoquePreMovimentoId == preMovimentoId
                            select movimento.Id;

                return query.Count() > 0;
            }
        }

        public async Task<decimal> Editar(MovimentoItemDto input)
        {
            try
            {
                using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estMovimentoBaixaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixa, long>>())
                using (var unidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {
                    decimal totalDocumento = 0;

                    var movimentoItem = estoqueMovimentoItemRepository.Object.Get(input.Id);
                    if (movimentoItem != null)
                    {
                        var unidade = unidadeRepository.Object.Get((long)movimentoItem.ProdutoUnidadeId);

                        var valorAntigo = movimentoItem.CustoUnitario * (movimentoItem.Quantidade / unidade.Fator);
                        var valorAtual = input.CustoUnitario * (movimentoItem.Quantidade / unidade.Fator);

                        movimentoItem.CustoUnitario = input.CustoUnitario;
                        movimentoItem.PerIPI = input.PerIPI;

                        await estoqueMovimentoItemRepository.Object.UpdateAsync(movimentoItem);

                        var movimento = estoqueMovimentoRepository.Object.Get(movimentoItem.MovimentoId);
                        if (movimento != null)
                        {
                            movimento.TotalDocumento += valorAtual - valorAntigo;
                            await estoqueMovimentoRepository.Object.UpdateAsync(movimento);
                        }

                        if (movimentoItem.EstoquePreMovimentoItemId != null)
                        {
                            var preMovimentoItem = estoquePreMovimentoItemRepository.Object.Get((long)movimentoItem.EstoquePreMovimentoItemId);

                            preMovimentoItem.CustoUnitario = input.CustoUnitario;
                            preMovimentoItem.PerIPI = input.PerIPI;

                            await estoquePreMovimentoItemRepository.Object.UpdateAsync(preMovimentoItem);

                            var preMovimento = estoquePreMovimentoRepository.Object.Get(preMovimentoItem.PreMovimentoId);
                            if (preMovimento != null)
                            {
                                preMovimento.TotalDocumento += valorAtual - valorAntigo;
                                await estoquePreMovimentoRepository.Object.UpdateAsync(preMovimento);
                            }
                        }

                        var baixa = estMovimentoBaixaRepository.Object.GetAll()
                                    .Where(w => w.MovimentoBaixaId == movimento.Id)
                                    .FirstOrDefault();

                        if (baixa != null)
                        {
                            var baixas = from movimentoBaixa in estMovimentoBaixaRepository.Object.GetAll()
                                         join movimentoDetalhe in estoqueMovimentoRepository.Object.GetAll()
                                         on movimentoBaixa.MovimentoBaixaId equals movimentoDetalhe.Id
                                         where movimentoBaixa.MovimentoBaixaPrincipalId == baixa.MovimentoBaixaPrincipalId
                                         select movimentoDetalhe;

                            totalDocumento = baixas.ToList().Sum(s => s.TotalDocumento);


                            var movimentoNotaFiscal = estoqueMovimentoRepository.Object.Get(baixa.MovimentoBaixaPrincipalId);
                            movimentoNotaFiscal.TotalDocumento = totalDocumento;
                            await estoqueMovimentoRepository.Object.UpdateAsync(movimentoNotaFiscal);


                            var preMovimentoNotaFiscal = estoquePreMovimentoRepository.Object.Get(movimentoNotaFiscal.EstoquePreMovimentoId);
                            preMovimentoNotaFiscal.TotalDocumento = totalDocumento;
                            await estoquePreMovimentoRepository.Object.UpdateAsync(preMovimentoNotaFiscal);

                        }
                    }


                    return totalDocumento;
                }


            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }
    }
}
