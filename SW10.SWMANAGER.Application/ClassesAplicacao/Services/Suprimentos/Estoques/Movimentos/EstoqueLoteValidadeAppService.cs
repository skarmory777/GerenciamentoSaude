using Abp.Application.Services.Dto;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using Abp.Linq.Extensions;
using SW10.SWMANAGER.Helpers;
using System.Text;
using Abp.Extensions;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.DomainServices;
using Abp.Collections.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos
{
    public class EstoqueLoteValidadeAppService : SWMANAGERAppServiceBase, IEstoqueLoteValidadeAppService
    {
        [UnitOfWork]
        public DefaultReturn<EstoquePreMovimentoLoteValidadeDto> CriarOuEditar(EstoquePreMovimentoLoteValidadeDto input)
        {
            var _retornoPadrao = new DefaultReturn<EstoquePreMovimentoLoteValidadeDto>
            {
                Errors = new List<ErroDto>()
            };

            try
            {
                var estoqueLoteValidade = new EstoquePreMovimentoLoteValidade();
                using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    estoqueLoteValidade = estoquePreMovimentoLoteValidadeRepository.Object.GetAll().Where(w => w.Id == input.Id).FirstOrDefault();
                    if (estoqueLoteValidade == null)
                    {
                        estoqueLoteValidade = new EstoquePreMovimentoLoteValidade
                        {
                            EstoquePreMovimentoItemId = input.EstoquePreMovimentoItemId
                        };
                    }

                    var loteValidade = new LoteValidadeDto();
                    if (input.LoteValidadeId == 0)
                    {
                        loteValidade = ObterPorLaboratorioLoteValidade(input.ProdutoId,input.LaboratorioId, input.Lote, input.Validade);

                        if (loteValidade != null)
                        {
                            estoqueLoteValidade.LoteValidadeId = loteValidade.Id;
                        }
                        else
                        {
                            estoqueLoteValidade.LoteValidade = new LoteValidade { EstLaboratorioId = input.LaboratorioId, Lote = input.Lote, Validade = input.Validade, ProdutoId = input.ProdutoId };
                        }
                    }
                    else
                    {
                        estoqueLoteValidade.LoteValidadeId = input.LoteValidadeId;
                    }

                    estoqueLoteValidade.Quantidade = input.Quantidade;

                    if (input.Id.Equals(0))
                    {
                        estoquePreMovimentoLoteValidadeRepository.Object.Insert(estoqueLoteValidade);
                    }
                    else
                    {
                        estoquePreMovimentoLoteValidadeRepository.Object.Update(estoqueLoteValidade);
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    //unitOfWork.Dispose();
                }

                var estoqueLoteValidadeDto = MapLoteValidade(estoqueLoteValidade);
                _retornoPadrao.ReturnObject = estoqueLoteValidadeDto;//.MapTo<EstoquePreMovimentoLoteValidadeDto>();
                                                                     //  }

                using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                {
                    produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItemLoteValidade(estoqueLoteValidadeDto);
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

        public async Task<EstoquePreMovimentoLoteValidadeDto> Obter(long id)
        {
            try
            {
                using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                {
                    var query = estoquePreMovimentoLoteValidadeRepository.Object.GetAll()
                        .Include(i => i.LoteValidade)
                        .Include(i => i.LoteValidade.EstoqueLaboratorio)
                        .Include(i => i.EstoquePreMovimentoItem)
                        .Where(w => w.Id == id).FirstOrDefault();

                    var entrada = MapLoteValidade(query);//.MapTo<EstoquePreMovimentoLoteValidadeDto>();

                    return entrada;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<PagedResultDto<EstoquePreMovimentoLoteValidadeDto>> ListarPorPreMovimentoItem(ListarEstoquePreMovimentoInput input)
        {
            try
            {
                long preMovimentoItemId;

                if (long.TryParse(input.Filtro, out preMovimentoItemId))
                {
                    var contarLoteValidades = 0;
                    List<EstoquePreMovimentoLoteValidadeDto> lotesValidadesDto = null;
                    using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                    {
                        var query = estoquePreMovimentoLoteValidadeRepository.Object
                          .GetAll()
                          .Include(i => i.LoteValidade)
                          .Include(i => i.EstoquePreMovimentoItem)
                          .Where(w => w.EstoquePreMovimentoItemId == preMovimentoItemId);


                        contarLoteValidades = await query.CountAsync();
                        var lotesValidades = await query.ToListAsync();
                        lotesValidadesDto = MapLoteValidade(lotesValidades);//.MapTo<List<EstoquePreMovimentoLoteValidadeDto>>();

                        lotesValidadesDto.ForEach(f => f.EntradaConfirmada = input.EntradaConfirmada);


                        return new PagedResultDto<EstoquePreMovimentoLoteValidadeDto>(contarLoteValidades, lotesValidadesDto);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public LoteValidadeDto ObterPorLaboratorioLoteValidade(long? produtoId, long? laboratorioId, string lote, DateTime validade)
        {
            try
            {
                using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
                {
                    var query = loteValidadeRepository.Object.GetAll().FirstOrDefault(w =>
                                    ((laboratorioId == 0 || laboratorioId == null) || w.EstLaboratorioId == laboratorioId)
                                                                         && w.ProdutoId == produtoId
                                                                         && w.Lote == lote
                                                                         && w.Validade == validade);

                    var loteValidade = LoteValidadeDto.Mapear(query);//.MapTo<LoteValidadeDto>();

                    return loteValidade;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public static EstoquePreMovimentoLoteValidadeDto MapLoteValidade(EstoquePreMovimentoLoteValidade preMovimentoLoteValidade)
        {
            EstoquePreMovimentoLoteValidadeDto preMovimentoLoteValidadeDto = new EstoquePreMovimentoLoteValidadeDto
            {
                CreationTime = preMovimentoLoteValidade.CreationTime,
                CreatorUserId = preMovimentoLoteValidade.CreatorUserId,
                DeleterUserId = preMovimentoLoteValidade.DeleterUserId,
                DeletionTime = preMovimentoLoteValidade.DeletionTime,
                EstoquePreMovimentoItemId = preMovimentoLoteValidade.EstoquePreMovimentoItemId,
                Id = preMovimentoLoteValidade.Id,
                IsDeleted = preMovimentoLoteValidade.IsDeleted,
                IsSistema = preMovimentoLoteValidade.IsSistema,
                LastModificationTime = preMovimentoLoteValidade.LastModificationTime,
                LastModifierUserId = preMovimentoLoteValidade.LastModifierUserId,
                Quantidade = preMovimentoLoteValidade.Quantidade,
                LoteValidadeId = preMovimentoLoteValidade.LoteValidadeId
            };

            if (preMovimentoLoteValidade.LoteValidade != null)
            {
                preMovimentoLoteValidadeDto.LoteValidade = new LoteValidadeDto
                {
                    Id = preMovimentoLoteValidade.LoteValidade.Id,
                    Lote = preMovimentoLoteValidade.LoteValidade.Lote,
                    Validade = preMovimentoLoteValidade.LoteValidade.Validade,
                    ProdutoLaboratorioId = preMovimentoLoteValidade.LoteValidade.EstLaboratorioId,
                    Produto = CarregarProdutoDto(preMovimentoLoteValidade.LoteValidade.Produto),
                    ProdutoLaboratorio = new ProdutoLaboratorioDto
                    {
                        Id = preMovimentoLoteValidade.LoteValidade.EstoqueLaboratorio != null ? preMovimentoLoteValidade.LoteValidade.EstoqueLaboratorio.Id : 0,
                        Codigo = preMovimentoLoteValidade.LoteValidade.EstoqueLaboratorio?.Codigo,
                        Descricao = preMovimentoLoteValidade.LoteValidade.EstoqueLaboratorio?.Descricao
                    }

                };
            }

            return preMovimentoLoteValidadeDto;
        }

        public static EstoquePreMovimentoLoteValidade MapLoteValidade(EstoquePreMovimentoLoteValidadeDto preMovimentoLoteValidadeDto)
        {
            EstoquePreMovimentoLoteValidade preMovimentoLoteValidade = new EstoquePreMovimentoLoteValidade
            {
                CreationTime = preMovimentoLoteValidadeDto.CreationTime,
                CreatorUserId = preMovimentoLoteValidadeDto.CreatorUserId,
                DeleterUserId = preMovimentoLoteValidadeDto.DeleterUserId,
                DeletionTime = preMovimentoLoteValidadeDto.DeletionTime,
                EstoquePreMovimentoItemId = preMovimentoLoteValidadeDto.EstoquePreMovimentoItemId,
                Id = preMovimentoLoteValidadeDto.Id,
                IsDeleted = preMovimentoLoteValidadeDto.IsDeleted,
                IsSistema = preMovimentoLoteValidadeDto.IsSistema,
                LastModificationTime = preMovimentoLoteValidadeDto.LastModificationTime,
                LastModifierUserId = preMovimentoLoteValidadeDto.LastModifierUserId,
                Quantidade = preMovimentoLoteValidadeDto.Quantidade,
                LoteValidadeId = preMovimentoLoteValidadeDto.LoteValidadeId
            };

            return preMovimentoLoteValidade;
        }

        public static List<EstoquePreMovimentoLoteValidadeDto> MapLoteValidade(List<EstoquePreMovimentoLoteValidade> preMovimentoLotesValidades)
        {
            List<EstoquePreMovimentoLoteValidadeDto> preMovimentoLotesValidadesDto = new List<EstoquePreMovimentoLoteValidadeDto>();

            foreach (var item in preMovimentoLotesValidades)
            {
                preMovimentoLotesValidadesDto.Add(MapLoteValidade(item));
            }

            return preMovimentoLotesValidadesDto;
        }

        public static List<EstoquePreMovimentoLoteValidade> MapLoteValidade(List<EstoquePreMovimentoLoteValidadeDto> preMovimentoLotesValidadesDto)
        {
            List<EstoquePreMovimentoLoteValidade> preMovimentoLotesValidades = new List<EstoquePreMovimentoLoteValidade>();

            foreach (var item in preMovimentoLotesValidadesDto)
            {
                preMovimentoLotesValidades.Add(MapLoteValidade(item));
            }

            return preMovimentoLotesValidades;
        }

        static ProdutoDto CarregarProdutoDto(Produto produto)
        {
            ProdutoDto produtoDto = new ProdutoDto();

            if (produto != null)
            {
                produtoDto.Id = produto.Id;
                produtoDto.Descricao = produto.Descricao;
                produtoDto.IsValidade = produto.IsValidade;
            }


            return produtoDto;

        }

        public async Task<List<GenericoIdNome>> ObterPorProdutoEstoque(long produtoId, long estoqueId, long preMovimentoId)
        {
            try
            {
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
                using (var estoqueMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoLoteValidade, long>>())
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var produtoLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueLaboratorio, long>>())
                {

                    var query = from mov in estoqueMovimentoRepository.Object.GetAll().AsNoTracking()
                                join item in estoqueMovimentoItemRepository.Object.GetAll().AsNoTracking()
                                on mov.Id equals item.MovimentoId
                                join iltv in estoqueMovimentoLoteValidadeRepository.Object.GetAll().AsNoTracking()
                                on item.Id equals iltv.EstoqueMovimentoItemId
                                into movjoin
                                from joinedmov in movjoin.DefaultIfEmpty()
                                join produtoLaboratorio in produtoLaboratorioRepository.Object.GetAll().AsNoTracking()
                               on joinedmov.LoteValidade.EstLaboratorioId equals produtoLaboratorio.Id
                                 into laboratoriojoin
                                from joinLaboratorio in laboratoriojoin.DefaultIfEmpty()
                                where mov.EstoqueId == estoqueId
                                   && item.ProdutoId == produtoId
                                select new { LoteValidade = joinedmov.LoteValidade, Quantidade = ((((mov.IsEntrada ? 1 : 0) * 2) - 1) * joinedmov.Quantidade), ProdutoLaboratorio = joinLaboratorio };



                    var queryPreMovimento = from mov in estoquePreMovimentoRepository.Object.GetAll().AsNoTracking()
                                            join item in estoquePreMovimentoItemRepository.Object.GetAll().AsNoTracking()
                                            on mov.Id equals item.PreMovimentoId
                                            join iltv in estoquePreMovimentoLoteValidadeRepository.Object.GetAll().AsNoTracking()
                                            on item.Id equals iltv.EstoquePreMovimentoItemId
                                            into movjoin
                                            from joinedmov in movjoin.DefaultIfEmpty()

                                            join produtoLaboratorio in produtoLaboratorioRepository.Object.GetAll().AsNoTracking()
                            on joinedmov.LoteValidade.EstLaboratorioId equals produtoLaboratorio.Id

                                            where mov.EstoqueId == estoqueId
                                               && item.ProdutoId == produtoId
                                               && mov.Id == preMovimentoId
                                            select new { LoteValidade = joinedmov.LoteValidade, Quantidade = ((((mov.IsEntrada ? 1 : 0) * 2) - 1) * joinedmov.Quantidade), ProdutoLaboratorio = produtoLaboratorio };

                    var queryList = query.AsNoTracking().ToList();
                    var queryPreMovimentoList = queryPreMovimento.AsNoTracking().ToList();

                    queryList.AddRange(queryPreMovimentoList);

                    var lotesValidade = queryList.GroupBy(g => g.LoteValidade).Select(s => new { LoteValidade = s.Key, Qtd = s.Sum(soma => soma.Quantidade), ProdutoLaboratorio = s.Key.EstoqueLaboratorio }).ToList();

                    var listaGenericoIdNome = new List<GenericoIdNome>();

                    foreach (var item in lotesValidade.OrderBy(o => o.LoteValidade.Validade))
                    {
                        var nome = string.Concat("Lt:", item.LoteValidade.Lote
                                                , "     Validade: ", String.Format("{0:dd/MM/yyyy}", item.LoteValidade.Validade)
                                                , item.ProdutoLaboratorio != null ? string.Concat("     Laboratório: ", item.ProdutoLaboratorio.Descricao) : string.Empty
                                                , "     Qtd: ", item.Qtd);

                        listaGenericoIdNome.Add(new GenericoIdNome { Id = item.LoteValidade.Id, Nome = nome });
                    }

                    return listaGenericoIdNome;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<IResultDropdownList<long>> ListarDropdownSaldo(DropdownInput dropdownInput)
        {
            if (!dropdownInput.id.IsNullOrEmpty())
            {
                var loteValidadeId = TryParseNullable(dropdownInput.id);

                var result = await this.ObterPorProdutoEstoquePorLoteValidadeId(loteValidadeId);
                var resultReturn = new ResultDropdownList<long>
                {
                    TotalCount = result.Count,
                    Items = result.Select(x => new DropdownItems<long>() { id = x.Id, text = x.Nome }).ToList()
                };
                return resultReturn;
            }
            else
            {
                var produtoIdString = dropdownInput.filtros != null && dropdownInput.filtros.Length != 0 && dropdownInput.filtros[0] != null ? dropdownInput.filtros[0] : null;
                var estoqueIdString = dropdownInput.filtros != null && dropdownInput.filtros.Length != 0 && dropdownInput.filtros[1] != null ? dropdownInput.filtros[1] : null;
                var preMovimentoIdString = dropdownInput.filtros != null && dropdownInput.filtros.Length != 0 && dropdownInput.filtros[2] != null ? dropdownInput.filtros[2] : null;
                var loteValidadeIdString = dropdownInput.filtros != null && dropdownInput.filtros.Length != 0 && dropdownInput.filtros[3] != null ? dropdownInput.filtros[3] : null;

                long? produtoId = TryParseNullable(produtoIdString);
                long? estoqueId = TryParseNullable(estoqueIdString);
                long? preMovimentoId = TryParseNullable(preMovimentoIdString);
                long? loteValidadeId = TryParseNullable(loteValidadeIdString);

                var result = await this.ObterPorProdutoEstoquePorSaldo(produtoId ?? 0, estoqueId ?? 0, preMovimentoId ?? 0, loteValidadeId);
                var resultReturn = new ResultDropdownList<long>
                {
                    TotalCount = result.Count,
                    Items = result.Select(x => new DropdownItems<long>() { id = x.Id, text = x.Nome }).ToList()
                };
                return resultReturn;
            }
        }

        public long? TryParseNullable(string val)
        {
            long outValue;
            return long.TryParse(val, out outValue) ? (long?)outValue : null;
        }

        public async Task<List<GenericoIdNome>> ObterPorProdutoEstoquePorLoteValidadeId(long? loteValidadeId = null)
        {
            try
            {
                using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                {
                    var result = await produtoSaldoRepository.Object.GetAll()
                        .Include(x => x.Produto)
                        .Include(x => x.LoteValidade)
                        .Include(x => x.LoteValidade.EstoqueLaboratorio)
                        .AsNoTracking()
                        .Where(x => loteValidadeId.HasValue && x.LoteValidadeId == loteValidadeId).ToListAsync().ConfigureAwait(false);

                    return result.Select(
                        x => new GenericoIdNome
                        {
                            Id = x.LoteValidadeId ?? 0,
                            Nome = string.Concat(
                                "Lt:", x.LoteValidade != null ? x.LoteValidade.Lote : ""
                                , "     Validade: ", x.LoteValidade != null ? string.Format("{0:dd/MM/yyyy}", x.LoteValidade.Validade) : ""
                                , x.LoteValidade != null && x.LoteValidade.EstoqueLaboratorio != null ? string.Concat("     Laboratório: ", x.LoteValidade.EstoqueLaboratorio.Descricao) : string.Empty
                                , "     Qtd: ", x.QuantidadeAtual)

                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<List<GenericoIdNome>> ObterPorProdutoEstoquePorSaldo(long produtoId, long estoqueId, long preMovimentoId, long? loteValidadeId = null)
        {
            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                {
                    EstoquePreMovimento estoquePreMovimento = null;
                    if (preMovimentoId != 0)
                    {
                        estoquePreMovimento = await estoquePreMovimentoRepository.Object
                            .GetAll()
                            .AsNoTracking()
                            .Include(x => x.Itens)
                            .Include(x => x.Itens.Select(z => z.Produto))
                            .Include(x => x.Itens.Select(z => z.EstoquePreMovimentosLoteValidades))
                            .FirstOrDefaultAsync(x => x.Id == preMovimentoId).ConfigureAwait(false);
                    }

                    var result = produtoSaldoRepository.Object.GetAll()
                        .Include(x => x.Produto)
                        .Include(x => x.LoteValidade)
                        .Include(x => x.LoteValidade.EstoqueLaboratorio)
                        .AsNoTracking()
                        .Where(x => x.ProdutoId == produtoId && x.EstoqueId == estoqueId);

                    if (estoquePreMovimento != null && estoquePreMovimento.EstTipoOperacaoId == (long)EnumTipoOperacao.Devolucao)
                    {
                        result = result.Where(x => x.QuantidadeAtual >= 0);

                        if (loteValidadeId.HasValue)
                        {
                            result = result.Where(x => x.LoteValidadeId == loteValidadeId);
                        }
                    }
                    else
                    {
                        if (estoquePreMovimento != null && estoquePreMovimento.EstTipoMovimentoId == (long)EnumTipoMovimento.Perda_Saida)
                        {
                            result = result.Where(x => x.QuantidadeAtual > 0);

                            if (loteValidadeId.HasValue)
                            {
                                result = result.Where(x => x.LoteValidadeId == loteValidadeId);
                            }
                        }
                        else
                        {
                            result = result.Where(x => x.QuantidadeAtual > 0 && (x.LoteValidade == null || x.LoteValidade.Validade >= DateTime.Today || (loteValidadeId.HasValue && x.LoteValidadeId == loteValidadeId)));
                        }
                    }

                    var produtoSaldoList = await result.ToListAsync().ConfigureAwait(false);

                    var resultList = new List<GenericoIdNome>();

                    if (estoquePreMovimento != null && !estoquePreMovimento.Itens.IsNullOrEmpty())
                    {
                        var itemsNoMovimento = new List<ItemsNoMovimento>();
                        foreach (var item in estoquePreMovimento.Itens.Where(x => x.PreMovimentoItemEstadoId == (long)EnumPreMovimentoEstado.Pendente || x.PreMovimentoItemEstadoId == null))
                        {
                            if (!item.Produto.IsLote)
                            {
                                itemsNoMovimento.Add(new ItemsNoMovimento(item.ProdutoId, null, item.Quantidade));
                            }
                            else
                            {
                                itemsNoMovimento.AddRange(item.EstoquePreMovimentosLoteValidades
                                    .Select(x => new ItemsNoMovimento(item.ProdutoId, x.LoteValidadeId, x.Quantidade)).ToList());
                            }
                        }
                        foreach (var resultItem in produtoSaldoList)
                        {
                            var nome = string.Concat(
                                    "Lt:", resultItem.LoteValidade != null ? resultItem.LoteValidade.Lote : ""
                                    , "     Validade: ", resultItem.LoteValidade != null ? string.Format("{0:dd/MM/yyyy}", resultItem.LoteValidade.Validade) : ""
                                    , resultItem.LoteValidade != null && resultItem.LoteValidade.EstoqueLaboratorio != null ? string.Concat("     Laboratório: ", resultItem.LoteValidade.EstoqueLaboratorio.Descricao) : string.Empty
                                    , "     Qtd: ", resultItem.QuantidadeAtual);

                            var items = itemsNoMovimento.Where(x => x.ProdutoId == resultItem.ProdutoId && x.LoteValidadeId == resultItem.LoteValidadeId).ToList();
                            if (items != null && !items.IsNullOrEmpty())
                            {
                                resultItem.QuantidadeAtual -= items.Sum(x => x.Quantidade);

                                if (resultItem.QuantidadeAtual <= 0 && resultItem.LoteValidadeId != loteValidadeId)
                                {
                                    continue;
                                }

                                nome = string.Concat(
                                    "Lt:", resultItem.LoteValidade != null ? resultItem.LoteValidade.Lote : ""
                                    , "     Validade: ", resultItem.LoteValidade != null ? string.Format("{0:dd/MM/yyyy}", resultItem.LoteValidade.Validade) : ""
                                    , resultItem.LoteValidade != null && resultItem.LoteValidade.EstoqueLaboratorio != null ?
                                      string.Concat("     Laboratório: ", resultItem.LoteValidade.EstoqueLaboratorio.Descricao) : string.Empty
                                    , "     Qtd: ", resultItem.QuantidadeAtual
                                    , "     <b>Em Uso no Movimento</b> ");
                            }
                            resultList.Add(new GenericoIdNome(resultItem.LoteValidadeId ?? 0, nome));
                        }
                    }
                    else
                    {
                        resultList = produtoSaldoList.Select(
                            x => new GenericoIdNome
                            {
                                Id = x.LoteValidadeId ?? 0,
                                Nome = string.Concat(
                                    "Lt:", x.LoteValidade != null ? x.LoteValidade.Lote : ""
                                    , "     Validade: ", x.LoteValidade != null ? string.Format("{0:dd/MM/yyyy}", x.LoteValidade.Validade) : ""
                                    , x.LoteValidade != null && x.LoteValidade.EstoqueLaboratorio != null ? string.Concat("     Laboratório: ", x.LoteValidade.EstoqueLaboratorio.Descricao) : string.Empty
                                    , "     Qtd: ", x.QuantidadeAtual)

                            }).ToList();
                    }
                    return resultList;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        private class ItemsNoMovimento
        {
            public long? ProdutoId { get; set; }
            public long? LoteValidadeId { get; set; }
            public decimal Quantidade { get; set; }

            public ItemsNoMovimento()
            {

            }
            public ItemsNoMovimento(long? produtoId, long? loteValidadeId, decimal quantidade)
            {
                ProdutoId = produtoId;
                LoteValidadeId = loteValidadeId;
                Quantidade = quantidade;
            }
        }

        //public async Task<List<GenericoIdNome>> ObterPorProdutoEstoquePorSaldo(long produtoId, long estoqueId, long preMovimentoId, long? loteValidadeId = null)
        //{
        //    try
        //    {
        //        using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
        //        {
        //            var result =  await produtoSaldoRepository.Object.GetAll()
        //                .Include(x => x.Produto)
        //                .Include(x => x.LoteValidade)
        //                .Include(x => x.LoteValidade.EstoqueLaboratorio)
        //                .AsNoTracking()
        //                .Where(x => x.ProdutoId == produtoId && x.EstoqueId == estoqueId)
        //                .Where(x=> x.QuantidadeAtual > 0 
        //                    && ( x.LoteValidade == null 
        //                        || x.LoteValidade.Validade >= DateTime.Today 
        //                        || (loteValidadeId.HasValue && x.LoteValidadeId == loteValidadeId))).ToListAsync().ConfigureAwait(false);

        //            return result.Select(
        //                x => new GenericoIdNome
        //                {
        //                    Id = x.LoteValidadeId ?? 0,
        //                    Nome = string.Concat(
        //                        "Lt:", x.LoteValidade != null ? x.LoteValidade.Lote : ""
        //                        , "     Validade: ", x.LoteValidade != null ? string.Format("{0:dd/MM/yyyy}", x.LoteValidade.Validade) : ""
        //                        , x.LoteValidade != null && x.LoteValidade.EstoqueLaboratorio != null ? string.Concat("     Laboratório: ", x.LoteValidade.EstoqueLaboratorio.Descricao) : string.Empty
        //                        , "     Qtd: ", x.QuantidadeAtual)

        //                }).ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroPesquisar"), ex);
        //    }

        //}

        public async Task Excluir(EstoquePreMovimentoLoteValidadeDto input)
        {
            try
            {
                using (var estoquePreMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoLoteValidade, long>>())
                using (var produtoSaldoDomainService = IocManager.Instance.ResolveAsDisposable<IProdutoSaldoDomainService>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    await estoquePreMovimentoLoteValidadeRepository.Object.DeleteAsync(input.Id);
                    produtoSaldoDomainService.Object.AtualizarSaldoPreMovimentoItemLoteValidade(input);

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }


        public async Task<List<GenericoIdNome>> ObterPorProdutoEstoqueComSaida(long produtoId
                                                                             , long estoqueId
                                                                             , long tipoMovimentoId
                                                                             , long? unidadeOrganizacionalId
                                                                             , long? atendimentoId)
        {
            try
            {
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                using (var estoqueMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoItem, long>>())
                using (var estoqueMovimentoLoteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimentoLoteValidade, long>>())
                using (var produtoLaboratorioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueLaboratorio, long>>())
                {
                    var query = from mov in estoqueMovimentoRepository.Object.GetAll()
                                join item in estoqueMovimentoItemRepository.Object.GetAll()
                                on mov.Id equals item.MovimentoId

                                join iltv in estoqueMovimentoLoteValidadeRepository.Object.GetAll()
                                on item.Id equals iltv.EstoqueMovimentoItemId


                                join produtoLaboratorio in produtoLaboratorioRepository.Object.GetAll()
                               on iltv.LoteValidade.EstLaboratorioId equals produtoLaboratorio.Id
                                  into laboratoriojoin

                                from joinLaboratorio in laboratoriojoin.DefaultIfEmpty()


                                where mov.EstoqueId == estoqueId
                                   && item.ProdutoId == produtoId
                                  && mov.EstTipoOperacaoId == (long)EnumTipoOperacao.Saida

                                  && mov.EstTipoMovimentoId == tipoMovimentoId

                                  && (
                                      ((tipoMovimentoId == (long)EnumTipoMovimento.Setor_Saida
                                     || tipoMovimentoId == (long)EnumTipoMovimento.GastoSala_Saida)
                                     && mov.UnidadeOrganizacionalId == unidadeOrganizacionalId)

                                     || (tipoMovimentoId == (long)EnumTipoMovimento.Paciente_Saida
                                        && mov.AtendimentoId == atendimentoId)
                                     )

                                //(((mov.IsEntrada ? 1 : 0) * 2) - 1)
                                select new { LoteValidade = iltv.LoteValidade, Quantidade = iltv.Quantidade, ProdutoLaboratorio = joinLaboratorio };



                    //var queryPreMovimento = from mov in _estoquePreMovimentoRepository.GetAll()
                    //                        join item in _estoquePreMovimentoItemRepository.GetAll()
                    //                        on mov.Id equals item.PreMovimentoId
                    //                        join iltv in _estoquePreMovimentoLoteValidadeRepository.GetAll()
                    //                        on item.Id equals iltv.EstoquePreMovimentoItemId
                    //                        into movjoin
                    //                        from joinedmov in movjoin.DefaultIfEmpty()

                    //                        join produtoLaboratorio in _produtoLaboratorioRepository.GetAll()
                    //        on joinedmov.LoteValidade.ProdutoLaboratorioId equals produtoLaboratorio.Id

                    //                        where mov.EstoqueId == estoqueId
                    //                           && item.ProdutoId == produtoId
                    //                           && mov.Id == preMovimentoId
                    //                        select new { LoteValidade = joinedmov.LoteValidade, Quantidade = ((((mov.IsEntrada ? 1 : 0) * 2) - 1) * joinedmov.Quantidade), ProdutoLaboratorio = produtoLaboratorio };

                    var queryList = query.ToList();
                    //  var queryPreMovimentoList = queryPreMovimento.ToList();

                    //   queryList.AddRange(queryPreMovimentoList);

                    var lotesValidade = queryList.GroupBy(g => g.LoteValidade).Select(s => new { LoteValidade = s.Key, Qtd = s.Sum(soma => soma.Quantidade), ProdutoLaboratorio = s.Key.EstoqueLaboratorio }).ToList();

                    var listaGenericoIdNome = new List<GenericoIdNome>();

                    foreach (var item in lotesValidade.OrderBy(o => o.LoteValidade.Validade))
                    {
                        var nome = String.Concat("Lt:", item.LoteValidade.Lote
                                                , "     Validade: ", String.Format("{0:dd/MM/yyyy}", item.LoteValidade.Validade)
                                                , item.ProdutoLaboratorio != null ? string.Concat("     Laboratório: ", item.ProdutoLaboratorio.Descricao) : string.Empty
                                                , "     Qtd: ", item.Qtd);
                        listaGenericoIdNome.Add(new GenericoIdNome { Id = item.LoteValidade.Id, Nome = nome });
                    }

                    return listaGenericoIdNome;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public List<GenericoIdNome> ObterPorProdutoEstoqueComSaida(long produtoId, long estoqueId, long laboratorioId)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {

                var listaGenericoIdNome = new List<GenericoIdNome>();

                var query = produtoSaldoRepository.Object.GetAll()
                            .Where(w => w.LoteValidade.EstLaboratorioId == laboratorioId
                                     && w.ProdutoId == produtoId
                                     && w.EstoqueId == estoqueId
                                     && w.QuantidadeAtual > 0).AsNoTracking().ToList();

                foreach (var item in query)
                {
                    var generico = new GenericoIdNome();

                    generico.Id = item.Id;
                    generico.Nome = String.Concat("Lt: ", item.LoteValidade.Lote, "     Validade: ", String.Format("{ 0:dd / MM / yyyy}", item.LoteValidade.Validade));

                    listaGenericoIdNome.Add(generico);
                }

                return listaGenericoIdNome;
            }
        }



        public async Task<IResultDropdownList<long>> ListarProdutoDropdownPorLaboratorio(DropdownInput dropdownInput)
        {
            var fromClause = @"LoteValidade 
                                INNER JOIN ProdutoSaldo  ON LoteValidade.ProdutoId = ProdutoSaldo.ProdutoId AND LoteValidade.Id = ProdutoSaldo.LoteValidadeId 
                                LEFT JOIN EstLaboratorio ON LoteValidade.EstEstoqueLaboratorioId = EstLaboratorio.Id";

            return await Select2Helper.CreateSelect2(this)
                .AddIdField("LoteValidade.Id")
                .AddTextField("CONCAT('Lt: ',LoteValidade.Lote, ' Val: ',convert(varchar, LoteValidade.Validade, 103), ' Qtd: ', ProdutoSaldo.QuantidadeAtual, ' Lab: ', EstLaboratorio.Descricao)")
                .AddFromClause(fromClause)
                .AddOrderByClause("LoteValidade.Validade ASC, LoteValidade.Lote DESC ")
                .AddWhereMethod((input, dapperParameters) =>
                {
                    var whereBuilder = new StringBuilder();

                    if (!input.id.IsNullOrEmpty())
                    {
                        dapperParameters.Add("loteValidadeId", input.id);
                        whereBuilder.Append(" ProdutoSaldo.IsDeleted = 0  AND LoteValidade.IsDeleted = 0 AND LoteValidade.Id = @loteValidadeId ");
                    }
                    else
                    {
                        long produtoId = input.filtros.Length > 0 && input.filtros[0] != null ? long.Parse(input.filtros[0]) : 0;
                        long estoqueId = input.filtros.Length > 1 && input.filtros[1] != null ? long.Parse(input.filtros[1]) : 0;
                        long laboratorioId = input.filtros.Length > 2 && input.filtros[2] != null ? long.Parse(input.filtros[2]) : 0;
                        long loteValidadeId = input.filtros.Length > 3 && input.filtros[3] != null ? long.Parse(input.filtros[3]) : 0;
                        bool isEntrada = input.filtros.Length > 4 && input.filtros[4] != null ? bool.Parse(input.filtros[4]) : false;

                        dapperParameters.Add("produtoId", produtoId);
                        dapperParameters.Add("estoqueId", estoqueId);
                        dapperParameters.Add("laboratorioId", laboratorioId);
                        dapperParameters.Add("loteValidadeId", loteValidadeId);

                        whereBuilder.Append(" ProdutoSaldo.IsDeleted = 0  AND LoteValidade.IsDeleted = 0 AND LoteValidade.ProdutoId = @produtoId AND ProdutoSaldo.EstoqueId = @estoqueId");

                        if (loteValidadeId == 0)
                        {
                            // IsEntrada = true - Devolução
                            if (isEntrada)
                            {
                                whereBuilder.Append(" AND QuantidadeAtual >= 0");
                            }
                            else {
                                whereBuilder.Append(" AND QuantidadeAtual > 0");
                            }

                            whereBuilder.Append(" AND LoteValidade.Validade >= GETDATE()");
                        }
                        else {
                            whereBuilder.Append(" AND LoteValidade.Id = @loteValidadeId");
                        }

                        whereBuilder.WhereIf(laboratorioId != 0, " AND LoteValidade.LaboratorioId = @laboratorioId");
                        whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND (LoteValidade.Lote LIKE '%' + @search + '%' OR LoteValidade.Validade LIKE '%' + @search + '%' OR ProdutoSaldo.QuantidadeAtual LIKE '%' + @search + '%')");
                    }


                    return whereBuilder.ToString();
                }).ExecuteAsync(dropdownInput);
        }

        public async Task<List<LoteValidadeGridDto>> ObterPorProdutoEstoqueLaboratorio(long produtoId, long estoqueId, long? laboratorioId)
        {
            using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
            {
                var query = produtoSaldoRepository.Object.GetAll()
                            .Include(x => x.LoteValidade)
                            .Where(w => w.LoteValidade.EstLaboratorioId == laboratorioId
                                     && w.ProdutoId == produtoId
                                     && w.EstoqueId == estoqueId
                                     && w.QuantidadeAtual > 0 && (w.LoteValidade != null && w.LoteValidade.Validade >= DateTime.Today))
                            .GroupBy(g => g.LoteValidadeId)
                            .Select(s => new { Key = s.Key, Soma = s.Sum(soma => soma.QuantidadeAtual), LoteValidade = s.FirstOrDefault().LoteValidade });

                var result = await query.AsQueryable()
                                    .AsNoTracking()
                                    .OrderBy("LoteValidade.Validade, LoteValidade.Lote DESC")
                                    .Select(s => new LoteValidadeGridDto
                                    {
                                        Id = s.Key ?? 0,
                                        Lote = s.LoteValidade.Lote,
                                        Validade = s.LoteValidade.Validade,
                                        Laboratorio = s.LoteValidade.EstoqueLaboratorio.Descricao,
                                        Quantidade = s.Soma,
                                        LaboratorioId = s.LoteValidade.EstoqueLaboratorio.Id,
                                        LoteValidadeId = s.LoteValidade.Id
                                    }).ToListAsync();

                return result;
            }
        }

        public LoteValidadeDto Obter(long produtoId, string lote, DateTime validade, long? laboratorioId)
        {
            using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
            {
                var loteValidade = loteValidadeRepository.Object.GetAll()
                                                          .Where(w => w.ProdutoId == produtoId
                                                                  && w.Lote == lote
                                                                  && w.Validade == validade
                                                                  && (laboratorioId == null || w.EstLaboratorioId == laboratorioId))
                                                          .FirstOrDefault();

                if (loteValidade != null)
                {
                    return LoteValidadeDto.Mapear(loteValidade);
                }

                return null;
            }
        }

        public async Task<PagedResultDto<LoteValidadeGridDto>> ListarPorProduto(LoteValidadeListarInput input)
        {
            try
            {
                using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                {
                    var query = produtoSaldoRepository.Object.GetAll()
                                                .Include(i => i.LoteValidade)
                                                    .Include(i => i.LoteValidade.EstoqueLaboratorio)
                                                    .Where(w => w.ProdutoId == input.produtoId && w.LoteValidadeId != null)
                                                    .GroupBy(g => g.LoteValidadeId)
                                                    .Select(s => new { Key = s.Key, Soma = s.Sum(soma => soma.QuantidadeAtual), LoteValidade = s.FirstOrDefault().LoteValidade });
                    var count = await query.CountAsync();

                    var result = query.AsQueryable()
                                .AsNoTracking()
                                .OrderBy("Soma")
                                .PageBy(input)
                                .Select(s => new LoteValidadeGridDto
                                {
                                    Id = s.Key ?? 0,
                                    Lote = s.LoteValidade.Lote,
                                    Validade = s.LoteValidade.Validade,
                                    Laboratorio = s.LoteValidade.EstoqueLaboratorio.Descricao,
                                    Quantidade = s.Soma
                                })
                                .ToList();
                    return new PagedResultDto<LoteValidadeGridDto> { TotalCount = count, Items = result };
                }
            }
            catch (Exception ex)
            {

            }

            return null;

        }

        public async Task<LoteValidadeDto> ObterPorId(long id)
        {
            using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
            {
                var loteValidade = loteValidadeRepository.Object.GetAll()
                                                      .Include(i => i.Produto)
                                                      .Include(i => i.EstoqueLaboratorio)
                                                      .Where(w => w.Id == id)
                                                      .FirstOrDefault();

                if (loteValidade != null)
                {
                    return LoteValidadeDto.Mapear(loteValidade);//.MapTo<LoteValidadeDto>();
                }
            }
            return null;
        }
    }
}
