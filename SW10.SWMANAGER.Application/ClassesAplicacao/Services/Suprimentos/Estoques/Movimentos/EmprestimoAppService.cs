using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Validacoes;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class EmprestimoAppService : SWMANAGERAppServiceBase, IEmprestimoAppService
    {
        private DefaultReturn<EstoquePreMovimentoDto> _retornoPadrao;

        public EmprestimoAppService()
        {
            _retornoPadrao = new DefaultReturn<EstoquePreMovimentoDto>
            {
                Warnings = new List<ErroDto>()
            };
        }

        public async Task<DefaultReturn<EstoquePreMovimentoDto>> CriarOuEditarSolicitacaoEmprestimo(EstoquePreMovimentoDto input)
        {
            var itens = new List<EstoquePreMovimentoItemSolicitacaoDto>();
            itens = JsonConvert.DeserializeObject<List<EstoquePreMovimentoItemSolicitacaoDto>>(input.Itens);

            if (itens == null || itens.Count == 0)
            {
                _retornoPadrao.Errors.Add(new ErroDto()
                {
                    Descricao = $"Não é possível criar uma solicitação sem itens."
                });

                return _retornoPadrao;
            }

            try
            {

                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estoqueSolicitacaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>())
                using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    input.Movimento = input.Emissao;
                    input.IsEntrada = (input.EstTipoOperacaoId == (long)EnumTipoOperacao.Entrada);
                    input.EstTipoMovimentoId = (input.EstTipoOperacaoId == (long)EnumTipoOperacao.Entrada)
                        ? (long)EnumTipoMovimento.Emprestimo_Entrada
                        : (long)EnumTipoMovimento.Emprestimo_Saida;

                    if (input.Id.Equals(0))
                    {
                        input.CentroCustoId = input.CentroCustoId != 0 ? input.CentroCustoId : null;
                        var preMovimento = EstoquePreMovimentoDto.Mapear(input);
                        preMovimento.EstoqueEmprestimo = EstoqueEmprestimoDto.Mapear(input.EstoqueEmprestimo);
                        preMovimento.IsEntrada = (input.EstTipoOperacaoId == (long)EnumTipoOperacao.Entrada);
                        preMovimento.Movimento = DateTime.Now;
                        preMovimento.GrupoOperacaoId = (long)EnumGrupoOperacao.Solicitacao;
                        preMovimento.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Pendente;
                        preMovimento.Documento = await ultimoIdAppService.Object.ObterProximoCodigo("Solicitacao");
                        input.Documento = preMovimento.Documento;
                        input.Id = await estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(preMovimento);

                        foreach (var item in itens)
                        {
                            var preMovimentoItem = new EstoquePreMovimentoItem
                            {
                                PreMovimentoId = input.Id,
                                ProdutoId = item.ProdutoId,
                                ProdutoUnidadeId = (long)item.ProdutoUnidadeId,
                                Quantidade = item.Quantidade,
                                PreMovimentoItemEstadoId = (long)EnumPreMovimentoEstado.Pendente
                            };
                            estoquePreMovimentoItemRepository.Object.InsertAndGetId(preMovimentoItem);

                            var solicitacaoItem = new EstoqueSolicitacaoItem()
                            {
                                SolicitacaoId = input.Id,
                                ProdutoId = item.ProdutoId,
                                ProdutoUnidadeId = (long)item.ProdutoUnidadeId,
                                Quantidade = item.Quantidade,
                                EstadoSolicitacaoItemId = (long)EnumPreMovimentoEstado.Pendente,
                                QuantidadeAtendida = 0
                            };
                            estoqueSolicitacaoItemRepository.Object.Insert(solicitacaoItem);
                        }
                    }
                    else
                    {
                        var preMovimentoEdit = estoquePreMovimentoRepository.Object.Get(input.Id);

                        if (preMovimentoEdit == null)
                        {
                            _retornoPadrao.Errors.Add(new ErroDto()
                            {
                                Descricao = $"Solicitação de Empréstimo não encontrada."
                            });

                            return _retornoPadrao;
                        }

                        preMovimentoEdit.EstTipoMovimentoId = input.EstTipoMovimentoId;
                        preMovimentoEdit.EmpresaId = input.EmpresaId;
                        preMovimentoEdit.Documento = input.Documento;
                        preMovimentoEdit.EstoqueId = input.EstoqueId;
                        preMovimentoEdit.Emissao = input.Emissao;
                        preMovimentoEdit.Observacao = input.Observacao;
                        preMovimentoEdit.UnidadeOrganizacionalId = input.UnidadeOrganizacionalId;
                        preMovimentoEdit.Id = estoquePreMovimentoRepository.Object.InsertOrUpdateAndGetId(preMovimentoEdit);

                        var idsPreMovimentoItem = itens.Select(x => x.Id).ToList();
                        var dbPreMovimentoItens = await estoquePreMovimentoItemRepository.Object
                            .GetAllListAsync(x => x.PreMovimentoId == preMovimentoEdit.Id && !x.IsDeleted);
                        var idsDbsPreMovimentoItens = dbPreMovimentoItens.Select(x => x.Id).ToList();
                        var dbSolicitacaoItens = await estoqueSolicitacaoItemRepository.Object
                            .GetAllListAsync(x => x.SolicitacaoId == preMovimentoEdit.Id && !x.IsDeleted);

                        //Inativando itens removidos
                        foreach (var idDbPreMovItem in
                            idsDbsPreMovimentoItens.Where(x => !idsPreMovimentoItem.Contains(x)))
                        {
                            var preMovimentoItem = dbPreMovimentoItens.FirstOrDefault(x => x.Id == idDbPreMovItem);
                            await estoqueSolicitacaoItemRepository.Object.DeleteAsync(
                                dbSolicitacaoItens.FirstOrDefault(x => x.ProdutoId == preMovimentoItem.ProdutoId));
                            await estoquePreMovimentoItemRepository.Object.DeleteAsync(preMovimentoItem.Id);
                        }


                        foreach (var item in itens)
                        {
                            EstoquePreMovimentoItem preMovimentoItem;
                            EstoqueSolicitacaoItem solicitacaoItem;

                            if (item.Id != default)
                            {
                                preMovimentoItem = dbPreMovimentoItens.FirstOrDefault(x => x.Id == item.Id);
                                var produtoIdAlterado = preMovimentoItem.ProdutoId;
                                if (preMovimentoItem != null)
                                {
                                    preMovimentoItem.ProdutoId = item.ProdutoId;
                                    preMovimentoItem.ProdutoUnidadeId = (long)item.ProdutoUnidadeId;
                                    preMovimentoItem.Quantidade = item.Quantidade;
                                }

                                solicitacaoItem = dbSolicitacaoItens.FirstOrDefault(x => x.ProdutoId == produtoIdAlterado);
                                if (solicitacaoItem != null)
                                {
                                    solicitacaoItem.ProdutoId = item.ProdutoId;
                                    solicitacaoItem.ProdutoUnidadeId = (long)item.ProdutoUnidadeId;
                                    solicitacaoItem.Quantidade = item.Quantidade;
                                }
                            }
                            else
                            {
                                preMovimentoItem = new EstoquePreMovimentoItem
                                {
                                    PreMovimentoId = preMovimentoEdit.Id,
                                    ProdutoId = item.ProdutoId,
                                    ProdutoUnidadeId = (long)item.ProdutoUnidadeId,
                                    Quantidade = item.Quantidade,
                                    Id = item.Id,
                                    PreMovimentoItemEstadoId = (long)EnumPreMovimentoEstado.Pendente
                                };
                                solicitacaoItem = new EstoqueSolicitacaoItem()
                                {
                                    SolicitacaoId = input.Id,
                                    ProdutoId = item.ProdutoId,
                                    ProdutoUnidadeId = (long)item.ProdutoUnidadeId,
                                    Quantidade = item.Quantidade,
                                    EstadoSolicitacaoItemId = (long)EnumPreMovimentoEstado.Pendente,
                                    QuantidadeAtendida = 0
                                };
                            }

                            await estoquePreMovimentoItemRepository.Object.InsertOrUpdateAndGetIdAsync(preMovimentoItem);
                            await estoqueSolicitacaoItemRepository.Object.InsertOrUpdateAndGetIdAsync(solicitacaoItem);
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWork.Dispose();

                    _retornoPadrao.ReturnObject = input;
                }
            }
            catch (Exception ex)
            {
                var erro = new ErroDto();

                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    erro = new ErroDto
                    {
                        CodigoErro = inner.HResult.ToString(),
                        Descricao = inner.Message.ToString()
                    };
                    _retornoPadrao.Errors.Add(erro);
                }
                else
                {
                    erro.CodigoErro = ex.HResult.ToString();
                    erro.Descricao = ex.Message.ToString();
                    _retornoPadrao.Errors.Add(erro);
                }
            }

            return _retornoPadrao;
        }

        public async Task<EstoquePreMovimentoDto> ObterSolicitacaoParaBaixa(long preMovimentoId)
        {
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var preMovimentoValidacaoService = IocManager.Instance.ResolveAsDisposable<PreMovimentoValidacaoService>())
                {
                    var result = await estoquePreMovimentoRepository.Object
                        .GetAllIncluding(
                            x => x.EstTipoMovimento,
                            x => x.EstTipoOperacao,
                            x => x.EstoqueEmprestimo,
                            x => x.EstoqueEmprestimo.SisPessoa)
                        .SingleOrDefaultAsync(x => x.Id == preMovimentoId);

                    var preMovimentoDto = EstoquePreMovimentoDto.Mapear(result);
                    preMovimentoDto.PossuiLoteValidade = preMovimentoValidacaoService.Object.ExisteLoteValidadePendente(preMovimentoDto);

                    return preMovimentoDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<EstoquePreMovimentoItemSolicitacaoDto>> ObterItensDaSolicitacao(long preMovimentoId)
        {
            var contarPreMovimentos = 0;
            long idGrid = 1;
            List<EstoquePreMovimentoItemSolicitacaoDto> preMovimentoItens = null;
            try
            {
                using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {
                    var estoquePreMovimento = await estoquePreMovimentoRepository.Object.GetAsync(preMovimentoId);

                    var query = estoquePreMovimentoItemRepository.Object.GetAll()
                        .AsNoTracking()
                        .Include(x => x.EstoquePreMovimento)
                        .Include(x => x.EstoquePreMovimentosLoteValidades.Select(lv => lv.LoteValidade))
                        .Where(m => m.PreMovimentoId == preMovimentoId)
                        .Select(
                            s => new EstoquePreMovimentoItemSolicitacaoDto
                            {
                                Id = s.Id,
                                Produto = s.Produto.Descricao,
                                EstadoSolicitacaoItemId = s.PreMovimentoItemEstadoId.HasValue ? (long)s.PreMovimentoItemEstadoId : 0,
                                QuantidadeAtendida = 0,
                                QuantidadeSolicitada = s.Quantidade,
                                Quantidade = s.Quantidade - 0,
                                ProdutoId = s.ProdutoId,
                                ProdutoUnidade = s.ProdutoUnidade.Descricao,
                                ProdutoUnidadeId = s.ProdutoUnidadeId,
                                IsLote = s.Produto.IsLote,
                                NumeroSerie = s.NumeroSerie
                            });

                    contarPreMovimentos = await query.CountAsync().ConfigureAwait(false);

                    preMovimentoItens = await query.OrderBy(p => p.Produto).ToListAsync().ConfigureAwait(false);

                    foreach (var item in preMovimentoItens)
                    {
                        item.IdGrid = idGrid++;
                        if (item.IsLote)
                        {
                            var produtoSaldo = await produtoSaldoRepository.Object
                                .GetAllIncluding(x => x.LoteValidade, x => x.LoteValidade.EstoqueLaboratorio)
                                .Where(x => x.ProdutoId == item.ProdutoId && x.LoteValidade.Validade > DateTime.Now &&
                                            x.QuantidadeAtual > 0 && x.EstoqueId == estoquePreMovimento.EstoqueId)
                                .OrderBy(x => x.LoteValidade.Validade)
                                .FirstOrDefaultAsync();

                            if (produtoSaldo != null)
                            {
                                var loteSugerido = produtoSaldo.LoteValidade;
                                item.LoteSugeridoId = loteSugerido.Id;
                                item.LoteSugeridoName = "Lt: " + loteSugerido.Lote + " Val: " + loteSugerido.Validade + " Qtd: " + produtoSaldo.QuantidadeAtual +
                                    " Lab: " + loteSugerido.EstoqueLaboratorio?.Descricao;
                            }
                        }
                    }
                }

                return new PagedResultDto<EstoquePreMovimentoItemSolicitacaoDto>(contarPreMovimentos, preMovimentoItens);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<DefaultReturn<EstoquePreMovimentoDto>> AtenderBaixa(EstoquePreMovimentoDto preMovimento)
        {
            bool isSaida = preMovimento.EstTipoOperacaoId == (long)EnumTipoOperacao.Saida;
            //Validação
            if (isSaida)
            {
                using (var movimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IMovimentoValidacaoDomainService>())
                {

                    _retornoPadrao.Errors = movimentoValidacaoDomainService.Object.ValidarConfirmacaoSolicitacao(preMovimento);

                    if (_retornoPadrao.Errors.Count > 0)
                        return _retornoPadrao;
                }
            }

            //Persistência
            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            using (var estoqueSolicitacaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>())
            using (var estoqueMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueMovimentoAppService>())
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
            using (var unitOfWork = unitOfWorkManager.Object.Begin())
            {
                var produtosQuantidades = new List<ProdutoQuantidade>();
                var somaQtdAtendida = 0m;
                var statusAtendimento = new List<long>
                {
                    (long) EnumPreMovimentoEstado.TotalmenteAtendido,
                    (long) EnumPreMovimentoEstado.TotalmenteSuspenso
                };

                var novoEstoquePreMovimento = new EstoquePreMovimento
                {
                    Documento = preMovimento.Documento,
                    Emissao = preMovimento.Emissao,
                    Movimento = preMovimento.Emissao,
                    EstoqueId = preMovimento.EstoqueId,
                    Observacao = preMovimento.Observacao,
                    UnidadeOrganizacionalId = preMovimento.UnidadeOrganizacionalId,
                    PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Autorizado,
                    IsEntrada = preMovimento.IsEntrada,
                    EstTipoOperacaoId = preMovimento.EstTipoOperacaoId,
                    EstTipoMovimentoId = preMovimento.EstTipoMovimentoId,
                    GrupoOperacaoId = (long)EnumGrupoOperacao.Movimentos,
                    EstoquePreMovimentoParentId = preMovimento.Id,
                    EstoqueEmprestimoId = preMovimento.EstoqueEmprestimoId
                };

                var nomeTabela = isSaida ? "SaidaProduto" : "EntradaProduto";
                nomeTabela = "SaidaProduto";
                novoEstoquePreMovimento.Documento = await ultimoIdAppService.Object.ObterProximoCodigo(nomeTabela);

                await estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(novoEstoquePreMovimento);
                var itens = JsonConvert.DeserializeObject<List<EstoquePreMovimentoItemSolicitacaoDto>>(preMovimento.Itens);

                foreach (var item in itens.Where(x => !statusAtendimento
                    .Contains(x.EstadoSolicitacaoItemId) && x.QuantidadeAtendida.HasValue && x.QuantidadeAtendida != 0))
                {
                    produtosQuantidades.Add(new ProdutoQuantidade
                    {
                        ProdutoId = item.ProdutoId,
                        Quantidade = (float)item.Quantidade
                    });

                    if (!string.IsNullOrEmpty(item.NumerosSerieJson))
                    {
                        var numerosSerie = JsonConvert.DeserializeObject<List<NumeroSerieGridDto>>(item.NumerosSerieJson);

                        foreach (var numeroSerie in numerosSerie)
                        {
                            var movimentoItem = new EstoquePreMovimentoItem
                            {
                                PreMovimentoId = novoEstoquePreMovimento.Id,
                                ProdutoId = item.ProdutoId,
                                ProdutoUnidadeId = item.ProdutoUnidadeId,
                                Quantidade = 1,
                                NumeroSerie = numeroSerie.NumeroSerie
                            };

                            movimentoItem.Id = await estoquePreMovimentoItemRepository.Object.InsertAndGetIdAsync(movimentoItem);

                            produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItemPreMovimento(novoEstoquePreMovimento, movimentoItem);
                        }

                        somaQtdAtendida = numerosSerie.Count();
                    }
                    else if (item.IsLote)
                    {
                        var listaSomaQuantidade = await estoqueMovimentoItemRepository.Object.GetAllListAsync(x => x.EstoquePreMovimentoItemId == item.Id);

                        somaQtdAtendida = listaSomaQuantidade.Sum(s => s.Quantidade);

                        var novoPreMovimentoItem = new EstoquePreMovimentoItem
                        {
                            PreMovimentoId = novoEstoquePreMovimento.Id,
                            ProdutoId = item.ProdutoId,
                            ProdutoUnidadeId = item.ProdutoUnidadeId,
                            Quantidade =
                                unidadeAppService.Object.ObterQuantidadeReferencia((long)item.ProdutoUnidadeId,
                                    (decimal)item.QuantidadeAtendida)
                        };

                        novoPreMovimentoItem.Id = estoquePreMovimentoItemRepository.Object.InsertAndGetId(novoPreMovimentoItem);

                        var lotesValidades = JsonConvert.DeserializeObject<List<LoteValidadeGridDto>>(
                            item.LotesValidadesJson, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });

                        if (lotesValidades != null)
                        {
                            foreach (var itemLoteValidade in lotesValidades.Where(x => x.Id.Equals(0)))
                            {
                                var movimentoLoteValidade = new EstoquePreMovimentoLoteValidade
                                {
                                    EstoquePreMovimentoItemId = novoPreMovimentoItem.Id,
                                    LoteValidadeId = itemLoteValidade.LoteValidadeId,
                                    Quantidade = unidadeAppService.Object.ObterQuantidadeReferencia(
                                        (long)item.ProdutoUnidadeId, (decimal)itemLoteValidade.Quantidade)
                                };

                                estoquePreMovimentoLoteValidadeRepository.Object.Insert(movimentoLoteValidade);
                                produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItemLoteValidade(EstoqueLoteValidadeAppService.MapLoteValidade(movimentoLoteValidade));
                            }

                            somaQtdAtendida = lotesValidades.Sum(x => x.Quantidade.Value);
                        }
                    }
                    else
                    {
                        var qtdMovimento = item.QuantidadeAtendida.Value - (item.QuantidadeSolicitada - item.Quantidade);

                        var preMovimentoItem = new EstoquePreMovimentoItem
                        {
                            PreMovimentoId = novoEstoquePreMovimento.Id,
                            ProdutoId = item.ProdutoId,
                            ProdutoUnidadeId = item.ProdutoUnidadeId,
                            Quantidade = unidadeAppService.Object.ObterQuantidadeReferencia((long)item.ProdutoUnidadeId, qtdMovimento)
                        };

                        await estoquePreMovimentoItemRepository.Object.InsertAndGetIdAsync(preMovimentoItem);

                        somaQtdAtendida = item.QuantidadeAtendida.Value;
                    }

                    var solicitacaoItem = await estoqueSolicitacaoItemRepository.Object.GetAsync(item.Id);
                    if (solicitacaoItem != null)
                    {
                        solicitacaoItem.QuantidadeAtendida = somaQtdAtendida;
                        if (solicitacaoItem.Quantidade <= solicitacaoItem.QuantidadeAtendida)
                        {
                            solicitacaoItem.EstadoSolicitacaoItemId = (long)EnumPreMovimentoEstado.TotalmenteAtendido;
                            item.EstadoSolicitacaoItemId = (long)EnumPreMovimentoEstado.TotalmenteAtendido;
                        }

                        estoqueSolicitacaoItemRepository.Object.Update(solicitacaoItem);
                    }
                }

                var estoquePreMovimento = estoquePreMovimentoRepository.Object.Get(preMovimento.Id);
                if (estoquePreMovimento != null)
                {
                    if (itens.Count(x => statusAtendimento.Contains(x.EstadoSolicitacaoItemId)) == itens.Count)
                    {
                        estoquePreMovimento.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.TotalmenteAtendido;
                    }
                    else
                    {
                        estoquePreMovimento.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.ParcialmenteAtendido;
                    }

                    estoquePreMovimentoRepository.Object.Update(estoquePreMovimento);
                }

                unitOfWork.Complete();
                unitOfWorkManager.Object.Current.SaveChanges();

                var retornoMovimento = await estoqueMovimentoAppService.Object.GerarMovimentoEntrada(novoEstoquePreMovimento.Id);

                if (!retornoMovimento.Errors.Any() && estoquePreMovimento.EstTipoOperacaoId != (long)EnumTipoOperacao.Devolucao &&
                    estoquePreMovimento.PreMovimentoEstadoId.Equals((long)EnumPreMovimentoEstado.TotalmenteAtendido))
                {
                    _retornoPadrao = await CriarDevolucao(estoquePreMovimento.Id);
                }

                unitOfWork.Complete();
                unitOfWorkManager.Object.Current.SaveChanges();
                unitOfWork.Dispose();

                _retornoPadrao.ReturnObject = EstoquePreMovimentoDto.Mapear(novoEstoquePreMovimento);

                return _retornoPadrao;
            }
        }

        private async Task<DefaultReturn<EstoquePreMovimentoDto>> CriarDevolucao(long preMovimentoId)
        {
            using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            using (var estoqueSolicitacaoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>())
            using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            {
                var now = DateTime.Now;
                var preMovimento = await estoquePreMovimentoRepository.Object.GetAllIncluding(x => x.EstoqueEmprestimo)
                    .Where(x => x.Id == preMovimentoId).SingleOrDefaultAsync();
                var itensSolicitacao = await estoqueSolicitacaoItemRepository.Object.GetAllListAsync(x => x.SolicitacaoId == preMovimentoId);
                if (preMovimento == null || itensSolicitacao.Count.Equals(0))
                {
                    _retornoPadrao.Errors.Add(ErroDto.Criar("EST0004", "Não foi possível efetuar a criação automática de Devolução."));
                    return _retornoPadrao;
                }

                var preMovimentoDevolucao = new EstoquePreMovimento()
                {
                    Movimento = now,
                    PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Pendente,
                    IsEntrada = !preMovimento.IsEntrada,
                    Emissao = now,
                    UnidadeOrganizacionalId = preMovimento.UnidadeOrganizacionalId,
                    Observacao = preMovimento.Observacao,
                    EstoqueId = preMovimento.EstoqueId,
                    EstTipoMovimentoId = preMovimento.EstTipoMovimentoId == (long)EnumTipoMovimento.Emprestimo_Entrada
                        ? (long)EnumTipoMovimento.Emprestimo_Saida : (long)EnumTipoMovimento.Emprestimo_Entrada,
                    EstTipoOperacaoId = (long)EnumTipoOperacao.Devolucao,
                    GrupoOperacaoId = (long)EnumGrupoOperacao.Solicitacao,
                    EstoquePreMovimentoParentId = preMovimento.Id
                };
                preMovimentoDevolucao.Documento = await ultimoIdAppService.Object.ObterProximoCodigo("Solicitacao");
                preMovimentoDevolucao.EstoqueEmprestimo = new EstoqueEmprestimo()
                {
                    ContatoNome = preMovimento.EstoqueEmprestimo.ContatoNome,
                    ContatoEmail = preMovimento.EstoqueEmprestimo.ContatoEmail,
                    ContatoTelefone = preMovimento.EstoqueEmprestimo.ContatoTelefone,
                    SisPessoaId = preMovimento.EstoqueEmprestimo.SisPessoaId
                };

                preMovimentoDevolucao.Id = await estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(preMovimentoDevolucao);

                foreach (var item in itensSolicitacao)
                {
                    var preMovimentoItem = new EstoquePreMovimentoItem
                    {
                        PreMovimentoId = preMovimentoDevolucao.Id,
                        ProdutoId = item.ProdutoId,
                        ProdutoUnidadeId = item.ProdutoUnidadeId,
                        Quantidade = item.Quantidade,
                        PreMovimentoItemEstadoId = (long)EnumPreMovimentoEstado.Pendente
                    };
                    estoquePreMovimentoItemRepository.Object.InsertAndGetId(preMovimentoItem);

                    var solicitacaoItem = new EstoqueSolicitacaoItem()
                    {
                        SolicitacaoId = preMovimentoDevolucao.Id,
                        ProdutoId = item.ProdutoId,
                        ProdutoUnidadeId = item.ProdutoUnidadeId,
                        Quantidade = item.Quantidade,
                        EstadoSolicitacaoItemId = (long)EnumPreMovimentoEstado.Pendente,
                        QuantidadeAtendida = 0
                    };
                    estoqueSolicitacaoItemRepository.Object.Insert(solicitacaoItem);
                }

            }

            return _retornoPadrao;
        }
    }
}
