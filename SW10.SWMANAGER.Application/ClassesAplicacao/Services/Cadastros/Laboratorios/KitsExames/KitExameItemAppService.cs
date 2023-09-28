using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames
{
    public class KitExameItemAppService : SWMANAGERAppServiceBase, IKitExameItemAppService
    {
        private readonly IRepository<KitExameItem, long> _kitExameItemRepository;
        private readonly IAtendimentoAppService _atendimentoAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUltimoIdAppService _ultimoIdAppService;

        public KitExameItemAppService(
            IRepository<KitExameItem, long> kitExameItemRepository,
            IAtendimentoAppService atendimentoAppService,
            IUnitOfWorkManager unitOfWorkManager,
            IUltimoIdAppService ultimoIdAppService
            )
        {
            _kitExameItemRepository = kitExameItemRepository;
            _atendimentoAppService = atendimentoAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _ultimoIdAppService = ultimoIdAppService;
        }

        [UnitOfWork]
        public async Task<KitExameItemDto> CriarOuEditar(KitExameItemDto input)
        {
            try
            {
                var kitExameItem = input.MapTo<KitExameItem>();
                if (input.Id.Equals(0))
                {
                    var ultimosId = await _ultimoIdAppService.ListarTodos();
                    var ultimoId = ultimosId.Items.Where(m => m.NomeTabela == "KitExameItem").FirstOrDefault();
                    kitExameItem.Codigo = ultimoId.Codigo;
                    input.Codigo = kitExameItem.Codigo;
                    var codigo = Convert.ToInt64(ultimoId.Codigo);
                    codigo++;
                    ultimoId.Codigo = codigo.ToString();
                    await _ultimoIdAppService.CriarOuEditar(ultimoId);
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var _KitExameItemId = await _kitExameItemRepository.InsertAndGetIdAsync(kitExameItem);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                        input.Id = _KitExameItemId;
                    }
                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var _KitExameItem = await _kitExameItemRepository.UpdateAsync(kitExameItem);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                        input = _KitExameItem.MapTo<KitExameItemDto>();
                    }
                }
                return input;
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
                    await _kitExameItemRepository.DeleteAsync(id);
                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<KitExameItemDto>> ListarTodos()
        {
            try
            {
                var query = _kitExameItemRepository
                    .GetAll();

                var admissoesMedicasDto = await query.ToListAsync();

                return new ListResultDto<KitExameItemDto> { Items = admissoesMedicasDto.MapTo<List<KitExameItemDto>>() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<KitExameItemDto>> Listar(ListarInput input)
        {
            var contarAdmissoesMedicas = 0;
            List<KitExameItemDto> admissoesMedicasDto = new List<KitExameItemDto>();
            try
            {
                var query = _kitExameItemRepository
                    .GetAll()
                    .Where(m => m.CreationTime >= input.StartDate && m.CreationTime <= input.EndDate)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        //m.Descricao.Contains(input.Filtro) ||
                        m.Codigo.Contains(input.Filtro)
                    )
                    .AsQueryable()
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input);

                contarAdmissoesMedicas = await query.CountAsync();
                var admissoesMedicas = await query.ToListAsync();
                admissoesMedicasDto = admissoesMedicas.MapTo<List<KitExameItemDto>>();
                return new PagedResultDto<KitExameItemDto>(
                    contarAdmissoesMedicas,
                    admissoesMedicasDto
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<KitExameItemDto> Obter(long id)
        {
            try
            {
                var query = await _kitExameItemRepository
                    .GetAll()
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var result = query.MapTo<KitExameItemDto>();
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<KitExameItemDto>> ListarFiltro(string filtro)
        {
            try
            {
                var query = _kitExameItemRepository
                    .GetAll()
                    .WhereIf(!filtro.IsNullOrWhiteSpace(), m =>
                         m.CreationTime.ToShortTimeString().Contains(filtro) ||
                         //m.Descricao.Contains(filtro) ||
                         m.Codigo.Contains(filtro)
                    );

                var admissoesMedicas = await query.ToListAsync();
                var admissoesMedicasDto = admissoesMedicas.MapTo<List<KitExameItemDto>>();

                return new ListResultDto<KitExameItemDto> { Items = admissoesMedicasDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropDown(DropdownInput input)
        {
            int pageInt = int.Parse(input.page) - 1;
            var numberOfObjectsPerPage = int.Parse(input.totalPorPagina);
            List<KitExameItemDto> kitExameItemDtos = new List<KitExameItemDto>();
            try
            {
                var query = from p in _kitExameItemRepository.GetAll()
                            .Include(m => m.FaturamentoItem)
                        .WhereIf(!input.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(input.search.ToLower()) //||
                        //m.Descricao.ToLower().Contains(input.search.ToLower())
                        )
                            orderby p.Codigo ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.FaturamentoItem.Descricao) };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<IndexKitExameDto>> ListarPorKit(ListarInput input)
        {
            try
            {

                using (var _kitExameItemRepository2 = IocManager.Instance.ResolveAsDisposable<IRepository<KitExameItem, long>>())
                {

                    long id = Convert.ToInt64(input.Filtro);
                    var query = _kitExameItemRepository2.Object
                        .GetAll()
                        .Include(m => m.FaturamentoItem)
                        .Where(m => m.KitExameId == id)
                       ;


                    var count = await query.CountAsync();
                    var result =   query.AsQueryable()
                                .AsNoTracking()
                                .OrderBy(input.Sorting)
                                .PageBy(input)
                                .Select(s=> new IndexKitExameDto { KitExameId = s.KitExameId
                                                                 , FaturamentoItemId= s.FaturamentoItemId
                                                                 , ExameDescricao=s.FaturamentoItem.Descricao
                                                                 , MaterialDescricao = s.FaturamentoItem.Material!=null? s.FaturamentoItem.Material.Descricao: string.Empty
                                                                 , MaterialId = s.FaturamentoItem.MaterialId})
                                .ToList();

                    //var result = query.MapTo<List<KitExameItemDto>>();
                    return new PagedResultDto<IndexKitExameDto> { TotalCount = count, Items = result };

                    //IndexKitExameDto
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
