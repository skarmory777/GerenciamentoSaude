using Abp.Dependency;
using Abp.Domain.Repositories;
using Dapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Validacoes
{
    public class PreMovimentoValidacaoService : SWMANAGERAppServiceBase
    {
        public bool IsValidaProdutoEmInventario { get; set; }

        private static List<long> TipoFretesSemICMS = new List<long> {
            0, // Contratação do Frete por conta do Remetente
            1,
            3, // Transporte Próprio por conta do Remetente
            4, // Transporte Próprio por conta do Destinatário
            5, // Transporte Próprio por conta do Destinatário
            6, // Sem Ocorrência de transporte. 
            9  // Sem Ocorrência de transporte.
        };

        List<ErroDto> Lista = new List<ErroDto>();
        public List<ErroDto> Validar(EstoquePreMovimentoDto preMovimento, List<EstoquePreMovimentoItemDto> itens, bool isValidaProdutoEmInventario = false)
        {
            IsValidaProdutoEmInventario = isValidaProdutoEmInventario;

            if (ExisteConfirmacaoEntrada(preMovimento))
            {
                Lista.Add(new ErroDto { CodigoErro = "EST0002", Descricao = "Esta entrada já foi confirmada." });
            }

            if (preMovimento.CentroCustoId == null && preMovimento.TipoMovimentoId == (long)EnumTipoOperacao.Entrada)
            {
                Lista.Add(new ErroDto { CodigoErro = "EST0015" });
            }

            //ValidarTotalProdutos(preMovimento);
           //ValidarTotalDocumento(preMovimento, itens);

            var idsProdutos = itens.Select(s => s.ProdutoId);

            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            {
                var produtosDto = ProdutoDto.Mapear(produtoRepository.Object.GetAll().AsNoTracking().Where(w => idsProdutos.Any(a => a == w.Id)).ToList());

                foreach (var item in itens)
                {
                    ValidarItemProduto(item);
                    item.Produto = produtosDto.Where(w => w.Id == item.ProdutoId).FirstOrDefault(); //_produtoRepository.Get(item.ProdutoId);
                                                                                                    //  item.Produto = ProdutoDto.Mapear(produto);//.MapTo<ProdutoDto>();

                    ValidarGrupoEstoqueItem(item, (long)preMovimento.EstoqueId);

                    if (!preMovimento.IsEntrada)
                    {
                        var validarDataVencimeneto = preMovimento.TipoDocumentoId != 4;
                        item.EstoqueId = (long)preMovimento.EstoqueId;

                        ValidarItemSaida(item, (item.Produto != null && (item.Produto.IsValidade || item.Produto.IsLote)), validarDataVencimeneto);
                    }

                    if (IsValidaProdutoEmInventario)
                    {
                        ValidarProdutoEmInventario(item.ProdutoId, preMovimento.EstoqueId ?? 0, preMovimento.InventarioId ?? 0);
                    }
                }
            }

            return Lista;
        }

        public void ValidarItemProduto(EstoquePreMovimentoItemDto item)
        {
            if (!string.IsNullOrEmpty(item.NumeroSerie) && item.Quantidade != 1)
            {
                Lista.Add(new ErroDto { CodigoErro = "EST0002", Descricao = "Para produtos com número de série deve ser sempre 1." });
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

                if (preMovimentosItem.Any())
                {
                    var preMovimentosItemIds = preMovimentosItem.Select(x => x.Id).ToList();
                    var queryLotesValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll().AsNoTracking().Where(m => preMovimentosItemIds.Contains(m.EstoquePreMovimentoItemId));
                    foreach (var item in preMovimentosItem)
                    {
                        var lotesValidade = queryLotesValidade.Where(x => x.EstoquePreMovimentoItemId == item.Id).ToList();

                        var somaLoteValidade = lotesValidade.Sum(s => s.Quantidade);

                        if (somaLoteValidade != item.Quantidade)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        private static bool ExisteConfirmacaoEntrada(EstoquePreMovimentoDto preMovimento)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            {
                return estoqueMovimentoRepository.Object.GetAll().AsNoTracking().Any(w => w.EstoquePreMovimentoId == preMovimento.Id);
            }
        }

        private List<ErroDto> ValidarItemSaida(EstoquePreMovimentoItemDto preMovimentoItem, bool validarLoteValidade, bool validarDataVencimeneto)
        {
            ValidarItem(preMovimentoItem);
            ValidarSeExisteProdutoNoEstoque(preMovimentoItem);

            if (!validarLoteValidade || preMovimentoItem.LoteValidadeId == null)
            {
                return Lista;
            }
            
            using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
            {
                var loteValidade = loteValidadeRepository.Object.Get((long)preMovimentoItem.LoteValidadeId);

                ValidarLoteValidade(new EstoquePreMovimentoLoteValidadeDto
                    {
                        Lote = loteValidade.Lote
                        ,
                        Validade = loteValidade.Validade
                        ,
                        LaboratorioId = loteValidade.EstLaboratorioId
                        ,
                        EstoquePreMovimentoItemId = preMovimentoItem.Id
                    }
                    , validarDataVencimeneto);
            }
            return Lista;
        }

        private List<ErroDto> ValidarItem(EstoquePreMovimentoItemDto preMovimentoItem)
        {
            if (ExisteConfirmacaoEntrada(new EstoquePreMovimentoDto { Id = preMovimentoItem.PreMovimentoId }))
            {
                Lista.Add(new ErroDto { CodigoErro = "EST0002", Descricao = "Esta entrada já foi confirmada." });
            }

            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            {
                var preMovimento = estoquePreMovimentoRepository.Object.GetAll()
                    .AsNoTracking()
                    .FirstOrDefault(w => w.Id == preMovimentoItem.PreMovimentoId);
                ValidarProduto(preMovimentoItem.ProdutoId, preMovimento);
                ValidarUnidade(preMovimentoItem);

                return Lista;
            }
        }

        private void ValidarProduto(long produtoId, EstoquePreMovimento preMovimento)
        {
            if (produtoId == 0)
            {
                Lista.Add(new ErroDto { CodigoErro = "EST0005", Descricao = "Produto obrigatório." });
            }
            else
            {
                using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                {
                    var produto = produtoRepository.Object.GetAll()
                        .AsNoTracking()
                        .Include(i => i.ContaAdministrativa)
                        .Include(i => i.ContaAdministrativa.ContaAdministrativaEmpresas)
                        .FirstOrDefault(w => w.Id == produtoId);
                    if (produto == null)
                    {
                        Lista.Add(new ErroDto { CodigoErro = "EST0006", Descricao = "Produto não cadastrado." });
                    }
                    else
                    {
                        if (produto.IsPrincipal)
                        {
                            Lista.Add(new ErroDto { CodigoErro = "EST0004", Descricao = "Não é permitido utilizar um produto mestre em entrada de produto." });
                        }

                        if (produto.IsBloqueioCompra)
                        {
                            Lista.Add(new ErroDto { CodigoErro = "EST0009", Descricao = "Este produto não movimento estoque." });
                        }

                        if (!produto.IsAtivo)
                        {
                            Lista.Add(new ErroDto { CodigoErro = "EST0010", Descricao = "Produto inativo." });
                        }

                        if (preMovimento.EstTipoMovimentoId != (long) EnumTipoMovimento.NotaFiscal_Entrada)
                        {
                            return;
                        }
                        
                        if (produto.ContaAdministrativaId == null || produto.ContaAdministrativaId == 0)
                        {
                            Lista.Add(new ErroDto { CodigoErro = "EST0011", Parametros = new List<object> { produto.Descricao } });
                        }
                        else
                        {
                            if (!produto.ContaAdministrativa.IsDespesa)
                            {
                                Lista.Add(new ErroDto { CodigoErro = "EST0012", Parametros = new List<object> { produto.Descricao } });
                            }

                            var contaEmpresas = produto.ContaAdministrativa.ContaAdministrativaEmpresas.Where(w => w.EmpresaId == preMovimento.EmpresaId);

                            if (contaEmpresas == null || !contaEmpresas.Any())
                            {
                                Lista.Add(new ErroDto { CodigoErro = "EST0013", Parametros = new List<object> { produto.Descricao } });
                            }
                        }
                    }
                }
            }
        }

        private void ValidarUnidade(EstoquePreMovimentoItemDto preMovimentoItem)
        {
            if (preMovimentoItem.ProdutoUnidadeId == null || preMovimentoItem.ProdutoUnidadeId == 0)
            {
                Lista.Add(new ErroDto { CodigoErro = "EST0008", Descricao = "Unidade obrigatória." });
            }
            else
            {
                using (var produtoUnidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
                {
                    var produtoUnidade = produtoUnidadeRepository.Object.GetAll().AsNoTracking().Where(w => w.ProdutoId == preMovimentoItem.ProdutoId && w.UnidadeId == preMovimentoItem.ProdutoUnidadeId);

                    if (produtoUnidade == null || !produtoUnidade.Any())
                    {
                        Lista.Add(new ErroDto { CodigoErro = "EST0007", Descricao = "Unidade não relacionada ao produto selecionado." });
                    }
                }
            }
        }

        private List<ErroDto> ValidarLoteValidade(EstoquePreMovimentoLoteValidadeDto loteValidade, bool validarDataVencimento)
        {
            if (validarDataVencimento && loteValidade.Validade < DateTime.Now)
            {
                Lista.Add(new ErroDto { CodigoErro = "LT0001", Descricao = "Validade Vencida." });
            }
            return Lista;
        }

        private void ValidarSeExisteProdutoNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem)
        {
            if (preMovimentoItem.Produto.IsValidade || preMovimentoItem.Produto.IsLote)
            {
                if (preMovimentoItem.LoteValidadeId == null)
                {
                    using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                    {
                        var preMovimentoLoteValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll().AsNoTracking().Where(w => w.EstoquePreMovimentoItemId == preMovimentoItem.Id).FirstOrDefault();

                        preMovimentoItem.LoteValidadeId = preMovimentoLoteValidade?.LoteValidadeId;
                    }
                }

                ValidarSeExisteProdutoLoteValidadeNoEstoque(preMovimentoItem);
            }
            else if (preMovimentoItem.Produto.IsSerie)
            {
                ValidarSeExisteProdutoNumeroSerieNoEstoque(preMovimentoItem);
            }
            else
            {
                ValidarSeExisteProdutoSemLoteValidadeNoEstoque(preMovimentoItem);
            }
        }

        private void ValidarSeExisteProdutoLoteValidadeNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var produtoSaldoQuantidadeAtual = produtoSaldoRepository.Object.GetAll()
                    .Include(x => x.Produto)
                    .AsNoTracking()
                    .Where(x => x.ProdutoId == preMovimentoItem.ProdutoId && x.EstoqueId == preMovimentoItem.EstoqueId && x.LoteValidadeId == preMovimentoItem.LoteValidadeId)
                    .Sum(x => (decimal?)x.QuantidadeAtual) ?? 0;

                if (produtoSaldoQuantidadeAtual < preMovimentoItem.Quantidade)
                {
                    Lista.Add(new ErroDto { CodigoErro = "EST0003", Descricao = string.Format("Quantidade do produto {0} é insuficiente nesse estoque no Lote/Validade informado.", preMovimentoItem.Produto.Descricao) });
                }
            }
        }


        private void ValidarSeExisteProdutoSemLoteValidadeNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var produtoSaldoQuantidadeAtual = produtoSaldoRepository.Object.GetAll()
                        .Include(x => x.Produto)
                        .Include(x => x.LoteValidade)
                        .Include(x => x.LoteValidade.EstoqueLaboratorio)
                        .AsNoTracking()
                        .Where(x => x.ProdutoId == preMovimentoItem.ProdutoId && x.EstoqueId == preMovimentoItem.EstoqueId)
                        .Sum(x => x.QuantidadeAtual);

                if (produtoSaldoQuantidadeAtual < preMovimentoItem.Quantidade)
                {
                    Lista.Add(new ErroDto { CodigoErro = "EST0003", Descricao = string.Format("Quantidade do produto {0} é insuficiente no estoque informado.", preMovimentoItem.Produto.Descricao) });
                }

            }
        }

        //TODO: Ver como resolver a questão do número de serie e produtoSaldo.
        private void ValidarSeExisteProdutoNumeroSerieNoEstoque(EstoquePreMovimentoItemDto preMovimentoItem)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            using (var estoqueMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoLoteValidade, long>>())
            {

                var query = from mov in estoqueMovimentoRepository.Object.GetAll()
                            join item in estoqueMovimentoItemRepository.Object.GetAll()
                            on mov.Id equals item.MovimentoId
                            where mov.EstoqueId == preMovimentoItem.EstoqueId
                               && item.ProdutoId == preMovimentoItem.ProdutoId
                               && item.NumeroSerie == preMovimentoItem.NumeroSerie
                            select new { Quantidade = (((mov.IsEntrada ? 1 : 0) * 2) - 1) };



                var queryList = query.AsNoTracking().ToList();



                var quantidadeEstoque = queryList.Count();

                if (quantidadeEstoque < preMovimentoItem.Quantidade)
                {
                    Lista.Add(new ErroDto { CodigoErro = "EST0003", Descricao = string.Format("Não existe o produto {0} com o número de série {1} no estoque informado.", preMovimentoItem.Produto.Descricao, preMovimentoItem.NumeroSerie) });
                }
            }
        }

        private List<ErroDto> ValidarSaidaEstoque(EstoquePreMovimento preMovimentoSaida)
        {
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            {
                var itensSaida = estoquePreMovimentoItemRepository.Object.GetAll().AsNoTracking()
                .Include(i => i.Produto)
                .Include(i => i.ProdutoUnidade)
                .Where(w => w.PreMovimentoId == preMovimentoSaida.Id).ToList();

                foreach (var item in itensSaida)
                {
                    var itemSaidaDto = EstoquePreMovimentoItemDto.MapPreMovimentoItem(item);
                    itemSaidaDto.EstoqueId = (long)preMovimentoSaida.EstoqueId;

                    var loteValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll().AsNoTracking().FirstOrDefault(w => w.EstoquePreMovimentoItemId == item.Id);
                    if (loteValidade != null)
                    {
                        itemSaidaDto.LoteValidadeId = loteValidade.LoteValidadeId;
                    }
                    ValidarSeExisteProdutoNoEstoque(itemSaidaDto);

                }

                return Lista;
            }
        }

        public List<ErroDto> ValidarBaixaVale(EstoqueMovimento movimentoBaixa, List<long> movimentoIds)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            {
                decimal somaValores = 0;
                var movimentos = new List<EstoqueMovimento>();

                foreach (var id in movimentoIds)
                {
                    var movimento = estoqueMovimentoRepository.Object.Get(id);

                    if (movimento != null)
                    {
                        movimentos.Add(movimento);

                        // somaValores += movimento.TotalDocumento;
                    }
                }

                if (movimentos.Sum(s => s.TotalDocumento) != movimentoBaixa.TotalDocumento)
                {
                    var erroDto = new ErroDto { CodigoErro = "BAX0001" };
                    Lista.Add(erroDto);
                }


                if (movimentos.GroupBy(g => g.SisFornecedorId).Count() > 1)
                {
                    var erroDto = new ErroDto { CodigoErro = "BAX0002" };
                    Lista.Add(erroDto);
                }
                else if (movimentos[0].SisFornecedorId != movimentoBaixa.SisFornecedorId)
                {
                    var erroDto = new ErroDto { CodigoErro = "BAX0003" };
                    Lista.Add(erroDto);
                }
                return Lista;
            }

        }

        public List<ErroDto> ValidarFornecedoresBaixaVale(string baixasIds)
        {
            var ids = new List<long>();
            var movimentos = new List<EstoqueMovimento>();

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
                    return Lista;
                }
                
                var erroDto = new ErroDto { CodigoErro = "BAX0002" };
                Lista.Add(erroDto);
                return Lista;
            }
        }

        public List<ErroDto> ValidarItemDevolucao(EstoquePreMovimentoItemDto preMovimentoItem, bool validarLoteValidade, bool validarDataVencimeneto)
        {
            ValidarItem(preMovimentoItem);
            ValidarSeExisteSaidaProduto(preMovimentoItem);

            return Lista;
        }

        private void ValidarSeExisteSaidaProduto(EstoquePreMovimentoItemDto preMovimentoItem)
        {
            if (preMovimentoItem.Produto.IsValidade || preMovimentoItem.Produto.IsLote)
            {
                if (preMovimentoItem.LoteValidadeId == null)
                {
                    using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                    {
                        var preMovimentoLoteValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll().AsNoTracking().Where(w => w.EstoquePreMovimentoItemId == preMovimentoItem.Id).FirstOrDefault();

                        preMovimentoItem.LoteValidadeId = preMovimentoLoteValidade.LoteValidadeId;
                    }
                }
                ValidarSeExisteSaidaProdutoLoteValidade(preMovimentoItem);
            }
            else
            {
                ValidarSeExisteSaidaProdutoSemLoteValidade(preMovimentoItem);
            }
        }

        private void ValidarSeExisteSaidaProdutoLoteValidade(EstoquePreMovimentoItemDto preMovimentoItem)
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

                if (quantidadeEstoque - quantidadeEstoqueDevolucao - quantidadeEstoquePreMovimentoDevolucao < preMovimentoItem.Quantidade)
                {
                    Lista.Add(new ErroDto { CodigoErro = "DEV0001" });
                }
            }
        }

        private void ValidarSeExisteSaidaProdutoSemLoteValidade(EstoquePreMovimentoItemDto preMovimentoItem)
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
                    Lista.Add(new ErroDto { CodigoErro = "DEV0001" });
                }
            }
        }

        void ValidarTotalProdutos(EstoquePreMovimentoDto preMovimento)
        {
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var produtoUnidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
            {

                var itens = estoquePreMovimentoItemRepository.Object.GetAll().AsNoTracking()
                                                              .Where(w => w.PreMovimentoId == preMovimento.Id)
                                                              .ToList();

                decimal totalItens = 0;

                foreach (var item in itens)
                {
                    var unidade = produtoUnidadeRepository.Object.GetAll()
                                                           .Where(w => w.UnidadeId == item.ProdutoUnidadeId)
                                                           .Include(i => i.Unidade)
                                                           .FirstOrDefault();

                    if (unidade != null)
                    {
                        totalItens += item.CustoUnitario * (item.Quantidade / unidade.Unidade.Fator);
                    }
                }

                if (Math.Round(totalItens, 2, MidpointRounding.AwayFromZero) != Math.Round(preMovimento.TotalProduto, 2, MidpointRounding.AwayFromZero))
                {
                    if (MathHelper.TruncateTwoDigits(totalItens) != MathHelper.TruncateTwoDigits(preMovimento.TotalProduto))
                    {
                        Lista.Add(new ErroDto { CodigoErro = "", Descricao = "Soma dos valores dos itens diferente do informado no Total Produtos." });
                    }
                }
            }
        }


        public static decimal CalculaValorICMS(EstoquePreMovimentoDto preMovimento, List<EstoquePreMovimentoItemDto> itens)
        {

            if (!preMovimento.TipoFreteId.HasValue || TipoFretesSemICMS.Contains(preMovimento.TipoFreteId.Value))
            {
                return 0;
            }
            return Math.Round(itens.Sum(x => x.ValorICMS), 3, MidpointRounding.AwayFromZero);
        }

        public static decimal CalculaValorICMS(EstoquePreMovimentoDto preMovimento, EstoquePreMovimentoItemDto item)
        {

            if (!preMovimento.TipoFreteId.HasValue || TipoFretesSemICMS.Contains(preMovimento.TipoFreteId.Value))
            {
                return 0;
            }
            return Math.Round(item.ValorICMS, 3, MidpointRounding.AwayFromZero);
        }

        public static decimal CalculaValorICMS(EstoquePreMovimentoDto preMovimento, decimal? valorICMS)
        {

            if (!preMovimento.TipoFreteId.HasValue || TipoFretesSemICMS.Contains(preMovimento.TipoFreteId.Value))
            {
                return 0;
            }
            return Math.Round(valorICMS ?? 0, 3, MidpointRounding.AwayFromZero);
        }

        private decimal CalculaValorIPI(EstoquePreMovimentoDto preMovimento, List<EstoquePreMovimentoItemDto> itens)
        {
            return Math.Round(itens.Sum(x => x.ValorIPI), 3, MidpointRounding.AwayFromZero);
        }

        void ValidarTotalDocumento(EstoquePreMovimentoDto preMovimento, List<EstoquePreMovimentoItemDto> itens)
        {
            preMovimento.DescontoPer = preMovimento.DescontoPer != null ? preMovimento.DescontoPer : 0;

            var desconto = preMovimento.ValorDesconto != 0 ? preMovimento.ValorDesconto : preMovimento.DescontoPer;

            var valorIPI = CalculaValorIPI(preMovimento, itens);
            var valorICMS = CalculaValorICMS(preMovimento, itens);
            var valorTotal = preMovimento.TotalProduto + preMovimento.ValorAcrescimo - desconto + preMovimento.ValorFrete + valorIPI + valorICMS;

            if (valorTotal == null)
            {
                valorTotal = 0;
            }

            if (Math.Round(valorTotal ?? 0, 2, MidpointRounding.AwayFromZero) != Math.Round(preMovimento.TotalDocumento, 2, MidpointRounding.AwayFromZero))
            {
                if (MathHelper.TruncateTwoDigits(valorTotal.Value) != MathHelper.TruncateTwoDigits(preMovimento.TotalDocumento))
                {
                    Lista.Add(new ErroDto { CodigoErro = "", Descricao = "Valor total do documento inválido." });
                }
            }
        }

        private void ValidarGrupoEstoqueItem(EstoquePreMovimentoItemDto preMovimentoItem, long estoqueId)
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


                if (query.AsNoTracking().Count() == 0)
                {
                    Lista.Add(
                        new ErroDto
                        {
                            CodigoErro = "",
                            Descricao = string.Format("Grupo do produto {0} não esta relacionado ao estoque informado.", preMovimentoItem.Produto.Descricao)
                        });
                }
            }
        }

        private void ValidarProdutoEmInventario(long produtoId, long estoqueId, long inventarioId)
        {
            var query = @"select est.Descricao as Estoque
                               , pro.DescricaoResumida as Produto
                             from EstInventario inv
                             join EstInventarioItem ivIt on ivIt.InventarioId = inv.Id
                             join Est_Estoque est on est.Id = inv.EstoqueId
                             join Est_Produto pro on pro.Id = ivIt.ProdutoId
                             where inv.EstoqueId = @EstoqueId
                               and inv.StatusInventarioId <> 4
                               and ivIt.ProdutoId = @ProdutoId
                               and inv.Id <> @InventarioId";

            try
            {

                using (var connection = new SqlConnection(this.GetConnection()))
                {

                    var produtosEstoque = connection.QueryAsync<EstoqueProduto>(query, new { EstoqueId = estoqueId, ProdutoId = produtoId, InventarioId = inventarioId });

                    var lista = produtosEstoque.Result?.ToList();

                    if (lista != null && lista.Count() > 0)
                    {
                        Lista.Add(new ErroDto { CodigoErro = "INV0002", Parametros = new List<object> { lista[0].Produto, lista[0].Estoque } });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
