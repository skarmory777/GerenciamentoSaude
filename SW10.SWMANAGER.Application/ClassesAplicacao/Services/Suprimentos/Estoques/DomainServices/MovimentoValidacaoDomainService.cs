using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Validacoes;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices
{
    public class MovimentoValidacaoDomainService : SWMANAGERDomainServiceBase, IMovimentoValidacaoDomainService
    {
        public List<ErroDto> ValidarFornecedoresBaixaVale(string baixasIds)
        {
            var ids = new List<long>();
            var movimentos = new List<EstoqueMovimento>();
            var lista = new List<ErroDto>();

            if (string.IsNullOrEmpty(baixasIds) || !baixasIds.TrimEnd('-').Split('-').Any())
            {
                lista.Add(ErroDto.Criar("BAX0004"));
            }
            else
            {
                baixasIds.TrimEnd('-').Split('-').ToList().ForEach(f => ids.Add(long.Parse(f)));

                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                {
                    foreach (var id in ids)
                    {
                        var movimento = estoqueMovimentoRepository.Object.Get(id);

                        if (movimento != null)
                        {
                            movimentos.Add(movimento);
                        }
                    }

                    if (movimentos.GroupBy(g => g.SisFornecedorId).Count() <= 1)
                    {
                        return lista;
                    }

                    lista.Add(ErroDto.Criar("BAX0002"));
                }
            }

            return lista;
        }

        public List<ErroDto> ValidarFornecedoresMovimentosItemBaixaConsignados(string baixasItensIds)
        {
            var ids = new List<long>();
            var movimentos = new List<EstoqueMovimento>();
            var lista = new List<ErroDto>();

            try
            {
                if (string.IsNullOrEmpty(baixasItensIds) || !baixasItensIds.TrimEnd('-').Split('-').Any())
                {
                    lista.Add(ErroDto.Criar("BAX0004"));
                }
                else
                {
                    baixasItensIds.TrimEnd('-').Split('-').ToList().ForEach(f => ids.Add(long.Parse(f)));

                    using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                    using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
                    {
                        var query = from mov in estoqueMovimentoRepository.Object.GetAll()
                                    join item in estoqueMovimentoItemRepository.Object.GetAll()
                                    on mov.Id equals item.MovimentoId
                                    into movjoin
                                    from joinedmov in movjoin.DefaultIfEmpty()
                                    where ids.Any(a => a == joinedmov.Id)
                                    select mov;

                        movimentos = query.ToList();

                        if (movimentos.GroupBy(g => g.SisFornecedorId).Count() <= 1)
                        {
                            return lista;
                        }

                        lista.Add(ErroDto.Criar("BAX0002"));
                    }
                }
            }
            catch (Exception)
            {

            }
            return lista;
        }

        public List<ErroDto> ValidarConfirmacaoSolicitacao(EstoquePreMovimentoDto preMovimento)
        {
            var Lista = new List<ErroDto>();

            //TODO: Precisamos rever isso.
            if (preMovimento.EstTipoOperacaoId == (long)EnumTipoOperacao.Devolucao)
            {
                return Lista;
            }

            var itens = MontarListarItens(preMovimento.Itens);

            foreach (var item in itens)
            {
                item.EstoqueId = (long)preMovimento.EstoqueId;
                ValidarSeExisteProdutoNoEstoqueSolicitacao(item, Lista);
            }

            return Lista;
        }


        /// <summary>
        /// Montar Listar Itens
        /// </summary>
        /// <param name="json">Estoque PreMovimentoItem Solicitacao Dto </param>
        /// <returns></returns>
        private static IEnumerable<EstoquePreMovimentoItemDto> MontarListarItens(string json)
        {
            var preMovimentoItens = new List<EstoquePreMovimentoItemDto>();
            var statusAtendimento = new List<long> { (long)EnumPreMovimentoEstado.TotalmenteAtendido, (long)EnumPreMovimentoEstado.TotalmenteSuspenso };
            var itens = JsonConvert.DeserializeObject<List<EstoquePreMovimentoItemSolicitacaoDto>>(json);

            if (itens == null || itens.Count == 0)
            {
                return preMovimentoItens;
            }

            foreach (var item in itens.Where(x => !statusAtendimento.Contains(x.EstadoSolicitacaoItemId)))
            {
                if (item.QuantidadeAtendida == null || item.QuantidadeAtendida == 0)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(item.NumerosSerieJson))
                {
                    var numerosSerie = JsonConvert.DeserializeObject<List<NumeroSerieGridDto>>(item.NumerosSerieJson);

                    foreach (var numeroSerie in numerosSerie)
                    {
                        var preMovimentoItemDto = new EstoquePreMovimentoItemDto
                        {
                            ProdutoId = item.ProdutoId,
                            ProdutoUnidadeId = item.ProdutoUnidadeId,
                            Quantidade = 1,
                            NumeroSerie = numeroSerie.NumeroSerie,
                            Produto = new ProdutoDto { Descricao = item.Produto, IsSerie = true }
                        };
                        preMovimentoItens.Add(preMovimentoItemDto);
                    }
                }
                else if (item.IsLote)
                {
                    var preMovimentoItemDto = new EstoquePreMovimentoItemDto
                    {
                        ProdutoId = item.ProdutoId,
                        ProdutoUnidadeId = item.ProdutoUnidadeId,
                        Quantidade = (decimal)item.QuantidadeAtendida,
                        Produto = new ProdutoDto()
                    };
                    preMovimentoItemDto.Produto = new ProdutoDto { Descricao = item.Produto };

                    preMovimentoItens.Add(preMovimentoItemDto);

                    var lotesValidades = JsonConvert.DeserializeObject<List<LoteValidadeGridDto>>(item.LotesValidadesJson, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                    if (lotesValidades == null)
                    {
                        continue;
                    }

                    preMovimentoItemDto.Produto.IsLote = true;
                    preMovimentoItemDto.PreMovimentoLotesValidades = new List<EstoquePreMovimentoLoteValidadeDto>();

                    foreach (var itemLoteValidade in lotesValidades.Where(x=> x.Id == 0))
                    {
                        var loteValidade = new LoteValidadeDto
                        {
                            Lote = itemLoteValidade.Lote,
                            ProdutoId = preMovimentoItemDto.ProdutoId,
                            Validade = itemLoteValidade.Validade,
                            ProdutoLaboratorioId = itemLoteValidade.LaboratorioId,
                            Id = itemLoteValidade.LoteValidadeId
                        };

                        var preMovimentoLoteValidadeDto = new EstoquePreMovimentoLoteValidadeDto
                        {
                            LoteValidadeId = loteValidade.Id,
                            Quantidade = (decimal)itemLoteValidade.Quantidade,
                            LoteValidade = loteValidade
                        };

                        preMovimentoItemDto.PreMovimentoLotesValidades.Add(preMovimentoLoteValidadeDto);
                    }
                }
                else
                {
                    var preMovimentoItemDto = new EstoquePreMovimentoItemDto
                    {
                        ProdutoId = item.ProdutoId,
                        ProdutoUnidadeId = item.ProdutoUnidadeId,
                        Quantidade = item.QuantidadeAtendida.Value,
                        Produto = new ProdutoDto { Descricao = item.Produto}
                    };
                    preMovimentoItens.Add(preMovimentoItemDto);
                }
            }

            return preMovimentoItens;
        }


        /// <summary>
        /// Validar Se Existe Produto No Estoque Solicitacao
        /// </summary>
        /// <param name="preMovimentoItem">pre Movimento Item</param>
        /// <param name="lista"> lista de erros</param>
        private static void ValidarSeExisteProdutoNoEstoqueSolicitacao(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            ValidarUnidade(preMovimentoItem, lista);
            if (preMovimentoItem.Produto.IsValidade || preMovimentoItem.Produto.IsLote)
            {
                foreach (var item in preMovimentoItem.PreMovimentoLotesValidades)
                {
                    preMovimentoItem.LoteValidadeId = item.LoteValidadeId;
                    ValidarQuantidadeAtendida(item, preMovimentoItem.Produto.Descricao, lista);
                    ValidarSeExisteProdutoLoteValidadeNoEstoque(item, preMovimentoItem.EstoqueId, preMovimentoItem.Produto.Descricao, lista);
                }
            }
            else if (preMovimentoItem.Produto.IsSerie)
            {
                ValidarSeExisteProdutoNumeroSerieNoEstoque(preMovimentoItem, lista);
            }
            else
            {
                ValidarSeExisteProdutoNoEstoque(preMovimentoItem, lista);
            }

        }

        private static void ValidarUnidade(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            if (preMovimentoItem.ProdutoUnidadeId == null || preMovimentoItem.ProdutoUnidadeId == 0)
            {
                lista.Add(new ErroDto { CodigoErro = "EST0008", Descricao = "Unidade obrigatória." });
                return;
            }

            using (var produtoUnidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
            {
                var produtoUnidade = produtoUnidadeRepository.Object.GetAll().AsNoTracking().Where(w =>
                    w.ProdutoId == preMovimentoItem.ProdutoId && w.UnidadeId == preMovimentoItem.ProdutoUnidadeId);

                if (!produtoUnidade.Any())
                {
                    lista.Add(new ErroDto
                    { CodigoErro = "EST0007", Descricao = "Unidade não relacionada ao produto selecionado." });
                }
            }
        }


        /// <summary>
        /// Validar Quantidade Atendida
        /// </summary>
        /// <param name="preMovimentoItem">pre Movimento Item</param>
        /// <param name="lista"> lista de erros</param>
        private static void ValidarQuantidadeAtendida(EstoquePreMovimentoLoteValidadeDto preMovimentoLoteValidade, string produto, ICollection<ErroDto> lista)
        {
            if (preMovimentoLoteValidade.Quantidade < 0)
            {
                lista.Add(ErroDto.Criar("EST0004", $"Quantidade do produto  {produto} atendido é maior que a quantidade solicitada."));
            }
        }

        private static void ValidarQuantidadeAtendida(EstoquePreMovimentoItemDto preMovimentoItem, string produto, ICollection<ErroDto> lista)
        {
            if (preMovimentoItem.QuantidadeAtendida < 0)
            {
                lista.Add(ErroDto.Criar("EST0004", $"Quantidade do produto  {produto} atendido é maior que a quantidade solicitada."));
            }
        }

        /// <summary>
        /// Validar Se Existe Produto Sem Lote Validade No Estoque
        /// </summary>
        /// <param name="preMovimentoItem">pre Movimento Item</param>
        /// <param name="lista"> lista de erros</param>
        private static void ValidarSeExisteProdutoNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var produtoSaldo = produtoSaldoRepository.Object.FirstOrDefault(x => x.ProdutoId == preMovimentoItem.ProdutoId && x.EstoqueId == preMovimentoItem.EstoqueId);
                
                if (produtoSaldo == null || produtoSaldo.QuantidadeAtual < preMovimentoItem.Quantidade)
                {
                    var produtoNome = $" <b>Produto: {preMovimentoItem.Produto.Descricao} </b>";
                    lista.Add(ErroDto.Criar("EST0003", $"{produtoNome} quantidade de produto insuficiente."));
                }
            }
        }

        /// <summary>
        /// Validar Se Existe Produto Numero Serie No Estoque
        /// </summary>
        /// <param name="preMovimentoItem">pre Movimento Item</param>
        /// <param name="lista"> lista de erros</param>
        private static void ValidarSeExisteProdutoNumeroSerieNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            {
                var query = from mov in estoqueMovimentoRepository.Object.GetAll()
                            join item in estoqueMovimentoItemRepository.Object.GetAll()
                            on mov.Id equals item.MovimentoId
                            //into movjoin
                            //from joinedmov in movjoin.DefaultIfEmpty()
                            where mov.EstoqueId == preMovimentoItem.EstoqueId
                               && item.ProdutoId == preMovimentoItem.ProdutoId
                               && item.NumeroSerie == preMovimentoItem.NumeroSerie
                            select new { Quantidade = ((((mov.IsEntrada ? 1 : 0) * 2) - 1) * item.Quantidade) };



                var queryList = query.ToList();

                var lotesValidade = queryList.Sum(s => s.Quantidade);

                if (lotesValidade < preMovimentoItem.Quantidade)
                {
                    lista.Add(ErroDto.Criar("EST0003", $"Produto {preMovimentoItem.Produto.Descricao} com número de série {preMovimentoItem.NumeroSerie} não disponível no estoque informado."));
                }

            }
        }

        /// <summary>
        /// Validar Se Existe Produto Lote Validade No Estoque
        /// </summary>
        /// <param name="preMovimentoItem">pre Movimento Item</param>
        /// <param name="lista"> lista de erros</param>
        private static void ValidarSeExisteProdutoLoteValidadeNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var produtoSaldo = produtoSaldoRepository.Object.FirstOrDefault(x => x.LoteValidadeId == preMovimentoItem.LoteValidadeId && x.EstoqueId == preMovimentoItem.EstoqueId);
                var produtoNome = preMovimentoItem.Produto != null ? $" <b>Produto: {preMovimentoItem.Produto.Descricao} </b>" : "";
                var quantidade = preMovimentoItem.PreMovimentoLotesValidades.Where(x => x.LoteValidadeId == preMovimentoItem.LoteValidadeId).Sum(x => x.Quantidade);
                if (produtoSaldo.QuantidadeAtual < quantidade)
                {
                    lista.Add(ErroDto.Criar("EST0003", $"{produtoNome} Quantidade de produto insuficiente nesse estoque no Lote/Validade informado."));
                }
            }
        }

        private static void ValidarSeExisteProdutoLoteValidadeNoEstoque(EstoquePreMovimentoLoteValidadeDto preMovimentoLoteValidade, long estoqueId, string produto, ICollection<ErroDto> lista)
        {

            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var produtoSaldo = produtoSaldoRepository.Object.FirstOrDefault(x => x.LoteValidadeId == preMovimentoLoteValidade.LoteValidadeId && x.EstoqueId == estoqueId);
                var produtoNome = !string.IsNullOrEmpty(produto) ? $" <b>Produto: {produto} </b>" : "";
                if (produtoSaldo == null || produtoSaldo.QuantidadeAtual < preMovimentoLoteValidade.Quantidade)
                {
                    lista.Add(ErroDto.Criar("EST0003", $"{produtoNome} Quantidade de produto insuficiente nesse estoque no Lote/Validade informado."));
                }
            }
        }
    }
}
