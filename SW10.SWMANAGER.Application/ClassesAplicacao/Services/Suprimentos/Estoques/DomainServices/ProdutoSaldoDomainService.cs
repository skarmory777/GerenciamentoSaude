using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices
{
    public class ProdutoSaldoDomainService : SWMANAGERDomainServiceBase, IProdutoSaldoDomainService
    {
        public void AtualizarSaldoMovimento(long movimentoId)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            using (var estoqueMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoLoteValidade, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                var movimento = estoqueMovimentoRepository.Object.Get(movimentoId);
                if (movimento == null)
                {
                    return;
                }
                var movimentoItens = estoqueMovimentoItemRepository.Object.GetAll()
                    .AsNoTracking().Where(w => w.MovimentoId == movimentoId).ToList();

                foreach (var item in movimentoItens)
                {
                    var produto = produtoRepository.Object.Get(item.ProdutoId);
                    item.Produto = produto;

                    if (item.Produto.IsLote || item.Produto.IsValidade)
                    {
                        var preMovimentoIemLotesValidades = estoqueMovimentoLoteValidadeRepository.Object.GetAll()
                            .AsNoTracking()
                            .Where(w => w.EstoqueMovimentoItemId == item.Id).ToList();

                        foreach (var itemLoteValidade in preMovimentoIemLotesValidades)
                        {
                            var saldo = produtoSaldoRepository.Object
                                .GetAll()
                                .AsNoTracking()
                                .FirstOrDefault(w => w.EstoqueId == movimento.EstoqueId
                                                              && w.LoteValidadeId == itemLoteValidade.LoteValidadeId
                                                              && w.ProdutoId == item.ProdutoId);

                            var quantidade = CalcularQuantidade(movimento.IsEntrada, itemLoteValidade.Quantidade);

                            if (saldo == null)
                            {
                                var produtoSaldo = new ProdutoSaldo
                                {
                                    EstoqueId = (long)movimento.EstoqueId,
                                    LoteValidadeId = itemLoteValidade.LoteValidadeId,
                                    ProdutoId = item.ProdutoId,
                                    QuantidadeAtual = quantidade
                                };
                                if(!movimento.IsEntrada)
                                {
                                    this.Logger.Error($"AtualizarSaldoMovimento - TENTATIVA DE CADASTRO DE PRODUTO SALDO: MovimentoId {movimento.Id} EstoqueId {produtoSaldo.EstoqueId} ProdutoId {produtoSaldo.ProdutoId} LoteValidadeId {produtoSaldo.LoteValidadeId}");
                                }

                                produtoSaldoRepository.Object.Insert(produtoSaldo);
                            }
                            else
                            {
                                produtoSaldoRepository.Object.Update(saldo.Id, saldoUpdate =>
                                {
                                    saldoUpdate.QuantidadeAtual += quantidade;

                                    if (movimento.IsEntrada)
                                    {
                                        saldoUpdate.QuantidadeEntradaPendente = saldo.QuantidadeEntradaPendente - itemLoteValidade.Quantidade;
                                    }
                                    else
                                    {
                                        saldoUpdate.QuantidadeSaidaPendente = saldo.QuantidadeSaidaPendente - - itemLoteValidade.Quantidade;
                                    }
                                });
                            }
                        }
                    }
                    else
                    {
                        var saldo = produtoSaldoRepository.Object.GetAll()
                            .AsNoTracking()
                            .FirstOrDefault(w => w.EstoqueId == (long)movimento.EstoqueId && w.ProdutoId == item.ProdutoId);

                        var quantidade = CalcularQuantidade(movimento.IsEntrada, item.Quantidade);

                        if (saldo == null)
                        {
                            var produtoSaldo = new ProdutoSaldo
                            {
                                EstoqueId = (long)movimento.EstoqueId,
                                ProdutoId = item.ProdutoId,
                                QuantidadeAtual = quantidade
                            };

                            produtoSaldoRepository.Object.Insert(produtoSaldo);
                        }
                        else
                        {
                            produtoSaldoRepository.Object.Update(saldo.Id, saldoUpdate =>
                            {
                                saldoUpdate.QuantidadeAtual += quantidade;
                                if (movimento.IsEntrada)
                                {
                                    saldoUpdate.QuantidadeEntradaPendente = saldo.QuantidadeEntradaPendente - item.Quantidade;
                                }
                                else
                                {
                                    saldoUpdate.QuantidadeSaidaPendente = saldo.QuantidadeSaidaPendente - item.Quantidade;
                                }
                            });
                        }
                    }
                }
            }
        }

        public async Task AtualizarSaldoPreMovimentoItem(EstoquePreMovimentoItem preMovimentoItem)
        {
            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var preMovimento = await estoquePreMovimentoRepository.Object
                    .GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == preMovimentoItem.PreMovimentoId).ConfigureAwait(false);
                if (preMovimento == null)
                {
                    return;
                }

                var produtoSaldo = produtoSaldoRepository.Object.GetAll()
                    .AsNoTracking()
                    .FirstOrDefault(w => w.ProdutoId == preMovimentoItem.ProdutoId && w.EstoqueId == preMovimento.EstoqueId);

                if (produtoSaldo != null)
                {
                    produtoSaldoRepository.Object.Update(produtoSaldo.Id, produtoSaldoUpdate =>
                    {
                        if (preMovimento.IsEntrada)
                        {
                            produtoSaldoUpdate.QuantidadeEntradaPendente = ObterQuantidadeProduto(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada);
                        }
                        else
                        {
                            produtoSaldoUpdate.QuantidadeSaidaPendente = ObterQuantidadeProduto(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada);
                        }
                    });
                }
                else
                {
                    produtoSaldo = new ProdutoSaldo
                    {
                        EstoqueId = (long) preMovimento.EstoqueId,
                        ProdutoId = preMovimentoItem.ProdutoId
                    };

                    if (preMovimento.IsEntrada)
                    {
                        produtoSaldo.QuantidadeEntradaPendente = ObterQuantidadeProduto(preMovimentoItem.ProdutoId, (long) preMovimento.EstoqueId, preMovimento.IsEntrada);
                    }
                    else
                    {
                        this.Logger.Error($"AtualizarSaldoPreMovimentoItem - TENTATIVA DE CADASTRO DE PRODUTO SALDO: PreMovimentoItemId {preMovimentoItem.Id} PreMovimentoId {preMovimentoItem.PreMovimentoId} EstoqueId {produtoSaldo.EstoqueId} ProdutoId {produtoSaldo.ProdutoId} ");
                        produtoSaldo.QuantidadeSaidaPendente = ObterQuantidadeProduto(preMovimentoItem.ProdutoId, (long) preMovimento.EstoqueId, preMovimento.IsEntrada);
                    }

                    await produtoSaldoRepository.Object.InsertAsync(produtoSaldo);
                }
            }
        }

        public void AtualizarSaldoPreMovimentoItemLoteValidade(EstoquePreMovimentoLoteValidadeDto preMovimentoItemLoteValidade)
        {
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var preMovimentoItem = estoquePreMovimentoItemRepository.Object.Get(preMovimentoItemLoteValidade.EstoquePreMovimentoItemId);

                if (preMovimentoItem == null)
                {
                    return;
                }
                var preMovimento = estoquePreMovimentoRepository.Object.Get(preMovimentoItem.PreMovimentoId);

                if (preMovimento == null)
                {
                    return;
                }
                
                var produtoSaldo = produtoSaldoRepository.Object
                    .GetAll().AsNoTracking()
                    .FirstOrDefault(w => w.ProdutoId == preMovimentoItem.ProdutoId 
                    && w.EstoqueId == (long)preMovimento.EstoqueId 
                    && w.LoteValidadeId == preMovimentoItemLoteValidade.LoteValidadeId);

                var quantidade = preMovimentoItemLoteValidade.Quantidade;

                if (produtoSaldo != null)
                {
                    if (preMovimento.IsEntrada)
                    {
                        produtoSaldo.QuantidadeEntradaPendente += quantidade;
                    }
                    else
                    {
                        produtoSaldo.QuantidadeSaidaPendente += quantidade;
                    }

                    produtoSaldoRepository.Object.Update(produtoSaldo.Id,x=>
                    {
                        x.QuantidadeEntradaPendente = produtoSaldo.QuantidadeEntradaPendente;
                        x.QuantidadeSaidaPendente = produtoSaldo.QuantidadeSaidaPendente;
                    });
                }
                else
                {
                    produtoSaldo = new ProdutoSaldo
                    {
                        EstoqueId = (long)preMovimento.EstoqueId,
                        ProdutoId = preMovimentoItem.ProdutoId,
                        LoteValidadeId = preMovimentoItemLoteValidade.LoteValidadeId
                    };

                    if (preMovimento.IsEntrada)
                    {
                        produtoSaldo.QuantidadeEntradaPendente = quantidade;
                    }
                    else
                    {
                        this.Logger.Error($"AtualizarSaldoPreMovimentoItemLoteValidade - TENTATIVA DE CADASTRO DE PRODUTO SALDO:  PreMovimentoItemId {preMovimentoItem.Id} PreMovimentoId {preMovimentoItem.PreMovimentoId} EstoqueId {produtoSaldo.EstoqueId} ProdutoId {produtoSaldo.ProdutoId} LoteValidadeId {produtoSaldo.LoteValidadeId}");
                        produtoSaldo.QuantidadeSaidaPendente = quantidade;
                    }

                    produtoSaldoRepository.Object.Insert(produtoSaldo);
                }
            }
        }

        public void AtualizarSaldoPreMovimentoItemPreMovimento(EstoquePreMovimento preMovimento, EstoquePreMovimentoItem preMovimentoItem)
        {
            if (preMovimento == null)
            {
                return;
            }
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var produtoSaldo = produtoSaldoRepository.Object
                    .GetAll().AsNoTracking().FirstOrDefault(w => w.ProdutoId == preMovimentoItem.ProdutoId 
                    && w.EstoqueId == preMovimento.EstoqueId && w.LoteValidadeId == null);

                if (produtoSaldo != null)
                {
                    if (preMovimento.IsEntrada)
                    {
                        produtoSaldo.QuantidadeEntradaPendente = ObterQuantidadeProduto(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada);
                    }
                    else
                    {
                        produtoSaldo.QuantidadeSaidaPendente = ObterQuantidadeProduto(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada);
                    }

                    produtoSaldoRepository.Object.Update(produtoSaldo);

                }
                else
                {
                    produtoSaldo = new ProdutoSaldo
                    {
                        EstoqueId = (long)preMovimento.EstoqueId,
                        ProdutoId = preMovimentoItem.ProdutoId
                    };

                    if (preMovimento.IsEntrada)
                    {
                        produtoSaldo.QuantidadeEntradaPendente = ObterQuantidadeProduto(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada);
                    }
                    else
                    {
                        this.Logger.Error($"AtualizarSaldoPreMovimentoItemPreMovimento - TENTATIVA DE CADASTRO DE PRODUTO SALDO:  PreMovimentoItemId {preMovimentoItem.Id} PreMovimentoId {preMovimentoItem.PreMovimentoId} EstoqueId {produtoSaldo.EstoqueId} ProdutoId {produtoSaldo.ProdutoId}");
                        produtoSaldo.QuantidadeSaidaPendente = ObterQuantidadeProduto(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada);
                    }

                    produtoSaldoRepository.Object.Insert(produtoSaldo);
                }
            }
        }

        public void AtualizarSaldoPreMovimentoItemLoteValidadePreMovimento(EstoquePreMovimentoLoteValidade preMovimentoItemLoteValidade)
        {
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                {
                    var preMovimentoItem = preMovimentoItemLoteValidade.EstoquePreMovimentoItem;

                    if (preMovimentoItem == null)
                    {
                        return;
                    }
                    
                    var preMovimento = estoquePreMovimentoRepository.Object.Get(preMovimentoItem.PreMovimentoId);

                    if (preMovimento != null)
                    {
                        var produtoSaldo = produtoSaldoRepository.Object
                            .GetAll().AsNoTracking().FirstOrDefault(w => w.ProdutoId == preMovimentoItem.ProdutoId
                                                          && w.EstoqueId == (long)preMovimento.EstoqueId
                                                          && w.LoteValidadeId == preMovimentoItemLoteValidade.LoteValidadeId);

                        if (produtoSaldo != null)
                        {
                            if (preMovimento.IsEntrada)
                            {
                                produtoSaldo.QuantidadeEntradaPendente = ObterQuantidadeProdutoLoteValidade(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada, preMovimentoItemLoteValidade.LoteValidadeId);
                            }
                            else
                            {
                                produtoSaldo.QuantidadeSaidaPendente = ObterQuantidadeProdutoLoteValidade(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada, preMovimentoItemLoteValidade.LoteValidadeId);
                            }

                            produtoSaldoRepository.Object.Update(produtoSaldo);
                        }
                        else
                        {
                            produtoSaldo = new ProdutoSaldo
                            {
                                EstoqueId = (long)preMovimento.EstoqueId,
                                ProdutoId = preMovimentoItem.ProdutoId,
                                LoteValidadeId = preMovimentoItemLoteValidade.LoteValidadeId
                            };

                            if (preMovimento.IsEntrada)
                            {
                                produtoSaldo.QuantidadeEntradaPendente = ObterQuantidadeProdutoLoteValidade(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada, preMovimentoItemLoteValidade.LoteValidadeId);
                            }
                            else
                            {
                                this.Logger.Error($"AtualizarSaldoPreMovimentoItemLoteValidadePreMovimento - TENTATIVA DE CADASTRO DE PRODUTO SALDO: EstoqueId {produtoSaldo.EstoqueId} ProdutoId {produtoSaldo.ProdutoId} LoteValidadeId {produtoSaldo.LoteValidadeId}");
                                produtoSaldo.QuantidadeSaidaPendente = ObterQuantidadeProdutoLoteValidade(preMovimentoItem.ProdutoId, (long)preMovimento.EstoqueId, preMovimento.IsEntrada, preMovimentoItemLoteValidade.LoteValidadeId);
                            }

                            produtoSaldoRepository.Object.Insert(produtoSaldo);
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<DefaultReturn<ValidaProdutoSaldoDto>> ValidaSaldoPorProdutoLoteValidadeEstoque(ValidaProdutoSaldoDto input)
        {
            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var result = new DefaultReturn<ValidaProdutoSaldoDto>
                {
                    ReturnObject = input,
                    Errors = new List<ErroDto>()
                };
                
                var produto = await produtoRepository.Object.GetAll().AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == input.ProdutoId);
                if (produto == null)
                {
                    result.Errors.Add(ErroDto.Criar(descricao: string.Format("Produto {0} não encontrado!", produto.Descricao)));
                    return result;
                }

                ProdutoSaldo produtoSaldo;
                if (produto.IsLote)
                {
                    produtoSaldo = await produtoSaldoRepository.Object.GetAll().AsNoTracking()
                        .FirstOrDefaultAsync(x =>
                            x.ProdutoId == input.ProdutoId && x.EstoqueId == input.EstoqueId &&
                            x.LoteValidadeId == input.LoteValidadeId);
                }
                else
                {
                    produtoSaldo = await produtoSaldoRepository.Object.GetAll().AsNoTracking()
                        .FirstOrDefaultAsync(x => x.ProdutoId == input.ProdutoId && x.EstoqueId == input.EstoqueId);
                }

                if (produtoSaldo == null)
                {
                    result.Errors.Add(ErroDto.Criar(descricao: string.Format("Não há saldo para o produto {0} !",produto.Descricao)));
                    return result;
                }

                if (!input.IsEntrada && produtoSaldo.QuantidadeAtual < input.Quantidade)
                {
                    result.Errors.Add(ErroDto.Criar(descricao: string.Format("Não há saldo suficiente para o produto {0} !", produto.Descricao)));
                }

                return result;
            }
        }

        #region Metodos auxiliares

        private static decimal CalcularQuantidade(bool isEntrada, decimal quantadade)
        {
            return (((isEntrada ? 1 : 0) * 2) - 1) * quantadade;
        }

        private static decimal ObterQuantidadeProduto(long produtoId, long estoqueId, bool isEntrada)
        {
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {
                var queryPreMovimento = from mov in estoquePreMovimentoRepository.Object.GetAll()
                    join item in estoquePreMovimentoItemRepository.Object.GetAll()
                        on mov.Id equals item.PreMovimentoId
                        into movjoin
                    from joinedmov in movjoin.DefaultIfEmpty()
                    where mov.EstoqueId == estoqueId
                          && joinedmov.ProdutoId == produtoId
                          && mov.IsEntrada == isEntrada
                          && mov.PreMovimentoEstadoId != 2
                    select new { Quantidade = joinedmov.Quantidade };

                return queryPreMovimento.ToList().Sum(s => s.Quantidade);
            }
        }
        
        private static decimal ObterQuantidadeProdutoLoteValidade(long produtoId, long estoqueId, bool isEntrada, long loteValidadeId)
        {
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {
                var queryPreMovimento = from mov in estoquePreMovimentoRepository.Object.GetAll()
                    join item in estoquePreMovimentoItemRepository.Object.GetAll()
                        on mov.Id equals item.PreMovimentoId
                    join iltv in estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                        on item.Id equals iltv.EstoquePreMovimentoItemId
                        into movjoin
                    from joinedmov in movjoin.DefaultIfEmpty()
                    where mov.EstoqueId == estoqueId
                          && item.ProdutoId == produtoId
                          && mov.IsEntrada == isEntrada
                          && joinedmov.LoteValidadeId == loteValidadeId
                          && mov.PreMovimentoEstadoId != 2
                    select new { Quantidade = joinedmov.Quantidade };

                return queryPreMovimento.ToList().Sum(s => s.Quantidade);
            }
        }
        

        #endregion
    }
}
