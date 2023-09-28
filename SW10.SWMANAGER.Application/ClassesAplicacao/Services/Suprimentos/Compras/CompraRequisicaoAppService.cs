using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using RestSharp;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Inputs;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Xml;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ComprasRequisicao
{
    public class CompraRequisicaoAppService : SWMANAGERAppServiceBase, ICompraRequisicaoAppService
    {

        #region ↓ Atributos

        #region → Services
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUltimoIdAppService _ultimoIdAppService;
        private readonly IProdutoAppService _produtoAppService;
        private readonly IUnidadeAppService _unidadeAppService;
        private readonly IFornecedorAppService _fornecedorAppService;
        private readonly IFormaPagamentoAppService _formaPagamentoAppService;
        private readonly IProdutoLaboratorioAppService _produtoLaboratorioAppService;
        #endregion Servicos 

        #region → Repositorios
        private readonly IRepository<CompraRequisicao, long> _compraRequisicaoRepository;
        private readonly IRepository<CompraRequisicaoItem, long> _compraRequisicaoItemRepository;
        private readonly IRepository<CompraMotivoPedido, long> _compraMotivoPedidoRepository;
        private readonly IRepository<CompraAprovacaoStatus, long> _compraAprovacaoStatusRepository;
        private readonly IRepository<CompraRequisicaoTipo, long> _compraTipoRequisicaoRepository;
        private readonly IRepository<CompraRequisicaoModo, long> _compraRequisicaoModoRepository;
        private readonly IRepository<Produto, long> _produtoRepository;
        private readonly IRepository<ProdutoEstoque, long> _ressuprimentoRepository;
        private readonly IRepository<ProdutoSaldo, long> _produtoSaldoRepository;
        private readonly IRepository<CompraCotacaoItem, long> _compraCotacaoItemRepository;
        private readonly IRepository<CompraCotacao, long> _compraCotacaoRepository;
        #endregion  Repositorios

        #endregion

        #region ↓ Construtores

        public CompraRequisicaoAppService(
        #region → Services
            IUnitOfWorkManager unitOfWorkManager,
            IUltimoIdAppService ultimoServicoAppService,
            IProdutoAppService produtoAppService,
            IUnidadeAppService unidadeAppService,
            IFornecedorAppService fornecedorAppService,
            IFormaPagamentoAppService formaPagamentoAppService,
            IProdutoLaboratorioAppService produtoLaboratorioAppService,
        #endregion

        #region → Repositorios
            IRepository<CompraRequisicao, long> compraRequisicaoRepository,
            IRepository<CompraRequisicaoItem, long> compraRequisicaoItemRepository,
            IRepository<CompraMotivoPedido, long> compraMotivoPedidoRepository,
            IRepository<CompraAprovacaoStatus, long> compraAprovacaoStatusRepository,
            IRepository<CompraRequisicaoTipo, long> compraTipoRequisicaoRepository,
            IRepository<CompraRequisicaoModo, long> compraRequisicaoModoRepository,
            IRepository<Produto, long> produtoRepository,
            IRepository<ProdutoEstoque, long> ressuprimentoRepository,
            IRepository<ProdutoSaldo, long> produtoSaldoRepository,
            IRepository<CompraCotacaoItem, long> compraCotacaoItemRepository,
            IRepository<CompraCotacao, long> compraCotacaoRepository
        #endregion
        )
        {
            #region → Services
            _unitOfWorkManager = unitOfWorkManager;
            _ultimoIdAppService = ultimoServicoAppService;
            _produtoAppService = produtoAppService;
            _unidadeAppService = unidadeAppService;
            _fornecedorAppService = fornecedorAppService;
            _formaPagamentoAppService = formaPagamentoAppService;
            _produtoLaboratorioAppService = produtoLaboratorioAppService;
            #endregion

            #region → Repositorios
            _compraRequisicaoRepository = compraRequisicaoRepository;
            _compraRequisicaoItemRepository = compraRequisicaoItemRepository;
            _compraMotivoPedidoRepository = compraMotivoPedidoRepository;
            _compraAprovacaoStatusRepository = compraAprovacaoStatusRepository;
            _compraTipoRequisicaoRepository = compraTipoRequisicaoRepository;
            _compraRequisicaoModoRepository = compraRequisicaoModoRepository;
            _produtoRepository = produtoRepository;
            _ressuprimentoRepository = ressuprimentoRepository;
            _produtoSaldoRepository = produtoSaldoRepository;
            _compraCotacaoItemRepository = compraCotacaoItemRepository;
            _compraCotacaoRepository = compraCotacaoRepository;
            #endregion
        }

        #endregion

        #region ↓ Metodos

        #region → CRUD
        /// <summary>
        /// Inclui ou Edita uma Requisicao de Compra.
        /// Faz o mesmo nos itens da requisicao relacionados</summary>
        /// <param name="input"></param>
        /// <returns>CompraRequisicaoDto</returns>
        [UnitOfWork]
        public async Task<CompraRequisicaoDto> CriarOuEditar(CompraRequisicaoDto input)
        {
            try
            {
                var compraRequisicao = input.MapTo<CompraRequisicao>();

                var compraRequisicoesItem = new List<CompraRequisicaoItemDto>();

                //Deserializa os itens
                if (!input.RequisicoesItensJson.IsNullOrWhiteSpace())
                {
                    string requisicoesItensJson = input.RequisicoesItensJson.ToLower(); //← atJSON 

                    compraRequisicoesItem = JsonConvert.DeserializeObject<List<CompraRequisicaoItemDto>>(requisicoesItensJson);
                }

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    #region 1 - grava requisicao
                    if (input.Id.Equals(0))
                    {
                        compraRequisicao.Codigo = _ultimoIdAppService.ObterProximoCodigo("CompraRequisicao").Result;
                        compraRequisicao.AprovacaoStatusId = 1;
                        compraRequisicao.Id = await _compraRequisicaoRepository.InsertAndGetIdAsync(compraRequisicao);
                    }
                    else
                    {
                        compraRequisicao.AprovacaoStatusId = 1;
                        await _compraRequisicaoRepository.UpdateAsync(compraRequisicao);
                    }
                    #endregion 

                    #region 2 - grava itens da requisicao
                    foreach (var compraRequisicaoItem in compraRequisicoesItem)
                    {
                        compraRequisicaoItem.RequisicaoId = compraRequisicao.Id;

                        if (compraRequisicaoItem.IsDeleted)
                        {
                            await _compraRequisicaoItemRepository.DeleteAsync(compraRequisicaoItem.Id);
                        }
                        else
                        {
                            if (compraRequisicaoItem.Id.Equals(0))
                            {
                                await _compraRequisicaoItemRepository.InsertAndGetIdAsync(compraRequisicaoItem.MapTo<CompraRequisicaoItem>());
                            }
                            else
                            {
                                await _compraRequisicaoItemRepository.UpdateAsync(compraRequisicaoItem.MapTo<CompraRequisicaoItem>());
                            }
                        }
                    }

                    #endregion region

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

                return compraRequisicao.MapTo<CompraRequisicaoDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        [UnitOfWork]
        public async Task Excluir(CompraRequisicaoDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _compraRequisicaoRepository.DeleteAsync(input.Id);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CompraRequisicaoDto</returns>
        public async Task<CompraRequisicaoDto> Obter(long id)
        {
            try
            {
                var result = await _compraRequisicaoRepository
                    .GetAll()
                    .Include(m => m.Estoque)
                    .Include(m => m.MotivoPedido)
                    .Include(m => m.TipoRequisicao)
                    .Include(m => m.UnidadeOrganizacional)
                    .Include(m => m.AprovacaoStatus)
                    .Include(m => m.Empresa)
                    .Include(m => m.FinFormaPagamento)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var requisicaoCompra = result
                    .MapTo<CompraRequisicaoDto>();

                return requisicaoCompra;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async void VoltarRequisicaoStatusInicial(long requisicaoId)
        {
            try
            {
                var requisicao = _compraRequisicaoRepository.GetAll().Where(x => x.Id == requisicaoId).FirstOrDefault();

                requisicao.IsRequisicaoAprovada = false;
                requisicao.AprovacaoStatusId = 1;

                await _compraRequisicaoRepository.UpdateAsync(requisicao);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task<CompraRequisicaoDto> AprovarOuRecusarRequisicao(CompraRequisicaoDto input)
        {
            try
            {
                var compraRequisicao = input.MapTo<CompraRequisicao>();

                compraRequisicao.IsRequisicaoAprovada = true;

                var compraRequisicoesItem = new List<CompraRequisicaoItemDto>();

                //Deserializa os itens
                if (!input.RequisicoesItensJson.IsNullOrWhiteSpace())
                {
                    string requisicoesItensJson = input.RequisicoesItensJson.ToLower(); //← atJSON 

                    compraRequisicoesItem = JsonConvert.DeserializeObject<List<CompraRequisicaoItemDto>>(requisicoesItensJson);
                }

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {

                    #region 1 - grava requisicao

                    await _compraRequisicaoRepository.UpdateAsync(compraRequisicao);

                    #endregion 


                    #region 2 - grava itens da requisicao
                    foreach (var compraRequisicaoItem in compraRequisicoesItem)
                    {
                        compraRequisicaoItem.RequisicaoId = compraRequisicao.Id;

                        await _compraRequisicaoItemRepository.UpdateAsync(compraRequisicaoItem.MapTo<CompraRequisicaoItem>());
                    }

                    #endregion region

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

                return compraRequisicao.MapTo<CompraRequisicaoDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        #endregion Basico - CRUD

        #region → Basico - Listar
        /// <summary>
        /// Responsável por Listar as Compras Requisições
        /// </summary>
        /// <param name="input">Dados de entrada</param>
        /// <returns></returns>
        public async Task<PagedResultDto<CompraRequisicaoIndexDto>> Listar(ListarRequisicoesCompraInput input)
        {
            var contarComprasRequisicao = 0;

            long StatusRequisicao;
            long.TryParse(input.StatusRequisicao, out StatusRequisicao);

            List<CompraRequisicaoIndexDto> compraRequisicoesIndexDtos;

            try
            {
                DateTime startDate = ((DateTime)input.StartDate).Date;
                DateTime endDate = ((DateTime)input.EndDate).Date;
                endDate = endDate.AddDays(1);
                endDate = endDate.AddSeconds(-1);

                var query = _compraRequisicaoRepository
                    .GetAll()
                    .Include(m => m.Empresa)
                    .Include(m => m.AprovacaoStatus)
                    .WhereIf(!string.IsNullOrEmpty(input.EmpresaId), e => e.EmpresaId.ToString() == input.EmpresaId)
                    .WhereIf(!string.IsNullOrEmpty(input.UnidadeOrganizacionalId), e => e.UnidadeOrganizacionalId.ToString() == input.UnidadeOrganizacionalId)
                    .WhereIf(!string.IsNullOrEmpty(input.EstoqueId), e => e.EstoqueId.ToString() == input.EstoqueId)
                    .WhereIf(!string.IsNullOrEmpty(input.MotivoPedidoId), e => e.MotivoPedidoId.ToString() == input.MotivoPedidoId)
                    .WhereIf(!string.IsNullOrEmpty(input.Codigo), e => e.Codigo == input.Codigo)
                    .WhereIf(!string.IsNullOrEmpty(input.AprovacaoStatusId), e => e.AprovacaoStatusId.ToString() == input.AprovacaoStatusId)
                    .WhereIf(StatusRequisicao == 1, m => m.IsRequisicaoAprovada == true)
                    .WhereIf(StatusRequisicao == 2, m => m.IsRequisicaoAprovada == false)
                    .WhereIf(input.IsUrgente.HasValue, e => e.IsUrgente == input.IsUrgente)
                    .Where(_ => _.DataRequisicao >= startDate && _.DataRequisicao <= endDate)
                    .Select(m => new CompraRequisicaoIndexDto
                    {
                        Id = m.Id,
                        Codigo = m.Codigo,
                        Empresa = m.Empresa.RazaoSocial,
                        UnidadeOrganizacional = m.UnidadeOrganizacional.Descricao,
                        IsUrgente = m.IsUrgente,
                        IsRequisicaoAprovada = m.IsRequisicaoAprovada,
                        AprovacaoStatus = m.AprovacaoStatus.Descricao,
                        DataLimiteEntrega = m.DataLimiteEntrega,
                        DataRequisicao = m.DataRequisicao
                    });
                #endregion

                contarComprasRequisicao = await query
                    .CountAsync();

                input.Sorting = "DataRequisicao";

                compraRequisicoesIndexDtos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<CompraRequisicaoIndexDto>(
                contarComprasRequisicao,
                compraRequisicoesIndexDtos
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ListResultDto<CompraRequisicaoDto>> ListarTodos()
        {
            List<CompraRequisicaoDto> requisicoesCompraDtos = new List<CompraRequisicaoDto>();
            try
            {
                var requisicoesCompra = await _compraRequisicaoRepository
                    .GetAll()
                    //.Include(m => m.Cidade)
                    .AsNoTracking()
                    .ToListAsync();

                requisicoesCompraDtos = requisicoesCompra
                    .MapTo<List<CompraRequisicaoDto>>();

                return new ListResultDto<CompraRequisicaoDto> { Items = requisicoesCompraDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        #endregion

        #region → Gets

        /// <summary>
        /// Retorna um Dto de Modo de Requisicao to tipo Manual
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CompraRequisicaoDto</returns>
        public async Task<GenericoIdNome> ObterModoRequisicaoManual()
        {
            try
            {
                var result = await _compraRequisicaoModoRepository
                    .GetAll()
                    .Where(m => m.Codigo == "1")
                    .Select(m => new GenericoIdNome
                    {
                        Id = m.Id,
                        Nome = m.Descricao
                    })
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna um Dto de Modo de Requisicao to tipo Automatico
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CompraRequisicaoDto</returns>
        public async Task<GenericoIdNome> ObterModoRequisicaoAutomatico()
        {
            try
            {
                var result = await _compraRequisicaoModoRepository
                    .GetAll()
                    .Where(m => m.Codigo == "2")
                    .Select(m => new GenericoIdNome
                    {
                        Id = m.Id,
                        Nome = m.Descricao
                    })
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Retorna um Dto de Motivo de Pedido para Requisicao de Estoque
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Id e Descricao(Nome) do Motivo de Pedido Resposição de Estoque</returns>
        public async Task<GenericoIdNome> ObterMotivoPedidoReposicaoEstoque()
        {
            try
            {
                var result = await _compraMotivoPedidoRepository
                    .GetAll()
                    .Where(m => m.Codigo == "1")
                    .Select(m => new GenericoIdNome
                    {
                        Id = m.Id,
                        Nome = m.Descricao
                    })
                    .FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        #endregion

        #region → Requisicoes Itens
        /// <summary>
        /// Retorna lista(ListResultDto) de CompraRequisicaoItemDto.
        /// Carga dinamica no IdGrid para manipulacao dinamica.
        /// É Utilizado no action CriarOuEditar
        /// </summary>
        /// <param name="id">id da Requisicao</param>
        /// <returns>ListResultDto<CompraRequisicaoItemDto></returns>
        public async Task<ListResultDto<CompraRequisicaoItemDto>> ListarRequisicaoItem(long id)
        {
            try
            {
                var idGrid = 0;
                var query = _compraRequisicaoItemRepository
                            .GetAll()
                            .Where(m => m.RequisicaoId == id);

                var list = await query.ToListAsync();

                var listDto = list.MapTo<List<CompraRequisicaoItemDto>>();

                listDto.ForEach(m => m.IdGrid = ++idGrid);

                return new ListResultDto<CompraRequisicaoItemDto> { Items = listDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        /// <summary>
        /// Recebe uma lista de objetos e devolve a mesma lista para popular um grid
        /// Utilizado na funcao RetornaLista, esta que é usada no listAction do JTable
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CompraRequisicaoItemDto>> ListarItensJson(List<CompraRequisicaoItemDto> list)
        {
            try
            {
                var count = 0;
                if (list == null)
                {
                    list = new List<CompraRequisicaoItemDto>();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].IdGrid = i + 1;

                    //set produto e unidade requisicao
                    if (list[i].Produto == null)
                    {
                        var produto = await _produtoAppService.Obter(list[i].ProdutoId);
                        list[i].Produto = produto;
                    }

                    if (list[i].Unidade == null)
                    {
                        var unidade = await _unidadeAppService.Obter(list[i].UnidadeId);
                        list[i].Unidade = unidade;
                    }

                    //set produto e unidade aprovacao
                    if (list[i].ProdutoAprovacaoId != null)
                    {
                        var produto = await _produtoAppService.Obter((long)list[i].ProdutoAprovacaoId);
                        list[i].ProdutoAprovacao = produto;
                    }

                    if (list[i].UnidadeAprovacaoId != null)
                    {
                        var unidade = await _unidadeAppService.Obter((long)list[i].UnidadeAprovacaoId);
                        list[i].UnidadeAprovacao = unidade;
                    }
                }

                count = await Task.Run(() => list.Count());

                return new PagedResultDto<CompraRequisicaoItemDto>(
                      count,
                      list
                      );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar", ex));
            }
        }

        /// <summary>
        /// Traz uma listagem de Todos os Produtos com saldo abaixo do Estoque Minimo para ser usado em uma requisicao automatica
        /// </summary>
        /// <param name="input">Utilize a propriedade filtro para passar o id do Grupo de Produtos</param>
        /// <returns>ListResultDto<CompraRequisicaoItemDto></returns>
        //public async Task<PagedResultDto<CompraRequisicaoItemDto>> ListarRequisicaoAutomatica(ListarRequisicoesCompraInput input)
        public async Task<ListResultDto<CompraRequisicaoItemDto>> ListarRequisicaoAutomatica(ListarRequisicoesCompraInput input)
        {
            try
            {
                var idGrid = 0;
                long idGrupo = 0;
                long.TryParse(input.Filtro, out idGrupo);
                long idEstoque = 0;
                long.TryParse(input.EstoqueId, out idEstoque);

                /* ↓ Um expert em LINQ poderá simplificar a consulta abaixo ↓ */

                #region Query dos Produtos abaixo do Estoque Minimo
                var qryProdQtd = from prod in _produtoRepository.GetAll()
                                 join rsup in _ressuprimentoRepository.GetAll() on prod.Id equals rsup.ProdutoId into joinedRsup
                                 from ressupJoin in joinedRsup.DefaultIfEmpty()
                                 join saldo in _produtoSaldoRepository.GetAll() on prod.Id equals saldo.ProdutoId into joinedSaldo
                                 from saldoJoin in joinedSaldo.DefaultIfEmpty()

                                     //join unidade in _produtoUnidadeRepository.GetAll() on prod.Id equals unidade.ProdutoId into joinedUnidade
                                     //from saldoJoin in joinedSaldo.DefaultIfEmpty()

                                 where 1 == 1
                                     && (prod.GrupoId == idGrupo) // ← Grupo de Produtos passado no input
                                     && (saldoJoin.EstoqueId == idEstoque) // ← Estoque passado no input

                                     //&& (prod.Id == ressupJoin.ProdutoId)    //← Acho que nao precisa disso
                                     //&& (prod.Id == saldoJoin.ProdutoId)

                                     && (!prod.IsPrincipal) //← Não é Produto Principal
                                     && (!prod.IsBloqueioCompra) //← Produto liberado para Compra
                                     && (prod.IsAtivo == true) //← produto deve estar ativo
                                                               // TODO: Conferir se está retornando produtos excluidos

                                     //&& (!compraReqJoin.IsEncerrada)

                                     // ↓ o saldo deve estar abaixo do estoque minimo
                                     // saldo disponivel + requisicao pendente de compra < estoque minimo
                                     && (
                                           (saldoJoin.QuantidadeAtual + saldoJoin.QuantidadeEntradaPendente - saldoJoin.QuantidadeSaidaPendente)

                                           < ressupJoin.EstoqueMinimo
                                        )
                                 //TODO: testar se a formula saldo < estoque esta funcionando

                                 select new
                                 {
                                     ProdutoId = prod.Id,
                                     Produto = prod,
                                     EstoqueMinimo = ressupJoin.EstoqueMinimo,
                                     EstoqueMaximo = ressupJoin.EstoqueMaximo,
                                     PontoPedido = ressupJoin.PontoPedido,
                                     QuantidadeAtual = saldoJoin.QuantidadeAtual,
                                     QuantidadeEntradaPendente = saldoJoin.QuantidadeEntradaPendente,
                                     QuantidadeSaidaPendente = saldoJoin.QuantidadeSaidaPendente
                                 };
                #endregion

                input.Sorting = "ProdutoId";

                var list = await qryProdQtd.ToListAsync();

                #region Query para produtos em Requisicoes não encerradas
                var qryCmpReq = from compraReq in _compraRequisicaoRepository.GetAll()
                                join compraReqItem in _compraRequisicaoItemRepository.GetAll() on compraReq.Id equals compraReqItem.RequisicaoId into joinedRequisicao
                                from compraReqJoin in joinedRequisicao.DefaultIfEmpty()
                                where 1 == 1
                                && (!compraReq.IsRequisicaoAprovada == true)
                                select new
                                {
                                    compraReqJoin
                                } into t1
                                group t1 by t1.compraReqJoin.ProdutoId into g
                                select new
                                {
                                    ProdutoId = g.FirstOrDefault().compraReqJoin.ProdutoId,
                                    QtdReq = g.Sum(m => g.FirstOrDefault().compraReqJoin.Quantidade)
                                };
                #endregion

                var list2 = await qryCmpReq.ToListAsync();

                #region final
                var joinRes = (
                                from prod in qryProdQtd
                                join req in qryCmpReq on prod.ProdutoId equals req.ProdutoId into joined
                                from req in joined.DefaultIfEmpty()
                                    //from prodReqJoin in joined.DefaultIfEmpty()
                                where 1 == 1
                                && (
                                      ((prod.QuantidadeAtual + prod.QuantidadeEntradaPendente - prod.QuantidadeSaidaPendente)
                                      + (req.QtdReq == null ? default(Decimal) : req.QtdReq)
                                      )// prodReqJoin.QtdReq)
                                      < prod.EstoqueMinimo
                                   )
                                select new CompraRequisicaoItemDto
                                {
                                    ProdutoId = prod.ProdutoId,
                                    UnidadeId = 1,
                                    //↓ Ponto de Pedido - Saldo - Qtd total do Produto para o estoque selecionado em Requisicoes nao encerradas
                                    Quantidade = prod.PontoPedido - (prod.QuantidadeAtual + prod.QuantidadeEntradaPendente - prod.QuantidadeSaidaPendente) - (req.QtdReq == null ? default(Decimal) : req.QtdReq),
                                    //(req.QtdReq.ToString().IsNullOrEmpty() ? 0 : req.QtdReq),//req.QtdReq,//prodReqJoin.QtdReq,

                                    //ProdutoAprovacaoId = prod.ProdutoId,
                                    //UnidadeAprovacaoId = 1,
                                    ////↓ Ponto de Pedido - Saldo - Qtd total do Produto para o estoque selecionado em Requisicoes nao encerradas
                                    //QuantidadeAprovacao = null, //prod.PontoPedido - (prod.QuantidadeAtual + prod.QuantidadeEntradaPendente - prod.QuantidadeSaidaPendente) - prodReqJoin.QtdReq,

                                    ModoInclusao = "A" //← automatico
                                }
                              ).ToList();
                #endregion

                //var listDto = list.MapTo<List<CompraRequisicaoItemDto>>();

                var listDto = joinRes.MapTo<List<CompraRequisicaoItemDto>>();

                listDto.ForEach(m => m.IdGrid = ++idGrid);

                return new ListResultDto<CompraRequisicaoItemDto> { Items = listDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }


        }
        #endregion Requisicoes Itens

        #region Aprovacao

        #endregion

        #region Cotação
        public void EnviarCotacaoBionexo(long[] compraRequisicaoIds)
        {
            try
            {
                foreach (var compraRequisicaoId in compraRequisicaoIds)
                {
                    var listItensRequisicao = _compraRequisicaoItemRepository.GetAll().Include(x => x.Produto).Include(x => x.Requisicao).Where(m => m.RequisicaoId == compraRequisicaoId).ToList();

                    this.EnviarCotacaoWebserviceBionexo(listItensRequisicao);
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        private void EnviarCotacaoWebserviceBionexo(List<CompraRequisicaoItem> listItensRequisicao)
        {
            const string ERRO_RETORNO_BIONEXO = "-1";
            var requisicaoId = listItensRequisicao[0].Requisicao.Id;
            var requisicaoCodigo = listItensRequisicao[0].Requisicao.Codigo;
            var requisicaoObservacao = listItensRequisicao[0].Requisicao.Observacao;
            var requisicaoTitulo = (!string.IsNullOrEmpty(listItensRequisicao[0].Requisicao.Descricao) ? listItensRequisicao[0].Requisicao.Descricao.Trim() : ("Requisição de Compra " + listItensRequisicao[0].Requisicao.Codigo));
            var requisicaoFormaPagamentoBionexoId = (listItensRequisicao[0].Requisicao.FinFormaPagamentoId != null ? listItensRequisicao[0].Requisicao.FinFormaPagamento.CodigoBionexo : 1);
            var requisicaoDataVencimento = (listItensRequisicao[0].Requisicao.DataHoraVencimento != null ? listItensRequisicao[0].Requisicao.DataHoraVencimento?.ToString("dd/MM/yyyy") : DateTime.Now.AddDays(1).ToString("dd/MM/yyyy"));
            var requisicaoTimeVencimento = (listItensRequisicao[0].Requisicao.DataHoraVencimento != null ? listItensRequisicao[0].Requisicao.DataHoraVencimento?.ToString("HH:mm") : DateTime.Now.AddDays(1).ToString("HH:mm"));

            var client = new RestClient(ConfigurationManager.AppSettings.Get("BionexoWsBaseUrl"));
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/xml");
            var body = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:web=""http://webservice.bionexo.com/"">
                " + "\n" +
                            @"   <soapenv:Header/>
                " + "\n" +
                            @"   <soapenv:Body>
                " + "\n" +
                            @"   <web:post>
                " + "\n" +
                            @"    <login>" + ConfigurationManager.AppSettings.Get("BionexoWsLogin") + @"</login>
                " + "\n" +
                            @"    <password>" + ConfigurationManager.AppSettings.Get("BionexoWsPassword") + @"</password>
                " + "\n" +
                            @"    <operation>" + ConfigurationManager.AppSettings.Get("BionexoWsOperation") + @"</operation>
                " + "\n" +
                            @"    <parameters>" + ConfigurationManager.AppSettings.Get("BionexoWsParameters") + @"</parameters>
                " + "\n" +
                            @"    <xml>
                " + "\n" +
                            @"        <Pedido>
                " + "\n" +
                            @"            <Cabecalho>
                " + "\n" +
                            @"            <Requisicao>" + requisicaoId + @"</Requisicao>
                " + "\n" +
                            @"            <Moeda>Reais</Moeda>               
                " + "\n" +
                            @"            <Observacao>" + requisicaoObservacao + @"</Observacao>                      
                " + "\n" +
                            @"            <Titulo_Pdc>" + requisicaoTitulo + @"</Titulo_Pdc>       
                " + "\n" +
                            @"            <Id_Forma_Pagamento>" + requisicaoFormaPagamentoBionexoId + @"</Id_Forma_Pagamento>              
                " + "\n" +
                            @"            <Data_Vencimento>" + requisicaoDataVencimento + @"</Data_Vencimento> 
                " + "\n" +
                            @"            <Hora_Vencimento>" + requisicaoTimeVencimento + @"</Hora_Vencimento>               
                " + "\n" +
                            @"            </Cabecalho>  
                " + "\n" +
                            @"            <Itens_Requisicao>";

            var itensRequisicao = string.Empty;

            foreach (var itemRequisicao in listItensRequisicao)
            {
                itensRequisicao +=
                    "\n" +
                            @"                <Item_Requisicao>              
                " + "\n" +
                            @"                    <Codigo_Produto>" + itemRequisicao.Produto.Codigo + @"</Codigo_Produto>
                " + "\n" +
                            @"                    <Quantidade>" + itemRequisicao.QuantidadeAprovacao + @"</Quantidade>
                " + "\n" +
                            @"                </Item_Requisicao>";
            }

            body += itensRequisicao;

            body +=
                "\n" +
                        @"            </Itens_Requisicao>     
            " + "\n" +
                        @"        </Pedido>
            " + "\n" +
                        @"    </xml>
            " + "\n" +
                        @"     </web:post>
            " + "\n" +
                        @"   </soapenv:Body>
            " + "\n" +
                        @"</soapenv:Envelope> ";

            request.AddParameter("application/xml", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            using (TextReader reader = new StringReader(response.Content))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(reader);

                XmlNamespaceManager ns = new XmlNamespaceManager(xDoc.NameTable);
                ns.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                ns.AddNamespace("ns1", "http://webservice.bionexo.com/");

                XmlCDataSection cDataNode = (XmlCDataSection)(xDoc.SelectSingleNode("//soap:Envelope/soap:Body/ns1:postResponse/return", ns).ChildNodes[0]);
                var respostaBionexo = cDataNode.Data.Split(';'); // 1;06/10/2022 09:49:47;209198947
                var statusRetorno = respostaBionexo[0].ToString();
                var idRetorno = respostaBionexo[2].ToString();
                var dataRetornoBionexo = DateTime.Parse(respostaBionexo[1].ToString());
                long? idBionexo = null;

                if (statusRetorno != ERRO_RETORNO_BIONEXO) idBionexo = long.Parse(idRetorno);

                string mensagemErroRetornoBionexo = respostaBionexo[0] == ERRO_RETORNO_BIONEXO ? respostaBionexo[2] : string.Empty;

                var compraCotacaoEntity = _compraCotacaoRepository.GetAll().FirstOrDefault(x => x.RequisicaoId == requisicaoId);

                if (compraCotacaoEntity != null) {
                    compraCotacaoEntity.DataEnvioBionexo = dataRetornoBionexo;
                    compraCotacaoEntity.IdBionexo = idBionexo;
                    compraCotacaoEntity.MensagemErroRetornoBionexo = mensagemErroRetornoBionexo;
                    compraCotacaoEntity.UserIdEnvioBionexo = AbpSession.UserId;
                    _compraCotacaoRepository.Update(compraCotacaoEntity);
                }
                else {
                    var compraCotacao = new CompraCotacao();
                    compraCotacao.DataEnvioBionexo = dataRetornoBionexo;
                    compraCotacao.IdBionexo = idBionexo;
                    compraCotacao.MensagemErroRetornoBionexo = mensagemErroRetornoBionexo;
                    compraCotacao.UserIdEnvioBionexo = AbpSession.UserId;
                    compraCotacao.RequisicaoId = requisicaoId;
                    _compraCotacaoRepository.Insert(compraCotacao);
                }
                // Início da Requisição
                listItensRequisicao[0].Requisicao.DataInicioCotacao = DateTime.Now;
                _compraRequisicaoRepository.Update(listItensRequisicao[0].Requisicao);
            }
        }

        public void SalvarOuAtualizarDadosFornecedorProduto(CompraCotacaoFornecedorDto input)
        {
            try
            {
                var compraCotacaoFornecedor = _compraCotacaoRepository.GetAll().FirstOrDefault(x => x.SisFornecedorId == input.FornecedorId && x.RequisicaoId == input.RequisicaoId);

                if (compraCotacaoFornecedor != null)
                {
                    compraCotacaoFornecedor.FinFormaPagamentoId = (long)input.FormaPagamentoId;
                    compraCotacaoFornecedor.PrazoEntregaEmDias = input.PrazoEntregaFornecedorEmDias;
                    _compraCotacaoRepository.Update(compraCotacaoFornecedor);
                }
                else
                {
                    var compraCotacao = new CompraCotacao();
                    compraCotacao.FinFormaPagamentoId = (long)input.FormaPagamentoId;
                    compraCotacao.PrazoEntregaEmDias = input.PrazoEntregaFornecedorEmDias;
                    compraCotacao.RequisicaoId = input.RequisicaoId;
                    compraCotacao.SisFornecedorId = input.FornecedorId;
                    _compraCotacaoRepository.Insert(compraCotacao);

                    compraCotacaoFornecedor = compraCotacao;
                }

                var compraCotacaoItem = _compraCotacaoItemRepository.GetAll().FirstOrDefault(x => x.CompraCotacaoId == compraCotacaoFornecedor.Id && x.RequisicaoItemId == input.RequisicaoItemId);

                if (compraCotacaoItem == null)
                {
                    compraCotacaoItem = new CompraCotacaoItem();
                    compraCotacaoItem.OpcaoComprador = (bool)input.OpcaoComprador;
                    compraCotacaoItem.LaboratorioId = input.LaboratorioId;
                    compraCotacaoItem.PrazoEntregaEmDias = input.PrazoEntregaEmDias;
                    compraCotacaoItem.Quantidade = input.Quantidade;
                    compraCotacaoItem.ValorUnitario = (decimal)input.ValorUnitario;
                    compraCotacaoItem.CompraCotacaoId = compraCotacaoFornecedor.Id;
                    compraCotacaoItem.RequisicaoItemId = input.RequisicaoItemId;
                    _compraCotacaoItemRepository.Insert(compraCotacaoItem);
                }
                else
                {
                    compraCotacaoItem.OpcaoComprador = (bool)input.OpcaoComprador;
                    compraCotacaoItem.LaboratorioId = input.LaboratorioId;
                    compraCotacaoItem.PrazoEntregaEmDias = input.PrazoEntregaEmDias;
                    compraCotacaoItem.Quantidade = input.Quantidade;
                    compraCotacaoItem.ValorUnitario = (decimal)input.ValorUnitario;
                    _compraCotacaoItemRepository.Update(compraCotacaoItem);
                }

                if ((bool)input.OpcaoComprador)
                {
                    // O Produto atual foi marcado como opção de compra. Então será desmarcado os produto anterior com essa opção ativa.
                    var compraCotacaoItemUpdate = _compraCotacaoItemRepository.GetAll().FirstOrDefault(x => x.OpcaoComprador == true && x.RequisicaoItemId == input.RequisicaoItemId);

                    if (compraCotacaoItemUpdate != null) {
                        compraCotacaoItemUpdate.OpcaoComprador = false;
                        _compraCotacaoItemRepository.Update(compraCotacaoItemUpdate);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task<PagedResultDto<CompraRequisicaoIndexDto>> ListarCotacao(ListarRequisicoesCompraInput input)
        {
            var contarComprasRequisicao = 0;

            long StatusRequisicao;
            long.TryParse(input.StatusRequisicao, out StatusRequisicao);

            long StatusAprovacao;
            long.TryParse(input.StatusAprovacao, out StatusAprovacao);

            List<CompraRequisicaoIndexDto> compraRequisicoesIndexDtos;

            try
            {
                DateTime startDate = ((DateTime)input.StartDate).Date;
                DateTime endDate = ((DateTime)input.EndDate).Date;
                endDate = endDate.AddDays(1);
                endDate = endDate.AddSeconds(-1);

                #region query
                var query = _compraRequisicaoRepository
                    .GetAll()
                    .Include(m => m.Empresa)
                    .Include(m => m.Estoque)
                    .Include(m => m.MotivoPedido)
                    .Include(m => m.RequisicaoModo)
                    .Include(m => m.TipoRequisicao)
                    .Include(m => m.AprovacaoStatus)
                    .WhereIf(!string.IsNullOrEmpty(input.EmpresaId), e => e.EmpresaId.ToString() == input.EmpresaId)
                    .WhereIf(!string.IsNullOrEmpty(input.EstoqueId), e => e.EstoqueId.ToString() == input.EstoqueId)
                    .WhereIf(!string.IsNullOrEmpty(input.MotivoPedidoId), e => e.MotivoPedidoId.ToString() == input.MotivoPedidoId)
                    .WhereIf(!string.IsNullOrEmpty(input.Codigo), e => e.Codigo == input.Codigo)
                    .WhereIf(!string.IsNullOrEmpty(input.Id), e => e.Id == long.Parse(input.Id))
                    .WhereIf(StatusRequisicao == 1, m => m.IsRequisicaoAprovada == true)
                    .WhereIf(StatusRequisicao == 2, m => m.IsRequisicaoAprovada == false)
                    .WhereIf(StatusAprovacao == 1, m => m.IsOrdemCompraFinalizada == true)
                    .WhereIf(StatusAprovacao == 2, m => m.IsOrdemCompraFinalizada == false)
                    .WhereIf(!string.IsNullOrEmpty(input.AprovacaoStatusId), e => e.AprovacaoStatusId.ToString() == input.AprovacaoStatusId)
                    .WhereIf(input.IsUrgente.HasValue, e => e.IsUrgente == input.IsUrgente)
                    .Where(_ => _.DataRequisicao >= startDate && _.DataRequisicao <= endDate)
                    .Where(m => m.AprovacaoStatusId == 2)

                    .Select(m => new CompraRequisicaoIndexDto
                    {
                        Id = m.Id,
                        Codigo = m.Codigo,
                        Modo = m.RequisicaoModo.Descricao,
                        Empresa = m.Empresa.RazaoSocial,
                        Estoque = m.Estoque.Descricao,
                        MotivoPedido = m.MotivoPedido.Descricao,
                        IsUrgente = m.IsUrgente,
                        IsRequisicaoAprovada = m.IsRequisicaoAprovada,
                        IsOrdemCompraFinalizada = m.IsOrdemCompraFinalizada,
                        DataLimiteEntrega = m.DataLimiteEntrega,
                        DataRequisicao = m.DataRequisicao,
                        DataInicioCotacao = m.DataInicioCotacao
                    });
                #endregion

                contarComprasRequisicao = await query
                    .CountAsync();

                input.Sorting = "DataRequisicao";

                compraRequisicoesIndexDtos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<CompraRequisicaoIndexDto>(
                contarComprasRequisicao,
                compraRequisicoesIndexDtos
                );
        }

        public async Task<PagedResultDto<CompraCotacaoFornecedorDto>> ListarCotacaoFornecedorItem(long id, long? fornecedorId)
        {
            try
            {
                List<CompraCotacaoFornecedorDto> compraCotacaoFornecedorListDto = new List<CompraCotacaoFornecedorDto>();

                var idGrid = 0;
                var listaItensRequisicao = _compraRequisicaoItemRepository.GetAll().Where(m => m.RequisicaoId == id).ToList();

                if (fornecedorId != null)
                {
                    foreach (var itemRequisicao in listaItensRequisicao)
                    {
                        var compraCotacaoFornecedorDto = new CompraCotacaoFornecedorDto();

                        compraCotacaoFornecedorDto.IdGrid = ++idGrid;

                        var compraCotacaoItem = await _compraCotacaoItemRepository.GetAll().Include(x => x.CompraCotacao)
                            .FirstOrDefaultAsync(m => m.CompraCotacao.RequisicaoId == id && m.RequisicaoItemId == itemRequisicao.Id && m.CompraCotacao.SisFornecedorId == fornecedorId);

                        compraCotacaoFornecedorDto.RequisicaoId = itemRequisicao.RequisicaoId;
                        compraCotacaoFornecedorDto.RequisicaoItemId = itemRequisicao.Id;

                        compraCotacaoFornecedorDto.FornecedorId = (long)fornecedorId;
                        var fornecedor = await _fornecedorAppService.Obter(compraCotacaoFornecedorDto.FornecedorId);
                        compraCotacaoFornecedorDto.Fornecedor = fornecedor;

                        compraCotacaoFornecedorDto.ProdutoId = itemRequisicao.ProdutoId;
                        var produto = await _produtoAppService.Obter(compraCotacaoFornecedorDto.ProdutoId);
                        compraCotacaoFornecedorDto.Produto = produto;

                        compraCotacaoFornecedorDto.Quantidade = compraCotacaoItem != null ? compraCotacaoItem.Quantidade : (decimal)itemRequisicao.QuantidadeAprovacao;

                        compraCotacaoFornecedorDto.UnidadeId = itemRequisicao.UnidadeAprovacaoId;
                        if (compraCotacaoFornecedorDto.UnidadeId != null)
                        {
                            var unidade = await _unidadeAppService.Obter((long)compraCotacaoFornecedorDto.UnidadeId);
                            compraCotacaoFornecedorDto.Unidade = unidade;
                        }

                        compraCotacaoFornecedorDto.ValorUnitario = compraCotacaoItem?.ValorUnitario;
                        compraCotacaoFornecedorDto.OpcaoComprador = compraCotacaoItem?.OpcaoComprador;
                        compraCotacaoFornecedorDto.PrazoEntregaEmDias = compraCotacaoItem?.PrazoEntregaEmDias;
                        compraCotacaoFornecedorDto.PrazoEntregaFornecedorEmDias = compraCotacaoItem?.CompraCotacao.PrazoEntregaEmDias;

                        compraCotacaoFornecedorDto.FormaPagamentoId = compraCotacaoItem?.CompraCotacao.FinFormaPagamentoId;
                        if (compraCotacaoFornecedorDto.FormaPagamentoId != null)
                        {
                            var formaPagamento = await _formaPagamentoAppService.Obter((long)compraCotacaoFornecedorDto.FormaPagamentoId);
                            compraCotacaoFornecedorDto.FormaPagamento = formaPagamento;
                        }

                        compraCotacaoFornecedorDto.LaboratorioId = compraCotacaoItem?.LaboratorioId;
                        if (compraCotacaoFornecedorDto.LaboratorioId != null)
                        {
                            var laboratorio = await _produtoLaboratorioAppService.Obter((long)compraCotacaoFornecedorDto.LaboratorioId);
                            compraCotacaoFornecedorDto.Laboratorio = laboratorio;
                        }

                        compraCotacaoFornecedorListDto.Add(compraCotacaoFornecedorDto);
                    }
                }
                else
                {
                    var listaItensCompraCotacacao = await _compraCotacaoItemRepository.GetAll()
                        .Include(x => x.RequisicaoItem)
                        .Include(x => x.CompraCotacao)
                        .Where(m => m.CompraCotacao.RequisicaoId == id).ToListAsync();

                    foreach (var itemCompraCotacao in listaItensCompraCotacacao)
                    {
                        var compraCotacaoFornecedorDto = new CompraCotacaoFornecedorDto();

                        compraCotacaoFornecedorDto.IdGrid = ++idGrid;

                        compraCotacaoFornecedorDto.RequisicaoItemId = itemCompraCotacao.RequisicaoItem.Id;
                        compraCotacaoFornecedorDto.RequisicaoId = itemCompraCotacao.RequisicaoItem.RequisicaoId;

                        compraCotacaoFornecedorDto.FornecedorId = (long)itemCompraCotacao.CompraCotacao.SisFornecedorId;
                        var fornecedor = await _fornecedorAppService.Obter(compraCotacaoFornecedorDto.FornecedorId);
                        compraCotacaoFornecedorDto.Fornecedor = fornecedor;

                        compraCotacaoFornecedorDto.ProdutoId = itemCompraCotacao.RequisicaoItem.ProdutoId;
                        var produto = await _produtoAppService.Obter(compraCotacaoFornecedorDto.ProdutoId);
                        compraCotacaoFornecedorDto.Produto = produto;

                        compraCotacaoFornecedorDto.Quantidade = itemCompraCotacao.Quantidade;

                        compraCotacaoFornecedorDto.UnidadeId = itemCompraCotacao.RequisicaoItem.UnidadeAprovacaoId;
                        if (compraCotacaoFornecedorDto.UnidadeId != null)
                        {
                            var unidade = await _unidadeAppService.Obter((long)compraCotacaoFornecedorDto.UnidadeId);
                            compraCotacaoFornecedorDto.Unidade = unidade;
                        }

                        compraCotacaoFornecedorDto.ValorUnitario = itemCompraCotacao?.ValorUnitario;
                        compraCotacaoFornecedorDto.OpcaoComprador = itemCompraCotacao?.OpcaoComprador;
                        compraCotacaoFornecedorDto.PrazoEntregaEmDias = itemCompraCotacao?.PrazoEntregaEmDias;
                        compraCotacaoFornecedorDto.PrazoEntregaFornecedorEmDias = itemCompraCotacao?.CompraCotacao.PrazoEntregaEmDias;

                        compraCotacaoFornecedorDto.FormaPagamentoId = itemCompraCotacao?.CompraCotacao.FinFormaPagamentoId;
                        if (compraCotacaoFornecedorDto.FormaPagamentoId != null)
                        {
                            var formaPagamento = await _formaPagamentoAppService.Obter((long)compraCotacaoFornecedorDto.FormaPagamentoId);
                            compraCotacaoFornecedorDto.FormaPagamento = formaPagamento;
                        }

                        compraCotacaoFornecedorDto.LaboratorioId = itemCompraCotacao?.LaboratorioId;
                        if (compraCotacaoFornecedorDto.LaboratorioId != null)
                        {
                            var laboratorio = await _produtoLaboratorioAppService.Obter((long)itemCompraCotacao.LaboratorioId);
                            compraCotacaoFornecedorDto.Laboratorio = laboratorio;
                        }

                        compraCotacaoFornecedorListDto.Add(compraCotacaoFornecedorDto);
                    }
                }

                return new PagedResultDto<CompraCotacaoFornecedorDto> { Items = compraCotacaoFornecedorListDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        #endregion

        #region → Dropdowns
        /// <summary>
        /// Retorna lista paginada de Motivo de Pedido('Reposicao de Estoque', 'Aumento de Consumo', etc) para select2
        /// </summary>
        /// <param name="dropdownInput"></param>
        /// <returns></returns>
        public async Task<IResultDropdownList<long>> ListarMotivoPedidoDropdown(DropdownInput dropdownInput)
        {
            //long grupoId;

            //long.TryParse(dropdownInput.filtro, out grupoId);

            return await ListarDropdownLambda(dropdownInput
                                                     , _compraMotivoPedidoRepository
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                    , o => o.Descricao
                                                    );
        }

        public async Task<IResultDropdownList<long>> ListarAprovacaoStatusDropdown(DropdownInput dropdownInput)
        {
            //long grupoId;

            //long.TryParse(dropdownInput.filtro, out grupoId);

            return await ListarDropdownLambda(dropdownInput
                                                     , _compraAprovacaoStatusRepository
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                    , o => o.Descricao
                                                    );
        }

        /// <summary>
        /// Retorna lista paginada de TipoRequisicao('Produto', 'Servico') para select2
        /// </summary>
        /// <param name="dropdownInput"></param>
        /// <returns></returns>
        public async Task<IResultDropdownList<long>> ListarTipoRequisicaoDropdown(DropdownInput dropdownInput)
        {
            //long grupoId;
            //long.TryParse(dropdownInput.filtro, out grupoId);

            return await ListarDropdownLambda(dropdownInput
                                                     , _compraTipoRequisicaoRepository
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                    , o => o.Descricao
                                                    );
        }
        #endregion

        #region Relatorios
        public byte[] GerarRelatorioCompraRequisicao(CompraRequisicaoRelatorioDto input)
        {
            return this.CreateJasperReport("Suprimentos/Compras/RequisicaoCompras")
                .SetMethod(Method.POST)
                .AddParameter("id", input.CompraRequisicaoId.ToString())
                .GenerateReport();
        }
        #endregion
    }
}
