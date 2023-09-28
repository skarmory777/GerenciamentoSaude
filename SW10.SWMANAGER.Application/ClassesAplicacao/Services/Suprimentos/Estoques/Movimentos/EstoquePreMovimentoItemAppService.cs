using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
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
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices;
using static SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.EstoquePreMovimentoAppService;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class EstoquePreMovimentoItemAppService : SWMANAGERAppServiceBase, IEstoquePreMovimentoItemAppService
    {
        public async Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarSaidaPorCodigoBarra(string codigoBarra, long? estoqueId, long? preMovimentoId, decimal? quantidade)
        {
            using (var codigoBarraAppService = IocManager.Instance.ResolveAsDisposable<ICodigoBarraAppService>())
            {
                var dadosEtiqueta = await codigoBarraAppService.Object.ObterValorEtiqueta(codigoBarra);

                if (dadosEtiqueta != null)
                {
                    if (dadosEtiqueta.TipoEtiquetaCodigoBarra == Enumeradores.EnumTipoEtiquetaCodigoBarra.LoteValidade)
                    {
                        var estoquePreMovimentoItemDto = new EstoquePreMovimentoItemDto
                        {
                            LoteValidadeId = dadosEtiqueta.LoteValidadeId,
                            LaboratorioId = dadosEtiqueta.LoteValidade.ProdutoLaboratorioId,
                            Lote = dadosEtiqueta.LoteValidade.Lote,
                            Validade = dadosEtiqueta.LoteValidade.Validade,
                            ProdutoId = dadosEtiqueta.LoteValidade.Produto.Id,
                            Quantidade = quantidade ?? 1,
                            EstoqueId = estoqueId ?? 0,
                            ProdutoUnidadeId = dadosEtiqueta.UnidadeProdutoId,
                            PreMovimentoId = preMovimentoId ?? 0
                        };

                        return await CriarOuEditarSaida(estoquePreMovimentoItemDto);

                    }
                }
                else
                {
                    var defaultReturn = new DefaultReturn<EstoquePreMovimentoItemDto>
                    {
                        Errors = new List<ErroDto> { new ErroDto { CodigoErro = "ECB0001" } }
                    };
                    return defaultReturn;
                }
            }

            return null;
        }

        public async Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarKitEstoqueItem(EstoquePreMovimentoKitEstoqueItemDto input)
        {
            var _retornoPadrao = new DefaultReturn<EstoquePreMovimentoItemDto>();

            try
            {
                var estoquePreMovimentoItem = new EstoquePreMovimentoItem();

                using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<EstoquePreMovimentoValidacaoDomainService>())
                using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estoqueKitItemService = IocManager.Instance.ResolveAsDisposable<IEstoqueKitItemAppService>())
                using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                using (var produtoAppService = IocManager.Instance.ResolveAsDisposable<IProdutoAppService>())
                using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                {
                    // Obtêm somente os itens (Produtos) que fazem parte do estoque selecionado no Pré Movimento
                    var estoqueKitItens = await estoqueKitItemService.Object.ListarPeloKitEstoqueIdEEstoqueId(input.KitEstoqueId, input.EstoqueId);

                    foreach (var estoqueKitItem in estoqueKitItens)
                    {
                        var estoquePreMovimentoItemDto = new EstoquePreMovimentoItemDto();

                        estoquePreMovimentoItemDto.Produto = estoqueKitItem.Produto;
                        estoquePreMovimentoItemDto.CreationTime = DateTime.Now;
                        estoquePreMovimentoItemDto.EstoqueId = input.EstoqueId;
                        estoquePreMovimentoItemDto.PreMovimentoId = input.PreMovimentoId;
                        estoquePreMovimentoItemDto.ProdutoId = estoqueKitItem.Produto.Id;
                        estoquePreMovimentoItemDto.Quantidade = (estoqueKitItem.Quantidade * input.Quantidade);

                        var unidadeReferencial = await produtoAppService.Object.ObterUnidadePorTipo(estoquePreMovimentoItemDto.Produto.Id, 1);

                        if (unidadeReferencial != null) {
                            estoquePreMovimentoItemDto.ProdutoUnidadeId = unidadeReferencial.Id;
                        }
                        
                        estoquePreMovimentoItemDto.EstoqueKitItemId = estoqueKitItem.Id;

                        if (!estoquePreMovimentoItemDto.Produto.IsKit)
                        {
                            var preMovimento = estoquePreMovimentoRepository.Object.Get(estoquePreMovimentoItemDto.PreMovimentoId);

                            var validarDataVencimeneto = preMovimento.EstTipoMovimentoId != (long)EnumTipoMovimento.Perda_Saida;

                            estoquePreMovimentoItemDto.Quantidade = unidadeAppService.Object.ObterQuantidadeReferencia((long)estoquePreMovimentoItemDto.ProdutoUnidadeId, estoquePreMovimentoItemDto.Quantidade);

                            await preMovimentoValidacaoDomainService.Object.ValidarItemSaida(estoquePreMovimentoItemDto, (estoqueKitItem.Produto != null && (estoqueKitItem.Produto.IsValidade || estoqueKitItem.Produto.IsLote)), validarDataVencimeneto, _retornoPadrao.Errors);

                            if (_retornoPadrao.Errors.Count() == 0)
                            {
                                using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                                {
                                    estoquePreMovimentoItem = EstoquePreMovimentoItemDto.MapPreMovimentoItem(estoquePreMovimentoItemDto);
                                    estoquePreMovimentoItem.Quantidade = unidadeAppService.Object.ObterQuantidadeReferencia((long)estoquePreMovimentoItem.ProdutoUnidadeId, estoquePreMovimentoItem.Quantidade);
                                    if (estoquePreMovimentoItemDto.Id.Equals(0))
                                    {
                                        estoquePreMovimentoItemDto.Id = await estoquePreMovimentoItemRepository.Object.InsertAndGetIdAsync(estoquePreMovimentoItem);
                                    }
                                    else
                                    {
                                        await estoquePreMovimentoItemRepository.Object.UpdateAsync(estoquePreMovimentoItem);
                                    }

                                    if (estoquePreMovimentoItemDto.Produto != null && (estoquePreMovimentoItemDto.Produto.IsValidade || estoquePreMovimentoItemDto.Produto.IsLote) && ((estoquePreMovimentoItemDto.LaboratorioId != null && estoquePreMovimentoItemDto.Validade != null) || (estoquePreMovimentoItemDto.LoteValidadeId != 0 || estoquePreMovimentoItemDto.LoteValidadeId != null)))
                                    {
                                        var quantidadeSolicitada = estoquePreMovimentoItemDto.Quantidade;
                                        var quantidadeResidual = estoquePreMovimentoItemDto.Quantidade;

                                        // Obtêm os Lotes Validade do item
                                        var lotesValidadesItem = await estoqueLoteValidadeAppService.Object.ObterPorProdutoEstoqueLaboratorio(estoquePreMovimentoItemDto.ProdutoId, estoquePreMovimentoItemDto.EstoqueId, null);

                                        foreach (var loteValidade in lotesValidadesItem)
                                        {
                                            var quantidade = quantidadeResidual >= loteValidade.Quantidade ? loteValidade.Quantidade : quantidadeResidual;

                                            var estoquePreMovimentoLoteValidadeDto = new EstoquePreMovimentoLoteValidadeDto();

                                            estoquePreMovimentoLoteValidadeDto.LaboratorioId = (estoquePreMovimentoItemDto.LaboratorioId != null ? (long)estoquePreMovimentoItemDto.LaboratorioId : 0);
                                            estoquePreMovimentoLoteValidadeDto.Lote = estoquePreMovimentoItemDto.Lote;
                                            estoquePreMovimentoLoteValidadeDto.Validade = (estoquePreMovimentoItemDto.Validade != null ? (DateTime)estoquePreMovimentoItemDto.Validade : DateTime.MinValue);
                                            estoquePreMovimentoLoteValidadeDto.ProdutoId = estoquePreMovimentoItemDto.ProdutoId;
                                            estoquePreMovimentoLoteValidadeDto.EstoquePreMovimentoItemId = estoquePreMovimentoItemDto.Id;
                                            estoquePreMovimentoLoteValidadeDto.Quantidade = (decimal) quantidade;
                                            estoquePreMovimentoLoteValidadeDto.LoteValidadeId = loteValidade.Id;

                                            var retorno = estoqueLoteValidadeAppService.Object.CriarOuEditar(estoquePreMovimentoLoteValidadeDto);
                                            _retornoPadrao.Errors.AddRange(retorno.Errors);

                                            quantidadeResidual = quantidadeResidual - (decimal) quantidade;

                                            if (quantidadeResidual == 0)
                                            {
                                                break;
                                            }
                                        }
                                    }

                                    unitOfWork.Complete();
                                    unitOfWorkManager.Object.Current.SaveChanges();
                                    unitOfWork.Dispose();

                                    if (estoquePreMovimentoItemDto.Produto != null && !(estoquePreMovimentoItemDto.Produto.IsValidade || estoquePreMovimentoItemDto.Produto.IsLote) && (estoquePreMovimentoItemDto.LaboratorioId != null && estoquePreMovimentoItemDto.Validade != null))
                                    {
                                        estoquePreMovimentoItem.Produto.ContaAdministrativa = null;
                                        await produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItem(estoquePreMovimentoItem);
                                    }

                                    var query = await Obter(estoquePreMovimentoItemDto.Id);
                                    var newObj = EstoquePreMovimentoItemDto.MapPreMovimentoItem(query);

                                    var estoquePreMovimentoItemAuxDto = EstoquePreMovimentoItemDto.MapPreMovimentoItem(newObj);

                                    _retornoPadrao.ReturnObject = estoquePreMovimentoItemAuxDto;
                                }
                            }
                        }
                        else
                        {
                            if (_retornoPadrao.Errors == null)
                            {
                                _retornoPadrao.Errors = new List<ErroDto>();
                            }

                            using (var estoqueKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueKit, long>>())
                            {
                                var etoqueKit = estoqueKitRepository.Object.GetAll()
                                                                           .Include(i => i.Itens)
                                                                           .Where(w => w.ProdutoId == estoquePreMovimentoItemDto.ProdutoId)
                                                                           .FirstOrDefault();

                                foreach (var item in etoqueKit.Itens)
                                {
                                    EstoquePreMovimentoItemDto itemKit = new EstoquePreMovimentoItemDto
                                    {
                                        EstoqueId = estoquePreMovimentoItemDto.EstoqueId,
                                        PreMovimentoId = estoquePreMovimentoItemDto.PreMovimentoId,
                                        ProdutoId = item.ProdutoId,
                                        ProdutoUnidadeId = item.UnidadeId,
                                        Quantidade = item.Quantidade
                                    };

                                    var retorno = await CriarOuEditarSaida(itemKit);

                                    _retornoPadrao.Errors.AddRange(retorno.Errors);

                                }
                            }
                        }
                    }
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

            _retornoPadrao.Warnings = _retornoPadrao.Warnings ?? new List<ErroDto>();

            return _retornoPadrao;
        }

        public async Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarOuEditarSaida(EstoquePreMovimentoItemDto input)
        {
            var _retornoPadrao = new DefaultReturn<EstoquePreMovimentoItemDto>();

            try
            {
                var estoquePreMovimentoItem = new EstoquePreMovimentoItem();

                using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<EstoquePreMovimentoValidacaoDomainService>())
                using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                {
                    var produto = produtoRepository.Object.Get(input.ProdutoId);
                    input.Produto = ProdutoDto.Mapear(produto);

                    if (!input.Produto.IsKit)
                    {
                        var preMovimento = estoquePreMovimentoRepository.Object.Get(input.PreMovimentoId);

                        var validarDataVencimeneto = preMovimento.EstTipoMovimentoId != (long)EnumTipoMovimento.Perda_Saida;

                        input.Quantidade = unidadeAppService.Object.ObterQuantidadeReferencia((long)input.ProdutoUnidadeId, input.Quantidade);

                        await preMovimentoValidacaoDomainService.Object.ValidarItemSaida(input, (produto != null && (produto.IsValidade || produto.IsLote)), validarDataVencimeneto, _retornoPadrao.Errors);

                        if (_retornoPadrao.Errors.Count() == 0)
                        {
                            using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                            using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                            using (var unitOfWork = unitOfWorkManager.Object.Begin())
                            {
                                estoquePreMovimentoItem = EstoquePreMovimentoItemDto.MapPreMovimentoItem(input);
                                if (input.Id.Equals(0))
                                {
                                    input.Id = await estoquePreMovimentoItemRepository.Object.InsertAndGetIdAsync(estoquePreMovimentoItem);
                                }
                                else
                                {
                                    await estoquePreMovimentoItemRepository.Object.UpdateAsync(estoquePreMovimentoItem);
                                }


                                if (produto != null && (produto.IsValidade || produto.IsLote) && ((input.LaboratorioId != null && input.Validade != null) || (input.LoteValidadeId != 0 || input.LoteValidadeId != null)))
                                {
                                    var estoquePreMovimentoLoteValidadeDto = new EstoquePreMovimentoLoteValidadeDto
                                    {
                                        Id = input.EstoquePreMovimentoLoteValidadeId
                                                                                                                    ,
                                        LaboratorioId = input.LaboratorioId != null ? (long)input.LaboratorioId : 0
                                                                                                                    ,
                                        Lote = input.Lote
                                                                                                                    ,
                                        Validade = input.Validade != null ? (DateTime)input.Validade : DateTime.MinValue
                                                                                                                    ,
                                        ProdutoId = input.ProdutoId
                                                                                                                    ,
                                        EstoquePreMovimentoItemId = input.Id
                                                                                                                    ,
                                        Quantidade = input.Quantidade
                                                                                                                    ,
                                        LoteValidadeId = input.LoteValidadeId != null ? (long)input.LoteValidadeId : 0
                                    };
                                    var retorno = estoqueLoteValidadeAppService.Object.CriarOuEditar(estoquePreMovimentoLoteValidadeDto);

                                    _retornoPadrao.Errors.AddRange(retorno.Errors);
                                }

                                unitOfWork.Complete();
                                unitOfWorkManager.Object.Current.SaveChanges();
                                unitOfWork.Dispose();

                                if (produto != null && !(produto.IsValidade || produto.IsLote) && (input.LaboratorioId != null && input.Validade != null))
                                {
                                    estoquePreMovimentoItem.Produto.ContaAdministrativa = null;
                                    await produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItem(estoquePreMovimentoItem);
                                }

                                var query = await Obter(input.Id);
                                var newObj = EstoquePreMovimentoItemDto.MapPreMovimentoItem(query);

                                var estoquePreMovimentoItemDto = EstoquePreMovimentoItemDto.MapPreMovimentoItem(newObj);

                                _retornoPadrao.ReturnObject = estoquePreMovimentoItemDto;
                            }
                        }
                    }
                    else
                    {
                        if (_retornoPadrao.Errors == null)
                        {
                            _retornoPadrao.Errors = new List<ErroDto>();
                        }

                        using (var estoqueKitRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueKit, long>>())
                        {
                            var etoqueKit = estoqueKitRepository.Object.GetAll()
                                                                       .Include(i => i.Itens)
                                                                       .Where(w => w.ProdutoId == input.ProdutoId)
                                                                       .FirstOrDefault();

                            foreach (var item in etoqueKit.Itens)
                            {
                                EstoquePreMovimentoItemDto itemKit = new EstoquePreMovimentoItemDto
                                {
                                    EstoqueId = input.EstoqueId,
                                    PreMovimentoId = input.PreMovimentoId,
                                    ProdutoId = item.ProdutoId,
                                    ProdutoUnidadeId = item.UnidadeId,
                                    Quantidade = item.Quantidade
                                };

                                var retorno = await CriarOuEditarSaida(itemKit);

                                _retornoPadrao.Errors.AddRange(retorno.Errors);

                            }
                        }
                    }
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

            _retornoPadrao.Warnings = _retornoPadrao.Warnings ?? new List<ErroDto>();

            return _retornoPadrao;
        }

        public async Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarOuEditar(EstoquePreMovimentoItemDto input)
        {
            var _retornoPadrao = new DefaultReturn<EstoquePreMovimentoItemDto>();
            try
            {
                using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
                {
                    _retornoPadrao.Errors = preMovimentoValidacaoDomainService.Object.ValidarItem(input);

                    if (_retornoPadrao.Errors.Any())
                    {
                        return _retornoPadrao;
                    }

                    var estoquePreMovimentoItem = new EstoquePreMovimentoItem();
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                    using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                    using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        estoquePreMovimentoItem = EstoquePreMovimentoItemDto.MapPreMovimentoItem(input);//.MapTo<EstoquePreMovimentoItem>();
                        estoquePreMovimentoItem.Quantidade = unidadeAppService.Object.ObterQuantidadeReferencia((long)estoquePreMovimentoItem.ProdutoUnidadeId, estoquePreMovimentoItem.Quantidade);
                        if (input.Id.Equals(0))
                        {
                            input.Id = await estoquePreMovimentoItemRepository.Object.InsertAndGetIdAsync(estoquePreMovimentoItem);
                        }
                        else
                        {
                            await estoquePreMovimentoItemRepository.Object.UpdateAsync(estoquePreMovimentoItem);
                        }
                        var itemDto = await Obter(input.Id);
                        var itemEntity = EstoquePreMovimentoItemDto.MapPreMovimentoItem(itemDto);

                        var estoquePreMovimentoItemDto = EstoquePreMovimentoItemDto.MapPreMovimentoItem(itemEntity);

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        _retornoPadrao.ReturnObject = estoquePreMovimentoItemDto;
                        if (estoquePreMovimentoItem.Produto == null || estoquePreMovimentoItem.Produto.IsValidade || estoquePreMovimentoItem.Produto.IsLote)
                        {
                            return _retornoPadrao;
                        }

                        await produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItem(itemEntity);
                    }
                    return _retornoPadrao;
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

        public async Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarOuEditarTransferencia(EstoquePreMovimentoItemDto input, long transferenciaProdutoId, long transferenciaProdutoItemId)
        {
            using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
            using (var estoqueTransferenciaProdutoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProdutoItem, long>>())
            using (var estoqueTransferenciaProdutoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProduto, long>>())
            {
                var transferencia = estoqueTransferenciaProdutoRepository.Object.Get(transferenciaProdutoId);
                var itemTransferencia = new EstoqueTransferenciaProdutoItem();


                EstoquePreMovimentoItemDto entradaItemDto = new EstoquePreMovimentoItemDto();

                entradaItemDto.EstoquePreMovimentoLoteValidadeId = input.EstoquePreMovimentoLoteValidadeId;
                entradaItemDto.NumeroSerie = input.NumeroSerie;
                entradaItemDto.PreMovimentoId = transferencia.PreMovimentoEntradaId;
                entradaItemDto.ProdutoId = input.ProdutoId;
                entradaItemDto.ProdutoUnidadeId = input.ProdutoUnidadeId;
                entradaItemDto.Quantidade = input.Quantidade;



                if (transferenciaProdutoItemId != 0)
                {
                    itemTransferencia = estoqueTransferenciaProdutoItemRepository.Object.Get(transferenciaProdutoItemId);
                    entradaItemDto.Id = itemTransferencia.PreMovimentoEntradaItemId;
                    input.Id = itemTransferencia.PreMovimentoSaidaItemId;
                }


                input.PreMovimentoId = transferencia.PreMovimentoSaidaId;

                var retornoSaida = await CriarOuEditarSaida(input);
                var retornoEntrada = await CriarOuEditar(entradaItemDto);

                if (retornoSaida.Errors.Count == 0 && retornoEntrada.Errors.Count == 0 && input.Produto != null && (input.Produto.IsValidade || input.Produto.IsLote))
                {
                    var estoquePreMovimentoLoteValidadeDto = new EstoquePreMovimentoLoteValidadeDto
                    {
                        Id = input.EstoquePreMovimentoLoteValidadeId
                                                                                                    ,
                        LaboratorioId = input.LaboratorioId != null ? (long)input.LaboratorioId : 0
                                                                                                    ,
                        Lote = input.Lote
                                                                                                    ,
                        Validade = input.Validade != null ? (DateTime)input.Validade : DateTime.MinValue
                                                                                                    ,
                        ProdutoId = input.ProdutoId
                                                                                                    ,
                        EstoquePreMovimentoItemId = entradaItemDto.Id
                                                                                                    ,
                        Quantidade = input.Quantidade
                                                                                                    ,
                        LoteValidadeId = input.LoteValidadeId != null ? (long)input.LoteValidadeId : 0
                    };

                    var retornoLoteValidade = estoqueLoteValidadeAppService.Object.CriarOuEditar(estoquePreMovimentoLoteValidadeDto);

                    retornoEntrada.Errors.AddRange(retornoLoteValidade.Errors);
                }

                retornoEntrada.Errors.AddRange(retornoSaida.Errors);

                if (retornoEntrada.Errors.Count == 0)
                {
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        if (transferenciaProdutoItemId == 0)
                        {
                            var estoqueTransferenciaProdutoItem = new EstoqueTransferenciaProdutoItem
                            {
                                EstoqueTransferenciaProdutoID = transferenciaProdutoId,
                                PreMovimentoSaidaItemId = retornoSaida.ReturnObject.Id,
                                PreMovimentoEntradaItemId = retornoEntrada.ReturnObject.Id
                            };

                            await estoqueTransferenciaProdutoItemRepository.Object.InsertAndGetIdAsync(estoqueTransferenciaProdutoItem);
                        }
                        else
                        {
                            var estoqueTransferenciaProdutoItem = estoqueTransferenciaProdutoItemRepository.Object.Get(transferenciaProdutoItemId);

                            estoqueTransferenciaProdutoItem.EstoqueTransferenciaProdutoID = transferenciaProdutoId;
                            estoqueTransferenciaProdutoItem.PreMovimentoSaidaItemId = retornoSaida.ReturnObject.Id;
                            estoqueTransferenciaProdutoItem.PreMovimentoEntradaItemId = retornoEntrada.ReturnObject.Id;

                            await estoqueTransferenciaProdutoItemRepository.Object.UpdateAsync(estoqueTransferenciaProdutoItem);
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }

                return retornoEntrada;
            }
        }

        public async Task<EstoquePreMovimentoItemDto> Obter(long id)
        {
            try
            {
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {
                    var query = estoquePreMovimentoItemRepository.Object.GetAll()
                        .Include(i => i.Produto)
                        .Include(i => i.ProdutoUnidade)
                        .Where(w => w.Id == id).FirstOrDefault();

                    var entrada = EstoquePreMovimentoItemDto.MapPreMovimentoItem(query);

                    return entrada;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task Editar(EstoquePreMovimentoItemDto input)
        {
            try
            {
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {
                    await estoquePreMovimentoItemRepository.Object.UpdateAsync(EstoquePreMovimentoItemDto.MapPreMovimentoItem(input));// MapTo<EstoquePreMovimentoItem>());
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task ExcluirTodosItensKitEstoque(long id, long estoqueKitItemId)
        {
            try
            {
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var estoqueKitItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueKitItem, long>>())
                using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        var estoquePreMovimentoItem = estoquePreMovimentoItemRepository.Object.Get(id);
                        var estoqueKitItem = estoqueKitItemRepository.Object.Get(estoqueKitItemId);

                        var estoquePreMovimentoItens = estoquePreMovimentoItemRepository.Object.GetAll()
                                                                                            .Include(x => x.EstoqueKitItem)
                                                                                            .Include(x => x.EstoqueKitItem.EstoqueKit)
                                                                                            .Where(x => x.EstoqueKitItem.EstoqueKit.Id == estoqueKitItem.EstoqueKitId && x.PreMovimentoId == estoquePreMovimentoItem.PreMovimentoId).ToList();

                        var estoquePreMovimentoItensIds = estoquePreMovimentoItens.Select(s => s.Id).ToList();

                        var itemLotesValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                                                .Where(w => estoquePreMovimentoItensIds.Contains(w.EstoquePreMovimentoItemId)).ToList();

                        foreach (var item in itemLotesValidade)
                        {
                            await estoqueLoteValidadeAppService.Object.Excluir(EstoqueLoteValidadeAppService.MapLoteValidade(item));
                        }

                        foreach (var item in estoquePreMovimentoItens)
                        {
                            await estoquePreMovimentoItemRepository.Object.DeleteAsync(item.Id);

                            var estoquePreMovimentoItemSaldo = new EstoquePreMovimentoItem
                            {
                                PreMovimentoId = item.PreMovimentoId,
                                ProdutoId = item.ProdutoId
                            };

                            await produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItem(estoquePreMovimentoItemSaldo);
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }


        public async Task Excluir(long id)
        {
            try
            {
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        var estoquePreMovimentoItem = estoquePreMovimentoItemRepository.Object.Get(id);
                        var itemLotesValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                                                .Where(w => w.EstoquePreMovimentoItemId == id).ToList();

                        foreach (var item in itemLotesValidade)
                        {
                            await estoqueLoteValidadeAppService.Object.Excluir(EstoqueLoteValidadeAppService.MapLoteValidade(item));
                        }

                        await estoquePreMovimentoItemRepository.Object.DeleteAsync(id);

                        var estoquePreMovimentoItemSaldo = new EstoquePreMovimentoItem
                        {
                            PreMovimentoId = estoquePreMovimentoItem.PreMovimentoId,
                            ProdutoId = estoquePreMovimentoItem.ProdutoId
                        };

                        await produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItem(estoquePreMovimentoItemSaldo);

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }


        public async Task ExcluirPorPreMovimento(long id)
        {

            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            {
                var itens = estoquePreMovimentoItemRepository.Object.GetAll()
                                                              .AsNoTracking()
                                                              .Where(w => w.PreMovimentoId == id)
                                                              .Select(s => s.Id)
                                                              .ToList();
                foreach (var item in itens)
                {
                    Excluir(item);
                }
            }

        }



        public Task<PagedResultDto<EstoquePreMovimentoItemDto>> Listar(long Id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<EstoquePreMovimentoItemDto>> ListarPorMovimentacaoLoteValidade(ListarEstoquePreMovimentoInput input)
        {
            try
            {
                var contarPreMovimentos = 0;
                long _filtro;
                List<EstoquePreMovimentoItemDto> PreMovimentoItens = null;

                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                {
                    if (long.TryParse(input.Filtro, out _filtro))
                    {
                        var query = estoquePreMovimentoItemRepository.Object
                            .GetAll().Where(w => w.PreMovimentoId == _filtro
                                              && (w.Produto.IsValidade || w.Produto.IsLote))
                            .Include(i => i.Produto);

                        contarPreMovimentos = await query.CountAsync();

                        var preMovimentoItens = await query.ToListAsync();

                        PreMovimentoItens = EstoquePreMovimentoItemDto.MapPreMovimentoItem(preMovimentoItens);//.MapTo<List<EstoquePreMovimentoItemDto>>();
                        foreach (var item in PreMovimentoItens)
                        {
                            var qtdAtendida = estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                                                      .Where(w => w.EstoquePreMovimentoItemId == item.Id)
                                                      .ToList()
                                                      .Sum(s => s.Quantidade);


                            item.QuantidadeAtendida = qtdAtendida;
                        }
                    }

                    return new PagedResultDto<EstoquePreMovimentoItemDto>(contarPreMovimentos, PreMovimentoItens);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<EstoquePreMovimentoItemDto>> ListarPorMovimentacaoLoteValidadeProduto(ListarEstoquePreMovimentoInput input)
        {
            try
            {
                var contarPreMovimentos = 0;
                List<EstoquePreMovimentoItemDto> PreMovimentoItens = null;

                long preMovimentoId = long.Parse(input.Filtro);
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                {
                    var query = estoquePreMovimentoItemRepository.Object.GetAll().Where(w => w.Id == preMovimentoId);  // preMovimentoId)
                                                                                                                       //&& w.Produto.Id == long.Parse(produtoId )

                    contarPreMovimentos = await query.CountAsync();
                    var preMovimentoItens = await query.ToListAsync();
                    PreMovimentoItens = EstoquePreMovimentoItemDto.MapPreMovimentoItem(preMovimentoItens);//.MapTo<List<EstoquePreMovimentoItemDto>>();


                    return new PagedResultDto<EstoquePreMovimentoItemDto>(contarPreMovimentos, PreMovimentoItens);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }



        public async Task<EstoqueTransferenciaProdutoItemDto> ObterTransferenciaItem(long id)
        {
            try
            {
                using (var estoqueTransferenciaProdutoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProdutoItem, long>>())
                {
                    var query = estoqueTransferenciaProdutoItemRepository.Object.GetAll().Where(w => w.Id == id).FirstOrDefault();

                    var transferenciaItemDto = new EstoqueTransferenciaProdutoItemDto
                    {
                        Id = query.Id,
                        EstoqueTransferenciaProdutoID = query.EstoqueTransferenciaProdutoID,
                        PreMovimentoEntradaItemId = query.PreMovimentoEntradaItemId,
                        PreMovimentoSaidaItemId = query.PreMovimentoSaidaItemId
                    };

                    return transferenciaItemDto;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task ExcluirItemTransferencia(long id)
        {
            try
            {
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                using (var estoqueTransferenciaProdutoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueTransferenciaProdutoItem, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var transferenciaItem = estoqueTransferenciaProdutoItemRepository.Object.Get(id);
                    if (transferenciaItem != null)
                    {

                        EstoquePreMovimentoItem saidaItem = new EstoquePreMovimentoItem();
                        EstoquePreMovimentoItem entradaItem = new EstoquePreMovimentoItem();

                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            var saidaItemId = transferenciaItem.PreMovimentoSaidaItemId;
                            var entradaItemId = transferenciaItem.PreMovimentoEntradaItemId;

                            saidaItem = estoquePreMovimentoItemRepository.Object.Get(saidaItemId);
                            entradaItem = estoquePreMovimentoItemRepository.Object.Get(entradaItemId);

                            await estoquePreMovimentoItemRepository.Object.DeleteAsync(saidaItemId);
                            await estoquePreMovimentoItemRepository.Object.DeleteAsync(entradaItemId);

                            await estoqueTransferenciaProdutoItemRepository.Object.DeleteAsync(id);

                            await produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItem(saidaItem);
                            await produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItem(entradaItem);

                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<List<string>> ObterNumerosSerieProduto(long estoqueId, long produtoId)
        {
            using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
            using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
            {
                var query = from mov in estoqueMovimentoRepository.Object.GetAll()
                            join item in estoqueMovimentoItemRepository.Object.GetAll()
                            on mov.Id equals item.MovimentoId
                            into movjoin
                            from joinedmov in movjoin.DefaultIfEmpty()
                            where mov.EstoqueId == estoqueId
                               && joinedmov.ProdutoId == produtoId
                               && !String.IsNullOrEmpty(joinedmov.NumeroSerie)

                            //   group joinedmov by joinedmov.NumeroSerie  into g

                            select new
                            {
                                joinedmov.NumeroSerie,

                                Quantidade = ((((mov.IsEntrada ? 1 : 0) * 2) - 1) * joinedmov.Quantidade),

                                //(((joinedmov.IsEntrada ? 1 : 0) * 2) - 1) * joinedmov.Quantidade)
                            };


                var numerosSeries = query.GroupBy(g => g.NumeroSerie).Select(s => new { Quantidade = s.Sum(su => su.Quantidade), numerosSeries = s.Select(s2 => s2.NumeroSerie) });


                return query.Where(w => w.Quantidade > 0).Select(s => s.NumeroSerie).ToList();
            }

        }

        public async Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarOuEditarDevolucao(EstoquePreMovimentoItemDto input)
        {
            var _retornoPadrao = new DefaultReturn<EstoquePreMovimentoItemDto>();

            try
            {
                var estoquePreMovimentoItem = new EstoquePreMovimentoItem();


                using (var preMovimentoValidacaoDomainService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoValidacaoDomainService>())
                using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                using (var unidadeAppService = IocManager.Instance.ResolveAsDisposable<IUnidadeAppService>())
                using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var produto = await produtoRepository.Object.GetAsync(input.ProdutoId);
                    input.Produto = ProdutoDto.Mapear(produto);//.MapTo<ProdutoDto>();

                    var preMovimento = await estoquePreMovimentoRepository.Object.GetAsync(input.PreMovimentoId);
                    input.EstoquePreMovimento = EstoquePreMovimentoDto.MapPreMovimento(preMovimento);
                    var validarDataVencimeneto = preMovimento.EstTipoMovimentoId != 4;

                    input.Quantidade = unidadeAppService.Object.ObterQuantidadeReferencia((long)input.ProdutoUnidadeId, input.Quantidade);

                    _retornoPadrao.Errors = preMovimentoValidacaoDomainService.Object.ValidarItemDevolucao(input, (produto != null && (produto.IsValidade || produto.IsLote)), validarDataVencimeneto);

                    if (_retornoPadrao.Errors.Any())
                    {
                        return _retornoPadrao;
                    }
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        estoquePreMovimentoItem = EstoquePreMovimentoItemDto.MapPreMovimentoItem(input);
                        if (input.Id.Equals(0))
                        {
                            input.Id = await estoquePreMovimentoItemRepository.Object.InsertAndGetIdAsync(estoquePreMovimentoItem);
                        }
                        else
                        {
                            await estoquePreMovimentoItemRepository.Object.UpdateAsync(estoquePreMovimentoItem);
                        }

                        if (produto != null && (produto.IsValidade || produto.IsLote))
                        {
                            var estoquePreMovimentoLoteValidadeDto = new EstoquePreMovimentoLoteValidadeDto
                            {
                                Id = input.EstoquePreMovimentoLoteValidadeId
                                ,
                                LaboratorioId = input.LaboratorioId ?? 0
                                ,
                                Lote = input.Lote
                                ,
                                Validade = input.Validade ?? DateTime.MinValue
                                ,
                                ProdutoId = input.ProdutoId
                                ,
                                EstoquePreMovimentoItemId = input.Id
                                ,
                                Quantidade = input.Quantidade
                                ,
                                LoteValidadeId = input.LoteValidadeId ?? 0
                            };

                            var retorno = estoqueLoteValidadeAppService.Object.CriarOuEditar(estoquePreMovimentoLoteValidadeDto);

                            _retornoPadrao.Errors.AddRange(retorno.Errors);
                        }

                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();

                        if (produto != null && !(produto.IsValidade || produto.IsLote))
                        {
                            await produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItem(estoquePreMovimentoItem);
                        }

                        var query = await Obter(input.Id);
                        var newObj = EstoquePreMovimentoItemDto.MapPreMovimentoItem(query);

                        var estoquePreMovimentoItemDto = EstoquePreMovimentoItemDto.MapPreMovimentoItem(newObj);

                        _retornoPadrao.ReturnObject = estoquePreMovimentoItemDto;
                    }

                    return _retornoPadrao;
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

        public async Task<DefaultReturn<EstoquePreMovimentoItemDto>> CriarDevolucaoPorCodigoBarra(string codigoBarra, long? estoqueId, long? preMovimentoId, decimal? quantidade)
        {
            using (var codigoBarraAppService = IocManager.Instance.ResolveAsDisposable<ICodigoBarraAppService>())
            {
                var dadosEtiqueta = await codigoBarraAppService.Object.ObterValorEtiqueta(codigoBarra);

                if (dadosEtiqueta != null)
                {
                    if (dadosEtiqueta.TipoEtiquetaCodigoBarra == Enumeradores.EnumTipoEtiquetaCodigoBarra.LoteValidade)
                    {
                        var estoquePreMovimentoItemDto = new EstoquePreMovimentoItemDto
                        {
                            LoteValidadeId = dadosEtiqueta.LoteValidadeId,
                            LaboratorioId = dadosEtiqueta.LoteValidade.ProdutoLaboratorioId,
                            Lote = dadosEtiqueta.LoteValidade.Lote,
                            Validade = dadosEtiqueta.LoteValidade.Validade,
                            ProdutoId = dadosEtiqueta.LoteValidade.Produto.Id,
                            Quantidade = quantidade ?? 1,
                            EstoqueId = estoqueId ?? 0,
                            ProdutoUnidadeId = dadosEtiqueta.UnidadeProdutoId,
                            PreMovimentoId = preMovimentoId ?? 0
                        };

                        return await CriarOuEditarDevolucao(estoquePreMovimentoItemDto);

                    }
                }
                else
                {
                    var defaultReturn = new DefaultReturn<EstoquePreMovimentoItemDto>();
                    defaultReturn.Errors = new List<ErroDto> { new ErroDto { CodigoErro = "ECB0001" } };
                    return defaultReturn;
                }
            }

            return null;
        }

        public async Task<List<EstoquePreMovimentoItemDto>> ObterItensPorPreMovimento(long preMovimentoId)
        {
            using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            {
                var itensDto = new List<EstoquePreMovimentoItemDto>();
                var itemDto = new EstoquePreMovimentoItemDto();
                long idGrid = 0;
                var itens = await estoquePreMovimentoItemRepository.Object.GetAllListAsync(x => x.PreMovimentoId == preMovimentoId && !x.IsDeleted);

                foreach (var item in itens)
                {
                    itemDto = EstoquePreMovimentoItemDto.MapPreMovimentoItem(item);
                    itemDto.IdGrid = ++idGrid;
                    itensDto.Add(itemDto);
                }

                return itensDto;
            }
        }

        public List<EstoquePreMovimentoItemSolicitacaoDto> ObterItensSolicitacaoPorPreMovimento(long preMovimentoId)
        {
            using (var estoqueSolicitacaoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueSolicitacaoItem, long>>())
            using (var estoquePreMovimentoItemRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
            using (var estoquePreMovimentoRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
            using (var estoquePreMovimentoLoteValidadeRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
            using (var produtoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Produto, long>>())
            using (var unidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Unidade, long>>())
            {
                var itensSolicitacao = estoqueSolicitacaoItemRepositorio.Object.GetAll().AsNoTracking().Where(w => w.SolicitacaoId == preMovimentoId).ToList();
                var itensPreMovimento = estoquePreMovimentoItemRepositorio.Object.GetAllList(x => x.PreMovimentoId == preMovimentoId);
                var atendidos = estoquePreMovimentoRepositorio.Object
                    .GetAll().AsNoTracking()
                    .Include(x => x.Itens).Include(x => x.Itens.Select(z => z.EstoquePreMovimentosLoteValidades))
                    .Where(x => x.EstoquePreMovimentoParentId == preMovimentoId).ToList();
                var itemIds = itensSolicitacao.Select(x => x.SolicitacaoId);

                List<EstoquePreMovimentoItemSolicitacaoDto> itensDto = new List<EstoquePreMovimentoItemSolicitacaoDto>();
                EstoquePreMovimentoItemSolicitacaoDto itemDto;

                long idGrid = 0;
                foreach (var item in itensSolicitacao)
                {
                    itemDto = new EstoquePreMovimentoItemSolicitacaoDto
                    {
                        Id = item.Id,
                        ProdutoId = item.ProdutoId,
                        EstadoSolicitacaoItemId = item.EstadoSolicitacaoItemId,
                        PreMovimentoId = preMovimentoId,
                        EstoqueKitItemId = item.EstoqueKitItemId
                    };

                    if (itensPreMovimento.Any(x => x.ProdutoId == item.ProdutoId))
                    {
                        var itemPreMovimento = itensPreMovimento.FirstOrDefault(x => x.ProdutoId == item.ProdutoId);
                        itemDto.PreMovimentoItemId = itemPreMovimento?.Id;
                    }

                    if (item.ProdutoId != 0)
                    {
                        var produto = produtoRepository.Object.Get(item.ProdutoId);
                        if (produto != null)
                        {
                            itemDto.Produto = produto.Descricao;
                            itemDto.IsLote = produto.IsLote;
                        }
                    }

                    itemDto.ProdutoUnidadeId = item.ProdutoUnidadeId;

                    if (item.ProdutoUnidadeId != 0)
                    {
                        var unidade = unidadeRepository.Object.Get((long)item.ProdutoUnidadeId);
                        if (unidade != null)
                        {
                            itemDto.ProdutoUnidade = unidade.Descricao;
                        }
                    }

                    itemDto.QuantidadeSolicitada = item.Quantidade;
                    itemDto.QuantidadeAtendida = item.QuantidadeAtendida;

                    itemDto.Quantidade = item.Quantidade - item.QuantidadeAtendida;
                    var lotes = new List<EstoquePreMovimentoLoteValidadeDto>();
                    foreach (var atendido in atendidos)
                    {
                        foreach (var itemAtendido in atendido.Itens.Where(x => x.ProdutoId == itemDto.ProdutoId))
                        {
                            foreach (var estoquePreMovimentosLoteValidade in itemAtendido.EstoquePreMovimentosLoteValidades)
                            {
                                var itemValidade = EstoquePreMovimentoLoteValidadeDto.Mapear(estoquePreMovimentosLoteValidade);
                                itemValidade.EstoquePreMovimentoItem = null;
                                lotes.Add(itemValidade);
                            }
                        }
                    }

                    itemDto.LotesValidadesJson = JsonConvert.SerializeObject(lotes);

                    itemDto.IdGrid = ++idGrid;
                    itensDto.Add(itemDto);
                }

                return itensDto;
            }
        }

        public PagedResultDto<LoteValidadeGridDto> ListarLoteValidadeJson(ListarItensJsonInput input)
        {
            var lotesValidades = new List<LoteValidadeGridDto>();

            try
            {                
                if (!string.IsNullOrEmpty(input.Data))
                {
                    using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
                    using (var produtoLaboratorioRepositorio = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueLaboratorio, long>>())
                    {

                        lotesValidades = JsonConvert.DeserializeObject<List<LoteValidadeGridDto>>(input.Data,
                            new JsonSerializerSettings
                            {
                                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                                FloatParseHandling = FloatParseHandling.Decimal
                            }
                        );

                        foreach (var item in lotesValidades.Where(x => x.LoteValidadeId != 0))
                        {
                            var loteValidade = loteValidadeRepository.Object.Get(item.LoteValidadeId);

                            if (loteValidade != null)
                            {
                                item.Lote = loteValidade.Lote;
                                item.Validade = loteValidade.Validade;

                                if (loteValidade.EstLaboratorioId.HasValue)
                                {
                                    var laboratorio = produtoLaboratorioRepositorio.Object.Get(loteValidade.EstLaboratorioId.Value);
                                    item.LaboratorioId = laboratorio?.Id;
                                    item.Laboratorio = laboratorio?.Descricao;
                                }
                            }
                        }
                    }
                }                
            }
            catch (Exception)
            { }

            return new PagedResultDto<LoteValidadeGridDto>(lotesValidades.Count, lotesValidades);
        }


        public PagedResultDto<NumeroSerieGridDto> ListarNumeroSerieJson(ListarItensJsonInput input)
        {
            try
            {
                if (input == null)
                {
                    return null;
                }

                var contarNumeroSerie = 0;
                List<NumeroSerieGridDto> numerosSerie = new List<NumeroSerieGridDto>();
                if (!string.IsNullOrEmpty(input.Data))
                {
                    numerosSerie = JsonConvert.DeserializeObject<List<NumeroSerieGridDto>>(input.Data);
                }

                contarNumeroSerie = numerosSerie.Count;

                return new PagedResultDto<NumeroSerieGridDto>(contarNumeroSerie, numerosSerie);
            }
            catch (Exception)
            {

            }

            return null;
        }

        public PagedResultDto<LoteValidadeGridDto> ListarLoteValidadeJsonLista(ListarItensJsonInput input)//List<LoteValidadeGridDto> lista)
        {
            List<LoteValidadeGridDto> lista = new List<LoteValidadeGridDto>();

            return new PagedResultDto<LoteValidadeGridDto>(lista.Count, lista);
        }
    }
}
