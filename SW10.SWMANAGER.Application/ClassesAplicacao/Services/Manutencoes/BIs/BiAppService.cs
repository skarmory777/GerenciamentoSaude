using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Manutencoes.BIs;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.BIs.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.BIs
{
    public class BiAppService : SWMANAGERAppServiceBase, IBiAppService
    {
        private readonly IRepository<BI, long> _biRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public BiAppService(
            IRepository<BI, long> biRepository,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _biRepository = biRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [UnitOfWork]
        public async Task CriarOuEditar(BIDto input)
        {
            try
            {
                var output = input.MapTo<BI>();
                if (input.Id > 0)
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _biRepository.UpdateAsync(output);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        await _biRepository.InsertAsync(output);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("ErroSalvar", ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(BIDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await _biRepository.DeleteAsync(input.Id);
                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException("ErroSalvar", ex);
            }
        }

        public async Task<PagedResultDto<BIDto>> Listar(ListarInput input)
        {
            try
            {
                var count = 0;

                var query = _biRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrWhiteSpace(), m =>
                     m.Codigo.ToUpper().Contains(input.Filtro.ToUpper()) ||
                     m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                count = await query.CountAsync();

                var list = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var dtos = list
                    .MapTo<List<BIDto>>();

                return new PagedResultDto<BIDto> { TotalCount = count, Items = dtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<BIDto>> ListarTodos()
        {
            try
            {
                var query = _biRepository
                    .GetAll();

                var item = await query
                    .AsNoTracking()
                    .ToListAsync();

                var dtos = item
                    .MapTo<List<BIDto>>();

                return new ListResultDto<BIDto> { Items = dtos };

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<BIDto> Obter(long id)
        {
            try
            {
                var query = _biRepository
                    .GetAll()
                    .Include(m => m.Modulo)
                    .Include(m => m.Operacao)
                    .Where(m => m.Id == id);
                var result = await query.FirstOrDefaultAsync();
                var dto = result.MapTo<BIDto>();

                return dto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<BIDto> ObterPorOperacao(long id)
        {
            try
            {
                var query = _biRepository
                    .GetAll()
                    .Include(m => m.Modulo)
                    .Include(m => m.Operacao)
                    .Where(m => m.OperacaoId == id);
                var result = await query.FirstOrDefaultAsync();
                var dto = result.MapTo<BIDto>();

                return dto;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            //return await ListarCodigoDescricaoDropdown(dropdownInput, _prescricaoItemRepositorio);
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {

                var query = from p in _biRepository.GetAll()
                            //.Include(m => m.Divisao)
                            //.Include(m => m.Divisao.Subdivisoes)
                            //.Where(m => (m.Divisao.IsDivisaoPrincipal && m.Divisao.Subdivisoes.Count() == 0))
                            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                            )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) };

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

    }
}
