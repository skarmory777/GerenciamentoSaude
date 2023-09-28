using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Inputs;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras
{
    public class OrdemCompraAppService : SWMANAGERAppServiceBase, IOrdemCompraAppService
    {
        #region ↓ Atributos

        #region → Services
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUnidadeAppService _unidadeAppService;
        private readonly IProdutoAppService _produtoAppService;
        private readonly IUltimoIdAppService _ultimoIdAppService;
        #endregion Servicos 

        #region → Repositorios
        private readonly IRepository<CmpOrdemCompra, long> _ordemCompraRepository;
        private readonly IRepository<OrdemCompraItem, long> _ordemCompraItemRepository;
        private readonly IRepository<OrdemCompraStatus, long> _ordemCompraStatusRepository;
        #endregion  Repositorios

        #endregion

        #region ↓ Construtores

        public OrdemCompraAppService(
        #region → Services
            IUnitOfWorkManager unitOfWorkManager,
            IUnidadeAppService unidadeAppService,
            IProdutoAppService produtoAppService,
            IUltimoIdAppService ultimoIdAppService,
        #endregion

        #region → Repositorios
            IRepository<CmpOrdemCompra, long> ordemCompraRepository,
            IRepository<OrdemCompraStatus, long> ordemCompraStatusRepository,
            IRepository<OrdemCompraItem, long> ordemCompraItemRepository
        #endregion
        )
        {
            #region → Services
            _unitOfWorkManager = unitOfWorkManager;
            _unidadeAppService = unidadeAppService;
            _ultimoIdAppService = ultimoIdAppService;
            _produtoAppService = produtoAppService;
            #endregion

            #region → Repositorios
            _ordemCompraRepository = ordemCompraRepository;
            _ordemCompraStatusRepository = ordemCompraStatusRepository;
            _ordemCompraItemRepository = ordemCompraItemRepository;
            #endregion
        }

        #endregion

        #region ↓ Metodos

        #region → Basico - Listar
        /// <summary>
        /// Responsável por Listar as Ordens de Compras
        /// </summary>
        /// <param name="input">Dados de entrada</param>
        /// <returns></returns>
        public async Task<PagedResultDto<OrdemCompraIndexDto>> Listar(ListarOrdensCompraInput input)
        {
            var count = 0;

            List<OrdemCompraIndexDto> ordemCompraDtoResult;

            try
            {
                DateTime startDate = ((DateTime)input.StartDate).Date;
                DateTime endDate = ((DateTime)input.EndDate).Date;
                endDate = endDate.AddDays(1);
                endDate = endDate.AddSeconds(-1);

                var query = _ordemCompraRepository
                    .GetAll()
                    .Include(m => m.Empresa)
                    .Include(m => m.UnidadeOrganizacional)
                    .Include(m => m.OrdemCompraStatus)
                    .WhereIf(!string.IsNullOrEmpty(input.EmpresaId), e => e.EmpresaId.ToString() == input.EmpresaId)
                    .WhereIf(!string.IsNullOrEmpty(input.UnidadeOrganizacionalId), e => e.UnidadeOrganizacionalId.ToString() == input.UnidadeOrganizacionalId)
                    .WhereIf(!string.IsNullOrEmpty(input.Codigo), e => e.Codigo == input.Codigo)
                    .WhereIf(!string.IsNullOrEmpty(input.OrdemCompraStatusId), e => e.OrdemCompraStatusId.ToString() == input.OrdemCompraStatusId)
                    .Where(_ => _.DataOrdemCompra >= startDate && _.DataOrdemCompra <= endDate)
                    .Select(m => new OrdemCompraIndexDto
                    {
                        Id = m.Id,
                        Codigo = m.Codigo,
                        Empresa = m.Empresa.RazaoSocial,
                        UnidadeOrganizacional = m.UnidadeOrganizacional.Descricao,
                        DataOrdemCompra = m.DataOrdemCompra,
                        OrdemCompraStatus = m.OrdemCompraStatus.Descricao,
                        DataPrevistaEntrega = m.DataPrevistaEntrega,
                    });
                #endregion

                count = await query
                    .CountAsync();

                input.Sorting = "DataOrdemCompra";

                ordemCompraDtoResult = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<OrdemCompraIndexDto>(
                count,
                ordemCompraDtoResult
                );
        }

        public async Task<IResultDropdownList<long>> ListarOrdemCompraStatusDropdown(DropdownInput dropdownInput)
        {
            return await ListarDropdownLambda(dropdownInput
                                                     , _ordemCompraStatusRepository
                                                     , m => (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                                                    || m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                                                    , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                    , o => o.Descricao
                                                    );
        }

        public async Task<OrdemCompraDto> Obter(long id)
        {
            try
            {
                var result = await _ordemCompraRepository
                    .GetAll()
                    .Include(m => m.UnidadeOrganizacional)
                    .Include(m => m.OrdemCompraStatus)
                    .Include(m => m.Empresa)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var ordemCompra = result
                    .MapTo<OrdemCompraDto>();

                return ordemCompra;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        #endregion

        #region CRUD

        #endregion        
        [UnitOfWork]
        public async Task<OrdemCompraDto> CriarOuEditar(OrdemCompraDto input)
        {
            try
            {
                var ordemCompra = input.MapTo<CmpOrdemCompra>();

                var ordemComprasItem = new List<OrdemCompraItemDto>();

                //Deserializa os itens
                if (!input.OrdemCompraItensJson.IsNullOrWhiteSpace())
                {
                    string requisicoesItensJson = input.OrdemCompraItensJson.ToLower(); //← atJSON 

                    ordemComprasItem = JsonConvert.DeserializeObject<List<OrdemCompraItemDto>>(requisicoesItensJson);
                }

                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    #region 1 - grava requisicao
                    if (input.Id.Equals(0))
                    {
                        ordemCompra.Codigo = _ultimoIdAppService.ObterProximoCodigo("CmpOrdemCompra").Result;
                        ordemCompra.Id = await _ordemCompraRepository.InsertAndGetIdAsync(ordemCompra);
                    }
                    else
                    {
                        await _ordemCompraRepository.UpdateAsync(ordemCompra);
                    }
                    #endregion 

                    #region 2 - grava itens da requisicao
                    foreach (var ordemCompraItem in ordemComprasItem)
                    {
                        ordemCompraItem.OrdemCompraId = ordemCompra.Id;

                        if (ordemCompraItem.IsDeleted)
                        {
                            await _ordemCompraItemRepository.DeleteAsync(ordemCompraItem.Id);
                        }
                        else
                        {
                            if (ordemCompraItem.Id.Equals(0))
                            {
                                await _ordemCompraItemRepository.InsertAndGetIdAsync(ordemCompraItem.MapTo<OrdemCompraItem>());
                            }
                            else
                            {
                                await _ordemCompraItemRepository.UpdateAsync(ordemCompraItem.MapTo<OrdemCompraItem>());
                            }
                        }
                    }

                    #endregion region

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }

                return ordemCompra.MapTo<OrdemCompraDto>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _ordemCompraRepository.DeleteAsync(id);
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        #region → Requisicoes Itens
        public async Task<ListResultDto<OrdemCompraItemDto>> ListarRequisicaoItem(long id)
        {
            try
            {
                var idGrid = 0;
                var query = _ordemCompraItemRepository
                            .GetAll()
                            .Where(m => m.OrdemCompraId == id);

                var list = await query.ToListAsync();

                var listDto = list.MapTo<List<OrdemCompraItemDto>>();

                listDto.ForEach(m => m.IdGrid = ++idGrid);

                return new ListResultDto<OrdemCompraItemDto> { Items = listDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<OrdemCompraItemDto>> ListarItensJson(List<OrdemCompraItemDto> list)
        {
            try
            {
                var count = 0;
                if (list == null)
                {
                    list = new List<OrdemCompraItemDto>();
                }
                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].IdGrid = i + 1;

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
                }

                count = await Task.Run(() => list.Count());

                return new PagedResultDto<OrdemCompraItemDto>(
                      count,
                      list
                      );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar", ex));
            }
        }
        #endregion
    }
}
