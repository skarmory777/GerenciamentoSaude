using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using NFe.Classes.Informacoes.Detalhe;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class EstoquePreMovimentoLoteValidadeAppService : SWMANAGERAppServiceBase, IEstoquePreMovimentoLoteValidadeAppService
    {
        public async Task<decimal> ObterQuantidadeRestanteLoteValidade(long preMovimentoItemId)
        {
            try
            {
                using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {
                    var lista = estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                                .Where(w => w.EstoquePreMovimentoItemId == preMovimentoItemId).ToList();

                    var qtd = lista.Sum(s => s.Quantidade);


                    var quantidadeTotal = estoquePreMovimentoItemRepository.Object.Get(preMovimentoItemId).Quantidade;

                    return quantidadeTotal - qtd;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<InformacaoLoteValidadeTodosDto> ObterLotesValidadesPreMovimento(long preMovimentoId, List<det> NFeItens)
        {
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoqueImportacaoProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueImportacaoProduto, long>>())
            {

                var queryPreMovimentoItem = from preMovimentoItem in estoquePreMovimentoItemRepository.Object.GetAll()
                                            join importacaoProduto in estoqueImportacaoProdutoRepository.Object.GetAll()
                                            on preMovimentoItem.ProdutoId equals importacaoProduto.ProdutoId
                                            into _importacaoProduto
                                            from importacaoProduto in _importacaoProduto.DefaultIfEmpty()
                                            join loteValidade in estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                                            on preMovimentoItem.Id equals loteValidade.EstoquePreMovimentoItemId
                                            into _loteValidadejoin
                                            from loteValidade in _loteValidadejoin.DefaultIfEmpty()
                                            where preMovimentoItem.PreMovimentoId == preMovimentoId
                                                  && (preMovimentoItem.Produto.IsValidade || preMovimentoItem.Produto.IsLote)
                                            select new InformacaoLoteValidadeTodosDto
                                            {
                                                ProdutoId = preMovimentoItem.ProdutoId,
                                                CodigoProdutoNota = importacaoProduto.CodigoProdutoNota,
                                                Lote = loteValidade.LoteValidade.Lote,
                                                Validade = loteValidade.LoteValidade.Validade,
                                                LaboratorioId = loteValidade.LoteValidade.EstLaboratorioId,
                                                Laboratorio = new ProdutoLaboratorioDto
                                                {
                                                    Id = loteValidade.LoteValidade.EstoqueLaboratorio != null ? loteValidade.LoteValidade.EstoqueLaboratorio.Id : 0
                                                                 ,
                                                    Descricao = loteValidade.LoteValidade.EstoqueLaboratorio != null ? loteValidade.LoteValidade.EstoqueLaboratorio.Descricao : string.Empty
                                                },
                                                EstoquePreMovimentoLoteValidadeId = loteValidade.Id,
                                                DescricaoProduto = preMovimentoItem.Produto.Descricao,
                                                PreMovimentoItemId = preMovimentoItem.Id,
                                                Quantidade = preMovimentoItem.Quantidade
                                            };

                var lista = queryPreMovimentoItem.ToList();
                int i = 0;
                foreach (var item in lista)
                {
                    item.Index = i++;
                    if (NFeItens != null && NFeItens.Count() > 0)
                    {
                        item.DescricaoProdutoNota = NFeItens.Where(w => w.prod.cProd == item.CodigoProdutoNota).FirstOrDefault().prod.xProd;
                    }
                }

                return lista;
            }
        }

        public DefaultReturn<InformacaoLoteValidadeTodosDto> AtualizarLotesValidades(List<InformacaoLoteValidadeTodosDto> lotesValidades)
        {
            var _retornoPadrao = new DefaultReturn<InformacaoLoteValidadeTodosDto>();
            _retornoPadrao.Errors = new List<ErroDto>();

            try
            {


                using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    foreach (var item in lotesValidades)
                    {
                        var loteValidade = loteValidadeRepository.Object
                            .GetAll()
                            .FirstOrDefault(w => w.Lote == item.Lote
                                                 && w.Validade == item.Validade
                                                 && w.EstLaboratorioId == item.LaboratorioId
                                                 && w.ProdutoId == item.ProdutoId);

                        LoteValidade _loteValidade;
                        if (loteValidade != null)
                        {
                            _loteValidade = loteValidade;
                        }
                        else
                        {
                            _loteValidade = new LoteValidade
                            {
                                Lote = item.Lote,
                                Validade = (DateTime)item.Validade,
                                EstLaboratorioId = item.LaboratorioId,
                                ProdutoId = item.ProdutoId
                            };
                            _loteValidade.Id = loteValidadeRepository.Object.InsertAndGetId(_loteValidade);
                        }

                        EstoquePreMovimentoLoteValidade preMovimentoLoteValidade;
                        if (item.EstoquePreMovimentoLoteValidadeId != null)
                        {
                            preMovimentoLoteValidade = estoquePreMovimentoLoteValidadeRepository.Object.Get((long)item.EstoquePreMovimentoLoteValidadeId);
                            preMovimentoLoteValidade.LoteValidadeId = _loteValidade.Id;

                            estoquePreMovimentoLoteValidadeRepository.Object.Update(preMovimentoLoteValidade);
                        }
                        else
                        {
                            preMovimentoLoteValidade = new EstoquePreMovimentoLoteValidade
                            {
                                LoteValidadeId = _loteValidade.Id,
                                EstoquePreMovimentoItemId = item.PreMovimentoItemId
                            };
                            preMovimentoLoteValidade.LoteValidadeId = _loteValidade.Id;
                            preMovimentoLoteValidade.Quantidade = item.Quantidade;

                            estoquePreMovimentoLoteValidadeRepository.Object.Insert(preMovimentoLoteValidade);
                        }

                        var estoquePreMovimentoLoteValidadeDto = new EstoquePreMovimentoLoteValidadeDto
                        {
                            LoteValidadeId = preMovimentoLoteValidade.LoteValidadeId,
                            EstoquePreMovimentoItemId = preMovimentoLoteValidade.EstoquePreMovimentoItemId,
                            Quantidade = preMovimentoLoteValidade.Quantidade
                        };

                        produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItemLoteValidade(estoquePreMovimentoLoteValidadeDto);
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
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
