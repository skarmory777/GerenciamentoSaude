using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Dapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices
{
    public class EstoquePreMovimentoValidacaoDomainService : SWMANAGERDomainServiceBase, IEstoquePreMovimentoValidacaoDomainService
    {
        public async Task<List<ErroDto>> Validar(EstoquePreMovimentoDto preMovimento, List<EstoquePreMovimentoItemDto> itens, bool isValidaProdutoEmInventario = false)
        {
            var lista = new List<ErroDto>();

            ExisteConfirmacaoEntrada(preMovimento, lista);
            ExisteCentroDeCusto(preMovimento, lista);

            var idsProdutos = itens.Select(s => s.ProdutoId);

            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                var produtosDto = ProdutoDto.Mapear(produtoRepository.Object.GetAll().AsNoTracking().Where(w => idsProdutos.Any(a => a == w.Id)).ToList());

                foreach (var item in itens)
                {
                    ValidarItemProduto(item, lista);
                    item.Produto = produtosDto.FirstOrDefault(w => w.Id == item.ProdutoId);

                    ValidarGrupoEstoqueItem(item, (long)preMovimento.EstoqueId, lista);

                    if (!preMovimento.IsEntrada)
                    {
                        var validarDataVencimeneto = preMovimento.TipoDocumentoId != 4;
                        item.EstoqueId = (long)preMovimento.EstoqueId;
                        
                        await ValidarItemSaida(item, (preMovimento.EstTipoMovimentoId != (long)EnumTipoMovimento.Perda_Saida ? (item.Produto != null && (item.Produto.IsValidade || item.Produto.IsLote)) : false), validarDataVencimeneto, lista);
                    }

                    if (isValidaProdutoEmInventario)
                    {
                        ValidarProdutoEmInventario(item.ProdutoId, preMovimento.EstoqueId ?? 0, preMovimento.InventarioId ?? 0, lista);
                    }
                }

                await ValidarProdutoTotal(preMovimento, itens, lista);


            }
            return lista;
        }

        public async Task ValidarProdutoTotal(EstoquePreMovimentoDto preMovimento, List<EstoquePreMovimentoItemDto> itens, List<ErroDto> lista)
        {
            if (!preMovimento.IsEntrada)
            {
                using (var preMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {
                    var itemsParaValidar = itens.Where(x => x.PreMovimentoItemEstadoId == null
                        || x.PreMovimentoItemEstadoId == (long)EnumPreMovimentoEstado.Pendente
                        || x.PreMovimentoItemEstadoId == (long)EnumPreMovimentoEstado.ParcialmenteAtendido).ToList();

                    var produtosSemLoteValidade = itemsParaValidar.Where(x => !x.Produto.IsLote && !x.Produto.IsValidade)
                        .GroupBy(x => x.ProdutoId)
                        .Where(x => x.Count() > 1);

                    if (produtosSemLoteValidade.Any())
                    {
                        foreach (var item in produtosSemLoteValidade)
                        {
                            await ValidarProdutoSaldoTotalSemLoteValidade(item.Key, preMovimento.EstoqueId ?? 0, item.Sum(x => x.Quantidade), lista);
                        }
                    }

                    var produtosComLoteValidade = itemsParaValidar.Where(x => x.Produto.IsLote || x.Produto.IsValidade).GroupBy(x => x.ProdutoId);
                    if (produtosComLoteValidade.Any())
                    {
                        foreach (var produtoItem in produtosComLoteValidade)
                        {
                            var lotesQuantidades = new Dictionary<long, decimal>();
                            var preMovimentoLotesValidades = produtoItem.Where(preMovimentoItem => preMovimentoItem.PreMovimentoLotesValidades != null).SelectMany(preMovimentoItem => preMovimentoItem.PreMovimentoLotesValidades);
                            foreach (var loteValidade in preMovimentoLotesValidades)
                            {
                                if (!lotesQuantidades.ContainsKey(loteValidade.LoteValidadeId))
                                {
                                    lotesQuantidades.Add(loteValidade.LoteValidadeId, loteValidade.Quantidade);
                                }
                                else
                                {
                                    lotesQuantidades[loteValidade.LoteValidadeId] += loteValidade.Quantidade;
                                }
                            }

                            foreach (var lote in lotesQuantidades)
                            {
                                await ValidarProdutoSaldoTotalComLoteValidade(produtoItem.Key, preMovimento.EstoqueId ?? 0, lote.Key, lote.Value, lista);
                            }
                        }
                    }
                }
            }
        }

        public async Task ValidarItemSaida(EstoquePreMovimentoItemDto preMovimentoItem, bool validarLoteValidade, bool validarDataVencimeneto, List<ErroDto>  lista )
        {
            ValidarItem(preMovimentoItem, lista);
            await ValidarSeExisteProdutoNoEstoque(preMovimentoItem, lista);

            if (validarLoteValidade && !preMovimentoItem.PreMovimentoLotesValidades.IsNullOrEmpty())
            {
                using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
                {
                    foreach(var preMovimentoLotesValidade in preMovimentoItem.PreMovimentoLotesValidades)
                    {
                        ValidarLoteValidade(preMovimentoLotesValidade, validarDataVencimeneto, lista);
                    }
                }
            }
        }

        public List<ErroDto> ValidarItem(EstoquePreMovimentoItemDto preMovimentoItem)
        {
            var lista = new List<ErroDto>();
            ValidarItem(preMovimentoItem, lista);
            return lista;
        }

        public void ValidarItem(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            if (ExisteConfirmacaoEntrada(new EstoquePreMovimentoDto { Id = preMovimentoItem.PreMovimentoId }, lista))
            {
                lista.Add(ErroDto.Criar("EST0002", string.Format("Produto: {0} - Esta entrada já foi confirmada.", preMovimentoItem.Produto?.Descricao)));
            }

            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {
                var preMovimento = estoquePreMovimentoRepository.Object.GetAll().AsNoTracking().FirstOrDefault(w => w.Id == preMovimentoItem.PreMovimentoId);
                ValidarProduto(preMovimentoItem.ProdutoId, preMovimento, lista);
                ValidarUnidade(preMovimentoItem, lista);

            }
        }

        public bool ExisteLoteValidadePendente(EstoquePreMovimentoDto preMovimento)
        {
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            {
                var preMovimentosItem = estoquePreMovimentoItemRepository.Object
                    .GetAll()
                    .AsNoTracking().Where(w => w.PreMovimentoId == preMovimento.Id && (w.Produto.IsLote || w.Produto.IsValidade)).ToList();

                if (!preMovimentosItem.Any())
                {
                    return false;
                }

                var preMovimentosItemIds = preMovimentosItem.Select(x => x.Id).ToList();
                var queryLotesValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                    .AsNoTracking().Where(m => preMovimentosItemIds.Contains(m.EstoquePreMovimentoItemId));
                foreach (var item in preMovimentosItem)
                {
                    var lotesValidade = queryLotesValidade.Where(x => x.EstoquePreMovimentoItemId == item.Id).ToList();
                    var somaLoteValidade = lotesValidade.Sum(s => s.Quantidade);

                    if (somaLoteValidade != item.Quantidade)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public async Task<List<ErroDto>> ValidarSaidaEstoque(EstoquePreMovimento preMovimentoSaida)
        {
            var lista = new List<ErroDto>();
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            {
                var itensSaida = await estoquePreMovimentoItemRepository.Object.GetAll().AsNoTracking()
                    .Include(i => i.Produto)
                    .Include(i => i.ProdutoUnidade)
                    .Where(w => w.PreMovimentoId == preMovimentoSaida.Id).ToListAsync();

                foreach (var item in itensSaida)
                {
                    var itemSaidaDto = EstoquePreMovimentoItemDto.MapPreMovimentoItem(item);
                    itemSaidaDto.EstoqueId = (long)preMovimentoSaida.EstoqueId;

                    var loteValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll().AsNoTracking().FirstOrDefault(w => w.EstoquePreMovimentoItemId == item.Id);
                    if (loteValidade != null)
                    {
                        itemSaidaDto.LoteValidadeId = loteValidade.LoteValidadeId;
                    }
                    await ValidarSeExisteProdutoNoEstoque(itemSaidaDto, lista);
                }

                return lista;
            }
        }

        public List<ErroDto> ValidarBaixaVale(EstoqueMovimento movimentoBaixa, List<long> movimentoIds)
        {
            var lista = new List<ErroDto>();
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            {
                var movimentos = estoqueMovimentoRepository.Object.GetAll().AsNoTracking().Where(x => movimentoIds.Contains(x.Id)).ToList();
                if (movimentos.Sum(s => s.TotalDocumento) != movimentoBaixa.TotalDocumento)
                {
                    ErroDto erroDto = new ErroDto { CodigoErro = "BAX0001" };
                    lista.Add(erroDto);
                }

                if (movimentos.GroupBy(g => g.SisFornecedorId).Count() > 1)
                {
                    ErroDto erroDto = new ErroDto { CodigoErro = "BAX0002" };
                    lista.Add(erroDto);
                }
                else if (movimentos[0].SisFornecedorId != movimentoBaixa.SisFornecedorId)
                {
                    ErroDto erroDto = new ErroDto { CodigoErro = "BAX0003" };
                    lista.Add(erroDto);
                }
                return lista;
            }
        }
        // TODO: Não pode ficar assim quando for ver vale
        public List<ErroDto> ValidarFornecedoresBaixaVale(string baixasIds)
        {
            var lista = new List<ErroDto>();
            List<long> ids = new List<long>();
            List<EstoqueMovimento> movimentos = new List<EstoqueMovimento>();

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

                if (movimentos.GroupBy(g => g.SisFornecedorId).Count() > 1)
                {
                    ErroDto erroDto = new ErroDto { CodigoErro = "BAX0002" };
                    lista.Add(erroDto);
                }
                return lista;
            }
        }

        public List<ErroDto> ValidarItemDevolucao(EstoquePreMovimentoItemDto preMovimentoItem, bool validarLoteValidade, bool validarDataVencimeneto)
        {
            var lista = new List<ErroDto>();
            ValidarItem(preMovimentoItem, lista);
            ValidarSeExisteSaidaProduto(preMovimentoItem, lista);

            return lista;
        }

        #region Validações Internas
        private static void ValidarSeExisteSaidaProduto(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            if (preMovimentoItem.Produto.IsValidade || preMovimentoItem.Produto.IsLote)
            {
                if (preMovimentoItem.LoteValidadeId == null)
                {
                    using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                    {
                        var preMovimentoLoteValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll().AsNoTracking().FirstOrDefault(w => w.EstoquePreMovimentoItemId == preMovimentoItem.Id);

                        preMovimentoItem.LoteValidadeId = preMovimentoLoteValidade.LoteValidadeId;
                    }
                }
                ValidarSeExisteSaidaProdutoLoteValidade(preMovimentoItem, lista);
            }
            else
            {
                ValidarSeExisteSaidaProdutoSemLoteValidade(preMovimentoItem, lista);
            }
        }

        private async Task ValidarProdutoSaldoTotalSemLoteValidade(long produtoId, long estoqueId, decimal quantidade, ICollection<ErroDto> lista)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                var produtoSaldoQuantidadeAtual = await produtoSaldoRepository.Object
                    .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.EstoqueId == estoqueId);

                if (produtoSaldoQuantidadeAtual == null || produtoSaldoQuantidadeAtual.QuantidadeAtual < quantidade)
                {
                    var produto = await produtoRepository.Object.FirstOrDefaultAsync(x => x.Id == produtoId);
                    lista.Add(new ErroDto { CodigoErro = "EST0003", Descricao = string.Format("Quantidade do produto {0} é insuficiente nesse estoque informado.", produto?.Descricao) });
                }

            }
        }

        private async Task ValidarProdutoSaldoTotalComLoteValidade(long produtoId, long estoqueId, long loteValidadeId, decimal quantidade, ICollection<ErroDto> lista)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
            {
                var produtoSaldoQuantidadeAtual = await produtoSaldoRepository.Object
                    .FirstOrDefaultAsync(x => x.ProdutoId == produtoId && x.EstoqueId == estoqueId && x.LoteValidadeId == loteValidadeId);

                if (produtoSaldoQuantidadeAtual == null || produtoSaldoQuantidadeAtual.QuantidadeAtual < quantidade)
                {
                    var produto = await produtoRepository.Object.FirstOrDefaultAsync(x => x.Id == produtoId);
                    var loteValidade = await loteValidadeRepository.Object.FirstOrDefaultAsync(x => x.Id == loteValidadeId);
                    lista.Add(new ErroDto { CodigoErro = "EST0003", Descricao = string.Format("Quantidade do produto {0} é insuficiente nesse estoque no Lote {1}.", produto?.Descricao, loteValidade?.Lote) });
                }
            }
        }

        private async Task ValidarSeExisteProdutoNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            if (preMovimentoItem.Produto.IsValidade || preMovimentoItem.Produto.IsLote)
            {
                await ValidarSeExisteProdutoLoteValidadeNoEstoque(preMovimentoItem, lista);
            }
            else if (preMovimentoItem.Produto.IsSerie)
            {
                ValidarSeExisteProdutoNumeroSerieNoEstoque(preMovimentoItem, lista);
            }
            else
            {
                await ValidarSeExisteProdutoSemLoteValidadeNoEstoque(preMovimentoItem, lista);
            }
        }

        private async Task ValidarSeExisteProdutoSemLoteValidadeNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {

                var produtoSaldoQuantidadeAtual = await produtoSaldoRepository.Object
                    .FirstOrDefaultAsync(x => x.ProdutoId == preMovimentoItem.ProdutoId && x.EstoqueId == preMovimentoItem.EstoqueId);
                
                if (produtoSaldoQuantidadeAtual == null || produtoSaldoQuantidadeAtual.QuantidadeAtual < preMovimentoItem.Quantidade)
                {
                    lista.Add(new ErroDto { 
                        CodigoErro = "EST0003", 
                        Descricao = string.Format("Quantidade do produto {0} é insuficiente no estoque informado.", preMovimentoItem.Produto?.Descricao) 
                    });
                }                
            }
        }

        private static async Task ValidarSeExisteProdutoLoteValidadeNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                if (!preMovimentoItem.PreMovimentoLotesValidades.IsNullOrEmpty())
                {
                    foreach (var preMovimentoLoteValidade in preMovimentoItem.PreMovimentoLotesValidades)
                    {
                        var produtoSaldoQuantidadeAtual = await produtoSaldoRepository.Object.GetAll()
                            .AsNoTracking()
                            .FirstOrDefaultAsync(x => x.ProdutoId == preMovimentoItem.ProdutoId
                                && x.EstoqueId == preMovimentoItem.EstoqueId
                                && x.LoteValidadeId == preMovimentoLoteValidade.LoteValidadeId);


                        if (produtoSaldoQuantidadeAtual == null || produtoSaldoQuantidadeAtual.QuantidadeAtual < preMovimentoLoteValidade.Quantidade)
                        {
                            lista.Add(new ErroDto { CodigoErro = "EST0003", Descricao = string.Format("Quantidade do produto {0} é insuficiente nesse estoque no Lote/Validade informado.", preMovimentoItem.Produto?.Descricao) });
                        }
                    }
                }
            }
        }

        //TODO: Ver como resolver a questão do número de serie e produtoSaldo.
        private static void ValidarSeExisteProdutoNumeroSerieNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            // using (var estoqueMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoLoteValidade, long>>())
            {
                var query = from mov in estoqueMovimentoRepository.Object.GetAll()
                            join item in estoqueMovimentoItemRepository.Object.GetAll()
                                on mov.Id equals item.MovimentoId
                            where mov.EstoqueId == preMovimentoItem.EstoqueId
                                  && item.ProdutoId == preMovimentoItem.ProdutoId
                                  && item.NumeroSerie == preMovimentoItem.NumeroSerie
                            select new { Quantidade = (((mov.IsEntrada ? 1 : 0) * 2) - 1) };

                if (query.AsNoTracking().ToList().Count < preMovimentoItem.Quantidade)
                {
                    lista.Add(new ErroDto { CodigoErro = "EST0003", Descricao = string.Format("Não existe o produto {0} com o número de série {1} no estoque informado.", preMovimentoItem.Produto?.Descricao, preMovimentoItem.NumeroSerie) });
                }
            }
        }

        private static void ValidarLoteValidade(EstoquePreMovimentoLoteValidadeDto preMovimentoLoteValidade, bool validarDataVencimento, ICollection<ErroDto> lista)
        {
            if (validarDataVencimento && preMovimentoLoteValidade.LoteValidade?.Validade < DateTime.Now)
            {
                lista.Add(new ErroDto { CodigoErro = "LT0001", Descricao = string.Format("Validade {0} Vencida.", preMovimentoLoteValidade.LoteValidade.Lote) });
            }
        }

        private static bool ExisteConfirmacaoEntrada(EstoquePreMovimentoDto preMovimento, ICollection<ErroDto> lista)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            {
                var result = estoqueMovimentoRepository.Object.GetAll().AsNoTracking().Any(w => w.EstoquePreMovimentoId == preMovimento.Id);
                if (result)
                {
                    lista.Add(ErroDto.Criar("EST0002", "Esta entrada já foi confirmada."));
                }

                return result;
            }
        }

        private static bool ExisteCentroDeCusto(EstoquePreMovimentoDto preMovimento, ICollection<ErroDto> lista)
        {
            var result = preMovimento.CentroCustoId == null && preMovimento.TipoMovimentoId == (long)EnumTipoOperacao.Entrada;
            if (result)
            {
                lista.Add(ErroDto.Criar("EST0015"));
            }

            return result;
        }

        private static void ValidarGrupoEstoqueItem(EstoquePreMovimentoItemDto preMovimentoItem, long estoqueId, ICollection<ErroDto> lista)
        {
            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            using (var estoqueGrupoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueGrupo, long>>())
            {
                var query = from grupoEstoque in estoqueGrupoRepository.Object.GetAll()
                            join produto in produtoRepository.Object.GetAll()
                                on grupoEstoque.GrupoId equals produto.GrupoId
                            where produto.Id == preMovimentoItem.ProdutoId
                                  && grupoEstoque.EstoqueId == estoqueId
                            select produto;


                if (!query.AsNoTracking().Any())
                {
                    lista.Add(ErroDto.Criar("", string.Format("Grupo do produto {0} não esta relacionado ao estoque informado.", preMovimentoItem.Produto?.Descricao)));
                }
            }
        }

        private static void ValidarProduto(long produtoId, EstoquePreMovimento preMovimento, ICollection<ErroDto> lista)
        {
            if (produtoId == 0)
            {
                lista.Add(new ErroDto { CodigoErro = "EST0005", Descricao ="Produto obrigatório."});
                return;
            }

            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                var produto = produtoRepository.Object.GetAll()
                    .AsNoTracking().Include(i => i.ContaAdministrativa)
                    .Include(i => i.ContaAdministrativa.ContaAdministrativaEmpresas).FirstOrDefault(w => w.Id == produtoId);
                if (produto == null)
                {
                    lista.Add(new ErroDto { CodigoErro = "EST0006", Descricao = string.Format("Produto: {0} - Produto não cadastrado.", produto.Descricao) });
                    return;
                }

                if (produto.IsPrincipal)
                {
                    lista.Add(new ErroDto { CodigoErro = "EST0004", Descricao = string.Format("Produto: {0} - Não é permitido utilizar um produto mestre em entrada de produto.", produto.Descricao) });
                }

                if (produto.IsBloqueioCompra)
                {
                    lista.Add(new ErroDto { CodigoErro = "EST0009", Descricao = string.Format("Produto: {0} - Este produto não pode ser movimentado estoque.", produto.Descricao) });
                }

                if (!produto.IsAtivo)
                {
                    lista.Add(new ErroDto { CodigoErro = "EST0010", Descricao = string.Format("Produto: {0} - Produto inativo.", produto.Descricao) });
                }

                if (preMovimento.EstTipoMovimentoId != (long)EnumTipoMovimento.NotaFiscal_Entrada)
                {
                    return;
                }

                if (produto.ContaAdministrativaId == null || produto.ContaAdministrativaId == 0)
                {
                    lista.Add(new ErroDto { CodigoErro = "EST0011", Parametros = new List<object> { produto.Descricao } });
                    return;
                }

                if (!produto.ContaAdministrativa.IsDespesa)
                {
                    lista.Add(new ErroDto { CodigoErro = "EST0012", Parametros = new List<object> { produto.Descricao } });
                }

                var contaEmpresas = produto.ContaAdministrativa.ContaAdministrativaEmpresas.Where(w => w.EmpresaId == preMovimento.EmpresaId);

                if (!contaEmpresas.Any())
                {
                    lista.Add(new ErroDto { CodigoErro = "EST0013", Parametros = new List<object> { produto.Descricao } });
                }
            }
        }

        private static void ValidarUnidade(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            if (preMovimentoItem.ProdutoUnidadeId == null || preMovimentoItem.ProdutoUnidadeId == 0)
            {
                lista.Add(new ErroDto { CodigoErro = "EST0008", Descricao = string.Format("Produto: {0} - Unidade obrigatória.", preMovimentoItem.Produto?.Descricao) });
                return;
            }

            using (var produtoUnidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
            {
                var produtoUnidade = produtoUnidadeRepository.Object.GetAll().AsNoTracking().Where(w =>
                    w.ProdutoId == preMovimentoItem.ProdutoId && w.UnidadeId == preMovimentoItem.ProdutoUnidadeId);

                if (!produtoUnidade.Any())
                {
                    lista.Add(new ErroDto
                    { CodigoErro = "EST0007", Descricao = string.Format("Produto: {0} - Unidade não relacionada ao produto selecionado.", preMovimentoItem.Produto?.Descricao) });
                }
            }
        }


        private static void ValidarItemProduto(EstoquePreMovimentoItemDto item, ICollection<ErroDto> lista)
        {
            if (!string.IsNullOrEmpty(item.NumeroSerie) && item.Quantidade != 1)
            {
                lista.Add(ErroDto.Criar("EST0002", string.Format("Produto {0} com número de série deve ter a quantidade sempre 1.",item.Produto?.Descricao)));
            }
        }

        private void ValidarProdutoEmInventario(long produtoId, long estoqueId, long inventarioId, ICollection<ErroDto> lista)
        {
            var query = $@"
                SELECT est.Descricao AS Estoque, pro.DescricaoResumida AS Produto
                     FROM EstInventario inv
                         JOIN EstInventarioItem ivIt ON ivIt.InventarioId = inv.Id
                         JOIn Est_Estoque est ON est.Id = inv.EstoqueId
                         JOIN Est_Produto pro ON pro.Id = ivIt.ProdutoId
                     WHERE inv.EstoqueId = @EstoqueId
                       AND inv.StatusInventarioId <> {StatusInventario.Fechado}
                       AND ivIt.ProdutoId = @ProdutoId
                       AND inv.Id <> @InventarioId";

            try
            {

                using (var connection = new SqlConnection(this.GetConnection()))
                {
                    var produtosEstoque = connection.QueryAsync<EstoqueProduto>(query, new { EstoqueId = estoqueId, ProdutoId = produtoId, InventarioId = inventarioId });

                    var items = produtosEstoque.Result?.ToList();

                    if (lista != null && lista.Any())
                    {
                        lista.Add(new ErroDto { CodigoErro = "INV0002", Parametros = new List<object> { items[0].Produto, items[0].Estoque } });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        // TODO: Isso mudará conforme o produtoSaldoHistorico
        private static void ValidarSeExisteSaidaProdutoLoteValidade(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            using (var estoqueMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoLoteValidade, long>>())
            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {

                var query = from mov in estoqueMovimentoRepository.Object.GetAll()
                            join item in estoqueMovimentoItemRepository.Object.GetAll()
                                on mov.Id equals item.MovimentoId
                            join iltv in estoqueMovimentoLoteValidadeRepository.Object.GetAll()
                                on item.Id equals iltv.EstoqueMovimentoItemId
                                into movjoin
                            from joinedmov in movjoin.DefaultIfEmpty()
                            where mov.EstoqueId == preMovimentoItem.EstoqueId
                                  && item.ProdutoId == preMovimentoItem.ProdutoId
                                  && joinedmov.LoteValidadeId == preMovimentoItem.LoteValidadeId
                                  && mov.IsEntrada == false

                                  && mov.EstTipoMovimentoId == preMovimentoItem.EstoquePreMovimento.EstTipoMovimentoId
                                  && mov.EstoqueId == preMovimentoItem.EstoquePreMovimento.EstoqueId
                                  && (preMovimentoItem.EstoquePreMovimento.UnidadeOrganizacionalId == null || mov.UnidadeOrganizacionalId == preMovimentoItem.EstoquePreMovimento.UnidadeOrganizacionalId)
                                  && (preMovimentoItem.EstoquePreMovimento.AtendimentoId == null || mov.AtendimentoId == preMovimentoItem.EstoquePreMovimento.AtendimentoId)

                            select new { LoteValidade = joinedmov.LoteValidade, Quantidade = joinedmov.Quantidade };

                var queryPreMovimento = from mov in estoquePreMovimentoRepository.Object.GetAll()
                                        join item in estoquePreMovimentoItemRepository.Object.GetAll()
                                            on mov.Id equals item.PreMovimentoId
                                        join iltv in estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                                            on item.Id equals iltv.EstoquePreMovimentoItemId
                                        //into movjoin
                                        // from joinedmov in movjoin.DefaultIfEmpty()
                                        where //mov.EstoqueId == preMovimentoItem.EstoqueId
                                              // && item.ProdutoId == preMovimentoItem.ProdutoId
                                              // && joinedmov.LoteValidadeId == preMovimentoItem.LoteValidadeId
                                              //  && mov.IsEntrada == false
                        ///Todo: Verificar Regra sobre movimentações ainda não confirmadas
                        mov.Id == preMovimentoItem.PreMovimentoId
                                        //  && item.Id != preMovimentoItem.Id
                                        select new { LoteValidade = iltv.LoteValidade, Quantidade = iltv.Quantidade };



                var queryDevolucao = from mov in estoqueMovimentoRepository.Object.GetAll()
                                     join item in estoqueMovimentoItemRepository.Object.GetAll()
                                         on mov.Id equals item.MovimentoId
                                     join iltv in estoqueMovimentoLoteValidadeRepository.Object.GetAll()
                                         on item.Id equals iltv.EstoqueMovimentoItemId
                                         into movjoin
                                     from joinedmov in movjoin.DefaultIfEmpty()
                                     where mov.EstoqueId == preMovimentoItem.EstoqueId
                                           && item.ProdutoId == preMovimentoItem.ProdutoId
                                           && joinedmov.LoteValidadeId == preMovimentoItem.LoteValidadeId
                                           && mov.IsEntrada == true
                                           && mov.EstTipoOperacaoId == (long)EnumTipoOperacao.Devolucao

                                           && mov.EstTipoMovimentoId == preMovimentoItem.EstoquePreMovimento.EstTipoMovimentoId
                                           && mov.EstoqueId == preMovimentoItem.EstoquePreMovimento.EstoqueId
                                           && (preMovimentoItem.EstoquePreMovimento.UnidadeOrganizacionalId == null || mov.UnidadeOrganizacionalId == preMovimentoItem.EstoquePreMovimento.UnidadeOrganizacionalId)
                                           && (preMovimentoItem.EstoquePreMovimento.AtendimentoId == null || mov.AtendimentoId == preMovimentoItem.EstoquePreMovimento.AtendimentoId)

                                     select new { LoteValidade = joinedmov.LoteValidade, Quantidade = joinedmov.Quantidade };

                var queryList = query.AsNoTracking().ToList();
                var lotesValidade = queryList.GroupBy(g => g.LoteValidade).Select(s => new { LoteValidade = s.Key, Qtd = s.Sum(soma => soma.Quantidade) }).ToList();
                var quantidadeEstoque = lotesValidade.Sum(s => s.Qtd);


                var queryDevolucaoList = queryDevolucao.AsNoTracking().ToList();
                var lotesValidadeDevolucao = queryDevolucaoList.GroupBy(g => g.LoteValidade).Select(s => new { LoteValidade = s.Key, Qtd = s.Sum(soma => soma.Quantidade) }).ToList();
                var quantidadeEstoqueDevolucao = lotesValidadeDevolucao.Sum(s => s.Qtd);


                var queryPreMovimentoList = queryPreMovimento.AsNoTracking().ToList();
                var lotesValidadePreMovimentoDevolucao = queryPreMovimentoList.GroupBy(g => g.LoteValidade).Select(s => new { LoteValidade = s.Key, Qtd = s.Sum(soma => soma.Quantidade) }).ToList();
                var quantidadeEstoquePreMovimentoDevolucao = lotesValidadePreMovimentoDevolucao.Sum(s => s.Qtd);

                if ((quantidadeEstoque - quantidadeEstoqueDevolucao - quantidadeEstoquePreMovimentoDevolucao) < preMovimentoItem.Quantidade)
                {
                    lista.Add(new ErroDto { CodigoErro = "DEV0001" });
                }
            }
        }

        // TODO: Isso mudará conforme o produtoSaldoHistorico
        private static void ValidarSeExisteSaidaProdutoSemLoteValidade(EstoquePreMovimentoItemDto preMovimentoItem, ICollection<ErroDto> lista)
        {
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {

                var query = from mov in estoqueMovimentoRepository.Object.GetAll()
                            join item in estoqueMovimentoItemRepository.Object.GetAll()
                                on mov.Id equals item.MovimentoId
                                into movjoin
                            from joinedmov in movjoin.DefaultIfEmpty()
                            where mov.EstoqueId == preMovimentoItem.EstoqueId
                                  && joinedmov.ProdutoId == preMovimentoItem.ProdutoId
                                  && mov.IsEntrada == false
                                  && mov.EstTipoMovimentoId == preMovimentoItem.EstoquePreMovimento.EstTipoMovimentoId
                                  && (preMovimentoItem.EstoquePreMovimento.UnidadeOrganizacionalId == null || mov.UnidadeOrganizacionalId == preMovimentoItem.EstoquePreMovimento.UnidadeOrganizacionalId)
                                  && (preMovimentoItem.EstoquePreMovimento.PacienteId == null || mov.PacienteId == preMovimentoItem.EstoquePreMovimento.PacienteId)
                            select new { joinedmov.Quantidade };



                var queryPreMovimento = from mov in estoquePreMovimentoRepository.Object.GetAll()
                                        join item in estoquePreMovimentoItemRepository.Object.GetAll()
                                            on mov.Id equals item.PreMovimentoId
                                        where
                                            mov.Id == preMovimentoItem.PreMovimentoId
                                        select new { item.Quantidade };

                var queryDevolucao = from mov in estoqueMovimentoRepository.Object.GetAll()
                                     join item in estoqueMovimentoItemRepository.Object.GetAll()
                                         on mov.Id equals item.MovimentoId
                                     where mov.EstoqueId == preMovimentoItem.EstoqueId
                                           && item.ProdutoId == preMovimentoItem.ProdutoId
                                           && mov.IsEntrada == true
                                           && mov.EstTipoOperacaoId == (long)EnumTipoOperacao.Devolucao
                                           && mov.EstTipoMovimentoId == preMovimentoItem.EstoquePreMovimento.EstTipoMovimentoId
                                           && mov.EstoqueId == preMovimentoItem.EstoquePreMovimento.EstoqueId
                                           && (preMovimentoItem.EstoquePreMovimento.UnidadeOrganizacionalId == null || mov.UnidadeOrganizacionalId == preMovimentoItem.EstoquePreMovimento.UnidadeOrganizacionalId)
                                           && (preMovimentoItem.EstoquePreMovimento.PacienteId == null || mov.PacienteId == preMovimentoItem.EstoquePreMovimento.PacienteId)
                                     select new { item.Quantidade };


                var queryList = query.AsNoTracking().ToList();
                var quantidadeEstoque = queryList.Sum(s => s.Quantidade);

                var queryDevolucaoList = queryDevolucao.AsNoTracking().ToList();
                var quantidadeEstoqueDevolucao = queryDevolucaoList.Sum(s => s.Quantidade);

                var queryPreMovimentoList = queryPreMovimento.AsNoTracking().ToList();
                var quantidadeEstoquePreMovimentoDevolucao = queryPreMovimentoList.Sum(s => s.Quantidade);

                if ((quantidadeEstoque - quantidadeEstoqueDevolucao - quantidadeEstoquePreMovimentoDevolucao) < preMovimentoItem.Quantidade)
                {
                    lista.Add(new ErroDto { CodigoErro = "DEV0001" });
                }
            }
        }

        #endregion
    }
}