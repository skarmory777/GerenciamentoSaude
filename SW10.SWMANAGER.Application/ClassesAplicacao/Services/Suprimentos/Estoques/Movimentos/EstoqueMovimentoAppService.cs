using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class EstoqueMovimentoAppService : SWMANAGERAppServiceBase, IEstoqueMovimentoAppService
    {
        public async Task<DefaultReturn<object>> GerarMovimentoEntrada(long preMovimentoId)
        {
            var retornoPadrao = new DefaultReturn<object>();
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    var preMovimento = estoquePreMovimentoRepository.Object.Get(preMovimentoId);
                    if (preMovimento == null)
                    {
                        return retornoPadrao;
                    }
                    
                    using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                    using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
                    using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                    {
                        var preMovimentosItens = await estoquePreMovimentoItemRepository.Object.GetAll().AsNoTracking()
                            .Include(x=> x.EstoquePreMovimentosLoteValidades)
                            .Include(x => x.EstoquePreMovimentosLoteValidades.Select(z=> z.LoteValidade))
                            .Include(x=> x.Produto)
                            .Where(w => w.PreMovimentoId == preMovimento.Id).ToListAsync().ConfigureAwait(false);
                        var _preMovimentoItens = EstoquePreMovimentoItemDto.MapPreMovimentoItem(preMovimentosItens);
                        retornoPadrao.Errors = await preMovimentoValidacaoDomainService.Object.Validar(EstoquePreMovimentoDto.MapPreMovimento(preMovimento), _preMovimentoItens, (preMovimento.EstTipoMovimentoId != (long)EnumTipoMovimento.Inventario_Entrada && preMovimento.EstTipoMovimentoId != (long)EnumTipoMovimento.Inventario_Saida));

                        if (preMovimento.EstTipoOperacaoId == (long)EnumTipoOperacao.Entrada)
                        {
                            var existeLoteValidadePendente = preMovimentoValidacaoDomainService.Object.ExisteLoteValidadePendente(EstoquePreMovimentoDto.MapPreMovimento(preMovimento));
                            if (existeLoteValidadePendente)
                            {
                                retornoPadrao.Errors.Add(ErroDto.Criar("EST003","Existe Produtos sem Informação de Lote/Validade"));
                            }
                        }

                        if (retornoPadrao.Errors.Any())
                        {
                            return retornoPadrao;
                        }
                        using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            var movimento = new EstoqueMovimento
                            {
                                AcrescimoDecrescimo = preMovimento.AcrescimoDecrescimo,
                                AplicacaoDireta = preMovimento.AplicacaoDireta,
                                CentroCustoId = preMovimento.CentroCustoId,
                                CFOPId = preMovimento.CFOPId,
                                Consiginado = preMovimento.Consiginado,
                                Contabiliza = preMovimento.Contabiliza,
                                DescontoPer = preMovimento.DescontoPer,
                                Documento = preMovimento.Documento,
                                Emissao = preMovimento.Emissao,
                                EmpresaId = preMovimento.EmpresaId,
                                EntragaProduto = preMovimento.EntragaProduto,
                                EstoqueId = preMovimento.EstoqueId,
                                EstoquePreMovimentoId = preMovimento.Id,
                                SisFornecedorId = preMovimento.SisFornecedorId,
                                Frete = preMovimento.Frete,
                                FretePer = preMovimento.FretePer,
                                Frete_SisFornecedorId = preMovimento.Frete_SisFornecedorId,
                                ICMSPer = preMovimento.ICMSPer,
                                InclusoNota = preMovimento.InclusoNota,
                                IsEntrada = preMovimento.IsEntrada,
                                Movimento = preMovimento.Movimento,
                                OrdemId = preMovimento.OrdemId,
                                PreMovimentoEstadoId = 2,
                                Quantidade = preMovimento.Quantidade,
                                Serie = preMovimento.Serie,
                                TipoFreteId = preMovimento.TipoFreteId,
                                TotalDocumento = preMovimento.TotalDocumento,
                                TotalProduto = preMovimento.TotalProduto,
                                ValorFrete = preMovimento.ValorFrete,
                                ValorICMS = preMovimento.ValorICMS,
                                PacienteId = preMovimento.PacienteId,
                                MedicoSolcitanteId = preMovimento.MedicoSolcitanteId,
                                UnidadeOrganizacionalId = preMovimento.UnidadeOrganizacionalId,
                                Observacao = preMovimento.Observacao,
                                AtendimentoId = preMovimento.AtendimentoId,
                                EstTipoMovimentoId = preMovimento.EstTipoMovimentoId,
                                EstTipoOperacaoId = preMovimento.EstTipoOperacaoId
                            };

                            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
                            using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                            using (var estoqueMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoLoteValidade, long>>())
                            {
                                var movimentoId = estoqueMovimentoRepository.Object.InsertAndGetId(movimento);

                                foreach (var item in preMovimentosItens)
                                {
                                    var movimentoItem = new EstoqueMovimentoItem
                                    {
                                        NumeroSerie = item.NumeroSerie,
                                        ProdutoId = item.ProdutoId,
                                        Quantidade = item.Quantidade,
                                        CustoUnitario = item.CustoUnitario,
                                        MovimentoId = movimentoId,
                                        PerIPI = item.PerIPI,
                                        ProdutoUnidadeId = item.ProdutoUnidadeId,
                                        EstoquePreMovimentoItemId = item.Id
                                    };

                                    var movimentoItemId =
                                        estoqueMovimentoItemRepository.Object.InsertAndGetId(movimentoItem);

                                    var movimentoLoteValidades = estoquePreMovimentoLoteValidadeRepository.Object
                                        .GetAll().AsNoTracking().Where(w => w.EstoquePreMovimentoItemId == item.Id).ToList();

                                    foreach (var itemLoteValidade in movimentoLoteValidades)
                                    {
                                        var loteValidade = new EstoqueMovimentoLoteValidade
                                        {
                                            Quantidade = itemLoteValidade.Quantidade,
                                            EstoqueMovimentoItemId = movimentoItemId,
                                            LoteValidadeId = itemLoteValidade.LoteValidadeId
                                        };

                                        estoqueMovimentoLoteValidadeRepository.Object.InsertAndGetId(loteValidade);
                                    }
                                }
                                
                                preMovimento.PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Conferencia;

                                estoquePreMovimentoRepository.Object.InsertOrUpdate(preMovimento);
                                produtoSaldoDomainService.Object.AtualizarSaldoMovimento(movimento.Id);

                                if (preMovimento.EstTipoMovimentoId == (long) EnumTipoMovimento.Paciente_Saida)
                                {
                                    await GerarFaturamento(preMovimento, _preMovimentoItens).ConfigureAwait(false);
                                }
                            }

                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                    return retornoPadrao;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(retornoPadrao, ex);
            }
            return retornoPadrao;
        }

        public async Task<DefaultReturn<object>> GerarMovimentoTransferencia(long transferenciaId)
        {
            var retornoValidacao = new DefaultReturn<object>();
            using (var estoqueTransferenciaProduto = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProduto, long>>())
            {
                var transferencia = await estoqueTransferenciaProduto.Object.GetAsync(transferenciaId);
                if (transferencia == null)
                {
                    return retornoValidacao;
                }
                
                retornoValidacao = await ValidarMovimento(transferencia.PreMovimentoSaidaId);
                retornoValidacao.Errors.AddRange((await ValidarMovimento(transferencia.PreMovimentoEntradaId)).Errors);

                if (retornoValidacao.Errors.Count != 0)
                {
                    return retornoValidacao;
                }
                var retornoSaida = await GerarMovimentoEntrada(transferencia.PreMovimentoSaidaId);
                var retornoEntrada = await GerarMovimentoEntrada(transferencia.PreMovimentoEntradaId);

                retornoSaida.Errors.AddRange(retornoEntrada.Errors);

                return retornoSaida;
            }
        }


        private static async Task<DefaultReturn<object>> ValidarMovimento(long preMovimentoId)
        {
            var retornoPadrao = new DefaultReturn<object>();
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                {
                    var preMovimento = await estoquePreMovimentoRepository.Object.GetAsync(preMovimentoId);
                    if (preMovimento == null)
                    {
                        return retornoPadrao;
                    }
                    
                    using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
                    using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                    {
                        var preMovimentosItens = await estoquePreMovimentoItemRepository.Object.GetAll().AsNoTracking()
                            .Include(x => x.EstoquePreMovimentosLoteValidades)
                            .Include(x => x.EstoquePreMovimentosLoteValidades.Select(z => z.LoteValidade))
                            .Include(x => x.Produto)
                            .Where(w => w.PreMovimentoId == preMovimentoId).ToListAsync().ConfigureAwait(false);
                        var preMovimentoItens = EstoquePreMovimentoItemDto.MapPreMovimentoItem(preMovimentosItens);
                        retornoPadrao.Errors = await preMovimentoValidacaoDomainService.Object.Validar(EstoquePreMovimentoDto.MapPreMovimento(preMovimento), preMovimentoItens);
                    }
                    return retornoPadrao;
                }
            }
            catch (Exception ex)
            {
                ErrorHandler(retornoPadrao, ex);
            }
            return retornoPadrao;
        }


        private static async Task GerarFaturamento(EstoquePreMovimento preMovimento, IEnumerable<EstoquePreMovimentoItemDto> itens)
        {
            var faturamentoContaItemInsertDto = new FaturamentoContaItemInsertDto
            {
                AtendimentoId = preMovimento.AtendimentoId ?? 0,
                Data = preMovimento.Movimento.Date
            };

            var itensFaturamentoIds = new List<FaturamentoContaItemDto>();

            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            using (var faturamentoContaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            {
                foreach (var item in itens)
                {
                    var produto = produtoRepository.Object.GetAll().FirstOrDefault(w => w.Id == item.ProdutoId);

                    if (produto == null || produto.FaturamentoItemId == null)
                    {
                        continue;
                    }
                    var contaItem = new FaturamentoContaItemDto
                    {
                        Id = (long) produto.FaturamentoItemId, Qtde = (float) item.Quantidade
                    };

                    itensFaturamentoIds.Add(contaItem);
                }
                faturamentoContaItemInsertDto.ItensFaturamento = itensFaturamentoIds;
                await faturamentoContaItemAppService.Object.InserirItensContaFaturamento(faturamentoContaItemInsertDto);
            }
        }


        public async Task<ListResultDto<MovimentoIndexDto>> ListarBaixaValePendente(ListarEstoquePreMovimentoInput input)
        {
            var contarEntradas = 0;
            var estoqueMovimentoDtos = new List<MovimentoIndexDto>();
            try
            {
                using (var estMovimentoBaixaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixa, long>>())
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                {

                    var queryBaixa = estMovimentoBaixaRepository.Object.GetAll();

                    var query = estoqueMovimentoRepository.Object
                        .GetAll()
                        .Where(w => !queryBaixa.Any(a => a.MovimentoBaixaId == w.Id) && w.EstTipoMovimentoId == (long)EnumTipoMovimento.Vale_Entrada
                               && (input.FornecedorId == null || w.SisFornecedorId == input.FornecedorId)
                               && (input.PeridoDe == null || input.PeridoDe <= w.Emissao)
                               && (input.PeridoAte == null || input.PeridoAte >= w.Emissao)
                               && (string.IsNullOrEmpty(input.Filtro) ||
                                   w.SisFornecedor.Descricao.Contains(input.Filtro.ToLower()) ||
                                   w.Empresa.NomeFantasia.Contains(input.Filtro.ToLower()) ||
                                   w.Documento.Contains(input.Filtro.ToLower()) ||
                                   w.TotalDocumento.ToString().Contains(input.Filtro.ToLower())
                               )
                        )
                        .Select(s => new MovimentoIndexDto
                        {
                            Id = s.Id
                                                            ,

                            DataEmissaoSaida = s.Emissao
                                                            ,
                            Documento = s.Documento
                                                            ,
                            Empresa = (s.Empresa != null) ? s.Empresa.NomeFantasia : string.Empty
                                                          ,
                            UsuarioId = s.CreatorUserId
                            ,
                            PreMovimentoEstadoId = s.PreMovimentoEstadoId

                           ,
                            IsEntrada = s.IsEntrada
                           ,
                            TipoMovimento = s.EstTipoOperacao.Descricao
                           ,
                            TipoOperacaoId = s.EstTipoOperacaoId
                           ,
                            ValorDocumento = s.TotalDocumento,
                            Fornecedor = (s.SisFornecedor != null) ? s.SisFornecedor.Descricao : string.Empty,
                            FornecedorId = (s.SisFornecedor != null) ? s.SisFornecedor.Id : (long?)null
                        });

                    contarEntradas = await query
                        .CountAsync();

                    estoqueMovimentoDtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    return new ListResultDto<MovimentoIndexDto> { Items = estoqueMovimentoDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<EstoqueMovimentoDto> Obter(long id)
        {
            try
            {
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                {
                    var query = estoqueMovimentoRepository.Object
                        .GetAll().AsNoTracking()
                        .Where(m => m.Id == id);

                    var result = await query.FirstOrDefaultAsync();
                    var preMovimento = EstoqueMovimentoDto.MapMovimento(result);

                    return preMovimento;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        

        public async Task<ListResultDto<MovimentoIndexDto>> listarMovimentosValeSelecionados(ListarEstoquePreMovimentoInput input)
        {
            var listaIds = new List<long>();

            var listaIdsString = input.Filtro.TrimEnd('-').Split('-');

            foreach (var item in listaIdsString)
            {
                listaIds.Add(long.Parse(item));
            }

            var contarEntradas = 0;
            var estoqueMovimentoDtos = new List<MovimentoIndexDto>();
            try
            {
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                {
                    var query = estoqueMovimentoRepository.Object
                        .GetAll()
                        .Where(w => listaIds.Any(a => a == w.Id))
                        .Select(s => new MovimentoIndexDto
                        {
                            Id = s.Id
                                                            ,

                            DataEmissaoSaida = s.Emissao
                                                            ,
                            Documento = s.Documento
                                                            ,
                            Empresa = (s.Empresa != null) ? s.Empresa.NomeFantasia : string.Empty
                                                          ,
                            UsuarioId = s.CreatorUserId
                            ,
                            PreMovimentoEstadoId = s.PreMovimentoEstadoId

                           ,
                            IsEntrada = s.IsEntrada
                           ,
                            TipoMovimento = s.EstTipoOperacao.Descricao
                           ,
                            TipoOperacaoId = s.EstTipoOperacaoId
                           ,
                            ValorDocumento = s.TotalDocumento,
                            Fornecedor = (s.SisFornecedor != null) ? s.SisFornecedor.Descricao : string.Empty
                        });

                    contarEntradas = await query
                        .CountAsync();

                    estoqueMovimentoDtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    return new ListResultDto<MovimentoIndexDto> { Items = estoqueMovimentoDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<MovimentoItemDto>> ListarItensValesSelecionados(ListarEstoquePreMovimentoInput input)
        {
            var listaIds = new List<long>();

            var listaIdsString = input.Filtro.TrimEnd('-').Split('-');

            foreach (var item in listaIdsString)
            {
                listaIds.Add(long.Parse(item));
            }

            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
            {
                var query = estoqueMovimentoItemRepository.Object.GetAll()
                           .Where(w => listaIds.Any(a => a == w.MovimentoId))
                           .Select(s => new MovimentoItemDto
                           {
                               Id = s.Id,
                               Produto = s.Produto.Descricao,
                               Quantidade = s.Quantidade,
                               ProdutoId = s.ProdutoId,
                               CustoUnitario = s.CustoUnitario,
                               NumeroSerie = s.NumeroSerie,
                               ProdutoUnidadeId = s.ProdutoUnidadeId
                           });

                var estoqueMovimentoItemDtos = await query
                     .AsNoTracking()
                     .ToListAsync();


                foreach (var item in estoqueMovimentoItemDtos)
                {
                    var unidade = await unidadeAppService.Object.Obter((long)item.ProdutoUnidadeId);
                    if (unidade == null)
                    {
                        continue;
                    }
                    item.Quantidade = item.Quantidade / unidade.Fator;
                    item.CustoTotal = item.CustoUnitario * item.Quantidade;
                    item.Unidade = unidade.Descricao;
                }

                return new ListResultDto<MovimentoItemDto> { Items = estoqueMovimentoItemDtos };
            }
        }

        public DefaultReturn<EstoqueMovimentoDto> CriarOuEditar(EstoqueMovimentoDto input, string valesIds)
        {
            var retornoPadrao = new DefaultReturn<EstoqueMovimentoDto>
            {
                Errors = new List<ErroDto>()
            };

            input.EstTipoOperacaoId = 1;
            var movimento = EstoqueMovimentoDto.MapMovimento(input);

            try
            {
                using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
                {
                    var ids = new List<long>();

                    valesIds.TrimEnd('-').Split('-').ToList().ForEach(f => ids.Add(long.Parse(f)));
                    retornoPadrao.Errors = preMovimentoValidacaoDomainService.Object.ValidarBaixaVale(movimento, ids);
                    
                    if (retornoPadrao.Errors.Count != 0)
                    {
                        return retornoPadrao;
                    }

                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        var preMovimento = CarregarPreMovimento(input);
                        preMovimento.Id = movimento.EstoquePreMovimentoId;

                        preMovimento.PreMovimentoEstadoId = (long) EnumPreMovimentoEstado.Conferencia;

                        long estoquePreMovimentoId = 0;

                        using (var estoquePreMovimentoRepository = IocManager.Instance
                            .ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                        {
                            if (preMovimento.Id == 0)
                            {
                                estoquePreMovimentoId = AsyncHelper.RunSync(() =>
                                    estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(preMovimento));
                            }
                            else
                            {
                                AsyncHelper.RunSync(
                                    () => estoquePreMovimentoRepository.Object.UpdateAsync(preMovimento));
                                estoquePreMovimentoId = preMovimento.Id;
                            }

                            input.EstoquePreMovimentoId = estoquePreMovimentoId;
                            movimento.EstoquePreMovimentoId = estoquePreMovimentoId;
                            movimento.PreMovimentoEstadoId = (long) EnumPreMovimentoEstado.Conferencia;
                        }

                        long movimentoId = 0;
                        using (var _estoqueMovimentoRepository =
                            IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                        {
                            if (movimento.Id == 0)
                            {
                                using (var _estMovimentoBaixaRepository = IocManager.Instance
                                    .ResolveAsDisposable<IRepository<EstMovimentoBaixa, long>>())
                                {
                                    input.Id = AsyncHelper.RunSync(() =>
                                        _estoqueMovimentoRepository.Object.InsertAndGetIdAsync(movimento));

                                    foreach (var item in ids)
                                    {
                                        var baixaVale = new EstMovimentoBaixa
                                        {
                                            MovimentoBaixaPrincipalId = input.Id,
                                            MovimentoBaixaId = item
                                        };

                                        _estMovimentoBaixaRepository.Object.InsertAsync(baixaVale);
                                    }
                                }
                            }
                            else
                            {
                                AsyncHelper.RunSync(() => _estoqueMovimentoRepository.Object.UpdateAsync(movimento));
                            }
                        }

                        retornoPadrao.ReturnObject = input;

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
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

        public async Task<ListResultDto<MovimentoItemDto>> ListarBaixaConsignadosPendente(ListarEstoquePreMovimentoInput input)
        {
            var contarEntradas = 0;
            
            var estoqueMovimentoItemDtos = new List<MovimentoItemDto>();
            try
            {
                using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
                using (var estMovimentoBaixaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixaItem, long>>())
                {
                    var queryBaixaItem = estMovimentoBaixaItemRepository.Object.GetAll()
                        .GroupBy(g => g.EstoqueMovimentoItemId)
                        .Select(s => new { EstoqueMovimentoItemId = s.Key, Quantidade = s.Sum(soma => soma.Quantidade) });


                    var query =
                    from mov in estoqueMovimentoRepository.Object.GetAll()
                    join item in estoqueMovimentoItemRepository.Object.GetAll()
                    on mov.Id equals item.MovimentoId
                    into movjoin
                    from joinedmov in movjoin.DefaultIfEmpty()

                    where mov.EstTipoMovimentoId == (long)EnumTipoMovimento.Consignado_Entrada
                        && !queryBaixaItem.Any(a => a.EstoqueMovimentoItemId == joinedmov.Id && a.Quantidade >= joinedmov.Quantidade)

                      && (input.FornecedorId == null || mov.SisFornecedorId == input.FornecedorId)
                      && (input.PeridoDe == null || input.PeridoDe <= mov.Emissao)
                      && (input.PeridoAte == null || input.PeridoAte >= mov.Emissao)
                      && (string.IsNullOrEmpty(input.Filtro) ||
                        mov.SisFornecedor.Descricao.ToLower().Contains(input.Filtro.ToLower()) ||
                        mov.Empresa.NomeFantasia.ToLower().Contains(input.Filtro.ToLower()) ||
                        joinedmov.Produto.Descricao.ToLower().Contains(input.Filtro.ToLower()) ||
                        joinedmov.NumeroSerie.ToLower().Contains(input.Filtro.ToLower())
                        )

                    select new MovimentoItemDto
                    {
                        Id = joinedmov.Id,
                        Produto = joinedmov.Produto.Descricao
                           ,
                        Quantidade = joinedmov.Quantidade - (queryBaixaItem.Where(w => w.EstoqueMovimentoItemId == joinedmov.Id).FirstOrDefault() != null ?
                                                              queryBaixaItem.Where(w => w.EstoqueMovimentoItemId == joinedmov.Id).FirstOrDefault().Quantidade : 0)
                           //Quantidade = 

                           ,
                        ProdutoId = joinedmov.ProdutoId
                           ,

                        CustoUnitario = joinedmov.CustoUnitario


                               ,
                        NumeroSerie = joinedmov.NumeroSerie,
                        Fornecedor = mov.SisFornecedor.Descricao,
                        Unidade = joinedmov.ProdutoUnidade.Unidade.Descricao,
                        ProdutoUnidadeId = joinedmov.ProdutoUnidadeId
                    };

                    contarEntradas = await query
                        .CountAsync();

                    estoqueMovimentoItemDtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in estoqueMovimentoItemDtos)
                    {
                        var unidade = await unidadeAppService.Object.Obter((long)item.ProdutoUnidadeId);
                        if (unidade != null)
                        {
                            item.Quantidade = item.Quantidade / unidade.Fator;
                            item.CustoTotal = item.CustoUnitario * item.Quantidade;
                        }
                    }

                    return new ListResultDto<MovimentoItemDto> { Items = estoqueMovimentoItemDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<MovimentoItemDto>> ListarMovimentosItemConsinagdoSelecionados(ListarEstoquePreMovimentoInput input)
        {
            var listaIds = new List<long>();

            var listaIdsString = input.Filtro.TrimEnd('-').Split('-');

            foreach (var item in listaIdsString.ToList())
            {
                if (!string.IsNullOrEmpty(item))
                {
                    listaIds.Add(long.Parse(item));
                }
            }

            var contarEntradas = 0;
            var estoqueMovimentoItemDtos = new List<MovimentoItemDto>();
            try
            {
                using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
                using (var estMovimentoBaixaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixaItem, long>>())
                {
                    var queryBaixaItem = estMovimentoBaixaItemRepository.Object.GetAll()
                                          .Where(w => listaIds.Any(a => a == w.EstoqueMovimentoItemId) && w.EstoqueMovimentoBaixaId == input.MovimentoId)
                                          .GroupBy(g => g.EstoqueMovimentoItemId)
                                          .Select(s => new { EstoqueMovimentoItemId = s.Key, Quantidade = s.Sum(soma => soma.Quantidade) });


                    var queryBaixaItemRestante = estMovimentoBaixaItemRepository.Object.GetAll()
                                          .Where(w => listaIds.Any(a => a == w.EstoqueMovimentoItemId))
                                          .GroupBy(g => g.EstoqueMovimentoItemId)
                                          .Select(s => new { EstoqueMovimentoItemId = s.Key, Quantidade = s.Sum(soma => soma.Quantidade) });


                    var query = from movimentoItem in estoqueMovimentoItemRepository.Object.GetAll()
                                join estMovimentoBaixa in estMovimentoBaixaItemRepository.Object.GetAll().Where(w => w.EstoqueMovimentoBaixaId == input.MovimentoId)
                                on movimentoItem.Id equals estMovimentoBaixa.EstoqueMovimentoItemId
                                into estBaixa
                                from est in estBaixa.DefaultIfEmpty()

                                join baixaItem in queryBaixaItem//_estMovimentoBaixaItemRepository.GetAll().Where(w => w.EstoqueMovimentoBaixaId==0)
                                on movimentoItem.Id equals baixaItem.EstoqueMovimentoItemId
                                into relacionamento
                                from m in relacionamento.DefaultIfEmpty()



                                join baixaItemRestante in queryBaixaItemRestante
                                on movimentoItem.Id equals baixaItemRestante.EstoqueMovimentoItemId
                                 into relacionamentoRestante
                                from restante in relacionamentoRestante.DefaultIfEmpty()

                                where listaIds.Any(a => a == movimentoItem.Id)
                                // && (input.MovimentoId == null || input.MovimentoId == 0 || input.MovimentoId == movimentoItem.MovimentoId)



                                //&& queryBaixaItem.Any(a => a.EstoqueMovimentoItemId == movimentoItem.Id
                                //                        && a.Quantidade >= movimentoItem.Quantidade)


                                //  .Where(w => listaIds.Any(a => a == w.Id)  )

                                select (new MovimentoItemDto
                                {
                                    Id = movimentoItem.Id,
                                    Produto = movimentoItem.Produto.Descricao
                                    ,
                                    Quantidade = movimentoItem.Quantidade - ((restante.Quantidade != null) ? restante.Quantidade : 0)
                                    ,
                                    ProdutoId = movimentoItem.ProdutoId
                                    ,

                                    CustoUnitario = movimentoItem.CustoUnitario
                                        ,

                                    NumeroSerie = movimentoItem.NumeroSerie,
                                    Unidade = movimentoItem.ProdutoUnidade.Unidade.Descricao,
                                    QuantidadeBaixa = m.Quantidade,
                                    BaixaItemId = est.Id,
                                    ProdutoUnidadeId = movimentoItem.ProdutoUnidadeId
                                });


                    contarEntradas = await query
                        .CountAsync();

                    estoqueMovimentoItemDtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    foreach (var item in estoqueMovimentoItemDtos)
                    {
                        item.Quantidade = unidadeAppService.Object.ObterQuantidadePorFator((long)item.ProdutoUnidadeId, item.Quantidade);
                        if (item.QuantidadeBaixa != null)
                        {
                            item.QuantidadeBaixa = unidadeAppService.Object.ObterQuantidadePorFator((long)item.ProdutoUnidadeId, (decimal)item.QuantidadeBaixa);
                            item.CustoTotal = item.CustoUnitario * (decimal)item.QuantidadeBaixa;
                        }

                    }

                    return new ListResultDto<MovimentoItemDto> { Items = estoqueMovimentoItemDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<MovimentoItemDto> ObterItem(long id)
        {
            try
            {
                using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
                {
                    var query = estoqueMovimentoItemRepository.Object
                        .GetAll()
                        .Where(m => m.Id == id);

                    var result = await query.FirstOrDefaultAsync();

                    var movimentoItemDto = new MovimentoItemDto
                    {
                        CustoUnitario = result.CustoUnitario,
                        Id = result.Id,
                        NumeroSerie = result.NumeroSerie,
                        ProdutoId = result.ProdutoId,
                        Quantidade = result.Quantidade,
                        ProdutoUnidadeId = result.ProdutoUnidadeId,
                        MovimentoId = result.MovimentoId
                    };
                    return movimentoItemDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public DefaultReturn<EstoqueMovimentoDto> CriarOuEditarBaixaConsignado(EstoqueMovimentoDto input)
        {
            var retornoPadrao = new DefaultReturn<EstoqueMovimentoDto> {Errors = new List<ErroDto>()};

            input.EstTipoOperacaoId = 1;
            var movimento = EstoqueMovimentoDto.MapMovimento(input);

            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var ids = new List<long>();

                    //if (!retornoPadrao.Errors.Any())
                    long estoquePreMovimentoId = 0;

                    var preMovimento = CarregarPreMovimento(input);
                    preMovimento.PreMovimentoEstadoId = (long) EnumPreMovimentoEstado.Conferencia;

                    using (var _estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                    using (var _estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                    {
                        if (input.Id == 0)
                        {
                            estoquePreMovimentoId = AsyncHelper.RunSync(() => _estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(preMovimento));
                        }
                        else
                        {
                            estoquePreMovimentoId = movimento.EstoquePreMovimentoId;
                            preMovimento.Id = estoquePreMovimentoId;
                            AsyncHelper.RunSync(() => _estoquePreMovimentoRepository.Object.UpdateAsync(preMovimento));
                        }


                        movimento.EstoquePreMovimentoId = estoquePreMovimentoId;
                        movimento.PreMovimentoEstadoId = (long) EnumPreMovimentoEstado.Conferencia;

                        if (input.Id == 0)
                        {
                            movimento.Id = AsyncHelper.RunSync(() => _estoqueMovimentoRepository.Object.InsertAndGetIdAsync(movimento));
                        }
                        else
                        {
                            AsyncHelper.RunSync(() => _estoqueMovimentoRepository.Object.UpdateAsync(movimento));
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();

                    retornoPadrao.ReturnObject = EstoqueMovimentoDto.MapMovimento(movimento);
                }

                return retornoPadrao;

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

        public DefaultReturn<EstoqueMovimentoDto> CriarOuEditarBaixaItemConsignado(MovimentoItemDto input)
        {

            var retornoPadrao = new DefaultReturn<EstoqueMovimentoDto> ();

            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                using (var estMovimentoBaixaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixaItem, long>>())
                {
                    var estMovimentoBaixaItem = new EstMovimentoBaixaItem
                    {
                        EstoqueMovimentoBaixaId = input.MovimentoId,
                        EstoqueMovimentoItemId = input.Id,
                        Quantidade = input.Quantidade
                    };
                    AsyncHelper.RunSync(() => estMovimentoBaixaItemRepository.Object.InsertAsync(estMovimentoBaixaItem));


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
                    retornoPadrao.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    retornoPadrao.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return retornoPadrao;
        }

        public async Task<DefaultReturn<MovimentoItemDto>> CriarOuEditarBaixaItem(MovimentoItemDto input)
        {
            var retornoPadrao = new DefaultReturn<MovimentoItemDto>();

            var baixaItem = new EstMovimentoBaixaItem();

            try
            {
                if (!retornoPadrao.Errors.Any())
                {
                    var movimentoItemDto = new MovimentoItemDto();
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    using (var estMovimentoBaixaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixaItem, long>>())
                    using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                    {
                        var quantidadeBaixaReferencia = unidadeAppService.Object.ObterQuantidadeReferencia((long)input.ProdutoUnidadeId, (decimal)input.QuantidadeBaixa);
                        if (input.BaixaItemId == null || input.BaixaItemId.Equals((long?)0))
                        {
                            baixaItem = new EstMovimentoBaixaItem
                            {
                                EstoqueMovimentoBaixaId = input.MovimentoId,
                                EstoqueMovimentoItemId = input.Id,
                                Quantidade = quantidadeBaixaReferencia
                            };


                            input.Id = await estMovimentoBaixaItemRepository.Object.InsertAndGetIdAsync(baixaItem);
                        }
                        else
                        {
                            baixaItem = await estMovimentoBaixaItemRepository.Object.GetAsync((long)input.BaixaItemId);

                            if (baixaItem != null)
                            {
                                baixaItem.Quantidade = quantidadeBaixaReferencia;

                                await estMovimentoBaixaItemRepository.Object.UpdateAsync(baixaItem);
                            }
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
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

        private static EstoquePreMovimento CarregarPreMovimento(EstoqueMovimentoDto movimentoDto)
        {
            var estoquePreMovimento = new EstoquePreMovimento
            {
                Id = movimentoDto.EstoquePreMovimentoId,
                AcrescimoDecrescimo = movimentoDto.AcrescimoDecrescimo,
                AplicacaoDireta = movimentoDto.AplicacaoDireta,
                AtendimentoId = movimentoDto.AtendimentoId,
                CentroCustoId = movimentoDto.CentroCustoId,
                CFOPId = movimentoDto.CFOPId,
                Consiginado = movimentoDto.Consiginado,
                Contabiliza = movimentoDto.Contabiliza,
                DescontoPer = movimentoDto.DescontoPer,
                Documento = movimentoDto.Documento,
                Emissao = movimentoDto.Emissao,
                EmpresaId = movimentoDto.EmpresaId,
                EntragaProduto = movimentoDto.EntragaProduto,
                EstoqueId = movimentoDto.EstoqueId,
                EstTipoMovimentoId = movimentoDto.EstTipoMovimentoId,
                EstTipoOperacaoId = movimentoDto.EstTipoOperacaoId,
                SisFornecedorId = movimentoDto.FornecedorId,
                FretePer = movimentoDto.FretePer,
                Frete_SisFornecedorId = movimentoDto.Frete_FornecedorId,
                ICMSPer = movimentoDto.ICMSPer,
                IsEntrada = movimentoDto.IsEntrada,
                MedicoSolcitanteId = movimentoDto.MedicoSolcitanteId,
                MotivoPerdaProdutoId = movimentoDto.MotivoPerdaProdutoId,
                Movimento = movimentoDto.Movimento,
                Observacao = movimentoDto.Observacao,
                OrdemId = movimentoDto.OrdemId,
                PacienteId = movimentoDto.PacienteId,
                PreMovimentoEstadoId = movimentoDto.PreMovimentoEstadoId,
                Quantidade = movimentoDto.Quantidade,
                Serie = movimentoDto.Serie,
                TipoFreteId = movimentoDto.TipoFreteId,
                TotalDocumento = movimentoDto.TotalDocumento,
                TotalProduto = movimentoDto.TotalProduto,
                UnidadeOrganizacionalId = movimentoDto.UnidadeOrganizacionalId,
                ValorFrete = movimentoDto.ValorFrete,
                ValorICMS = movimentoDto.ValorICMS
            };

            return estoquePreMovimento;
        }

        public async Task<ListResultDto<MovimentoIndexDto>> ListarVales(ListarEstoquePreMovimentoInput input)
        {

            var contarEntradas = 0;
            var estoqueMovimentoDtos = new List<MovimentoIndexDto>();


            var movimentoId = long.Parse(input.Filtro);
            try
            {
                using (var _estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                using (var _estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var _estMovimentoBaixaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixa, long>>())
                {

                    var query = from movimento in _estoqueMovimentoRepository.Object.GetAll()
                                join baixa in _estMovimentoBaixaRepository.Object.GetAll()
                                on movimento.Id equals baixa.MovimentoBaixaId

                                join movimentoPrincipal in _estoqueMovimentoRepository.Object.GetAll()
                                on baixa.MovimentoBaixaPrincipalId equals movimentoPrincipal.Id

                                join preMovimento in _estoquePreMovimentoRepository.Object.GetAll()
                                on movimentoPrincipal.EstoquePreMovimentoId equals preMovimento.Id



                                where (preMovimento.Id == movimentoId)
                                select (new MovimentoIndexDto
                                {
                                    Id = movimento.Id
                                                                    ,

                                    DataEmissaoSaida = movimento.Emissao
                                                                    ,
                                    Documento = movimento.Documento
                                                                    ,
                                    Empresa = (movimento.Empresa != null) ? movimento.Empresa.NomeFantasia : string.Empty
                                                                  ,
                                    UsuarioId = movimento.CreatorUserId
                                    ,
                                    PreMovimentoEstadoId = movimento.PreMovimentoEstadoId

                                   ,
                                    IsEntrada = movimento.IsEntrada
                                   ,
                                    TipoMovimento = movimento.EstTipoOperacao.Descricao
                                   ,
                                    TipoOperacaoId = movimento.EstTipoOperacaoId
                                   ,
                                    ValorDocumento = movimento.TotalDocumento,
                                    Fornecedor = (movimento.SisFornecedor != null) ? movimento.SisFornecedor.Descricao : string.Empty
                                });

                    contarEntradas = await query
                        .CountAsync();

                    estoqueMovimentoDtos = await query
                        .AsNoTracking()
                        .ToListAsync();

                    return new ListResultDto<MovimentoIndexDto> { Items = estoqueMovimentoDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<MovimentoIndexDto>> ListarNota(ListarEstoquePreMovimentoInput input)
        {

            var contarEntradas = 0;
            var estoqueMovimentoDtos = new List<MovimentoIndexDto>();

            var movimentoId = long.Parse(input.Filtro);
            try
            {
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estMovimentoBaixaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixa, long>>())
                {
                    var query = from movimento in estoqueMovimentoRepository.Object.GetAll()
                                join baixa in estMovimentoBaixaRepository.Object.GetAll()
                                on movimento.Id equals baixa.MovimentoBaixaPrincipalId

                                join movimentoPrincipal in estoqueMovimentoRepository.Object.GetAll()
                                on baixa.MovimentoBaixaId equals movimentoPrincipal.Id

                                join preMovimento in estoquePreMovimentoRepository.Object.GetAll()
                                on movimentoPrincipal.EstoquePreMovimentoId equals preMovimento.Id

                                where (preMovimento.Id == movimentoId)
                                select (new MovimentoIndexDto
                                {
                                    Id = movimento.Id
                                                                    ,

                                    DataEmissaoSaida = movimento.Emissao
                                                                    ,
                                    Documento = movimento.Documento
                                                                    ,
                                    Empresa = (movimento.Empresa != null) ? movimento.Empresa.NomeFantasia : string.Empty
                                                                  ,
                                    UsuarioId = movimento.CreatorUserId
                                    ,
                                    PreMovimentoEstadoId = movimento.PreMovimentoEstadoId

                                   ,
                                    IsEntrada = movimento.IsEntrada
                                   ,
                                    TipoMovimento = movimento.EstTipoOperacao.Descricao
                                   ,
                                    TipoOperacaoId = movimento.EstTipoOperacaoId
                                   ,
                                    ValorDocumento = movimento.TotalDocumento,
                                    Fornecedor = (movimento.SisFornecedor != null) ? movimento.SisFornecedor.Descricao : string.Empty
                                });

                    contarEntradas = await query
                        .CountAsync();

                    estoqueMovimentoDtos = await query
                        .AsNoTracking()
                        .ToListAsync();


                    return new ListResultDto<MovimentoIndexDto> { Items = estoqueMovimentoDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<MovimentoItemDto>> ListarItensConsignados(ListarEstoquePreMovimentoInput input)
        {
            var preMovimentoId = long.Parse(input.Filtro);
            var estoqueMovimentoDtos = new List<MovimentoItemDto>();

            try
            {
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
                using (var estMovimentoBaixaItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstMovimentoBaixaItem, long>>())
                using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                {

                    var query = from movimento in estoqueMovimentoRepository.Object.GetAll()
                                join baixaItem in estMovimentoBaixaItemRepository.Object.GetAll()
                                on movimento.Id equals baixaItem.EstoqueMovimentoBaixaId

                                join movimentoItem in estoqueMovimentoItemRepository.Object.GetAll()
                                on baixaItem.EstoqueMovimentoItemId equals movimentoItem.Id

                                where movimento.EstoquePreMovimentoId == preMovimentoId

                                select (new MovimentoItemDto
                                {
                                    Id = movimentoItem.Id,
                                    Produto = movimentoItem.Produto.Descricao
                                            ,
                                    Quantidade = baixaItem.Quantidade
                                            ,
                                    ProdutoId = movimentoItem.ProdutoId
                                            ,

                                    CustoUnitario = movimentoItem.CustoUnitario
                                                ,
                                    NumeroSerie = movimentoItem.NumeroSerie,
                                    Unidade = movimentoItem.ProdutoUnidade.Unidade.Descricao,
                                    ProdutoUnidadeId = movimentoItem.ProdutoUnidadeId
                                });

                    var contarEntradas = await query
                          .CountAsync();

                    estoqueMovimentoDtos = await query
                         .AsNoTracking()
                         .ToListAsync();



                    foreach (var item in estoqueMovimentoDtos)
                    {
                        var unidade = await unidadeAppService.Object.Obter((long)item.ProdutoUnidadeId);
                        if (unidade != null)
                        {
                            item.Quantidade = item.Quantidade / unidade.Fator;
                            item.CustoTotal = item.Quantidade * item.CustoUnitario;
                            item.Unidade = unidade.Descricao;
                        }
                    }

                    return new ListResultDto<MovimentoItemDto> { Items = estoqueMovimentoDtos };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}

