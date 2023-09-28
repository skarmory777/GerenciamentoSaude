using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Compras
{
    /// <summary>
    /// Serviço de Compras
    /// Serviços comuns a Requisicao de Compras, Aprovação de Compras, etc
    /// </summary>
    public class CompraAppService : SWMANAGERAppServiceBase, ICompraAppService
    {

        #region ↓ Atributos

        #region → Services
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        //private readonly IUltimoIdAppService _ultimoIdAppService;
        //private readonly IProdutoAppService _produtoAppService;
        //private readonly IUnidadeAppService _unidadeAppService;
        #endregion Servicos 

        #region → Repositorios
        //private readonly IRepository<CompraRequisicao, long> _compraRequisicaoRepository;
        //private readonly IRepository<CompraRequisicaoItem, long> _compraRequisicaoItemRepository;
        private readonly IRepository<CompraMotivoPedido, long> _compraMotivoPedidoRepository;
        //private readonly IRepository<CompraRequisicaoTipo, long> _compraTipoRequisicaoRepository;
        //private readonly IRepository<Produto, long> _produtoRepository;
        //private readonly IRepository<ProdutoEstoque, long> _ressuprimentoRepository;
        //private readonly IRepository<ProdutoSaldo, long> _produtoSaldoRepository;
        #endregion  Repositorios

        #endregion

        #region ↓ Construtores

        public CompraAppService(
        #region → Services
            IUnitOfWorkManager unitOfWorkManager,
            //IUltimoIdAppService ultimoServicoAppService,
            //IProdutoAppService produtoAppService,
            //IUnidadeAppService unidadeAppService,
        #endregion

        #region → Repositorios
            //IRepository<CompraRequisicao, long> compraRequisicaoRepository,
            //IRepository<CompraRequisicaoItem, long> compraRequisicaoItemRepository,
            IRepository<CompraMotivoPedido, long> compraMotivoPedidoRepository
        //IRepository<CompraRequisicaoTipo, long> compraTipoRequisicaoRepository,
        //IRepository<Produto, long> produtoRepository,
        //IRepository<ProdutoEstoque, long> ressuprimentoRepository,
        //IRepository<ProdutoSaldo, long> produtoSaldoRepository
        #endregion
        )
        {
            #region → Services
            _unitOfWorkManager = unitOfWorkManager;
            //_ultimoIdAppService = ultimoServicoAppService;
            //_produtoAppService = produtoAppService;
            //_unidadeAppService = unidadeAppService;
            #endregion

            #region → Repositorios
            //_compraRequisicaoRepository = compraRequisicaoRepository;
            //_compraRequisicaoItemRepository = compraRequisicaoItemRepository;
            _compraMotivoPedidoRepository = compraMotivoPedidoRepository;
            //_compraTipoRequisicaoRepository = compraTipoRequisicaoRepository;
            //_produtoRepository = produtoRepository;
            //_ressuprimentoRepository = ressuprimentoRepository;
            //_produtoSaldoRepository = produtoSaldoRepository;
            #endregion
        }

        #endregion

        #region ↓ Metodos

        #region → Basico - CRUD

        /// <summary>
        /// Retorna um Dto de MotivoPedido ( Ex.:Aumento de Consumo, Reposição de Estoque, Setor, Paciente)
        /// </summary>
        /// <param name="id">id do MotivoPedido</param>
        /// <returns></returns>
        public async Task<CompraMotivoPedidoDto> ObterMotivoPedido(long id)
        {
            try
            {
                var result = await _compraMotivoPedidoRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var motivoPedido = result
                    .MapTo<CompraMotivoPedidoDto>();

                return motivoPedido;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        #endregion Basico - CRUD

        #region → Basico - Listar

        #region → → Motivo Pedido
        /// <summary>
        /// Retorna lista paginada de Motivos de Pedido
        /// </summary>
        /// <param name="input"></param>
        /// <returns>PagedResultDto<CompraMotivoPedidoDto></returns>
        public async Task<PagedResultDto<CompraMotivoPedidoDto>> ListarMotivosPedidos(ListarInput input)
        {
            var contarComprasRequisicao = 0;

            List<CompraMotivoPedido> compraMotivoPedido;
            List<CompraMotivoPedidoDto> compraMotivoPedidoDtos;

            try
            {

                #region query
                var query = _compraMotivoPedidoRepository
                    .GetAll();
                //.Include(m => m.Empresa)
                //.WhereIf(!string.IsNullOrEmpty(input.EmpresaId), e => e.EmpresaId.ToString() == input.EmpresaId)
                #endregion

                contarComprasRequisicao = await query
                    .CountAsync();

                compraMotivoPedido = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                compraMotivoPedidoDtos = compraMotivoPedido
                    .MapTo<List<CompraMotivoPedidoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<CompraMotivoPedidoDto>(
                contarComprasRequisicao,
                compraMotivoPedidoDtos
                );
        }

        /// <summary>
        /// Retorna Lista de Motivos de Pedido
        /// </summary>
        /// <returns>ListResultDto<CompraMotivoPedidoDto></returns>
        public async Task<ListResultDto<CompraMotivoPedidoDto>> ListarTodosMotivosPedidos()
        {
            List<CompraMotivoPedidoDto> compraMotivoPedidoDtos = new List<CompraMotivoPedidoDto>();
            try
            {
                var motivosPedidos = await _compraMotivoPedidoRepository
                    .GetAll()
                    //.Include(m => m.Cidade)
                    .AsNoTracking()
                    .ToListAsync();

                compraMotivoPedidoDtos = motivosPedidos
                    .MapTo<List<CompraMotivoPedidoDto>>();

                return new ListResultDto<CompraMotivoPedidoDto> { Items = compraMotivoPedidoDtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        #endregion

        #endregion

        #region → Gets
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
        #endregion

        #endregion Metodos

    }
}
